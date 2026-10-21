"""Function_SQRT (C#: Internals/Functions/MathBase/Function_SQRT.cs)."""
from __future__ import annotations
import math
from toolgood_algorithm.internals.numeric import ZERO
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _Math1, from_float


@register("SQRT")
class Function_SQRT(_Math1):
    def name(self) -> str:
        return "Sqrt"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        if a.number_value() < ZERO:
            return self._parameter_error(1)
        return Operand.create(from_float(math.sqrt(float(a.number_value()))))
