"""OperandJson (C#: Operands/OperandJson.cs)."""
from __future__ import annotations

from decimal import Decimal, InvalidOperation

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.lit_json.json_data import JsonData
from toolgood_algorithm.internals.numeric import ONE, ZERO, plain, plain_strip

from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.operands.operand_array import OperandArray
from toolgood_algorithm.operands.operand_string import OperandString
from toolgood_algorithm.operands.operand import json_to_operand


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
