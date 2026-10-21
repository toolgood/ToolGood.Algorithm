"""Function_MODE (C#: Internals/Functions/MathSum/Function_MODE.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_util import flatten_to_big_decimal_list
from toolgood_algorithm.internals.numeric import ZERO
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _SumFunctionN


@register("MODE")
class Function_MODE(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "Mode", 1)

    def name(self) -> str:
        return "Mode"

    def evaluate(self, engine, tp) -> Operand:
        error, args = self._collect(engine, tp)
        if error is not None:
            return error

        values: list[Decimal] = []
        if not flatten_to_big_decimal_list(args, values):
            return self._function_error()
        if not values:
            return self._function_error()

        counts: dict[Decimal, int] = {}
        for value in values:
            counts[value] = counts.get(value, 0) + 1

        mode_key = ZERO
        max_count = 1
        for key, count in counts.items():
            if count > max_count:
                max_count = count
                mode_key = key
        if max_count == 1:
            return self._function_error()
        return Operand.create(mode_key)

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        if len(self.funcs) == 1:
            self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        else:
            for func in self.funcs:
                func._get_parameter_types(none_engine, result, OperandType.NUMBER)
