"""Tests ported from ``java/.../AlgorithmEngine/DateTimesTest.java``.

One Java ``@Test`` maps to one pytest function (snake_case).
"""
from __future__ import annotations

import pytest

from toolgood_algorithm import AlgorithmEngine, MyDate

_DEFAULT_DATE = MyDate(1900, 1, 1, 0, 0, 0)
_DEFAULT_TIME = MyDate(0, 0, 0, 0, 0, 0)


def test_datevalue():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_my_date("DATEVALUE('2016-01-01')", MyDate(1900, 1, 1, 0, 0, 0))
    assert dt.Year == 2016
    assert dt.Month == 1
    assert dt.Day == 1

    dt = engine.try_evaluate_my_date("DATEVALUE（'2016-01-01'）", MyDate(1900, 1, 1, 0, 0, 0))
    assert dt.Year == 2016
    assert dt.Month == 1
    assert dt.Day == 1

    dt = engine.try_evaluate_my_date("DATEVALUE('2016/01/01')", MyDate(1900, 1, 1, 0, 0, 0))
    assert dt.Year == 2016
    assert dt.Month == 1
    assert dt.Day == 1

    dt = engine.try_evaluate_my_date("DATEVALUE('1691234899000',0)", MyDate.now())
    assert dt.Year == 2023
    assert dt.Month == 8
    assert dt.Day == 5
    assert dt.Hour == 19
    assert dt.Minute == 28
    assert dt.Second == 19

    dt = engine.try_evaluate_my_date("DATEVALUE('1691234899',0)", MyDate.now())
    assert dt.Year == 2023
    assert dt.Month == 8
    assert dt.Day == 5
    assert dt.Hour == 19
    assert dt.Minute == 28
    assert dt.Second == 19

    engine.UseLocalTime = True
    dt = engine.try_evaluate_my_date("DATEVALUE('1691234899000',0)", MyDate.now())
    assert dt.Year == 2023
    assert dt.Month == 8
    assert dt.Day == 5
    assert dt.Hour == 19
    assert dt.Minute == 28
    assert dt.Second == 19

    dt = engine.try_evaluate_my_date("DATEVALUE('1691234899',0)", MyDate.now())
    assert dt.Year == 2023
    assert dt.Month == 8
    assert dt.Day == 5
    assert dt.Hour == 19
    assert dt.Minute == 28
    assert dt.Second == 19

    dt = engine.try_evaluate_my_date("DATEVALUE('1691234899000')", MyDate.now())
    assert dt.Year == 2023
    assert dt.Month == 8
    assert dt.Day == 5
    assert dt.Hour == 19
    assert dt.Minute == 28
    assert dt.Second == 19

    dt = engine.try_evaluate_my_date("DATEVALUE('1691234899')", MyDate.now())
    assert dt.Year == 2023
    assert dt.Month == 8
    assert dt.Day == 5
    assert dt.Hour == 19
    assert dt.Minute == 28
    assert dt.Second == 19


def test_timestamp():
    engine = AlgorithmEngine()
    engine.UseLocalTime = True

    dt = engine.try_evaluate("TIMESTAMP('2016-01-01')", 0)
    assert dt == 1451577600000

    dt = engine.try_evaluate("TIMESTAMP('2016/01/01')", 0)
    assert dt == 1451577600000

    dt = engine.try_evaluate("TIMESTAMP('2016-01-01',0)", 0)
    assert dt == 1451577600000

    dt = engine.try_evaluate("TIMESTAMP('2016-01-01',1)", 0)
    assert dt == 1451577600


def test_timevalue():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_my_date("TIMEVALUE('12:12:12')", MyDate(0, 0, 0, 0, 0, 0))
    assert dt.Hour == 12
    assert dt.Minute == 12
    assert dt.Second == 12


def test_date():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_my_date("DATE(2016,1,1)", MyDate(1900, 1, 1, 0, 0, 0))
    assert dt.Year == 2016
    assert dt.Month == 1
    assert dt.Day == 1


