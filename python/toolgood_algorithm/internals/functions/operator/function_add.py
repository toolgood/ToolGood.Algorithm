"""Function_Add (C#: Internals/Functions/Operator/Function_Add.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.internals.numeric import DECIMAL_MAX
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _Arithmetic


@register("ADD")
class Function_Add(_Arithmetic):
    NAME = "+"
    OP_TEXT = " + "

    def name(self) -> str:
        return "+"

    def _apply(self, a: Decimal, b: Decimal) -> Operand:
        result = a + b
        if abs(result) > DECIMAL_MAX:
            return self._num_error()
        return Operand.create(result)
