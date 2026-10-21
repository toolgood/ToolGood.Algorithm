"""Function_CONCATENATE (C#: Internals/Functions/String/Function_CONCATENATE.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_n import Function_N
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("CONCATENATE")
class Function_CONCATENATE(Function_N):
    def __init__(self, funcs):
        _require_args(funcs, "Concatenate", 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "Concatenate"

    def evaluate(self, engine, tp) -> Operand:
        if not self.funcs:
            return Operand.create("")
        if len(self.funcs) == 1:
            a = self._get_text(engine, tp, 0)
            if a.is_error_or_none():
                return a
            return a
        sb: list[str] = []
        for i in range(len(self.funcs)):
            a = self._get_text(engine, tp, i)
            if a.is_error_or_none():
                return a
            sb.append(a.text_value())
        return Operand.create("".join(sb))

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        for func in self.funcs:
            func._get_parameter_types(none_engine, result, OperandType.TEXT)
