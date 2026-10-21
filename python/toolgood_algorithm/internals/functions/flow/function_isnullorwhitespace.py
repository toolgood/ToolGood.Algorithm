"""Function_ISNULLORWHITESPACE (C#: Internals/Functions/Flow/Function_ISNULLORWHITESPACE.cs)."""
from __future__ import annotations
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _IsBase


@register("ISNULLORWHITESPACE")
class Function_ISNULLORWHITESPACE(_IsBase):
    def name(self) -> str:
        return "ISNULLORWHITESPACE"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self.func1.evaluate(engine, tp)
        if args1.is_null():
            return Operand.TRUE
        text_arg = self._convert_to_text(args1, 1)
        if text_arg.is_error_or_none():
            return text_arg
        return Operand.create(text_arg.text_value().strip() == "")
