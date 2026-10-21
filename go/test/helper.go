// Package test 是 ToolGood.Algorithm Go 版的单元测试，
// 按 csharp/ToolGood.Algorithm.Test 的测试类一一对应拆分文件。
package test

import (
	"fmt"
	"math"
	"testing"

	alg "github.com/toolgood/ToolGood.Algorithm/go/algorithm"
)

// newEngine 创建默认引擎（与 C# new AlgorithmEngine() 等价）。
func newEngine() *alg.AlgorithmEngine {
	return alg.NewAlgorithmEngine(false)
}

// ------------------------------------------------------------------ 求值辅助

// evalOperand 求值并返回原始操作数，失败时返回错误操作数。
func evalOperand(e *alg.AlgorithmEngine, exp string) alg.Operand {
	fn := e.ParseWithoutError(exp)
	if fn == nil {
		return alg.Error("parse error: " + e.LastError)
	}
	return e.EvaluateSafe(fn)
}

// evalInt 等价 C# TryEvaluate(exp, 0)。
func evalInt(e *alg.AlgorithmEngine, exp string) int {
	return e.TryEvaluateInt(exp, 0)
}

// evalIntDef 等价 C# TryEvaluate(exp, defaultValue)。
func evalIntDef(e *alg.AlgorithmEngine, exp string, def int) int {
	return e.TryEvaluateInt(exp, def)
}

// evalFloat 等价 C# TryEvaluate(exp, 0.0)。
func evalFloat(e *alg.AlgorithmEngine, exp string) float64 {
	return e.TryEvaluateDouble(exp, 0)
}

// evalFloatDef 等价 C# TryEvaluate(exp, defaultValue)。
func evalFloatDef(e *alg.AlgorithmEngine, exp string, def float64) float64 {
	return e.TryEvaluateDouble(exp, def)
}

// evalText 等价 C# TryEvaluate(exp, "")。
func evalText(e *alg.AlgorithmEngine, exp string) string {
	return e.TryEvaluateText(exp, "")
}

// evalTextDef 等价 C# TryEvaluate(exp, defaultValue)。
func evalTextDef(e *alg.AlgorithmEngine, exp string, def string) string {
	return e.TryEvaluateText(exp, def)
}

// evalBool 等价 C# TryEvaluate(exp, false)。
func evalBool(e *alg.AlgorithmEngine, exp string) bool {
	return e.TryEvaluateBool(exp, false)
}

// evalBoolDef 等价 C# TryEvaluate(exp, defaultValue)。
func evalBoolDef(e *alg.AlgorithmEngine, exp string, def bool) bool {
	return e.TryEvaluateBool(exp, def)
}

// evalDecimal 等价 C# TryEvaluate(exp, 0m)。
func evalDecimal(e *alg.AlgorithmEngine, exp string) alg.Decimal {
	return e.TryEvaluateNumber(exp, alg.DecimalZero)
}

// evalDate 等价 C# TryEvaluate(exp, default(MyDate))。
func evalDate(e *alg.AlgorithmEngine, exp string) *alg.MyDate {
	return e.TryEvaluateMyDate(exp, nil)
}

// ------------------------------------------------------------------ 断言

func assertTrue(t *testing.T, cond bool, format string, args ...interface{}) {
	t.Helper()
	if !cond {
		t.Fatalf(format, args...)
	}
}

func assertFalse(t *testing.T, cond bool, format string, args ...interface{}) {
	t.Helper()
	if cond {
		t.Fatalf(format, args...)
	}
}

// assertInt 整数相等。
func assertInt(t *testing.T, want, got int) {
	t.Helper()
	if want != got {
		t.Fatalf("want %d, got %d", want, got)
	}
}

// assertInt64 64 位整数相等。
func assertInt64(t *testing.T, want, got int64) {
	t.Helper()
	if want != got {
		t.Fatalf("want %d, got %d", want, got)
	}
}

// assertFloat 浮点相等（默认 1e-9 容差）。
func assertFloat(t *testing.T, want, got float64) {
	t.Helper()
	if math.Abs(want-got) > 1e-9 {
		t.Fatalf("want %v, got %v", want, got)
	}
}

// assertFloatRound 先按 digits 位小数四舍五入再比较（对应 C# Math.Round(t, digits)）。
func assertFloatRound(t *testing.T, want, got float64, digits int) {
	t.Helper()
	scale := math.Pow(10, float64(digits))
	w := math.Round(want*scale) / scale
	g := math.Round(got*scale) / scale
	if math.Abs(w-g) > 1e-9 {
		t.Fatalf("want %v (rounded %d), got %v", want, digits, got)
	}
}

// assertText 字符串相等。
func assertText(t *testing.T, want, got string) {
	t.Helper()
	if want != got {
		t.Fatalf("want %q, got %q", want, got)
	}
}

// assertBool 布尔相等。
func assertBool(t *testing.T, want, got bool) {
	t.Helper()
	if want != got {
		t.Fatalf("want %v, got %v", want, got)
	}
}

// assertDecimal 十进制数值相等（按数值比较，忽略末尾 0）。
func assertDecimal(t *testing.T, want string, got alg.Decimal) {
	t.Helper()
	w := alg.MustDecimal(want)
	if !w.Equal(got) {
		t.Fatalf("want %s, got %s", want, got.String())
	}
}

// assertDecimalValue 十进制数值相等（传入期望值）。
func assertDecimalValue(t *testing.T, want, got alg.Decimal) {
	t.Helper()
	if !want.Equal(got) {
		t.Fatalf("want %s, got %s", want.String(), got.String())
	}
}

// assertNull 值应为 nil（对应 C# Assert.IsNull）。
func assertNull(t *testing.T, v interface{}) {
	t.Helper()
	if v != nil {
		t.Fatalf("want nil, got %#v", v)
	}
}

// assertNotNull 值不应为 nil（对应 C# Assert.IsNotNull）。
func assertNotNull(t *testing.T, v interface{}) {
	t.Helper()
	if v == nil {
		t.Fatalf("want not nil")
	}
}

// assertNoError 引擎不应有错误。
func assertNoError(t *testing.T, e *alg.AlgorithmEngine) {
	t.Helper()
	if e.LastError != "" {
		t.Fatalf("unexpected engine error: %s", e.LastError)
	}
}

// assertLastError 引擎应记录错误。
func assertLastError(t *testing.T, e *alg.AlgorithmEngine) {
	t.Helper()
	if e.LastError == "" {
		t.Fatalf("expected engine error, got none")
	}
}

// assertThrows 断言函数会 panic（对应 C# Assert.Throws / 越界异常）。
func assertThrows(t *testing.T, fn func()) {
	t.Helper()
	defer func() {
		if r := recover(); r == nil {
			t.Fatalf("expected panic, got none")
		}
	}()
	fn()
}

// assertContains 字符串包含子串。
func assertContains(t *testing.T, text, sub string) {
	t.Helper()
	if !contains(text, sub) {
		t.Fatalf("%q does not contain %q", text, sub)
	}
}

func contains(text, sub string) bool {
	return len(sub) == 0 || (len(text) >= len(sub) && indexOf(text, sub) >= 0)
}

func indexOf(text, sub string) int {
	for i := 0; i+len(sub) <= len(text); i++ {
		if text[i:i+len(sub)] == sub {
			return i
		}
	}
	return -1
}

// dumpOperand 调试输出：类型与文本。
func dumpOperand(o alg.Operand) string {
	if o == nil {
		return "<nil>"
	}
	return fmt.Sprintf("%s(%s)", o.Type().String(), o.String())
}
