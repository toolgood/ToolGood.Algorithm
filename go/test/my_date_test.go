package test

import (
	"testing"
	"time"

	alg "github.com/toolgood/ToolGood.Algorithm/go/algorithm"
)

// TestMyDateTest 移植自
// csharp/ToolGood.Algorithm.Test/Helpers/MyDateTest.cs。
func TestMyDateTest(t *testing.T) {
	t.Run("Constructor_DateTime_Test", func(t *testing.T) {
		dt := time.Date(2024, 6, 15, 10, 30, 45, 0, time.UTC)
		myDate := alg.NewMyDateFromTime(dt)

		assertInt(t, 2024, myDate.Year)
		assertInt(t, 6, myDate.Month)
		assertInt(t, 15, myDate.Day)
		assertInt(t, 10, myDate.Hour)
		assertInt(t, 30, myDate.Minute)
		assertInt(t, 45, myDate.Second)
	})

	t.Run("Constructor_TimeSpan_Test", func(t *testing.T) {
		// 差异：Go 无 TimeSpan 类型，用「只有时间分量」的 MyDate 表示；
		// C# 的 Year / Month 为 null，Go 中以 0 表示未设置。
		myDate := alg.NewMyDate(0, 0, 1, 10, 30, 45)

		assertInt(t, 0, myDate.Year)
		assertInt(t, 0, myDate.Month)
		assertInt(t, 1, myDate.Day)
		assertInt(t, 10, myDate.Hour)
		assertInt(t, 30, myDate.Minute)
		assertInt(t, 45, myDate.Second)
	})

	t.Run("Constructor_Params_Test", func(t *testing.T) {
		myDate := alg.NewMyDate(2024, 6, 15, 10, 30, 45)

		assertInt(t, 2024, myDate.Year)
		assertInt(t, 6, myDate.Month)
		assertInt(t, 15, myDate.Day)
		assertInt(t, 10, myDate.Hour)
		assertInt(t, 30, myDate.Minute)
		assertInt(t, 45, myDate.Second)
	})

	t.Run("Parse_DateTime_Test", func(t *testing.T) {
		myDate := alg.ParseMyDate("2024-06-15 10:30:45")
		assertNotNull(t, myDate)
		assertInt(t, 2024, myDate.Year)
		assertInt(t, 6, myDate.Month)
		assertInt(t, 15, myDate.Day)
		assertInt(t, 10, myDate.Hour)
		assertInt(t, 30, myDate.Minute)
		assertInt(t, 45, myDate.Second)
	})

	t.Run("Parse_DateTime_Slash_Test", func(t *testing.T) {
		myDate := alg.ParseMyDate("2024/06/15 10:30:45")
		assertNotNull(t, myDate)
		assertInt(t, 2024, myDate.Year)
		assertInt(t, 6, myDate.Month)
		assertInt(t, 15, myDate.Day)
		assertInt(t, 10, myDate.Hour)
		assertInt(t, 30, myDate.Minute)
		assertInt(t, 45, myDate.Second)
	})

	t.Run("Parse_Date_Test", func(t *testing.T) {
		myDate := alg.ParseMyDate("2024-06-15")
		assertNotNull(t, myDate)
		assertInt(t, 2024, myDate.Year)
		assertInt(t, 6, myDate.Month)
		assertInt(t, 15, myDate.Day)
		assertInt(t, 0, myDate.Hour)
		assertInt(t, 0, myDate.Minute)
		assertInt(t, 0, myDate.Second)
	})

	t.Run("Parse_Time_Test", func(t *testing.T) {
		myDate := alg.ParseMyDate("10:30:45")
		assertNotNull(t, myDate)
		assertInt(t, 0, myDate.Year)
		assertInt(t, 0, myDate.Month)
		assertInt(t, 0, myDate.Day)
		assertInt(t, 10, myDate.Hour)
		assertInt(t, 30, myDate.Minute)
		assertInt(t, 45, myDate.Second)
	})

	t.Run("Parse_Time_NoSeconds_Test", func(t *testing.T) {
		myDate := alg.ParseMyDate("10:30")
		assertNotNull(t, myDate)
		assertInt(t, 0, myDate.Year)
		assertInt(t, 0, myDate.Month)
		assertInt(t, 0, myDate.Day)
		assertInt(t, 10, myDate.Hour)
		assertInt(t, 30, myDate.Minute)
		assertInt(t, 0, myDate.Second)
	})

	t.Run("Parse_Invalid_Test", func(t *testing.T) {
		// 注意：*MyDate 为 typed nil，直接传给 assertNull 会得到非 nil 接口，故用 == nil 判断。
		assertTrue(t, alg.ParseMyDate("invalid date") == nil, "无效日期应返回 nil")
	})

	t.Run("ToDateTime_Test", func(t *testing.T) {
		myDate := alg.NewMyDate(2024, 6, 15, 10, 30, 45)
		dt := myDate.ToDateTime()

		assertInt(t, 2024, dt.Year())
		assertInt(t, 6, int(dt.Month()))
		assertInt(t, 15, dt.Day())
		assertInt(t, 10, dt.Hour())
		assertInt(t, 30, dt.Minute())
		assertInt(t, 45, dt.Second())
		// 差异：Go 的 time.Time 用 Location 表示时区，ToDateTime 固定返回 UTC。
		assertTrue(t, dt.Location() == time.UTC, "Kind 应为 UTC")
	})

	t.Run("ToDateTime_Kind_Test", func(t *testing.T) {
		myDate := alg.NewMyDate(2024, 6, 15, 10, 30, 45)
		dt := myDate.ToDateTime()
		// 差异：Go 版 ToDateTime 没有 DateTimeKind 参数，恒为 UTC。
		assertTrue(t, dt.Location() == time.UTC, "Kind 应为 UTC")
	})

	t.Run("ToTimeSpan_Test", func(t *testing.T) {
		// 差异：Go 无 TimeSpan 类型，直接断言 MyDate 的日 / 时 / 分 / 秒分量。
		myDate := alg.NewMyDate(0, 0, 1, 10, 30, 45)

		assertInt(t, 1, myDate.Day)
		assertInt(t, 10, myDate.Hour)
		assertInt(t, 30, myDate.Minute)
		assertInt(t, 45, myDate.Second)
	})

	t.Run("AddYears_Test", func(t *testing.T) {
		myDate := alg.NewMyDate(2024, 6, 15, 10, 30, 45)
		result := myDate.AddYears(1)

		assertInt(t, 2025, result.Year)
		assertInt(t, 6, result.Month)
		assertInt(t, 15, result.Day)
	})

	t.Run("AddYears_Negative_Test", func(t *testing.T) {
		myDate := alg.NewMyDate(2024, 6, 15, 10, 30, 45)
		result := myDate.AddYears(-1)

		assertInt(t, 2023, result.Year)
	})

	t.Run("AddMonths_Test", func(t *testing.T) {
		myDate := alg.NewMyDate(2024, 6, 15, 10, 30, 45)
		result := myDate.AddMonths(1)

		assertInt(t, 7, result.Month)
	})

	t.Run("AddMonths_Overflow_Test", func(t *testing.T) {
		myDate := alg.NewMyDate(2024, 11, 15, 10, 30, 45)
		result := myDate.AddMonths(2)

		assertInt(t, 2025, result.Year)
		assertInt(t, 1, result.Month)
	})

	t.Run("AddDays_Test", func(t *testing.T) {
		myDate := alg.NewMyDate(2024, 6, 15, 10, 30, 45)
		result := myDate.AddDays(5)

		assertInt(t, 20, result.Day)
	})

	t.Run("AddDays_Negative_Test", func(t *testing.T) {
		myDate := alg.NewMyDate(2024, 6, 15, 10, 30, 45)
		result := myDate.AddDays(-5)

		assertInt(t, 10, result.Day)
	})

	t.Run("AddHours_Test", func(t *testing.T) {
		myDate := alg.NewMyDate(2024, 6, 15, 10, 30, 45)
		result := myDate.AddHours(5)

		assertInt(t, 15, result.Hour)
	})

	t.Run("AddHours_NoOverflow_Test", func(t *testing.T) {
		myDate := alg.NewMyDate(2024, 6, 15, 10, 30, 45)
		result := myDate.AddHours(2)

		assertInt(t, 12, result.Hour)
	})

	t.Run("AddMinutes_Test", func(t *testing.T) {
		myDate := alg.NewMyDate(2024, 6, 15, 10, 30, 45)
		result := myDate.AddMinutes(15)

		assertInt(t, 45, result.Minute)
	})

	t.Run("AddMinutes_NoOverflow_Test", func(t *testing.T) {
		myDate := alg.NewMyDate(2024, 6, 15, 10, 30, 45)
		result := myDate.AddMinutes(10)

		assertInt(t, 40, result.Minute)
	})

	t.Run("AddSeconds_Test", func(t *testing.T) {
		myDate := alg.NewMyDate(2024, 6, 15, 10, 30, 45)
		result := myDate.AddSeconds(10)

		assertInt(t, 55, result.Second)
	})

	t.Run("AddSeconds_NoOverflow_Test", func(t *testing.T) {
		myDate := alg.NewMyDate(2024, 6, 15, 10, 30, 45)
		result := myDate.AddSeconds(5)

		assertInt(t, 50, result.Second)
	})

	t.Run("ToString_DateTime_Test", func(t *testing.T) {
		myDate := alg.NewMyDate(2024, 6, 15, 10, 30, 45)
		str := myDate.String()

		assertContains(t, str, "2024")
		assertContains(t, str, "06")
		assertContains(t, str, "15")
	})

	t.Run("ToString_TimeOnly_Test", func(t *testing.T) {
		myDate := alg.NewMyDate(0, 0, 0, 10, 30, 45)
		str := myDate.String()

		assertContains(t, str, "10:30:45")
	})

	t.Run("Implicit_DateTime_To_MyDate_Test", func(t *testing.T) {
		// 差异：Go 无隐式转换，直接使用 NewMyDateFromTime。
		dt := time.Date(2024, 6, 15, 10, 30, 45, 0, time.UTC)
		myDate := alg.NewMyDateFromTime(dt)

		assertInt(t, 2024, myDate.Year)
		assertInt(t, 6, myDate.Month)
		assertInt(t, 15, myDate.Day)
	})

	t.Run("Implicit_MyDate_To_DateTime_Test", func(t *testing.T) {
		// 差异：Go 无隐式转换，直接调用 ToDateTime。
		myDate := alg.NewMyDate(2024, 6, 15, 10, 30, 45)
		dt := myDate.ToDateTime()

		assertInt(t, 2024, dt.Year())
		assertInt(t, 6, int(dt.Month()))
		assertInt(t, 15, dt.Day())
	})

	t.Run("Implicit_MyDate_To_TimeSpan_Test", func(t *testing.T) {
		// 差异：Go 无 TimeSpan 类型，直接断言 MyDate 分量。
		myDate := alg.NewMyDate(0, 0, 1, 10, 30, 45)

		assertInt(t, 1, myDate.Day)
		assertInt(t, 10, myDate.Hour)
		assertInt(t, 30, myDate.Minute)
		assertInt(t, 45, myDate.Second)
	})
}
