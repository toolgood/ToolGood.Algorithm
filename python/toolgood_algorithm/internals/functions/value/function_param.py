"""Function_PARAM (C#: Internals/Functions/Value/Function_PARAM.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("PARAM")
class Function_PARAM(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Param", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Param"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        if tp is not None:
            result = tp(engine, args1.text_value())
            if result is not None:
                return result
        if self.func2 is not None:
            return self.func2.evaluate(engine, tp)
        return Operand.error(f"Parameter '{args1.text_value()}' is missing.")

    def get_result_type(self) -> OperandType:
        return OperandType.NONE

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NONE)
