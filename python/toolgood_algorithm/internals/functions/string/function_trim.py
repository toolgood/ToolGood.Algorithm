"""Function_TRIM (C#: Internals/Functions/String/Function_TRIM.cs)."""
from __future__ import annotations
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _SPACES, _Text1, _java_trim


@register("TRIM")
class Function_TRIM(_Text1):
    def name(self) -> str:
        return "Trim"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        text = _java_trim(args1.text_value())
        text = _SPACES.sub(" ", text)
        if text == args1.text_value():
            return args1
        return Operand.create(text)
