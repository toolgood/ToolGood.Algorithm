"""OperandBoolean (C#: Operands/OperandBoolean.cs)."""
from __future__ import annotations

from decimal import Decimal, InvalidOperation

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.lit_json.json_data import JsonData
from toolgood_algorithm.internals.numeric import ONE, ZERO, plain, plain_strip

from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.operands.operand_string import OperandString


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
