"""Function_SUMPRODUCT (C#: Internals/Functions/MathSum/Function_SUMPRODUCT.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float

from ._common import _SumFunctionN


@register("SUMPRODUCT")
class Function_SUMPRODUCT(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "SUMPRODUCT", 1)

    def name(self) -> str:
        return "SUMPRODUCT"

    def evaluate(self, engine, tp) -> Operand:
        arrays: list[list[Decimal]] = []
        for i in range(len(self.funcs)):
            arg = self._get_array(engine, tp, i)
            if arg.is_error_or_none():
                return arg
            values: list[Decimal] = []
            for item in arg.array_value():
                if item.is_number():
                    values.append(item.number_value())
            arrays.append(values)

        min_length = len(arrays[0])
        for i in range(1, len(arrays)):
            if len(arrays[i]) < min_length:
                min_length = len(arrays[i])
        if min_length == 0:
            return Operand.ZERO

        result = 0.0
        for i in range(min_length):
            product = 1.0
            for values in arrays:
                product *= float(values[i])
            result += product
        return Operand.create(from_float(result))

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        for func in self.funcs:
            func._get_parameter_types(none_engine, result, OperandType.ARRAY)
