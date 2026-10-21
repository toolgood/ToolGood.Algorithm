"""Function_Number (C#: Internals/Functions/Value/Function_Number.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_0 import Function_0
from toolgood_algorithm.operands.operand import Operand


class Function_Number(Function_0):
    def __init__(self, value: Operand):
        self._value = value

    def name(self) -> str:
        return "Num"

    def evaluate(self, engine, tp) -> Operand:
        return self._value

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append(str(self._value.number_value()))
