"""Date/time functions (DATE, DATEDIF, NETWORKDAYS, YEARFRAC, ...).

Direct port of the Java ``internals/functions/dateTimes`` package.  Every date
is converted to :class:`datetime.datetime` for the arithmetic (the Java code
relies on Joda-Time ``DateTime``) and wrapped back into :class:`MyDate`.
"""
from __future__ import annotations

import calendar
import datetime as _dt
from decimal import ROUND_HALF_UP, Decimal

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import (
    Function_1,
    Function_2,
    Function_3,
    Function_6,
    Function_N,
    _require_args,
)
from toolgood_algorithm.operands.my_date import MyDate
from toolgood_algorithm.internals.numeric import DECIMAL128, divide
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

_EPOCH = _dt.datetime(1970, 1, 1)
_UTC = _dt.timezone.utc

# ``FunctionUtil.StartDateUtc`` in the original: 1970-01-01T00:00:00Z
_START_DATE_UTC_MS = 0

# Excel serial number upper bounds used by DATEVALUE / TIMESTAMP
_MAX_SERIAL_DAY = 2958465
_MAX_SECONDS = 253402232399


# ---------------------------------------------------------------------------
# helpers
# ---------------------------------------------------------------------------


def _whole_days(start: _dt.datetime, end: _dt.datetime) -> int:
    """``Days.daysBetween(start, end).getDays()`` - truncated towards zero."""
    delta = end - start
    seconds = delta.days * 86400 + delta.seconds
    if seconds >= 0:
        return seconds // 86400
    return -((-seconds) // 86400)


def _start_of_day(dt: _dt.datetime) -> _dt.datetime:
    return dt.replace(hour=0, minute=0, second=0, microsecond=0)


def _month_days(year: int, month: int) -> int:
    return calendar.monthrange(year, month)[1]


def _add_months(dt: _dt.datetime, months: int) -> _dt.datetime:
    """``DateTime.plusMonths`` - the day of month is clamped."""
    index = dt.month - 1 + months
    year = dt.year + index // 12
    month = index % 12 + 1
    day = min(dt.day, _month_days(year, month))
    return dt.replace(year=year, month=month, day=day)


def _add_years(dt: _dt.datetime, years: int) -> _dt.datetime:
    year = dt.year + years
    day = min(dt.day, _month_days(year, dt.month))
    return dt.replace(year=year, day=day)


def _from_ms(milliseconds: int) -> _dt.datetime:
    return _EPOCH + _dt.timedelta(milliseconds=milliseconds)


def _local_wall_clock(dt: _dt.datetime) -> _dt.datetime:
    """UTC wall clock -> wall clock of the local zone (``toDateTime(default)``)."""
    return dt.replace(tzinfo=_UTC).astimezone().replace(tzinfo=None)


def _create_utc(dt: _dt.datetime, engine) -> Operand:
    if engine.UseLocalTime:
        dt = _local_wall_clock(dt)
    return Operand.create(MyDate(dt))


def _ratio(days: Decimal, denominator: int) -> Decimal:
    """10-decimal HALF_UP division used by YEARFRAC."""
    return divide(days, Decimal(denominator), DECIMAL128).quantize(
        Decimal("1e-10"), rounding=ROUND_HALF_UP
    )


class _DateTime2(Function_2):
    """Two parameter shell whose first argument is a date."""

    def __init__(self, funcs, display_name: str):
        if not isinstance(funcs, (list, tuple)):
            funcs = [funcs]
        _require_args(funcs, display_name, 2, 2)
        super().__init__(funcs)

    def get_result_type(self) -> OperandType:
        return OperandType.DATE

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.DATE)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


class _DateTime1(Function_1):
    """One parameter shell whose argument is a date."""

    def __init__(self, funcs, display_name: str):
        if not isinstance(funcs, (list, tuple)):
            funcs = [funcs]
        _require_args(funcs, display_name, 1, 1)
        super().__init__(funcs)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.DATE)


# ---------------------------------------------------------------------------
# add / subtract
# ---------------------------------------------------------------------------


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


@register("ADDHOURS")
class Function_ADDHOURS(_DateTime2):
    def __init__(self, funcs):
        super().__init__(funcs, "AddHours")

    def name(self) -> str:
        return "AddHours"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_date_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        return Operand.create(args1.date_value().add_hours(args2.int_value()))


@register("ADDMINUTES")
class Function_ADDMINUTES(_DateTime2):
    def __init__(self, funcs):
        super().__init__(funcs, "AddMinutes")

    def name(self) -> str:
        return "AddMinutes"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_date_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        return Operand.create(args1.date_value().add_minutes(args2.int_value()))


