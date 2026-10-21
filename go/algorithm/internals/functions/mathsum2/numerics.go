package mathsum2

import (
	"math"
)

// 本文件用 float64 实现 MathSum2 依赖的 MathNet 数值核（等价于 C# 的
// ToolGood.Algorithm.MathNet.Numerics：SpecialFunctions 中的 Gamma / Beta / Erf 部分，
// 以及 RootFind 的求根）。Go 版 mathnet 包完成后可替换为本文件的函数体。
//
// 与 C# 的差异：C# 全程使用 decimal（28~29 位有效数字），此处为 float64（约 15~16 位）。

const (
	sqrt2   = 1.4142135623730950488016887242096980785696718753769
	sqrt2Pi = 2.5066282746310005024157652848110452530069867406099
	// betaEps 连分式的收敛阈值。
	betaEps = 1e-16
	// fpmin 防止连分式除零的极小值。
	fpmin = 1e-300
)

// gammaLn 对应 C# SpecialFunctions.GammaLn。
func gammaLn(z float64) float64 {
	if z <= 0 && z == math.Trunc(z) {
		return math.Inf(1)
	}
	v, _ := math.Lgamma(z)
	return v
}

// factorialLn 对应 C# SpecialFunctions.FactorialLn。
func factorialLn(x int) float64 {
	return gammaLn(float64(x) + 1)
}

// binomialLn 对应 C# SpecialFunctions.BinomialLn。
func binomialLn(n, k int) float64 {
	if k < 0 || k > n {
		return math.Inf(-1)
	}
	return factorialLn(n) - factorialLn(k) - factorialLn(n-k)
}

// betaContinuedFraction 是不完全贝塔函数的连分式（Lentz 算法）。
func betaContinuedFraction(a, b, x float64) float64 {
	qab := a + b
	qap := a + 1
	qam := a - 1
	c := 1.0
	d := 1.0 - qab*x/qap
	if math.Abs(d) < fpmin {
		d = fpmin
	}
	d = 1 / d
	h := d
	for m := 1; m <= 400; m++ {
		m2 := 2 * m
		aa := float64(m) * (b - float64(m)) * x / ((qam + float64(m2)) * (a + float64(m2)))
		d = 1 + aa*d
		if math.Abs(d) < fpmin {
			d = fpmin
		}
		c = 1 + aa/c
		if math.Abs(c) < fpmin {
			c = fpmin
		}
		d = 1 / d
		h *= d * c
		aa = -(a + float64(m)) * (qab + float64(m)) * x / ((a + float64(m2)) * (qap + float64(m2)))
		d = 1 + aa*d
		if math.Abs(d) < fpmin {
			d = fpmin
		}
		c = 1 + aa/c
		if math.Abs(c) < fpmin {
			c = fpmin
		}
		d = 1 / d
		delta := d * c
		h *= delta
		if math.Abs(delta-1) < betaEps {
			break
		}
	}
	return h
}

// betaRegularized 正则化不完全贝塔函数 I_x(a,b)（对应 C# SpecialFunctions.BetaRegularized）。
func betaRegularized(a, b, x float64) float64 {
	if x <= 0 {
		return 0
	}
	if x >= 1 {
		return 1
	}
	if a <= 0 || b <= 0 {
		return math.NaN()
	}
	lnFront := gammaLn(a+b) - gammaLn(a) - gammaLn(b) + a*math.Log(x) + b*math.Log1p(-x)
	front := math.Exp(lnFront)
	if x < (a+1)/(a+b+2) {
		return front * betaContinuedFraction(a, b, x) / a
	}
	return 1 - front*betaContinuedFraction(b, a, 1-x)/b
}

