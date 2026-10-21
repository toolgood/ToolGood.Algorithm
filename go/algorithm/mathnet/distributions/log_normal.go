package distributions

import (
	"math"

	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/mathnet/specialfunctions"
)

// LogNormal 对数正态分布，对应 C# MathNet/Distributions/LogNormal.cs。

// LogNormalCDF 返回对数正态分布累积分布函数值。
func LogNormalCDF(mu, sigma, x float64) float64 {
	if x < 0.0 {
		return 0.0
	}
	return 0.5 * (1.0 + specialfunctions.Erf((math.Log(x)-mu)/(sigma*sqrt2)))
}

// LogNormalInvCDF 返回对数正态分布累积分布函数的反函数值。
func LogNormalInvCDF(mu, sigma, p float64) float64 {
	if p <= 0.0 {
		return 0.0
	}
	if p >= 1.0 {
		return decimalMaxValue
	}
	return math.Exp(mu - sigma*sqrt2*specialfunctions.ErfcInv(2.0*p))
}
