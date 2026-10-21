package specialfunctions

import "math"

// 与 C# MathNet/Precision.cs 一致的容差常数。

const (
	// decimalPrecision C# Precision.DecimalPrecision。
	decimalPrecision = 0.0000000000000000000000000001
	// defaultDecimalAccuracy C# Precision.DefaultDecimalAccuracy。
	defaultDecimalAccuracy = decimalPrecision * 10
)

// almostEqual 对应 C# 的 decimal.AlmostEqual（绝对误差比较）。
func almostEqual(a, b float64) bool {
	return math.Abs(a-b) < defaultDecimalAccuracy
}
