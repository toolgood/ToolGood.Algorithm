package distributions

import (
	"math"

	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/mathnet/specialfunctions"
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/mathnet/statistics"
)

// Binomial 二项分布，对应 C# MathNet/Distributions/Binomial.cs。

// BinomialPMF 返回二项分布概率质量函数值。
func BinomialPMF(p float64, n, k int) float64 {
	if k < 0 || k > n {
		return 0.0
	}

	if p == 0.0 {
		if k == 0 {
			return 1.0
		}
		return 0.0
	}

	if p == 1.0 {
		if k == n {
			return 1.0
		}
		return 0.0
	}

	return math.Exp(statistics.BinomialLn(n, k) + (float64(k) * math.Log(p)) + (float64(n-k) * math.Log(1.0-p)))
}

// BinomialCDF 返回二项分布累积分布函数值。
func BinomialCDF(p float64, n int, x float64) float64 {
	if x < 0.0 {
		return 0.0
	}

	if x > float64(n) {
		return 1.0
	}

	if p <= 0.0 {
		// p=0 时全部概率集中在 k=0
		return 1.0
	}

	if p >= 1.0 {
		// p=1 时全部概率集中在 k=n
		if x >= float64(n) {
			return 1.0
		}
		return 0.0
	}

	if x >= float64(n) {
		// x=n 时 n-k=0, BetaRegularized 内部会出现 0/0
		return 1.0
	}

	k := math.Floor(x)
	return specialfunctions.BetaRegularized(float64(n)-k, k+1, 1-p)
}
