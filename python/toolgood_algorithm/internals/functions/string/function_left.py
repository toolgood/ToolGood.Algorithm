"""Function_LEFT (C#: Internals/Functions/String/Function_LEFT.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("LEFT")
class Function_LEFT(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Left", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Left"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        text = args1.text_value()
        if not text:
            return Operand.create("")
        if self.func2 is None:
            return Operand.create(text[:1])
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        count = args2.int_value()
        if count < 0:
            return self._parameter_error(2)
        if count >= len(text):
            return args1
        return Operand.create(text[:count])

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
