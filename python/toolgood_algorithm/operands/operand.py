"""``Operand`` - base class and factory for the dynamic values.

C# splits the concrete types into ``Operands/Operand*.cs``; each type lives in
its own module here as well.  The singletons (``TRUE``/``FALSE``/``NULL`` ...)
are created by :mod:`toolgood_algorithm.operands` after all subclasses have
been imported, which keeps this module free of circular imports.
"""
from __future__ import annotations

import datetime as _dt
from decimal import Decimal
from typing import Optional

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.lit_json.json_data import JsonData
from toolgood_algorithm.lit_json.json_mapper import JsonMapper
from toolgood_algorithm.operands.my_date import MyDate

_INT_CACHE_OFFSET = 1000
_INT_CACHE_SIZE = 2001


class Operand:
    """Abstract base class of every value manipulated by the engine."""

    #: small int cache, filled by ``toolgood_algorithm.operands._init_singletons``
    _int_cache: list = []

    # ------------------------------------------------------------------ types
    def is_error_or_none(self) -> bool:
        return False

    def is_none(self) -> bool:
        return False

    def is_null(self) -> bool:
        return False

    def is_number(self) -> bool:
        return False

    def is_text(self) -> bool:
        return False

    def is_boolean(self) -> bool:
        return False

    def is_array(self) -> bool:
        return False

    def is_date(self) -> bool:
        return False

    def is_json(self) -> bool:
        return False

    def is_array_json(self) -> bool:
        return False

    def is_error(self) -> bool:
        return False

    def error_msg(self) -> Optional[str]:
        return None

    def type(self) -> OperandType:
        raise NotImplementedError

    # ----------------------------------------------------------------- values
    def number_value(self) -> Decimal:
        raise TypeError("operand has no number value")

    def double_value(self) -> float:
        raise TypeError("operand has no double value")

    def int_value(self) -> int:
        raise TypeError("operand has no int value")

    def long_value(self) -> int:
        raise TypeError("operand has no long value")

    def text_value(self) -> str:
        raise TypeError("operand has no text value")

    def boolean_value(self) -> bool:
        raise TypeError("operand has no boolean value")

    def array_value(self) -> list["Operand"]:
        raise TypeError("operand has no array value")

    def json_value(self) -> JsonData:
        raise TypeError("operand has no json value")

    def date_value(self) -> MyDate:
        raise TypeError("operand has no date value")

    # ------------------------------------------------------- factory methods
    @staticmethod
    def create(obj) -> "Operand":
        if isinstance(obj, Operand):
            return obj
        if isinstance(obj, bool):
            return Operand.TRUE if obj else Operand.FALSE
        from toolgood_algorithm.operands.operand_array import OperandArray
        from toolgood_algorithm.operands.operand_decimal import OperandDecimal
        from toolgood_algorithm.operands.operand_json import OperandJson
        from toolgood_algorithm.operands.operand_my_date import OperandMyDate
        from toolgood_algorithm.operands.operand_string import OperandString

        cache = Operand._int_cache
        if isinstance(obj, int):
            if -_INT_CACHE_OFFSET <= obj <= _INT_CACHE_OFFSET:
                return cache[obj + _INT_CACHE_OFFSET]
            return OperandDecimal(Decimal(obj))
        if isinstance(obj, float):
            if obj != obj or obj in (float("inf"), float("-inf")):
                return Operand.error("Number is NaN or Infinity!")
            if obj == int(obj) and -_INT_CACHE_OFFSET <= obj <= _INT_CACHE_OFFSET:
                return cache[int(obj) + _INT_CACHE_OFFSET]
            return OperandDecimal(Decimal(str(obj)))
        if isinstance(obj, Decimal):
            if obj == obj.to_integral_value() and Decimal(-_INT_CACHE_OFFSET) <= obj <= Decimal(_INT_CACHE_OFFSET):
                return cache[int(obj) + _INT_CACHE_OFFSET]
            return OperandDecimal(obj)
        if isinstance(obj, str):
            return OperandString(obj)
        if isinstance(obj, MyDate):
            return OperandMyDate(obj)
        if isinstance(obj, (_dt.datetime, _dt.date)):
            return OperandMyDate(MyDate(obj))
        if isinstance(obj, JsonData):
            return OperandJson(obj)
        if isinstance(obj, (list, tuple)):
            return OperandArray(list(obj))
        if obj is None:
            return Operand.NULL
        raise TypeError(f"cannot convert {type(obj)!r} to Operand")

    @staticmethod
    def create_json(txt: str) -> "Operand":
        if txt and ((txt.startswith("{") and txt.endswith("}")) or (txt.startswith("[") and txt.endswith("]"))):
            try:
                return Operand.create(JsonMapper.to_object(txt))
            except Exception:  # noqa: BLE001
                return Operand.error("Convert to json error!")
        return Operand.error("Convert to json error!")

    @staticmethod
    def error(msg: str, *args) -> "Operand":
        from toolgood_algorithm.operands.operand_error import OperandError

        return OperandError(msg, args)

    # ------------------------------------------------------------ conversions
    def to_number(self, error_message: str = None) -> "Operand":
        return Operand.error(error_message if error_message is not None else "Convert to number error!")

    def to_boolean(self, error_message: str = None) -> "Operand":
        return Operand.error(error_message if error_message is not None else "Convert to bool error!")

    def to_text(self, error_message: str = None) -> "Operand":
        return Operand.error(error_message if error_message is not None else "Convert to string error!")

    def to_my_date(self, error_message: str = None) -> "Operand":
        return Operand.error(error_message if error_message is not None else "Convert to date error!")

    def to_array(self, error_message: str = None) -> "Operand":
        return Operand.error(error_message if error_message is not None else "Convert to array error!")

    def to_json(self, error_message: str = None) -> "Operand":
        return Operand.error(error_message if error_message is not None else "Convert to json error!")


def json_to_operand(data: JsonData) -> Operand:
    from toolgood_algorithm.operands.operand_json import OperandJson
    from toolgood_algorithm.operands.operand_string import OperandString

    if data.is_string():
        return OperandString(data.string_value())
    if data.is_boolean():
        return Operand.create(data.boolean_value())
    if data.is_double():
        return Operand.create(data.number_value())
    if data.is_null():
        return Operand.NULL
    return OperandJson(data)


def _format_message(message: str, args) -> str:
    """Best effort ``string.Format`` / ``MessageFormat`` replacement."""
    if not args:
        return message
    try:
        return message.format(*args)
    except (IndexError, KeyError, ValueError):
        out = message
        for i, value in enumerate(args):
            out = out.replace("{" + str(i) + "}", str(value))
        return out
