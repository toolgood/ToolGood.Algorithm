"""Function_DATEDIF (C#: Internals/Functions/DateTimes/Function_DATEDIF.cs)."""
from __future__ import annotations
import datetime as _dt
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_3 import Function_3
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _add_years, _month_days, _whole_days


@register("DATEDIF")
class Function_DATEDIF(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "DateDif", 3, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "DateDif"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_date_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_date_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        args3 = self._get_text_3(engine, tp)
        if args3.is_error_or_none():
            return args3

        start = args1.date_value().to_datetime()
        end = args2.date_value().to_datetime()
        t = args3.text_value()

        key = t.upper()
        if key == "Y":
            if start.month < end.month:
                return Operand.create(end.year - start.year)
            if start.month == end.month and start.day <= end.day:
                return Operand.create(end.year - start.year)
            return Operand.create(end.year - start.year - 1)
        if key == "M":
            months = end.year * 12 + end.month - start.year * 12 - start.month
            if start.day <= end.day:
                return Operand.create(months)
            return Operand.create(months - 1)
        if key == "D":
            return Operand.create(_whole_days(start, end))
        if key == "YD":
            # Excel: build a candidate with the start year and the end month/day,
            # add one year when the candidate precedes the start date.
            cand = _dt.datetime(
                start.year, end.month, min(end.day, _month_days(start.year, end.month))
            )
            if cand < start:
                cand = _add_years(cand, 1)
            return Operand.create(_whole_days(start, cand))
        if key == "MD":
            day = end.day - start.day
            if day < 0:
                day += _month_days(start.year, start.month)
            return Operand.create(day)
        if key == "YM":
            months = end.month - start.month
            if end.day < start.day:
                months -= 1
            if months < 0:
                months += 12
            return Operand.create(months)
        return self._parameter_error(3)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.DATE)
        self.func2._get_parameter_types(none_engine, result, OperandType.DATE)
        self.func3._get_parameter_types(none_engine, result, OperandType.TEXT)
