"""Function_ERROR (C#: Internals/Functions/Value/Function_ERROR.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_1 import Function_1
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("ERROR")
class Function_ERROR(Function_1):
    def __init__(self, funcs):
        if isinstance(funcs, (list, tuple)):
            _require_args(funcs, "Error", 1, 1)
            super().__init__(funcs)
        else:
            super().__init__([funcs])

    def name(self) -> str:
        return "Error"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        return Operand.error(args1.text_value())

    def get_result_type(self) -> OperandType:
        return OperandType.ERROR

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        if self.func1 is not None:
            self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