def test_time():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_my_date("time(11,12,13)", MyDate(0, 0, 0, 0, 0, 0))
    assert dt.Hour == 11
    assert dt.Minute == 12
    assert dt.Second == 13


def test_now():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_my_date("now()", MyDate(1900, 1, 1, 0, 0, 0))
    assert str(MyDate.now()) == str(dt)


def test_today():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_my_date("Today()", MyDate(1900, 1, 1, 0, 0, 0))
    today = MyDate.now()
    assert today.Year == dt.Year
    assert today.Month == dt.Month
    assert today.Day == dt.Day


def test_year():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("year(now())", 0)
    assert MyDate.now().Year == dt


def test_month():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("Month(now())", 0)
    assert MyDate.now().Month == dt


def test_day():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("Day(now())", 0)
    assert MyDate.now().Day == dt


def test_hour():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("Hour(now())", 0)
    assert MyDate.now().Hour == dt


def test_minute():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("Minute(now())", 0)
    assert MyDate.now().Minute == dt


def test_second():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("Second(now())", 0)
    assert MyDate.now().Second == dt


def test_weekday():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("WEEKDAY(date(2017,1,7))", 0)
    assert dt == 7
    dt = engine.try_evaluate("WEEKDAY(date(2017,1,7),1)", 0)
    assert dt == 7
    dt = engine.try_evaluate("WEEKDAY(date(2017,1,7),2)", 0)
    assert dt == 6
    dt = engine.try_evaluate("WEEKDAY(date(2017,1,7),3)", 0)
    assert dt == 5

    dt = engine.try_evaluate("WEEKDAY(date(2017,1,8),1)", 0)
    assert dt == 1
    dt = engine.try_evaluate("WEEKDAY(date(2017,1,8),2)", 0)
    assert dt == 7
    dt = engine.try_evaluate("WEEKDAY(date(2017,1,8),3)", 0)
    assert dt == 6

    dt = engine.try_evaluate("WEEKDAY(date(2017,1,2),1)", 0)
    assert dt == 2
    dt = engine.try_evaluate("WEEKDAY(date(2017,1,2),2)", 0)
    assert dt == 1
    dt = engine.try_evaluate("WEEKDAY(date(2017,1,2),3)", 0)
    assert dt == 0


