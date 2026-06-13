package toolgood.algorithm.operands;

import org.junit.Test;
import static org.junit.Assert.*;

import toolgood.algorithm.MyDate;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

public class OperandsTest {

    // region Operand 静态属性测试

    @Test
    public void Operand_StaticProperties_Test() {
        assertEquals("ToolGood.Algorithm 6.2", Operand.Version.TextValue());
        assertEquals(true, Operand.True.BooleanValue());
        assertEquals(false, Operand.False.BooleanValue());
        assertEquals(1, Operand.One.IntValue());
        assertEquals(0, Operand.Zero.IntValue());
        assertTrue(Operand.Null.IsNull());
    }

    // endregion

    // region Operand Create 测试

    @Test
    public void Operand_Create_Bool_Test() {
        Operand op = Operand.Create(true);
        assertTrue(op.IsBoolean());
        assertEquals(true, op.BooleanValue());

        op = Operand.Create(false);
        assertTrue(op.IsBoolean());
        assertEquals(false, op.BooleanValue());
    }

    @Test
    public void Operand_Create_Int_Test() {
        Operand op = Operand.Create(100);
        assertTrue(op.IsNumber());
        assertEquals(100, op.IntValue());
        assertEquals(0, new BigDecimal("100").compareTo(op.NumberValue()));

        op = Operand.Create(-100);
        assertTrue(op.IsNumber());
        assertEquals(-100, op.IntValue());

        op = Operand.Create(0);
        assertTrue(op.IsNumber());
        assertEquals(0, op.IntValue());

        op = Operand.Create(1000);
        assertTrue(op.IsNumber());
        assertEquals(1000, op.IntValue());

        op = Operand.Create(-1000);
        assertTrue(op.IsNumber());
        assertEquals(-1000, op.IntValue());
    }

    @Test
    public void Operand_Create_Long_Test() {
        Operand op = Operand.Create(100000L);
        assertTrue(op.IsNumber());
        assertEquals(100000L, op.LongValue());
    }

    @Test
    public void Operand_Create_Double_Test() {
        Operand op = Operand.Create(3.14159);
        assertTrue(op.IsNumber());
        assertEquals(3.14159, op.DoubleValue(), 0.00001);
    }

    @Test
    public void Operand_Create_Decimal_Test() {
        Operand op = Operand.Create(new BigDecimal("3.14159"));
        assertTrue(op.IsNumber());
        assertEquals(0, new BigDecimal("3.14159").compareTo(op.NumberValue()));
    }

    @Test
    public void Operand_Create_String_Test() {
        Operand op = Operand.Create("hello");
        assertTrue(op.IsText());
        assertEquals("hello", op.TextValue());

        op = Operand.Create("");
        assertTrue(op.IsText());
        assertEquals("", op.TextValue());

        op = Operand.Create((String) null);
        assertTrue(op.IsNull());
    }

    @Test
    public void Operand_Create_DateTime_Test() {
        MyDate dt = new MyDate(2024, 1, 15, 10, 30, 45);
        Operand op = Operand.Create(dt);
        assertTrue(op.IsDate());
        assertEquals(2024, (int) op.DateValue().Year);
        assertEquals(1, (int) op.DateValue().Month);
        assertEquals(15, (int) op.DateValue().Day);
        assertEquals(10, op.DateValue().Hour);
        assertEquals(30, op.DateValue().Minute);
        assertEquals(45, op.DateValue().Second);
    }

    @Test
    public void Operand_Create_TimeSpan_Test() {
        MyDate ts = new MyDate(null, null, 1, 2, 3, 4);
        Operand op = Operand.Create(ts);
        assertTrue(op.IsDate());
        assertEquals(1, (int) op.DateValue().Day);
        assertEquals(2, op.DateValue().Hour);
        assertEquals(3, op.DateValue().Minute);
        assertEquals(4, op.DateValue().Second);
    }

    @Test
    public void Operand_Create_List_Test() {
        List<Operand> list = new ArrayList<Operand>();
        list.add(Operand.Create(1));
        list.add(Operand.Create(2));
        list.add(Operand.Create(3));
        Operand op = Operand.Create(list);
        assertTrue(op.IsArray());
        assertEquals(3, op.ArrayValue().size());
        assertEquals(1, op.ArrayValue().get(0).IntValue());
        assertEquals(2, op.ArrayValue().get(1).IntValue());
        assertEquals(3, op.ArrayValue().get(2).IntValue());
    }

