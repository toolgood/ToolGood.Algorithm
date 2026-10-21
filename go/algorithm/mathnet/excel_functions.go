package mathnet

import (
	"sort"

	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/mathnet/distributions"
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/mathnet/specialfunctions"
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/mathnet/statistics"
)

// Excel 兼容的分布函数，对应 C# MathNet/ExcelFunctions.cs。

// NormSDist 标准正态分布累积分布函数。
func NormSDist(z float64) float64 {
	return distributions.NormalCDF(0, 1, z)
}

// NormSInv 标准正态分布累积分布函数的反函数。
func NormSInv(probability float64) float64 {
	return distributions.NormalInvCDF(0, 1, probability)
}

// NormDist 正态分布：cumulative 为真返回 CDF，否则返回 PDF。
func NormDist(x, mean, standardDev float64, cumulative bool) float64 {
	if cumulative {
		return distributions.NormalCDF(mean, standardDev, x)
	}
	return distributions.NormalPDF(mean, standardDev, x)
}

// NormInv 正态分布累积分布函数的反函数。
func NormInv(probability, mean, standardDev float64) float64 {
	return distributions.NormalInvCDF(mean, standardDev, probability)
}

// TDist 学生 t 分布，tails 只能为 1 或 2（否则 panic，对应 C# 抛 ArgumentOutOfRangeException）。
func TDist(x float64, degreesFreedom int, tails int) float64 {
	switch tails {
	case 1:
		return 1 - distributions.StudentTCDF(0, 1, degreesFreedom, x)

	case 2:
		return 1 - distributions.StudentTCDF(0, 1, degreesFreedom, x) + distributions.StudentTCDF(0, 1, degreesFreedom, -x)

	default:
		panic("tails is out of range")
	}
}

// TInv 学生 t 分布累积分布函数的反函数。
func TInv(probability float64, degreesFreedom int) float64 {
	return -distributions.StudentTInvCDF(0, 1, degreesFreedom, probability/2)
}

// FDist F 分布累积分布函数。
func FDist(x float64, degreesFreedom1, degreesFreedom2 int) float64 {
	return 1 - distributions.FisherSnedecorCDF(degreesFreedom1, degreesFreedom2, x)
}

// FInv F 分布累积分布函数的反函数。
func FInv(probability float64, degreesFreedom1, degreesFreedom2 int) float64 {
	return distributions.FisherSnedecorInvCDF(degreesFreedom1, degreesFreedom2, 1-probability)
}

// BetaDist 贝塔分布累积分布函数。
func BetaDist(x, alpha, beta float64) float64 {
	return distributions.BetaCDF(alpha, beta, x)
}

// BetaInv 贝塔分布累积分布函数的反函数。
func BetaInv(probability, alpha, beta float64) float64 {
	return distributions.BetaInvCDF(alpha, beta, probability)
}

// GammaDist 伽马分布：cumulative 为真返回 CDF，否则返回 PDF。
func GammaDist(x, alpha, beta float64, cumulative bool) float64 {
	if cumulative {
		return distributions.GammaCDF(alpha, 1/beta, x)
	}
	return distributions.GammaPDF(alpha, 1/beta, x)
}

// GammaInv 伽马分布累积分布函数的反函数。
func GammaInv(probability, alpha, beta float64) float64 {
	return distributions.GammaInvCDF(alpha, 1/beta, probability)
}

// GAMMALN 返回 ln(Γ(z))。
func GAMMALN(z float64) float64 {
	return specialfunctions.GammaLn(z)
}

// HypgeomDist 超几何分布概率质量函数。
func HypgeomDist(k, draws, success, population int) float64 {
	return distributions.HypergeometricPMF(population, success, draws, k)
}

// NegbinomDist 负二项分布概率质量函数。
func NegbinomDist(k int, r, p float64) float64 {
	return distributions.NegativeBinomialPMF(r, p, k)
}

// LognormDist 对数正态分布累积分布函数。
func LognormDist(x, mu, sigma float64) float64 {
	return distributions.LogNormalCDF(mu, sigma, x)
}

// LogInv 对数正态分布累积分布函数的反函数。
func LogInv(p, mu, sigma float64) float64 {
	return distributions.LogNormalInvCDF(mu, sigma, p)
}

// BinomDist 二项分布：state 为假返回 PMF，否则返回 CDF。
func BinomDist(k, n int, p float64, state bool) float64 {
	if state == false {
		return distributions.BinomialPMF(p, n, k)
	}
	return distributions.BinomialCDF(p, n, float64(k))
}

// Poisson 泊松分布：state 为假返回 PMF，否则返回 CDF。
func Poisson(k int, lambda float64, state bool) float64 {
	if state == false {
		return distributions.PoissonPMF(lambda, k)
	}
	return distributions.PoissonCDF(lambda, float64(k))
}

// Quartile 四分位数：quant 为 0/1/2/3/4 分别对应最小值、Q1、中位数、Q3、最大值。
func Quartile(array []float64, quant int) float64 {
	switch quant {
	case 0:
		return statistics.Minimum(array)

	case 1:
		return statistics.QuantileCustomInplace(array, 0.25)

	case 2:
		return statistics.QuantileCustomInplace(array, 0.5)

	case 3:
		return statistics.QuantileCustomInplace(array, 0.75)

	case 4:
		return statistics.Maximum(array)

	default:
		panic("quant is out of range")
	}
}

// Percentile 计算分位数（0 <= k <= 1）。
func Percentile(array []float64, k float64) float64 {
	return statistics.QuantileCustomInplace(array, k)
}

// PercentRank 计算 x 在数组中的分位排名（会就地排序 array）。
func PercentRank(array []float64, x float64) float64 {
	sort.Float64s(array)
	return statistics.QuantileRank(array, x)
}
