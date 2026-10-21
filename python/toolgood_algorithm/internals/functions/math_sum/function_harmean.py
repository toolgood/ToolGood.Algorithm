"""Function_HARMEAN (C#: Internals/Functions/MathSum/Function_HARMEAN.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_util import flatten_to_big_decimal_list
from toolgood_algorithm.internals.numeric import ZERO
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float

from ._common import _SumFunctionN


@register("HARMEAN")
class Function_HARMEAN(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "HarMean", 1)

    def name(self) -> str:
        return "HarMean"

    def evaluate(self, engine, tp) -> Operand:
        error, args = self._collect(engine, tp)
        if error is not None:
            return error

        values: list[Decimal] = []
        if not flatten_to_big_decimal_list(args, values):
            return self._function_error()
        if not values:
            return self._function_error()

        total = 0.0
        for value in values:
            if value == ZERO:
                return self._function_error()
            total += 1.0 / float(value)
        if total == 0:
            return self._function_error()
        return Operand.create(from_float(len(values) / total))

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        if len(self.funcs) == 1:
            self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        else:
            for func in self.funcs:
                func._get_parameter_types(none_engine, result, OperandType.NUMBER)
