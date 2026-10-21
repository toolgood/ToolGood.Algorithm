"""OperandNull (C#: Operands/OperandNull.cs)."""
from __future__ import annotations

from decimal import Decimal, InvalidOperation

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.lit_json.json_data import JsonData
from toolgood_algorithm.internals.numeric import ONE, ZERO, plain, plain_strip

from toolgood_algorithm.operands.operand import Operand


class OperandNull(Operand):
    __slots__ = ()

    def type(self) -> OperandType:
        return OperandType.NULL

    def is_null(self) -> bool:
        return True

    def __str__(self) -> str:
        return "null"