@register("ADDMONTHS")
class Function_ADDMONTHS(_DateTime2):
    def __init__(self, funcs):
        super().__init__(funcs, "AddMonths")

    def name(self) -> str:
        return "AddMonths"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_date_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        return Operand.create(args1.date_value().add_months(args2.int_value()))


@register("ADDSECONDS")
class Function_ADDSECONDS(_DateTime2):
    def __init__(self, funcs):
        super().__init__(funcs, "AddSeconds")

    def name(self) -> str:
        return "AddSeconds"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_date_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        return Operand.create(args1.date_value().add_seconds(args2.int_value()))


@register("ADDYEARS")
class Function_ADDYEARS(_DateTime2):
    def __init__(self, funcs):
        super().__init__(funcs, "AddYears")

    def name(self) -> str:
        return "AddYears"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_date_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        return Operand.create(args1.date_value().add_years(args2.int_value()))


# ---------------------------------------------------------------------------
# construction / conversion
# ---------------------------------------------------------------------------


@register("DATE")
class Function_DATE(Function_6):
    def __init__(self, funcs):
        _require_args(funcs, "Date", 3, 6)
        super().__init__(funcs)

    def name(self) -> str:
        return "Date"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3

        year = args1.int_value()
        month = args2.int_value()
        day = args3.int_value()

        try:
            base = _add_months(_dt.datetime(year, 1, 1), month - 1)
            base = base + _dt.timedelta(days=day - 1)
        except (ValueError, OverflowError):
            return self._parameter_error(1)

        hour = 0
        minute = 0
        second = 0
        if self.func4 is not None:
            args4 = self._get_number_4(engine, tp)
            if args4.is_error_or_none():
                return args4
            hour = args4.int_value()
            if hour < 0 or hour > 23:
                return self._parameter_error(4)
        if self.func5 is not None:
            args5 = self._get_number_5(engine, tp)
            if args5.is_error_or_none():
                return args5
            minute = args5.int_value()
            if minute < 0 or minute > 59:
                return self._parameter_error(5)
        if self.func6 is not None:
            args6 = self._get_number_6(engine, tp)
            if args6.is_error_or_none():
                return args6
            second = args6.int_value()
            if second < 0 or second > 59:
                return self._parameter_error(6)

        return Operand.create(
            MyDate(base.year, base.month, base.day, hour, minute, second)
        )

    def get_result_type(self) -> OperandType:
        return OperandType.DATE

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func4 is not None:
            self.func4._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func5 is not None:
            self.func5._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func6 is not None:
            self.func6._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("DATEVALUE")
class Function_DATEVALUE(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "DateValue", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "DateValue"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self.func1.evaluate(engine, tp)
        if args1.is_error_or_none():
            return args1
        if args1.is_date():
            return args1

        type_ = 0
        if self.func2 is not None:
            args2 = self._get_number_2(engine, tp)
            if args2.is_error_or_none():
                return args2
            type_ = args2.int_value()

        if type_ == 0:
            if args1.is_text():
                md = MyDate.parse(args1.text_value())
                if md is not None:
                    return Operand.create(md)
            arg1 = self._convert_to_number(args1, 1)
            if arg1.is_error_or_none():
                return arg1
            if arg1.long_value() <= _MAX_SERIAL_DAY:
                return arg1.to_my_date(None)
            if arg1.long_value() <= _MAX_SECONDS:
                return _create_utc(_from_ms(arg1.long_value() * 1000), engine)
            return _create_utc(_from_ms(arg1.long_value()), engine)
        if type_ == 1:
            arg1 = self._convert_to_text(args1, 1)
            if arg1.is_error_or_none():
                return arg1
            md = MyDate.parse(arg1.text_value())
            if md is not None:
                return Operand.create(md)
        elif type_ == 2:
            arg1 = self._convert_to_number(args1, 1)
            if arg1.is_error_or_none():
                return arg1
            return arg1.to_my_date(None)
        elif type_ == 3:
            arg1 = self._convert_to_number(args1, 1)
            if arg1.is_error_or_none():
                return arg1
            return _create_utc(_from_ms(arg1.long_value()), engine)
        elif type_ == 4:
            arg1 = self._convert_to_number(args1, 1)
            if arg1.is_error_or_none():
                return arg1
            return _create_utc(_from_ms(arg1.long_value() * 1000), engine)
        return self._parameter_error(1)

    def get_result_type(self) -> OperandType:
        return OperandType.DATE

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NONE)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("TIME")
class Function_TIME(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "Time", 2, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "Time"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        hour = args1.int_value()
        minute = args2.int_value()
        if hour < 0 or hour > 23:
            return self._parameter_error(1)
        if minute < 0 or minute > 59:
            return self._parameter_error(2)

        second = 0
        if self.func3 is not None:
            args3 = self._get_number_3(engine, tp)
            if args3.is_error_or_none():
                return args3
            second = args3.int_value()
            if second < 0 or second > 59:
                return self._parameter_error(3)
        return Operand.create(MyDate(0, 0, 0, hour, minute, second))

    def get_result_type(self) -> OperandType:
        return OperandType.DATE

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("TIMEVALUE")
class Function_TIMEVALUE(Function_1):
    def __init__(self, funcs):
        if not isinstance(funcs, (list, tuple)):
            funcs = [funcs]
        _require_args(funcs, "TimeValue", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "TimeValue"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        md = MyDate.parse(args1.text_value())
        if md is not None:
            return Operand.create(md)
        return self._parameter_error(1)

    def get_result_type(self) -> OperandType:
        return OperandType.DATE

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)


@register("TIMESTAMP")
class Function_TIMESTAMP(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Timestamp", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Timestamp"

    def evaluate(self, engine, tp) -> Operand:
        type_ = 0
        if self.func2 is not None:
            args2 = self._get_number_2(engine, tp)
            if args2.is_error_or_none():
                return args2
            type_ = args2.int_value()

        args0 = self._get_date_1(engine, tp)
        if args0.is_error_or_none():
            return args0
        value = args0.date_value().to_datetime()
        if engine.UseLocalTime:
            # ``ToDateTime(default).withZone(UTC)``: the wall clock is local
            value = value.astimezone(_UTC).replace(tzinfo=None)
        delta = value - _EPOCH
        ms = (delta.days * 86400 + delta.seconds) * 1000 - _START_DATE_UTC_MS
        if type_ == 0:
            return Operand.create(Decimal(ms))
        if type_ == 1:
            return Operand.create(Decimal(ms) / Decimal(1000))
        return self._parameter_error(2)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.DATE)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


# ---------------------------------------------------------------------------
# component accessors
# ---------------------------------------------------------------------------


@register("YEAR")
class Function_YEAR(_DateTime1):
    def __init__(self, funcs):
        super().__init__(funcs, "Year")

    def name(self) -> str:
        return "Year"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_date_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        if args1.date_value().Year is None:
            return self._parameter_error(1)
        return Operand.create(args1.date_value().Year)


@register("MONTH")
class Function_MONTH(_DateTime1):
    def __init__(self, funcs):
        super().__init__(funcs, "Month")

    def name(self) -> str:
        return "Month"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_date_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        if args1.date_value().Month is None:
            return self._parameter_error(1)
        return Operand.create(args1.date_value().Month)


@register("DAY")
class Function_DAY(_DateTime1):
    def __init__(self, funcs):
        super().__init__(funcs, "Day")

    def name(self) -> str:
        return "Day"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_date_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        if args1.date_value().Day is None:
            return self._parameter_error(1)
        return Operand.create(args1.date_value().Day)


@register("HOUR")
class Function_HOUR(_DateTime1):
    def __init__(self, funcs):
        super().__init__(funcs, "Hour")

    def name(self) -> str:
        return "Hour"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_date_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        return Operand.create(args1.date_value().Hour)


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


@register("SECOND")
class Function_SECOND(_DateTime1):
    def __init__(self, funcs):
        super().__init__(funcs, "Second")

    def name(self) -> str:
        return "Second"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_date_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        return Operand.create(args1.date_value().Second)


# ---------------------------------------------------------------------------
# differences
# ---------------------------------------------------------------------------


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


@register("DAYS")
class Function_DAYS(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Days", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Days"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_date_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_date_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        end = _start_of_day(args1.date_value().to_datetime())
        start = _start_of_day(args2.date_value().to_datetime())
        return Operand.create(_whole_days(start, end))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.DATE)
        self.func2._get_parameter_types(none_engine, result, OperandType.DATE)


@register("DAYS360")
class Function_DAYS360(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "Days360", 2, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "Days360"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_date_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_date_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        start = args1.date_value().to_datetime()
        end = args2.date_value().to_datetime()

        method = False
        if self.func3 is not None:
            args3 = self._get_boolean_3(engine, tp)
            if args3.is_error_or_none():
                return args3
            method = args3.boolean_value()

        days = (
            end.year * 360
            + (end.month - 1) * 30
            - start.year * 360
            - (start.month - 1) * 30
        )
        if method:
            # European method: only 31 becomes 30
            days += min(end.day, 30) - min(start.day, 30)
        else:
            # US (NASD) method
            start_day = start.day
            end_day = end.day
            if start_day == _month_days(start.year, start.month):
                start_day = 30
            if end_day == _month_days(end.year, end.month):
                end_day = 31 if start_day < 30 else 30
            days += end_day - start_day
        return Operand.create(days)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.DATE)
        self.func2._get_parameter_types(none_engine, result, OperandType.DATE)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.BOOLEAN)


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


