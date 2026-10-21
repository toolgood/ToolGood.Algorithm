package mathtrigonometric

import "math"

// 本文件提供三角函数组共用的辅助方法（对应 C# 的 MathEx 常量与溢出检查）。

var (
	// decimalPI 对应 C# MathEx.PI。
	decimalPI = MustDecimal("3.14159265358979323846264338327950288419716939937510")
	decTwo    = DecFromInt(2)
	dec180    = DecFromInt(180)
	dec66     = DecFromInt(66)
)

// decimalHalfPi 对应 C# MathEx.PI / 2。
var decimalHalfPi = decimalPI.Div(decTwo)

// isOverflow 判断数值是否超出 C# decimal 的可表示范围。
func isOverflow(d Decimal) bool {
	return d.Abs().Greater(DecimalMax)
}

// fromFloat 把 float64 转成 Decimal；NaN / Inf 返回 false。
func fromFloat(f float64) (Decimal, bool) {
	if math.IsNaN(f) || math.IsInf(f, 0) {
		return DecimalZero, false
	}
	return DecFromFloat(f), true
}
