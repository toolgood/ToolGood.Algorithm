"""Function_XNPV (C#: Internals/Functions/Financial/Function_XNPV.cs)."""
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

from ._common import _DAYS_IN_YEAR, _arg_array, _dates_from_array, _numbers_from_array, _whole_days


@register("XNPV")
class Function_XNPV(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "XNPV", 3, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "XNPV"

    def evaluate(self, engine, tp) -> Operand:
        rate_arg = self._get_number_1(engine, tp)
        if rate_arg.is_error_or_none():
            return rate_arg
        rate = rate_arg.number_value()
        if rate == ONE.copy_negate():
            return self._div0_error()

        values_arg = self._get_array_2(engine, tp)
        if values_arg.is_error_or_none():
            return values_arg
        values, error = _numbers_from_array(values_arg, "XNPV", 2)
        if error is not None:
            return error

        dates_arg = _arg_array(self, engine, tp, 3)
        if dates_arg.is_error_or_none():
            return dates_arg
        dates, error = _dates_from_array(dates_arg, "XNPV", 3)
        if error is not None:
            return Operand.error(error)

        if len(values) != len(dates):
            return self._function_error()
        if len(values) == 0:
            return self._parameter_error(2)

        base_date = dates[0]
        xnpv = ZERO
        for i, value in enumerate(values):
            exponent = divide(Decimal(_whole_days(base_date, dates[i])), _DAYS_IN_YEAR)
            factor = from_float(math.pow(float(ONE + rate), float(exponent)))
            xnpv += divide(value, factor)
        return Operand.create(xnpv)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.func3._get_parameter_types(none_engine, result, OperandType.ARRAY)
