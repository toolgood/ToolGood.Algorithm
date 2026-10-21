"""Function_EOMONTH (C#: Internals/Functions/DateTimes/Function_EOMONTH.cs)."""
from __future__ import annotations
import datetime as _dt
from toolgood_algorithm.operands.my_date import MyDate
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _DateTime2, _add_months


@register("EOMONTH")
class Function_EOMONTH(_DateTime2):
    def __init__(self, funcs):
        super().__init__(funcs, "EOMonth")

    def name(self) -> str:
        return "EOMonth"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_date_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        try:
            dt = _add_months(args1.date_value().to_datetime(), args2.int_value() + 1)
            dt = _dt.datetime(dt.year, dt.month, 1) - _dt.timedelta(days=1)
        except (ValueError, OverflowError):
            return self._parameter_error(1)
        return Operand.create(MyDate(dt))