    @Test
    public void Operand_Create_Collection_String_Test() {
        List<Operand> list = new ArrayList<Operand>();
        list.add(Operand.Create("a"));
        list.add(Operand.Create("b"));
        list.add(Operand.Create("c"));
        Operand op = Operand.Create(list);
        assertTrue(op.IsArray());
        assertEquals(3, op.ArrayValue().size());
        assertEquals("a", op.ArrayValue().get(0).TextValue());
        assertEquals("b", op.ArrayValue().get(1).TextValue());
        assertEquals("c", op.ArrayValue().get(2).TextValue());
    }

    @Test
    public void Operand_Create_Collection_Int_Test() {
        List<Operand> list = new ArrayList<Operand>();
        list.add(Operand.Create(1));
        list.add(Operand.Create(2));
        list.add(Operand.Create(3));
        Operand op = Operand.Create(list);
        assertTrue(op.IsArray());
        assertEquals(3, op.ArrayValue().size());
        assertEquals(1, op.ArrayValue().get(0).IntValue());
        assertEquals(2, op.ArrayValue().get(1).IntValue());
        assertEquals(3, op.ArrayValue().get(2).IntValue());
    }

    @Test
    public void Operand_Create_Collection_Double_Test() {
        List<Operand> list = new ArrayList<Operand>();
        list.add(Operand.Create(1.1));
        list.add(Operand.Create(2.2));
        list.add(Operand.Create(3.3));
        Operand op = Operand.Create(list);
        assertTrue(op.IsArray());
        assertEquals(3, op.ArrayValue().size());
        assertEquals(1.1, op.ArrayValue().get(0).DoubleValue(), 0.001);
        assertEquals(2.2, op.ArrayValue().get(1).DoubleValue(), 0.001);
        assertEquals(3.3, op.ArrayValue().get(2).DoubleValue(), 0.001);
    }

    @Test
    public void Operand_Create_Collection_Bool_Test() {
        List<Operand> list = new ArrayList<Operand>();
        list.add(Operand.Create(true));
        list.add(Operand.Create(false));
        list.add(Operand.Create(true));
        Operand op = Operand.Create(list);
        assertTrue(op.IsArray());
        assertEquals(3, op.ArrayValue().size());
        assertEquals(true, op.ArrayValue().get(0).BooleanValue());
        assertEquals(false, op.ArrayValue().get(1).BooleanValue());
        assertEquals(true, op.ArrayValue().get(2).BooleanValue());
    }

    @Test
    public void Operand_CreateJson_Test() {
        Operand op = Operand.CreateJson("{\"name\":\"test\",\"value\":123}");
        assertTrue(op.IsJson());

        op = Operand.CreateJson("[1,2,3]");
        assertTrue(op.IsJson());

        op = Operand.CreateJson("invalid json");
        assertTrue(op.IsError());
    }

    @Test
    public void Operand_Error_Test() {
        Operand op = Operand.Error("test error");
        assertTrue(op.IsError());
        assertEquals("test error", op.ErrorMsg());

        op = Operand.Error("error {0}", "param");
        assertTrue(op.IsError());
        assertEquals("error param", op.ErrorMsg());
    }

    @Test
    public void Operand_CreateNull_Test() {
        Operand op = Operand.Null;
        assertTrue(op.IsNull());
    }

    // endregion

    // region OperandInt 测试

    @Test
    public void OperandInt_Properties_Test() {
        Operand op = Operand.Create(42);
        assertEquals(OperandType.NUMBER, op.Type());
        assertTrue(op.IsNumber());
        assertFalse(op.IsNull());
        assertFalse(op.IsText());
        assertFalse(op.IsBoolean());
        assertFalse(op.IsArray());
        assertFalse(op.IsDate());
        assertFalse(op.IsJson());
        assertFalse(op.IsError());
    }

    @Test
    public void OperandInt_ToNumber_Test() {
        Operand op = Operand.Create(42);
        Operand result = op.ToNumber(null);
        assertTrue(result.IsNumber());
        assertEquals(42, result.IntValue());
    }

