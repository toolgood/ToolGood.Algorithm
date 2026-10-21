package specialfunctions

import "math"

// 贝塞尔函数 BesselJ / BesselY，沿用原有 Numerical Recipes 式实现，
// 对应 C# MathNet/SpecialFunctions/Bessel.cs。

// BesselJ 返回第一类贝塞尔函数 J_n(x)。
func BesselJ(n int, x float64) float64 {
	if x == 0 {
		if n == 0 {
			return 1
		}
		return 0
	}

	if n < 0 {
		n = -n
	}

	ax := math.Abs(x)
	if ax < 1e-10 {
		if n == 0 {
			return 1
		}
		return 0
	}

	if n == 0 {
		return besselJ0(x)
	}
	if n == 1 {
		return besselJ1(x)
	}

	if ax > float64(n) {
		// 递推关系: J_{k+1}(x) = 2k/x * J_k(x) - J_{k-1}(x)
		j0 := besselJ0(x)
		j1 := besselJ1(x)
		var jn float64

		for k := 1; k < n; k++ {
			jn = (2.0*float64(k)/x)*j1 - j0
			j0 = j1
			j1 = jn
		}
		return j1
	}

	// Miller 算法(向下递推)计算 J_n
	m := int(1.5*float64(n) + 10)
	j := make([]float64, m+2)
	j[m+1] = 0
	j[m] = 1

	for k := m; k >= 1; k-- {
		j[k-1] = (2.0*float64(k)/x)*j[k] - j[k+1]
	}

	sum := 0.0
	for k := 0; k <= m; k += 2 {
		sum += 2.0 * j[k]
	}
	sum -= j[0]

	return j[n] / sum
}

// BesselY 返回第二类贝塞尔函数 Y_n(x)，要求 x > 0。
func BesselY(n int, x float64) float64 {
	if n < 0 {
		n = -n
	}

	if n == 0 {
		return besselY0(x)
	}
	if n == 1 {
		return besselY1(x)
	}

	// 递推关系: Y_{k+1}(x) = 2k/x * Y_k(x) - Y_{k-1}(x)
	y0 := besselY0(x)
	y1 := besselY1(x)
	var yn float64

	for k := 1; k < n; k++ {
		yn = (2.0*float64(k)/x)*y1 - y0
		y0 = y1
		y1 = yn
	}

	return y1
}

func besselJ0(x float64) float64 {
	ax := math.Abs(x)
	if ax < 8.0 {
		y1 := x * x
		ans1 := 57568490574.0 + y1*(-13362590354.0+y1*(651619640.7+
			y1*(-11214424.18+y1*(77392.33017+y1*(-184.9052456)))))
		ans2 := 57568490411.0 + y1*(1029532985.0+y1*(9494680.718+
			y1*(59272.64853+y1*(267.8532712+y1*1.0))))
		return ans1 / ans2
	}
	z := 8.0 / ax
	y2 := z * z
	xx := ax - 0.78539816339744830962
	ans3 := 1.0 + y2*(-0.1098628627e-2+y2*(0.2734510407e-4+
		y2*(-0.2073370639e-5+y2*0.2093887211e-6)))
	ans4 := -0.1562499995e-1 + y2*(0.1430488765e-3+
		y2*(-0.6911147651e-5+y2*(0.7621095161e-6-
			y2*0.934935152e-7)))
	return math.Sqrt(0.63661977236758134308/ax) * (math.Cos(xx)*ans3 - z*math.Sin(xx)*ans4)
}

func besselJ1(x float64) float64 {
	ax := math.Abs(x)
	if ax < 8.0 {
		y1 := x * x
		ans1 := x * (72362614232.0 + y1*(-7895059235.0+y1*(242396853.1+
			y1*(-2972611.439+y1*(15704.48260+y1*(-30.16036606))))))
		ans2 := 144725228442.0 + y1*(2300535178.0+y1*(18583304.74+
			y1*(99447.43394+y1*(376.9991397+y1*1.0))))
		return ans1 / ans2
	}
	z := 8.0 / ax
	y2 := z * z
	xx := ax - 2.35619449019234492885
	ans3 := 1.0 + y2*(0.183105e-2+y2*(-0.3516396496e-4+
		y2*(0.2457520174e-5+y2*(-0.240337019e-6))))
	ans4 := 0.04687499995 + y2*(-0.2002690873e-3+
		y2*(0.8449199096e-5+y2*(-0.88228987e-6+
			y2*0.105787412e-6)))
	ans := math.Sqrt(0.63661977236758134308/ax) * (math.Cos(xx)*ans3 - z*math.Sin(xx)*ans4)
	if x < 0 {
		return -ans
	}
	return ans
}

func besselY0(x float64) float64 {
	if x < 8.0 {
		y1 := x * x
		ans1 := -2957821389.0 + y1*(7062834065.0+y1*(-512359803.6+
			y1*(10879881.29+y1*(-86327.92757+y1*228.4622733))))
		ans2 := 40076544269.0 + y1*(745249964.8+y1*(7189466.438+
			y1*(47447.26470+y1*(226.1030244+y1*1.0))))
		return (ans1 / ans2) + 0.63661977236758134308*besselJ0(x)*math.Log(x)
	}
	z := 8.0 / x
	y2 := z * z
	xx := x - 0.78539816339744830962
	ans3 := 1.0 + y2*(-0.1098628627e-2+y2*(0.2734510407e-4+
		y2*(-0.2073370639e-5+y2*0.2093887211e-6)))
	ans4 := -0.1562499995e-1 + y2*(0.1430488765e-3+
		y2*(-0.6911147651e-5+y2*(0.7621095161e-6-
			y2*0.934935152e-7)))
	return math.Sqrt(0.63661977236758134308/x) * (math.Sin(xx)*ans3 + z*math.Cos(xx)*ans4)
}

func besselY1(x float64) float64 {
	if x < 8.0 {
		y1 := x * x
		ans1 := x * (-0.4900604943e13 + y1*(0.1275274390e13+
			y1*(-0.5153438139e11+y1*(0.7349264551e9+
				y1*(-0.4237922726e7+y1*0.8511937935e4)))))
		ans2 := 0.2499580570e14 + y1*(0.4244419664e12+
			y1*(0.3733650367e10+y1*(0.2245904002e8+
				y1*(0.1020426050e6+y1*(0.3549632885e3+y1)))))
		return (ans1 / ans2) + 0.63661977236758134308*(besselJ1(x)*math.Log(x)-1.0/x)
	}
	z := 8.0 / x
	y2 := z * z
	xx := x - 2.35619449019234492885
	ans3 := 1.0 + y2*(0.183105e-2+y2*(-0.3516396496e-4+
		y2*(0.2457520174e-5+y2*(-0.240337019e-6))))
	ans4 := 0.04687499995 + y2*(-0.2002690873e-3+
		y2*(0.8449199096e-5+y2*(-0.88228987e-6+
			y2*0.105787412e-6)))
	return math.Sqrt(0.63661977236758134308/x) * (math.Sin(xx)*ans3 + z*math.Cos(xx)*ans4)
}
