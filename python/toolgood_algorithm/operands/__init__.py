"""Operand types (``Operands/`` in the C# tree).

One module per operand type, mirroring ``Operand.cs`` / ``OperandArray.cs`` /
``OperandBoolean.cs`` ... .  The singletons (``Operand.TRUE`` ...) and the
CamelCase aliases are created here, after every subclass has been imported, so
that ``operand.py`` stays free of circular imports.
"""
from __future__ import annotations

from .key_value import KeyValue
from .my_date import MyDate
from .operand import Operand, json_to_operand
from .operand_array import OperandArray
from .operand_boolean import OperandBoolean
from .operand_decimal import OperandDecimal
from .operand_error import OperandError
from .operand_int import OperandInt
from .operand_json import OperandJson
from .operand_key_value import OperandKeyValue
from .operand_key_value_list import OperandKeyValueList
from .operand_my_date import OperandMyDate
from .operand_none import OperandNone
from .operand_null import OperandNull
from .operand_string import OperandString

__all__ = [
    "Operand",
    "OperandArray",
    "OperandBoolean",
    "OperandDecimal",
    "OperandError",
    "OperandInt",
    "OperandJson",
    "OperandKeyValue",
    "OperandKeyValueList",
    "OperandMyDate",
    "OperandNone",
    "OperandNull",
    "OperandString",
    "KeyValue",
    "MyDate",
    "json_to_operand",
]


def _init_singletons() -> None:
    from .operand import _INT_CACHE_OFFSET, _INT_CACHE_SIZE

    Operand._int_cache = [OperandInt(i - _INT_CACHE_OFFSET) for i in range(_INT_CACHE_SIZE)]
    Operand.TRUE = OperandBoolean(True)
    Operand.FALSE = OperandBoolean(False)
    Operand.NULL = OperandNull()
    Operand.NONE = OperandNone()
    Operand.ONE = Operand._int_cache[1 + _INT_CACHE_OFFSET]
    Operand.ZERO = Operand._int_cache[_INT_CACHE_OFFSET]
    Operand.VERSION = OperandString("ToolGood.Algorithm 6.3")


def _install_aliases() -> None:
    """CamelCase wrappers (not plain assignments, so overrides keep working)."""

    def make(name):
        def method(self, *args, **kwargs):
            return getattr(self, name)(*args, **kwargs)

        return method

    for camel, snake in (
        ("IsErrorOrNone", "is_error_or_none"),
        ("IsNone", "is_none"),
        ("IsNull", "is_null"),
        ("IsNumber", "is_number"),
        ("IsText", "is_text"),
        ("IsBoolean", "is_boolean"),
        ("IsArray", "is_array"),
        ("IsDate", "is_date"),
        ("IsJson", "is_json"),
        ("IsArrayJson", "is_array_json"),
        ("IsError", "is_error"),
        ("ErrorMsg", "error_msg"),
        ("Type", "type"),
        ("NumberValue", "number_value"),
        ("DoubleValue", "double_value"),
        ("IntValue", "int_value"),
        ("LongValue", "long_value"),
        ("TextValue", "text_value"),
        ("BooleanValue", "boolean_value"),
        ("ArrayValue", "array_value"),
        ("JsonValue", "json_value"),
        ("DateValue", "date_value"),
        ("ToNumber", "to_number"),
        ("ToBoolean", "to_boolean"),
        ("ToText", "to_text"),
        ("ToMyDate", "to_my_date"),
        ("ToArray", "to_array"),
        ("ToJson", "to_json"),
    ):
        setattr(Operand, camel, make(snake))

    Operand.Create = staticmethod(Operand.create)
    Operand.CreateJson = staticmethod(Operand.create_json)
    Operand.Error = staticmethod(Operand.error)


_init_singletons()
_install_aliases()
