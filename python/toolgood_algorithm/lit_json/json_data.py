"""Small JSON wrapper compatible with the LitJson ``JsonData`` API.

Only the surface used by the algorithm engine is implemented: type predicates,
``inst_array`` / ``inst_object`` accessors, ``ToString`` and a parser.
"""
from __future__ import annotations

import json
from decimal import Decimal
from enum import IntEnum


class JsonType(IntEnum):
    NONE = 0
    OBJECT = 1
    ARRAY = 2
    STRING = 3
    DOUBLE = 4
    BOOLEAN = 5
    NULL = 6


class JsonException(ValueError):
    pass


class JsonData:
    __slots__ = ("_type", "_value")

    def __init__(self, value=None, type_: JsonType = None):
        if type_ is not None:
            self._type = type_
            self._value = value
            return
        self._type, self._value = self._detect(value)

    @staticmethod
    def _detect(value):
        if value is None:
            return JsonType.NULL, None
        if isinstance(value, bool):
            return JsonType.BOOLEAN, value
        if isinstance(value, JsonData):
            return value._type, value._value
        if isinstance(value, dict):
            return JsonType.OBJECT, {str(k): JsonData(v) for k, v in value.items()}
        if isinstance(value, (list, tuple)):
            return JsonType.ARRAY, [JsonData(v) for v in value]
        if isinstance(value, str):
            return JsonType.STRING, value
        if isinstance(value, (int, float, Decimal)):
            return JsonType.DOUBLE, Decimal(str(value))
        return JsonType.NULL, None

    # ------------------------------------------------------------- predicates
    def is_none(self) -> bool:
        return self._type == JsonType.NONE

    def is_object(self) -> bool:
        return self._type == JsonType.OBJECT

    def is_array(self) -> bool:
        return self._type == JsonType.ARRAY

    def is_string(self) -> bool:
        return self._type == JsonType.STRING

    def is_double(self) -> bool:
        return self._type == JsonType.DOUBLE

    def is_boolean(self) -> bool:
        return self._type == JsonType.BOOLEAN

    def is_null(self) -> bool:
        return self._type == JsonType.NULL

    @property
    def inst_array(self) -> list["JsonData"]:
        return self._value if self._type == JsonType.ARRAY else []

    @property
    def inst_object(self) -> dict[str, "JsonData"]:
        return self._value if self._type == JsonType.OBJECT else {}

    # ---------------------------------------------------------------- values
    def boolean_value(self) -> bool:
        return bool(self._value)

    def number_value(self) -> Decimal:
        if self._type == JsonType.DOUBLE:
            return self._value
        return Decimal(0)

    def string_value(self) -> str:
        return self._value if self._type == JsonType.STRING else ""

    def count(self) -> int:
        if self._type in (JsonType.ARRAY, JsonType.OBJECT):
            return len(self._value)
        return 0

    def get_child(self, key):
        if isinstance(key, int):
            if self._type == JsonType.ARRAY and 0 <= key < len(self._value):
                return self._value[key]
            return None
        if self._type == JsonType.OBJECT:
            return self._value.get(key)
        return None

    def contains_key(self, key: str) -> bool:
        return self._type == JsonType.OBJECT and key in self._value

    def to_python(self):
        if self._type == JsonType.OBJECT:
            return {k: v.to_python() for k, v in self._value.items()}
        if self._type == JsonType.ARRAY:
            return [v.to_python() for v in self._value]
        return self._value

    # ------------------------------------------------------------ serialisation
    def to_json(self) -> str:
        return _write(self)

    def __str__(self) -> str:
        return _write(self)

    __repr__ = __str__


def _escape(text: str) -> str:
    out = []
    for ch in text:
        if ch == '"':
            out.append('\\"')
        elif ch == "\\":
            out.append("\\\\")
        elif ch == "\n":
            out.append("\\n")
        elif ch == "\r":
            out.append("\\r")
        elif ch == "\t":
            out.append("\\t")
        elif ch == "\b":
            out.append("\\b")
        elif ch == "\f":
            out.append("\\f")
        elif ord(ch) < 0x20:
            out.append("\\u%04x" % ord(ch))
        else:
            out.append(ch)
    return "".join(out)


def _write(data: JsonData) -> str:
    t = data._type
    if t == JsonType.NULL:
        return "null"
    if t == JsonType.BOOLEAN:
        return "true" if data._value else "false"
    if t == JsonType.STRING:
        return '"' + _escape(data._value) + '"'
    if t == JsonType.DOUBLE:
        return format(data._value, "f")
    if t == JsonType.ARRAY:
        return "[" + ",".join(_write(v) for v in data._value) + "]"
    if t == JsonType.OBJECT:
        return "{" + ",".join('"' + _escape(k) + '":' + _write(v) for k, v in data._value.items()) + "}"
    return "null"
