package test

import (
	"testing"
	"time"

	alg "github.com/toolgood/ToolGood.Algorithm/go/algorithm"
)

// assertDateParts 逐分量比较日期（对应 C# 的 DateTime 相等断言）。
func assertDateParts(t *testing.T, got *alg.MyDate, y, m, d, h, mi, s int) {
	t.Helper()
	if got == nil {
		t.Fatalf("want %04d-%02d-%02d %02d:%02d:%02d, got nil", y, m, d, h, mi, s)
	}
	if got.Year != y || got.Month != m || got.Day != d ||
		got.Hour != h || got.Minute != mi || got.Second != s {
		t.Fatalf("want %04d-%02d-%02d %02d:%02d:%02d, got %s",
			y, m, d, h, mi, s, got.String())
	}
}

// TestDateTimesTest 对应 csharp/ToolGood.Algorithm.Test/AlgorithmEngine/DateTimesTest.cs。
func TestDateTimesTest(t *testing.T) {
	t.Run("DATEVALUE_Test", func(t *testing.T) {
		engine := newEngine()
		dt := evalDate(engine, "DATEVALUE('2016-01-01')")
		assertDateParts(t, dt, 2016, 1, 1, 0, 0, 0)
		dt = evalDate(engine, "DATEVALUE（'2016-01-01'）")
		assertDateParts(t, dt, 2016, 1, 1, 0, 0, 0)

		dt = evalDate(engine, "DATEVALUE('2016/01/01')")
		assertDateParts(t, dt, 2016, 1, 1, 0, 0, 0)

		// 差异：C# 断言的是 dt.ToLocalTime()，此处按本机时区（UTC+8）的本地时间断言。
		// Go 默认 UseLocalTime = true，与 C# 一致。
		dt = evalDate(engine, "DATEVALUE('1691234899000',0)")
		assertDateParts(t, dt, 2023, 8, 5, 19, 28, 19)

		dt = evalDate(engine, "DATEVALUE('1691234899',0)")
		assertDateParts(t, dt, 2023, 8, 5, 19, 28, 19)

		engine.UseLocalTime = true
		dt = evalDate(engine, "DATEVALUE('1691234899000',0)")
		assertDateParts(t, dt, 2023, 8, 5, 19, 28, 19)

		dt = evalDate(engine, "DATEVALUE('1691234899',0)")
		assertDateParts(t, dt, 2023, 8, 5, 19, 28, 19)

		dt = evalDate(engine, "DATEVALUE('1691234899000')")
		assertDateParts(t, dt, 2023, 8, 5, 19, 28, 19)

		dt = evalDate(engine, "DATEVALUE('1691234899')")
		assertDateParts(t, dt, 2023, 8, 5, 19, 28, 19)
	})

	t.Run("TIMESTAMP_Test", func(t *testing.T) {
		engine := newEngine()
		// 按本机时区（UTC+8）断言：本地 2016-01-01 00:00 对应 1451577600000。
		engine.UseLocalTime = true

		assertInt(t, 1451577600000, evalInt(engine, "TIMESTAMP('2016-01-01')"))
		assertInt(t, 1451577600000, evalInt(engine, "TIMESTAMP('2016/01/01')"))
		assertInt(t, 1451577600000, evalInt(engine, "TIMESTAMP('2016-01-01',0)"))
		assertInt(t, 1451577600, evalInt(engine, "TIMESTAMP('2016-01-01',1)"))
	})

	t.Run("TIMEVALUE_test", func(t *testing.T) {
		engine := newEngine()
		dt := evalDate(engine, "TIMEVALUE('12:12:12')")
		assertDateParts(t, dt, 0, 0, 0, 12, 12, 12)
	})

	t.Run("DATE_test", func(t *testing.T) {
		engine := newEngine()
		dt := evalDate(engine, "DATE(2016,1,1)")
		assertDateParts(t, dt, 2016, 1, 1, 0, 0, 0)
	})

	t.Run("time_test", func(t *testing.T) {
		engine := newEngine()
		dt := evalDate(engine, "time(11,12,13)")
		assertDateParts(t, dt, 0, 0, 0, 11, 12, 13)
	})

	t.Run("Now_test", func(t *testing.T) {
		engine := newEngine()
		dt := evalDate(engine, "now()")
		now := time.Now()
		assertDateParts(t, dt, now.Year(), int(now.Month()), now.Day(),
			now.Hour(), now.Minute(), now.Second())
	})

	t.Run("Today_test", func(t *testing.T) {
		engine := newEngine()
		dt := evalDate(engine, "Today()")
		now := time.Now()
		assertDateParts(t, dt, now.Year(), int(now.Month()), now.Day(), 0, 0, 0)
	})

	t.Run("Year_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, time.Now().Year(), evalInt(engine, "year(now())"))
	})

	t.Run("Month_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, int(time.Now().Month()), evalInt(engine, "Month(now())"))
	})

	t.Run("Day_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, time.Now().Day(), evalInt(engine, "Day(now())"))
	})

	t.Run("Hour_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, time.Now().Hour(), evalInt(engine, "Hour(now())"))
	})

	t.Run("Minute_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, time.Now().Minute(), evalInt(engine, "Minute(now())"))
	})

	t.Run("Second_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, time.Now().Second(), evalInt(engine, "Second(now())"))
	})

	t.Run("WEEKDAY_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 7, evalInt(engine, "WEEKDAY(date(2017,1,7))"))
		assertInt(t, 7, evalInt(engine, "WEEKDAY(date(2017,1,7),1)"))
		assertInt(t, 6, evalInt(engine, "WEEKDAY(date(2017,1,7),2)"))
		assertInt(t, 5, evalInt(engine, "WEEKDAY(date(2017,1,7),3)"))

		assertInt(t, 1, evalInt(engine, "WEEKDAY(date(2017,1,8),1)"))
		assertInt(t, 7, evalInt(engine, "WEEKDAY(date(2017,1,8),2)"))
		assertInt(t, 6, evalInt(engine, "WEEKDAY(date(2017,1,8),3)"))

		assertInt(t, 2, evalInt(engine, "WEEKDAY(date(2017,1,2),1)"))
		assertInt(t, 1, evalInt(engine, "WEEKDAY(date(2017,1,2),2)"))
		assertInt(t, 0, evalInt(engine, "WEEKDAY(date(2017,1,2),3)"))
	})

	t.Run("WEEKDAY_ExcelCompatible_Test", func(t *testing.T) {
		engine := newEngine()

		// 2017-1-2 (周一)
		assertInt(t, 2, evalInt(engine, "WEEKDAY(date(2017,1,2),1)"))
		assertInt(t, 1, evalInt(engine, "WEEKDAY(date(2017,1,2),2)"))
		assertInt(t, 0, evalInt(engine, "WEEKDAY(date(2017,1,2),3)"))
		assertInt(t, 1, evalInt(engine, "WEEKDAY(date(2017,1,2),11)"))
		assertInt(t, 7, evalInt(engine, "WEEKDAY(date(2017,1,2),12)"))
		assertInt(t, 6, evalInt(engine, "WEEKDAY(date(2017,1,2),13)"))
		assertInt(t, 5, evalInt(engine, "WEEKDAY(date(2017,1,2),14)"))
		assertInt(t, 4, evalInt(engine, "WEEKDAY(date(2017,1,2),15)"))
		assertInt(t, 3, evalInt(engine, "WEEKDAY(date(2017,1,2),16)"))
		assertInt(t, 2, evalInt(engine, "WEEKDAY(date(2017,1,2),17)"))

		// 2017-1-7 (周六)
		assertInt(t, 7, evalInt(engine, "WEEKDAY(date(2017,1,7),1)"))
		assertInt(t, 6, evalInt(engine, "WEEKDAY(date(2017,1,7),2)"))
		assertInt(t, 5, evalInt(engine, "WEEKDAY(date(2017,1,7),3)"))
		assertInt(t, 6, evalInt(engine, "WEEKDAY(date(2017,1,7),11)"))
		assertInt(t, 5, evalInt(engine, "WEEKDAY(date(2017,1,7),12)"))
		assertInt(t, 4, evalInt(engine, "WEEKDAY(date(2017,1,7),13)"))
		assertInt(t, 3, evalInt(engine, "WEEKDAY(date(2017,1,7),14)"))
		assertInt(t, 2, evalInt(engine, "WEEKDAY(date(2017,1,7),15)"))
		assertInt(t, 1, evalInt(engine, "WEEKDAY(date(2017,1,7),16)"))
		assertInt(t, 7, evalInt(engine, "WEEKDAY(date(2017,1,7),17)"))

		// 2017-1-8 (周日)
		assertInt(t, 1, evalInt(engine, "WEEKDAY(date(2017,1,8),1)"))
		assertInt(t, 7, evalInt(engine, "WEEKDAY(date(2017,1,8),2)"))
		assertInt(t, 6, evalInt(engine, "WEEKDAY(date(2017,1,8),3)"))
		assertInt(t, 7, evalInt(engine, "WEEKDAY(date(2017,1,8),11)"))
		assertInt(t, 6, evalInt(engine, "WEEKDAY(date(2017,1,8),12)"))
		assertInt(t, 5, evalInt(engine, "WEEKDAY(date(2017,1,8),13)"))
		assertInt(t, 4, evalInt(engine, "WEEKDAY(date(2017,1,8),14)"))
		assertInt(t, 3, evalInt(engine, "WEEKDAY(date(2017,1,8),15)"))
		assertInt(t, 2, evalInt(engine, "WEEKDAY(date(2017,1,8),16)"))
		assertInt(t, 1, evalInt(engine, "WEEKDAY(date(2017,1,8),17)"))
	})

	t.Run("DATEDIF_Test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 41, evalInt(engine, "DATEDIF('1975-1-30','2017-1-7','y')"))
		assertInt(t, 503, evalInt(engine, "DATEDIF('1975-1-30','2017-1-7','m')"))
		assertInt(t, 15318, evalInt(engine, "DATEDIF('1975-1-30','2017-1-7','d')"))
		assertInt(t, 342, evalInt(engine, "DATEDIF('1975-1-30','2017-1-7','yd')"))
		assertInt(t, 11, evalInt(engine, "DATEDIF('1975-1-30','2017-1-7','ym')"))
		assertInt(t, 8, evalInt(engine, "DATEDIF('1975-1-30','2017-1-7','md')"))
	})

	t.Run("DAYS_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 15318, evalInt(engine, "DAYS('2017-1-7','1975-1-30')"))
		assertInt(t, 2, evalInt(engine, "DAYS('2016-3-1','2016-2-28')"))
		assertInt(t, -2, evalInt(engine, "DAYS('2016-2-28','2016-3-1')"))
	})

	t.Run("DAYS360_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 15097, evalInt(engine, "DAYS360('1975-1-30','2017-1-7')"))
	})

	t.Run("DATEDIF_YD_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 92, evalInt(engine, "DATEDIF('2015-6-15','2016-9-15','yd')"))
		assertInt(t, 0, evalInt(engine, "DATEDIF('2015-3-1','2016-3-1','yd')"))
		assertInt(t, 1, evalInt(engine, "DATEDIF('2015-3-1','2016-3-2','yd')"))
	})

	t.Run("DAYS360_us_method_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 30, evalInt(engine, "DAYS360('2016-2-29','2016-3-31')"))
		assertInt(t, 30, evalInt(engine, "DAYS360('2016-2-29','2016-3-31',false)"))
		assertInt(t, 30, evalInt(engine, "DAYS360('2016-2-29','2016-3-31',false)"))
	})

	t.Run("DAYS360_europe_method_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 5, evalInt(engine, "DAYS360('2016-1-15','2016-1-20',true)"))
		assertInt(t, 1, evalInt(engine, "DAYS360('2016-1-31','2016-2-1',true)"))
		assertInt(t, 60, evalInt(engine, "DAYS360('2016-1-31','2016-3-31',true)"))
		assertInt(t, 15, evalInt(engine, "DAYS360('2016-1-15','2016-1-31',true)"))
	})

	t.Run("EDATE_test", func(t *testing.T) {
		engine := newEngine()
		dt := evalDate(engine, "EDATE(\"2012-1-31\",32)")
		assertDateParts(t, dt, 2014, 9, 30, 0, 0, 0)
	})

	t.Run("EOMONTH_test", func(t *testing.T) {
		engine := newEngine()
		dt := evalDate(engine, "EOMONTH(\"2012-2-1\",32)")
		assertDateParts(t, dt, 2014, 10, 31, 0, 0, 0)
	})

	t.Run("NETWORKDAYS_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 262, evalInt(engine, "NETWORKDAYS(\"2012-1-1\",\"2013-1-1\")"))
	})

	t.Run("NETWORKDAYS_holidays_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 7, evalInt(engine, "NETWORKDAYS('2024-1-1','2024-1-10','2024-1-5')"))
		assertInt(t, 6, evalInt(engine, "NETWORKDAYS('2024-1-1','2024-1-10','2024-1-5','2024-1-8')"))
	})

	t.Run("WORKDAY_test", func(t *testing.T) {
		engine := newEngine()
		dt := evalDate(engine, "WORKDAY(\"2012-1-2\",145)")
		assertDateParts(t, dt, 2012, 7, 23, 0, 0, 0)
	})

	t.Run("WORKDAY_ExcelCompatible_test", func(t *testing.T) {
		engine := newEngine()
		dt := evalDate(engine, "WORKDAY(\"2024-01-15\", 5)")
		assertDateParts(t, dt, 2024, 1, 22, 0, 0, 0)

		dt = evalDate(engine, "WORKDAY(\"2024-01-15\", -5)")
		assertDateParts(t, dt, 2024, 1, 8, 0, 0, 0)

		dt = evalDate(engine, "WORKDAY(\"2024-01-15\", 0)")
		assertDateParts(t, dt, 2024, 1, 15, 0, 0, 0)

		dt = evalDate(engine, "WORKDAY(\"2024-01-12\", 1)")
		assertDateParts(t, dt, 2024, 1, 15, 0, 0, 0)

		dt = evalDate(engine, "WORKDAY(\"2024-01-15\", -1)")
		assertDateParts(t, dt, 2024, 1, 12, 0, 0, 0)
	})

	t.Run("WORKDAY_holidays_test", func(t *testing.T) {
		engine := newEngine()
		dt := evalDate(engine, "WORKDAY('2024-1-15', 5, '2024-1-17')")
		assertDateParts(t, dt, 2024, 1, 23, 0, 0, 0)

		dt = evalDate(engine, "WORKDAY('2024-1-15', 5, '2024-1-17', '2024-1-18')")
		assertDateParts(t, dt, 2024, 1, 24, 0, 0, 0)
	})

	t.Run("DATE_ExcelCompatible_test", func(t *testing.T) {
		engine := newEngine()
		dt := evalDate(engine, "DATE(2024, 1, 15)")
		assertDateParts(t, dt, 2024, 1, 15, 0, 0, 0)

		dt = evalDate(engine, "DATE(2024, 13, 1)")
		assertDateParts(t, dt, 2025, 1, 1, 0, 0, 0)

		dt = evalDate(engine, "DATE(2024, 1, 32)")
		assertDateParts(t, dt, 2024, 2, 1, 0, 0, 0)

		dt = evalDate(engine, "DATE(2024, 0, 1)")
		assertDateParts(t, dt, 2023, 12, 1, 0, 0, 0)

		dt = evalDate(engine, "DATE(2024, 2, 30)")
		assertDateParts(t, dt, 2024, 3, 1, 0, 0, 0)

		dt = evalDate(engine, "DATE(2024, -1, 1)")
		assertDateParts(t, dt, 2023, 11, 1, 0, 0, 0)

		dt = evalDate(engine, "DATE(2024, 12, 0)")
		assertDateParts(t, dt, 2024, 11, 30, 0, 0, 0)

		dt = evalDate(engine, "DATE(2024, 1, -1)")
		assertDateParts(t, dt, 2023, 12, 30, 0, 0, 0)
	})

	t.Run("WEEKNUM_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 2, evalInt(engine, "WEEKNUM(\"2016-1-3\")"))
		assertInt(t, 1, evalInt(engine, "WEEKNUM(\"2016-1-2\")"))

		assertInt(t, 2, evalInt(engine, "WEEKNUM(\"2016-1-4\",2)"))
		assertInt(t, 1, evalInt(engine, "WEEKNUM(\"2016-1-3\",2)"))
		assertInt(t, 1, evalInt(engine, "WEEKNUM(\"2016-1-2\",2)"))
	})

	t.Run("WEEKNUM_ExcelCompatible_Test", func(t *testing.T) {
		engine := newEngine()

		// 2016-1-1 (周五)
		assertInt(t, 1, evalInt(engine, "WEEKNUM(date(2016,1,1),1)"))
		assertInt(t, 1, evalInt(engine, "WEEKNUM(date(2016,1,1),2)"))
		assertInt(t, 1, evalInt(engine, "WEEKNUM(date(2016,1,1),11)"))
		assertInt(t, 1, evalInt(engine, "WEEKNUM(date(2016,1,1),12)"))
		assertInt(t, 1, evalInt(engine, "WEEKNUM(date(2016,1,1),13)"))
		assertInt(t, 1, evalInt(engine, "WEEKNUM(date(2016,1,1),14)"))
		assertInt(t, 1, evalInt(engine, "WEEKNUM(date(2016,1,1),15)"))
		assertInt(t, 1, evalInt(engine, "WEEKNUM(date(2016,1,1),16)"))
		assertInt(t, 1, evalInt(engine, "WEEKNUM(date(2016,1,1),17)"))

		// 2016-1-3 (周日)
		assertInt(t, 2, evalInt(engine, "WEEKNUM(date(2016,1,3),1)"))
		assertInt(t, 1, evalInt(engine, "WEEKNUM(date(2016,1,3),2)"))
		assertInt(t, 1, evalInt(engine, "WEEKNUM(date(2016,1,3),11)"))
		assertInt(t, 2, evalInt(engine, "WEEKNUM(date(2016,1,3),16)"))
		assertInt(t, 2, evalInt(engine, "WEEKNUM(date(2016,1,3),17)"))

		// 2016-1-4 (周一)
		assertInt(t, 2, evalInt(engine, "WEEKNUM(date(2016,1,4),1)"))
		assertInt(t, 2, evalInt(engine, "WEEKNUM(date(2016,1,4),2)"))
		assertInt(t, 2, evalInt(engine, "WEEKNUM(date(2016,1,4),11)"))
		assertInt(t, 1, evalInt(engine, "WEEKNUM(date(2016,1,4),21)"))

		// ISO 8601 (type 21)
		assertInt(t, 1, evalInt(engine, "WEEKNUM(date(2016,1,4),21)"))
		assertInt(t, 53, evalInt(engine, "WEEKNUM(date(2016,1,3),21)"))
		assertInt(t, 53, evalInt(engine, "WEEKNUM(date(2021,1,1),21)"))
		assertInt(t, 1, evalInt(engine, "WEEKNUM(date(2021,1,4),21)"))
	})

	t.Run("YEARFRAC_test", func(t *testing.T) {
		engine := newEngine()
		v := evalFloat(engine, "YEARFRAC('2012-1-1', '2012-7-1')")
		assertFloatRound(t, 0.5, v, 3)

		v = evalFloat(engine, "YEARFRAC('2012-1-1', '2013-1-1')")
		assertFloatRound(t, 1.0, v, 3)

		v = evalFloat(engine, "YEARFRAC('2012-1-1', '2012-7-1', 0)")
		assertFloatRound(t, 0.5, v, 3)

		v = evalFloat(engine, "YEARFRAC('2012-1-1', '2012-7-1', 1)")
		assertFloatRound(t, 0.49726776, v, 3)

		v = evalFloat(engine, "YEARFRAC('2012-1-1', '2012-7-1', 2)")
		assertFloatRound(t, 0.505555556, v, 3)

		v = evalFloat(engine, "YEARFRAC('2012-1-1', '2012-7-1', 3)")
		assertFloatRound(t, 0.498630137, v, 3)

		v = evalFloat(engine, "YEARFRAC('2012-1-1', '2012-7-1', 4)")
		assertFloatRound(t, 0.5, v, 3)
	})

	t.Run("YEARFRAC_basis0_30_360_test", func(t *testing.T) {
		engine := newEngine()
		v := evalFloat(engine, "YEARFRAC('2012-2-29','2012-3-31',0)")
		assertFloat(t, 31.0/360.0, v)

		v = evalFloat(engine, "YEARFRAC('2012-1-31','2012-3-31',0)")
		assertFloat(t, 60.0/360.0, v)

		v = evalFloat(engine, "YEARFRAC('2012-2-29','2013-2-28',0)")
		assertFloat(t, 360.0/360.0, v)

		v = evalFloat(engine, "YEARFRAC('2012-1-1','2012-7-1',0)")
		assertFloat(t, 0.5, v)
	})

	t.Run("AddYears_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 2001, evalInt(engine, "'2000-02-01'.addYears(1).year()"))
		assertInt(t, 2001, evalInt(engine, "'2000/02/01'.addYears(1).year()"))
	})

	t.Run("AddMonths_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 3, evalInt(engine, "'2000-02-01'.AddMonths(1).Month()"))
	})

	t.Run("AddDays_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 2, evalInt(engine, "'2000-02-01'.AddDays(1).Day()"))
	})

	t.Run("AddHours_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 13, evalInt(engine, "'2000-02-01 12:05:06'.AddHours(1).Hour()"))
	})

	t.Run("AddMinutes_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 6, evalInt(engine, "'2000-02-01 12:05:06'.AddMinutes(1).Minute()"))
	})

	t.Run("AddSeconds_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 7, evalInt(engine, "'2000-02-01 12:05:06'.AddSeconds(1).Second()"))
		assertInt(t, 7, evalInt(engine, "'2000-02-01 10:05:06'.AddSeconds(1).Second()"))
		assertInt(t, 7, evalInt(engine, "'2000-02-01 20:05:06'.AddSeconds(1).Second()"))
		assertInt(t, 7, evalInt(engine, "'2000-02-01 9:05:06'.AddSeconds(1).Second()"))
		assertInt(t, 7, evalInt(engine, "'7:05:06'.AddSeconds(1).Second()"))
		assertInt(t, 7, evalInt(engine, "'10:05:06'.AddSeconds(1).Second()"))
		assertInt(t, 7, evalInt(engine, "'20:05:06'.AddSeconds(1).Second()"))
		assertInt(t, 0, evalInt(engine, "'2000-02-01 24:05:06'.AddSeconds(1).Second()"))
	})

	t.Run("MethodStyle_DATEVALUE_test", func(t *testing.T) {
		engine := newEngine()
		dt := evalDate(engine, "'2016-01-01'.DATEVALUE()")
		assertDateParts(t, dt, 2016, 1, 1, 0, 0, 0)
	})

	t.Run("MethodStyle_TIMEVALUE_test", func(t *testing.T) {
		engine := newEngine()
		dt := evalDate(engine, "'12:12:12'.TIMEVALUE()")
		assertDateParts(t, dt, 0, 0, 0, 12, 12, 12)
	})

	t.Run("MethodStyle_YEAR_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 2016, evalInt(engine, "'2016-05-15'.YEAR()"))
	})

	t.Run("MethodStyle_MONTH_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 5, evalInt(engine, "'2016-05-15'.MONTH()"))
	})

	t.Run("MethodStyle_DAY_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 15, evalInt(engine, "'2016-05-15'.DAY()"))
	})

	t.Run("MethodStyle_HOUR_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 12, evalInt(engine, "'12:30:45'.HOUR()"))
	})

	t.Run("MethodStyle_MINUTE_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 30, evalInt(engine, "'12:30:45'.MINUTE()"))
	})

	t.Run("MethodStyle_SECOND_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 45, evalInt(engine, "'12:30:45'.SECOND()"))
	})

	t.Run("MethodStyle_ADDYEARS_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 2001, evalInt(engine, "'2000-01-01'.ADDYEARS(1).YEAR()"))
	})

	t.Run("MethodStyle_ADDMONTHS_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 3, evalInt(engine, "'2000-01-15'.ADDMONTHS(2).MONTH()"))
	})

	t.Run("MethodStyle_ADDDAYS_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 11, evalInt(engine, "'2000-01-01'.ADDDAYS(10).DAY()"))
	})

	t.Run("MethodStyle_ADDHOURS_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 15, evalInt(engine, "'2000-01-01 10:00:00'.ADDHOURS(5).HOUR()"))
	})

	t.Run("MethodStyle_ADDMINUTES_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 15, evalInt(engine, "'2000-01-01 10:30:00'.ADDMINUTES(45).MINUTE()"))
	})

	t.Run("MethodStyle_ADDSECONDS_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 15, evalInt(engine, "'2000-01-01 10:00:30'.ADDSECONDS(45).SECOND()"))
	})

	t.Run("MethodStyle_TIMESTAMP_test", func(t *testing.T) {
		engine := newEngine()
		// 差异：同 TIMESTAMP_Test，按本机时区（UTC+8）的本地时间断言。
		engine.UseLocalTime = true

		assertInt(t, 1451577600000, evalInt(engine, "'2016-01-01'.TIMESTAMP()"))
		assertInt(t, 1451577600, evalInt(engine, "'2016-01-01'.TIMESTAMP(1)"))
	})

	t.Run("LeapYear_Test", func(t *testing.T) {
		engine := newEngine()
		dt := evalDate(engine, "DATE(2020,2,29)")
		assertDateParts(t, dt, 2020, 2, 29, 0, 0, 0)

		dt = evalDate(engine, "DATE(2024,2,29)")
		assertDateParts(t, dt, 2024, 2, 29, 0, 0, 0)

		dt = evalDate(engine, "DATE(2019,2,28)")
		assertDateParts(t, dt, 2019, 2, 28, 0, 0, 0)

		assertInt(t, 2020, evalInt(engine, "'2020-02-29'.YEAR()"))
		assertInt(t, 2, evalInt(engine, "'2020-02-29'.MONTH()"))
		assertInt(t, 29, evalInt(engine, "'2020-02-29'.DAY()"))
	})

	t.Run("LeapYear_AddDays_Test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 29, evalInt(engine, "'2020-02-28'.ADDDAYS(1).DAY()"))
		assertInt(t, 29, evalInt(engine, "'2020-03-01'.ADDDAYS(-1).DAY()"))
	})

	t.Run("MonthBoundary_Test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 2, evalInt(engine, "DATE(2020,1,31).ADDMONTHS(1).MONTH()"))
		assertInt(t, 2, evalInt(engine, "DATE(2020,3,31).ADDMONTHS(-1).MONTH()"))
	})

	t.Run("YearBoundary_Test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 2021, evalInt(engine, "DATE(2020,12,31).ADDDAYS(1).YEAR()"))
		assertInt(t, 1, evalInt(engine, "DATE(2020,12,31).ADDDAYS(1).MONTH()"))
		assertInt(t, 1, evalInt(engine, "DATE(2020,12,31).ADDDAYS(1).DAY()"))

		assertInt(t, 2020, evalInt(engine, "DATE(2021,1,1).ADDDAYS(-1).YEAR()"))
		assertInt(t, 12, evalInt(engine, "DATE(2021,1,1).ADDDAYS(-1).MONTH()"))
		assertInt(t, 31, evalInt(engine, "DATE(2021,1,1).ADDDAYS(-1).DAY()"))
	})
}
