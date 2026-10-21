"""Function_CORREL (C#: Internals/Functions/MathSum/Function_CORREL.cs)."""
from __future__ import annotations
import math
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float

from ._common import _SumFunctionN


@register("CORREL")
class Function_CORREL(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "CORREL", 2, 2)

    def name(self) -> str:
        return "CORREL"

    def evaluate(self, engine, tp) -> Operand:
        array1 = self._get_array(engine, tp, 0)
        if array1.is_error_or_none():
            return array1
        array2 = self._get_array(engine, tp, 1)
        if array2.is_error_or_none():
            return array2

        x_values: list[Decimal] = []
        for item in array1.array_value():
            if item.is_number():
                x_values.append(item.number_value())
            else:
                return self._parameter_error(1)

        y_values: list[Decimal] = []
        for item in array2.array_value():
            if item.is_number():
                y_values.append(item.number_value())
            else:
                return self._parameter_error(2)

        if len(x_values) != len(y_values) or len(x_values) < 2:
            return self._function_error()

        n = len(x_values)
        sum_x = 0.0
        sum_y = 0.0
        for i in range(n):
            sum_x += float(x_values[i])
            sum_y += float(y_values[i])

        mean_x = sum_x / n
        mean_y = sum_y / n

        numerator = 0.0
        denom_x = 0.0
        denom_y = 0.0
        for i in range(n):
            dx = float(x_values[i]) - mean_x
            dy = float(y_values[i]) - mean_y
            numerator += dx * dy
            denom_x += dx * dx
            denom_y += dy * dy

        if denom_x == 0 or denom_y == 0:
            return self._div0_error()
        return Operand.create(from_float(numerator / math.sqrt(denom_x * denom_y)))

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        self.funcs[1]._get_parameter_types(none_engine, result, OperandType.ARRAY)
