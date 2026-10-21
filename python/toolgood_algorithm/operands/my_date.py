"""``MyDate`` - a Date/TimeSpan hybrid value used by the engine.

Direct port of ``toolgood.algorithm.MyDate`` (Java) with the formatting helper
kept compatible with the C# ``DateTime`` custom format strings.
"""
from __future__ import annotations

import datetime as _dt
import re
from typing import Optional

from toolgood_algorithm.internals.numeric import DECIMAL32, plain, to_decimal
from decimal import Decimal

_DATE_PATTERNS = [
    # (regex, kind)
    (re.compile(r"^(\d{4})-(1[012]|0?\d)-(30|31|[012]?\d) ([01]?\d|2[0123]):([012345]?\d):([012345]?\d)$"), "datetime"),
    (re.compile(r"^(\d{4})/(1[012]|0?\d)/(30|31|[012]?\d) ([01]?\d|2[0123]):([012345]?\d):([012345]?\d)$"), "datetime"),
    (re.compile(r"^(\d{4})-(1[012]|0?\d)-(30|31|[012]?\d) ([01]?\d|2[0123]):([012345]?\d)$"), "datetime_min"),
    (re.compile(r"^(\d{4})/(1[012]|0?\d)/(30|31|[012]?\d) ([01]?\d|2[0123]):([012345]?\d)$"), "datetime_min"),
    (re.compile(r"^(\d{4})-(1[012]|0?\d)-(30|31|[012]?\d)$"), "date"),
    (re.compile(r"^(\d{4})/(1[012]|0?\d)/(30|31|[012]?\d)$"), "date"),
    (re.compile(r"^(\d+) (2[0123]|[01]?\d):([012345]?\d):([012345]?\d)$"), "day_time"),
    (re.compile(r"^(2[0123]|[01]?\d):([012345]?\d):([012345]?\d)$"), "time"),
    (re.compile(r"^(2[0123]|[01]?\d):([012345]?\d)$"), "hour_minute"),
]

_MONTH_DAYS = (0, 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31)


def _is_leap(year: int) -> bool:
    return year % 4 == 0 and (year % 100 != 0 or year % 400 == 0)


def _days_in_month(year: int, month: int) -> int:
    if month == 2 and not _is_leap(year):
        return 28
    return _MONTH_DAYS[month]


