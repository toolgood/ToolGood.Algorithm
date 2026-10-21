"""Function_NETWORKDAYS (C#: Internals/Functions/DateTimes/Function_NETWORKDAYS.cs)."""
from __future__ import annotations
import datetime as _dt
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_n import Function_N
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _start_of_day, _whole_days


@register("NETWORKDAYS")
class Function_NETWORKDAYS(Function_N):
    def __init__(self, funcs):
        _require_args(funcs, "NetworkDays", 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "NetworkDays"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_date(engine, tp, 0)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_date(engine, tp, 1)
        if args2.is_error_or_none():
            return args2

        start = _start_of_day(args1.date_value().to_datetime())
        end = _start_of_day(args2.date_value().to_datetime())

        holidays = set()
        for i in range(2, len(self.funcs)):
            ar = self._get_date(engine, tp, i)
            if ar.is_error_or_none():
                return ar
            holidays.add(_start_of_day(ar.date_value().to_datetime()))

        negative = False
        if start > end:
            start, end = end, start
            negative = True

        total_days = _whole_days(start, end) + 1
        full_weeks = total_days // 7
        remainder = total_days % 7
        days = full_weeks * 5
        for i in range(remainder):
            d = start + _dt.timedelta(days=i)
            if d.isoweekday() not in (6, 7):
                days += 1
        for holiday in holidays:
            if start <= holiday <= end and holiday.isoweekday() not in (6, 7):
                days -= 1
        return Operand.create(-days if negative else days)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        for func in self.funcs:
            func._get_parameter_types(none_engine, result, OperandType.DATE)
