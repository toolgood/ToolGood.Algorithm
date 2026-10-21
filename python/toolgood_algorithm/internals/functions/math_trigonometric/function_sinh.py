"""Function_SINH (C#: Internals/Functions/MathTrigonometric/Function_SINH.cs)."""
from __future__ import annotations
import math
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float

from ._common import _Trig1


@register("SINH")
class Function_SINH(_Trig1):
    def name(self) -> str:
        return "Sinh"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        x = float(args1.number_value())
        # double 上限约 1.8e308,|x|>=709 时 Math.sinh 返回 Infinity
        if x >= 709 or x <= -709:
            return self._parameter_error(1)
        return Operand.create(from_float(math.sinh(x)))
