"""Function_DEGREES (C#: Internals/Functions/MathTrigonometric/Function_DEGREES.cs)."""
from __future__ import annotations
import math
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float

from ._common import _Trig1


@register("DEGREES")
class Function_DEGREES(_Trig1):
    def name(self) -> str:
        return "Degrees"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        return Operand.create(from_float(float(args1.number_value()) / math.pi * 180))
