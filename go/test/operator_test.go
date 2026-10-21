package test

import (
	"testing"

	alg "github.com/toolgood/ToolGood.Algorithm/go/algorithm"
)

// operatorAssertRoundTrip 等价 C# OperatorTest.AssertRoundTrip：
// Parse(exp) → ToString 还原 → 重新 Parse 求值，要求结果值与是否错误完全一致。
// C# 版遇到第一处不一致即抛异常中止；这里用 Errorf 汇总全部不一致项，便于定位。
func operatorAssertRoundTrip(t *testing.T, exp string) {
	t.Helper()

	engine1 := newEngine()
	fn, err := engine1.Parse(exp)
	if err != nil {
		t.Errorf("往返测试失败: [%s] 解析失败: %v", exp, err)
		return
	}
	text := alg.ToString(fn)
	result1 := engine1.EvaluateSafe(fn)

	engine2 := newEngine()
	fn2, err := engine2.Parse(text)
	if err != nil {
		t.Errorf("往返测试失败: [%s] 还原为 [%s] 后重新解析失败: %v", exp, text, err)
		return
	}
	result2 := engine2.EvaluateSafe(fn2)

	if result1.IsError() != result2.IsError() || result1.String() != result2.String() {
		t.Errorf("往返测试失败: [%s] 还原为 [%s]，原结果=[%s](IsError=%v)，往返结果=[%s](IsError=%v)",
			exp, text, result1.String(), result1.IsError(), result2.String(), result2.IsError())
	}
}

// TestOperatorTest 对应 C# ToolGood.Algorithm.Test.OperatorTest。
func TestOperatorTest(t *testing.T) {
	t.Run("arithmetic_test", func(t *testing.T) {
		e := newEngine()
		assertInt(t, 5, evalInt(e, "1+(3*2+2)/2"))
		assertInt(t, 25, evalInt(e, "(8-3)*(3+2)"))
		assertInt(t, 4, evalInt(e, "(8-3)*(3+2) % 7"))

		assertInt(t, 5, evalInt(e, "2+3"))
		assertInt(t, 5, evalInt(e, "(2)+3"))
		assertInt(t, 28, evalInt(e, "2+3*2+10/2*4"))

		assertInt(t, 2110, evalInt(e, "2.1e3 + 10"))
		assertInt(t, 2110, evalInt(e, "2.1e+03 + 10"))
		assertInt(t, 2110, evalInt(e, "2.1e+3 + 10"))

		assertFloat(t, 10.0021, evalFloat(e, "2.1e-3 + 10"))
	})

	t.Run("connect_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "aabb", evalText(e, "'aa'&'bb'"))
		assertText(t, "5", evalText(e, "'3'+2"))
	})

	t.Run("conditional_test", func(t *testing.T) {
		e := newEngine()
		assertInt(t, 1, evalInt(e, "-7 < -2 ?1 : 2"))
		assertInt(t, 1, evalInt(e, "-7 < -2 ?1 ： 2"))
		assertInt(t, 1, evalInt(e, "-7 < -2 ?1 ： (7>1?3:2)"))
		assertInt(t, 1, evalInt(e, "-7 < -2 ?1 ：2"))
		assertInt(t, 1, evalInt(e, "-7 < -2 ？ 1 : 2"))
		assertInt(t, 1, evalInt(e, "-7 < -2 ？1 : 2"))
		assertInt(t, 1, evalInt(e, "-7 < -2 ？1 ： 2"))
		assertInt(t, 2, evalInt(e, "(!(-7 < -2))？1：2"))
		assertInt(t, 2, evalInt(e, "1>2？1：2"))
		assertInt(t, 1, evalInt(e, "1！=2？1：2"))
	})

	t.Run("percentage_test", func(t *testing.T) {
		e := newEngine()
		assertFloat(t, 1.0, evalFloat(e, "100%"))
		assertFloat(t, 0.5, evalFloat(e, "50%"))
		assertFloat(t, 2.0, evalFloat(e, "200%"))
		assertFloat(t, 50.0, evalFloat(e, "100*50%"))
		assertFloat(t, 100.5, evalFloat(e, "100+50%"))
	})

	t.Run("null_operation_test", func(t *testing.T) {
		e := newEngine()
		assertInt(t, 0, evalInt(e, "'111'*null"))
	})

	t.Run("overflow_test", func(t *testing.T) {
		e := newEngine()
		evalFloat(e, "79228162514264337593543950335 + 1")
		assertLastError(t, e)

		e = newEngine()
		evalFloat(e, "-79228162514264337593543950335 - 1")
		assertLastError(t, e)

		e = newEngine()
		evalFloat(e, "79228162514264337593543950335 * 2")
		assertLastError(t, e)

		e = newEngine()
		evalFloat(e, "79228162514264337593543950335 / 0.1")
		assertLastError(t, e)
	})

	t.Run("tostring_roundtrip_test", func(t *testing.T) {
		// Sub 减法非结合：右操作数为同优先级或更低优先级表达式时必须保留括号
		operatorAssertRoundTrip(t, "1 - (2 - 3)")
		operatorAssertRoundTrip(t, "10 - (2 + 3)")
		operatorAssertRoundTrip(t, "2 - (3 + 4)")
		operatorAssertRoundTrip(t, "(1 + 2) - (3 - 4)")
		operatorAssertRoundTrip(t, "1 - (2 - (3 - 4))")

		// Add / Connect：与连接运算符 & 及比较运算符混用
		operatorAssertRoundTrip(t, "1 + (2 & 3)")
		operatorAssertRoundTrip(t, "10 - (2 & 3)")
		operatorAssertRoundTrip(t, "(1 & 2) + 3")
		operatorAssertRoundTrip(t, "'a' & (1 - 2)")
		operatorAssertRoundTrip(t, "1 & (2 - 3)")
		operatorAssertRoundTrip(t, "(1 > 0) + 1")
		operatorAssertRoundTrip(t, "(1 > 0) & 'a'")

		// AND / OR：与低优先级逻辑运算符混用
		operatorAssertRoundTrip(t, "false() && (true() || true())")
		operatorAssertRoundTrip(t, "true() || (false() && true())")

		// 回归：乘除模与算术混合
		operatorAssertRoundTrip(t, "(2 + 3) * 4")
		operatorAssertRoundTrip(t, "8 / (2 * 2)")
		operatorAssertRoundTrip(t, "2 * (3 * 4)")
		operatorAssertRoundTrip(t, "1 + 2 * 3")
		operatorAssertRoundTrip(t, "(1 + 2) * 3")
		operatorAssertRoundTrip(t, "(1 - 2) * (3 - 4)")

		// 回归：字符串连接链
		operatorAssertRoundTrip(t, "'a' & 'b' & 'c'")
		operatorAssertRoundTrip(t, "'a' & ('b' & 'c')")

		// 回归：IF 嵌套
		operatorAssertRoundTrip(t, "true() && (1 > 0 ? true() : false())")
	})
}
