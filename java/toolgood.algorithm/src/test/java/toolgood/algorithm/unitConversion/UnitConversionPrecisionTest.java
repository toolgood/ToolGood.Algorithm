package toolgood.algorithm.unitconversion;

import static org.junit.Assert.assertTrue;

import java.math.BigDecimal;

import org.junit.Test;

import toolgood.algorithm.AlgorithmEngineHelper;

/**
 * 精确分数系数防回归测试。
 * 这些系数由 6 位近似值重构为精确分数（如 short ton = 50000/45359237），
 * 反向换算误差应远小于 1e-9。若有人改回近似值，本测试将失败。
 */
public class UnitConversionPrecisionTest {

    private static void assertClose(String src, String tar, BigDecimal value, double expected) throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(value, src, tar, "精度测试");
        double d = result.doubleValue();
        double rel = Math.abs(d - expected) / Math.max(Math.abs(expected), 1e-30);
        assertTrue(rel < 1e-9);
    }

    // region 质量

    @Test
    public void Mass_ShortTon_Exact_Test() throws Exception {
        assertClose("short ton", "kg", BigDecimal.ONE, 907.18474);
        assertClose("kg", "short ton", BigDecimal.ONE, 1.0 / 907.18474);
        assertClose("short ton", "lb", BigDecimal.ONE, 2000);
    }

    @Test
    public void Mass_LongTon_Exact_Test() throws Exception {
        assertClose("long ton", "kg", BigDecimal.ONE, 1016.0469088);
        assertClose("kg", "long ton", BigDecimal.ONE, 1.0 / 1016.0469088);
        assertClose("long ton", "lb", BigDecimal.ONE, 2240);
    }

    // endregion

    // region 面积

    @Test
    public void Area_Mu_Exact_Test() throws Exception {
        assertClose("亩", "m²", BigDecimal.ONE, 666.6666666666667);
        assertClose("m²", "亩", BigDecimal.ONE, 0.0015);
    }

    // endregion

    // region 体积

    @Test
    public void Volume_CubicFoot_Exact_Test() throws Exception {
        assertClose("ft³", "l", BigDecimal.ONE, 28.316846592);
        assertClose("l", "ft³", BigDecimal.ONE, 0.03531466672148859);
        assertClose("ft³", "in³", BigDecimal.ONE, 1728);
    }

    @Test
    public void Volume_CubicInch_Exact_Test() throws Exception {
        assertClose("in³", "l", BigDecimal.ONE, 0.016387064);
        assertClose("l", "in³", BigDecimal.ONE, 61.0237440947323);
    }

    @Test
    public void Volume_ImperialPint_Exact_Test() throws Exception {
        assertClose("imperial pint", "l", BigDecimal.ONE, 0.56826125);
        assertClose("l", "imperial pint", BigDecimal.ONE, 1.7597539863927023);
    }

    @Test
    public void Volume_ImperialGallon_Exact_Test() throws Exception {
        assertClose("imperial gallon", "l", BigDecimal.ONE, 4.54609);
        assertClose("l", "imperial gallon", BigDecimal.ONE, 0.2199692482990878);
        assertClose("imperial gallon", "imperial pint", BigDecimal.ONE, 8);
        assertClose("imperial gallon", "imperial quart", BigDecimal.ONE, 4);
    }

    @Test
    public void Volume_ImperialQuart_Exact_Test() throws Exception {
        assertClose("imperial quart", "l", BigDecimal.ONE, 1.1365225);
        assertClose("l", "imperial quart", BigDecimal.ONE, 0.87987699319635115);
    }

    @Test
    public void Volume_USPint_Exact_Test() throws Exception {
        assertClose("US pint", "l", BigDecimal.ONE, 0.473176473);
        assertClose("l", "US pint", BigDecimal.ONE, 2.1133764188651873);
    }

    @Test
    public void Volume_USGallon_Exact_Test() throws Exception {
        assertClose("US gallon", "l", BigDecimal.ONE, 3.785411784);
        assertClose("l", "US gallon", BigDecimal.ONE, 0.2641720523581484);
        assertClose("US gallon", "US quart", BigDecimal.ONE, 4);
    }

    // endregion
}
