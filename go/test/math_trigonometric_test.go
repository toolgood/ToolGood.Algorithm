package test

import (
	"math"
	"testing"
)

// TestMathTrigonometricTest 对应 C# ToolGood.Algorithm.Test.MathTrigonometricTest。
func TestMathTrigonometricTest(t *testing.T) {
	t.Run("degrees_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 180.0, evalFloat(e, "degrees(pi())"))
	})

	t.Run("RADIANS_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, math.Pi, evalFloat(e, "RADIANS(180)"), 10)
	})

	t.Run("cos_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.540302306, evalFloat(e, "cos(1)"), 6)
	})

	t.Run("cosh_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 1.543080635, evalFloat(e, "cosh(1)"), 6)
	})

	t.Run("sin_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.841470985, evalFloat(e, "sin(1)"), 6)
	})

	t.Run("sinh_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 1.175201194, evalFloat(e, "sinh(1)"), 6)
	})

	t.Run("tan_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 1.557407725, evalFloat(e, "tan(1)"), 6)
	})

	t.Run("tanh_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.761594156, evalFloat(e, "tanh(1)"), 6)
	})

	t.Run("acos_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 1.047197551, evalFloat(e, "acos(0.5)"), 6)
	})

	t.Run("acosh_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.96242365, evalFloat(e, "acosh(1.5)"), 6)
	})

	t.Run("asin_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.523598776, evalFloat(e, "asin(0.5)"), 6)
	})

	t.Run("asinh_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 1.194763217, evalFloat(e, "asinh(1.5)"), 6)
	})

	t.Run("atan_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.785398163, evalFloat(e, "atan(1)"), 6)
	})

	t.Run("atanh_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.549306144, evalFloat(e, "atanh(0.5)"), 6)
	})

	t.Run("atan2_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 1.107148718, evalFloat(e, "atan2(1,2)"), 6)
	})

	t.Run("cot_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.642092616, evalFloat(e, "cot(1)"), 6)
	})

	t.Run("coth_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 1.313035285, evalFloat(e, "coth(1)"), 6)
	})

	t.Run("csc_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 1.188395106, evalFloat(e, "csc(1)"), 6)
	})

	t.Run("csch_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.850918128, evalFloat(e, "csch(1)"), 6)
	})

	t.Run("sec_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 1.850815718, evalFloat(e, "sec(1)"), 6)
	})

	t.Run("sech_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.648054274, evalFloat(e, "sech(1)"), 6)
	})

	t.Run("acot_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.785398163, evalFloat(e, "acot(1)"), 6)
	})

	t.Run("acoth_test", func(t *testing.T) {
		e := newEngine()
		assertFloatRound(t, 0.549306144, evalFloat(e, "acoth(2)"), 6)
	})

	t.Run("hyperbolic_overflow_test", func(t *testing.T) {
		e := newEngine()

		// tanh/coth 大参数趋近 ±1
		assertFloat(t, 1.0, evalFloat(e, "tanh(1000)"))
		assertFloat(t, -1.0, evalFloat(e, "tanh(-1000)"))
		assertFloat(t, 1.0, evalFloat(e, "coth(1000)"))
		assertFloat(t, -1.0, evalFloat(e, "coth(-1000)"))

		// sech/csch 大参数趋近 0（默认值取 1.0，以便错误时能与期望的 0 区分）
		assertFloat(t, 0.0, evalFloatDef(e, "sech(1000)", 1.0))
		assertFloat(t, 0.0, evalFloatDef(e, "csch(1000)", 1.0))

		// sinh/cosh 大参数超出 decimal 范围，返回错误
		e = newEngine()
		evalFloat(e, "sinh(1000)")
		assertLastError(t, e)

		e = newEngine()
		evalFloat(e, "cosh(1000)")
		assertLastError(t, e)
	})
}
