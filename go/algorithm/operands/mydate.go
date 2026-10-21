package operands

import (
	"fmt"
	"strings"
	"time"
)

// MyDate 日期 / 时间段混合值，对应 C# / Java 的 MyDate。
//
// Year / Month / Day 为 0 表示「未设置」，对应原实现的 null 语义。
type MyDate struct {
	Year   int
	Month  int
	Day    int
	Hour   int
	Minute int
	Second int
}

var monthDays = [13]int{0, 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31}

func isLeapYear(year int) bool {
	return year%4 == 0 && (year%100 != 0 || year%400 == 0)
}

func daysInMonth(year, month int) int {
	if month < 1 {
		month = 1
	}
	if month > 12 {
		month = 12
	}
	if month == 2 && !isLeapYear(year) {
		return 28
	}
	return monthDays[month]
}

func floorDivInt(a, b int) int {
	q := a / b
	if a%b != 0 && (a < 0) != (b < 0) {
		q--
	}
	return q
}

func floorModInt(a, b int) int { return a - floorDivInt(a, b)*b }

// NewMyDate 由年月日时分秒创建 MyDate。
func NewMyDate(year, month, day, hour, minute, second int) *MyDate {
	return &MyDate{Year: year, Month: month, Day: day, Hour: hour, Minute: minute, Second: second}
}

// NewMyDateFromTime 由 time.Time 创建 MyDate。
func NewMyDateFromTime(t time.Time) *MyDate {
	return &MyDate{
		Year: t.Year(), Month: int(t.Month()), Day: t.Day(),
		Hour: t.Hour(), Minute: t.Minute(), Second: t.Second(),
	}
}

// NewMyDateFromDate 由年月日创建 MyDate。
func NewMyDateFromDate(year, month, day int) *MyDate {
	return &MyDate{Year: year, Month: month, Day: day}
}

// MyDateNow 当前时间。
func MyDateNow() *MyDate { return NewMyDateFromTime(time.Now()) }

// NewMyDateFromNumber 由 Excel 序列值创建 MyDate。
func NewMyDateFromNumber(num Decimal) *MyDate {
	days := int(num.Trunc().Int64())
	d := &MyDate{}
	if days > 365 {
		start := time.Date(1900, 1, 1, 0, 0, 0, 0, time.UTC).AddDate(0, 0, days-ExcelEpochDays)
		d.Year = start.Year()
		d.Month = int(start.Month())
		d.Day = start.Day()
	} else {
		d.Day = days
	}
	frac := num.Sub(DecFromInt(int64(days)))
	hours := frac.Mul(DecFromInt(24))
	d.Hour = int(hours.Trunc().Int64())
	minutes := hours.Sub(DecFromInt(int64(d.Hour))).Mul(DecFromInt(60))
	d.Minute = int(minutes.Trunc().Int64())
	seconds := minutes.Sub(DecFromInt(int64(d.Minute))).Mul(DecFromInt(60))
	d.Second = int(seconds.RoundHalfUp(0).Int64())
	if d.Second == 60 {
		d.Second = 0
		d.Minute++
		if d.Minute == 60 {
			d.Minute = 0
			d.Hour++
		}
	}
	return d
}

var datePatterns = []struct {
	layout string
	kind   string
}{
	{"2006-1-2 15:04:05", "datetime"},
	{"2006/1/2 15:04:05", "datetime"},
	{"2006-1-2 15:04", "datetime_min"},
	{"2006/1/2 15:04", "datetime_min"},
	{"2006-1-2", "date"},
	{"2006/1/2", "date"},
}

// ParseMyDate 解析日期文本，失败返回 nil。
func ParseMyDate(txt string) *MyDate {
	t := strings.TrimSpace(txt)
	if t == "" {
		return nil
	}
	for _, p := range datePatterns {
		if parsed, err := time.Parse(p.layout, t); err == nil {
			d := NewMyDateFromTime(parsed)
			return d
		}
	}
	// 纯时间 / 天+时间
	if d := parseTimeOnly(t); d != nil {
		return d
	}
	return nil
}