// gammaLowerRegularized 正则化下不完全伽马函数 P(a,x)（对应 C# SpecialFunctions.GammaLowerRegularized）。
func gammaLowerRegularized(a, x float64) float64 {
	if x <= 0 {
		return 0
	}
	if a <= 0 {
		return math.NaN()
	}
	lnPrefix := -x + a*math.Log(x) - gammaLn(a)
	if x < a+1 {
		// 级数展开
		ap := a
		total := 1 / a
		delta := total
		for i := 0; i < 1000; i++ {
			ap++
			delta *= x / ap
			total += delta
			if math.Abs(delta) < math.Abs(total)*1e-16 {
				break
			}
		}
		return total * math.Exp(lnPrefix)
	}
	// Q(a,x) 连分式
	b := x + 1 - a
	c := 1 / fpmin
	d := 1 / b
	h := d
	for i := 1; i < 1000; i++ {
		an := -float64(i) * (float64(i) - a)
		b += 2
		d = an*d + b
		if math.Abs(d) < fpmin {
			d = fpmin
		}
		c = b + an/c
		if math.Abs(c) < fpmin {
			c = fpmin
		}
		d = 1 / d
		delta := d * c
		h *= delta
		if math.Abs(delta-1) < 1e-16 {
			break
		}
	}
	q := math.Exp(lnPrefix) * h
	return 1 - q
}

// findRoot 在 [lower, upper] 上二分求根（对应 C# Brent.FindRoot 的契约）。
func findRoot(f func(float64) float64, lower, upper, accuracy float64) float64 {
	fLow := f(lower)
	fHigh := f(upper)
	if fLow == 0 {
		return lower
	}
	if fHigh == 0 {
		return upper
	}
	if fLow*fHigh > 0 {
		return math.NaN()
	}
	for i := 0; i < 300; i++ {
		mid := 0.5 * (lower + upper)
		fMid := f(mid)
		if fMid == 0 || (upper-lower) <= accuracy {
			return mid
		}
		if (fLow < 0) != (fMid < 0) {
			upper = mid
			fHigh = fMid
		} else {
			lower = mid
			fLow = fMid
		}
	}
	return 0.5 * (lower + upper)
}

// gammaLowerRegularizedInv 是 P(a,x) 的反函数（对应 C# GammaLowerRegularizedInv）。
func gammaLowerRegularizedInv(a, y0 float64) float64 {
	if y0 <= 0 {
		return 0
	}
	if y0 >= 1 {
		return math.Inf(1)
	}
	lo, hi := 0.0, math.Max(1, a)
	for gammaLowerRegularized(a, hi) < y0 {
		hi *= 2
		if hi > 1e300 {
			break
		}
	}
	for i := 0; i < 200; i++ {
		mid := 0.5 * (lo + hi)
		if gammaLowerRegularized(a, mid) < y0 {
			lo = mid
		} else {
			hi = mid
		}
		if hi-lo <= 1e-14*math.Max(1, hi) {
			break
		}
	}
	return 0.5 * (lo + hi)
}

// erfcInv 对应 C# SpecialFunctions.ErfcInv。
func erfcInv(z float64) float64 {
	return math.Erfcinv(z)
}

// numericResult 把 float64 结果转成操作数；NaN / Inf 视为函数错误
// （对应 C# 捕获 OverflowException / InvalidOperationException 后返回 FunctionError）。
func numericResult(errFn func() Operand, v float64) Operand {
	if math.IsNaN(v) || math.IsInf(v, 0) {
		return errFn()
	}
	return CreateDecimal(DecFromFloat(v))
}

// tryGetInt 对应 C# FunctionBase.TryGetInt：超出 int 范围时返回 false。
func tryGetInt(arg Operand) (int, bool) {
	v := arg.NumberValue().Trunc()
	if v.Less(DecFromInt(math.MinInt32)) || v.Greater(DecFromInt(math.MaxInt32)) {
		return 0, false
	}
	return int(v.Int64()), true
}

// numberArgTypes 把所有参数标记为 NUMBER（booleanLast 为 true 时最后一个参数标记为 BOOLEAN）。
func numberArgTypes(engine *AlgorithmEngine, f *BaseFunction, result *[]ParameterType, booleanLast bool) {
	count := f.ArgCount()
	for i := 0; i < count; i++ {
		if booleanLast && i == count-1 {
			f.Arg(i).GetParameterTypes(engine, result, OperandTypeBoolean)
			continue
		}
		f.Arg(i).GetParameterTypes(engine, result, OperandTypeNumber)
	}
}
