"""Function_ABS (C#: Internals/Functions/MathBase/Function_ABS.cs)."""
from __future__ import annotations
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _Math1


@register("ABS")
class Function_ABS(_Math1):
    def name(self) -> str:
        return "Abs"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        return Operand.create(abs(a.number_value()))
