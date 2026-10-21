"""Function_CSCH (C#: Internals/Functions/MathTrigonometric/Function_CSCH.cs)."""
from __future__ import annotations
import math
from decimal import Decimal
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float

from ._common import _Trig1


@register("CSCH")
class Function_CSCH(_Trig1):
    def name(self) -> str:
        return "Csch"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        x = float(args1.number_value())
        # csch 在 |x| 较大时趋近 0,提前返回避免 Infinity
        if x >= 709 or x <= -709:
            return Operand.create(Decimal(0))
        d = math.sinh(x)
        if d == 0:
            return self._div0_error()
        return Operand.create(from_float(1.0 / d))
