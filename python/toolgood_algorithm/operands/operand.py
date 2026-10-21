"""``Operand`` - the dynamically typed value used by the expression engine.

Ported from ``toolgood.algorithm.Operand`` (the Java port merges all the C#
``Operand*`` subclasses into one file; the same layout is kept here).
"""
from __future__ import annotations

import datetime as _dt
from decimal import Decimal, InvalidOperation
from typing import Iterable, Optional

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.lit_json.json_data import JsonData
from toolgood_algorithm.lit_json.json_mapper import JsonMapper
from toolgood_algorithm.operands.my_date import MyDate
from toolgood_algorithm.internals.numeric import DECIMAL_MAX, ONE, ZERO, plain, plain_strip

_INT_CACHE_OFFSET = 1000
_INT_CACHE_SIZE = 2001


class Operand:
    """Abstract base class of every value manipulated by the engine."""

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
        if isinstance(obj, int):
            if -_INT_CACHE_OFFSET <= obj <= _INT_CACHE_OFFSET:
                return _INT_CACHE[obj + _INT_CACHE_OFFSET]
            return OperandDecimal(Decimal(obj))
        if isinstance(obj, float):
            if obj != obj or obj in (float("inf"), float("-inf")):
                return Operand.error("Number is NaN or Infinity!")
            if obj == int(obj) and -_INT_CACHE_OFFSET <= obj <= _INT_CACHE_OFFSET:
                return _INT_CACHE[int(obj) + _INT_CACHE_OFFSET]
            return OperandDecimal(Decimal(str(obj)))
        if isinstance(obj, Decimal):
            if obj == obj.to_integral_value() and Decimal(-_INT_CACHE_OFFSET) <= obj <= Decimal(_INT_CACHE_OFFSET):
                return _INT_CACHE[int(obj) + _INT_CACHE_OFFSET]
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


class OperandInt(Operand):
    __slots__ = ("_value",)

    def __init__(self, value: int):
        self._value = value

    def is_number(self) -> bool:
        return True

    def type(self) -> OperandType:
        return OperandType.NUMBER

    def int_value(self) -> int:
        return self._value

    def long_value(self) -> int:
        return self._value

    def double_value(self) -> float:
        return float(self._value)

    def number_value(self) -> Decimal:
        return Decimal(self._value)

    def to_number(self, error_message: str = None) -> Operand:
        return self

    def to_boolean(self, error_message: str = None) -> Operand:
        if self._value == 0:
            return Operand.FALSE
        if self._value == 1:
            return Operand.TRUE
        if error_message is None:
            error_message = "Convert number to bool error!"
        return Operand.error(error_message)

    def to_text(self, error_message: str = None) -> Operand:
        return OperandString(str(self._value))

    def to_my_date(self, error_message: str = None) -> Operand:
        return OperandMyDate(MyDate(self._value))

    def __str__(self) -> str:
        return str(self._value)


class OperandDecimal(Operand):
    __slots__ = ("_value",)

    def __init__(self, value: Decimal):
        self._value = value

    def is_number(self) -> bool:
        return True

    def type(self) -> OperandType:
        return OperandType.NUMBER

    def int_value(self) -> int:
        return int(self._value)

    def long_value(self) -> int:
        return int(self._value)

    def double_value(self) -> float:
        return float(self._value)

    def number_value(self) -> Decimal:
        return self._value

    def to_number(self, error_message: str = None) -> Operand:
        return self

    def to_boolean(self, error_message: str = None) -> Operand:
        if self._value == ZERO:
            return Operand.FALSE
        if self._value == ONE:
            return Operand.TRUE
        if error_message is None:
            error_message = "Convert number to bool error!"
        return Operand.error(error_message)

    def to_text(self, error_message: str = None) -> Operand:
        return OperandString(plain_strip(self._value))

    def to_my_date(self, error_message: str = None) -> Operand:
        return OperandMyDate(MyDate(self._value))

    def __str__(self) -> str:
        return plain(self._value)


