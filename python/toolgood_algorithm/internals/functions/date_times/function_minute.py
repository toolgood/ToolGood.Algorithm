"""Function_MINUTE (C#: Internals/Functions/DateTimes/Function_MINUTE.cs)."""
from __future__ import annotations
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _DateTime1


@register("MINUTE")
class Function_MINUTE(_DateTime1):
    def __init__(self, funcs):
        super().__init__(funcs, "Minute")

    def name(self) -> str:
        return "Minute"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_date_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        return Operand.create(args1.date_value().Minute)
