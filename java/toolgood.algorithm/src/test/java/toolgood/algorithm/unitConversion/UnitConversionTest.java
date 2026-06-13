package toolgood.algorithm.unitconversion;

import org.junit.Test;
import static org.junit.Assert.*;

import toolgood.algorithm.AlgorithmEngineHelper;

import java.math.BigDecimal;

public class UnitConversionTest {

    // region 距离单位转换测试

    @Test
    public void DistanceConverter_MeterToKilometer_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1000"), "m", "km", "测试");
        assertEquals(0, new BigDecimal("1").compareTo(result));
    }

    @Test
    public void DistanceConverter_KilometerToMeter_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "km", "m", "测试");
        assertEquals(0, new BigDecimal("1000").compareTo(result));
    }

    @Test
    public void DistanceConverter_MeterToCentimeter_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "m", "cm", "测试");
        assertEquals(0, new BigDecimal("100").compareTo(result));
    }

    @Test
    public void DistanceConverter_MeterToMillimeter_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "m", "mm", "测试");
        assertEquals(0, new BigDecimal("1000").compareTo(result));
    }

    @Test
    public void DistanceConverter_MeterToDecimeter_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "m", "dm", "测试");
        assertEquals(0, new BigDecimal("10").compareTo(result));
    }

    @Test
    public void DistanceConverter_CentimeterToMeter_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("100"), "cm", "m", "测试");
        assertEquals(0, new BigDecimal("1").compareTo(result));
    }

    @Test
    public void DistanceConverter_MillimeterToMeter_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1000"), "mm", "m", "测试");
        assertEquals(0, new BigDecimal("1").compareTo(result));
    }

    @Test
    public void DistanceConverter_ChineseUnit_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "米", "千米", "测试");
        assertEquals(0, new BigDecimal("0.001").compareTo(result));

        result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "米", "厘米", "测试");
        assertEquals(0, new BigDecimal("100").compareTo(result));

        result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "米", "毫米", "测试");
        assertEquals(0, new BigDecimal("1000").compareTo(result));
    }

    @Test
    public void DistanceConverter_FootToMeter_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "ft", "m", "测试");
        assertEquals(0.3048, result.doubleValue(), 0.0001);
    }

    @Test
    public void DistanceConverter_InchToMeter_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "in", "m", "测试");
        assertEquals(0.0254, result.doubleValue(), 0.0001);
    }

    @Test
    public void DistanceConverter_YardToMeter_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "yd", "m", "测试");
        assertEquals(0.9144, result.doubleValue(), 0.0001);
    }

    @Test
    public void DistanceConverter_MileToMeter_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "mile", "m", "测试");
        assertEquals(1609.344, result.doubleValue(), 0.1);
    }

    // endregion

    // region 面积单位转换测试

    @Test
    public void AreaConverter_SquareMeterToSquareKilometer_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1000000"), "m2", "km2", "测试");
        assertEquals(0, new BigDecimal("1").compareTo(result));
    }

    @Test
    public void AreaConverter_SquareMeterToSquareCentimeter_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "m2", "cm2", "测试");
        assertEquals(0, new BigDecimal("10000").compareTo(result));
    }

    @Test
    public void AreaConverter_SquareMeterToSquareMillimeter_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "m2", "mm2", "测试");
        assertEquals(0, new BigDecimal("1000000").compareTo(result));
    }

    @Test
    public void AreaConverter_SquareMeterToSquareDecimeter_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "m2", "dm2", "测试");
        assertEquals(0, new BigDecimal("100").compareTo(result));
    }

    @Test
    public void AreaConverter_ChineseUnit_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "平方米", "平方厘米", "测试");
        assertEquals(0, new BigDecimal("10000").compareTo(result));

        result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "平方米", "平方毫米", "测试");
        assertEquals(0, new BigDecimal("1000000").compareTo(result));
    }

    @Test
    public void AreaConverter_HectareToSquareMeter_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "ha", "m2", "测试");
        assertEquals(0, new BigDecimal("10000").compareTo(result));
    }

    @Test
    public void AreaConverter_MuToSquareMeter_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "亩", "m2", "测试");
        assertEquals(666.667, result.doubleValue(), 0.1);
    }

    @Test
    public void AreaConverter_SquareFootToSquareMeter_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "ft2", "m2", "测试");
        assertEquals(0.092903, result.doubleValue(), 0.0001);
    }

    // endregion

    // region 体积单位转换测试

    @Test
    public void VolumeConverter_CubicMeterToLiter_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "m3", "L", "测试");
        assertEquals(0, new BigDecimal("1000").compareTo(result));
    }

    @Test
    public void VolumeConverter_LiterToMilliliter_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "L", "cm3", "测试");
        assertEquals(0, new BigDecimal("1000").compareTo(result));
    }

    @Test
    public void VolumeConverter_CubicMeterToCubicCentimeter_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "m3", "cm3", "测试");
        assertEquals(0, new BigDecimal("1000000").compareTo(result));
    }

    @Test
    public void VolumeConverter_CubicMeterToCubicMillimeter_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "m3", "mm3", "测试");
        assertEquals(0, new BigDecimal("1000000000").compareTo(result));
    }

    @Test
    public void VolumeConverter_ChineseUnit_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "立方米", "升", "测试");
        assertEquals(0, new BigDecimal("1000").compareTo(result));

        result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "升", "毫升", "测试");
        assertEquals(0, new BigDecimal("1000").compareTo(result));
    }

    @Test
    public void VolumeConverter_CubicFootToLiter_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "ft3", "L", "测试");
        assertEquals(28.3168, result.doubleValue(), 0.1);
    }

    @Test
    public void VolumeConverter_USGallonToLiter_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "US gallon", "L", "测试");
        assertEquals(3.78541, result.doubleValue(), 0.1);
    }

    // endregion

    // region 质量单位转换测试

    @Test
    public void MassConverter_KilogramToGram_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "kg", "g", "测试");
        assertEquals(0, new BigDecimal("1000").compareTo(result));
    }

    @Test
    public void MassConverter_KilogramToTon_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1000"), "kg", "t", "测试");
        assertEquals(0, new BigDecimal("1").compareTo(result));
    }

    @Test
    public void MassConverter_TonToKilogram_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "t", "kg", "测试");
        assertEquals(0, new BigDecimal("1000").compareTo(result));
    }

    @Test
    public void MassConverter_GramToKilogram_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1000"), "g", "kg", "测试");
        assertEquals(0, new BigDecimal("1").compareTo(result));
    }

    @Test
    public void MassConverter_ChineseUnit_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "千克", "克", "测试");
        assertEquals(0, new BigDecimal("1000").compareTo(result));

        result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "吨", "千克", "测试");
        assertEquals(0, new BigDecimal("1000").compareTo(result));
    }

    @Test
    public void MassConverter_PoundToKilogram_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "lb", "kg", "测试");
        assertEquals(0.453592, result.doubleValue(), 0.0001);
    }

    @Test
    public void MassConverter_OunceToKilogram_Test() throws Exception {
        BigDecimal result = AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "oz", "kg", "测试");
        assertEquals(0.0283495, result.doubleValue(), 0.0001);
    }

    // endregion

    // region 无效单位测试

    @Test
    public void UnitConversion_InvalidUnit_Test() {
        boolean hasException = false;
        try {
            AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "invalid", "m", "测试");
        } catch (Exception e) {
            hasException = true;
        }
        assertTrue(hasException);

        hasException = false;
        try {
            AlgorithmEngineHelper.UnitConversion(new BigDecimal("1"), "m", "invalid", "测试");
        } catch (Exception e) {
            hasException = true;
        }
        assertTrue(hasException);
    }

    // endregion
}