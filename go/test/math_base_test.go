package test

import (
	"testing"
)

// TestMathBaseTest 对应 C# ToolGood.Algorithm.Test.MathBaseTest。
func TestMathBaseTest(t *testing.T) {
	t.Run("Pi_test", func(t *testing.T) {
		e := newEngine()
		v := evalFloat(e, "pi()")
		// 差异：PI 常量小数位与 C# 略有不同，按 9 位小数比较（同 C# Math.Round(t, 9)）
		assertFloatRound(t, 3.141592654, v, 9)
	})

	t.Run("abs_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 1.2, evalFloat(e, "abs(-1.2)"))
	})

	t.Run("QUOTIENT_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 2.0, evalFloat(e, "QUOTIENT(7,3)"))
	})

	t.Run("MOD_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 1.0, evalFloat(e, "MOD(7,3)"))
	})

	t.Run("SIGN_test", func(t *testing.T) {
		e := newEngine()
		assertInt(t, 0, evalInt(e, "SIGN(0)"))
		assertInt(t, 1, evalInt(e, "SIGN(9)"))
		assertInt(t, -1, evalInt(e, "SIGN(-9)"))
	})

	t.Run("SQRT_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 3.0, evalFloat(e, "SQRT(9)"))
	})

	t.Run("SUM_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 10.0, evalFloat(e, "SUM(1,2,3,4)"))
	})

	t.Run("TRUNC_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 9.0, evalFloat(e, "TRUNC(9.222)"))
		assertFloat(t, -9.0, evalFloat(e, "TRUNC(-9.222)"))
	})

	t.Run("TRUNC_ExcelCompatible_test", func(t *testing.T) {
		e := newEngine()

		assertFloat(t, 8.0, evalFloat(e, "TRUNC(8.9)"))
		assertFloat(t, 8.91, evalFloat(e, "TRUNC(8.912, 2)"))
		assertFloat(t, 8.9, evalFloat(e, "TRUNC(8.912, 1)"))
		assertFloat(t, -5.0, evalFloat(e, "TRUNC(-5.5)"))
		assertFloat(t, 100.0, evalFloat(e, "TRUNC(123.456, -2)"))
		assertFloat(t, 120.0, evalFloat(e, "TRUNC(123.456, -1)"))
		assertFloat(t, -100.0, evalFloat(e, "TRUNC(-123.456, -2)"))
		assertFloat(t, 0.0, evalFloat(e, "TRUNC(0)"))
		assertFloatRound(t, 3.1415, evalFloat(e, "TRUNC(PI(), 4)"), 4)
	})

	t.Run("int_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 9.0, evalFloat(e, "int(9.222)"))
		assertFloat(t, -10.0, evalFloat(e, "int(-9.222)"))
	})

	t.Run("GCD_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 1.0, evalFloat(e, "GCD(3,5,7)"))
		assertFloat(t, 3.0, evalFloat(e, "GCD(30,21)"))
	})

	t.Run("LCM_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 105.0, evalFloat(e, "LCM(3,5,7)"))
	})

	t.Run("combin_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 45.0, evalFloat(e, "combin(10,2)"))
	})

	t.Run("PERMUT_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 90.0, evalFloat(e, "PERMUT(10,2)"))
	})

	// ---------------------------------------------------------------- 四舍五入

	t.Run("ROUND_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 4.33, evalFloat(e, "ROUND(4.333,2)"))
	})

	t.Run("ROUND_single_param_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 5.0, evalFloat(e, "ROUND(4.5)"))
		assertFloat(t, 4.0, evalFloat(e, "ROUND(4.4)"))
		assertFloat(t, -5.0, evalFloat(e, "ROUND(-4.5)"))
	})

	t.Run("ROUND_negative_digits_test", func(t *testing.T) {
		e := newEngine()
		// 负数位数向左取整
		assertFloat(t, 1230.0, evalFloat(e, "ROUND(1234.567,-1)"))
		assertFloat(t, 1200.0, evalFloat(e, "ROUND(1234.567,-2)"))
		assertFloat(t, 1000.0, evalFloat(e, "ROUND(1234.567,-3)"))
		assertFloat(t, 0.0, evalFloat(e, "ROUND(1234.567,-4)"))
		// AwayFromZero 舍入
		assertFloat(t, 20.0, evalFloat(e, "ROUND(15,-1)"))
		// 超出 -15 报错
		evalFloat(e, "ROUND(1234.567,-16)")
		assertLastError(t, e)
	})

	t.Run("FACTDOUBLE_limit_test", func(t *testing.T) {
		e := newEngine()
		// 基础值
		v := evalFloat(e, "FACTDOUBLE(6)")
		assertFloat(t, 48.0, v)
		v = evalFloat(e, "FACTDOUBLE(7)")
		assertFloat(t, 105.0, v)

		// 29、45 在 decimal 范围内正常
		v = evalFloat(e, "FACTDOUBLE(29)")
		assertTrue(t, e.LastError == "" && v > 0, "FACTDOUBLE(29) 期望无错误且 >0，实际 %v, err=%q", v, e.LastError)

		v = evalFloat(e, "FACTDOUBLE(45)")
		assertTrue(t, e.LastError == "" && v > 0, "FACTDOUBLE(45) 期望无错误且 >0，实际 %v, err=%q", v, e.LastError)

		// 46!! 溢出，报错
		evalFloat(e, "FACTDOUBLE(46)")
		assertLastError(t, e)

		// 负数报错
		e = newEngine()
		evalFloat(e, "FACTDOUBLE(-1)")
		assertLastError(t, e)
	})

	t.Run("ROUNDDOWN_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 4.33, evalFloat(e, "ROUNDDOWN(4.333,2)"))
		assertFloat(t, -3.141, evalFloat(e, "ROUNDDOWN(-3.141592, 3)"))
	})

	t.Run("ROUNDUP_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 4.34, evalFloat(e, "ROUNDUP(4.333,2)"))
		assertFloat(t, -3.142, evalFloat(e, "ROUNDUP(-3.141592, 3)"))
	})

	t.Run("CEILING_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 4.4, evalFloat(e, "CEILING(4.333,0.1)"))
		assertFloat(t, 5, evalFloat(e, "CEILING(4.333)"))
	})

	t.Run("FLOOR_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 4.3, evalFloat(e, "FLOOR(4.363,0.1)"))
		assertFloat(t, 4, evalFloat(e, "FLOOR(4.333)"))
	})

	t.Run("even_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 6.0, evalFloat(e, "even(4.363)"))
	})

	t.Run("odd_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 5, evalFloat(e, "odd(4.363)"))
	})

	t.Run("MROUND_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 4, evalFloat(e, "MROUND(4.363,2)"))
		assertFloat(t, 6, evalFloat(e, "MROUND(5.363,2)"))
	})

	// ---------------------------------------------------------------- 随机数

	t.Run("Rand_test", func(t *testing.T) {
		e := newEngine()
		v := evalFloat(e, "RAND()")
		assertTrue(t, v >= 0, "RAND() 期望 >=0，实际 %v", v)
		assertTrue(t, v < 1, "RAND() 期望 <1，实际 %v", v)
	})

	t.Run("RANDBETWEEN_test", func(t *testing.T) {
		e := newEngine()
		v := evalFloat(e, "RANDBETWEEN(2,99)")
		assertTrue(t, v >= 2, "RANDBETWEEN(2,99) 期望 >=2，实际 %v", v)
		assertTrue(t, v <= 99, "RANDBETWEEN(2,99) 期望 <=99，实际 %v", v)
	})

	// ---------------------------------------------------------------- 幂/对数/阶乘

	t.Run("fact_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 720.0, evalFloat(e, "fact(6)"))
		assertFloat(t, 6.0, evalFloat(e, "fact(3)"))
	})

	t.Run("factdouble_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 3840.0, evalFloat(e, "factdouble(10)"))
	})

	t.Run("POWER_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 100.0, evalFloat(e, "POWER(10,2)"))
	})

	t.Run("exp_test", func(t *testing.T) {
		e := newEngine()
		// 差异：Go 用 float64 计算 EXP，C# 用 decimal 级数 → 按 6 位小数比较
		assertFloatRound(t, 7.389056099, evalFloat(e, "exp(2)"), 6)
	})

	t.Run("ln_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 1.386294361, evalFloat(e, "ln(4)"), 6)
	})

	t.Run("log_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 1.0, evalFloat(e, "log(10)"), 6)
		assertFloatRound(t, 3.0, evalFloat(e, "log(8,2)"), 6)
	})

	t.Run("log10_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 1.0, evalFloat(e, "log10(10)"), 6)
	})

	t.Run("MULTINOMIAL_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 60.0, evalFloat(e, "MULTINOMIAL(1,2,3)"), 6)
		assertFloatRound(t, 12600.0, evalFloat(e, "MULTINOMIAL(1,2,3,4)"), 6)
		assertFloatRound(t, 12600.0, evalFloat(e, "MULTINOMIAL(1,2,3,4.1)"), 6)
	})

	t.Run("PRODUCT_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 24.0, evalFloat(e, "PRODUCT(1,2,3,4)"), 6)
	})

	t.Run("SQRTPI_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 3.069980124, evalFloat(e, "SQRTPI(3)"), 6)
	})

	t.Run("SUMSQ_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 5.0, evalFloat(e, "SUMSQ(1,2)"), 6)
	})

	// ---------------------------------------------------------------- 工程函数

	t.Run("ERF_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.0, evalFloat(e, "ERF(0)"), 6)
		assertFloatRound(t, 0.842701, evalFloat(e, "ERF(1)"), 6)
		assertFloatRound(t, -0.842701, evalFloat(e, "ERF(-1)"), 6)
		assertFloatRound(t, 0.520499878, evalFloat(e, "ERF(0.5)"), 6)
		assertFloatRound(t, 0.999999985, evalFloat(e, "ERF(4)"), 6)
		assertFloatRound(t, 1, evalFloat(e, "ERF(5)"), 6)
		assertFloatRound(t, 1, evalFloat(e, "ERF(7)"), 6)
	})

	t.Run("ERFC_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 1.0, evalFloat(e, "ERFC(0)"), 6)
		assertFloatRound(t, 0.157299, evalFloat(e, "ERFC(1)"), 6)
		assertFloatRound(t, 1.99997791, evalFloat(e, "ERFC(-3)"), 6)
		assertFloatRound(t, 0.033894854, evalFloat(e, "ERFC(1.5)"), 6)
	})

	t.Run("DELTA_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 1.0, evalFloat(e, "DELTA(5, 5)"))
		assertFloat(t, 0.0, evalFloat(e, "DELTA(5, 4)"))
		assertFloat(t, 0.0, evalFloat(e, "DELTA(5)"))
		assertFloat(t, 1.0, evalFloat(e, "DELTA(0)"))
	})

	t.Run("GESTEP_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 1.0, evalFloat(e, "GESTEP(5, 4)"))
		assertFloat(t, 1.0, evalFloat(e, "GESTEP(5, 5)"))
		assertFloat(t, 0.0, evalFloat(e, "GESTEP(5, 6)"))
		assertFloat(t, 1.0, evalFloat(e, "GESTEP(5)"))
		assertFloat(t, 0.0, evalFloat(e, "GESTEP(-1)"))
	})

	// ---------------------------------------------------------------- 百分比

	t.Run("Percentage_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 0.5, evalFloat(e, "50%"))
		assertFloat(t, 1.0, evalFloat(e, "100%"))
		assertFloat(t, 0.01, evalFloat(e, "1%"))
		assertFloat(t, 0.0, evalFloat(e, "0%"))
		assertFloat(t, 2.0, evalFloat(e, "200%"))
		assertFloat(t, 0.255, evalFloat(e, "25.5%"))
	})

	// ---------------------------------------------------------------- 边界值

	t.Run("DivisionByZero_Test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 0, evalFloat(e, "1/0"))
		assertFloat(t, 0, evalFloat(e, "0/0"))
	})

	t.Run("Overflow_Test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 0, evalFloat(e, "99999999999999999999*99999999999999999999"))
	})

	t.Run("NullOperation_Test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 0, evalFloat(e, "null+1"))
		assertFloat(t, 0, evalFloat(e, "null*100"))
		assertFloat(t, 0, evalFloat(e, "null-null"))
	})

	t.Run("SquareRootNegative_Test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 0, evalFloat(e, "sqrt(-1)"))
	})

	t.Run("LogNegative_Test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 0, evalFloat(e, "log(-1)"))
		assertFloat(t, 0, evalFloat(e, "ln(0)"))
	})

	// ---------------------------------------------------------------- Excel 兼容性

	t.Run("ODD_ExcelCompatible_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 1.0, evalFloat(e, "ODD(1)"))
		assertFloat(t, 3.0, evalFloat(e, "ODD(2)"))
		assertFloat(t, 3.0, evalFloat(e, "ODD(3)"))
		assertFloat(t, -1.0, evalFloat(e, "ODD(-1)"))
		assertFloat(t, -3.0, evalFloat(e, "ODD(-2)"))
		assertFloat(t, -3.0, evalFloat(e, "ODD(-3)"))
		assertFloat(t, -5.0, evalFloat(e, "ODD(-4)"))
		assertFloat(t, 3.0, evalFloat(e, "ODD(1.5)"))
		assertFloat(t, -3.0, evalFloat(e, "ODD(-1.5)"))
	})

	t.Run("EVEN_ExcelCompatible_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 2.0, evalFloat(e, "EVEN(2)"))
		assertFloat(t, 2.0, evalFloat(e, "EVEN(1)"))
		assertFloat(t, -2.0, evalFloat(e, "EVEN(-1)"))
		assertFloat(t, -2.0, evalFloat(e, "EVEN(-2)"))
		assertFloat(t, -4.0, evalFloat(e, "EVEN(-3)"))
		assertFloat(t, 2.0, evalFloat(e, "EVEN(1.5)"))
		assertFloat(t, -2.0, evalFloat(e, "EVEN(-1.5)"))
		assertFloat(t, 0.0, evalFloat(e, "EVEN(0)"))
	})

	t.Run("CEILING_ExcelCompatible_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 3.0, evalFloat(e, "CEILING(2.5, 1)"))
		assertFloat(t, -3.0, evalFloat(e, "CEILING(-2.5, -1)"))
		assertFloat(t, -2.0, evalFloat(e, "CEILING(-2.5, 1)"))
		assertFloat(t, 1.5, evalFloat(e, "CEILING(1.5, 0.1)"))
		assertFloat(t, 0.24, evalFloat(e, "CEILING(0.234, 0.01)"))
	})

	t.Run("FLOOR_ExcelCompatible_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 3.0, evalFloat(e, "FLOOR(3.7, 1)"))
		assertFloat(t, -3.0, evalFloat(e, "FLOOR(-3.7, -1)"))
		assertFloat(t, -4.0, evalFloat(e, "FLOOR(-3.7, 1)"))
		assertFloat(t, 2.0, evalFloat(e, "FLOOR(3.7, 2)"))
		assertFloat(t, 0.0, evalFloat(e, "FLOOR(0, 1)"))
	})

	t.Run("MROUND_ExcelCompatible_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 9.0, evalFloat(e, "MROUND(10, 3)"))
		assertFloat(t, -9.0, evalFloat(e, "MROUND(-10, -3)"))
		assertFloat(t, 1.4, evalFloat(e, "MROUND(1.3, 0.2)"))
		assertFloat(t, -1.4, evalFloat(e, "MROUND(-1.3, -0.2)"))
	})

	t.Run("ROUNDDOWN_ExcelCompatible_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 3.0, evalFloat(e, "ROUNDDOWN(3.7, 0)"))
		assertFloat(t, -3.0, evalFloat(e, "ROUNDDOWN(-3.7, 0)"))
		assertFloat(t, 3.77, evalFloat(e, "ROUNDDOWN(3.777, 2)"))
		assertFloat(t, -3.77, evalFloat(e, "ROUNDDOWN(-3.777, 2)"))
		assertFloat(t, 70.0, evalFloat(e, "ROUNDDOWN(76.9, -1)"))
		assertFloat(t, -70.0, evalFloat(e, "ROUNDDOWN(-76.9, -1)"))
	})
}
