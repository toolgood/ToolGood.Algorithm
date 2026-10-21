"""Function_COUNT (C#: Internals/Functions/MathSum/Function_COUNT.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_util import flatten_to_list
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _SumFunctionN


@register("COUNT")
class Function_COUNT(_SumFunctionN):
    def __init__(self, funcs):
        super().__init__(funcs, "Count", 1)

    def name(self) -> str:
        return "Count"

    def evaluate(self, engine, tp) -> Operand:
        error, args = self._collect(engine, tp)
        if error is not None:
            return error

        items: list[Operand] = []
        if not flatten_to_list(args, items):
            return self._function_error()
        return Operand.create(len(items))

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        if len(self.funcs) == 1:
            self.funcs[0]._get_parameter_types(none_engine, result, OperandType.ARRAY)
        else:
            for func in self.funcs:
                func._get_parameter_types(none_engine, result, OperandType.NONE)
