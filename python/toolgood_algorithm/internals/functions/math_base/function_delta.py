"""Function_DELTA (C#: Internals/Functions/MathBase/Function_DELTA.cs)."""
from __future__ import annotations
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.internals.numeric import ZERO
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("DELTA")
class Function_DELTA(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "DELTA", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Delta"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        num1 = a.number_value()
        num2 = ZERO
        if self.func2 is not None:
            b = self._get_number_2(engine, tp)
            if b.is_error_or_none():
                return b
            num2 = b.number_value()
        return Operand.create(1 if num1 == num2 else 0)