def test_weekday_excel_compatible():
    engine = AlgorithmEngine()

    # 2017年1月1日是周日，1月7日是周六，1月2日是周一
    # type 1: 周日=1, 周六=7 ; type 2: 周一=1, 周日=7 ; type 3: 周一=0, 周日=6

    # 2017-1-2 (周一)
    assert engine.try_evaluate("WEEKDAY(date(2017,1,2),1)", 0) == 2
    assert engine.try_evaluate("WEEKDAY(date(2017,1,2),2)", 0) == 1
    assert engine.try_evaluate("WEEKDAY(date(2017,1,2),3)", 0) == 0
    assert engine.try_evaluate("WEEKDAY(date(2017,1,2),11)", 0) == 1
    assert engine.try_evaluate("WEEKDAY(date(2017,1,2),12)", 0) == 7
    assert engine.try_evaluate("WEEKDAY(date(2017,1,2),13)", 0) == 6
    assert engine.try_evaluate("WEEKDAY(date(2017,1,2),14)", 0) == 5
    assert engine.try_evaluate("WEEKDAY(date(2017,1,2),15)", 0) == 4
    assert engine.try_evaluate("WEEKDAY(date(2017,1,2),16)", 0) == 3
    assert engine.try_evaluate("WEEKDAY(date(2017,1,2),17)", 0) == 2

    # 2017-1-7 (周六)
    assert engine.try_evaluate("WEEKDAY(date(2017,1,7),1)", 0) == 7
    assert engine.try_evaluate("WEEKDAY(date(2017,1,7),2)", 0) == 6
    assert engine.try_evaluate("WEEKDAY(date(2017,1,7),3)", 0) == 5
    assert engine.try_evaluate("WEEKDAY(date(2017,1,7),11)", 0) == 6
    assert engine.try_evaluate("WEEKDAY(date(2017,1,7),12)", 0) == 5
    assert engine.try_evaluate("WEEKDAY(date(2017,1,7),13)", 0) == 4
    assert engine.try_evaluate("WEEKDAY(date(2017,1,7),14)", 0) == 3
    assert engine.try_evaluate("WEEKDAY(date(2017,1,7),15)", 0) == 2
    assert engine.try_evaluate("WEEKDAY(date(2017,1,7),16)", 0) == 1
    assert engine.try_evaluate("WEEKDAY(date(2017,1,7),17)", 0) == 7

    # 2017-1-8 (周日)
    assert engine.try_evaluate("WEEKDAY(date(2017,1,8),1)", 0) == 1
    assert engine.try_evaluate("WEEKDAY(date(2017,1,8),2)", 0) == 7
    assert engine.try_evaluate("WEEKDAY(date(2017,1,8),3)", 0) == 6
    assert engine.try_evaluate("WEEKDAY(date(2017,1,8),11)", 0) == 7
    assert engine.try_evaluate("WEEKDAY(date(2017,1,8),12)", 0) == 6
    assert engine.try_evaluate("WEEKDAY(date(2017,1,8),13)", 0) == 5
    assert engine.try_evaluate("WEEKDAY(date(2017,1,8),14)", 0) == 4
    assert engine.try_evaluate("WEEKDAY(date(2017,1,8),15)", 0) == 3
    assert engine.try_evaluate("WEEKDAY(date(2017,1,8),16)", 0) == 2
    assert engine.try_evaluate("WEEKDAY(date(2017,1,8),17)", 0) == 1


def test_datedif():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("DATEDIF('1975-1-30','2017-1-7','y')", 0)
    assert dt == 41

    dt = engine.try_evaluate("DATEDIF('1975-1-30','2017-1-7','m')", 0)
    assert dt == 503

    dt = engine.try_evaluate("DATEDIF('1975-1-30','2017-1-7','d')", 0)
    assert dt == 15318

    dt = engine.try_evaluate("DATEDIF('1975-1-30','2017-1-7','yd')", 0)
    assert dt == 342

    dt = engine.try_evaluate("DATEDIF('1975-1-30','2017-1-7','ym')", 0)
    assert dt == 11

    dt = engine.try_evaluate("DATEDIF('1975-1-30','2017-1-7','md')", 0)
    assert dt == 8


def test_days():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("DAYS('2017-1-7','1975-1-30')", 0)
    assert dt == 15318

    dt = engine.try_evaluate("DAYS('2016-3-1','2016-2-28')", 0)
    assert dt == 2

    dt = engine.try_evaluate("DAYS('2016-2-28','2016-3-1')", 0)
    assert dt == -2


def test_days360():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("DAYS360('1975-1-30','2017-1-7')", 0)
    assert dt == 15097


def test_datedif_yd():
    engine = AlgorithmEngine()
    # Excel 官方例子: DATEDIF("6/15/2015","9/15/2016","YD") = 92
    dt = engine.try_evaluate("DATEDIF('2015-6-15','2016-9-15','yd')", 0)
    assert dt == 92

    # 跨闰年: 同月日天数差应为 0
    dt = engine.try_evaluate("DATEDIF('2015-3-1','2016-3-1','yd')", 0)
    assert dt == 0

    dt = engine.try_evaluate("DATEDIF('2015-3-1','2016-3-2','yd')", 0)
    assert dt == 1


def test_days360_us_method():
    engine = AlgorithmEngine()
    # US(NASD) 方法: start 为 2 月最后一天按 30 日调整, end 月末按调整后的 startDay 决定 30/31
    dt = engine.try_evaluate("DAYS360('2016-2-29','2016-3-31')", 0)
    assert dt == 30

    dt = engine.try_evaluate("DAYS360('2016-2-29','2016-3-31',false)", 0)
    assert dt == 30


