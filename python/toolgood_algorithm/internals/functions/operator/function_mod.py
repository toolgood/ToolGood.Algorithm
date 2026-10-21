"""Function_Mod (C#: Internals/Functions/Operator/Function_Mod.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.internals.numeric import ZERO
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _Arithmetic


@register("MOD")
class Function_Mod(_Arithmetic):
    OP_TEXT = " % "

    def name(self) -> str:
        return "%"

    def _apply(self, a: Decimal, b: Decimal) -> Operand:
        if b == ZERO:
            return self._div0_error()
        # Excel MOD: the result takes the sign of the divisor
        r = a % b
        if r != ZERO and (r < 0) != (b < 0):
            r += b
        return Operand.create(r)

    def to_string(self, sb: list, add_brackets: bool) -> None:
        if add_brackets:
            sb.append("(")
        self.func1.to_string(sb, True)
        sb.append(" % ")
        self.func2.to_string(sb, True)
        if add_brackets:
            sb.append(")")
