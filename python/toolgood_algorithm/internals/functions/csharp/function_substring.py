"""Function_SUBSTRING (C#: Internals/Functions/Csharp/Function_SUBSTRING.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_3 import Function_3
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("SUBSTRING")
class Function_SUBSTRING(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "Substring", 2, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "Substring"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        text = args1.text_value()
        start_index = args2.int_value() - engine.ExcelIndex

        if start_index < 0:
            return self._parameter_error(2)
        if start_index >= len(text):
            return Operand.create("")

        if self.func3 is None:
            return Operand.create(text[start_index:])

        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3

        length = args3.int_value()
        if length < 0:
            return self._parameter_error(3)
        if start_index + length > len(text):
            length = len(text) - start_index
        return Operand.create(text[start_index:start_index + length])

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
