package mathnet

import "math"

// 容差与近似比较，对应 C# MathNet/Precision.cs。

const (
	// DecimalPrecision C# Precision.DecimalPrecision。
	DecimalPrecision = 0.0000000000000000000000000001
	// PositiveDecimalPrecision C# Precision.PositiveDecimalPrecision。
	PositiveDecimalPrecision = 2 * DecimalPrecision
	// DefaultDecimalAccuracy C# Precision.DefaultDecimalAccuracy。
	defaultDecimalAccuracy = DecimalPrecision * 10
)

// AlmostEqualNormRelative 相对误差比较；a、b 有一方接近 0 时退化为绝对误差比较。
func AlmostEqualNormRelative(a, b, diff, maximumError float64) bool {
	if math.Abs(a) < DecimalPrecision || math.Abs(b) < DecimalPrecision {
		return math.Abs(diff) < maximumError
	}

	if (a == 0 && math.Abs(b) < maximumError) || (b == 0 && math.Abs(a) < maximumError) {
		return true
	}

	return math.Abs(diff) < maximumError*math.Max(math.Abs(a), math.Abs(b))
}

// AlmostEqualRelative 以默认精度做相对误差比较。
func AlmostEqualRelative(a, b float64) bool {
	return AlmostEqualNormRelative(a, b, a-b, defaultDecimalAccuracy)
}

// AlmostEqual 以默认精度做绝对误差比较。
func AlmostEqual(a, b float64) bool {
	return AlmostEqualNorm(a, b, a-b, defaultDecimalAccuracy)
}

// AlmostEqualNorm 绝对误差比较。
func AlmostEqualNorm(a, b, diff, maximumAbsoluteError float64) bool {
	return math.Abs(diff) < maximumAbsoluteError
}
