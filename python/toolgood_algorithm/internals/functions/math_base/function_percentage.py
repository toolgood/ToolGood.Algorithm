"""Function_Percentage (C#: Internals/Functions/MathBase/Function_Percentage.cs)."""
from __future__ import annotations
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _Math1, from_float


@register("PERCENTAGE")
class Function_Percentage(_Math1):
    def name(self) -> str:
        return "%"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        return Operand.create(from_float(float(a.number_value()) / 100.0))

    def to_string(self, sb: list, add_brackets: bool) -> None:
        if add_brackets:
            sb.append("(")
        self.func1.to_string(sb, False)
        sb.append("%")
        if add_brackets:
            sb.append(")")