def test_edate():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_my_date('EDATE("2012-1-31",32)', MyDate(1900, 1, 1, 0, 0, 0))
    expected = MyDate.parse("2014-09-30")
    assert expected.Year == dt.Year
    assert expected.Month == dt.Month
    assert expected.Day == dt.Day


def test_eomonth():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_my_date('EOMONTH("2012-2-1",32)', MyDate(1900, 1, 1, 0, 0, 0))
    expected = MyDate.parse("2014-10-31")
    assert expected.Year == dt.Year
    assert expected.Month == dt.Month
    assert expected.Day == dt.Day


def test_networkdays():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate('NETWORKDAYS("2012-1-1","2013-1-1")', 0)
    assert dt == 262


def test_workday():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_my_date('WORKDAY("2012-1-2",145)', MyDate(1900, 1, 1, 0, 0, 0))
    expected = MyDate.parse("2012-07-23")
    assert expected.Year == dt.Year
    assert expected.Month == dt.Month
    assert expected.Day == dt.Day


def test_workday_excel_compatible():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_my_date('WORKDAY("2024-01-15", 5)', MyDate(1900, 1, 1, 0, 0, 0))
    assert dt.Year == 2024
    assert dt.Month == 1
    assert dt.Day == 22

    dt = engine.try_evaluate_my_date('WORKDAY("2024-01-15", -5)', MyDate(1900, 1, 1, 0, 0, 0))
    assert dt.Year == 2024
    assert dt.Month == 1
    assert dt.Day == 8

    dt = engine.try_evaluate_my_date('WORKDAY("2024-01-15", 0)', MyDate(1900, 1, 1, 0, 0, 0))
    assert dt.Year == 2024
    assert dt.Month == 1
    assert dt.Day == 15

    dt = engine.try_evaluate_my_date('WORKDAY("2024-01-12", 1)', MyDate(1900, 1, 1, 0, 0, 0))
    assert dt.Year == 2024
    assert dt.Month == 1
    assert dt.Day == 15

    dt = engine.try_evaluate_my_date('WORKDAY("2024-01-15", -1)', MyDate(1900, 1, 1, 0, 0, 0))
    assert dt.Year == 2024
    assert dt.Month == 1
    assert dt.Day == 12


def test_date_excel_compatible():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_my_date("DATE(2024, 1, 15)", MyDate(1900, 1, 1, 0, 0, 0))
    assert dt.Year == 2024
    assert dt.Month == 1
    assert dt.Day == 15

    dt = engine.try_evaluate_my_date("DATE(2024, 13, 1)", MyDate(1900, 1, 1, 0, 0, 0))
    assert dt.Year == 2025
    assert dt.Month == 1
    assert dt.Day == 1

    dt = engine.try_evaluate_my_date("DATE(2024, 1, 32)", MyDate(1900, 1, 1, 0, 0, 0))
    assert dt.Year == 2024
    assert dt.Month == 2
    assert dt.Day == 1

    dt = engine.try_evaluate_my_date("DATE(2024, 0, 1)", MyDate(1900, 1, 1, 0, 0, 0))
    assert dt.Year == 2023
    assert dt.Month == 12
    assert dt.Day == 1

    dt = engine.try_evaluate_my_date("DATE(2024, 2, 30)", MyDate(1900, 1, 1, 0, 0, 0))
    assert dt.Year == 2024
    assert dt.Month == 3
    assert dt.Day == 1

    dt = engine.try_evaluate_my_date("DATE(2024, -1, 1)", MyDate(1900, 1, 1, 0, 0, 0))
    assert dt.Year == 2023
    assert dt.Month == 11
    assert dt.Day == 1

    dt = engine.try_evaluate_my_date("DATE(2024, 12, 0)", MyDate(1900, 1, 1, 0, 0, 0))
    assert dt.Year == 2024
    assert dt.Month == 11
    assert dt.Day == 30

    dt = engine.try_evaluate_my_date("DATE(2024, 1, -1)", MyDate(1900, 1, 1, 0, 0, 0))
    assert dt.Year == 2023
    assert dt.Month == 12
    assert dt.Day == 30


