package specialfunctions

import "math"

// Gamma 函数相关实现，对应 C# MathNet/SpecialFunctions/Gamma.cs。

const (
	// gammaN 近似系数个数。
	gammaN = 10
	// gammaR 近似常数。
	gammaR = 10.900511
)

// gammaDk Gamma 近似系数。
var gammaDk = []float64{
	2.48574089138753565546e-5,
	1.05142378581721974210,
	-3.45687097222016235469,
	4.51227709466894823700,
	-2.98285225323576655721,
	1.05639711577126713077,
	-1.95428773191645869583e-1,
	1.70970543404441224307e-2,
	-5.71926117404305781283e-4,
	4.63399473359905636708e-6,
	-2.71994908488607703910e-9,
}

// GammaLn 返回 ln(Γ(z))。
func GammaLn(z float64) float64 {
	if z < 0.5 {
		s := gammaDk[0]
		for i := 1; i <= gammaN; i++ {
			s += gammaDk[i] / (float64(i) - z)
		}

		return lnPi -
			math.Log(math.Sin(math.Pi*z)) -
			math.Log(s) -
			logTwoSqrtEOverPi -
			((0.5 - z) * math.Log((0.5-z+gammaR)/math.E))
	}

	s := gammaDk[0]
	for i := 1; i <= gammaN; i++ {
		s += gammaDk[i] / (z + float64(i) - 1.0)
	}

	return math.Log(s) +
		logTwoSqrtEOverPi +
		((z - 0.5) * math.Log((z-0.5+gammaR)/math.E))
}

// GammaLowerRegularized 返回正则化下不完全 Gamma 函数 P(a,x)。
func GammaLowerRegularized(a, x float64) float64 {
	const epsilon = 0.000000000000001
	const big = 4503599627370496.0
	const bigInv = 2.22044604925031308085e-16

	if almostEqual(a, 0.0) {
		// C# 中 x≈0 与否则都返回 1
		return 1
	}

	if almostEqual(x, 0.0) {
		return 0
	}

	ax := (a * math.Log(x)) - x - GammaLn(a)
	if ax < -709.78271289338399 {
		if a < x {
			return 1
		}
		return 0
	}

	if x <= 1 || x <= a {
		r2 := a
		c2 := 1.0
		ans2 := 1.0

		for {
			r2 = r2 + 1
			c2 = c2 * x / r2
			ans2 += c2
			if !((c2 / ans2) > epsilon) {
				break
			}
		}

		return math.Exp(ax) * ans2 / a
	}

	c := 0
	y := 1 - a
	z := x + y + 1

	p3 := 1.0
	q3 := x
	p2 := x + 1
	q2 := z * x
	ans := p2 / q2

	var err float64

	for {
		c++
		y += 1
		z += 2
		yc := y * float64(c)

		p := (p2 * z) - (p3 * yc)
		q := (q2 * z) - (q3 * yc)

		if q != 0 {
			nextans := p / q
			err = math.Abs((ans - nextans) / nextans)
			ans = nextans
		} else {
			err = 1
		}

		p3 = p2
		p2 = p
		q3 = q2
		q2 = q

		if math.Abs(p) > big {
			p3 *= bigInv
			p2 *= bigInv
			q3 *= bigInv
			q2 *= bigInv
		}

		if !(err > epsilon) {
			break
		}
	}

	return 1 - (math.Exp(ax) * ans)
}

// GammaLowerRegularizedInv 返回 P(a,x) 的反函数。
func GammaLowerRegularizedInv(a, y0 float64) float64 {
	const epsilon = 0.000000000000001
	const big = 4503599627370496.0
	const threshold = 5 * epsilon

	if almostEqual(y0, 0.0) {
		return 0
	}

	if almostEqual(y0, 1.0) {
		return decimalMaxValue
	}

	y0 = 1 - y0

	xUpper := big
	xLower := 0.0
	yUpper := 1.0
	yLower := 0.0

	d := 1 / (9 * a)
	y := 1 - d - (0.98 * sqrt2 * ErfInv((2.0*y0)-1.0) * math.Sqrt(d))
	x := a * y * y * y
	lgm := GammaLn(a)

	for i := 0; i < 10; i++ {
		if x < xLower || x > xUpper {
			d = 0.0625
			break
		}

		y = 1 - GammaLowerRegularized(a, x)
		if y < yLower || y > yUpper {
			d = 0.0625
			break
		}

		if y < y0 {
			xUpper = x
			yLower = y
		} else {
			xLower = x
			yUpper = y
		}

		d = ((a - 1) * math.Log(x)) - x - lgm
		if d < -709.78271289338399 {
			d = 0.0625
			break
		}

		d = -math.Exp(d)
		d = (y - y0) / d
		if math.Abs(d/x) < epsilon {
			return x
		}

		if (d > (x / 4)) && (y0 < 0.05) {
			d = x / 10
		}

		x -= d
	}

	if xUpper == big {
		if x <= 0 {
			x = 1
		}

		for xUpper == big {
			x = (1 + d) * x
			y = 1 - GammaLowerRegularized(a, x)
			if y < y0 {
				xUpper = x
				yLower = y
				break
			}

			d = d + d
		}
	}

	dir := 0
	d = 0.5
	for i := 0; i < 400; i++ {
		x = xLower + (d * (xUpper - xLower))
		y = 1 - GammaLowerRegularized(a, x)
		lgm = (xUpper - xLower) / (xLower + xUpper)
		if math.Abs(lgm) < threshold {
			return x
		}

		lgm = (y - y0) / y0
		if math.Abs(lgm) < threshold {
			return x
		}

		if x <= 0 {
			return 0
		}

		if y >= y0 {
			xLower = x
			yUpper = y
			if dir < 0 {
				dir = 0
				d = 0.5
			} else {
				if dir > 1 {
					d = (0.5 * d) + 0.5
				} else {
					d = (y0 - yLower) / (yUpper - yLower)
				}
			}

			dir = dir + 1
		} else {
			xUpper = x
			yLower = y
			if dir > 0 {
				dir = 0
				d = 0.5
			} else {
				if dir < -1 {
					d = 0.5 * d
				} else {
					d = (y0 - yLower) / (yUpper - yLower)
				}
			}

			dir = dir - 1
		}
	}

	return x
}

// Gamma 返回 Γ(z)。
func Gamma(z float64) float64 {
	if z < 0.5 {
		s := gammaDk[0]
		for i := 1; i <= gammaN; i++ {
			s += gammaDk[i] / (float64(i) - z)
		}

		return math.Pi / (math.Sin(math.Pi*z) *
			s *
			twoSqrtEOverPi *
			math.Pow((0.5-z+gammaR)/math.E, 0.5-z))
	}

	s := gammaDk[0]
	for i := 1; i <= gammaN; i++ {
		s += gammaDk[i] / (z + float64(i) - 1.0)
	}

	return s * twoSqrtEOverPi * math.Pow((z-0.5+gammaR)/math.E, z-0.5)
}
