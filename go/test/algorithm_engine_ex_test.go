package test

import (
	"testing"
	"time"

	alg "github.com/toolgood/ToolGood.Algorithm/go/algorithm"
)

// TestAlgorithmEngineExTest 移植自
// csharp/ToolGood.Algorithm.Test/AlgorithmEngineEx/AlgorithmEngineExTest.cs。
func TestAlgorithmEngineExTest(t *testing.T) {
	t.Run("Constructor_Default_Test", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(false)
		assertFalse(t, engine.IgnoreCase, "IgnoreCase should be false")
		assertFalse(t, engine.UseTempDict, "UseTempDict should be false")
	})

	t.Run("Constructor_IgnoreCase_Test", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(true)
		assertTrue(t, engine.IgnoreCase, "IgnoreCase should be true")

		engine = alg.NewAlgorithmEngineEx(false)
		assertFalse(t, engine.IgnoreCase, "IgnoreCase should be false")
	})

	t.Run("AddParameter_Bool_Test", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(false)
		engine.AddParameter("flag", true)
		assertBool(t, true, evalBool(engine.AlgorithmEngine, "flag"))

		engine.AddParameter("flag", false)
		assertBool(t, false, evalBoolDef(engine.AlgorithmEngine, "flag", true))
	})

	t.Run("AddParameter_Int_Test", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(false)
		engine.AddParameter("num", 42)
		assertInt(t, 42, evalInt(engine.AlgorithmEngine, "num"))
	})

	t.Run("AddParameter_Long_Test", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(false)
		engine.AddParameter("num", int64(123456789))
		// 差异：Go 版 TryEvaluate 没有 int64 默认值重载，用 int 求值结果比对。
		assertInt(t, 123456789, evalInt(engine.AlgorithmEngine, "num"))
	})

	t.Run("AddParameter_Double_Test", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(false)
		engine.AddParameter("num", 3.14159)
		assertFloatRound(t, 3.14159, evalFloat(engine.AlgorithmEngine, "num"), 5)
	})

	t.Run("AddParameter_Decimal_Test", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(false)
		engine.AddParameter("num", alg.MustDecimal("123.45"))
		assertDecimal(t, "123.45", evalDecimal(engine.AlgorithmEngine, "num"))
	})

	t.Run("AddParameter_String_Test", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(false)
		engine.AddParameter("name", "ToolGood")
		assertText(t, "ToolGood", evalText(engine.AlgorithmEngine, "name"))
	})

	t.Run("AddParameter_DateTime_Test", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(false)
		dt := time.Date(2024, 6, 15, 10, 30, 0, 0, time.UTC)
		engine.AddParameter("dt", dt)
		assertInt(t, 2024, evalInt(engine.AlgorithmEngine, "year(dt)"))
		assertInt(t, 6, evalInt(engine.AlgorithmEngine, "month(dt)"))
		assertInt(t, 15, evalInt(engine.AlgorithmEngine, "day(dt)"))
	})

	t.Run("AddParameter_TimeSpan_Test", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(false)
		// 差异：Go 无 TimeSpan，用「只有时间分量」的 MyDate 等价表示 10:30:00。
		ts := alg.NewMyDate(0, 0, 0, 10, 30, 0)
		engine.AddParameter("ti", ts)
		assertInt(t, 10, evalInt(engine.AlgorithmEngine, "hour(ti)"))
		assertInt(t, 30, evalInt(engine.AlgorithmEngine, "minute(ti)"))
	})

	t.Run("AddParameter_List_Test", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(false)
		list := []alg.Operand{alg.CreateInt(1), alg.CreateInt(2), alg.CreateInt(3)}
		engine.AddParameter("arr", list)
		assertInt(t, 3, evalInt(engine.AlgorithmEngine, "count(arr)"))
		assertInt(t, 1, evalInt(engine.AlgorithmEngine, "arr[1]"))
	})

	t.Run("AddParameter_Collection_String_Test", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(false)
		engine.AddParameter("names", alg.Create([]string{"a", "b", "c"}))
		// COUNT 只统计可转为数值的项,文本数组计数为 0
		assertInt(t, 0, evalInt(engine.AlgorithmEngine, "count(names)"))
		assertText(t, "a", evalText(engine.AlgorithmEngine, "names[1]"))
	})

	t.Run("AddParameter_Collection_Int_Test", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(false)
		engine.AddParameter("nums", alg.Create([]int{1, 2, 3, 4, 5}))
		assertInt(t, 15, evalInt(engine.AlgorithmEngine, "sum(nums)"))
	})

	t.Run("AddParameter_Collection_Double_Test", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(false)
		engine.AddParameter("nums", alg.Create([]float64{1.1, 2.2, 3.3}))
		assertFloatRound(t, 6.6, evalFloat(engine.AlgorithmEngine, "sum(nums)"), 2)
	})

	t.Run("AddParameter_Collection_Bool_Test", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(false)
		engine.AddParameter("flags", alg.Create([]bool{true, false, true}))
		assertInt(t, 3, evalInt(engine.AlgorithmEngine, "count(flags)"))
	})

	t.Run("AddParameter_Operand_Test", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(false)
		engine.AddParameter("val", alg.CreateInt(100))
		assertInt(t, 100, evalInt(engine.AlgorithmEngine, "val"))
	})

	t.Run("AddParameterFromJson_Test", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(false)
		if err := engine.AddParameterFromJson(`{"name":"test","age":25,"active":true}`); err != nil {
			t.Fatalf("AddParameterFromJson: %v", err)
		}
		assertText(t, "test", evalText(engine.AlgorithmEngine, "name"))
		assertInt(t, 25, evalInt(engine.AlgorithmEngine, "age"))
		assertBool(t, true, evalBool(engine.AlgorithmEngine, "active"))
	})

	t.Run("AddParameterFromJson_Complex_Test", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(false)
		if err := engine.AddParameterFromJson(`{"灰色":"L","canBookCount":905,"saleCount":91}`); err != nil {
			t.Fatalf("AddParameterFromJson: %v", err)
		}
		assertText(t, "L", evalText(engine.AlgorithmEngine, "灰色"))
		assertInt(t, 905, evalInt(engine.AlgorithmEngine, "canBookCount"))
	})

	t.Run("AddParameterFromJson_Invalid_Test", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(false)
		err := engine.AddParameterFromJson("not a json")
		assertNotNull(t, err)
	})

	t.Run("ClearParameters_Test", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(false)
		engine.AddParameter("num", 42)
		assertInt(t, 42, evalInt(engine.AlgorithmEngine, "num"))

		engine.ClearParameters()
		assertInt(t, -1, evalIntDef(engine.AlgorithmEngine, "num", -1))
	})

	t.Run("UseTempDict_Test", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(false)
		engine.UseTempDict = true
		engine.AddParameter("num", 42)
		assertInt(t, 42, evalInt(engine.AlgorithmEngine, "num"))
	})

	t.Run("IgnoreCase_True_Test", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(true)
		engine.AddParameter("Name", "Test")
		assertText(t, "Test", evalText(engine.AlgorithmEngine, "name"))
		assertText(t, "Test", evalText(engine.AlgorithmEngine, "NAME"))
		assertText(t, "Test", evalText(engine.AlgorithmEngine, "Name"))
	})

	t.Run("IgnoreCase_False_Test", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(false)
		engine.AddParameter("Name", "Test")
		assertText(t, "Test", evalText(engine.AlgorithmEngine, "Name"))
		assertText(t, "default", evalTextDef(engine.AlgorithmEngine, "name", "default"))
	})

	t.Run("GetParameterEx_Missing_Test", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(false)
		assertText(t, "default", evalTextDef(engine.AlgorithmEngine, "missing_param", "default"))
	})

	t.Run("Complex_Expression_Test", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(false)
		engine.AddParameter("a", 10)
		engine.AddParameter("b", 20)
		engine.AddParameter("c", 30)

		assertInt(t, 60, evalInt(engine.AlgorithmEngine, "a + b + c"))
		assertInt(t, 170, evalInt(engine.AlgorithmEngine, "a * b - c"))
		assertInt(t, 20, evalInt(engine.AlgorithmEngine, "if(a > 5, b, c)"))
	})

	t.Run("Nested_Json_Test", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(false)
		if err := engine.AddParameterFromJson(`{"data":{"name":"test","value":123}}`); err != nil {
			t.Fatalf("AddParameterFromJson: %v", err)
		}
		assertText(t, "test", evalText(engine.AlgorithmEngine, "json(data)['name']"))
	})

	t.Run("Array_Operations_Test", func(t *testing.T) {
		engine := alg.NewAlgorithmEngineEx(false)
		engine.AddParameter("arr", alg.Create([]int{1, 2, 3, 4, 5}))

		assertInt(t, 15, evalInt(engine.AlgorithmEngine, "sum(arr)"))
		assertFloatRound(t, 3.0, evalFloat(engine.AlgorithmEngine, "average(arr)"), 2)
		assertInt(t, 5, evalInt(engine.AlgorithmEngine, "max(arr)"))
		assertInt(t, 1, evalInt(engine.AlgorithmEngine, "min(arr)"))
		assertInt(t, 5, evalInt(engine.AlgorithmEngine, "count(arr)"))
	})
}
