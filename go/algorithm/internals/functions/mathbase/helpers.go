package mathbase

import (
	"math"
	"strings"
)

// 本文件提供 MathBase 各函数共用的辅助方法，对应 C# 中的
// MathEx / decimal 运算符溢出检查 / TryGetInt / .NET "N" 格式化。

var (
	decTen     = DecFromInt(10)
	decHundred = DecFromInt(100)
	dec66      = DecFromInt(66)
	decIntMin  = DecFromInt(math.MinInt32)
	decIntMax  = DecFromInt(math.MaxInt32)

	// decimalPI 对应 C# MathEx.PI。
	decimalPI = MustDecimal("3.14159265358979323846264338327950288419716939937510")
)

// isOverflow 判断数值是否超出 C# decimal 的可表示范围
// （C# 中会产生 OverflowException，调用方据此返回错误值）。
func isOverflow(d Decimal) bool {
	return d.Abs().Greater(DecimalMax)
}

// decFromFloat 把 float64 转成 Decimal；NaN / Inf 返回 false。
func decFromFloat(f float64) (Decimal, bool) {
	if math.IsNaN(f) || math.IsInf(f, 0) {
		return DecimalZero, false
	}
	return DecFromFloat(f), true
}

// tryGetInt 对应 C# 的 TryGetInt：先向零截断，再判断是否落在 int 范围内。
func tryGetInt(op Operand) (int, bool) {
	t := op.NumberValue().Trunc()
	if t.Less(decIntMin) || t.Greater(decIntMax) {
		return 0, false
	}
	return int(t.Int64()), true
}

// pow10 返回 10 的 n 次幂（n >= 0）。
func pow10(n int) Decimal {
	return decTen.Pow(n)
}

// factorialChecked 计算 n 的阶乘，超出 decimal 范围时返回 false。
func factorialChecked(n int) (Decimal, bool) {
	d := DecimalOne
	for i := 2; i <= n; i++ {
		d = d.Mul(DecFromInt(int64(i)))
		if isOverflow(d) {
			return DecimalZero, false
		}
	}
	return d, true
}

// formatGrouped 等价于 .NET 的 ToString("N{digits}", InvariantCulture)：
// 四舍五入到 digits 位小数，补齐尾零并按千分位分组。
func formatGrouped(d Decimal, digits int) string {
	rounded := d.RoundHalfUp(int32(digits))
	negative := rounded.Sign() < 0
	s := rounded.Abs().String()
	intPart := s
	frac := ""
	if idx := strings.IndexByte(s, '.'); idx >= 0 {
		intPart = s[:idx]
		frac = s[idx+1:]
	}
	if digits > 0 {
		if len(frac) < digits {
			frac += strings.Repeat("0", digits-len(frac))
		} else if len(frac) > digits {
			frac = frac[:digits]
		}
	}
	var sb strings.Builder
	if negative {
		sb.WriteByte('-')
	}
	for i := 0; i < len(intPart); i++ {
		if i > 0 && (len(intPart)-i)%3 == 0 {
			sb.WriteByte(',')
		}
		sb.WriteByte(intPart[i])
	}
	if digits > 0 {
		sb.WriteByte('.')
		sb.WriteString(frac)
	}
	return sb.String()
}
