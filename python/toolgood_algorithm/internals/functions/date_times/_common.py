"""date_times: shared helpers (the private base classes live next to this file).

* ``_datetime1.py`` -> :class:`_DateTime1`
* ``_datetime2.py`` -> :class:`_DateTime2`

The classes are re-exported here so the function modules can keep importing
them from ``_common``.
"""
from __future__ import annotations

import calendar
import datetime as _dt
from decimal import ROUND_HALF_UP, Decimal

from toolgood_algorithm.internals.functions.date_times._datetime1 import _DateTime1
from toolgood_algorithm.internals.functions.date_times._datetime2 import _DateTime2
from toolgood_algorithm.internals.numeric import DECIMAL128, divide
from toolgood_algorithm.operands.my_date import MyDate
from toolgood_algorithm.operands.operand import Operand

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


__all__ = [
    "_DateTime1",
    "_DateTime2",
    "_EPOCH",
    "_UTC",
    "_START_DATE_UTC_MS",
    "_MAX_SERIAL_DAY",
    "_MAX_SECONDS",
    "_whole_days",
    "_start_of_day",
    "_month_days",
    "_add_months",
    "_add_years",
    "_from_ms",
    "_local_wall_clock",
    "_create_utc",
    "_ratio",
]
