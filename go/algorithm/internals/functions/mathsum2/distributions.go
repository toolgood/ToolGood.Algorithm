package mathsum2

import "math"

// 本文件用 float64 实现 C# MathNet.Numerics 中的分布函数（Normal / Beta / Gamma /
// StudentT / FisherSnedecor / Binomial / Poisson / Hypergeometric / NegativeBinomial /
// LogNormal / Exponential / Weibull），供 MathSum2 的函数调用。

// ------------------------------------------------------------------ Normal

func normalCDF(mean, stddev, x float64) float64 {
	return 0.5 * math.Erfc((mean-x)/(stddev*sqrt2))
}

func normalPDF(mean, stddev, x float64) float64 {
	d := (x - mean) / stddev
	return math.Exp(-0.5*d*d) / (sqrt2Pi * stddev)
}

func normalInvCDF(mean, stddev, p float64) float64 {
	return mean - stddev*sqrt2*erfcInv(2*p)
}

// ------------------------------------------------------------------ Beta

func betaCDF(a, b, x float64) float64 {
	if x < 0 {
		return 0
	}
	if x >= 1 {
		return 1
	}
	if a == 0 && b == 0 {
		if x >= 0 && x < 1 {
			return 0.5
		}
		return 1
	}
	if a == 0 {
		return 1
	}
	if b == 0 {
		if x >= 1 {
			return 1
		}
		return 0
	}
	if a == 1 && b == 1 {
		return x
	}
	return betaRegularized(a, b, x)
}

func betaInvCDF(a, b, p float64) float64 {
	return findRoot(func(x float64) float64 { return betaRegularized(a, b, x) - p }, 0, 1, 1e-12)
}

// ------------------------------------------------------------------ Gamma

func gammaCDF(shape, rate, x float64) float64 {
	if shape == 0 && rate == 0 {
		return 0
	}
	return gammaLowerRegularized(shape, x*rate)
}

func gammaPDF(shape, rate, x float64) float64 {
	if shape == 0 && rate == 0 {
		return 0
	}
	if shape == 1 {
		return rate * math.Exp(-rate*x)
	}
	if x == 0 && shape > 1 {
		return 0
	}
	if shape > 160 {
		return math.Exp(gammaPDFLn(shape, rate, x))
	}
	return math.Pow(rate, shape) * math.Pow(x, shape-1) * math.Exp(-rate*x) / math.Gamma(shape)
}

func gammaPDFLn(shape, rate, x float64) float64 {
	if shape == 0 && rate == 0 {
		return math.Inf(-1)
	}
	if shape == 1 {
		return math.Log(rate) - rate*x
	}
	return shape*math.Log(rate) - gammaLn(shape) + (shape-1)*math.Log(x) - rate*x
}

func gammaInvCDF(shape, rate, p float64) float64 {
	return gammaLowerRegularizedInv(shape, p) / rate
}

// ------------------------------------------------------------------ StudentT

func studentTCDF(location, scale float64, freedom int, x float64) float64 {
	k := (x - location) / scale
	h := float64(freedom) / (float64(freedom) + k*k)
	ib := 0.5 * betaRegularized(float64(freedom)/2, 0.5, h)
	if x <= location {
		return ib
	}
	return 1 - ib
}

func studentTInvCDF(location, scale float64, freedom int, p float64) float64 {
	if p == 0.5 {
		return location
	}
	return findRoot(func(x float64) float64 {
		k := (x - location) / scale
		h := float64(freedom) / (float64(freedom) + k*k)
		ib := 0.5 * betaRegularized(float64(freedom)/2, 0.5, h)
		if x <= location {
			return ib - p
		}
		return 1 - ib - p
	}, -800, 800, 1e-12)
}

// ------------------------------------------------------------------ FisherSnedecor

func fisherSnedecorCDF(d1, d2 int, x float64) float64 {
	f1 := float64(d1)
	f2 := float64(d2)
	return betaRegularized(f1/2, f2/2, f1*x/(f1*x+f2))
}

func fisherSnedecorInvCDF(d1, d2 int, p float64) float64 {
	f1 := float64(d1)
	f2 := float64(d2)
	return findRoot(func(x float64) float64 {
		return betaRegularized(f1/2, f2/2, f1*x/(f1*x+f2)) - p
	}, 0, 1000, 1e-12)
}

// ------------------------------------------------------------------ 离散分布

func binomPMF(p float64, n, k int) float64 {
	if k < 0 || k > n {
		return 0
	}
	if p == 0 {
		if k == 0 {
			return 1
		}
		return 0
	}
	if p == 1 {
		if k == n {
			return 1
		}
		return 0
	}
	return math.Exp(binomialLn(n, k) + float64(k)*math.Log(p) + float64(n-k)*math.Log(1-p))
}

func binomCDF(p float64, n int, x float64) float64 {
	if x < 0 {
		return 0
	}
	if x > float64(n) {
		return 1
	}
	if p <= 0 {
		return 1
	}
	if p >= 1 {
		if x >= float64(n) {
			return 1
		}
		return 0
	}
	if x >= float64(n) {
		return 1
	}
	k := math.Floor(x)
	return betaRegularized(float64(n)-k, k+1, 1-p)
}

func hypergeomPMF(population, success, draws, k int) float64 {
	if k < 0 || k > draws || k > success || k < draws+success-population {
		return 0
	}
	// 与 C# 一致：使用对数组合数避免大数溢出
	ln := binomialLn(success, k) + binomialLn(population-success, draws-k) - binomialLn(population, draws)
	return math.Exp(ln)
}

func negativeBinomPMF(r, p float64, k int) float64 {
	ln := gammaLn(r+float64(k)) - gammaLn(r) - gammaLn(float64(k)+1) + r*math.Log(p) + float64(k)*math.Log(1-p)
	return math.Exp(ln)
}

// ------------------------------------------------------------------ 其它连续分布

func logNormalCDF(mu, sigma, x float64) float64 {
	if x < 0 {
		return 0
	}
	return 0.5 * (1 + math.Erf((math.Log(x)-mu)/(sigma*sqrt2)))
}

func logNormalInvCDF(mu, sigma, p float64) float64 {
	if p <= 0 {
		return 0
	}
	if p >= 1 {
		return math.Inf(1)
	}
	return math.Exp(mu - sigma*sqrt2*erfcInv(2*p))
}

func exponentialPDF(rate, x float64) float64 {
	if x < 0 {
		return 0
	}
	return rate * math.Exp(-rate*x)
}

func exponentialCDF(rate, x float64) float64 {
	if x < 0 {
		return 0
	}
	return 1 - math.Exp(-rate*x)
}

func weibullPDF(shape, scale, x float64) float64 {
	if x < 0 {
		return 0
	}
	if x == 0 && shape == 1 {
		return shape / scale
	}
	// 用 Pow(x/scale, shape) 避免 Pow(x,shape)*Pow(scale,-shape) 分别溢出/下溢
	return shape * math.Pow(x/scale, shape-1) * math.Exp(-math.Pow(x/scale, shape)) / scale
}

func weibullCDF(shape, scale, x float64) float64 {
	if x < 0 {
		return 0
	}
	return -math.Expm1(-math.Pow(x/scale, shape))
}
