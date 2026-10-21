"""KeyValue (C#: Operands/KeyValue.cs)."""
from __future__ import annotations

from toolgood_algorithm.operands.operand import Operand


class KeyValue:
    __slots__ = ("key", "value")

    def __init__(self, key: str = "", value: Operand = None):
        self.key = key
        self.value = value if value is not None else Operand.NULL
