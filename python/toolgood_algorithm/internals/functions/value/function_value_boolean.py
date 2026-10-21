"""Function_ValueBoolean (C#: Internals/Functions/Value/Function_ValueBoolean.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_0 import Function_0
from toolgood_algorithm.operands.operand import Operand


class Function_ValueBoolean(Function_0):
    def __init__(self, value: bool):
        self._value = value

    def name(self) -> str:
        return "True" if self._value else "False"

    def evaluate(self, engine, tp) -> Operand:
        return Operand.TRUE if self._value else Operand.FALSE

    def get_result_type(self) -> OperandType:
        return OperandType.BOOLEAN

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append(self.name())
