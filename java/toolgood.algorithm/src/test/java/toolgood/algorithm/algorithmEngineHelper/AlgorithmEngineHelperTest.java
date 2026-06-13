package toolgood.algorithm.algorithmenginehelper;

import org.junit.Test;
import static org.junit.Assert.*;

import toolgood.algorithm.AlgorithmEngineHelper;
import toolgood.algorithm.internals.DiyNameInfo;

import java.math.BigDecimal;

public class AlgorithmEngineHelperTest {

    @Test
    public void GetDiyNames_test() throws Exception {
        DiyNameInfo p = AlgorithmEngineHelper.GetDiyNames("dd");
        assertEquals("dd", p.Parameters.get(0).toString());

        DiyNameInfo p3 = AlgorithmEngineHelper.GetDiyNames("dd");
        assertEquals("dd", p3.Parameters.get(0).toString());

        DiyNameInfo p5 = AlgorithmEngineHelper.GetDiyNames("ddd(d1,22)");
        assertEquals("ddd", p5.Functions.get(0).Name);
        assertEquals("d1", p5.Parameters.get(0).toString());

        DiyNameInfo p6 = AlgorithmEngineHelper.GetDiyNames("长");
        assertEquals("长", p6.Parameters.get(0).toString());

        DiyNameInfo p7 = AlgorithmEngineHelper.GetDiyNames("ddd+2");
        assertEquals("ddd", p7.Parameters.get(0).toString());

        DiyNameInfo p8 = AlgorithmEngineHelper.GetDiyNames("{\"A\": 0.6,\"B\": 0.4,\"C\": 0.6,\"E\": 0.33,\"F\": 0.29,\"Z\": 0.15\n"
                + ",\"EB\": 0.7,\"EE\": 0.65,\"EA\": 0.85,\"AB\": 1.0,\"BC\": 1.0,\"AA\":1.0\n"
                + ",\"EBC\": 1.15,\"BAB\": 1.25,\"BCB\": 1.25,\"BBC\": 1.25,\"CBB\": 1.25,\"EBA\": 1.2,\"AAA\": 1.4}[瓦楞]");
        assertEquals("瓦楞", p8.Parameters.get(0).toString());
    }

    @Test
    public void IsParameter_Test() {
        boolean b = AlgorithmEngineHelper.IsParameter("false");
        assertFalse(b);

        b = AlgorithmEngineHelper.IsParameter("f11");
        assertTrue(b);

        b = AlgorithmEngineHelper.IsParameter("f11+1");
        assertFalse(b);

        b = AlgorithmEngineHelper.IsParameter("f11++");
        assertFalse(b);
    }

    @Test
    public void UnitConversion_test() throws Exception {
        BigDecimal b = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "米", "千米", "测试");
        assertEquals(0, new BigDecimal("0.001").compareTo(b));
        b = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "米", "分米", "测试");
        assertEquals(0, new BigDecimal("10").compareTo(b));
        b = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "米", "厘米", "测试");
        assertEquals(0, new BigDecimal("100").compareTo(b));
        b = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "米", "mm", "测试");
        assertEquals(0, new BigDecimal("1000").compareTo(b));

        b = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "m2", "dm2", "测试");
        assertEquals(0, new BigDecimal("100").compareTo(b));
        b = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "m2", "cm2", "测试");
        assertEquals(0, new BigDecimal("10000").compareTo(b));
        b = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "m2", "mm2", "测试");
        assertEquals(0, new BigDecimal("1000000").compareTo(b));

        b = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "m3", "dm3", "测试");
        assertEquals(0, new BigDecimal("1000").compareTo(b));
        b = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "m3", "cm3", "测试");
        assertEquals(0, new BigDecimal("1000000").compareTo(b));
        b = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "m3", "mm3", "测试");
        assertEquals(0, new BigDecimal("1000000000").compareTo(b));

        b = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "t", "kg", "测试");
        assertEquals(0, new BigDecimal("1000").compareTo(b));
        b = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "t", "g", "测试");
        assertEquals(0, new BigDecimal("1000000").compareTo(b));
    }

    @Test
    public void CheckFormula_test() {
        boolean b = AlgorithmEngineHelper.CheckFormula("1+1");
        assertTrue(b);

        b = AlgorithmEngineHelper.CheckFormula("1+");
        assertEquals(false, b);

        b = AlgorithmEngineHelper.CheckFormula("@123+1");
        assertEquals(false, b);
    }
}
