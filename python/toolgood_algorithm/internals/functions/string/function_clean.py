"""Function_CLEAN (C#: Internals/Functions/String/Function_CLEAN.cs)."""
from __future__ import annotations
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _Text1


@register("CLEAN")
class Function_CLEAN(_Text1):
    def name(self) -> str:
        return "Clean"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        text = args1.text_value()
        if not any(ord(c) < 32 for c in text):
            return args1
        return Operand.create("".join(c for c in text if ord(c) >= 32))