func parseTimeOnly(t string) *MyDate {
	parts := strings.SplitN(t, " ", 2)
	timePart := t
	day := 0
	if len(parts) == 2 {
		if n, ok := TryParseDecimal(parts[0]); ok && n.IsInteger() {
			day = int(n.Int64())
			timePart = parts[1]
		} else {
			return nil
		}
	}
	segs := strings.Split(timePart, ":")
	if len(segs) == 2 {
		segs = append(segs, "0")
	}
	if len(segs) != 3 {
		return nil
	}
	h, ok1 := TryParseDecimal(segs[0])
	m, ok2 := TryParseDecimal(segs[1])
	s, ok3 := TryParseDecimal(segs[2])
	if !ok1 || !ok2 || !ok3 {
		return nil
	}
	if !h.IsInteger() || !m.IsInteger() || !s.IsInteger() {
		return nil
	}
	hi, mi, si := int(h.Int64()), int(m.Int64()), int(s.Int64())
	if hi < 0 || hi > 23 || mi < 0 || mi > 59 || si < 0 || si > 59 {
		return nil
	}
	return &MyDate{Day: day, Hour: hi, Minute: mi, Second: si}
}

// ToDateTime 转换为 time.Time（越界值会被夹紧）。
func (d *MyDate) ToDateTime() time.Time {
	year := d.Year
	if year <= 0 {
		year = 1
	}
	month := d.Month
	if month < 1 || month > 12 {
		month = 1
	}
	day := d.Day
	if day < 1 {
		day = 1
	}
	if max := daysInMonth(year, month); day > max {
		day = max
	}
	hour := clampInt(d.Hour, 0, 23)
	minute := clampInt(d.Minute, 0, 59)
	second := clampInt(d.Second, 0, 59)
	return time.Date(year, time.Month(month), day, hour, minute, second, 0, time.UTC)
}

func clampInt(v, lo, hi int) int {
	if v < lo {
		return lo
	}
	if v > hi {
		return hi
	}
	return v
}

// DayOfWeek 星期（周一=1 ... 周日=7）。
func (d *MyDate) DayOfWeek() int {
	wd := int(d.ToDateTime().Weekday())
	if wd == 0 {
		return 7
	}
	return wd
}

// DayOfYear 一年中的第几天。
func (d *MyDate) DayOfYear() int { return d.ToDateTime().YearDay() }

func (d *MyDate) plus(years, months, days, hours, minutes, seconds int) *MyDate {
	base := d.ToDateTime()
	year := base.Year() + years
	month := int(base.Month()) + months
	year += floorDivInt(month-1, 12)
	month = floorModInt(month-1, 12) + 1
	day := base.Day()
	if max := daysInMonth(year, month); day > max {
		day = max
	}
	result := time.Date(year, time.Month(month), day, base.Hour(), base.Minute(), base.Second(), 0, time.UTC)
	result = result.AddDate(0, 0, days).Add(time.Duration(hours)*time.Hour +
		time.Duration(minutes)*time.Minute + time.Duration(seconds)*time.Second)
	return NewMyDateFromTime(result)
}

func (d *MyDate) hasFullDate() bool { return d.Year > 0 && d.Month > 0 && d.Day > 0 }

// AddYears 加年。
func (d *MyDate) AddYears(n int) *MyDate {
	if d.hasFullDate() {
		return d.plus(n, 0, 0, 0, 0, 0)
	}
	return &MyDate{Year: d.Year + n, Month: d.Month, Day: d.Day, Hour: d.Hour, Minute: d.Minute, Second: d.Second}
}

// AddMonths 加月。
func (d *MyDate) AddMonths(n int) *MyDate {
	if d.hasFullDate() {
		return d.plus(0, n, 0, 0, 0, 0)
	}
	t := d.Month + n
	if t >= 1 && t <= 12 {
		return &MyDate{Year: d.Year, Month: t, Day: d.Day, Hour: d.Hour, Minute: d.Minute, Second: d.Second}
	}
	return d.plus(0, n, 0, 0, 0, 0)
}

// AddDays 加天。
func (d *MyDate) AddDays(n int) *MyDate {
	if d.Year > 1900 {
		t := d.Day + n
		if t >= 1 && t <= 28 {
			return &MyDate{Year: d.Year, Month: d.Month, Day: t, Hour: d.Hour, Minute: d.Minute, Second: d.Second}
		}
		return d.plus(0, 0, n, 0, 0, 0)
	}
	return d.addTimeSpan(n * 86400)
}

