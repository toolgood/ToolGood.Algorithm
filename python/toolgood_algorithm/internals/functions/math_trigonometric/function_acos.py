"""Function_ACOS (C#: Internals/Functions/MathTrigonometric/Function_ACOS.cs)."""
from __future__ import annotations
import math
from decimal import Decimal
from toolgood_algorithm.internals.numeric import ONE
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float

from ._common import _Trig1


@register("ACOS")
class Function_ACOS(_Trig1):
    def name(self) -> str:
        return "Acos"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        x = args1.number_value()
        if x < Decimal(-1) or x > ONE:
            return self._parameter_error(1)
        return Operand.create(from_float(math.acos(float(x))))
