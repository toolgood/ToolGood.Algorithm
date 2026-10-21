"""Function_ValueText (C#: Internals/Functions/Value/Function_ValueText.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_0 import Function_0
from toolgood_algorithm.operands.operand import Operand


class Function_ValueText(Function_0):
    def __init__(self, value: Operand):
        self._value = value

    def name(self) -> str:
        return "Value"

    def evaluate(self, engine, tp) -> Operand:
        return self._value

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append('"')
        text = self._value.text_value()
        for old, new in (
            ("\\", "\\\\"),
            ("\r", "\\r"),
            ("\n", "\\n"),
            ("\t", "\\t"),
            ("\0", "\\0"),
            ("\v", "\\v"),
            ("\a", "\\a"),
            ("\b", "\\b"),
            ("\f", "\\f"),
            ('"', '\\"'),
        ):
            text = text.replace(old, new)
        sb.append(text)
        sb.append('"')
