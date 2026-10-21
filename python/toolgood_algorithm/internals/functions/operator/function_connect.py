"""Function_Connect (C#: Internals/Functions/Operator/Function_Connect.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _Arithmetic


@register("CONNECT")
class Function_Connect(_Arithmetic):
    OP_TEXT = " & "
    RESULT = OperandType.TEXT

    def name(self) -> str:
        return "&"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        return Operand.create(args1.text_value() + args2.text_value())
