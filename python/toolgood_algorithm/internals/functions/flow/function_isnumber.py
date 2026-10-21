"""Function_ISNUMBER (C#: Internals/Functions/Flow/Function_ISNUMBER.cs)."""
from __future__ import annotations
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _IsBase


@register("ISNUMBER")
class Function_ISNUMBER(_IsBase):
    def name(self) -> str:
        return "ISNUMBER"

    def evaluate(self, engine, tp) -> Operand:
        return Operand.TRUE if self.func1.evaluate(engine, tp).is_number() else Operand.FALSE
