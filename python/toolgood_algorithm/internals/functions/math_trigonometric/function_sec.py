"""Function_SEC (C#: Internals/Functions/MathTrigonometric/Function_SEC.cs)."""
from __future__ import annotations
import math
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float

from ._common import _Trig1


@register("SEC")
class Function_SEC(_Trig1):
    def name(self) -> str:
        return "Sec"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        d = math.cos(float(args1.number_value()))
        if d == 0:
            return self._div0_error()
        return Operand.create(from_float(1.0 / d))
