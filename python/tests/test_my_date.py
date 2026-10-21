"""Tests for ``MyDate``."""
from __future__ import annotations

import datetime as dt
from decimal import Decimal

import pytest

from toolgood_algorithm import MyDate


def test_parse_variants():
    assert str(MyDate.parse("2024-1-2 3:4:5")) == "2024-01-02 03:04:05"
    assert str(MyDate.parse("2024/01/02 03:04:05")) == "2024-01-02 03:04:05"
    assert str(MyDate.parse("2024-01-02 03:04")) == "2024-01-02 03:04:00"
    assert str(MyDate.parse("2024-01-02")) == "2024-01-02 00:00:00"
    assert str(MyDate.parse("5 12:30:00")) == "5 12:30:00"
    assert str(MyDate.parse("12:30:00")) == "12:30:00"
    assert str(MyDate.parse("12:30")) == "12:30:00"
    assert MyDate.parse("not a date") is None


def test_to_string_and_format():
    date = MyDate(2024, 3, 5, 6, 7, 8)
    assert str(date) == "2024-03-05 06:07:08"
    assert date.to_string("yyyy-MM-dd") == "2024-03-05"
    assert date.to_string("yyyy年M月d日") == "2024年3月5日"
    assert date.to_string("HH:mm:ss") == "06:07:08"


def test_serial_number_round_trip():
    date = MyDate(2024, 1, 1)
    number = date.to_number()
    assert number == Decimal("45292")
    restored = MyDate(number)
    assert (restored.Year, restored.Month, restored.Day) == (2024, 1, 1)


def test_add_operations():
    date = MyDate(2024, 1, 31)
    assert str(date.add_months(1)) == "2024-02-29 00:00:00"
    assert str(date.add_days(1)) == "2024-02-01 00:00:00"
    assert str(date.add_years(1)) == "2025-01-31 00:00:00"
    assert str(MyDate(2024, 1, 1, 23, 59, 59).add_seconds(1)) == "2024-01-02 00:00:00"


def test_weekday_and_day_of_year():
    # 2024-01-01 is a Monday
    assert MyDate(2024, 1, 1).day_of_week() == 1
    assert MyDate(2024, 1, 7).day_of_week() == 7
    assert MyDate(2024, 12, 31).day_of_year() == 366


def test_time_only_values():
    value = MyDate.parse("12:30:00")
    assert value.Year is None
    assert value.to_number() == Decimal("0.5208333333333333333333333333")
    assert str(value.add_hours(12)) == "1 00:30:00"


def test_camel_case_api():
    date = MyDate(2024, 1, 2)
    assert date.ToNumber() == date.to_number()
    assert date.ToLong() == date.to_long()
    assert str(date.AddDays(1)) == "2024-01-03 00:00:00"
    assert date.DayOfWeek() == 2


def test_from_datetime():
    date = MyDate(dt.datetime(2024, 5, 6, 7, 8, 9))
    assert str(date) == "2024-05-06 07:08:09"
    assert date.to_datetime() == dt.datetime(2024, 5, 6, 7, 8, 9)


# ---------------------------------------------------------------------------
# Ported from java/.../helpers/MyDateTest.java
# ---------------------------------------------------------------------------

def test_constructor_date_time():
    source = MyDate(2024, 6, 15, 10, 30, 45)
    my_date = MyDate(source)

    assert my_date.Year == 2024
    assert my_date.Month == 6
    assert my_date.Day == 15
    assert my_date.Hour == 10
    assert my_date.Minute == 30
    assert my_date.Second == 45


def test_constructor_time_span():
    ts = MyDate(None, None, 1, 10, 30, 45)
    my_date = MyDate(ts)

    assert my_date.Year is None
    assert my_date.Month is None
    assert my_date.Day == 1
    assert my_date.Hour == 10
    assert my_date.Minute == 30
    assert my_date.Second == 45


def test_constructor_params():
    my_date = MyDate(2024, 6, 15, 10, 30, 45)

    assert my_date.Year == 2024
    assert my_date.Month == 6
    assert my_date.Day == 15
    assert my_date.Hour == 10
    assert my_date.Minute == 30
    assert my_date.Second == 45


def test_parse_date_time():
    my_date = MyDate.parse("2024-06-15 10:30:45")
    assert my_date.Year == 2024
    assert my_date.Month == 6
    assert my_date.Day == 15
    assert my_date.Hour == 10
    assert my_date.Minute == 30
    assert my_date.Second == 45