    @Test
    public void OperandInt_ToBoolean_Test() {
        Operand op = Operand.Create(0);
        Operand result = op.ToBoolean(null);
        assertTrue(result.IsBoolean());
        assertEquals(false, result.BooleanValue());

        op = Operand.Create(1);
        result = op.ToBoolean(null);
        assertTrue(result.IsBoolean());
        assertEquals(true, result.BooleanValue());

        op = Operand.Create(2);
        result = op.ToBoolean(null);
        assertTrue(result.IsError());
    }

    @Test
    public void OperandInt_ToText_Test() {
        Operand op = Operand.Create(42);
        Operand result = op.ToText(null);
        assertTrue(result.IsText());
        assertEquals("42", result.TextValue());
    }

    @Test
    public void OperandInt_ToString_Test() {
        Operand op = Operand.Create(42);
        assertEquals("42", op.toString());
    }

    // endregion

    // region OperandDouble 测试

    @Test
    public void OperandDouble_Properties_Test() {
        Operand op = Operand.Create(3.14);
        assertEquals(OperandType.NUMBER, op.Type());
        assertTrue(op.IsNumber());
        assertEquals(3.14, op.DoubleValue(), 0.001);
        assertEquals(0, new BigDecimal("3.14").compareTo(op.NumberValue()));
        assertEquals(3, op.IntValue());
        assertEquals(3L, op.LongValue());
    }

    @Test
    public void OperandDouble_ToBoolean_Test() {
        Operand op = Operand.Create(0.0);
        Operand result = op.ToBoolean(null);
        assertTrue(result.IsBoolean());
        assertEquals(false, result.BooleanValue());

        op = Operand.Create(1.0);
        result = op.ToBoolean(null);
        assertTrue(result.IsBoolean());
        assertEquals(true, result.BooleanValue());

        op = Operand.Create(2.5);
        result = op.ToBoolean(null);
        assertTrue(result.IsError());
    }

    @Test
    public void OperandDouble_ToText_Test() {
        Operand op = Operand.Create(3.14);
        Operand result = op.ToText(null);
        assertTrue(result.IsText());
        assertEquals("3.14", result.TextValue());
    }

    // endregion

    // region OperandDecimal 测试

    @Test
    public void OperandDecimal_Properties_Test() {
        Operand op = Operand.Create(new BigDecimal("123.456"));
        assertEquals(OperandType.NUMBER, op.Type());
        assertTrue(op.IsNumber());
        assertEquals(0, new BigDecimal("123.456").compareTo(op.NumberValue()));
        assertEquals(123.456, op.DoubleValue(), 0.001);
        assertEquals(123, op.IntValue());
    }

    @Test
    public void OperandDecimal_ToBoolean_Test() {
        Operand op = Operand.Create(new BigDecimal("0"));
        Operand result = op.ToBoolean(null);
        assertTrue(result.IsBoolean());
        assertEquals(false, result.BooleanValue());

        op = Operand.Create(new BigDecimal("1"));
        result = op.ToBoolean(null);
        assertTrue(result.IsBoolean());
        assertEquals(true, result.BooleanValue());
    }

    @Test
    public void OperandDecimal_ToText_Test() {
        Operand op = Operand.Create(new BigDecimal("123.45"));
        Operand result = op.ToText(null);
        assertTrue(result.IsText());
        assertEquals("123.45", result.TextValue());
    }

    // endregion

    // region OperandBoolean 测试

    @Test
    public void OperandBoolean_Properties_Test() {
        Operand op = Operand.Create(true);
        assertEquals(OperandType.BOOLEAN, op.Type());
        assertTrue(op.IsBoolean());
        assertFalse(op.IsNumber());
        assertEquals(true, op.BooleanValue());

        op = Operand.Create(false);
        assertEquals(OperandType.BOOLEAN, op.Type());
        assertTrue(op.IsBoolean());
        assertEquals(false, op.BooleanValue());
    }

    @Test
    public void OperandBoolean_ToNumber_Test() {
        Operand op = Operand.Create(true);
        Operand result = op.ToNumber(null);
        assertTrue(result.IsNumber());
        assertEquals(1, result.IntValue());

        op = Operand.Create(false);
        result = op.ToNumber(null);
        assertTrue(result.IsNumber());
        assertEquals(0, result.IntValue());
    }

    @Test
    public void OperandBoolean_ToText_Test() {
        Operand op = Operand.Create(true);
        Operand result = op.ToText(null);
        assertTrue(result.IsText());
        assertEquals("TRUE", result.TextValue());

        op = Operand.Create(false);
        result = op.ToText(null);
        assertTrue(result.IsText());
        assertEquals("FALSE", result.TextValue());
    }

