"""Function_Version (C#: Internals/Functions/Value/Function_Version.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_0 import Function_0
from toolgood_algorithm.operands.operand import Operand


class Function_Version(Function_0):
    def name(self) -> str:
        return "ALGORITHMVERSION"

    def evaluate(self, engine, tp) -> Operand:
        return Operand.VERSION

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append(self.name())
