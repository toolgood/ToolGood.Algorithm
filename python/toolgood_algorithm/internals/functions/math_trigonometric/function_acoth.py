"""Function_ACOTH (C#: Internals/Functions/MathTrigonometric/Function_ACOTH.cs)."""
from __future__ import annotations
import math
from decimal import Decimal
from toolgood_algorithm.internals.numeric import ONE
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float

from ._common import _Trig1


@register("ACOTH")
class Function_ACOTH(_Trig1):
    def name(self) -> str:
        return "Acoth"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        d = args1.number_value()
        if d >= Decimal(-1) and d <= ONE:
            return self._parameter_error(1)
        dd = float(d)
        return Operand.create(from_float(0.5 * math.log((dd + 1) / (dd - 1))))
