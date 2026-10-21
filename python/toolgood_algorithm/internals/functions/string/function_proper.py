"""Function_PROPER (C#: Internals/Functions/String/Function_PROPER.cs)."""
from __future__ import annotations
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _Text1, _is_letter, _lower_char, _upper_char


@register("PROPER")
class Function_PROPER(_Text1):
    def name(self) -> str:
        return "Proper"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        text = args1.text_value()
        if not text:
            return Operand.create(text)

        need_modify = False
        is_first = True
        for c in text:
            if not _is_letter(c):
                is_first = True
            else:
                if is_first:
                    if c.islower():
                        need_modify = True
                else:
                    if c.isupper():
                        need_modify = True
                if need_modify:
                    break
                is_first = False

        if not need_modify:
            return args1

        chars: list[str] = []
        is_first = True
        for c in text:
            if not _is_letter(c):
                chars.append(c)
                is_first = True
            else:
                chars.append(_upper_char(c) if is_first else _lower_char(c))
                is_first = False
        return Operand.create("".join(chars))
