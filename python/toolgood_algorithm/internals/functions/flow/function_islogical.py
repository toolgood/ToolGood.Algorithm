"""Function_ISLOGICAL (C#: Internals/Functions/Flow/Function_ISLOGICAL.cs)."""
from __future__ import annotations
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _IsBase


@register("ISLOGICAL")
class Function_ISLOGICAL(_IsBase):
    def name(self) -> str:
        return "ISLOGICAL"

    def evaluate(self, engine, tp) -> Operand:
        return Operand.TRUE if self.func1.evaluate(engine, tp).is_boolean() else Operand.FALSE
