package test

import "testing"

// TestMathSum2Test 对应 C# ToolGood.Algorithm.Test.MathSum2.MathSum2Test。
//
// 差异：C# 的分布/特殊函数使用 decimal 级数计算，Go 版用 float64，
// 因此统一按 6 位小数比较（assertFloatRound），末位可能有 ±1 的偏差。
func TestMathSum2Test(t *testing.T) {
	t.Run("NORMSDIST_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.841344746, evalFloat(e, "NORMSDIST(1)"), 6)
	})

	t.Run("NORMDIST_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.105649774, evalFloat(e, "NORMDIST(3,8,4,1)"), 6)
		assertFloatRound(t, 0.045662271, evalFloat(e, "NORMDIST(3,8,4,0)"), 6)
	})

	t.Run("NORMINV_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 10.5248637, evalFloat(e, "NORMINV(0.8,8,3)"), 6)
	})

	t.Run("NORMSINV_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, -0.524400513, evalFloat(e, "NORMSINV(0.3)"), 6)
	})

	t.Run("BETADIST_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.97494877, evalFloat(e, "BETADIST(0.5,11,22)"), 6)
	})

	t.Run("BETAINV_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.336640759, evalFloat(e, "BETAINV(0.5,23,45)"), 6)
	})

	t.Run("BETAINV_param_count_test", func(t *testing.T) {
		e := newEngine()
		// 参数个数不足时,构造函数抛出异常并被引擎捕获,返回默认值而非空引用
		evalFloat(e, "BETAINV(0.5,23)")
		assertLastError(t, e)
	})

	t.Run("BINOMDIST_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.000817409, evalFloat(e, "BINOMDIST(12,45,0.5,0)"), 6)
		assertFloatRound(t, 0.00122945, evalFloat(e, "BINOMDIST(12,45,0.5,1)"), 6)
	})

	t.Run("EXPONDIST_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.049787068, evalFloat(e, "EXPONDIST(3,1,0)"), 6)
		assertFloatRound(t, 0.950212932, evalFloat(e, "EXPONDIST(3,1,1)"), 6)
	})

	t.Run("FDIST_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.701465776, evalFloat(e, "FDIST(0.4,2,3)"), 6)
	})

	t.Run("FINV_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.402651432, evalFloat(e, "FINV(0.7,2,3)"), 6)
	})

	t.Run("GAMMADIST_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.001723627, evalFloat(e, "GAMMADIST(0.5,3,4,0)"), 6)
		assertFloatRound(t, 0.000296478, evalFloat(e, "GAMMADIST(0.5,3,4,1)"), 6)
	})

	t.Run("GAMMAINV_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 6.140176811, evalFloat(e, "GAMMAINV(0.2,3,4)"), 6)
	})

	t.Run("GAMMALN_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 1.791759469, evalFloat(e, "GAMMALN(4)"), 6)
	})

	t.Run("HYPGEOMDIST_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.08715016, evalFloat(e, "HYPGEOMDIST(23,45,45,100)"), 6)
	})

	t.Run("LOGINV_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 15.01122624, evalFloat(e, "LOGINV(0.1,45,33)"), 6)
	})

	t.Run("LOGNORMDIST_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.326019201, evalFloat(e, "LOGNORMDIST(15,23,45)"), 6)
	})

	t.Run("NEGBINOMDIST_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.053463314, evalFloat(e, "NEGBINOMDIST(23,45,0.7)"), 6)
	})

	t.Run("POISSON_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.082884384, evalFloat(e, "POISSON(23,23,0)"), 6)
		assertFloatRound(t, 0.555149936, evalFloat(e, "POISSON(23,23,1)"), 6)
	})

	t.Run("TDIST_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.120925677, evalFloat(e, "TDIST(1.2,24,1)"), 6)
		assertFloatRound(t, 0.241851353, evalFloat(e, "TDIST(1.2,24,2)"), 6)
	})

	t.Run("TINV_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 1.614756561, evalFloat(e, "TINV(0.12,23)"), 6)
	})

	t.Run("WEIBULL_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.105160683, evalFloat(e, "WEIBULL(1,2,3,1)"), 6)
		assertFloatRound(t, 0.198853182, evalFloat(e, "WEIBULL(1,2,3,0)"), 6)
		// 以下为非法参数调用：C# 只要求不抛异常，不断言结果
		evalFloat(e, "WEIBULL(-1,2,3,0)")
		evalFloat(e, "WEIBULL(-1,-2,3,0)")
		evalFloat(e, "WEIBULL(-1,-2,-3,0)")
		evalFloat(e, "WEIBULL(-1,-2,-3,-1)")
	})

	t.Run("WEIBULL_tiny_x_cdf_test", func(t *testing.T) {
		e := newEngine()
		// 极小 x 时 CDF 应趋近于 0 且非负(修复前 ExponentialMinusOne 符号错误导致负值)
		got := evalFloat(e, "WEIBULL(1E-28,1,1,1)")
		assertNoError(t, e)
		assertTrue(t, got >= 0.0, "want >= 0, got %v", got)
	})

	t.Run("FISHER_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.8291140383, evalFloat(e, "FISHER(0.68)"), 6)
	})

	t.Run("FISHERINV_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.537049567, evalFloat(e, "FISHERINV(0.6)"), 6)
	})

	t.Run("FISHERINV_overflow_test", func(t *testing.T) {
		e := newEngine()
		// 2x 超出 decimal 的 e 指数范围(约 ±66)时,修复前抛 OverflowException
		// 修复后 tanh 趋近于 ±1,与 Java 版 double 计算一致
		assertFloat(t, 1.0, evalFloat(e, "FISHERINV(40)"))
		assertFloat(t, -1.0, evalFloat(e, "FISHERINV(-40)"))
		// 边界内仍正常计算
		assertFloatRound(t, 0.537049567, evalFloat(e, "FISHERINV(0.6)"), 6)
	})

	t.Run("BESSELI_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.981666, evalFloat(e, "BESSELI(1.5, 1)"), 6)
		assertFloatRound(t, 1.646723, evalFloat(e, "BESSELI(1.5, 0)"), 6)
	})

	t.Run("BESSELJ_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.557937, evalFloat(e, "BESSELJ(1.5, 1)"), 6)
		assertFloatRound(t, 0.511828, evalFloat(e, "BESSELJ(1.5, 0)"), 6)
	})

	t.Run("BESSELK_test", func(t *testing.T) {
		e := newEngine()
		// C# 断言容差 0.01，保留原精度要求（3 位小数 + 0.01 容差）
		assertFloatRound(t, 0.277, evalFloat(e, "BESSELK(1.5, 1)"), 3)
		assertFloatRound(t, 0.214, evalFloat(e, "BESSELK(1.5, 0)"), 3)
	})

	t.Run("BESSELY_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, -0.412, evalFloat(e, "BESSELY(1.5, 1)"), 3)
		assertFloatRound(t, 0.498, evalFloat(e, "BESSELY(2.5, 0)"), 3)
	})

	t.Run("BESSELK_overflow_test", func(t *testing.T) {
		e := newEngine()
		// 差异：C# 用 decimal 级数计算，x 过小时 K_n(x) 超出 decimal 范围而报错；
		// Go 用 float64，K_2(1E-28)≈2/x²=2e56 仍在 float64 范围内，故断言量级。
		got := evalFloat(e, "BESSELK(1E-28,2)")
		assertNoError(t, e)
		assertTrue(t, got > 1e56 && got < 3e56, "want ≈2e56, got %v", got)
	})

	t.Run("BESSELY_overflow_test", func(t *testing.T) {
		e := newEngine()
		// 差异：同上，C# 因 decimal 溢出报错；Go 的 float64 结果为 Y_2(1E-28)≈-4/π·1e56。
		got := evalFloat(e, "BESSELY(1E-28,2)")
		assertNoError(t, e)
		assertTrue(t, got < -1e56 && got > -2e56, "want ≈-1.27e56, got %v", got)
	})

	t.Run("BESSELJ_overflow_test", func(t *testing.T) {
		e := newEngine()
		// Miller 递推中间值超出 decimal 范围,应返回错误而非抛异常
		evalFloat(e, "BESSELJ(1E-10,100)")
		assertLastError(t, e)
	})
}
