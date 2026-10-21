package distributions

import (
	"math"

	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/mathnet/specialfunctions"
)

// NegativeBinomial 负二项分布，对应 C# MathNet/Distributions/NegativeBinomial.cs。

// NegativeBinomialPMF 返回负二项分布概率质量函数值。
func NegativeBinomialPMF(r, p float64, k int) float64 {
	return math.Exp(NegativeBinomialPMFLn(r, p, k))
}

// NegativeBinomialPMFLn 返回负二项分布概率质量函数的对数。
func NegativeBinomialPMFLn(r, p float64, k int) float64 {
	return specialfunctions.GammaLn(r+float64(k)) -
		specialfunctions.GammaLn(r) -
		specialfunctions.GammaLn(float64(k)+1.0) +
		(r * math.Log(p)) +
		(float64(k) * math.Log(1.0-p))
}
