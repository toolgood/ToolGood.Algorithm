"""Function_UPPER (C#: Internals/Functions/String/Function_UPPER.cs)."""
from __future__ import annotations
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _Text1


@register("UPPER")
class Function_UPPER(_Text1):
    def name(self) -> str:
        return "Upper"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        text = args1.text_value()
        for c in text:
            if c.islower():
                return Operand.create(text.upper())
        return args1