    @Test
    public void OperandBoolean_ToString_Test() {
        Operand op = Operand.Create(true);
        assertEquals("true", op.toString());

        op = Operand.Create(false);
        assertEquals("false", op.toString());
    }

    // endregion

    // region OperandString 测试

    @Test
    public void OperandString_Properties_Test() {
        Operand op = Operand.Create("hello world");
        assertEquals(OperandType.TEXT, op.Type());
        assertTrue(op.IsText());
        assertFalse(op.IsNumber());
        assertFalse(op.IsBoolean());
        assertEquals("hello world", op.TextValue());
    }

    @Test
    public void OperandString_ToNumber_Test() {
        Operand op = Operand.Create("42");
        Operand result = op.ToNumber(null);
        assertTrue(result.IsNumber());
        assertEquals(42, result.IntValue());

        op = Operand.Create("3.14");
        result = op.ToNumber(null);
        assertTrue(result.IsNumber());
        assertEquals(3.14, result.DoubleValue(), 0.001);

        op = Operand.Create("not a number");
        result = op.ToNumber(null);
        assertTrue(result.IsError());
    }

    @Test
    public void OperandString_ToBoolean_Test() {
        Operand op = Operand.Create("true");
        Operand result = op.ToBoolean(null);
        assertTrue(result.IsBoolean());
        assertEquals(true, result.BooleanValue());

        op = Operand.Create("false");
        result = op.ToBoolean(null);
        assertTrue(result.IsBoolean());
        assertEquals(false, result.BooleanValue());

        op = Operand.Create("TRUE");
        result = op.ToBoolean(null);
        assertTrue(result.IsBoolean());
        assertEquals(true, result.BooleanValue());

        op = Operand.Create("not bool");
        result = op.ToBoolean(null);
        assertTrue(result.IsError());
    }

    @Test
    public void OperandString_ToMyDate_Test() {
        Operand op = Operand.Create("2024-01-15");
        Operand result = op.ToMyDate(null);
        assertTrue(result.IsDate());
        assertEquals(2024, (int) result.DateValue().Year);
        assertEquals(1, (int) result.DateValue().Month);
        assertEquals(15, (int) result.DateValue().Day);

        op = Operand.Create("10:30:45");
        result = op.ToMyDate(null);
        assertTrue(result.IsDate());
        assertEquals(10, result.DateValue().Hour);
        assertEquals(30, result.DateValue().Minute);
        assertEquals(45, result.DateValue().Second);

        op = Operand.Create("not a date");
        result = op.ToMyDate(null);
        assertTrue(result.IsError());
    }

    @Test
    public void OperandString_ToJson_Test() {
        Operand op = Operand.Create("{\"name\":\"test\"}");
        Operand result = op.ToJson(null);
        assertTrue(result.IsJson());

        op = Operand.Create("[1,2,3]");
        result = op.ToJson(null);
        assertTrue(result.IsJson());

        op = Operand.Create("not json");
        result = op.ToJson(null);
        assertTrue(result.IsError());
    }

    @Test
    public void OperandString_ToArray_Test() {
        Operand op = Operand.Create("test");
        Operand result = op.ToArray(null);
        assertTrue(result.IsError());
    }

    @Test
    public void OperandString_ToString_Test() {
        Operand op = Operand.Create("hello");
        assertEquals("\"hello\"", op.toString());

        op = Operand.Create("hello\"world");
        assertEquals("\"hello\\\"world\"", op.toString());

        op = Operand.Create("hello\nworld");
        assertEquals("\"hello\\nworld\"", op.toString());

        op = Operand.Create("hello\tworld");
        assertEquals("\"hello\\tworld\"", op.toString());
    }

    // endregion

    // region OperandNull 测试

    @Test
    public void OperandNull_Properties_Test() {
        Operand op = Operand.Null;
        assertEquals(OperandType.NULL, op.Type());
        assertTrue(op.IsNull());
        assertFalse(op.IsNumber());
        assertFalse(op.IsText());
        assertFalse(op.IsBoolean());
        assertFalse(op.IsError());
    }

    @Test
    public void OperandNull_ToString_Test() {
        Operand op = Operand.Null;
        assertEquals("null", op.toString());
    }

    // endregion

    // region OperandError 测试

