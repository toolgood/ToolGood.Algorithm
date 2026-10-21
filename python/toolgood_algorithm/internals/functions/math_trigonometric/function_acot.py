"""Function_ACOT (C#: Internals/Functions/MathTrigonometric/Function_ACOT.cs)."""
from __future__ import annotations
import math
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float

from ._common import _Trig1


@register("ACOT")
class Function_ACOT(_Trig1):
    def name(self) -> str:
        return "Acot"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        dz = float(args1.number_value())
        return Operand.create(from_float(math.pi / 2 - math.atan(dz)))
