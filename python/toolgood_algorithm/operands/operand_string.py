"""OperandString (C#: Operands/OperandString.cs)."""
from __future__ import annotations

from decimal import Decimal, InvalidOperation

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.lit_json.json_data import JsonData
from toolgood_algorithm.internals.numeric import ONE, ZERO, plain, plain_strip

from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.lit_json.json_mapper import JsonMapper
from toolgood_algorithm.operands.my_date import MyDate


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