class OperandArray(Operand):
    __slots__ = ("_value",)

    def __init__(self, value: list[Operand]):
        self._value = value

    def is_array(self) -> bool:
        return True

    def type(self) -> OperandType:
        return OperandType.ARRAY

    def array_value(self) -> list[Operand]:
        return self._value

    def to_number(self, error_message: str = None) -> Operand:
        if error_message is None:
            error_message = "Convert array to number error!"
        return Operand.error(error_message)

    def to_boolean(self, error_message: str = None) -> Operand:
        if error_message is None:
            error_message = "Convert array to bool error!"
        return Operand.error(error_message)

    def to_text(self, error_message: str = None) -> Operand:
        return OperandString(str(self))

    def to_array(self, error_message: str = None) -> Operand:
        return self

    def to_json(self, error_message: str = None) -> Operand:
        try:
            return Operand.create_json(str(self))
        except Exception:  # noqa: BLE001
            if error_message is None:
                error_message = "Convert array to json error!"
            return Operand.error(error_message)

    def __str__(self) -> str:
        return "[" + ",".join(str(v) for v in self._value) + "]"


class OperandBoolean(Operand):
    __slots__ = ("_value",)

    def __init__(self, value: bool):
        self._value = value

    def is_boolean(self) -> bool:
        return True

    def type(self) -> OperandType:
        return OperandType.BOOLEAN

    def boolean_value(self) -> bool:
        return self._value

    def to_number(self, error_message: str = None) -> Operand:
        return Operand.ONE if self._value else Operand.ZERO

    def to_boolean(self, error_message: str = None) -> Operand:
        return self

    def to_text(self, error_message: str = None) -> Operand:
        return OperandString("TRUE" if self._value else "FALSE")

    def to_my_date(self, error_message: str = None) -> Operand:
        if error_message is None:
            error_message = "Convert bool to date error!"
        return Operand.error(error_message)

    def to_array(self, error_message: str = None) -> Operand:
        if error_message is None:
            error_message = "Convert bool to array error!"
        return Operand.error(error_message)

    def to_json(self, error_message: str = None) -> Operand:
        if error_message is None:
            error_message = "Convert bool to json error!"
        return Operand.error(error_message)

    def __str__(self) -> str:
        return "true" if self._value else "false"


class OperandMyDate(Operand):
    __slots__ = ("_value",)

    def __init__(self, value: MyDate):
        self._value = value

    def is_date(self) -> bool:
        return True

    def type(self) -> OperandType:
        return OperandType.DATE

    def date_value(self) -> MyDate:
        return self._value

    def to_number(self, error_message: str = None) -> Operand:
        return Operand.create(self._value.to_number())

    def to_boolean(self, error_message: str = None) -> Operand:
        return Operand.TRUE if self._value.to_number() != ZERO else Operand.FALSE

    def to_text(self, error_message: str = None) -> Operand:
        return OperandString(str(self._value))

    def to_my_date(self, error_message: str = None) -> Operand:
        return self

    def to_array(self, error_message: str = None) -> Operand:
        if error_message is None:
            error_message = "Convert date to array error!"
        return Operand.error(error_message)

    def to_json(self, error_message: str = None) -> Operand:
        if error_message is None:
            error_message = "Convert date to json error!"
        return Operand.error(error_message)

    def __str__(self) -> str:
        return '"' + str(self._value) + '"'