// AddHours 加小时。
func (d *MyDate) AddHours(n int) *MyDate {
	t := d.Hour + n
	if d.Year > 0 && t >= 0 && t < 24 {
		return &MyDate{Year: d.Year, Month: d.Month, Day: d.Day, Hour: t, Minute: d.Minute, Second: d.Second}
	}
	if d.Year > 1900 {
		return d.plus(0, 0, 0, n, 0, 0)
	}
	return d.addTimeSpan(n * 3600)
}

// AddMinutes 加分钟。
func (d *MyDate) AddMinutes(n int) *MyDate {
	t := d.Minute + n
	if d.Year > 0 && t >= 0 && t <= 59 {
		return &MyDate{Year: d.Year, Month: d.Month, Day: d.Day, Hour: d.Hour, Minute: t, Second: d.Second}
	}
	if d.Year > 1900 {
		return d.plus(0, 0, 0, 0, n, 0)
	}
	return d.addTimeSpan(n * 60)
}

// AddSeconds 加秒。
func (d *MyDate) AddSeconds(n int) *MyDate {
	t := d.Second + n
	if d.Year > 0 && t >= 0 && t <= 59 {
		return &MyDate{Year: d.Year, Month: d.Month, Day: d.Day, Hour: d.Hour, Minute: d.Minute, Second: t}
	}
	if d.Year > 1900 {
		return d.plus(0, 0, 0, 0, 0, n)
	}
	return d.addTimeSpan(n)
}

func (d *MyDate) addTimeSpan(totalSeconds int) *MyDate {
	total := d.Hour*3600 + d.Minute*60 + d.Second + totalSeconds
	newSecond := floorModInt(total, 60)
	totalMinutes := (total - newSecond) / 60
	newMinute := floorModInt(totalMinutes, 60)
	totalHours := (totalMinutes - newMinute) / 60
	newHour := floorModInt(totalHours, 24)
	newDay := (totalHours - newHour) / 24
	return &MyDate{Day: newDay, Hour: newHour, Minute: newMinute, Second: newSecond}
}

// AddDate 加上另一个日期的各分量。
func (d *MyDate) AddDate(other *MyDate) *MyDate {
	return d.plus(other.Year, other.Month, other.Day, other.Hour, other.Minute, other.Second)
}

// SubDate 减去另一个日期的各分量。
func (d *MyDate) SubDate(other *MyDate) *MyDate {
	return d.plus(-other.Year, -other.Month, -other.Day, -other.Hour, -other.Minute, -other.Second)
}

// AddNumber 加上天数。
func (d *MyDate) AddNumber(num Decimal) *MyDate { return NewMyDateFromNumber(d.ToNumber().Add(num)) }

// SubNumber 减去天数。
func (d *MyDate) SubNumber(num Decimal) *MyDate { return NewMyDateFromNumber(d.ToNumber().Sub(num)) }

// MulNumber 乘以数值。
func (d *MyDate) MulNumber(num Decimal) *MyDate { return NewMyDateFromNumber(d.ToNumber().Mul(num)) }

// DivNumber 除以数值。
func (d *MyDate) DivNumber(num Decimal) *MyDate {
	return NewMyDateFromNumber(d.ToNumber().Div32(num))
}

// ToNumber 转换为 Excel 序列值。
func (d *MyDate) ToNumber() Decimal {
	result := DecFromInt(int64(d.Second)).Div(DecFromInt(60))
	result = result.Add(DecFromInt(int64(d.Minute))).Div(DecFromInt(60))
	result = result.Add(DecFromInt(int64(d.Hour))).Div(DecFromInt(24))
	if d.Year > 1900 && d.Month > 0 && d.Day > 0 {
		start := time.Date(d.Year, time.Month(d.Month), d.Day, 0, 0, 0, 0, time.UTC)
		base := time.Date(1900, 1, 1, 0, 0, 0, 0, time.UTC)
		days := int(start.Sub(base).Hours()/24) + ExcelEpochDays
		return result.Add(DecFromInt(int64(days)))
	}
	if d.Day != 0 {
		return result.Add(DecFromInt(int64(d.Day)))
	}
	return result
}

