"""Function_ISNONTEXT (C#: Internals/Functions/Flow/Function_ISNONTEXT.cs)."""
from __future__ import annotations
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _IsBase


@register("ISNONTEXT")
class Function_ISNONTEXT(_IsBase):
    def name(self) -> str:
        return "ISNONTEXT"

    def evaluate(self, engine, tp) -> Operand:
        return Operand.FALSE if self.func1.evaluate(engine, tp).is_text() else Operand.TRUE
