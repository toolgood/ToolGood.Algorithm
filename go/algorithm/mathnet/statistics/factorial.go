package statistics

import "github.com/toolgood/ToolGood.Algorithm/go/algorithm/mathnet/specialfunctions"

// Factorial 阶乘相关函数，对应 C# MathNet/Statistics/Factorial.cs
// （C# 中声明为 SpecialFunctions 的部分类成员，Go 版按文件归属放在 statistics 包）。

// FactorialLn 返回 ln(x!)。
func FactorialLn(x int) float64 {
	if x <= 1 {
		return 0
	}
	return specialfunctions.GammaLn(float64(x + 1))
}

// BinomialLn 返回 ln(C(n,k))，参数非法时返回 decimal.MinValue。
func BinomialLn(n, k int) float64 {
	if k < 0 || n < 0 || k > n {
		return decimalMinValue
	}

	return FactorialLn(n) - FactorialLn(k) - FactorialLn(n-k)
}
