"""Function_NOT (C#: Internals/Functions/Flow/Function_NOT.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_1 import Function_1
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("NOT")
class Function_NOT(Function_1):
    def __init__(self, funcs):
        super().__init__(funcs if isinstance(funcs, (list, tuple)) else [funcs])
        _require_args([self.func1], "NOT", 1, 1)

    def name(self) -> str:
        return "NOT"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_boolean_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        return Operand.FALSE if args1.boolean_value() else Operand.TRUE

    def get_result_type(self) -> OperandType:
        return OperandType.BOOLEAN

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.BOOLEAN)
