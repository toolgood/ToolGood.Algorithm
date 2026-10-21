"""date_times: shared helpers and base classes (kept out of the per-class files)."""
from __future__ import annotations
import calendar
import datetime as _dt
from decimal import ROUND_HALF_UP, Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_1 import Function_1
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.operands.my_date import MyDate
from toolgood_algorithm.internals.numeric import DECIMAL128, divide
from toolgood_algorithm.operands.operand import Operand


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

_EPOCH = _dt.datetime(1970, 1, 1)

_UTC = _dt.timezone.utc

_START_DATE_UTC_MS = 0

_MAX_SERIAL_DAY = 2958465

_MAX_SECONDS = 253402232399


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