def test_weeknum():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate('WEEKNUM("2016-1-3")', 0)
    assert dt == 2
    dt = engine.try_evaluate('WEEKNUM("2016-1-2")', 0)
    assert dt == 1

    dt = engine.try_evaluate('WEEKNUM("2016-1-4",2)', 0)
    assert dt == 2
    dt = engine.try_evaluate('WEEKNUM("2016-1-3",2)', 0)
    assert dt == 1
    dt = engine.try_evaluate('WEEKNUM("2016-1-2",2)', 0)
    assert dt == 1


def test_weeknum_excel_compatible():
    engine = AlgorithmEngine()

    # 2016-1-1 (周五)
    assert engine.try_evaluate("WEEKNUM(date(2016,1,1),1)", 0) == 1
    assert engine.try_evaluate("WEEKNUM(date(2016,1,1),2)", 0) == 1
    assert engine.try_evaluate("WEEKNUM(date(2016,1,1),11)", 0) == 1
    assert engine.try_evaluate("WEEKNUM(date(2016,1,1),12)", 0) == 1
    assert engine.try_evaluate("WEEKNUM(date(2016,1,1),13)", 0) == 1
    assert engine.try_evaluate("WEEKNUM(date(2016,1,1),14)", 0) == 1
    assert engine.try_evaluate("WEEKNUM(date(2016,1,1),15)", 0) == 1
    assert engine.try_evaluate("WEEKNUM(date(2016,1,1),16)", 0) == 1
    assert engine.try_evaluate("WEEKNUM(date(2016,1,1),17)", 0) == 1

    # 2016-1-3 (周日)
    assert engine.try_evaluate("WEEKNUM(date(2016,1,3),1)", 0) == 2
    assert engine.try_evaluate("WEEKNUM(date(2016,1,3),2)", 0) == 1
    assert engine.try_evaluate("WEEKNUM(date(2016,1,3),11)", 0) == 1
    assert engine.try_evaluate("WEEKNUM(date(2016,1,3),16)", 0) == 2
    assert engine.try_evaluate("WEEKNUM(date(2016,1,3),17)", 0) == 2

    # 2016-1-4 (周一)
    assert engine.try_evaluate("WEEKNUM(date(2016,1,4),1)", 0) == 2
    assert engine.try_evaluate("WEEKNUM(date(2016,1,4),2)", 0) == 2
    assert engine.try_evaluate("WEEKNUM(date(2016,1,4),11)", 0) == 2
    assert engine.try_evaluate("WEEKNUM(date(2016,1,4),21)", 0) == 1

    # ISO 8601 (type 21)
    assert engine.try_evaluate("WEEKNUM(date(2016,1,4),21)", 0) == 1
    assert engine.try_evaluate("WEEKNUM(date(2016,1,3),21)", 0) == 53

    assert engine.try_evaluate("WEEKNUM(date(2021,1,1),21)", 0) == 53
    assert engine.try_evaluate("WEEKNUM(date(2021,1,4),21)", 0) == 1


def test_yearfrac():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("YEARFRAC('2012-1-1', '2012-7-1')", 0.0)
    assert t == pytest.approx(0.5, abs=1e-3)

    t = engine.try_evaluate("YEARFRAC('2012-1-1', '2013-1-1')", 0.0)
    assert t == pytest.approx(1.0, abs=1e-3)

    t = engine.try_evaluate("YEARFRAC('2012-1-1', '2012-7-1', 0)", 0.0)
    assert t == pytest.approx(0.5, abs=1e-3)

    t = engine.try_evaluate("YEARFRAC('2012-1-1', '2012-7-1', 1)", 0.0)
    assert t == pytest.approx(0.49726776, abs=1e-3)

    t = engine.try_evaluate("YEARFRAC('2012-1-1', '2012-7-1', 2)", 0.0)
    assert t == pytest.approx(0.505555556, abs=1e-3)

    t = engine.try_evaluate("YEARFRAC('2012-1-1', '2012-7-1', 3)", 0.0)
    assert t == pytest.approx(0.498630137, abs=1e-3)

    t = engine.try_evaluate("YEARFRAC('2012-1-1', '2012-7-1', 4)", 0.0)
    assert t == pytest.approx(0.5, abs=1e-3)


