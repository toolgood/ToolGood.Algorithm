package test

import (
	"strings"
	"testing"

	alg "github.com/toolgood/ToolGood.Algorithm/go/algorithm"
)

// TestOperandsTest 对应 C# ToolGood.Algorithm.Test.Operands.OperandsTest。
func TestOperandsTest(t *testing.T) {
	// ---------------------------------------------------------- Operand 静态属性测试
	t.Run("Operand_StaticProperties_Test", func(t *testing.T) {
		assertText(t, "ToolGood.Algorithm 6.3", alg.Version.TextValue())
		assertBool(t, true, alg.True.BooleanValue())
		assertBool(t, false, alg.False.BooleanValue())
		assertInt(t, 1, alg.One.IntValue())
		assertInt(t, 0, alg.Zero.IntValue())
		assertTrue(t, alg.Null.IsNull(), "want Null.IsNull()")
	})

	// ---------------------------------------------------------- Operand Create 测试
	t.Run("Operand_Create_Bool_Test", func(t *testing.T) {
		op := alg.Create(true)
		assertTrue(t, op.IsBoolean(), "want IsBoolean")
		assertBool(t, true, op.BooleanValue())

		op = alg.Create(false)
		assertTrue(t, op.IsBoolean(), "want IsBoolean")
		assertBool(t, false, op.BooleanValue())
	})

	t.Run("Operand_Create_Int_Test", func(t *testing.T) {
		op := alg.CreateInt(100)
		assertTrue(t, op.IsNumber(), "want IsNumber")
		assertInt(t, 100, op.IntValue())
		assertDecimal(t, "100", op.NumberValue())

		op = alg.CreateInt(-100)
		assertTrue(t, op.IsNumber(), "want IsNumber")
		assertInt(t, -100, op.IntValue())

		op = alg.CreateInt(0)
		assertTrue(t, op.IsNumber(), "want IsNumber")
		assertInt(t, 0, op.IntValue())

		op = alg.CreateInt(1000)
		assertTrue(t, op.IsNumber(), "want IsNumber")
		assertInt(t, 1000, op.IntValue())

		op = alg.CreateInt(-1000)
		assertTrue(t, op.IsNumber(), "want IsNumber")
		assertInt(t, -1000, op.IntValue())
	})

	t.Run("Operand_Create_Long_Test", func(t *testing.T) {
		op := alg.CreateInt64(100000)
		assertTrue(t, op.IsNumber(), "want IsNumber")
		assertInt64(t, 100000, op.LongValue())
	})

	t.Run("Operand_Create_Double_Test", func(t *testing.T) {
		op := alg.Create(3.14159)
		assertTrue(t, op.IsNumber(), "want IsNumber")
		// C# 容差 0.00001；Decimal 为任意精度，按 5 位小数比较
		assertFloatRound(t, 3.14159, op.DoubleValue(), 5)
	})

	t.Run("Operand_Create_Decimal_Test", func(t *testing.T) {
		op := alg.CreateDecimal(alg.MustDecimal("3.14159"))
		assertTrue(t, op.IsNumber(), "want IsNumber")
		assertDecimal(t, "3.14159", op.NumberValue())
	})

	t.Run("Operand_Create_String_Test", func(t *testing.T) {
		op := alg.CreateString("hello")
		assertTrue(t, op.IsText(), "want IsText")
		assertText(t, "hello", op.TextValue())

		op = alg.CreateString("")
		assertTrue(t, op.IsText(), "want IsText")
		assertText(t, "", op.TextValue())

		op = alg.Create(nil)
		assertTrue(t, op.IsNull(), "want IsNull")
	})

	t.Run("Operand_Create_DateTime_Test", func(t *testing.T) {
		dt := alg.NewMyDate(2024, 1, 15, 10, 30, 45)
		op := alg.CreateMyDate(dt)
		assertTrue(t, op.IsDate(), "want IsDate")
		assertInt(t, 2024, op.DateValue().Year)
		assertInt(t, 1, op.DateValue().Month)
		assertInt(t, 15, op.DateValue().Day)
		assertInt(t, 10, op.DateValue().Hour)
		assertInt(t, 30, op.DateValue().Minute)
		assertInt(t, 45, op.DateValue().Second)
	})

	t.Run("Operand_Create_TimeSpan_Test", func(t *testing.T) {
		// C# new TimeSpan(1, 2, 3, 4) = 1 天 2 时 3 分 4 秒
		ts := alg.NewMyDate(0, 0, 1, 2, 3, 4)
		op := alg.CreateMyDate(ts)
		assertTrue(t, op.IsDate(), "want IsDate")
		assertInt(t, 1, op.DateValue().Day)
		assertInt(t, 2, op.DateValue().Hour)
		assertInt(t, 3, op.DateValue().Minute)
		assertInt(t, 4, op.DateValue().Second)
	})

	t.Run("Operand_Create_List_Test", func(t *testing.T) {
		list := []alg.Operand{alg.CreateInt(1), alg.CreateInt(2), alg.CreateInt(3)}
		op := alg.CreateArray(list)
		assertTrue(t, op.IsArray(), "want IsArray")
		assertInt(t, 3, len(op.ArrayValue()))
		assertInt(t, 1, op.ArrayValue()[0].IntValue())
		assertInt(t, 2, op.ArrayValue()[1].IntValue())
		assertInt(t, 3, op.ArrayValue()[2].IntValue())
	})

	t.Run("Operand_Create_Collection_String_Test", func(t *testing.T) {
		op := alg.Create([]string{"a", "b", "c"})
		assertTrue(t, op.IsArray(), "want IsArray")
		assertInt(t, 3, len(op.ArrayValue()))
		assertText(t, "a", op.ArrayValue()[0].TextValue())
		assertText(t, "b", op.ArrayValue()[1].TextValue())
		assertText(t, "c", op.ArrayValue()[2].TextValue())
	})

	t.Run("Operand_Create_Collection_Int_Test", func(t *testing.T) {
		op := alg.Create([]int{1, 2, 3})
		assertTrue(t, op.IsArray(), "want IsArray")
		assertInt(t, 3, len(op.ArrayValue()))
		assertInt(t, 1, op.ArrayValue()[0].IntValue())
		assertInt(t, 2, op.ArrayValue()[1].IntValue())
		assertInt(t, 3, op.ArrayValue()[2].IntValue())
	})

	t.Run("Operand_Create_Collection_Double_Test", func(t *testing.T) {
		op := alg.Create([]float64{1.1, 2.2, 3.3})
		assertTrue(t, op.IsArray(), "want IsArray")
		assertInt(t, 3, len(op.ArrayValue()))
		assertFloatRound(t, 1.1, op.ArrayValue()[0].DoubleValue(), 3)
		assertFloatRound(t, 2.2, op.ArrayValue()[1].DoubleValue(), 3)
		assertFloatRound(t, 3.3, op.ArrayValue()[2].DoubleValue(), 3)
	})

	t.Run("Operand_Create_Collection_Bool_Test", func(t *testing.T) {
		op := alg.Create([]bool{true, false, true})
		assertTrue(t, op.IsArray(), "want IsArray")
		assertInt(t, 3, len(op.ArrayValue()))
		assertBool(t, true, op.ArrayValue()[0].BooleanValue())
		assertBool(t, false, op.ArrayValue()[1].BooleanValue())
		assertBool(t, true, op.ArrayValue()[2].BooleanValue())
	})

	t.Run("Operand_CreateJson_Test", func(t *testing.T) {
		op := alg.CreateJson(`{"name":"test","value":123}`)
		assertTrue(t, op.IsJson(), "want IsJson")

		op = alg.CreateJson("[1,2,3]")
		assertTrue(t, op.IsJson(), "want IsJson")

		op = alg.CreateJson("invalid json")
		assertTrue(t, op.IsError(), "want IsError")
	})

	t.Run("Operand_Error_Test", func(t *testing.T) {
		op := alg.Error("test error")
		assertTrue(t, op.IsError(), "want IsError")
		assertText(t, "test error", op.ErrorMsg())

		op = alg.Errorf("error {0}", "param")
		assertTrue(t, op.IsError(), "want IsError")
		assertText(t, "error param", op.ErrorMsg())
	})

	t.Run("Operand_CreateNull_Test", func(t *testing.T) {
		op := alg.Null
		assertTrue(t, op.IsNull(), "want IsNull")
	})

	// ---------------------------------------------------------- OperandInt 测试
	t.Run("OperandInt_Properties_Test", func(t *testing.T) {
		op := alg.CreateInt(42)
		assertTrue(t, op.Type() == alg.OperandTypeNumber, "want NUMBER, got %s", op.Type())
		assertTrue(t, op.IsNumber(), "want IsNumber")
		assertFalse(t, op.IsNull(), "want !IsNull")
		assertFalse(t, op.IsText(), "want !IsText")
		assertFalse(t, op.IsBoolean(), "want !IsBoolean")
		assertFalse(t, op.IsArray(), "want !IsArray")
		assertFalse(t, op.IsDate(), "want !IsDate")
		assertFalse(t, op.IsJson(), "want !IsJson")
		assertFalse(t, op.IsError(), "want !IsError")
	})

	t.Run("OperandInt_ToNumber_Test", func(t *testing.T) {
		op := alg.CreateInt(42)
		result := op.ToNumber("")
		assertTrue(t, result.IsNumber(), "want IsNumber")
		assertInt(t, 42, result.IntValue())
	})

	t.Run("OperandInt_ToBoolean_Test", func(t *testing.T) {
		op := alg.CreateInt(0)
		result := op.ToBoolean("")
		assertTrue(t, result.IsBoolean(), "want IsBoolean")
		assertBool(t, false, result.BooleanValue())

		op = alg.CreateInt(1)
		result = op.ToBoolean("")
		assertTrue(t, result.IsBoolean(), "want IsBoolean")
		assertBool(t, true, result.BooleanValue())

		op = alg.CreateInt(2)
		result = op.ToBoolean("")
		assertTrue(t, result.IsError(), "want IsError")
	})

	t.Run("OperandInt_ToText_Test", func(t *testing.T) {
		op := alg.CreateInt(42)
		result := op.ToText("")
		assertTrue(t, result.IsText(), "want IsText")
		assertText(t, "42", result.TextValue())
	})

	t.Run("OperandInt_ToString_Test", func(t *testing.T) {
		op := alg.CreateInt(42)
		assertText(t, "42", op.String())
	})

	// ---------------------------------------------------------- OperandDouble 测试
	t.Run("OperandDouble_Properties_Test", func(t *testing.T) {
		op := alg.Create(3.14)
		assertTrue(t, op.Type() == alg.OperandTypeNumber, "want NUMBER, got %s", op.Type())
		assertTrue(t, op.IsNumber(), "want IsNumber")
		assertFloatRound(t, 3.14, op.DoubleValue(), 3)
		assertDecimal(t, "3.14", op.NumberValue())
		assertInt(t, 3, op.IntValue())
		assertInt64(t, 3, op.LongValue())
	})

	t.Run("OperandDouble_ToBoolean_Test", func(t *testing.T) {
		op := alg.Create(0.0)
		result := op.ToBoolean("")
		assertTrue(t, result.IsBoolean(), "want IsBoolean")
		assertBool(t, false, result.BooleanValue())

		op = alg.Create(1.0)
		result = op.ToBoolean("")
		assertTrue(t, result.IsBoolean(), "want IsBoolean")
		assertBool(t, true, result.BooleanValue())

		op = alg.Create(2.5)
		result = op.ToBoolean("")
		assertTrue(t, result.IsError(), "want IsError")
	})

	t.Run("OperandDouble_ToText_Test", func(t *testing.T) {
		op := alg.Create(3.14)
		result := op.ToText("")
		assertTrue(t, result.IsText(), "want IsText")
		assertText(t, "3.14", result.TextValue())
	})

	// ---------------------------------------------------------- OperandDecimal 测试
	t.Run("OperandDecimal_Properties_Test", func(t *testing.T) {
		op := alg.CreateDecimal(alg.MustDecimal("123.456"))
		assertTrue(t, op.Type() == alg.OperandTypeNumber, "want NUMBER, got %s", op.Type())
		assertTrue(t, op.IsNumber(), "want IsNumber")
		assertDecimal(t, "123.456", op.NumberValue())
		assertFloatRound(t, 123.456, op.DoubleValue(), 3)
		assertInt(t, 123, op.IntValue())
	})

	t.Run("OperandDecimal_ToBoolean_Test", func(t *testing.T) {
		op := alg.CreateDecimal(alg.MustDecimal("0"))
		result := op.ToBoolean("")
		assertTrue(t, result.IsBoolean(), "want IsBoolean")
		assertBool(t, false, result.BooleanValue())

		op = alg.CreateDecimal(alg.MustDecimal("1"))
		result = op.ToBoolean("")
		assertTrue(t, result.IsBoolean(), "want IsBoolean")
		assertBool(t, true, result.BooleanValue())
	})

	t.Run("OperandDecimal_ToText_Test", func(t *testing.T) {
		op := alg.CreateDecimal(alg.MustDecimal("123.45"))
		result := op.ToText("")
		assertTrue(t, result.IsText(), "want IsText")
		assertText(t, "123.45", result.TextValue())
	})

	// ---------------------------------------------------------- OperandBoolean 测试
	t.Run("OperandBoolean_Properties_Test", func(t *testing.T) {
		op := alg.Create(true)
		assertTrue(t, op.Type() == alg.OperandTypeBoolean, "want BOOLEAN, got %s", op.Type())
		assertTrue(t, op.IsBoolean(), "want IsBoolean")
		assertFalse(t, op.IsNumber(), "want !IsNumber")
		assertBool(t, true, op.BooleanValue())

		op = alg.Create(false)
		assertTrue(t, op.Type() == alg.OperandTypeBoolean, "want BOOLEAN, got %s", op.Type())
		assertTrue(t, op.IsBoolean(), "want IsBoolean")
		assertBool(t, false, op.BooleanValue())
	})

	t.Run("OperandBoolean_ToNumber_Test", func(t *testing.T) {
		op := alg.Create(true)
		result := op.ToNumber("")
		assertTrue(t, result.IsNumber(), "want IsNumber")
		assertInt(t, 1, result.IntValue())

		op = alg.Create(false)
		result = op.ToNumber("")
		assertTrue(t, result.IsNumber(), "want IsNumber")
		assertInt(t, 0, result.IntValue())
	})

	t.Run("OperandBoolean_ToText_Test", func(t *testing.T) {
		op := alg.Create(true)
		result := op.ToText("")
		assertTrue(t, result.IsText(), "want IsText")
		assertText(t, "TRUE", result.TextValue())

		op = alg.Create(false)
		result = op.ToText("")
		assertTrue(t, result.IsText(), "want IsText")
		assertText(t, "FALSE", result.TextValue())
	})

	t.Run("OperandBoolean_ToString_Test", func(t *testing.T) {
		op := alg.Create(true)
		assertText(t, "true", op.String())

		op = alg.Create(false)
		assertText(t, "false", op.String())
	})

	// ---------------------------------------------------------- OperandString 测试
	t.Run("OperandString_Properties_Test", func(t *testing.T) {
		op := alg.CreateString("hello world")
		assertTrue(t, op.Type() == alg.OperandTypeText, "want TEXT, got %s", op.Type())
		assertTrue(t, op.IsText(), "want IsText")
		assertFalse(t, op.IsNumber(), "want !IsNumber")
		assertFalse(t, op.IsBoolean(), "want !IsBoolean")
		assertText(t, "hello world", op.TextValue())
	})

	t.Run("OperandString_ToNumber_Test", func(t *testing.T) {
		op := alg.CreateString("42")
		result := op.ToNumber("")
		assertTrue(t, result.IsNumber(), "want IsNumber")
		assertInt(t, 42, result.IntValue())

		op = alg.CreateString("3.14")
		result = op.ToNumber("")
		assertTrue(t, result.IsNumber(), "want IsNumber")
		assertFloatRound(t, 3.14, result.DoubleValue(), 3)

		op = alg.CreateString("not a number")
		result = op.ToNumber("")
		assertTrue(t, result.IsError(), "want IsError")
	})

	t.Run("OperandString_ToBoolean_Test", func(t *testing.T) {
		op := alg.CreateString("true")
		result := op.ToBoolean("")
		assertTrue(t, result.IsBoolean(), "want IsBoolean")
		assertBool(t, true, result.BooleanValue())

		op = alg.CreateString("false")
		result = op.ToBoolean("")
		assertTrue(t, result.IsBoolean(), "want IsBoolean")
		assertBool(t, false, result.BooleanValue())

		op = alg.CreateString("TRUE")
		result = op.ToBoolean("")
		assertTrue(t, result.IsBoolean(), "want IsBoolean")
		assertBool(t, true, result.BooleanValue())

		op = alg.CreateString("not bool")
		result = op.ToBoolean("")
		assertTrue(t, result.IsError(), "want IsError")
	})

	t.Run("OperandString_ToMyDate_Test", func(t *testing.T) {
		op := alg.CreateString("2024-01-15")
		result := op.ToMyDate("")
		assertTrue(t, result.IsDate(), "want IsDate")
		assertInt(t, 2024, result.DateValue().Year)
		assertInt(t, 1, result.DateValue().Month)
		assertInt(t, 15, result.DateValue().Day)

		op = alg.CreateString("10:30:45")
		result = op.ToMyDate("")
		assertTrue(t, result.IsDate(), "want IsDate")
		assertInt(t, 10, result.DateValue().Hour)
		assertInt(t, 30, result.DateValue().Minute)
		assertInt(t, 45, result.DateValue().Second)

		op = alg.CreateString("not a date")
		result = op.ToMyDate("")
		assertTrue(t, result.IsError(), "want IsError")
	})

	t.Run("OperandString_ToJson_Test", func(t *testing.T) {
		op := alg.CreateString(`{"name":"test"}`)
		result := op.ToJson("")
		assertTrue(t, result.IsJson(), "want IsJson")

		op = alg.CreateString("[1,2,3]")
		result = op.ToJson("")
		assertTrue(t, result.IsJson(), "want IsJson")

		op = alg.CreateString("not json")
		result = op.ToJson("")
		assertTrue(t, result.IsError(), "want IsError")
	})

	t.Run("OperandString_ToArray_Test", func(t *testing.T) {
		op := alg.CreateString("test")
		result := op.ToArray("")
		assertTrue(t, result.IsError(), "want IsError")
	})

	t.Run("OperandString_ToString_Test", func(t *testing.T) {
		op := alg.CreateString("hello")
		assertText(t, `"hello"`, op.String())

		op = alg.CreateString("hello\"world")
		assertText(t, `"hello\"world"`, op.String())

		op = alg.CreateString("hello\nworld")
		assertText(t, `"hello\nworld"`, op.String())

		op = alg.CreateString("hello\tworld")
		assertText(t, `"hello\tworld"`, op.String())
	})

	// ---------------------------------------------------------- OperandNull 测试
	t.Run("OperandNull_Properties_Test", func(t *testing.T) {
		op := alg.Null
		assertTrue(t, op.Type() == alg.OperandTypeNull, "want NULL, got %s", op.Type())
		assertTrue(t, op.IsNull(), "want IsNull")
		assertFalse(t, op.IsNumber(), "want !IsNumber")
		assertFalse(t, op.IsText(), "want !IsText")
		assertFalse(t, op.IsBoolean(), "want !IsBoolean")
		assertFalse(t, op.IsError(), "want !IsError")
	})

	t.Run("OperandNull_ToString_Test", func(t *testing.T) {
		op := alg.Null
		assertText(t, "null", op.String())
	})

	// ---------------------------------------------------------- OperandError 测试
	t.Run("OperandError_Properties_Test", func(t *testing.T) {
		op := alg.Error("test error message")
		assertTrue(t, op.Type() == alg.OperandTypeError, "want ERROR, got %s", op.Type())
		assertTrue(t, op.IsError(), "want IsError")
		assertFalse(t, op.IsNull(), "want !IsNull")
		assertFalse(t, op.IsNumber(), "want !IsNumber")
		assertFalse(t, op.IsText(), "want !IsText")
		assertText(t, "test error message", op.ErrorMsg())
	})

	t.Run("OperandError_Conversions_Test", func(t *testing.T) {
		op := alg.Error("error")

		assertTrue(t, op.ToNumber("").IsError(), "want ToNumber IsError")
		assertTrue(t, op.ToBoolean("").IsError(), "want ToBoolean IsError")
		assertTrue(t, op.ToText("").IsError(), "want ToText IsError")
		assertTrue(t, op.ToArray("").IsError(), "want ToArray IsError")
		assertTrue(t, op.ToMyDate("").IsError(), "want ToMyDate IsError")
		assertTrue(t, op.ToJson("").IsError(), "want ToJson IsError")
	})

	// ---------------------------------------------------------- OperandArray 测试
	t.Run("OperandArray_Properties_Test", func(t *testing.T) {
		list := []alg.Operand{alg.CreateInt(1), alg.CreateInt(2), alg.CreateInt(3)}
		op := alg.CreateArray(list)
		assertTrue(t, op.Type() == alg.OperandTypeArray, "want ARRAY, got %s", op.Type())
		assertTrue(t, op.IsArray(), "want IsArray")
		assertFalse(t, op.IsNumber(), "want !IsNumber")
		assertFalse(t, op.IsText(), "want !IsText")
		assertInt(t, 3, len(op.ArrayValue()))
	})

	t.Run("OperandArray_ToText_Test", func(t *testing.T) {
		list := []alg.Operand{alg.CreateInt(1), alg.CreateInt(2), alg.CreateInt(3)}
		op := alg.CreateArray(list)
		result := op.ToText("")
		assertTrue(t, result.IsText(), "want IsText")
		assertText(t, "[1,2,3]", result.TextValue())
	})

	t.Run("OperandArray_ToArray_Test", func(t *testing.T) {
		list := []alg.Operand{alg.CreateInt(1), alg.CreateInt(2)}
		op := alg.CreateArray(list)
		result := op.ToArray("")
		assertTrue(t, result.IsArray(), "want IsArray")
		assertInt(t, 2, len(result.ArrayValue()))
	})

	t.Run("OperandArray_ToJson_Test", func(t *testing.T) {
		list := []alg.Operand{alg.CreateInt(1), alg.CreateInt(2)}
		op := alg.CreateArray(list)
		result := op.ToJson("")
		assertTrue(t, result.IsJson(), "want IsJson")
	})

	t.Run("OperandArray_ToString_Test", func(t *testing.T) {
		list := []alg.Operand{alg.CreateInt(1), alg.CreateString("test"), alg.Create(true)}
		op := alg.CreateArray(list)
		assertText(t, `[1,"test",true]`, op.String())
	})

	// ---------------------------------------------------------- OperandMyDate 测试
	t.Run("OperandMyDate_Properties_Test", func(t *testing.T) {
		dt := alg.NewMyDate(2024, 6, 15, 10, 30, 45)
		op := alg.CreateMyDate(dt)
		assertTrue(t, op.Type() == alg.OperandTypeDate, "want DATE, got %s", op.Type())
		assertTrue(t, op.IsDate(), "want IsDate")
		assertFalse(t, op.IsNumber(), "want !IsNumber")
		assertFalse(t, op.IsText(), "want !IsText")
		assertInt(t, 2024, op.DateValue().Year)
		assertInt(t, 6, op.DateValue().Month)
		assertInt(t, 15, op.DateValue().Day)
		assertInt(t, 10, op.DateValue().Hour)
		assertInt(t, 30, op.DateValue().Minute)
		assertInt(t, 45, op.DateValue().Second)
	})

	t.Run("OperandMyDate_ToText_Test", func(t *testing.T) {
		dt := alg.NewMyDate(2024, 6, 15, 10, 30, 45)
		op := alg.CreateMyDate(dt)
		result := op.ToText("")
		assertTrue(t, result.IsText(), "want IsText")
	})

	t.Run("OperandMyDate_ToMyDate_Test", func(t *testing.T) {
		dt := alg.NewMyDate(2024, 6, 15, 10, 30, 45)
		op := alg.CreateMyDate(dt)
		result := op.ToMyDate("")
		assertTrue(t, result.IsDate(), "want IsDate")
		assertInt(t, 2024, result.DateValue().Year)
	})

	t.Run("OperandMyDate_ToString_Test", func(t *testing.T) {
		dt := alg.NewMyDate(2024, 6, 15, 10, 30, 45)
		op := alg.CreateMyDate(dt)
		assertTrue(t, strings.HasPrefix(op.String(), `"2024-06-15`), "want prefix %q, got %q", `"2024-06-15`, op.String())
	})

	// ---------------------------------------------------------- Implicit Operator 测试（Go 无隐式转换，改用工厂函数）
	t.Run("Operand_Implicit_Int_Test", func(t *testing.T) {
		var op alg.Operand = alg.CreateInt(42)
		assertTrue(t, op.IsNumber(), "want IsNumber")
		assertInt(t, 42, op.IntValue())
	})

	t.Run("Operand_Implicit_Double_Test", func(t *testing.T) {
		var op alg.Operand = alg.Create(3.14)
		assertTrue(t, op.IsNumber(), "want IsNumber")
		assertFloatRound(t, 3.14, op.DoubleValue(), 3)
	})

	t.Run("Operand_Implicit_Decimal_Test", func(t *testing.T) {
		var op alg.Operand = alg.CreateDecimal(alg.MustDecimal("123.45"))
		assertTrue(t, op.IsNumber(), "want IsNumber")
		assertDecimal(t, "123.45", op.NumberValue())
	})

	t.Run("Operand_Implicit_Bool_Test", func(t *testing.T) {
		var op alg.Operand = alg.Create(true)
		assertTrue(t, op.IsBoolean(), "want IsBoolean")
		assertBool(t, true, op.BooleanValue())

		op = alg.Create(false)
		assertTrue(t, op.IsBoolean(), "want IsBoolean")
		assertBool(t, false, op.BooleanValue())
	})

	t.Run("Operand_Implicit_String_Test", func(t *testing.T) {
		var op alg.Operand = alg.CreateString("hello")
		assertTrue(t, op.IsText(), "want IsText")
		assertText(t, "hello", op.TextValue())
	})

	t.Run("Operand_Implicit_DateTime_Test", func(t *testing.T) {
		dt := alg.NewMyDate(2024, 1, 15, 0, 0, 0)
		var op alg.Operand = alg.CreateMyDate(dt)
		assertTrue(t, op.IsDate(), "want IsDate")
		assertInt(t, 2024, op.DateValue().Year)
	})

	t.Run("Operand_Implicit_TimeSpan_Test", func(t *testing.T) {
		// C# new TimeSpan(10, 30, 0) = 10 时 30 分
		ts := alg.NewMyDate(0, 0, 0, 10, 30, 0)
		var op alg.Operand = alg.CreateMyDate(ts)
		assertTrue(t, op.IsDate(), "want IsDate")
		assertInt(t, 10, op.DateValue().Hour)
		assertInt(t, 30, op.DateValue().Minute)
	})

	t.Run("Operand_Implicit_List_Test", func(t *testing.T) {
		var op alg.Operand = alg.Create([]int{1, 2, 3})
		assertTrue(t, op.IsArray(), "want IsArray")
		assertInt(t, 3, len(op.ArrayValue()))

		op = alg.Create([]string{"a", "b"})
		assertTrue(t, op.IsArray(), "want IsArray")
		assertInt(t, 2, len(op.ArrayValue()))

		op = alg.Create([]float64{1.1, 2.2})
		assertTrue(t, op.IsArray(), "want IsArray")
		assertInt(t, 2, len(op.ArrayValue()))

		op = alg.Create([]bool{true, false})
		assertTrue(t, op.IsArray(), "want IsArray")
		assertInt(t, 2, len(op.ArrayValue()))
	})
}
