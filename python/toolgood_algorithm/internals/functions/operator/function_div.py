"""Function_Div (C#: Internals/Functions/Operator/Function_Div.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.internals.numeric import DECIMAL_MAX, ONE, ZERO
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _Arithmetic


@register("DIV")
class Function_Div(_Arithmetic):
    OP_TEXT = " / "

    def name(self) -> str:
        return "/"

    def _apply(self, a: Decimal, b: Decimal) -> Operand:
        from toolgood_algorithm.internals.numeric import divide

        if b == ZERO:
            return self._div0_error()
        if b == ONE:
            return Operand.create(a)
        result = divide(a, b)
        if abs(result) > DECIMAL_MAX:
            return self._num_error()
        return Operand.create(result)

    def to_string(self, sb: list, add_brackets: bool) -> None:
        if add_brackets:
            sb.append("(")
        self.func1.to_string(sb, True)
        sb.append(" / ")
        self.func2.to_string(sb, True)
        if add_brackets:
            sb.append(")")
