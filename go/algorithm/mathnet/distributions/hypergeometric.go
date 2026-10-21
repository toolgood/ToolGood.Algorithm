package distributions

import (
	"math"

	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/mathnet/statistics"
)

// Hypergeometric 超几何分布，对应 C# MathNet/Distributions/Hypergeometric.cs。

// HypergeometricPMF 返回超几何分布概率质量函数值。
func HypergeometricPMF(population, success, draws, k int) float64 {
	if k < 0 || k > draws || k > success || k < draws+success-population {
		return 0
	}
	logResult := statistics.BinomialLn(success, k) +
		statistics.BinomialLn(population-success, draws-k) -
		statistics.BinomialLn(population, draws)
	return math.Exp(logResult)
}
