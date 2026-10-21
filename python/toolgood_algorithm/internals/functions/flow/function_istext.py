"""Function_ISTEXT (C#: Internals/Functions/Flow/Function_ISTEXT.cs)."""
from __future__ import annotations
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _IsBase


@register("ISTEXT")
class Function_ISTEXT(_IsBase):
    def name(self) -> str:
        return "ISTEXT"

    def evaluate(self, engine, tp) -> Operand:
        return Operand.TRUE if self.func1.evaluate(engine, tp).is_text() else Operand.FALSE
