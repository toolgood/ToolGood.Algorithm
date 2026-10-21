package distributions

import (
	"math"

	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/mathnet/specialfunctions"
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/mathnet/statistics"
)

// Poisson 泊松分布，对应 C# MathNet/Distributions/Poisson.cs。

// PoissonPMF 返回泊松分布概率质量函数值。
func PoissonPMF(lambda float64, k int) float64 {
	return math.Exp(-lambda + (float64(k) * math.Log(lambda)) - statistics.FactorialLn(k))
}

// PoissonCDF 返回泊松分布累积分布函数值。
func PoissonCDF(lambda, x float64) float64 {
	return 1.0 - specialfunctions.GammaLowerRegularized(x+1, lambda)
}
