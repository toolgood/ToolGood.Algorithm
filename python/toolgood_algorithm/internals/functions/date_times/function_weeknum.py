"""Function_WEEKNUM (C#: Internals/Functions/DateTimes/Function_WEEKNUM.cs)."""
from __future__ import annotations
import datetime as _dt
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("WEEKNUM")
class Function_WEEKNUM(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Weeknum", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Weeknum"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_date_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        start = args1.date_value().to_datetime()

        return_type = 1
        if self.func2 is not None:
            args2 = self._get_number_2(engine, tp)
            if args2.is_error_or_none():
                return args2
            return_type = args2.int_value()
            if return_type not in (1, 2, 11, 12, 13, 14, 15, 16, 17, 21):
                return self._parameter_error(2)

        if return_type == 21:
            # ISO 8601: week 1 contains the first Thursday of the year
            iso_dow = start.isoweekday()  # 1 = Monday ... 7 = Sunday
            thursday = start + _dt.timedelta(days=4 - iso_dow)
            week_number = (thursday.timetuple().tm_yday - 1) // 7 + 1
            return Operand.create(week_number)

        jan1 = _dt.datetime(start.year, 1, 1)
        day_of_year = start.timetuple().tm_yday
        day_of_week_jan1 = jan1.isoweekday() % 7

        if return_type == 1 or return_type == 17:
            week_start_day = 0
        elif return_type == 2 or return_type == 11:
            week_start_day = 1
        elif return_type == 12:
            week_start_day = 2
        elif return_type == 13:
            week_start_day = 3
        elif return_type == 14:
            week_start_day = 4
        elif return_type == 15:
            week_start_day = 5
        else:
            week_start_day = 6

        days_until_week_start = (day_of_week_jan1 - week_start_day + 7) % 7
        adjusted_day_of_year = day_of_year + days_until_week_start
        week = -(-adjusted_day_of_year // 7)  # Math.ceil(x / 7.0)
        return Operand.create(week)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.DATE)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
