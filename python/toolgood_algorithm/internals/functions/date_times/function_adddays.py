"""Function_ADDDAYS (C#: Internals/Functions/DateTimes/Function_ADDDAYS.cs)."""
from __future__ import annotations
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _DateTime2


@register("ADDDAYS")
class Function_ADDDAYS(_DateTime2):
    def __init__(self, funcs):
        super().__init__(funcs, "AddDays")

    def name(self) -> str:
        return "AddDays"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_date_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        return Operand.create(args1.date_value().add_days(args2.int_value()))
