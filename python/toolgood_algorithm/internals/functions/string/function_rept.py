"""Function_REPT (C#: Internals/Functions/String/Function_REPT.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("REPT")
class Function_REPT(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Rept", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Rept"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        newtext = args1.text_value()
        length = args2.int_value()
        if length < 0:
            return self._parameter_error(2)
        if length == 0:
            return Operand.create("")
        if not newtext:
            return args1
        if length > 32767 // len(newtext):
            return self._parameter_error(2)
        if length == 1:
            return args1
        return Operand.create(newtext * length)

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
