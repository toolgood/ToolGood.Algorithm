"""OperandDecimal (C#: Operands/OperandDecimal.cs)."""
from __future__ import annotations

from decimal import Decimal, InvalidOperation

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.lit_json.json_data import JsonData
from toolgood_algorithm.internals.numeric import ONE, ZERO, plain, plain_strip

from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.operands.operand_my_date import OperandMyDate
from toolgood_algorithm.operands.operand_string import OperandString


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
