package test

import (
	"testing"
	"time"

	alg "github.com/toolgood/ToolGood.Algorithm/go/algorithm"
	calc "github.com/toolgood/ToolGood.Algorithm/go/algorithm/calculationlogic"
)

// TestCalculationLogicTest 移植自
// csharp/ToolGood.Algorithm.Test/CalculationLogic/CalculationLogicTest.cs。
func TestCalculationLogicTest(t *testing.T) {
	createEngine := func(useInfo bool) *calc.CalculationLogicEngine {
		return calc.NewCalculationLogicEngine(alg.NewFunctionCache(nil), useInfo)
	}

	t.Run("SetSceneName_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.SetSceneName("场景1")
		assertText(t, "===== 场景1 =====\n", logic.ToInfoString())
	})

	t.Run("SetSceneName_NoInfo_Test", func(t *testing.T) {
		logic := createEngine(false)
		logic.SetSceneName("场景1")
		assertText(t, "", logic.ToInfoString())
	})

	t.Run("InitValue_Int_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.InitValue("a", 1, 0, "")
		logic.InitValue("b", 2, 0, "")
		assertText(t, "[初始] a=1;b=2;\n", logic.ToInfoString())
	})

	t.Run("InitValue_String_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.InitValue("s", "hello", 0, "")
		assertText(t, "[初始] s=hello;\n", logic.ToInfoString())
	})

	t.Run("InitValue_Decimal_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.InitValue("d", alg.MustDecimal("1.5"), 0, "")
		assertText(t, "[初始] d=1.5;\n", logic.ToInfoString())
	})

	t.Run("InitValue_Double_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.InitValue("dd", 1.5, 0, "")
		assertText(t, "[初始] dd=1.5;\n", logic.ToInfoString())
	})

	t.Run("InitValue_Bool_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.InitValue("flag", true, 0, "")
		assertText(t, "[初始] flag=True;\n", logic.ToInfoString())
	})

	t.Run("InitValue_LayerRemark_Test", func(t *testing.T) {
		// InitValue 的层级与备注不参与信息输出
		logic := createEngine(true)
		logic.InitValue("i", 100, 2, "备注")
		assertText(t, "[初始] i=100;\n", logic.ToInfoString())
	})

	t.Run("InitValue_CanBeUsedInCondition_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.InitValue("a", 1, 0, "")
		logic.InitValue("b", 2, 0, "")
		ok, err := logic.CheckCondition("a<b", 0, "")
		assertNull(t, err)
		assertTrue(t, ok, "a<b 应为真")
		ok, err = logic.CheckCondition("a>b", 0, "")
		assertNull(t, err)
		assertFalse(t, ok, "a>b 应为假")
	})

	t.Run("InitValue_DateTime_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.InitValue("dt", time.Date(2026, 8, 31, 12, 0, 0, 0, time.UTC), 0, "")
		assertText(t, "[初始] dt=\"2026-08-31 12:00:00\";\n", logic.ToInfoString())
	})

	t.Run("InitValue_DateTime_LayerRemark_Test", func(t *testing.T) {
		// InitValue 的层级与备注不参与信息输出
		logic := createEngine(true)
		logic.InitValue("dt", time.Date(2026, 8, 31, 12, 0, 0, 0, time.UTC), 2, "备注")
		assertText(t, "[初始] dt=\"2026-08-31 12:00:00\";\n", logic.ToInfoString())
	})

	t.Run("InitValue_DateTime_CanBeUsedInCondition_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.InitValue("dt", time.Date(2026, 8, 31, 12, 0, 0, 0, time.UTC), 0, "")
		for _, cond := range []string{"YEAR(dt)=2026", "MONTH(dt)=8", "dt>DATEVALUE('2026-08-30')"} {
			ok, err := logic.CheckCondition(cond, 0, "")
			assertNull(t, err)
			assertTrue(t, ok, cond+" 应为真")
		}
	})

	t.Run("CheckCondition_True_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.InitValue("a", 1, 0, "")
		logic.InitValue("b", 2, 0, "")
		ok, err := logic.CheckCondition("a<b", 0, "")
		assertNull(t, err)
		assertTrue(t, ok, "a<b 应为真")
		assertText(t, "[初始] a=1;b=2;\n[成功] if a<b: // 1<2\n", logic.ToInfoString())
	})

	t.Run("CheckCondition_False_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.InitValue("a", 1, 0, "")
		logic.InitValue("b", 2, 0, "")
		ok, err := logic.CheckCondition("a>b", 0, "")
		assertNull(t, err)
		assertFalse(t, ok, "a>b 应为假")
		assertText(t, "[初始] a=1;b=2;\n[失败] if a>b: // 1>2\n", logic.ToInfoString())
	})

	t.Run("CheckCondition_LayerRemark_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.InitValue("a", 1, 0, "")
		logic.InitValue("b", 2, 0, "")
		ok, err := logic.CheckCondition("a<b", 1, "备注")
		assertNull(t, err)
		assertTrue(t, ok, "a<b 应为真")
		assertText(t, "[初始] a=1;b=2;\n[成功]    if a<b: // 1<2 // 备注\n", logic.ToInfoString())
	})

	t.Run("CheckCondition_StringCompare_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.InitValue("s", "abc", 0, "")
		ok, err := logic.CheckCondition("s='abc'", 0, "")
		assertNull(t, err)
		assertTrue(t, ok, "s='abc' 应为真")
		ok, err = logic.CheckCondition("s='ab'", 0, "")
		assertNull(t, err)
		assertFalse(t, ok, "s='ab' 应为假")
		assertText(t, "[初始] s=abc;\n[成功] if s='abc': // \"abc\"='abc'\n[失败] if s='ab': // \"abc\"='ab'\n", logic.ToInfoString())
	})

	t.Run("CheckCondition_NonBoolean_Throws_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.InitValue("a", 1, 0, "")
		logic.InitValue("b", 2, 0, "")
		ok, err := logic.CheckCondition("a+b", 0, "")
		assertFalse(t, ok, "条件不是布尔值时应返回 false")
		assertNotNull(t, err)
		assertText(t, "The condition must be a boolean value!", err.Error())
		assertText(t, "[初始] a=1;b=2;\n[条件][错误] if a+b: // 1+2 = The condition must be a boolean value!\n", logic.ToInfoString())
	})

	t.Run("CheckCondition_Error_Throws_Test", func(t *testing.T) {
		logic := createEngine(true)
		ok, err := logic.CheckCondition("1/0=0", 0, "")
		assertFalse(t, ok, "求值出错时应返回 false")
		assertNotNull(t, err)
		assertText(t, "Function '/' Div 0 error!", err.Error())
		assertText(t, "[条件][错误] if 1/0=0: // 1/0=0 = Function '/' Div 0 error!\n", logic.ToInfoString())
	})

	t.Run("SetFormula_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.InitValue("a", 1, 0, "")
		logic.InitValue("b", 2, 0, "")
		if err := logic.SetFormula("x", "a*3+b", 1, "备注"); err != nil {
			t.Fatalf("SetFormula: %v", err)
		}
		assertText(t, "[初始] a=1;b=2;\n[赋值]    x = a*3+b = 1*3+2 = 5 // 备注\n", logic.ToInfoString())
		// 公式结果应已写入参数
		ok, err := logic.CheckCondition("x=5", 0, "")
		assertNull(t, err)
		assertTrue(t, ok, "x 应等于 5")
	})

	t.Run("SetFormula_StringConcat_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.InitValue("s", "hello", 0, "")
		if err := logic.SetFormula("y", "s&'!'", 0, ""); err != nil {
			t.Fatalf("SetFormula: %v", err)
		}
		assertText(t, "[初始] s=hello;\n[赋值] y = s&'!' = \"hello\"&'!' = hello!\n", logic.ToInfoString())
		ok, err := logic.CheckCondition("y='hello!'", 0, "")
		assertNull(t, err)
		assertTrue(t, ok, "y 应为 hello!")
	})

	t.Run("SetFormula_TextAndNumber_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.InitValue("a", 1, 0, "")
		if err := logic.SetFormula("z", "'text'&a", 0, ""); err != nil {
			t.Fatalf("SetFormula: %v", err)
		}
		assertText(t, "[初始] a=1;\n[赋值] z = 'text'&a = 'text'&1 = text1\n", logic.ToInfoString())
	})

	t.Run("SetFormula_Error_Throws_Test", func(t *testing.T) {
		logic := createEngine(true)
		err := logic.SetFormula("x", "1/0", 0, "")
		assertNotNull(t, err)
		assertText(t, "Function '/' Div 0 error!", err.Error())
		assertText(t, "[赋值][错误] x = 1/0 = 1/0 // Function '/' Div 0 error!\n", logic.ToInfoString())
	})

	t.Run("SetValue_String_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.SetValue("s", "文本", 0, "")
		assertText(t, "[赋值] s = \"文本\"\n", logic.ToInfoString())
		ok, err := logic.CheckCondition("s='文本'", 0, "")
		assertNull(t, err)
		assertTrue(t, ok, "s 应为 文本")
	})

	t.Run("SetValue_StringEscape_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.SetValue("sq", `a"b`, 0, "")
		assertText(t, `[赋值] sq = "a\"b"`+"\n", logic.ToInfoString())
	})

	t.Run("SetValue_Decimal_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.SetValue("d", alg.MustDecimal("1.5"), 0, "")
		assertText(t, "[赋值] d = 1.5\n", logic.ToInfoString())
	})

	t.Run("SetValue_Double_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.SetValue("dd", 1.5, 0, "")
		assertText(t, "[赋值] dd = 1.5\n", logic.ToInfoString())
	})

	t.Run("SetValue_Bool_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.SetValue("flag", true, 0, "")
		assertText(t, "[赋值] flag = True\n", logic.ToInfoString())
	})

	t.Run("SetValue_LayerRemark_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.SetValue("i", 100, 1, "备注")
		assertText(t, "[赋值]    i = 100 // 备注\n", logic.ToInfoString())
	})

	t.Run("SetValue_DateTime_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.SetValue("dt", time.Date(2026, 8, 31, 12, 0, 0, 0, time.UTC), 0, "")
		assertText(t, "[赋值] dt = \"2026-08-31 12:00:00\"\n", logic.ToInfoString())
	})

	t.Run("SetValue_DateTime_LayerRemark_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.SetValue("dt", time.Date(2026, 8, 31, 12, 0, 0, 0, time.UTC), 1, "备注")
		assertText(t, "[赋值]    dt = \"2026-08-31 12:00:00\" // 备注\n", logic.ToInfoString())
	})

	t.Run("SetValue_DateTime_CanBeUsedInCondition_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.SetValue("dt", time.Date(2026, 8, 31, 12, 0, 0, 0, time.UTC), 0, "")
		for _, cond := range []string{"YEAR(dt)=2026", "MONTH(dt)=8", "dt>DATEVALUE('2026-08-30')"} {
			ok, err := logic.CheckCondition(cond, 0, "")
			assertNull(t, err)
			assertTrue(t, ok, cond+" 应为真")
		}
	})

	t.Run("GetValue_InitValue_Int_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.InitValue("a", 1, 0, "")
		operand := logic.GetValue("a")
		assertTrue(t, operand.IsNumber(), "应为数字")
		assertTrue(t, operand.Type() == alg.OperandTypeNumber, "类型应为 NUMBER")
		assertDecimal(t, "1", operand.NumberValue())
	})

	t.Run("GetValue_InitValue_String_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.InitValue("s", "hello", 0, "")
		operand := logic.GetValue("s")
		assertTrue(t, operand.IsText(), "应为文本")
		assertTrue(t, operand.Type() == alg.OperandTypeText, "类型应为 TEXT")
		assertText(t, "hello", operand.TextValue())
	})

	t.Run("GetValue_InitValue_Decimal_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.InitValue("d", alg.MustDecimal("1.5"), 0, "")
		operand := logic.GetValue("d")
		assertTrue(t, operand.IsNumber(), "应为数字")
		assertDecimal(t, "1.5", operand.NumberValue())
	})

	t.Run("GetValue_InitValue_Double_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.InitValue("dd", 1.5, 0, "")
		operand := logic.GetValue("dd")
		assertTrue(t, operand.IsNumber(), "应为数字")
		assertDecimal(t, "1.5", operand.NumberValue())
	})

	t.Run("GetValue_InitValue_Bool_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.InitValue("flag", true, 0, "")
		operand := logic.GetValue("flag")
		assertTrue(t, operand.IsBoolean(), "应为布尔")
		assertTrue(t, operand.Type() == alg.OperandTypeBoolean, "类型应为 BOOLEAN")
		assertTrue(t, operand.BooleanValue(), "值应为 true")
	})

	t.Run("GetValue_InitValue_DateTime_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.InitValue("dt", time.Date(2026, 8, 31, 12, 0, 0, 0, time.UTC), 0, "")
		operand := logic.GetValue("dt")
		assertTrue(t, operand.IsDate(), "应为日期")
		assertTrue(t, operand.Type() == alg.OperandTypeDate, "类型应为 DATE")
		assertTrue(t, operand.DateValue().ToDateTime().Equal(time.Date(2026, 8, 31, 12, 0, 0, 0, time.UTC)), "日期值不一致")
	})

	t.Run("GetValue_SetValue_DateTime_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.SetValue("dt", time.Date(2026, 8, 31, 12, 0, 0, 0, time.UTC), 0, "")
		operand := logic.GetValue("dt")
		assertTrue(t, operand.IsDate(), "应为日期")
		assertTrue(t, operand.Type() == alg.OperandTypeDate, "类型应为 DATE")
		assertTrue(t, operand.DateValue().ToDateTime().Equal(time.Date(2026, 8, 31, 12, 0, 0, 0, time.UTC)), "日期值不一致")
	})

	t.Run("GetValue_SetValue_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.SetValue("v", "text", 0, "")
		operand := logic.GetValue("v")
		assertTrue(t, operand.IsText(), "应为文本")
		assertText(t, "text", operand.TextValue())
	})

	t.Run("GetValue_SetFormula_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.InitValue("a", 1, 0, "")
		logic.InitValue("b", 2, 0, "")
		if err := logic.SetFormula("x", "a*3+b", 0, ""); err != nil {
			t.Fatalf("SetFormula: %v", err)
		}
		operand := logic.GetValue("x")
		assertTrue(t, operand.IsNumber(), "应为数字")
		assertDecimal(t, "5", operand.NumberValue())
	})

	t.Run("GetValue_AfterSetFormulaOverwrite_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.InitValue("a", 1, 0, "")
		if err := logic.SetFormula("a", "a+10", 0, ""); err != nil {
			t.Fatalf("SetFormula: %v", err)
		}
		operand := logic.GetValue("a")
		assertTrue(t, operand.IsNumber(), "应为数字")
		assertDecimal(t, "11", operand.NumberValue())
	})

	t.Run("GetValue_NotFound_ReturnsError_Test", func(t *testing.T) {
		logic := createEngine(true)
		operand := logic.GetValue("missing")
		assertTrue(t, operand.IsError(), "应为错误")
		assertText(t, "Parameter [missing] is missing.", operand.ErrorMsg())
	})

	t.Run("GetValue_NoInfo_Test", func(t *testing.T) {
		// 关闭信息记录不影响取值
		logic := createEngine(false)
		logic.InitValue("a", 1, 0, "")
		assertDecimal(t, "1", logic.GetValue("a").NumberValue())
	})

	t.Run("BlankLine_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.BlankLine()
		assertText(t, "\n", logic.ToInfoString())

		logic.BlankLine()
		logic.BlankLine()
		assertText(t, "\n\n\n", logic.ToInfoString())
	})

	t.Run("ToInfoString_Empty_Test", func(t *testing.T) {
		logic := createEngine(true)
		assertText(t, "", logic.ToInfoString())
	})

	t.Run("ToInfoString_Disabled_Test", func(t *testing.T) {
		// 关闭信息记录后，所有操作都不产生信息
		logic := createEngine(false)
		logic.SetSceneName("场景1")
		logic.InitValue("a", 1, 0, "")
		_, _ = logic.CheckCondition("a>0", 0, "")
		logic.SetValue("b", 2, 0, "")
		_ = logic.SetFormula("c", "a+1", 0, "")
		logic.BlankLine()
		assertText(t, "", logic.ToInfoString())
	})

	t.Run("ToInfoString_FullFlow_Test", func(t *testing.T) {
		logic := createEngine(true)
		logic.SetSceneName("场景1")
		logic.InitValue("a", 1, 0, "")
		logic.InitValue("b", 2, 0, "")
		if ok, _ := logic.CheckCondition("a>b", 0, ""); ok {
			logic.SetValue("c", 3, 1, "")
			_ = logic.SetFormula("c", "a*5+b", 1, "")
		} else if ok, _ := logic.CheckCondition("a<b", 0, ""); ok {
			logic.SetValue("c", 4, 1, "")
			_ = logic.SetFormula("c", "a*3+b", 1, "")
		}
		logic.BlankLine()
		logic.SetValue("e", 5, 0, "")

		expected := "[初始] a=1;b=2;\n" +
			"===== 场景1 =====\n" +
			"[失败] if a>b: // 1>2\n" +
			"[成功] if a<b: // 1<2\n" +
			"[赋值]    c = 4\n" +
			"[赋值]    c = a*3+b = 1*3+2 = 5\n" +
			"\n" +
			"[赋值] e = 5\n"
		assertText(t, expected, logic.ToInfoString())
	})
}
