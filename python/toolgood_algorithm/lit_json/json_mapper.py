"""``JsonMapper`` - JSON text to :class:`JsonData` (``LitJson/JsonMapper.cs``)."""
from __future__ import annotations

from toolgood_algorithm.lit_json.json_data import JsonData
from toolgood_algorithm.lit_json.json_exception import JsonException
from toolgood_algorithm.lit_json.json_reader import JsonReader


class JsonMapper:
    """Small replacement for LitJson's ``JsonMapper``."""

    @staticmethod
    def to_object(text: str) -> JsonData:
        reader = JsonReader(text)
        value = reader.read_value()
        reader.skip_ws()
        if reader.pos != len(text):
            raise JsonException(f"additional text at position {reader.pos}")
        return JsonData(value)


JsonMapper.ToObject = JsonMapper.to_object
