"""Function_RAND (C#: Internals/Functions/Value/Function_RAND.cs)."""
from __future__ import annotations
import random
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_0 import Function_0
from toolgood_algorithm.operands.operand import Operand


class Function_RAND(Function_0):
    def name(self) -> str:
        return "Rand"

    def evaluate(self, engine, tp) -> Operand:
        return Operand.create(Decimal(str(random.random())))

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append("Rand()")

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER
