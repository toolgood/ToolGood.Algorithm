package toolgood.algorithm.algorithmengineex;

import org.junit.Test;
import static org.junit.Assert.*;

import toolgood.algorithm.AlgorithmEngineEx;
import toolgood.algorithm.MyDate;
import toolgood.algorithm.Operand;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

public class AlgorithmEngineExTest {

    // region 构造函数测试

    @Test
    public void Constructor_Default_Test() {
        AlgorithmEngineEx engine = new AlgorithmEngineEx();
        assertFalse(engine.IgnoreCase);
        assertFalse(engine.UseTempDict);
    }

    @Test
    public void Constructor_IgnoreCase_Test() {
        AlgorithmEngineEx engine = new AlgorithmEngineEx(true);
        assertTrue(engine.IgnoreCase);

        engine = new AlgorithmEngineEx(false);
        assertFalse(engine.IgnoreCase);
    }

    // endregion

    // region AddParameter 测试

    @Test
    public void AddParameter_Bool_Test() {
        AlgorithmEngineEx engine = new AlgorithmEngineEx();
        engine.AddParameter("flag", true);
        boolean result = engine.TryEvaluate("flag", false);
        assertEquals(true, result);

        engine.AddParameter("flag", false);
        result = engine.TryEvaluate("flag", true);
        assertEquals(false, result);
    }

    @Test
    public void AddParameter_Int_Test() {
        AlgorithmEngineEx engine = new AlgorithmEngineEx();
        engine.AddParameter("num", 42);
        int result = engine.TryEvaluate("num", 0);
        assertEquals(42, result);
    }

    @Test
    public void AddParameter_Long_Test() {
        AlgorithmEngineEx engine = new AlgorithmEngineEx();
        engine.AddParameter("num", 123456789L);
        long result = engine.TryEvaluate("num", 0L);
        assertEquals(123456789L, result);
    }

    @Test
    public void AddParameter_Double_Test() {
        AlgorithmEngineEx engine = new AlgorithmEngineEx();
        engine.AddParameter("num", 3.14159);
        double result = engine.TryEvaluate("num", 0.0);
        assertEquals(3.14159, result, 0.00001);
    }

    @Test
    public void AddParameter_Decimal_Test() {
        AlgorithmEngineEx engine = new AlgorithmEngineEx();
        engine.AddParameter("num", new BigDecimal("123.45"));
        BigDecimal result = engine.TryEvaluate("num", BigDecimal.ZERO);
        assertEquals(0, new BigDecimal("123.45").compareTo(result));
    }

    @Test
    public void AddParameter_String_Test() {
        AlgorithmEngineEx engine = new AlgorithmEngineEx();
        engine.AddParameter("name", "ToolGood");
        String result = engine.TryEvaluate("name", "");
        assertEquals("ToolGood", result);
    }

    @Test
    public void AddParameter_DateTime_Test() {
        AlgorithmEngineEx engine = new AlgorithmEngineEx();
        MyDate dt = new MyDate(2024, 6, 15, 10, 30, 0);
        engine.AddParameter("dt", dt);
        int result = engine.TryEvaluate("year(dt)", 0);
        assertEquals(2024, result);
        result = engine.TryEvaluate("month(dt)", 0);
        assertEquals(6, result);
        result = engine.TryEvaluate("day(dt)", 0);
        assertEquals(15, result);
    }

    @Test
    public void AddParameter_TimeSpan_Test() {
        AlgorithmEngineEx engine = new AlgorithmEngineEx();
        MyDate ts = new MyDate(null, null, null, 10, 30, 0);
        engine.AddParameter("ti", ts);
        int result = engine.TryEvaluate("hour(ti)", 0);
        assertEquals(10, result);
        result = engine.TryEvaluate("minute(ti)", 0);
        assertEquals(30, result);
    }

    @Test
    public void AddParameter_List_Test() {
        AlgorithmEngineEx engine = new AlgorithmEngineEx();
        List<Operand> list = new ArrayList<Operand>();
        list.add(Operand.Create(1));
        list.add(Operand.Create(2));
        list.add(Operand.Create(3));
        engine.AddParameter("arr", list);
        int result = engine.TryEvaluate("count(arr)", 0);
        assertEquals(3, result);
        result = engine.TryEvaluate("arr[1]", 0);
        assertEquals(1, result);
    }

    @Test
    public void AddParameter_Collection_String_Test() {
        AlgorithmEngineEx engine = new AlgorithmEngineEx();
        List<Operand> list = new ArrayList<Operand>();
        list.add(Operand.Create("a"));
        list.add(Operand.Create("b"));
        list.add(Operand.Create("c"));
        engine.AddParameter("names", Operand.Create(list));
        int result = engine.TryEvaluate("count(names)", 0);
        assertEquals(3, result);
    }

    @Test
    public void AddParameter_Collection_Int_Test() {
        AlgorithmEngineEx engine = new AlgorithmEngineEx();
        List<Operand> list = new ArrayList<Operand>();
        list.add(Operand.Create(1));
        list.add(Operand.Create(2));
        list.add(Operand.Create(3));
        list.add(Operand.Create(4));
        list.add(Operand.Create(5));
        engine.AddParameter("nums", Operand.Create(list));
        int result = engine.TryEvaluate("sum(nums)", 0);
        assertEquals(15, result);
    }

    @Test
    public void AddParameter_Collection_Double_Test() {
        AlgorithmEngineEx engine = new AlgorithmEngineEx();
        List<Operand> list = new ArrayList<Operand>();
        list.add(Operand.Create(1.1));
        list.add(Operand.Create(2.2));
        list.add(Operand.Create(3.3));
        engine.AddParameter("nums", Operand.Create(list));
        double result = engine.TryEvaluate("sum(nums)", 0.0);
        assertEquals(6.6, result, 0.01);
    }

    @Test
    public void AddParameter_Collection_Bool_Test() {
        AlgorithmEngineEx engine = new AlgorithmEngineEx();
        List<Operand> list = new ArrayList<Operand>();
        list.add(Operand.Create(true));
        list.add(Operand.Create(false));
        list.add(Operand.Create(true));
        engine.AddParameter("flags", Operand.Create(list));
        int result = engine.TryEvaluate("count(flags)", 0);
        assertEquals(3, result);
    }

    @Test
    public void AddParameter_Operand_Test() {
        AlgorithmEngineEx engine = new AlgorithmEngineEx();
        engine.AddParameter("val", Operand.Create(100));
        int result = engine.TryEvaluate("val", 0);
        assertEquals(100, result);
    }

    // endregion

    // region AddParameterFromJson 测试

    @Test
    public void AddParameterFromJson_Test() throws Exception {
        AlgorithmEngineEx engine = new AlgorithmEngineEx();
        engine.AddParameterFromJson("{\"name\":\"test\",\"age\":25,\"active\":true}");

        String result = engine.TryEvaluate("name", "");
        assertEquals("test", result);

        int result2 = engine.TryEvaluate("age", 0);
        assertEquals(25, result2);

        boolean boolResult = engine.TryEvaluate("active", false);
        assertEquals(true, boolResult);
    }

    @Test
    public void AddParameterFromJson_Complex_Test() throws Exception {
        AlgorithmEngineEx engine = new AlgorithmEngineEx();
        engine.AddParameterFromJson("{\"灰色\":\"L\",\"canBookCount\":905,\"saleCount\":91}");

        String result = engine.TryEvaluate("灰色", "");
        assertEquals("L", result);

        int result2 = engine.TryEvaluate("canBookCount", 0);
        assertEquals(905, result2);
    }

    @Test
    public void AddParameterFromJson_Invalid_Test() {
        AlgorithmEngineEx engine = new AlgorithmEngineEx();
        boolean hasException = false;
        try {
            engine.AddParameterFromJson("not a json");
        } catch (Exception e) {
            hasException = true;
        }
        assertTrue(hasException);
    }

    // endregion

    // region ClearParameters 测试

    @Test
    public void ClearParameters_Test() {
        AlgorithmEngineEx engine = new AlgorithmEngineEx();
        engine.AddParameter("num", 42);
        int result = engine.TryEvaluate("num", 0);
        assertEquals(42, result);

        engine.ClearParameters();
        result = engine.TryEvaluate("num", -1);
        assertEquals(-1, result);
    }

    // endregion

    // region UseTempDict 测试

    @Test
    public void UseTempDict_Test() {
        AlgorithmEngineEx engine = new AlgorithmEngineEx();
        engine.UseTempDict = true;
        engine.AddParameter("num", 42);

        int result = engine.TryEvaluate("num", 0);
        assertEquals(42, result);
    }

    // endregion

    // region IgnoreCase 测试

    @Test
    public void IgnoreCase_True_Test() {
        AlgorithmEngineEx engine = new AlgorithmEngineEx(true);
        engine.AddParameter("Name", "Test");

        String result = engine.TryEvaluate("name", "");
        assertEquals("Test", result);

        result = engine.TryEvaluate("NAME", "");
        assertEquals("Test", result);

        result = engine.TryEvaluate("Name", "");
        assertEquals("Test", result);
    }

    @Test
    public void IgnoreCase_False_Test() {
        AlgorithmEngineEx engine = new AlgorithmEngineEx(false);
        engine.AddParameter("Name", "Test");

        String result = engine.TryEvaluate("Name", "");
        assertEquals("Test", result);

        result = engine.TryEvaluate("name", "default");
        assertEquals("default", result);
    }

    // endregion

    // region GetParameterEx 测试

    @Test
    public void GetParameterEx_Missing_Test() {
        AlgorithmEngineEx engine = new AlgorithmEngineEx();
        String result = engine.TryEvaluate("missing_param", "default");
        assertEquals("default", result);
    }

    // endregion

    // region 综合测试

    @Test
    public void Complex_Expression_Test() {
        AlgorithmEngineEx engine = new AlgorithmEngineEx();
        engine.AddParameter("a", 10);
        engine.AddParameter("b", 20);
        engine.AddParameter("c", 30);

        int result = engine.TryEvaluate("a + b + c", 0);
        assertEquals(60, result);

        result = engine.TryEvaluate("a * b - c", 0);
        assertEquals(170, result);

        result = engine.TryEvaluate("if(a > 5, b, c)", 0);
        assertEquals(20, result);
    }

    @Test
    public void Nested_Json_Test() throws Exception {
        AlgorithmEngineEx engine = new AlgorithmEngineEx();
        engine.AddParameterFromJson("{\"data\":{\"name\":\"test\",\"value\":123}}");

        String result = engine.TryEvaluate("json(data)['name']", "");
        assertEquals("test", result);
    }

    @Test
    public void Array_Operations_Test() {
        AlgorithmEngineEx engine = new AlgorithmEngineEx();
        List<Operand> list = new ArrayList<Operand>();
        list.add(Operand.Create(1));
        list.add(Operand.Create(2));
        list.add(Operand.Create(3));
        list.add(Operand.Create(4));
        list.add(Operand.Create(5));
        engine.AddParameter("arr", Operand.Create(list));

        int result = engine.TryEvaluate("sum(arr)", 0);
        assertEquals(15, result);

        double result2 = engine.TryEvaluate("average(arr)", 0.0);
        assertEquals(3.0, result2, 0.01);

        result = engine.TryEvaluate("max(arr)", 0);
        assertEquals(5, result);

        result = engine.TryEvaluate("min(arr)", 0);
        assertEquals(1, result);

        result = engine.TryEvaluate("count(arr)", 0);
        assertEquals(5, result);
    }

    // endregion
}
