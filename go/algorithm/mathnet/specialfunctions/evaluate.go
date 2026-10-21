package specialfunctions

// Evaluate 对应 C# MathNet/SpecialFunctions/Evaluate.cs：多项式与 Chebyshev 求值。

// Polynomial 用 Horner 法求多项式的值（coefficients[0] 为常数项）。
func Polynomial(z float64, coefficients []float64) float64 {
	sum := coefficients[len(coefficients)-1]
	for i := len(coefficients) - 2; i >= 0; i-- {
		sum *= z
		sum += coefficients[i]
	}
	return sum
}

// ChebyshevA 移植自 MathNet.Numerics.SpecialFunctions.Evaluate.ChebyshevA。
// 系数按逆序存储，即零阶项在数组末尾。
func ChebyshevA(coefficients []float64, x float64) float64 {
	p := 0
	b0 := coefficients[p]
	p++
	b1 := 0.0
	i := len(coefficients) - 1
	var b2 float64
	for {
		b2 = b1
		b1 = b0
		b0 = x*b1 - b2 + coefficients[p]
		p++
		i--
		if i <= 0 {
			break
		}
	}
	return 0.5 * (b0 - b2)
}
