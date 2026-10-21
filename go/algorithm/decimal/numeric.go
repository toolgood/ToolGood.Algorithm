package decimal

import "math"

// ExcelEpochDays Excel / .NET 序列化日期转换使用的偏移天数。
const ExcelEpochDays = 2

// DecimalMaxValue C# decimal.MaxValue。
var DecimalMaxValue = DecimalMax

func Divide(a, b Decimal) Decimal { return a.Div(b) }

func TruncDecimal(d Decimal) int { return int(d.Trunc().Int64()) }

func RoundHalfAway(d Decimal, digits int32) Decimal { return d.RoundHalfUp(digits) }

func IsNaNOrInf(f float64) bool {
	return math.IsNaN(f) || math.IsInf(f, 0)
}

// Plain 等价于 BigDecimal.toPlainString()。
func Plain(d Decimal) string { return d.String() }

// PlainStripValue 去掉末尾 0 的普通字符串。
func PlainStripValue(d Decimal) string { return d.PlainStrip() }
