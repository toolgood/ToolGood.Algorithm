package rootfinding

import "math"

// Brent 求根法，对应 C# MathNet/RootFinding/Brent.cs。

// 与 C# MathNet/Precision.cs 一致的容差常数（子包不能反向依赖 mathnet 根包）。
const (
	decimalPrecision         = 0.0000000000000000000000000001
	positiveDecimalPrecision = 2 * decimalPrecision
	defaultDecimalAccuracy   = decimalPrecision * 10
	// decimalMinValue C# decimal.MinValue（xMid 初值）。
	decimalMinValue = -79228162514264337593543950335.0
)

// almostEqualRelative 相对误差比较（C# Precision.AlmostEqualRelative）。
func almostEqualRelative(a, b float64) bool {
	return almostEqualNormRelative(a, b, a-b, defaultDecimalAccuracy)
}

// almostEqualNormRelative 相对误差比较（C# Precision.AlmostEqualNormRelative）。
func almostEqualNormRelative(a, b, diff, maximumError float64) bool {
	if math.Abs(a) < decimalPrecision || math.Abs(b) < decimalPrecision {
		return math.Abs(diff) < maximumError
	}

	if (a == 0 && math.Abs(b) < maximumError) || (b == 0 && math.Abs(a) < maximumError) {
		return true
	}

	return math.Abs(diff) < maximumError*math.Max(math.Abs(a), math.Abs(b))
}

// FindRoot 在 [lowerBound, upperBound] 内求 f 的根，失败时 panic（对应 C# 抛 InvalidOperationException）。
func FindRoot(f func(float64) float64, lowerBound, upperBound, accuracy float64) float64 {
	if root, ok := TryFindRoot(f, lowerBound, upperBound, accuracy, 100); ok {
		return root
	}

	panic("RootFindingFailed")
}

// TryFindRoot 尝试求根，未收敛返回 false。
func TryFindRoot(f func(float64) float64, lowerBound, upperBound, accuracy float64, maxIterations int) (float64, bool) {
	fmin := f(lowerBound)
	fmax := f(upperBound)
	froot := fmax
	d := 0.0
	e := 0.0

	root := upperBound
	xMid := decimalMinValue

	if sign(fmin) == sign(fmax) {
		return 0, false
	}

	for i := 0; i <= maxIterations; i++ {
		if sign(froot) == sign(fmax) {
			upperBound = lowerBound
			fmax = fmin
			e = root - lowerBound
			d = e
		}

		if math.Abs(fmax) < math.Abs(froot) {
			lowerBound = root
			root = upperBound
			upperBound = lowerBound
			fmin = froot
			froot = fmax
			fmax = fmin
		}

		xAcc1 := positiveDecimalPrecision*math.Abs(root) + 0.5*accuracy
		xMidOld := xMid
		xMid = (upperBound - root) / 2.0

		if math.Abs(xMid) <= xAcc1 || almostEqualNormRelative(0, froot, froot, accuracy) {
			return root, true
		}

		if xMid == xMidOld {
			return 0, false
		}

		if math.Abs(e) >= xAcc1 && math.Abs(fmin) > math.Abs(froot) {
			s := froot / fmin
			var p, q float64
			if almostEqualRelative(lowerBound, upperBound) {
				p = 2.0 * xMid * s
				q = 1.0 - s
			} else {
				q = fmin / fmax
				r := froot / fmax
				p = s * (2.0*xMid*q*(q-r) - (root-lowerBound)*(r-1.0))
				q = (q - 1.0) * (r - 1.0) * (s - 1.0)
			}

			if p > 0.0 {
				q = -q
			}

			p = math.Abs(p)
			if 2.0*p < math.Min(3.0*xMid*q-math.Abs(xAcc1*q), math.Abs(e*q)) {
				e = d
				d = p / q
			} else {
				d = xMid
				e = d
			}
		} else {
			d = xMid
			e = d
		}

		lowerBound = root
		fmin = froot
		if math.Abs(d) > xAcc1 {
			root += d
		} else {
			root += brentSign(xAcc1, xMid)
		}

		froot = f(root)
	}

	return 0, false
}

// sign 对应 C# Math.Sign。
func sign(v float64) int {
	switch {
	case v > 0:
		return 1
	case v < 0:
		return -1
	default:
		return 0
	}
}

// brentSign 取 a 的绝对值，符号随 b。
func brentSign(a, b float64) float64 {
	if b >= 0 {
		if a >= 0 {
			return a
		}
		return -a
	}
	if a >= 0 {
		return -a
	}
	return a
}
