"""OperandType (C#: Enums/OperandType.cs)."""
from __future__ import annotations

from enum import IntEnum


class OperandType(IntEnum):
    """Operation result types."""

    NONE = 0
    NULL = 1
    ERROR = 2
    DATE = 3
    ARRAY = 4
    NUMBER = 5
    BOOLEAN = 6
    TEXT = 7
    JSON = 8
    ARRAYJSON = 9