def test_parse_date_time_slash():
    my_date = MyDate.parse("2024/06/15 10:30:45")
    assert my_date.Year == 2024
    assert my_date.Month == 6
    assert my_date.Day == 15
    assert my_date.Hour == 10
    assert my_date.Minute == 30
    assert my_date.Second == 45


def test_parse_date():
    my_date = MyDate.parse("2024-06-15")
    assert my_date.Year == 2024
    assert my_date.Month == 6
    assert my_date.Day == 15
    assert my_date.Hour == 0
    assert my_date.Minute == 0
    assert my_date.Second == 0


def test_parse_time():
    my_date = MyDate.parse("10:30:45")
    assert my_date.Year is None
    assert my_date.Month is None
    assert my_date.Day is None
    assert my_date.Hour == 10
    assert my_date.Minute == 30
    assert my_date.Second == 45


def test_parse_time_no_seconds():
    my_date = MyDate.parse("10:30")
    assert my_date.Year is None
    assert my_date.Month is None
    assert my_date.Day is None
    assert my_date.Hour == 10
    assert my_date.Minute == 30
    assert my_date.Second == 0


def test_parse_invalid():
    my_date = MyDate.parse("invalid date")
    assert my_date is None


def test_to_date_time():
    my_date = MyDate(2024, 6, 15, 10, 30, 45)
    value = my_date.to_datetime()

    assert value.year == 2024
    assert value.month == 6
    assert value.day == 15
    assert value.hour == 10
    assert value.minute == 30
    assert value.second == 45


@pytest.mark.skip(reason="Python to_datetime() 返回 naive datetime，无 DateTimeZone 时区参数")
def test_to_date_time_kind():
    my_date = MyDate(2024, 6, 15, 10, 30, 45)
    my_date.to_datetime()


def test_add_years():
    my_date = MyDate(2024, 6, 15, 10, 30, 45)
    result = my_date.add_years(1)

    assert result.Year == 2025
    assert result.Month == 6
    assert result.Day == 15


def test_add_years_negative():
    my_date = MyDate(2024, 6, 15, 10, 30, 45)
    result = my_date.add_years(-1)

    assert result.Year == 2023


def test_add_months():
    my_date = MyDate(2024, 6, 15, 10, 30, 45)
    result = my_date.add_months(1)

    assert result.Month == 7


def test_add_months_overflow():
    my_date = MyDate(2024, 11, 15, 10, 30, 45)
    result = my_date.add_months(2)

    assert result.Year == 2025
    assert result.Month == 1


def test_add_days():
    my_date = MyDate(2024, 6, 15, 10, 30, 45)
    result = my_date.add_days(5)

    assert result.Day == 20


def test_add_days_negative():
    my_date = MyDate(2024, 6, 15, 10, 30, 45)
    result = my_date.add_days(-5)

    assert result.Day == 10


def test_add_hours():
    my_date = MyDate(2024, 6, 15, 10, 30, 45)
    result = my_date.add_hours(5)

    assert result.Hour == 15


def test_add_hours_no_overflow():
    my_date = MyDate(2024, 6, 15, 10, 30, 45)
    result = my_date.add_hours(2)

    assert result.Hour == 12


def test_add_minutes():
    my_date = MyDate(2024, 6, 15, 10, 30, 45)
    result = my_date.add_minutes(15)

    assert result.Minute == 45


def test_add_minutes_no_overflow():
    my_date = MyDate(2024, 6, 15, 10, 30, 45)
    result = my_date.add_minutes(10)

    assert result.Minute == 40


def test_add_seconds():
    my_date = MyDate(2024, 6, 15, 10, 30, 45)
    result = my_date.add_seconds(10)

    assert result.Second == 55


def test_add_seconds_no_overflow():
    my_date = MyDate(2024, 6, 15, 10, 30, 45)
    result = my_date.add_seconds(5)

    assert result.Second == 50


def test_to_string_date_time():
    my_date = MyDate(2024, 6, 15, 10, 30, 45)
    value = str(my_date)

    assert "2024" in value
    assert "06" in value
    assert "15" in value


def test_to_string_time_only():
    my_date = MyDate(None, None, None, 10, 30, 45)
    value = str(my_date)

    assert "10:30:45" in value
