"""Function_Sub (C#: Internals/Functions/Operator/Function_Sub.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.internals.numeric import DECIMAL_MAX, ZERO
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _Arithmetic


@register("SUB")
class Function_Sub(_Arithmetic):
    OP_TEXT = " - "

    def name(self) -> str:
        return "-"

    def _apply(self, a: Decimal, b: Decimal) -> Operand:
        if b == ZERO:
            return Operand.create(a)
        result = a - b
        if abs(result) > DECIMAL_MAX:
            return self._num_error()
        return Operand.create(result)
