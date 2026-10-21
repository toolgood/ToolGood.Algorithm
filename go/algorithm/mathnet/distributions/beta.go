package distributions

import (
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/mathnet/rootfinding"
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/mathnet/specialfunctions"
)

// Beta 贝塔分布，对应 C# MathNet/Distributions/Beta.cs。

// BetaCDF 返回贝塔分布累积分布函数值。
func BetaCDF(a, b, x float64) float64 {
	if x < 0.0 {
		return 0.0
	}

	if x >= 1.0 {
		return 1.0
	}

	if a == 0.0 && b == 0.0 {
		if x >= 0.0 && x < 1.0 {
			return 0.5
		}

		return 1.0
	}

	if a == 0.0 {
		return 1.0
	}

	if b == 0.0 {
		if x >= 1.0 {
			return 1.0
		}
		return 0.0
	}

	if a == 1.0 && b == 1.0 {
		return x
	}

	return specialfunctions.BetaRegularized(a, b, x)
}

// BetaInvCDF 返回贝塔分布累积分布函数的反函数值。
func BetaInvCDF(a, b, p float64) float64 {
	return rootfinding.FindRoot(func(x float64) float64 {
		return specialfunctions.BetaRegularized(a, b, x) - p
	}, 0.0, 1.0, 1e-12)
}
