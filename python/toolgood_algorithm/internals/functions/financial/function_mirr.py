"""Function_MIRR (C#: Internals/Functions/Financial/Function_MIRR.cs)."""
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

from ._common import _numbers_from_array


@register("MIRR")
class Function_MIRR(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "MIRR", 3, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "MIRR"

    def evaluate(self, engine, tp) -> Operand:
        values_arg = self._get_array_1(engine, tp)
        if values_arg.is_error_or_none():
            return values_arg
        values, error = _numbers_from_array(values_arg, "MIRR", 1)
        if error is not None:
            return error

        finance_rate_arg = self._get_number_2(engine, tp)
        if finance_rate_arg.is_error_or_none():
            return finance_rate_arg
        finance_rate = finance_rate_arg.number_value()

        reinvest_rate_arg = self._get_number_3(engine, tp)
        if reinvest_rate_arg.is_error_or_none():
            return reinvest_rate_arg
        reinvest_rate = reinvest_rate_arg.number_value()

        n = len(values)
        if n == 0:
            return self._parameter_error(1)
        if n == 1:
            return self._div0_error()

        npv_negative = ZERO
        npv_positive = ZERO
        for i, value in enumerate(values):
            if value < ZERO:
                factor = from_float(math.pow(float(ONE + finance_rate), i))
                npv_negative += divide(value, factor)
            else:
                factor = from_float(math.pow(float(ONE + reinvest_rate), n - 1 - i))
                npv_positive += value * factor

        if npv_negative == ZERO or npv_positive == ZERO:
            return self._div0_error()

        base = divide(npv_positive.copy_negate(), npv_negative)
        exponent = divide(ONE, Decimal(n - 1))
        mirr = from_float(math.pow(float(base), float(exponent))) - ONE
        return Operand.create(mirr)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
