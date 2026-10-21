"""JsonType (C#: LitJson/JsonType.cs)."""
from __future__ import annotations

from enum import IntEnum


class JsonType(IntEnum):
    NONE = 0
    OBJECT = 1
    ARRAY = 2
    STRING = 3
    DOUBLE = 4
    BOOLEAN = 5
    NULL = 6