def test_yearfrac_basis0_30_360():
    engine = AlgorithmEngine()
    # basis 0 (US NASD 30/360): 2012-2-29 为 2 月最后一天 → d1=30, end 3-31 保持 31
    t = engine.try_evaluate("YEARFRAC('2012-2-29','2012-3-31',0)", 0.0)
    assert t == pytest.approx(31.0 / 360.0, abs=1e-6)

    # 两月均按 30 日: (60-30)/360
    t = engine.try_evaluate("YEARFRAC('2012-1-31','2012-3-31',0)", 0.0)
    assert t == pytest.approx(60.0 / 360.0, abs=1e-6)

    t = engine.try_evaluate("YEARFRAC('2012-2-29','2013-2-28',0)", 0.0)
    assert t == pytest.approx(1.0, abs=1e-6)

    t = engine.try_evaluate("YEARFRAC('2012-1-1','2012-7-1',0)", 0.0)
    assert t == pytest.approx(0.5, abs=1e-6)


def test_add_years():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("'2000-02-01'.addYears(1).year()", 0)
    assert dt == 2001

    dt = engine.try_evaluate("'2000/02/01'.addYears(1).year()", 0)
    assert dt == 2001


def test_add_months():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("'2000-02-01'.AddMonths(1).Month()", 0)
    assert dt == 3


def test_add_days():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("'2000-02-01'.AddDays(1).Day()", 0)
    assert dt == 2


def test_add_hours():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("'2000-02-01 12:05:06'.AddHours(1).Hour()", 0)
    assert dt == 13


def test_add_minutes():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("'2000-02-01 12:05:06'.AddMinutes(1).Minute()", 0)
    assert dt == 6


def test_add_seconds():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("'2000-02-01 12:05:06'.AddSeconds(1).Second()", 0)
    assert dt == 7

    dt = engine.try_evaluate("'2000-02-01 10:05:06'.AddSeconds(1).Second()", 0)
    assert dt == 7

    dt = engine.try_evaluate("'2000-02-01 20:05:06'.AddSeconds(1).Second()", 0)
    assert dt == 7

    dt = engine.try_evaluate("'2000-02-01 9:05:06'.AddSeconds(1).Second()", 0)
    assert dt == 7

    dt = engine.try_evaluate("'7:05:06'.AddSeconds(1).Second()", 0)
    assert dt == 7

    dt = engine.try_evaluate("'10:05:06'.AddSeconds(1).Second()", 0)
    assert dt == 7

    dt = engine.try_evaluate("'20:05:06'.AddSeconds(1).Second()", 0)
    assert dt == 7

    dt = engine.try_evaluate("'2000-02-01 24:05:06'.AddSeconds(1).Second()", 0)
    assert dt == 0


# 方法式调用测试


def test_method_style_datevalue():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_my_date("'2016-01-01'.DATEVALUE()", MyDate(1900, 1, 1, 0, 0, 0))
    assert dt.Year == 2016
    assert dt.Month == 1
    assert dt.Day == 1


def test_method_style_timevalue():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_my_date("'12:12:12'.TIMEVALUE()", MyDate(0, 0, 0, 0, 0, 0))
    assert dt.Hour == 12
    assert dt.Minute == 12
    assert dt.Second == 12


def test_method_style_year():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("'2016-05-15'.YEAR()", 0)
    assert dt == 2016


def test_method_style_month():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("'2016-05-15'.MONTH()", 0)
    assert dt == 5


