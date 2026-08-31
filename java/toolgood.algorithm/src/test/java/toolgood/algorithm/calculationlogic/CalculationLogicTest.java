package toolgood.algorithm.calculationlogic;

import org.joda.time.DateTime;
import org.joda.time.DateTimeZone;
import org.junit.Test;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;

import java.math.BigDecimal;

import static org.junit.Assert.*;

public class CalculationLogicTest {

    private static CalculationLogicEngine CreateEngine() {
        return CreateEngine(true);
    }

    private static CalculationLogicEngine CreateEngine(boolean useCalculationLogicInfo) {
        return new CalculationLogicEngine(new toolgood.algorithm.FunctionCache(), useCalculationLogicInfo);
    }

    // #region SetSceneName 测试

    @Test
    public void SetSceneName_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.SetSceneName("场景1");
        assertEquals("===== 场景1 =====\n", logic.ToInfoString());
    }

    @Test
    public void SetSceneName_NoInfo_Test() {
        CalculationLogicEngine logic = CreateEngine(false);
        logic.SetSceneName("场景1");
        assertEquals("", logic.ToInfoString());
    }

    // #endregion

    // #region InitValue 测试

    @Test
    public void InitValue_Int_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.InitValue("a", 1);
        logic.InitValue("b", 2);
        assertEquals("[初始] a=1;b=2;\n", logic.ToInfoString());
    }

    @Test
    public void InitValue_String_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.InitValue("s", "hello");
        assertEquals("[初始] s=hello;\n", logic.ToInfoString());
    }

    @Test
    public void InitValue_Decimal_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.InitValue("d", new BigDecimal("1.5"));
        assertEquals("[初始] d=1.5;\n", logic.ToInfoString());
    }

    @Test
    public void InitValue_Double_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.InitValue("dd", 1.5);
        assertEquals("[初始] dd=1.5;\n", logic.ToInfoString());
    }

    @Test
    public void InitValue_Bool_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.InitValue("flag", true);
        assertEquals("[初始] flag=True;\n", logic.ToInfoString());
    }

    @Test
    public void InitValue_LayerRemark_Test() {
        // InitValue 的层级与备注不参与信息输出
        CalculationLogicEngine logic = CreateEngine();
        logic.InitValue("i", 100, 2, "备注");
        assertEquals("[初始] i=100;\n", logic.ToInfoString());
    }

    @Test
    public void InitValue_CanBeUsedInCondition_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.InitValue("a", 1);
        logic.InitValue("b", 2);
        assertTrue(logic.CheckCondition("a<b"));
        assertFalse(logic.CheckCondition("a>b"));
    }

    @Test
    public void InitValue_DateTime_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.InitValue("dt", new DateTime(2026, 8, 31, 12, 0, 0, DateTimeZone.UTC));
        assertEquals("[初始] dt=\"2026-08-31 12:00:00\";\n", logic.ToInfoString());
    }

    @Test
    public void InitValue_DateTime_LayerRemark_Test() {
        // InitValue 的层级与备注不参与信息输出
        CalculationLogicEngine logic = CreateEngine();
        logic.InitValue("dt", new DateTime(2026, 8, 31, 12, 0, 0, DateTimeZone.UTC), 2, "备注");
        assertEquals("[初始] dt=\"2026-08-31 12:00:00\";\n", logic.ToInfoString());
    }

    @Test
    public void InitValue_DateTime_CanBeUsedInCondition_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.InitValue("dt", new DateTime(2026, 8, 31, 12, 0, 0, DateTimeZone.UTC));
        assertTrue(logic.CheckCondition("YEAR(dt)=2026"));
        assertTrue(logic.CheckCondition("MONTH(dt)=8"));
        assertTrue(logic.CheckCondition("dt>DATEVALUE('2026-08-30')"));
    }

    // #endregion

    // #region CheckCondition 测试

    @Test
    public void CheckCondition_True_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.InitValue("a", 1);
        logic.InitValue("b", 2);
        assertTrue(logic.CheckCondition("a<b"));
        assertEquals("[初始] a=1;b=2;\n[成功] if a<b: // 1<2\n", logic.ToInfoString());
    }

    @Test
    public void CheckCondition_False_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.InitValue("a", 1);
        logic.InitValue("b", 2);
        assertFalse(logic.CheckCondition("a>b"));
        assertEquals("[初始] a=1;b=2;\n[失败] if a>b: // 1>2\n", logic.ToInfoString());
    }

    @Test
    public void CheckCondition_LayerRemark_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.InitValue("a", 1);
        logic.InitValue("b", 2);
        assertTrue(logic.CheckCondition("a<b", 1, "备注"));
        assertEquals("[初始] a=1;b=2;\n[成功]    if a<b: // 1<2 // 备注\n", logic.ToInfoString());
    }

    @Test
    public void CheckCondition_StringCompare_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.InitValue("s", "abc");
        assertTrue(logic.CheckCondition("s='abc'"));
        assertFalse(logic.CheckCondition("s='ab'"));
        assertEquals("[初始] s=abc;\n[成功] if s='abc': // \"abc\"='abc'\n[失败] if s='ab': // \"abc\"='ab'\n", logic.ToInfoString());
    }

    @Test
    public void CheckCondition_NonBoolean_Throws_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.InitValue("a", 1);
        logic.InitValue("b", 2);
        IllegalArgumentException ex = assertThrows(IllegalArgumentException.class, () -> logic.CheckCondition("a+b"));
        assertEquals("The condition must be a boolean value!", ex.getMessage());
        assertEquals("[初始] a=1;b=2;\n[条件][错误] if a+b: // 1+2 = The condition must be a boolean value!\n", logic.ToInfoString());
    }

    @Test
    public void CheckCondition_Error_Throws_Test() {
        CalculationLogicEngine logic = CreateEngine();
        IllegalArgumentException ex = assertThrows(IllegalArgumentException.class, () -> logic.CheckCondition("1/0=0"));
        assertEquals("Function '/' Div 0 error!", ex.getMessage());
        assertEquals("[条件][错误] if 1/0=0: // 1/0=0 = Function '/' Div 0 error!\n", logic.ToInfoString());
    }

    // #endregion

    // #region SetFormula 测试

    @Test
    public void SetFormula_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.InitValue("a", 1);
        logic.InitValue("b", 2);
        logic.SetFormula("x", "a*3+b", 1, "备注");
        assertEquals("[初始] a=1;b=2;\n[赋值]    x = a*3+b = 1*3+2 = 5 // 备注\n", logic.ToInfoString());
        // 公式结果应已写入参数
        assertTrue(logic.CheckCondition("x=5"));
    }

    @Test
    public void SetFormula_StringConcat_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.InitValue("s", "hello");
        logic.SetFormula("y", "s&'!'");
        assertEquals("[初始] s=hello;\n[赋值] y = s&'!' = \"hello\"&'!' = hello!\n", logic.ToInfoString());
        assertTrue(logic.CheckCondition("y='hello!'"));
    }

    @Test
    public void SetFormula_TextAndNumber_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.InitValue("a", 1);
        logic.SetFormula("z", "'text'&a");
        assertEquals("[初始] a=1;\n[赋值] z = 'text'&a = 'text'&1 = text1\n", logic.ToInfoString());
    }

    @Test
    public void SetFormula_Error_Throws_Test() {
        CalculationLogicEngine logic = CreateEngine();
        IllegalArgumentException ex = assertThrows(IllegalArgumentException.class, () -> logic.SetFormula("x", "1/0"));
        assertEquals("Function '/' Div 0 error!", ex.getMessage());
        assertEquals("[赋值][错误] x = 1/0 = 1/0 // Function '/' Div 0 error!\n", logic.ToInfoString());
    }

    // #endregion

    // #region SetValue 测试

    @Test
    public void SetValue_String_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.SetValue("s", "文本");
        assertEquals("[赋值] s = \"文本\"\n", logic.ToInfoString());
        assertTrue(logic.CheckCondition("s='文本'"));
    }

    @Test
    public void SetValue_StringEscape_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.SetValue("sq", "a\"b");
        assertEquals("[赋值] sq = \"a\\\"b\"\n", logic.ToInfoString());
    }

    @Test
    public void SetValue_Decimal_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.SetValue("d", new BigDecimal("1.5"));
        assertEquals("[赋值] d = 1.5\n", logic.ToInfoString());
    }

    @Test
    public void SetValue_Double_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.SetValue("dd", 1.5);
        assertEquals("[赋值] dd = 1.5\n", logic.ToInfoString());
    }

    @Test
    public void SetValue_Bool_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.SetValue("flag", true);
        assertEquals("[赋值] flag = True\n", logic.ToInfoString());
    }

    @Test
    public void SetValue_LayerRemark_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.SetValue("i", 100, 1, "备注");
        assertEquals("[赋值]    i = 100 // 备注\n", logic.ToInfoString());
    }

    @Test
    public void SetValue_DateTime_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.SetValue("dt", new DateTime(2026, 8, 31, 12, 0, 0, DateTimeZone.UTC));
        assertEquals("[赋值] dt = \"2026-08-31 12:00:00\"\n", logic.ToInfoString());
    }

    @Test
    public void SetValue_DateTime_LayerRemark_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.SetValue("dt", new DateTime(2026, 8, 31, 12, 0, 0, DateTimeZone.UTC), 1, "备注");
        assertEquals("[赋值]    dt = \"2026-08-31 12:00:00\" // 备注\n", logic.ToInfoString());
    }

    @Test
    public void SetValue_DateTime_CanBeUsedInCondition_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.SetValue("dt", new DateTime(2026, 8, 31, 12, 0, 0, DateTimeZone.UTC));
        assertTrue(logic.CheckCondition("YEAR(dt)=2026"));
        assertTrue(logic.CheckCondition("MONTH(dt)=8"));
        assertTrue(logic.CheckCondition("dt>DATEVALUE('2026-08-30')"));
    }

    // #endregion

    // #region GetValue 测试

    @Test
    public void GetValue_InitValue_Int_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.InitValue("a", 1);
        Operand operand = logic.GetValue("a");
        assertTrue(operand.IsNumber());
        assertEquals(OperandType.NUMBER, operand.Type());
        assertEquals(0, operand.NumberValue().compareTo(BigDecimal.ONE));
    }

    @Test
    public void GetValue_InitValue_String_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.InitValue("s", "hello");
        Operand operand = logic.GetValue("s");
        assertTrue(operand.IsText());
        assertEquals(OperandType.TEXT, operand.Type());
        assertEquals("hello", operand.TextValue());
    }

    @Test
    public void GetValue_InitValue_Decimal_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.InitValue("d", new BigDecimal("1.5"));
        Operand operand = logic.GetValue("d");
        assertTrue(operand.IsNumber());
        assertEquals(0, operand.NumberValue().compareTo(new BigDecimal("1.5")));
    }

    @Test
    public void GetValue_InitValue_Double_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.InitValue("dd", 1.5);
        Operand operand = logic.GetValue("dd");
        assertTrue(operand.IsNumber());
        assertEquals(0, operand.NumberValue().compareTo(new BigDecimal("1.5")));
    }

    @Test
    public void GetValue_InitValue_Bool_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.InitValue("flag", true);
        Operand operand = logic.GetValue("flag");
        assertTrue(operand.IsBoolean());
        assertEquals(OperandType.BOOLEAN, operand.Type());
        assertTrue(operand.BooleanValue());
    }

    @Test
    public void GetValue_InitValue_DateTime_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.InitValue("dt", new DateTime(2026, 8, 31, 12, 0, 0, DateTimeZone.UTC));
        Operand operand = logic.GetValue("dt");
        assertTrue(operand.IsDate());
        assertEquals(OperandType.DATE, operand.Type());
        assertEquals("2026-08-31 12:00:00", operand.DateValue().toString());
    }

    @Test
    public void GetValue_SetValue_DateTime_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.SetValue("dt", new DateTime(2026, 8, 31, 12, 0, 0, DateTimeZone.UTC));
        Operand operand = logic.GetValue("dt");
        assertTrue(operand.IsDate());
        assertEquals(OperandType.DATE, operand.Type());
        assertEquals("2026-08-31 12:00:00", operand.DateValue().toString());
    }

    @Test
    public void GetValue_SetValue_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.SetValue("v", "text");
        Operand operand = logic.GetValue("v");
        assertTrue(operand.IsText());
        assertEquals("text", operand.TextValue());
    }

    @Test
    public void GetValue_SetFormula_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.InitValue("a", 1);
        logic.InitValue("b", 2);
        logic.SetFormula("x", "a*3+b");
        Operand operand = logic.GetValue("x");
        assertTrue(operand.IsNumber());
        assertEquals(0, operand.NumberValue().compareTo(new BigDecimal("5")));
    }

    @Test
    public void GetValue_AfterSetFormulaOverwrite_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.InitValue("a", 1);
        logic.SetFormula("a", "a+10");
        Operand operand = logic.GetValue("a");
        assertTrue(operand.IsNumber());
        assertEquals(0, operand.NumberValue().compareTo(new BigDecimal("11")));
    }

    @Test
    public void GetValue_NotFound_ReturnsError_Test() {
        CalculationLogicEngine logic = CreateEngine();
        Operand operand = logic.GetValue("missing");
        assertTrue(operand.IsError());
        assertEquals("Parameter [missing] is missing.", operand.ErrorMsg());
    }

    @Test
    public void GetValue_NoInfo_Test() {
        // 关闭信息记录不影响取值
        CalculationLogicEngine logic = CreateEngine(false);
        logic.InitValue("a", 1);
        assertEquals(0, logic.GetValue("a").NumberValue().compareTo(BigDecimal.ONE));
    }

    // #endregion

    // #region BlankLine 测试

    @Test
    public void BlankLine_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.BlankLine();
        assertEquals("\n", logic.ToInfoString());

        logic.BlankLine();
        logic.BlankLine();
        assertEquals("\n\n\n", logic.ToInfoString());
    }

    // #endregion

    // #region ToInfoString 测试

    @Test
    public void ToInfoString_Empty_Test() {
        CalculationLogicEngine logic = CreateEngine();
        assertEquals("", logic.ToInfoString());
    }

    @Test
    public void ToInfoString_Disabled_Test() {
        // 关闭信息记录后，所有操作都不产生信息
        CalculationLogicEngine logic = CreateEngine(false);
        logic.SetSceneName("场景1");
        logic.InitValue("a", 1);
        logic.CheckCondition("a>0");
        logic.SetValue("b", 2);
        logic.SetFormula("c", "a+1");
        logic.BlankLine();
        assertEquals("", logic.ToInfoString());
    }

    @Test
    public void ToInfoString_FullFlow_Test() {
        CalculationLogicEngine logic = CreateEngine();
        logic.SetSceneName("场景1");
        logic.InitValue("a", 1);
        logic.InitValue("b", 2);
        if (logic.CheckCondition("a>b")) {
            logic.SetValue("c", 3, 1);
            logic.SetFormula("c", "a*5+b", 1);
        } else if (logic.CheckCondition("a<b")) {
            logic.SetValue("c", 4, 1);
            logic.SetFormula("c", "a*3+b", 1);
        }
        logic.BlankLine();
        logic.SetValue("e", 5);

        String expected = "[初始] a=1;b=2;\n"
                + "===== 场景1 =====\n"
                + "[失败] if a>b: // 1>2\n"
                + "[成功] if a<b: // 1<2\n"
                + "[赋值]    c = 4\n"
                + "[赋值]    c = a*3+b = 1*3+2 = 5\n"
                + "\n"
                + "[赋值] e = 5\n";
        assertEquals(expected, logic.ToInfoString());
    }

    // #endregion
}
