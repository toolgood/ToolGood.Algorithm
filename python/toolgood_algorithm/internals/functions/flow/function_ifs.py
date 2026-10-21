"""Function_IFS (C#: Internals/Functions/Flow/Function_IFS.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_n import Function_N
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("IFS")
class Function_IFS(Function_N):
    def __init__(self, funcs):
        _require_args(funcs, "IFS", 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "IFS"

    def evaluate(self, engine, tp) -> Operand:
        for i in range(0, len(self.funcs) - 1, 2):
            condition = self._get_boolean(engine, tp, i)
            if condition.is_error_or_none():
                return condition
            if condition.boolean_value():
                return self.funcs[i + 1].evaluate(engine, tp)
        return self._function_error()

    def get_result_type(self) -> OperandType:
        for i in range(0, len(self.funcs) - 1, 2):
            t = self.funcs[i + 1].get_result_type()
            if t != OperandType.NONE:
                return t
        return OperandType.NONE

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        for i in range(0, len(self.funcs) - 1, 2):
            self.funcs[i]._get_parameter_types(none_engine, result, OperandType.BOOLEAN)
            self.funcs[i + 1]._get_parameter_types(none_engine, result, OperandType.NONE)
