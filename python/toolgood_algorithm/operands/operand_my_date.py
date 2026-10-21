"""OperandMyDate (C#: Operands/OperandMyDate.cs)."""
from __future__ import annotations

from decimal import Decimal, InvalidOperation

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.lit_json.json_data import JsonData
from toolgood_algorithm.internals.numeric import ONE, ZERO, plain, plain_strip

from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.operands.operand_string import OperandString
from toolgood_algorithm.operands.my_date import MyDate


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
