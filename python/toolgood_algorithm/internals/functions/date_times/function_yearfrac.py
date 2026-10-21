"""Function_YEARFRAC (C#: Internals/Functions/DateTimes/Function_YEARFRAC.cs)."""
from __future__ import annotations
import calendar
import datetime as _dt
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_3 import Function_3
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _month_days, _ratio, _whole_days


@register("YEARFRAC")
class Function_YEARFRAC(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "YEARFRAC", 2, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "YEARFRAC"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_date_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        start = args1.date_value().to_datetime()

        args2 = self._get_date_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        end = args2.date_value().to_datetime()

        basis = 0
        if self.func3 is not None:
            args3 = self._get_number_3(engine, tp)
            if args3.is_error_or_none():
                return args3
            basis = args3.int_value()
            if basis < 0 or basis > 4:
                return self._parameter_error(3)

        return Operand.create(self._calculate_year_frac(start, end, basis))

    def _calculate_year_frac(self, start: _dt.datetime, end: _dt.datetime, basis: int) -> Decimal:
        if start > end:
            start, end = end, start
        if basis == 0:
            return self._calculate_30_360(start, end)
        if basis == 1:
            return self._calculate_actual_actual(start, end)
        if basis == 2:
            return _ratio(Decimal(_whole_days(start, end)), 360)
        if basis == 3:
            return _ratio(Decimal(_whole_days(start, end)), 365)
        if basis == 4:
            return self._calculate_30_360_e(start, end)
        return self._calculate_30_360(start, end)

    @staticmethod
    def _calculate_30_360(start: _dt.datetime, end: _dt.datetime) -> Decimal:
        d1 = start.day
        d2 = end.day
        d1_feb_last = start.month == 2 and start.day == _month_days(start.year, 2)
        d2_feb_last = end.month == 2 and end.day == _month_days(end.year, 2)

        if d1 == 31 and d2 == 31:
            d1 = 30
            d2 = 30
        elif d1 == 31:
            d1 = 30
        elif d1 == 30 and d2 == 31:
            d2 = 30
        elif d1_feb_last and d2_feb_last:
            d1 = 30
            d2 = 30
        elif d1_feb_last:
            d1 = 30

        days = 360 * (end.year - start.year) + 30 * (end.month - start.month) + (d2 - d1)
        return _ratio(Decimal(days), 360)

    @staticmethod
    def _calculate_30_360_e(start: _dt.datetime, end: _dt.datetime) -> Decimal:
        d1 = min(30, start.day)
        d2 = min(30, end.day)
        days = 360 * (end.year - start.year) + 30 * (end.month - start.month) + (d2 - d1)
        return _ratio(Decimal(days), 360)

    @staticmethod
    def _calculate_actual_actual(start: _dt.datetime, end: _dt.datetime) -> Decimal:
        if start.year == end.year:
            days_in_year = 366 if calendar.isleap(start.year) else 365
            return _ratio(Decimal(_whole_days(start, end)), days_in_year)

        result = Decimal(0)
        days_in_start_year = 366 if calendar.isleap(start.year) else 365
        days_in_end_year = 366 if calendar.isleap(end.year) else 365

        year_end = _dt.datetime(start.year, 12, 31)
        result += _ratio(Decimal(_whole_days(start, year_end)), days_in_start_year)
        year_start = _dt.datetime(end.year, 1, 1)
        result += _ratio(Decimal(_whole_days(year_start, end)), days_in_end_year)
        result += Decimal(end.year - start.year - 1)
        return result

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.DATE)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.DATE)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
