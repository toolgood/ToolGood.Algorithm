"""OperandKeyValue (C#: Operands/OperandKeyValue.cs)."""
from __future__ import annotations

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.operands.key_value import KeyValue
from toolgood_algorithm.operands.operand import Operand


class OperandKeyValue(Operand):
    __slots__ = ("_value",)

    def __init__(self, value: "KeyValue"):
        self._value = value

    def is_array_json(self) -> bool:
        return True

    def type(self) -> OperandType:
        return OperandType.ARRAYJSON

    def value(self) -> "KeyValue":
        return self._value
