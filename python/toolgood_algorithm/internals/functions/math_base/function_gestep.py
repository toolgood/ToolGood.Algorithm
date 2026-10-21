"""Function_GESTEP (C#: Internals/Functions/MathBase/Function_GESTEP.cs)."""
from __future__ import annotations
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.internals.numeric import ZERO
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("GESTEP")
class Function_GESTEP(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "GESTEP", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "GEStep"

    def evaluate(self, engine, tp) -> Operand:
        a = self._get_number_1(engine, tp)
        if a.is_error_or_none():
            return a
        number = a.number_value()
        step = ZERO
        if self.func2 is not None:
            b = self._get_number_2(engine, tp)
            if b.is_error_or_none():
                return b
            step = b.number_value()
        return Operand.create(1 if number >= step else 0)
