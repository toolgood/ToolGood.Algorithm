"""Function_INT (C#: Internals/Functions/MathBase/Function_INT.cs)."""
from __future__ import annotations
from decimal import ROUND_FLOOR, Decimal
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _Math1


@register("INT")
class Function_INT(_Math1):
    def name(self) -> str:
        return "Int"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        return Operand.create(a.number_value().quantize(Decimal(1), rounding=ROUND_FLOOR))
