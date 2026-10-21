"""Function_GUID (C#: Internals/Functions/Value/Function_GUID.cs)."""
from __future__ import annotations
import uuid
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_0 import Function_0
from toolgood_algorithm.operands.operand import Operand


class Function_GUID(Function_0):
    def name(self) -> str:
        return "Guid"

    def evaluate(self, engine, tp) -> Operand:
        return Operand.create(str(uuid.uuid4()))

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append("Guid()")

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT
