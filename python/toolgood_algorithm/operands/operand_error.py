"""OperandError (C#: Operands/OperandError.cs)."""
from __future__ import annotations

from decimal import Decimal, InvalidOperation

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.lit_json.json_data import JsonData
from toolgood_algorithm.internals.numeric import ONE, ZERO, plain, plain_strip

from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.operands.operand import _format_message


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
