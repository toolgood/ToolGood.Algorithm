package test

import (
	"testing"
)

// TestStrictModeTest 对应 csharp/ToolGood.Algorithm.Test/AlgorithmEngine/StrictModeTest.cs。
//
// C# 的 Cylinder 辅助类由 engine_test.go 中的 newCylinderEngine 承接（同一 package），
// 避免重复声明。
func TestStrictModeTest(t *testing.T) {
	// ------------------------------------------------------------------ 二元 && 运算符

	t.Run("and_strict_should_error_when_right_is_error", func(t *testing.T) {
		engine := newEngine()
		engine.UseStrictMode = true
		v := evalBoolDef(engine, "(1>2) && ERROR('test')", true)
		// 严格模式下，即使左边为 false，右边错误也会传播
		assertBool(t, true, v)
		assertLastError(t, engine)
	})

	t.Run("and_strict_all_valid", func(t *testing.T) {
		engine := newEngine()
		engine.UseStrictMode = true
		v := evalBoolDef(engine, "2>1 && 3>2", false)
		assertBool(t, true, v)
		assertNoError(t, engine)
	})

	t.Run("and_not_strict_short_circuit", func(t *testing.T) {
		engine := newEngine()
		engine.UseStrictMode = false
		// 非严格模式：1>2 为 false，短路不执行右边
		v := evalBoolDef(engine, "(1>2) && ERROR('test')", true)
		assertBool(t, false, v)
		assertNoError(t, engine)
	})

	t.Run("and_not_strict_error_on_left", func(t *testing.T) {
		engine := newEngine()
		engine.UseStrictMode = false
		// 非严格模式：左边错误无法短路，应报错
		v := evalBoolDef(engine, "ERROR('left') && (2>1)", true)
		assertBool(t, true, v)
		assertLastError(t, engine)
	})

	t.Run("and_not_strict_continue_when_left_true", func(t *testing.T) {
		engine := newEngine()
		engine.UseStrictMode = false
		// 非严格模式：左边为 true，继续执行右边
		v := evalBoolDef(engine, "(2>1) && ERROR('right')", true)
		assertBool(t, true, v)
		assertLastError(t, engine)
	})

	t.Run("and_not_strict_all_valid", func(t *testing.T) {
		engine := newEngine()
		engine.UseStrictMode = false
		v := evalBoolDef(engine, "2>1 && 3>2", false)
		assertBool(t, true, v)
		assertNoError(t, engine)
	})

	// ------------------------------------------------------------------ 二元 || 运算符

	t.Run("or_strict_should_error_when_right_is_error", func(t *testing.T) {
		engine := newEngine()
		engine.UseStrictMode = true
		// 严格模式下，即使左边为 true，右边错误也会传播
		v := evalBoolDef(engine, "(2>1) || ERROR('test')", false)
		assertBool(t, false, v)
		assertLastError(t, engine)
	})

	t.Run("or_strict_all_valid", func(t *testing.T) {
		engine := newEngine()
		engine.UseStrictMode = true
		v := evalBoolDef(engine, "1>2 || 3>2", false)
		assertBool(t, true, v)
		assertNoError(t, engine)
	})

	t.Run("or_not_strict_short_circuit", func(t *testing.T) {
		engine := newEngine()
		engine.UseStrictMode = false
		// 非严格模式：2>1 为 true，短路不执行右边
		v := evalBoolDef(engine, "(2>1) || ERROR('test')", false)
		assertBool(t, true, v)
		assertNoError(t, engine)
	})

	t.Run("or_not_strict_error_on_left", func(t *testing.T) {
		engine := newEngine()
		engine.UseStrictMode = false
		// 非严格模式：左边错误无法短路，应报错
		v := evalBoolDef(engine, "ERROR('left') || (2>1)", false)
		assertBool(t, false, v)
		assertLastError(t, engine)
	})

	t.Run("or_not_strict_continue_when_left_false", func(t *testing.T) {
		engine := newEngine()
		engine.UseStrictMode = false
		// 非严格模式：左边为 false，继续执行右边
		v := evalBoolDef(engine, "(1>2) || ERROR('right')", true)
		assertBool(t, true, v)
		assertLastError(t, engine)
	})

	t.Run("or_not_strict_all_valid", func(t *testing.T) {
		engine := newEngine()
		engine.UseStrictMode = false
		v := evalBoolDef(engine, "1>2 || 3>2", false)
		assertBool(t, true, v)
		assertNoError(t, engine)
	})

	// ------------------------------------------------------------------ n 元 AND() 函数

	t.Run("and_n_strict_should_error_when_any_param_is_error", func(t *testing.T) {
		engine := newEngine()
		engine.UseStrictMode = true
		v := evalBoolDef(engine, "AND(true(), true(), ERROR('test'))", true)
		assertBool(t, true, v)
		assertLastError(t, engine)
	})

	t.Run("and_n_strict_all_valid", func(t *testing.T) {
		engine := newEngine()
		engine.UseStrictMode = true
		v := evalBoolDef(engine, "AND(true(), 1=1, 3>2)", false)
		assertBool(t, true, v)
		assertNoError(t, engine)
	})

	t.Run("and_n_not_strict_short_circuit", func(t *testing.T) {
		engine := newEngine()
		engine.UseStrictMode = false
		// 非严格模式：遇到 false 就短路
		v := evalBoolDef(engine, "AND(true(), false(), ERROR('test'))", true)
		assertBool(t, false, v)
		assertNoError(t, engine)
	})

	t.Run("and_n_not_strict_error_before_short_circuit", func(t *testing.T) {
		engine := newEngine()
		engine.UseStrictMode = false
		// 非严格模式：错误在短路条件之前，应报错
		v := evalBoolDef(engine, "AND(ERROR('first'), false(), true())", true)
		assertBool(t, true, v)
		assertLastError(t, engine)
	})

	t.Run("and_n_not_strict_all_valid", func(t *testing.T) {
		engine := newEngine()
		engine.UseStrictMode = false
		v := evalBoolDef(engine, "AND(true(), 1=1, 3>2)", false)
		assertBool(t, true, v)
		assertNoError(t, engine)
	})

	// ------------------------------------------------------------------ n 元 OR() 函数

	t.Run("or_n_strict_should_error_when_any_param_is_error", func(t *testing.T) {
		engine := newEngine()
		engine.UseStrictMode = true
		v := evalBoolDef(engine, "OR(false(), false(), ERROR('test'))", true)
		assertBool(t, true, v)
		assertLastError(t, engine)
	})

	t.Run("or_n_strict_all_valid", func(t *testing.T) {
		engine := newEngine()
		engine.UseStrictMode = true
		v := evalBoolDef(engine, "OR(false(), 1=2, 3>2)", false)
		assertBool(t, true, v)
		assertNoError(t, engine)
	})

	t.Run("or_n_not_strict_short_circuit", func(t *testing.T) {
		engine := newEngine()
		engine.UseStrictMode = false
		// 非严格模式：遇到 true 就短路
		v := evalBoolDef(engine, "OR(false(), true(), ERROR('test'))", false)
		assertBool(t, true, v)
		assertNoError(t, engine)
	})

	t.Run("or_n_not_strict_error_before_short_circuit", func(t *testing.T) {
		engine := newEngine()
		engine.UseStrictMode = false
		// 非严格模式：错误在短路条件之前，应报错
		v := evalBoolDef(engine, "OR(ERROR('first'), true(), false())", true)
		assertBool(t, true, v)
		assertLastError(t, engine)
	})

	t.Run("or_n_not_strict_all_valid", func(t *testing.T) {
		engine := newEngine()
		engine.UseStrictMode = false
		v := evalBoolDef(engine, "OR(false(), 1=2, 3>2)", false)
		assertBool(t, true, v)
		assertNoError(t, engine)
	})

	// ------------------------------------------------------------------ 链式短路

	t.Run("chained_and_not_strict_short_circuit", func(t *testing.T) {
		engine := newEngine()
		engine.UseStrictMode = false
		// (1>2) && ... 短路，跳过 ERROR
		v := evalBoolDef(engine, "(1>2) && ERROR('a') && ERROR('b')", true)
		assertBool(t, false, v)
		assertNoError(t, engine)
	})

	t.Run("chained_or_not_strict_short_circuit", func(t *testing.T) {
		engine := newEngine()
		engine.UseStrictMode = false
		// (2>1) || ... 短路，跳过 ERROR
		v := evalBoolDef(engine, "(2>1) || ERROR('a') || ERROR('b')", false)
		assertBool(t, true, v)
		assertNoError(t, engine)
	})

	t.Run("chained_and_strict_all_evaluated", func(t *testing.T) {
		engine := newEngine()
		engine.UseStrictMode = true
		// 严格模式：所有条件都要求值，遇到错误报错
		v := evalBoolDef(engine, "(1>2) && (3>2) && ERROR('test')", true)
		assertBool(t, true, v)
		assertLastError(t, engine)
	})

	t.Run("chained_or_strict_all_evaluated", func(t *testing.T) {
		engine := newEngine()
		engine.UseStrictMode = true
		// 严格模式：所有条件都要求值，遇到错误报错
		v := evalBoolDef(engine, "(2>1) || (3>2) || ERROR('test')", false)
		assertBool(t, false, v)
		assertLastError(t, engine)
	})

	// ------------------------------------------------------------------ 默认行为

	t.Run("default_mode_is_strict", func(t *testing.T) {
		engine := newEngine()
		// 默认 UseStrictMode = true
		v := evalBoolDef(engine, "(2>1) || ERROR('test')", false)
		assertBool(t, false, v)
		assertLastError(t, engine)
	})
}
