"""Function_COT (C#: Internals/Functions/MathTrigonometric/Function_COT.cs)."""
from __future__ import annotations
import math
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float

from ._common import _Trig1


@register("COT")
class Function_COT(_Trig1):
    def name(self) -> str:
        return "Cot"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        d = math.tan(float(args1.number_value()))
        if d == 0:
            return self._div0_error()
        return Operand.create(from_float(1.0 / d))
