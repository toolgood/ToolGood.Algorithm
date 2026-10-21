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
