package distributions

import (
	"math"

	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/mathnet/specialfunctions"
)

// Gamma 伽马分布，对应 C# MathNet/Distributions/Gamma.cs。

// GammaCDF 返回伽马分布累积分布函数值。
func GammaCDF(shape, rate, x float64) float64 {
	if shape == 0.0 && rate == 0.0 {
		return 0.0
	}

	return specialfunctions.GammaLowerRegularized(shape, x*rate)
}

// GammaPDF 返回伽马分布概率密度函数值。
func GammaPDF(shape, rate, x float64) float64 {
	if shape == 0.0 && rate == 0.0 {
		return 0.0
	}

	if shape == 1.0 {
		return rate * math.Exp(-rate*x)
	}

	if x == 0.0 && shape > 1.0 {
		// x=0 处 pdf 的极限值为 0；
		// (shape<1 时该极限发散，仍走下方公式由零底负数次幂异常表示)
		return 0.0
	}

	if shape > 160.0 {
		return math.Exp(GammaPDFLn(shape, rate, x))
	}

	return math.Pow(rate, shape) * math.Pow(x, shape-1.0) * math.Exp(-rate*x) / specialfunctions.Gamma(shape)
}

// GammaPDFLn 返回伽马分布概率密度函数的对数。
func GammaPDFLn(shape, rate, x float64) float64 {
	if shape == 0.0 && rate == 0.0 {
		return decimalMinValue
	}

	if shape == 1.0 {
		return math.Log(rate) - (rate * x)
	}

	return (shape * math.Log(rate)) + ((shape - 1.0) * math.Log(x)) - (rate * x) - specialfunctions.GammaLn(shape)
}

// GammaInvCDF 返回伽马分布累积分布函数的反函数值。
func GammaInvCDF(shape, rate, p float64) float64 {
	return specialfunctions.GammaLowerRegularizedInv(shape, p) / rate
}