class OperandError(Operand):
    __slots__ = ("_error_msg", "_args", "_formatted")

    def __init__(self, msg: str, args=()):
        self._error_msg = msg
        self._args = args if args else None
        self._formatted = None

    def type(self) -> OperandType:
        return OperandType.ERROR

    def is_error(self) -> bool:
        return True

    def is_error_or_none(self) -> bool:
        return True

    def error_msg(self) -> str:
        if self._formatted is None:
            if not self._args:
                self._formatted = self._error_msg
            else:
                self._formatted = _format_message(self._error_msg, self._args)
        return self._formatted

    def to_number(self, error_message: str = None) -> Operand:
        return self

    def to_boolean(self, error_message: str = None) -> Operand:
        return self

    def to_text(self, error_message: str = None) -> Operand:
        return self

    def to_array(self, error_message: str = None) -> Operand:
        return self

    def to_json(self, error_message: str = None) -> Operand:
        return self

    def to_my_date(self, error_message: str = None) -> Operand:
        return self

    def __str__(self) -> str:
        return "ERROR: " + self.error_msg()


class OperandJson(Operand):
    __slots__ = ("_value",)

    def __init__(self, value: JsonData):
        self._value = value

    def is_json(self) -> bool:
        return True

    def type(self) -> OperandType:
        return OperandType.JSON

    def json_value(self) -> JsonData:
        return self._value

    def to_json(self, error_message: str = None) -> Operand:
        return self

    def to_text(self, error_message: str = None) -> Operand:
        return OperandString(str(self._value))

    def to_array(self, error_message: str = None) -> Operand:
        if self._value.is_array():
            return OperandArray([json_to_operand(v) for v in self._value.inst_array])
        if error_message is None:
            error_message = "Convert json to array error!"
        return Operand.error(error_message)

    def to_boolean(self, error_message: str = None) -> Operand:
        if error_message is None:
            error_message = "Convert json to bool error!"
        return Operand.error(error_message)

    def to_number(self, error_message: str = None) -> Operand:
        if error_message is None:
            error_message = "Convert json to number error!"
        return Operand.error(error_message)

    def to_my_date(self, error_message: str = None) -> Operand:
        if error_message is None:
            error_message = "Convert json to date error!"
        return Operand.error(error_message)

    def __str__(self) -> str:
        return str(self._value)


class OperandNull(Operand):
    __slots__ = ()

    def type(self) -> OperandType:
        return OperandType.NULL

    def is_null(self) -> bool:
        return True

    def __str__(self) -> str:
        return "null"


class OperandNone(Operand):
    __slots__ = ()

    def type(self) -> OperandType:
        return OperandType.NONE

    def is_none(self) -> bool:
        return True

    def is_error_or_none(self) -> bool:
        return True

    def __str__(self) -> str:
        return "NONE"


class OperandString(Operand):
    __slots__ = ("_value",)

    def __init__(self, value: str):
        self._value = value

    def is_text(self) -> bool:
        return True

    def type(self) -> OperandType:
        return OperandType.TEXT

    def text_value(self) -> str:
        return self._value

    def to_number(self, error_message: str = None) -> Operand:
        text = self._value.strip()
        try:
            return Operand.create(int(text))
        except ValueError:
            pass
        try:
            return Operand.create(Decimal(text))
        except (InvalidOperation, ValueError):
            if error_message is None:
                return Operand.error("Convert string to number error!")
            return Operand.error(error_message)

    def to_text(self, error_message: str = None) -> Operand:
        return self

    def to_boolean(self, error_message: str = None) -> Operand:
        text = self._value.strip()
        if text in ("true", "TRUE", "True"):
            return Operand.TRUE
        if text in ("false", "FALSE", "False"):
            return Operand.FALSE
        if text in ("1", "是", "有"):
            return Operand.TRUE
        if text in ("0", "否", "不是", "无", "没有"):
            return Operand.FALSE
        if error_message is None:
            return Operand.error("Convert string to bool error!")
        return Operand.error(error_message)

    def to_my_date(self, error_message: str = None) -> Operand:
        date = MyDate.parse(self._value)
        if date is not None:
            return Operand.create(date)
        if error_message is None:
            return Operand.error("Convert string to date error!")
        return Operand.error(error_message)

    def to_array(self, error_message: str = None) -> Operand:
        if error_message is None:
            error_message = "Convert string to array error!"
        return Operand.error(error_message)

    def to_json(self, error_message: str = None) -> Operand:
        text = self._value.strip()
        if (text.startswith("{") and text.endswith("}")) or (text.startswith("[") and text.endswith("]")):
            try:
                return Operand.create(JsonMapper.to_object(text))
            except Exception:  # noqa: BLE001
                pass
        if error_message is None:
            return Operand.error("Convert string to json error!")
        return Operand.error(error_message)

    def __str__(self) -> str:
        value = self._value
        if value == "":
            return '""'
        needs_escape = any(ch == '"' or ch == "\\" or ord(ch) < 32 for ch in value)
        if not needs_escape:
            return '"' + value + '"'
        out = ['"']
        for ch in value:
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
            elif ord(ch) < 32:
                out.append("\\u%04x" % ord(ch))
            else:
                out.append(ch)
        out.append('"')
        return "".join(out)


