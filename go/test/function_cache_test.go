package test

import (
	"testing"

	alg "github.com/toolgood/ToolGood.Algorithm/go/algorithm"
)

// TestFunctionCacheTest 移植自
// csharp/ToolGood.Algorithm.Test/FunctionCacheTest.cs。
func TestFunctionCacheTest(t *testing.T) {
	t.Run("ParseWithCache_SameFormula_ReturnsSameInstance", func(t *testing.T) {
		cache := alg.NewFunctionCache(nil)
		r1, err := cache.ParseWithCache("abs(1)")
		assertNull(t, err)
		r2, err := cache.ParseWithCache("abs(1)")
		assertNull(t, err)
		assertNotNull(t, r1)
		assertTrue(t, r1 == r2, "同一公式应返回同一实例")
	})

	t.Run("ParseWithCache_DifferentFormulas_ReturnsDifferentInstances", func(t *testing.T) {
		cache := alg.NewFunctionCache(nil)
		r1, err := cache.ParseWithCache("abs(1)")
		assertNull(t, err)
		r2, err := cache.ParseWithCache("abs(2)")
		assertNull(t, err)
		assertTrue(t, r1 != nil && r2 != nil, "两个结果都不应为空")
		assertTrue(t, r1 != r2, "不同公式应返回不同实例")
	})

	t.Run("ParseWithCache_StringLeaf_ReturnsFunctionBase", func(t *testing.T) {
		cache := alg.NewFunctionCache(nil)
		// 修复: String 叶子节点也写入缓存(修复前直接返回不缓存)
		r1, err := cache.ParseWithCache("'abc'")
		assertNull(t, err)
		r2, err := cache.ParseWithCache("'abc'")
		assertNull(t, err)
		assertNotNull(t, r1)
		assertTrue(t, r1 == r2, "字符串叶子也应命中缓存")
	})

	t.Run("ParseWithCache_NestedFunction_ReturnsFunctionBase", func(t *testing.T) {
		cache := alg.NewFunctionCache(nil)
		r, err := cache.ParseWithCache("max(1,2,3)")
		assertNull(t, err)
		assertNotNull(t, r)
	})

	t.Run("ParseWithCache_DiyExpression_ReturnsFunctionBase", func(t *testing.T) {
		cache := alg.NewFunctionCache(nil)
		r, err := cache.ParseWithCache("myVar")
		assertNull(t, err)
		assertNotNull(t, r)
	})

	t.Run("ParseConditionWithCache_SimpleCondition_ReturnsFunctionBase", func(t *testing.T) {
		cache := alg.NewFunctionCache(nil)
		r1, err := cache.ParseConditionWithCache("1=1")
		assertNull(t, err)
		r2, err := cache.ParseConditionWithCache("1=1")
		assertNull(t, err)
		assertNotNull(t, r1)
		assertTrue(t, r1 == r2, "同一条件应返回同一实例")
	})

	t.Run("ParseConditionWithCache_ComplexCondition_ReturnsFunctionBase", func(t *testing.T) {
		cache := alg.NewFunctionCache(nil)
		r, err := cache.ParseConditionWithCache("(1=1 || 2=2) && 3=3")
		assertNull(t, err)
		assertNotNull(t, r)
	})

	t.Run("ParseConditionWithCache_StringLeaf_NoReentrantGetOrAdd", func(t *testing.T) {
		cache := alg.NewFunctionCache(nil)
		// 修复: 条件表达式内的字符串叶子直接走计算树路径(CreateCalculate)
		r1, err := cache.ParseConditionWithCache("'abc' > ''")
		assertNull(t, err)
		r2, err := cache.ParseConditionWithCache("'abc' > ''")
		assertNull(t, err)
		assertNotNull(t, r1)
		assertTrue(t, r1 == r2, "字符串叶子条件应命中缓存")
	})

	t.Run("ParseConditionWithCache_FormulaInCondition_ReturnsFunctionBase", func(t *testing.T) {
		cache := alg.NewFunctionCache(nil)
		r, err := cache.ParseConditionWithCache("abs(1) > 0")
		assertNull(t, err)
		assertNotNull(t, r)
	})

	t.Run("ParseWithCache_And_ParseConditionWithCache_Independent", func(t *testing.T) {
		cache := alg.NewFunctionCache(nil)
		f1, err := cache.ParseWithCache("abs(1)")
		assertNull(t, err)
		f2, err := cache.ParseConditionWithCache("1=1")
		assertNull(t, err)
		assertTrue(t, f1 != nil && f2 != nil, "两个结果都不应为空")
		assertTrue(t, f1 != f2, "计算缓存与条件缓存应相互独立")
	})
}