class MyDate:
    __slots__ = ("Year", "Month", "Day", "Hour", "Minute", "Second")

    def __init__(self, year=None, month=None, day=None, hour=0, minute=0, second=0, _skip=False):
        if _skip:
            return
        if isinstance(year, MyDate):
            other = year
            self.Year = other.Year
            self.Month = other.Month
            self.Day = other.Day
            self.Hour = other.Hour
            self.Minute = other.Minute
            self.Second = other.Second
            return
        if isinstance(year, _dt.datetime):
            self.Year = year.year
            self.Month = year.month
            self.Day = year.day
            self.Hour = year.hour
            self.Minute = year.minute
            self.Second = year.second
            return
        if isinstance(year, _dt.date):
            self.Year = year.year
            self.Month = year.month
            self.Day = year.day
            self.Hour = 0
            self.Minute = 0
            self.Second = 0
            return
        if isinstance(year, (int, float, Decimal)) and month is None and day is None:
            self._init_from_number(Decimal(str(year)))
            return
        self.Year = year
        self.Month = month
        self.Day = day
        self.Hour = int(hour or 0)
        self.Minute = int(minute or 0)
        self.Second = int(second or 0)

    # ------------------------------------------------------------------ ctor
    def _init_from_number(self, num: Decimal) -> None:
        days = int(num)
        if days > 365:
            start = _dt.date(1900, 1, 1) + _dt.timedelta(days=days - 2)
            self.Year = start.year
            self.Month = start.month
            self.Day = start.day
        else:
            self.Year = None
            self.Month = None
            self.Day = days
        d = num - Decimal(days)
        hours = d * Decimal(24)
        self.Hour = int(hours)
        minutes = (hours - Decimal(self.Hour)) * Decimal(60)
        self.Minute = int(minutes)
        seconds = (minutes - Decimal(self.Minute)) * Decimal(60)
        self.Second = int(seconds.to_integral_value(rounding="ROUND_HALF_UP"))
        if self.Second == 60:
            self.Second = 0
            self.Minute += 1
            if self.Minute == 60:
                self.Minute = 0
                self.Hour += 1

    # ----------------------------------------------------------------- parse
    @staticmethod
    def parse(txt: str) -> Optional["MyDate"]:
        if txt is None:
            return None
        t = txt.strip()
        for regex, kind in _DATE_PATTERNS:
            m = regex.match(t)
            if not m:
                continue
            g = m.groups()
            date = MyDate(_skip=True)
            date.Hour = 0
            date.Minute = 0
            date.Second = 0
            if kind in ("datetime", "datetime_min"):
                date.Year = int(g[0])
                date.Month = int(g[1])
                date.Day = int(g[2])
                date.Hour = int(g[3])
                date.Minute = int(g[4])
                date.Second = int(g[5]) if kind == "datetime" else 0
            elif kind == "date":
                date.Year = int(g[0])
                date.Month = int(g[1])
                date.Day = int(g[2])
            elif kind == "day_time":
                date.Year = None
                date.Month = None
                date.Day = int(g[0])
                date.Hour = int(g[1])
                date.Minute = int(g[2])
                date.Second = int(g[3])
            elif kind == "time":
                date.Year = None
                date.Month = None
                date.Day = None
                date.Hour = int(g[0])
                date.Minute = int(g[1])
                date.Second = int(g[2])
            elif kind == "hour_minute":
                date.Year = None
                date.Month = None
                date.Day = None
                date.Hour = int(g[0])
                date.Minute = int(g[1])
            return date
        return None

    @staticmethod
    def now() -> "MyDate":
        return MyDate(_dt.datetime.now())

    # -------------------------------------------------------------- datetime
    def to_datetime(self) -> _dt.datetime:
        year = self.Year if self.Year and self.Year > 0 else 1
        month = self.Month if self.Month and 1 <= self.Month <= 12 else 1
        day = self.Day if self.Day and 1 <= self.Day <= 31 else 1
        day = min(day, _days_in_month(year, month))
        hour = min(max(self.Hour, 0), 23)
        minute = min(max(self.Minute, 0), 59)
        second = min(max(self.Second, 0), 59)
        return _dt.datetime(year, month, day, hour, minute, second)

    def day_of_week(self) -> int:
        # joda ``dayOfWeek``: Monday = 1 ... Sunday = 7
        return self.to_datetime().isoweekday()

    def day_of_year(self) -> int:
        return self.to_datetime().timetuple().tm_yday

    # --------------------------------------------------------------- arithmetic
    def _plus(self, years=0, months=0, days=0, hours=0, minutes=0, seconds=0) -> "MyDate":
        base = self.to_datetime()
        year = base.year + years
        month = base.month + months
        year += (month - 1) // 12
        month = (month - 1) % 12 + 1
        day = min(base.day, _days_in_month(year, month))
        result = base.replace(year=year, month=month, day=day)
        result = result + _dt.timedelta(days=days, hours=hours, minutes=minutes, seconds=seconds)
        return MyDate(result)

    def add_years(self, d: int) -> "MyDate":
        if self.Year is not None and self.Month is not None and self.Day is not None:
            return self._plus(years=d)
        return MyDate((self.Year or 0) + d, self.Month, self.Day, self.Hour, self.Minute, self.Second)

    def add_months(self, d: int) -> "MyDate":
        # The C# implementation always goes through DateTime arithmetic, which
        # clamps the day when the target month is shorter (Jan 31 + 1 -> Feb 29).
        if self.Year is not None and self.Month is not None and self.Day is not None:
            return self._plus(months=d)
        t = (self.Month or 0) + d
        if 1 <= t <= 12:
            return MyDate(self.Year, t, self.Day, self.Hour, self.Minute, self.Second)
        return self._plus(months=d)

    def add_days(self, d: int) -> "MyDate":
        if self.Year is not None and self.Year > 1900:
            t = (self.Day or 0) + d
            if 1 <= t <= 28:
                return MyDate(self.Year, self.Month, t, self.Hour, self.Minute, self.Second)
            return self._plus(days=d)
        return self._add_time_span(d * 86400)

    def add_hours(self, d: int) -> "MyDate":
        t = self.Hour + d
        if self.Year is not None and 0 <= t < 24:
            return MyDate(self.Year, self.Month, self.Day, t, self.Minute, self.Second)
        if self.Year is not None and self.Year > 1900:
            return self._plus(hours=d)
        return self._add_time_span(d * 3600)

    def add_minutes(self, d: int) -> "MyDate":
        t = self.Minute + d
        if self.Year is not None and 0 <= t <= 59:
            return MyDate(self.Year, self.Month, self.Day, self.Hour, t, self.Second)
        if self.Year is not None and self.Year > 1900:
            return self._plus(minutes=d)
        return self._add_time_span(d * 60)

    def add_seconds(self, d: int) -> "MyDate":
        t = self.Second + d
        if self.Year is not None and 0 <= t <= 59:
            return MyDate(self.Year, self.Month, self.Day, self.Hour, self.Minute, t)
        if self.Year is not None and self.Year > 1900:
            return self._plus(seconds=d)
        return self._add_time_span(d)

    def _add_time_span(self, total_seconds: int) -> "MyDate":
        total = self.Hour * 3600 + self.Minute * 60 + self.Second + total_seconds
        new_second = total % 60
        total_minutes = (total - new_second) // 60
        new_minute = total_minutes % 60
        total_hours = (total_minutes - new_minute) // 60
        new_hour = total_hours % 24
        new_day = (total_hours - new_hour) // 24
        result = MyDate(_skip=True)
        result.Year = None
        result.Month = None
        result.Day = new_day
        result.Hour = new_hour
        result.Minute = new_minute
        result.Second = new_second
        return result

    def add_date(self, num: "MyDate") -> "MyDate":
        return self._plus(
            years=num.Year or 0,
            months=num.Month or 0,
            days=num.Day or 0,
            hours=num.Hour,
            minutes=num.Minute,
            seconds=num.Second,
        )

    def sub_date(self, num: "MyDate") -> "MyDate":
        return self._plus(
            years=-(num.Year or 0),
            months=-(num.Month or 0),
            days=-(num.Day or 0),
            hours=-num.Hour,
            minutes=-num.Minute,
            seconds=-num.Second,
        )

    def add_number(self, num: Decimal) -> "MyDate":
        return MyDate(self.to_number() + num)

    def sub_number(self, num: Decimal) -> "MyDate":
        return MyDate(self.to_number() - num)

    def mul_number(self, num: Decimal) -> "MyDate":
        return MyDate(self.to_number() * num)

    def div_number(self, num: Decimal) -> "MyDate":
        from toolgood_algorithm.internals.numeric import divide

        return MyDate(divide(self.to_number(), num, DECIMAL32))

    # ------------------------------------------------------------------ values
    def to_number(self) -> Decimal:
        result = Decimal(self.Second) / Decimal(60)
        result = (result + Decimal(self.Minute)) / Decimal(60)
        result = (result + Decimal(self.Hour)) / Decimal(24)
        if self.Year is not None and self.Year > 1900 and self.Month and self.Day:
            start = _dt.date(self.Year, self.Month, self.Day)
            days = (start - _dt.date(1900, 1, 1)).days + 2
            return result + Decimal(days)
        if self.Day is not None:
            return result + Decimal(self.Day)
        return result

    def to_long(self) -> int:
        d = 0
        if self.Year is not None:
            d += self.Year * 10000000000
        if self.Month is not None:
            d += self.Month * 100000000
        if self.Day is not None:
            d += self.Day * 1000000
        d += self.Hour * 10000
        d += self.Minute * 100
        d += self.Second
        return d

    # --------------------------------------------------------------- formatting
    def __str__(self) -> str:
        buf: list[str] = []
        if self.Year is not None and self.Year > 0:
            buf.append(str(self.Year))
            buf.append("-")
            buf.append(f"{self.Month:02d}")
            buf.append("-")
            buf.append(f"{self.Day:02d}")
            buf.append(" ")
            buf.append(f"{self.Hour:02d}")
            buf.append(":")
            buf.append(f"{self.Minute:02d}")
            buf.append(":")
            buf.append(f"{self.Second:02d}")
        else:
            if self.Day is not None and self.Day > 0:
                buf.append(str(self.Day))
                buf.append(" ")
            buf.append(f"{self.Hour:02d}")
            buf.append(":")
            buf.append(f"{self.Minute:02d}")
            buf.append(":")
            buf.append(f"{self.Second:02d}")
        return "".join(buf)

    def to_string(self, f: str = None) -> str:
        if f is None:
            return str(self)
        if self.Year is None or self.Year == 0:
            return str(self)
        return format_datetime(self.to_datetime(), f)

    def __repr__(self) -> str:  # pragma: no cover - debugging helper
        return f"MyDate({self.Year!r},{self.Month!r},{self.Day!r},{self.Hour},{self.Minute},{self.Second})"

    # ------------------------------------------------------------- C#-style API
    def ToNumber(self):
        return self.to_number()

    def ToLong(self):
        return self.to_long()

    def ToDateTime(self):
        return self.to_datetime()

    def AddYears(self, value):
        return self.add_years(value)

    def AddMonths(self, value):
        return self.add_months(value)

    def AddDays(self, value):
        return self.add_days(value)

    def AddHours(self, value):
        return self.add_hours(value)

    def AddMinutes(self, value):
        return self.add_minutes(value)

    def AddSeconds(self, value):
        return self.add_seconds(value)

    def DayOfWeek(self):
        return self.day_of_week()

    def DayOfYear(self):
        return self.day_of_year()


