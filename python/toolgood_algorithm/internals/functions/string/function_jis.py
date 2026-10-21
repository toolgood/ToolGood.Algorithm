"""Function_JIS (C#: Internals/Functions/String/Function_JIS.cs)."""
from __future__ import annotations
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _Text1, _to_sbc


@register("JIS")
class Function_JIS(_Text1):
    def name(self) -> str:
        return "Jis"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        return Operand.create(_to_sbc(args1.text_value()))