def test_method_style_day():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("'2016-05-15'.DAY()", 0)
    assert dt == 15


def test_method_style_hour():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("'12:30:45'.HOUR()", 0)
    assert dt == 12


def test_method_style_minute():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("'12:30:45'.MINUTE()", 0)
    assert dt == 30


def test_method_style_second():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("'12:30:45'.SECOND()", 0)
    assert dt == 45


def test_method_style_addyears():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("'2000-01-01'.ADDYEARS(1).YEAR()", 0)
    assert dt == 2001


def test_method_style_addmonths():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("'2000-01-15'.ADDMONTHS(2).MONTH()", 0)
    assert dt == 3


def test_method_style_adddays():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("'2000-01-01'.ADDDAYS(10).DAY()", 0)
    assert dt == 11


def test_method_style_addhours():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("'2000-01-01 10:00:00'.ADDHOURS(5).HOUR()", 0)
    assert dt == 15


def test_method_style_addminutes():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("'2000-01-01 10:30:00'.ADDMINUTES(45).MINUTE()", 0)
    assert dt == 15


def test_method_style_addseconds():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("'2000-01-01 10:00:30'.ADDSECONDS(45).SECOND()", 0)
    assert dt == 15


def test_method_style_timestamp():
    engine = AlgorithmEngine()
    engine.UseLocalTime = True
    dt = engine.try_evaluate("'2016-01-01'.TIMESTAMP()", 0)
    assert dt == 1451577600000

    dt = engine.try_evaluate("'2016-01-01'.TIMESTAMP(1)", 0)
    assert dt == 1451577600


# 边界值测试


def test_leap_year():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_my_date("DATE(2020,2,29)", MyDate(1900, 1, 1, 0, 0, 0))
    assert dt.Year == 2020
    assert dt.Month == 2
    assert dt.Day == 29

    dt = engine.try_evaluate_my_date("DATE(2024,2,29)", MyDate(1900, 1, 1, 0, 0, 0))
    assert dt.Year == 2024
    assert dt.Month == 2
    assert dt.Day == 29

    dt = engine.try_evaluate_my_date("DATE(2019,2,28)", MyDate(1900, 1, 1, 0, 0, 0))
    assert dt.Year == 2019
    assert dt.Month == 2
    assert dt.Day == 28

    year = engine.try_evaluate("'2020-02-29'.YEAR()", 0)
    assert year == 2020
    month = engine.try_evaluate("'2020-02-29'.MONTH()", 0)
    assert month == 2
    day = engine.try_evaluate("'2020-02-29'.DAY()", 0)
    assert day == 29


def test_leap_year_add_days():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("'2020-02-28'.ADDDAYS(1).DAY()", 0)
    assert dt == 29

    dt = engine.try_evaluate("'2020-03-01'.ADDDAYS(-1).DAY()", 0)
    assert dt == 29


def test_month_boundary():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("DATE(2020,1,31).ADDMONTHS(1).MONTH()", 0)
    assert dt == 2

    dt = engine.try_evaluate("DATE(2020,3,31).ADDMONTHS(-1).MONTH()", 0)
    assert dt == 2


def test_year_boundary():
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("DATE(2020,12,31).ADDDAYS(1).YEAR()", 0)
    assert dt == 2021
    dt = engine.try_evaluate("DATE(2020,12,31).ADDDAYS(1).MONTH()", 0)
    assert dt == 1
    dt = engine.try_evaluate("DATE(2020,12,31).ADDDAYS(1).DAY()", 0)
    assert dt == 1

    dt = engine.try_evaluate("DATE(2021,1,1).ADDDAYS(-1).YEAR()", 0)
    assert dt == 2020
    dt = engine.try_evaluate("DATE(2021,1,1).ADDDAYS(-1).MONTH()", 0)
    assert dt == 12
    dt = engine.try_evaluate("DATE(2021,1,1).ADDDAYS(-1).DAY()", 0)
    assert dt == 31
