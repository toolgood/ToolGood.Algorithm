"""OperandArray (C#: Operands/OperandArray.cs)."""
from __future__ import annotations

from decimal import Decimal, InvalidOperation

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.lit_json.json_data import JsonData
from toolgood_algorithm.internals.numeric import ONE, ZERO, plain, plain_strip

from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.operands.operand_string import OperandString


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