    @Test
    public void OperandError_Properties_Test() {
        Operand op = Operand.Error("test error message");
        assertEquals(OperandType.ERROR, op.Type());
        assertTrue(op.IsError());
        assertFalse(op.IsNull());
        assertFalse(op.IsNumber());
        assertFalse(op.IsText());
        assertEquals("test error message", op.ErrorMsg());
    }

    @Test
    public void OperandError_Conversions_Test() {
        Operand op = Operand.Error("error");

        assertTrue(op.ToNumber(null).IsError());
        assertTrue(op.ToBoolean(null).IsError());
        assertTrue(op.ToText(null).IsError());
        assertTrue(op.ToArray(null).IsError());
        assertTrue(op.ToMyDate(null).IsError());
        assertTrue(op.ToJson(null).IsError());
    }

    // endregion

    // region OperandArray 测试

    @Test
    public void OperandArray_Properties_Test() {
        List<Operand> list = new ArrayList<Operand>();
        list.add(Operand.Create(1));
        list.add(Operand.Create(2));
        list.add(Operand.Create(3));
        Operand op = Operand.Create(list);
        assertEquals(OperandType.ARRAY, op.Type());
        assertTrue(op.IsArray());
        assertFalse(op.IsNumber());
        assertFalse(op.IsText());
        assertEquals(3, op.ArrayValue().size());
    }

    @Test
    public void OperandArray_ToText_Test() {
        List<Operand> list = new ArrayList<Operand>();
        list.add(Operand.Create(1));
        list.add(Operand.Create(2));
        list.add(Operand.Create(3));
        Operand op = Operand.Create(list);
        Operand result = op.ToText(null);
        assertTrue(result.IsText());
        assertEquals("[1,2,3]", result.TextValue());
    }

    @Test
    public void OperandArray_ToArray_Test() {
        List<Operand> list = new ArrayList<Operand>();
        list.add(Operand.Create(1));
        list.add(Operand.Create(2));
        Operand op = Operand.Create(list);
        Operand result = op.ToArray("");
        assertTrue(result.IsArray());
        assertEquals(2, result.ArrayValue().size());
    }

    @Test
    public void OperandArray_ToJson_Test() {
        List<Operand> list = new ArrayList<Operand>();
        list.add(Operand.Create(1));
        list.add(Operand.Create(2));
        Operand op = Operand.Create(list);
        Operand result = op.ToJson(null);
        assertTrue(result.IsJson());
    }

    @Test
    public void OperandArray_ToString_Test() {
        List<Operand> list = new ArrayList<Operand>();
        list.add(Operand.Create(1));
        list.add(Operand.Create("test"));
        list.add(Operand.Create(true));
        Operand op = Operand.Create(list);
        assertEquals("[1,\"test\",true]", op.toString());
    }

    // endregion

    // region OperandMyDate 测试

    @Test
    public void OperandMyDate_Properties_Test() {
        MyDate dt = new MyDate(2024, 6, 15, 10, 30, 45);
        Operand op = Operand.Create(dt);
        assertEquals(OperandType.DATE, op.Type());
        assertTrue(op.IsDate());
        assertFalse(op.IsNumber());
        assertFalse(op.IsText());
        assertEquals(2024, (int) op.DateValue().Year);
        assertEquals(6, (int) op.DateValue().Month);
        assertEquals(15, (int) op.DateValue().Day);
        assertEquals(10, op.DateValue().Hour);
        assertEquals(30, op.DateValue().Minute);
        assertEquals(45, op.DateValue().Second);
    }

    @Test
    public void OperandMyDate_ToText_Test() {
        MyDate dt = new MyDate(2024, 6, 15, 10, 30, 45);
        Operand op = Operand.Create(dt);
        Operand result = op.ToText(null);
        assertTrue(result.IsText());
    }

    @Test
    public void OperandMyDate_ToMyDate_Test() {
        MyDate dt = new MyDate(2024, 6, 15, 10, 30, 45);
        Operand op = Operand.Create(dt);
        Operand result = op.ToMyDate(null);
        assertTrue(result.IsDate());
        assertEquals(2024, (int) result.DateValue().Year);
    }

    @Test
    public void OperandMyDate_ToString_Test() {
        MyDate dt = new MyDate(2024, 6, 15, 10, 30, 45);
        Operand op = Operand.Create(dt);
        assertTrue(op.toString().startsWith("\"2024-06-15"));
    }

    // endregion
}
