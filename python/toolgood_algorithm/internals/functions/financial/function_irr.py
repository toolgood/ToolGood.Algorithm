"""Function_IRR (C#: Internals/Functions/Financial/Function_IRR.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_n import Function_N
from toolgood_algorithm.internals.numeric import ONE, ZERO, divide
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float

from ._common import _EPS_SLOPE, _EPS_STEP, _MAX_ITERATIONS, _numbers_from_array


@register("IRR")
class Function_IRR(Function_N):
    def __init__(self, funcs):
        _require_args(funcs, "IRR", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "IRR"

    def evaluate(self, engine, tp) -> Operand:
        values_arg = self._get_array(engine, tp, 0)
        if values_arg.is_error_or_none():
            return values_arg
        values, error = _numbers_from_array(values_arg, "IRR", 1)
        if error is not None:
            return error

        if not values:
            return self._parameter_error(1)

        has_positive = any(v > ZERO for v in values)
        has_negative = any(v < ZERO for v in values)
        if not has_positive or not has_negative:
            return self._parameter_error(1)

        guess = from_float(0.1)
        if len(self.funcs) > 1:
            guess_arg = self._get_number(engine, tp, 1)
            if guess_arg.is_error_or_none():
                return guess_arg
            guess = guess_arg.number_value()

        try:
            return Operand.create(self._newton_raphson(values, guess))
        except (ArithmeticError, ValueError, OverflowError):
            return self._function_error()

    @staticmethod
    def _newton_raphson(values, guess):
        rate = guess
        for _ in range(_MAX_ITERATIONS):
            npv = ZERO
            dnpv = ZERO
            one_plus_rate = ONE + rate
            factor = ONE
            for i, value in enumerate(values):
                npv += divide(value, factor)
                dnpv -= divide(Decimal(i) * value, factor * one_plus_rate)
                factor = factor * one_plus_rate

            if abs(dnpv) < _EPS_SLOPE:
                break
            new_rate = rate - divide(npv, dnpv)
            if abs(new_rate - rate) < _EPS_STEP:
                return new_rate
            rate = new_rate
        raise ValueError("IRR calculation did not converge")

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        if len(self.funcs) > 1:
            self.funcs[1]._get_parameter_types(none_engine, result, OperandType.NUMBER)