// ToLong 转换为 yyyyMMddHHmmss 形式的整数。
func (d *MyDate) ToLong() int64 {
	var v int64
	if d.Year > 0 {
		v += int64(d.Year) * 10000000000
	}
	if d.Month > 0 {
		v += int64(d.Month) * 100000000
	}
	if d.Day > 0 {
		v += int64(d.Day) * 1000000
	}
	v += int64(d.Hour) * 10000
	v += int64(d.Minute) * 100
	v += int64(d.Second)
	return v
}

func (d *MyDate) String() string {
	if d.Year > 0 {
		return fmt.Sprintf("%04d-%02d-%02d %02d:%02d:%02d", d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second)
	}
	if d.Day > 0 {
		return fmt.Sprintf("%d %02d:%02d:%02d", d.Day, d.Hour, d.Minute, d.Second)
	}
	return fmt.Sprintf("%02d:%02d:%02d", d.Hour, d.Minute, d.Second)
}

// ToString 按 .NET 自定义格式输出。
func (d *MyDate) ToString(format string) string {
	if format == "" {
		return d.String()
	}
	if d.Year == 0 {
		return d.String()
	}
	return FormatDateTime(d.ToDateTime(), format)
}

// FormatDateTime .NET / Joda 风格自定义日期格式化。
func FormatDateTime(t time.Time, pattern string) string {
	if pattern == "" {
		return NewMyDateFromTime(t).String()
	}
	tokens := []string{"yyyy", "fff", "zzz", "yy", "MM", "dd", "HH", "hh", "mm", "ss", "ff",
		"M", "d", "H", "h", "m", "s", "f", "K", "tt"}
	var sb strings.Builder
	i := 0
	n := len(pattern)
	for i < n {
		c := pattern[i]
		if c == '\'' || c == '"' {
			quote := c
			i++
			for i < n && pattern[i] != quote {
				sb.WriteByte(pattern[i])
				i++
			}
			i++
			continue
		}
		if c == '\\' && i+1 < n {
			sb.WriteByte(pattern[i+1])
			i += 2
			continue
		}
		matched := ""
		for _, token := range tokens {
			if strings.HasPrefix(pattern[i:], token) {
				matched = token
				break
			}
		}
		if matched == "" {
			sb.WriteByte(c)
			i++
			continue
		}
		i += len(matched)
		switch matched {
		case "yyyy":
			sb.WriteString(fmt.Sprintf("%04d", t.Year()))
		case "yy":
			sb.WriteString(fmt.Sprintf("%02d", t.Year()%100))
		case "MM":
			sb.WriteString(fmt.Sprintf("%02d", int(t.Month())))
		case "M":
			sb.WriteString(fmt.Sprintf("%d", int(t.Month())))
		case "dd":
			sb.WriteString(fmt.Sprintf("%02d", t.Day()))
		case "d":
			sb.WriteString(fmt.Sprintf("%d", t.Day()))
		case "HH":
			sb.WriteString(fmt.Sprintf("%02d", t.Hour()))
		case "H":
			sb.WriteString(fmt.Sprintf("%d", t.Hour()))
		case "hh":
			h := t.Hour() % 12
			if h == 0 {
				h = 12
			}
			sb.WriteString(fmt.Sprintf("%02d", h))
		case "h":
			h := t.Hour() % 12
			if h == 0 {
				h = 12
			}
			sb.WriteString(fmt.Sprintf("%d", h))
		case "mm":
			sb.WriteString(fmt.Sprintf("%02d", t.Minute()))
		case "m":
			sb.WriteString(fmt.Sprintf("%d", t.Minute()))
		case "ss":
			sb.WriteString(fmt.Sprintf("%02d", t.Second()))
		case "s":
			sb.WriteString(fmt.Sprintf("%d", t.Second()))
		case "fff":
			sb.WriteString(fmt.Sprintf("%03d", t.Nanosecond()/1000000))
		case "ff":
			sb.WriteString(fmt.Sprintf("%02d", t.Nanosecond()/10000000))
		case "f":
			sb.WriteString(fmt.Sprintf("%d", t.Nanosecond()/100000000))
		case "tt":
			if t.Hour() < 12 {
				sb.WriteString("AM")
			} else {
				sb.WriteString("PM")
			}
		case "zzz":
			sb.WriteString("+00:00")
		case "K":
			sb.WriteString("Z")
		}
	}
	return sb.String()
}
