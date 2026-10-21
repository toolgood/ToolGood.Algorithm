"""Function_XIRR (C#: Internals/Functions/Financial/Function_XIRR.cs)."""
from __future__ import annotations
import math
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_3 import Function_3
from toolgood_algorithm.internals.numeric import ONE, ZERO, divide
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float

from ._common import _DAYS_IN_YEAR, _EPS_SLOPE, _EPS_STEP, _MAX_ITERATIONS, _dates_from_array, _numbers_from_array, _whole_days


@register("XIRR")
class Function_XIRR(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "XIRR", 2, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "XIRR"

    def evaluate(self, engine, tp) -> Operand:
        values_arg = self._get_array_1(engine, tp)
        if values_arg.is_error_or_none():
            return values_arg
        values, error = _numbers_from_array(values_arg, "XIRR", 1)
        if error is not None:
            return error

        dates_arg = self._get_array_2(engine, tp)
        if dates_arg.is_error_or_none():
            return dates_arg
        dates, error = _dates_from_array(dates_arg, "XIRR", 2)
        if error is not None:
            return Operand.error(error)

        if len(values) != len(dates) or len(values) < 2:
            return self._function_error()

        guess = from_float(0.1)
        if self.func3 is not None:
            guess_arg = self._get_number_3(engine, tp)
            if guess_arg.is_error_or_none():
                return guess_arg
            guess = guess_arg.number_value()

        try:
            return Operand.create(self._newton_raphson(values, dates, guess))
        except (ArithmeticError, ValueError, OverflowError):
            return self._function_error()

    @staticmethod
    def _newton_raphson(values, dates, rate):
        base_date = dates[0]
        exps = [divide(Decimal(_whole_days(base_date, d)), _DAYS_IN_YEAR) for d in dates]

        for _ in range(_MAX_ITERATIONS):
            npv = ZERO
            dnpv = ZERO
            one_plus_rate = ONE + rate
            log_base = math.log(float(one_plus_rate))
            for i, value in enumerate(values):
                factor = from_float(math.exp(float(exps[i]) * log_base))
                npv += divide(value, factor)
                dnpv -= divide(value * exps[i], factor * one_plus_rate)

            if abs(dnpv) < _EPS_SLOPE:
                break
            new_rate = rate - divide(npv, dnpv)
            if abs(new_rate - rate) < _EPS_STEP:
                return new_rate
            rate = new_rate
        raise ValueError("XIRR calculation did not converge")

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.func2._get_parameter_types(none_engine, result, OperandType.ARRAY)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
