"""Function_ACOSH (C#: Internals/Functions/MathTrigonometric/Function_ACOSH.cs)."""
from __future__ import annotations
import math
from toolgood_algorithm.internals.numeric import ONE
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float

from ._common import _Trig1


@register("ACOSH")
class Function_ACOSH(_Trig1):
    def name(self) -> str:
        return "Acosh"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        z = args1.number_value()
        if z < ONE:
            return self._parameter_error(1)
        dz = float(z)
        return Operand.create(from_float(math.log(dz + math.sqrt(dz * dz - 1))))
