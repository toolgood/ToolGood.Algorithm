package distributions

import (
	"math"

	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/mathnet/specialfunctions"
)

// Normal 正态分布，对应 C# MathNet/Distributions/Normal.cs。

// NormalCDF 返回正态分布累积分布函数值。
func NormalCDF(mean, stddev, x float64) float64 {
	return 0.5 * specialfunctions.Erfc((mean-x)/(stddev*sqrt2))
}

// NormalInvCDF 返回正态分布累积分布函数的反函数值。
func NormalInvCDF(mean, stddev, p float64) float64 {
	return mean - (stddev * sqrt2 * specialfunctions.ErfcInv(2.0*p))
}

// NormalPDF 返回正态分布概率密度函数值。
func NormalPDF(mean, stddev, x float64) float64 {
	d := (x - mean) / stddev
	return math.Exp(-0.5*d*d) / (sqrt2Pi * stddev)
}
