"""Function_COS (C#: Internals/Functions/MathTrigonometric/Function_COS.cs)."""
from __future__ import annotations
import math
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float

from ._common import _Trig1


@register("COS")
class Function_COS(_Trig1):
    def name(self) -> str:
        return "Cos"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        return Operand.create(from_float(math.cos(float(args1.number_value()))))
