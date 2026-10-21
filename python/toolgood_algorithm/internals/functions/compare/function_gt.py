"""Function_GT (C#: Internals/Functions/Compare/Function_GT.cs)."""
from __future__ import annotations
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _CompareBase


@register("GT")
class Function_GT(_CompareBase):
    OP_TEXT = " > "
    OP_NAME = ">"

    def _compare_numbers(self, a, b) -> bool:
        return a > b

    def _compare_texts(self, a, b) -> bool:
        return a > b

    def _null_result_when_mixed(self) -> Operand:
        return Operand.FALSE