# ---------------------------------------------------------------------------
# working day helpers
# ---------------------------------------------------------------------------


@register("WEEKDAY")
class Function_WEEKDAY(Function_2):
    _MAPPINGS = {
        12: (6, 7, 1, 2, 3, 4, 5),
        13: (5, 6, 7, 1, 2, 3, 4),
        14: (4, 5, 6, 7, 1, 2, 3),
        15: (3, 4, 5, 6, 7, 1, 2),
        16: (2, 3, 4, 5, 6, 7, 1),
    }

    def __init__(self, funcs):
        _require_args(funcs, "Weekday", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Weekday"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_date_1(engine, tp)
        if args1.is_error_or_none():
            return args1

        type_ = 1
        if self.func2 is not None:
            args2 = self._get_number_2(engine, tp)
            if args2.is_error_or_none():
                return args2
            type_ = args2.int_value()
            if type_ != 1 and type_ != 2 and type_ != 3 and (type_ < 11 or type_ > 17):
                return self._parameter_error(2)

        # 0 = Sunday ... 6 = Saturday (matches Joda ``dayOfWeek % 7``)
        day_of_week = args1.date_value().day_of_week() % 7

        if type_ == 1 or type_ == 17:
            return Operand.create(day_of_week + 1)
        if type_ == 2 or type_ == 11:
            return Operand.create(7 if day_of_week == 0 else day_of_week)
        if type_ == 3:
            return Operand.create(6 if day_of_week == 0 else day_of_week - 1)
        mapping = self._MAPPINGS.get(type_)
        if mapping is not None:
            return Operand.create(mapping[day_of_week])
        return self._parameter_error(2)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.DATE)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


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


@register("WORKDAY")
class Function_WORKDAY(Function_N):
    def __init__(self, funcs):
        _require_args(funcs, "Workday", 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Workday"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_date(engine, tp, 0)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number(engine, tp, 1)
        if args2.is_error_or_none():
            return args2

        start = args1.date_value().to_datetime()
        days = args2.int_value()
        holidays = set()
        for i in range(2, len(self.funcs)):
            ar = self._get_date(engine, tp, i)
            if ar.is_error_or_none():
                return ar
            holidays.add(ar.date_value().to_datetime())

        def is_weekend(dt: _dt.datetime) -> bool:
            return dt.isoweekday() in (6, 7)

        if days > 0:
            # walk day by day until Monday, consuming working days
            while start.isoweekday() != 1 and days > 0:
                start = start + _dt.timedelta(days=1)
                if is_weekend(start):
                    continue
                if start in holidays:
                    continue
                days -= 1
            if days == 0:
                return Operand.create(MyDate(start))

            # coarse jump: from a Monday every 5 working days is 7 days
            after_jump = start + _dt.timedelta(days=(days // 5) * 7)
            extra = 0
            for holiday in holidays:
                if start < holiday <= after_jump and not is_weekend(holiday):
                    extra += 1
            start = after_jump
            days = days % 5 + extra
            while days > 0:
                start = start + _dt.timedelta(days=1)
                if is_weekend(start):
                    continue
                if start in holidays:
                    continue
                days -= 1
        elif days < 0:
            # walk backwards until Friday, consuming working days
            while start.isoweekday() != 5 and days < 0:
                start = start - _dt.timedelta(days=1)
                if is_weekend(start):
                    continue
                if start in holidays:
                    continue
                days += 1
            if days == 0:
                return Operand.create(MyDate(start))

            # coarse jump: from a Friday every 5 working days is 7 days
            after_jump = start + _dt.timedelta(days=(-days // 5) * -7)
            extra = 0
            for holiday in holidays:
                if after_jump <= holiday < start and not is_weekend(holiday):
                    extra += 1
            start = after_jump
            days = -((-days) % 5) - extra
            while days < 0:
                start = start - _dt.timedelta(days=1)
                if is_weekend(start):
                    continue
                if start in holidays:
                    continue
                days += 1
        return Operand.create(MyDate(start))

    def get_result_type(self) -> OperandType:
        return OperandType.DATE

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.funcs[0]._get_parameter_types(none_engine, result, OperandType.DATE)
        self.funcs[1]._get_parameter_types(none_engine, result, OperandType.NUMBER)
        for i in range(2, len(self.funcs)):
            self.funcs[i]._get_parameter_types(none_engine, result, OperandType.DATE)


# ---------------------------------------------------------------------------
# month helpers
# ---------------------------------------------------------------------------


@register("EDATE")
class Function_EDATE(_DateTime2):
    def __init__(self, funcs):
        super().__init__(funcs, "EDate")

    def name(self) -> str:
        return "EDate"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_date_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        return Operand.create(args1.date_value().add_months(args2.int_value()))


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
