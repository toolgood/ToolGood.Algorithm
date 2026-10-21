package specialfunctions

import "math"

// BetaRegularized 返回正则化不完全 Beta 函数 I_x(a,b)。
func BetaRegularized(a, b, x float64) float64 {
	// a 或 b 为 0 属于退化情形，后续 bt*h/a 会出现 0/0，这里直接按极限值返回以避免除零
	if a == 0.0 {
		// I_x(0,b) = lim(a→0+) x^a = 0 (x=0) 或 1 (x>0)
		if x == 0.0 {
			return 0
		}
		return 1
	}

	if b == 0.0 {
		// I_x(a,0) = lim(b→0+) 1-(1-x)^b = 0 (x<1) 或 1 (x=1)
		if x == 1.0 {
			return 1
		}
		return 0
	}

	var bt float64
	if x == 0.0 || x == 1.0 {
		bt = 0.0
	} else {
		bt = math.Exp(GammaLn(a+b) - GammaLn(a) - GammaLn(b) + (a * math.Log(x)) + (b * math.Log(1.0-x)))
	}

	symmetryTransformation := x >= (a+1.0)/(a+b+2.0)

	eps := decimalPrecision

	if symmetryTransformation {
		x = 1.0 - x
		a, b = b, a
	}

	qab := a + b
	qap := a + 1.0
	qam := a - 1.0
	c := 1.0
	d := 1.0 - (qab * x / qap)

	if math.Abs(d) < eps {
		d = eps
	}

	d = 1.0 / d
	h := d

	for m, m2 := 1, 2; m <= 140; m, m2 = m+1, m2+2 {
		fm := float64(m)
		fm2 := float64(m2)
		aa := fm * (b - fm) * x / ((qam + fm2) * (a + fm2))
		d = 1.0 + (aa * d)

		if math.Abs(d) < eps {
			d = eps
		}

		c = 1.0 + (aa / c)
		if math.Abs(c) < eps {
			c = eps
		}

		d = 1.0 / d
		h *= d * c
		aa = -(a + fm) * (qab + fm) * x / ((a + fm2) * (qap + fm2))
		d = 1.0 + (aa * d)

		if math.Abs(d) < eps {
			d = eps
		}

		c = 1.0 + (aa / c)

		if math.Abs(c) < eps {
			c = eps
		}

		d = 1.0 / d
		del := d * c
		h *= del

		if math.Abs(del-1.0) <= eps {
			if symmetryTransformation {
				return 1.0 - (bt * h / a)
			}
			return bt * h / a
		}
	}

	if symmetryTransformation {
		return 1.0 - (bt * h / a)
	}
	return bt * h / a
}
