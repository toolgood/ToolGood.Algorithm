package distributions

import (
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/mathnet/rootfinding"
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/mathnet/specialfunctions"
)

// StudentT 学生 t 分布，对应 C# MathNet/Distributions/StudentT.cs。

// StudentTCDF 返回学生 t 分布累积分布函数值。
func StudentTCDF(location, scale float64, freedom int, x float64) float64 {
	k := (x - location) / scale
	h := float64(freedom) / (float64(freedom) + (k * k))
	ib := 0.5 * specialfunctions.BetaRegularized(float64(freedom)/2.0, 0.5, h)
	if x <= location {
		return ib
	}
	return 1.0 - ib
}

// StudentTInvCDF 返回学生 t 分布累积分布函数的反函数值。
func StudentTInvCDF(location, scale float64, freedom int, p float64) float64 {
	if p == 0.5 {
		return location
	}

	return rootfinding.FindRoot(func(x float64) float64 {
		k := (x - location) / scale
		h := float64(freedom) / (float64(freedom) + (k * k))
		ib := 0.5 * specialfunctions.BetaRegularized(float64(freedom)/2.0, 0.5, h)
		if x <= location {
			return ib - p
		}
		return 1.0 - ib - p
	}, -800, 800, 1e-12)
}
