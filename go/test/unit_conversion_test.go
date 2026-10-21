package test

import (
	"testing"

	alg "github.com/toolgood/ToolGood.Algorithm/go/algorithm"
)

// TestUnitConversionTest 移植自
// csharp/ToolGood.Algorithm.Test/UnitConversion/UnitConversionTest.cs。
func TestUnitConversionTest(t *testing.T) {
	mustConvert := func(t *testing.T, src, tar, value string) alg.Decimal {
		t.Helper()
		result, err := alg.UnitConversion(alg.MustDecimal(value), src, tar, "测试")
		if err != nil {
			t.Fatalf("UnitConversion(%s, %s, %s): %v", value, src, tar, err)
		}
		return result
	}

	t.Run("DistanceConverter_MeterToKilometer_Test", func(t *testing.T) {
		assertDecimal(t, "1", mustConvert(t, "m", "km", "1000"))
	})

	t.Run("DistanceConverter_KilometerToMeter_Test", func(t *testing.T) {
		assertDecimal(t, "1000", mustConvert(t, "km", "m", "1"))
	})

	t.Run("DistanceConverter_MeterToCentimeter_Test", func(t *testing.T) {
		assertDecimal(t, "100", mustConvert(t, "m", "cm", "1"))
	})

	t.Run("DistanceConverter_MeterToMillimeter_Test", func(t *testing.T) {
		assertDecimal(t, "1000", mustConvert(t, "m", "mm", "1"))
	})

	t.Run("DistanceConverter_MeterToDecimeter_Test", func(t *testing.T) {
		assertDecimal(t, "10", mustConvert(t, "m", "dm", "1"))
	})

	t.Run("DistanceConverter_CentimeterToMeter_Test", func(t *testing.T) {
		assertDecimal(t, "1", mustConvert(t, "cm", "m", "100"))
	})

	t.Run("DistanceConverter_MillimeterToMeter_Test", func(t *testing.T) {
		assertDecimal(t, "1", mustConvert(t, "mm", "m", "1000"))
	})

	t.Run("DistanceConverter_ChineseUnit_Test", func(t *testing.T) {
		assertDecimal(t, "0.001", mustConvert(t, "米", "千米", "1"))
		assertDecimal(t, "100", mustConvert(t, "米", "厘米", "1"))
		assertDecimal(t, "1000", mustConvert(t, "米", "毫米", "1"))
	})

	t.Run("DistanceConverter_ChiCun_Test", func(t *testing.T) {
		assertDecimal(t, "3", mustConvert(t, "m", "尺", "1"))
		assertDecimal(t, "30", mustConvert(t, "m", "寸", "1"))
		assertDecimal(t, "2", mustConvert(t, "尺", "m", "6"))
		assertDecimal(t, "100", mustConvert(t, "尺", "cm", "3"))
	})

	t.Run("DistanceConverter_FootToMeter_Test", func(t *testing.T) {
		assertFloatRound(t, 0.3048, mustConvert(t, "ft", "m", "1").Float64(), 8)
	})

	t.Run("DistanceConverter_InchToMeter_Test", func(t *testing.T) {
		assertFloatRound(t, 0.0254, mustConvert(t, "in", "m", "1").Float64(), 8)
	})

	t.Run("DistanceConverter_YardToMeter_Test", func(t *testing.T) {
		assertFloatRound(t, 0.9144, mustConvert(t, "yd", "m", "1").Float64(), 8)
	})

	t.Run("DistanceConverter_MileToMeter_Test", func(t *testing.T) {
		// C# 容差 0.1，四舍五入到 1 位等价。
		assertFloatRound(t, 1609.344, mustConvert(t, "mile", "m", "1").Float64(), 1)
	})

	t.Run("AreaConverter_SquareMeterToSquareKilometer_Test", func(t *testing.T) {
		assertDecimal(t, "1", mustConvert(t, "m2", "km2", "1000000"))
	})

	t.Run("AreaConverter_SquareMeterToSquareCentimeter_Test", func(t *testing.T) {
		assertDecimal(t, "10000", mustConvert(t, "m2", "cm2", "1"))
	})

	t.Run("AreaConverter_SquareMeterToSquareMillimeter_Test", func(t *testing.T) {
		assertDecimal(t, "1000000", mustConvert(t, "m2", "mm2", "1"))
	})

	t.Run("AreaConverter_SquareMeterToSquareDecimeter_Test", func(t *testing.T) {
		assertDecimal(t, "100", mustConvert(t, "m2", "dm2", "1"))
	})

	t.Run("AreaConverter_ChineseUnit_Test", func(t *testing.T) {
		assertDecimal(t, "10000", mustConvert(t, "平方米", "平方厘米", "1"))
		assertDecimal(t, "1000000", mustConvert(t, "平方米", "平方毫米", "1"))
	})

	t.Run("AreaConverter_HectareToSquareMeter_Test", func(t *testing.T) {
		assertDecimal(t, "10000", mustConvert(t, "ha", "m2", "1"))
	})

	t.Run("AreaConverter_MuToSquareMeter_Test", func(t *testing.T) {
		// C# 容差 0.1，四舍五入到 1 位等价。
		assertFloatRound(t, 666.667, mustConvert(t, "亩", "m2", "1").Float64(), 1)
	})

	t.Run("AreaConverter_SquareFootToSquareMeter_Test", func(t *testing.T) {
		// C# 容差 0.0001，四舍五入到 4 位等价。
		assertFloatRound(t, 0.092903, mustConvert(t, "ft2", "m2", "1").Float64(), 4)
	})

	t.Run("VolumeConverter_CubicMeterToLiter_Test", func(t *testing.T) {
		assertDecimal(t, "1000", mustConvert(t, "m3", "L", "1"))
	})

	t.Run("VolumeConverter_LiterToMilliliter_Test", func(t *testing.T) {
		assertDecimal(t, "1000", mustConvert(t, "L", "cm3", "1"))
	})

	t.Run("VolumeConverter_CubicMeterToCubicCentimeter_Test", func(t *testing.T) {
		assertDecimal(t, "1000000", mustConvert(t, "m3", "cm3", "1"))
	})

	t.Run("VolumeConverter_CubicMeterToCubicMillimeter_Test", func(t *testing.T) {
		assertDecimal(t, "1000000000", mustConvert(t, "m3", "mm3", "1"))
	})

	t.Run("VolumeConverter_ChineseUnit_Test", func(t *testing.T) {
		assertDecimal(t, "1000", mustConvert(t, "立方米", "升", "1"))
		assertDecimal(t, "1000", mustConvert(t, "升", "毫升", "1"))
	})

	t.Run("VolumeConverter_CubicFootToLiter_Test", func(t *testing.T) {
		// C# 容差 0.1，实际值 28.316846592 在 4 位小数上与期望一致。
		assertFloatRound(t, 28.3168, mustConvert(t, "ft3", "L", "1").Float64(), 4)
	})

	t.Run("VolumeConverter_USGallonToLiter_Test", func(t *testing.T) {
		// C# 容差 0.1，实际值 3.785411784 在 5 位小数上与期望一致。
		assertFloatRound(t, 3.78541, mustConvert(t, "US gallon", "L", "1").Float64(), 5)
	})

	t.Run("MassConverter_KilogramToGram_Test", func(t *testing.T) {
		assertDecimal(t, "1000", mustConvert(t, "kg", "g", "1"))
	})

	t.Run("MassConverter_KilogramToTon_Test", func(t *testing.T) {
		assertDecimal(t, "1", mustConvert(t, "kg", "t", "1000"))
	})

	t.Run("MassConverter_TonToKilogram_Test", func(t *testing.T) {
		assertDecimal(t, "1000", mustConvert(t, "t", "kg", "1"))
	})

	t.Run("MassConverter_GramToKilogram_Test", func(t *testing.T) {
		assertDecimal(t, "1", mustConvert(t, "g", "kg", "1000"))
	})

	t.Run("MassConverter_ChineseUnit_Test", func(t *testing.T) {
		assertDecimal(t, "1000", mustConvert(t, "千克", "克", "1"))
		assertDecimal(t, "1000", mustConvert(t, "吨", "千克", "1"))
	})

	t.Run("MassConverter_Milligram_Test", func(t *testing.T) {
		assertDecimal(t, "1000000", mustConvert(t, "kg", "mg", "1"))
		assertDecimal(t, "1000000", mustConvert(t, "kg", "毫克", "1"))
		assertDecimal(t, "0.0005", mustConvert(t, "mg", "kg", "500"))
	})

	t.Run("MassConverter_JinLiang_Test", func(t *testing.T) {
		assertDecimal(t, "2", mustConvert(t, "kg", "斤", "1"))
		assertDecimal(t, "20", mustConvert(t, "kg", "两", "1"))
		assertDecimal(t, "2.5", mustConvert(t, "斤", "kg", "5"))
		assertDecimal(t, "100", mustConvert(t, "两", "g", "2"))
	})

	t.Run("MassConverter_PoundToKilogram_Test", func(t *testing.T) {
		// C# 容差 0.0001；Go 精确值 0.45359237 在 6 位小数上与期望一致。
		assertFloatRound(t, 0.453592, mustConvert(t, "lb", "kg", "1").Float64(), 6)
	})

	t.Run("MassConverter_OunceToKilogram_Test", func(t *testing.T) {
		// C# 容差 0.0001；Go 精确值 0.0283495231... 在 7 位小数上与期望一致。
		assertFloatRound(t, 0.0283495, mustConvert(t, "oz", "kg", "1").Float64(), 7)
	})

	t.Run("UnitConversion_InvalidUnit_Test", func(t *testing.T) {
		// 差异：C# 抛异常，Go 返回 error。
		_, err := alg.UnitConversion(alg.DecimalOne, "invalid", "m", "测试")
		assertNotNull(t, err)

		_, err = alg.UnitConversion(alg.DecimalOne, "m", "invalid", "测试")
		assertNotNull(t, err)
	})
}
