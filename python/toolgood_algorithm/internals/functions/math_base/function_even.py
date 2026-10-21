"""Function_EVEN (C#: Internals/Functions/MathBase/Function_EVEN.cs)."""
from __future__ import annotations
from decimal import ROUND_CEILING, ROUND_FLOOR, Decimal
from toolgood_algorithm.internals.numeric import ONE, ZERO
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _Math1


@register("EVEN")
class Function_EVEN(_Math1):
    def name(self) -> str:
        return "Even"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        z = a.number_value()
        if z == ZERO:
            return Operand.ZERO
        two = Decimal(2)
        if z > ZERO:
            if z % two == ZERO:
                return a
            z = z.quantize(Decimal(1), rounding=ROUND_CEILING)
            if z % two == ZERO:
                return Operand.create(z)
            return Operand.create(z + ONE)
        if z % two == ZERO:
            return a
        z = z.quantize(Decimal(1), rounding=ROUND_FLOOR)
        if z % two == ZERO:
            return Operand.create(z)
        return Operand.create(z - ONE)
