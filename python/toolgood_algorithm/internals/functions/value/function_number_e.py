"""Function_Number_E (C#: Internals/Functions/Value/Function_Number_E.cs)."""
from __future__ import annotations
import math
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_0 import Function_0
from toolgood_algorithm.operands.operand import Operand


class Function_Number_E(Function_0):
    _OPERAND = Operand.create(math.e)

    def name(self) -> str:
        return "E"

    def evaluate(self, engine, tp) -> Operand:
        return self._OPERAND

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append("E()")
