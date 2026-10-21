"""CalculateTreeType (C#: Enums/CalculateTreeType.cs)."""
from __future__ import annotations

from __future__ import annotations
from enum import IntEnum


class CalculateTreeType(IntEnum):
    String = 0
    Add = 1
    Sub = 2
    Mul = 3
    Div = 4
    Mod = 5
    Connect = 6
    And = 7
    Or = 8
    OpGt = 9
    OpLt = 10
    OpGe = 11
    OpLe = 12
    OpEq = 13
    OpNe = 14
    Error = 15

    @staticmethod
    def int_to_enum(value: int) -> "CalculateTreeType | None":
        try:
            return CalculateTreeType(value)
        except ValueError:
            return None
