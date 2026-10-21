"""Function_TANH (C#: Internals/Functions/MathTrigonometric/Function_TANH.cs)."""
from __future__ import annotations
import math
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float

from ._common import _Trig1


@register("TANH")
class Function_TANH(_Trig1):
    def name(self) -> str:
        return "Tanh"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        x = float(args1.number_value())
        # tanh 在 |x| 较大时趋近 ±1,提前返回避免 Infinity
        if x >= 709:
            return Operand.create(from_float(1.0))
        if x <= -709:
            return Operand.create(from_float(-1.0))
        return Operand.create(from_float(math.tanh(x)))