class OperandKeyValue(Operand):
    __slots__ = ("_value",)

    def __init__(self, value: "KeyValue"):
        self._value = value

    def is_array_json(self) -> bool:
        return True

    def type(self) -> OperandType:
        return OperandType.ARRAYJSON

    def value(self) -> "KeyValue":
        return self._value


class OperandKeyValueList(Operand):
    __slots__ = ("_list",)

    def __init__(self):
        self._list: list[KeyValue] = []

    def is_array_json(self) -> bool:
        return True

    def type(self) -> OperandType:
        return OperandType.ARRAYJSON

    def array_value(self) -> list[Operand]:
        return [kv.value for kv in self._list]

    def to_text(self, error_message: str = None) -> Operand:
        return OperandString(str(self))

    def to_array(self, error_message: str = None) -> Operand:
        return OperandArray(self.array_value())

    def to_json(self, error_message: str = None) -> Operand:
        try:
            return Operand.create_json(str(self))
        except Exception:  # noqa: BLE001
            if error_message is None:
                error_message = "Convert to json error!"
            return Operand.error(error_message)

    def add_value(self, key_value: "KeyValue") -> None:
        self._list.append(key_value)

    def get_value(self, key: str) -> Optional[Operand]:
        for item in self._list:
            if item.key == key:
                return item.value
        return None

    def try_get_value(self, key: str):
        for item in self._list:
            if item.key == key:
                return item.value
        return None

    def contains_key(self, value: Operand) -> bool:
        text = value.text_value()
        return any(item.key == text for item in self._list)

    def contains_value(self, value: Operand) -> bool:
        text = value.text_value()
        for item in self._list:
            op = item.value
            if op.is_text():
                if op.text_value() == text:
                    return True
            elif op.is_number():
                if plain(op.number_value()) == text:
                    return True
            elif op.is_boolean():
                if str(op.boolean_value()).lower() == text.lower():
                    return True
        return False

    def __str__(self) -> str:
        return "{" + ",".join(OperandString(kv.key).__str__() + ":" + str(kv.value) for kv in self._list) + "}"


class KeyValue:
    __slots__ = ("key", "value")

    def __init__(self, key: str = "", value: Operand = None):
        self.key = key
        self.value = value if value is not None else Operand.NULL


def json_to_operand(data: JsonData) -> Operand:
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


_INT_CACHE = [OperandInt(i - _INT_CACHE_OFFSET) for i in range(_INT_CACHE_SIZE)]

Operand.TRUE = OperandBoolean(True)
Operand.FALSE = OperandBoolean(False)
Operand.NULL = OperandNull()
Operand.NONE = OperandNone()
Operand.ONE = _INT_CACHE[1 + _INT_CACHE_OFFSET]
Operand.ZERO = _INT_CACHE[0 + _INT_CACHE_OFFSET]
Operand.VERSION = OperandString("ToolGood.Algorithm 6.3")

# ------------------------------------------------------------- CamelCase API
# Thin wrappers (not direct assignments) so that subclass overrides are honoured.
def _operand_aliases():
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


_operand_aliases()
