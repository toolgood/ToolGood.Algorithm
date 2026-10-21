"""Function_FACT (C#: Internals/Functions/MathBase/Function_FACT.cs)."""
from __future__ import annotations
from toolgood_algorithm.internals.numeric import ONE
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _Math1


@register("FACT")
class Function_FACT(_Math1):
    def name(self) -> str:
        return "Fact"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        number = a.number_value()
        if number > 2147483647 or number < -2147483648:
            return self._parameter_error(1)
        z = int(number)
        if z < 0 or z > 27:
            return self._parameter_error(1)
        result = ONE
        for i in range(1, z + 1):
            result *= i
        return Operand.create(result)
