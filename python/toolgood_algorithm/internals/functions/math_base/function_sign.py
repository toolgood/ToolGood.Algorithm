"""Function_SIGN (C#: Internals/Functions/MathBase/Function_SIGN.cs)."""
from __future__ import annotations
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _Math1


@register("SIGN")
class Function_SIGN(_Math1):
    def name(self) -> str:
        return "Sign"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        value = a.number_value()
        return Operand.create(-1 if value < 0 else (1 if value > 0 else 0))
