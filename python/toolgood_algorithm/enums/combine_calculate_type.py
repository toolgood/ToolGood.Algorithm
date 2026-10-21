"""CombineCalculateType (C#: Enums/CombineCalculateType.cs)."""
from __future__ import annotations

from enum import IntEnum


class CombineCalculateType(IntEnum):
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

    @staticmethod
    def int_to_enum(value: int) -> "CombineCalculateType | None":
        try:
            return CombineCalculateType(value)
        except ValueError:
            return None
