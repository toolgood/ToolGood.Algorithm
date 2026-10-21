"""ConditionTreeType (C#: Enums/ConditionTreeType.cs)."""
from __future__ import annotations

from __future__ import annotations
from enum import IntEnum


class ConditionTreeType(IntEnum):
    String = 0
    And = 1
    Or = 2
    Error = 3

    @staticmethod
    def int_to_enum(value: int) -> "ConditionTreeType | None":
        try:
            return ConditionTreeType(value)
        except ValueError:
            return None
