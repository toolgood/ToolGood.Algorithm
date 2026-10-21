"""Function_INTERCEPT (C#: Internals/Functions/MathSum/Function_INTERCEPT.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float

from ._common import _SumFunctionN


@register("INTERCEPT")
class Function_INTERCEPT(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "INTERCEPT", 2, 2)

    def name(self) -> str:
        return "INTERCEPT"

    def evaluate(self, engine, tp) -> Operand:
        y_arg = self._get_array(engine, tp, 0)
        if y_arg.is_error_or_none():
            return y_arg
        x_arg = self._get_array(engine, tp, 1)
        if x_arg.is_error_or_none():
            return x_arg

        y_values: list[Decimal] = []
        for item in y_arg.array_value():
            if item.is_number():
                y_values.append(item.number_value())

        x_values: list[Decimal] = []
        for item in x_arg.array_value():
            if item.is_number():
                x_values.append(item.number_value())

        if len(y_values) != len(x_values) or len(y_values) < 2:
            return self._function_error()

        n = len(y_values)
        sum_x = 0.0
        sum_y = 0.0
        sum_xy = 0.0
        sum_x2 = 0.0
        for i in range(n):
            xv = float(x_values[i])
            yv = float(y_values[i])
            sum_x += xv
            sum_y += yv
            sum_xy += xv * yv
            sum_x2 += xv * xv

        mean_x = sum_x / n
        mean_y = sum_y / n

        denominator = n * sum_x2 - sum_x * sum_x
        if denominator == 0:
            return self._div0_error()
        slope = (n * sum_xy - sum_x * sum_y) / denominator
        return Operand.create(from_float(mean_y - slope * mean_x))

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.funcs[1]._get_parameter_types(none_engine, result, OperandType.ARRAY)
