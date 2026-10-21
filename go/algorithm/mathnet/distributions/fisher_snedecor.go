package distributions

import (
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/mathnet/rootfinding"
	"github.com/toolgood/ToolGood.Algorithm/go/algorithm/mathnet/specialfunctions"
)

// FisherSnedecor F 分布，对应 C# MathNet/Distributions/FisherSnedecor.cs。

// FisherSnedecorCDF 返回 F 分布累积分布函数值。
func FisherSnedecorCDF(d1, d2 int, x float64) float64 {
	fd1 := float64(d1)
	fd2 := float64(d2)
	return specialfunctions.BetaRegularized(fd1/2.0, fd2/2.0, fd1*x/(fd1*x+fd2))
}

// FisherSnedecorInvCDF 返回 F 分布累积分布函数的反函数值。
func FisherSnedecorInvCDF(d1, d2 int, p float64) float64 {
	fd1 := float64(d1)
	fd2 := float64(d2)
	return rootfinding.FindRoot(
		func(x float64) float64 {
			return specialfunctions.BetaRegularized(fd1/2.0, fd2/2.0, fd1*x/(fd1*x+fd2)) - p
		},
		0, 1000, 1e-12)
}