_FMT_TOKEN = re.compile(r"yyyy|yy|MM|M|dd|d|HH|H|hh|h|mm|m|ss|s|fff|ff|f|tt|zzz|K")


def format_datetime(dt: _dt.datetime, pattern: str) -> str:
    """Minimal .NET/Joda custom date formatter (yyyy, MM, dd, HH, mm, ss...)."""
    if not pattern:
        return str(dt)
    out: list[str] = []
    i = 0
    n = len(pattern)
    # token order matters: longest first
    tokens = (
        "yyyy", "fff", "zzz", "yy", "MM", "dd", "HH", "hh", "mm", "ss", "ff",
        "M", "d", "H", "h", "m", "s", "f", "K", "tt",
    )
    while i < n:
        c = pattern[i]
        if c in ("'", '"'):
            quote = c
            i += 1
            while i < n and pattern[i] != quote:
                out.append(pattern[i])
                i += 1
            i += 1
            continue
        if c == "\\" and i + 1 < n:
            out.append(pattern[i + 1])
            i += 2
            continue
        matched = None
        for token in tokens:
            if pattern.startswith(token, i):
                matched = token
                break
        if matched is None:
            out.append(c)
            i += 1
            continue
        i += len(matched)
        if matched == "yyyy":
            out.append(f"{dt.year:04d}")
        elif matched == "yy":
            out.append(f"{dt.year % 100:02d}")
        elif matched == "MM":
            out.append(f"{dt.month:02d}")
        elif matched == "M":
            out.append(str(dt.month))
        elif matched == "dd":
            out.append(f"{dt.day:02d}")
        elif matched == "d":
            out.append(str(dt.day))
        elif matched == "HH":
            out.append(f"{dt.hour:02d}")
        elif matched == "H":
            out.append(str(dt.hour))
        elif matched == "hh":
            out.append(f"{(dt.hour % 12) or 12:02d}")
        elif matched == "h":
            out.append(str((dt.hour % 12) or 12))
        elif matched == "mm":
            out.append(f"{dt.minute:02d}")
        elif matched == "m":
            out.append(str(dt.minute))
        elif matched == "ss":
            out.append(f"{dt.second:02d}")
        elif matched == "s":
            out.append(str(dt.second))
        elif matched == "fff":
            out.append(f"{dt.microsecond // 1000:03d}")
        elif matched == "ff":
            out.append(f"{dt.microsecond // 10000:02d}")
        elif matched == "f":
            out.append(str(dt.microsecond // 100000))
        elif matched == "tt":
            out.append("AM" if dt.hour < 12 else "PM")
        elif matched == "zzz":
            out.append("+00:00")
        elif matched == "K":
            out.append("Z")
    return "".join(out)
