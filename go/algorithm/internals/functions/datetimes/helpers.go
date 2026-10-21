package datetimes

import (
	"strings"
	"time"
)

// 本文件是日期函数的公共辅助方法，对应 C# 中散落在各函数里的
// DateTime / MyDate 边界处理（越界检查、OADate 转换、整数截断等）。

var (
	decMinInt32 = MustDecimal("-2147483648")
	decMaxInt32 = MustDecimal("2147483647")
	decMinInt64 = MustDecimal("-9223372036854775808")
	decMaxInt64 = MustDecimal("9223372036854775807")

	// epoch1899 DateTime.FromOADate 的基准日（1899-12-30）。
	epoch1899 = time.Date(1899, 12, 30, 0, 0, 0, 0, time.UTC)
)

const (
	millisPerDay = 86400000
)

// intValue 等价于 C# 的 decimal -> int 转换：先向零截断，再做范围检查。
func intValue(d Decimal) (int, bool) {
	t := d.Trunc()
	if t.Less(decMinInt32) || t.Greater(decMaxInt32) {
		return 0, false
	}
	return int(t.Int64()), true
}

// int64Value 等价于 C# 的 decimal -> long 转换（越界返回 false）。
func int64Value(d Decimal) (int64, bool) {
	t := d.Trunc()
	if t.Less(decMinInt64) || t.Greater(decMaxInt64) {
		return 0, false
	}
	return t.Int64(), true
}

// daysInMonth 某年某月的天数（等价 C# DateTime.DaysInMonth）。
func daysInMonth(year, month int) int {
	return time.Date(year, time.Month(month)+1, 1, 0, 0, 0, 0, time.UTC).AddDate(0, 0, -1).Day()
}

// validDate year 是否落在 DateTime 的合法范围内。
func validDate(t time.Time) bool {
	return t.Year() >= 1 && t.Year() <= 9999
}

// dateOnly 去掉时间分量（等价 C# DateTime.Date）。
func dateOnly(t time.Time) time.Time {
	return time.Date(t.Year(), t.Month(), t.Day(), 0, 0, 0, 0, time.UTC)
}

// wholeDays 两个时间之间的整天数，向零截断（等价 (a-b).Days）。
func wholeDays(start, end time.Time) int {
	return int(end.Sub(start) / (24 * time.Hour))
}

// addDaysChecked 加天数；结果超出 DateTime 范围时返回 false
// （对应 C# 抛 ArgumentOutOfRangeException / OverflowException）。
func addDaysChecked(t time.Time, n int) (time.Time, bool) {
	r := t.AddDate(0, 0, n)
	if !validDate(r) {
		return t, false
	}
	return r, true
}

// fromOADate 等价 C# DateTime.FromOADate：以 1899-12-30 为 0 的日数，
// 毫秒四舍五入（远离零）；超出 DateTime 范围返回 false。
func fromOADate(v Decimal) (*MyDate, bool) {
	ms := v.Mul(DecFromInt(millisPerDay)).RoundHalfUp(0)
	if ms.Less(decMinInt64) || ms.Greater(decMaxInt64) {
		return nil, false
	}
	total := ms.Int64()
	days := total / millisPerDay
	rem := total % millisPerDay
	if rem < 0 {
		days--
		rem += millisPerDay
	}
	base := epoch1899.AddDate(0, 0, int(days))
	if !validDate(base) {
		return nil, false
	}
	t := base.Add(time.Duration(rem) * time.Millisecond)
	if !validDate(t) {
		return nil, false
	}
	return NewMyDateFromTime(t), true
}

// myDateFromUnixMilli 由 Unix 毫秒时间戳创建 MyDate（UseLocalTime 时转本地时间）。
func myDateFromUnixMilli(ms int64, useLocalTime bool) (*MyDate, bool) {
	t := time.UnixMilli(ms).UTC()
	if !validDate(t) {
		return nil, false
	}
	if useLocalTime {
		t = t.Local()
	}
	return NewMyDateFromTime(t), true
}

// parseTimeSpan 解析 TimeSpan.TryParse 支持的常见格式：
// [-]d.hh:mm:ss[.fffffff]、[-]hh:mm[:ss[.fffffff]]。
// 纯时间串返回「日 + 时间」型 MyDate（年月日为空）。
func parseTimeSpan(txt string) (*MyDate, bool) {
	s := strings.TrimSpace(txt)
	if s == "" {
		return nil, false
	}
	negative := false
	if s[0] == '-' {
		negative = true
		s = s[1:]
	} else if s[0] == '+' {
		s = s[1:]
	}
	colon := strings.IndexByte(s, ':')
	if colon < 0 {
		// TimeSpan.Parse 允许纯数字，按「天」解释
		d, ok := TryParseDecimal(s)
		if !ok || !d.IsInteger() {
			return nil, false
		}
		days := int(d.Int64())
		if negative {
			days = -days
		}
		return NewMyDate(0, 0, days, 0, 0, 0), true
	}
	days := 0
	if idx := strings.IndexByte(s, '.'); idx > 0 && idx < colon {
		d, ok := TryParseDecimal(s[:idx])
		if !ok || !d.IsInteger() {
			return nil, false
		}
		days = int(d.Int64())
		s = s[idx+1:]
	}
	segs := strings.Split(s, ":")
	if len(segs) != 2 && len(segs) != 3 {
		return nil, false
	}
	h, ok1 := TryParseDecimal(segs[0])
	m, ok2 := TryParseDecimal(segs[1])
	if !ok1 || !ok2 || !h.IsInteger() || !m.IsInteger() {
		return nil, false
	}
	sec := DecimalZero
	if len(segs) == 3 {
		// 允许小数秒，按 TimeSpan 规则保留到秒（MyDate 只有秒精度）
		secText := segs[2]
		if idx := strings.IndexByte(secText, '.'); idx >= 0 {
			secText = secText[:idx]
		}
		var ok3 bool
		sec, ok3 = TryParseDecimal(secText)
		if !ok3 || !sec.IsInteger() {
			return nil, false
		}
	}
	hi, mi, si := int(h.Int64()), int(m.Int64()), int(sec.Int64())
	if hi < 0 || hi > 23 || mi < 0 || mi > 59 || si < 0 || si > 59 {
		return nil, false
	}
	if negative {
		days, hi, mi, si = -days, -hi, -mi, -si
	}
	return NewMyDate(0, 0, days, hi, mi, si), true
}
