"""Function_LN (C#: Internals/Functions/MathBase/Function_LN.cs)."""
from __future__ import annotations
import math
from toolgood_algorithm.internals.numeric import ZERO
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _Math1, from_float


@register("LN")
class Function_LN(_Math1):
    def name(self) -> str:
        return "Ln"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        z = a.number_value()
        if z <= ZERO:
            return self._parameter_error(1)
        return Operand.create(from_float(math.log(float(z))))
