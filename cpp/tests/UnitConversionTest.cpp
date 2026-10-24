// 移植自 csharp/ToolGood.Algorithm.Test/UnitConversion/UnitConversionTest.cs
//
// 类型对应：
//   AlgorithmEngineHelper.UnitConversion(decimal, src, tar, name)
//     -> AlgorithmEngineHelper::UnitConversion(const Decimal&, src, tar, name) -> Decimal
//   decimal 字面量 1000m -> Decimal::FromInt(1000)，0.001m -> Decimal::FromString("0.001")
//   (double)result       -> result.Float64()
//   try { ... } catch    -> C++ 侧抛 std::runtime_error，用 try/catch 标志位保持原语义
#include "cpptest.h"

#include <stdexcept>
#include <string>

#include "toolgood/AlgorithmEngineHelper.h"

using namespace toolgood;

namespace {

Decimal D(const char* text) { return Decimal::FromString(text); }

// ---------------- 距离单位 ----------------

TEST(UnitConversion, DistanceConverter_MeterToKilometer_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1000), "m", "km", "测试");
    Assert::AreEqual(Decimal::FromInt(1), result);
}

TEST(UnitConversion, DistanceConverter_KilometerToMeter_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "km", "m", "测试");
    Assert::AreEqual(Decimal::FromInt(1000), result);
}

TEST(UnitConversion, DistanceConverter_MeterToCentimeter_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "m", "cm", "测试");
    Assert::AreEqual(Decimal::FromInt(100), result);
}

TEST(UnitConversion, DistanceConverter_MeterToMillimeter_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "m", "mm", "测试");
    Assert::AreEqual(Decimal::FromInt(1000), result);
}

TEST(UnitConversion, DistanceConverter_MeterToDecimeter_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "m", "dm", "测试");
    Assert::AreEqual(Decimal::FromInt(10), result);
}

TEST(UnitConversion, DistanceConverter_CentimeterToMeter_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(100), "cm", "m", "测试");
    Assert::AreEqual(Decimal::FromInt(1), result);
}

TEST(UnitConversion, DistanceConverter_MillimeterToMeter_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1000), "mm", "m", "测试");
    Assert::AreEqual(Decimal::FromInt(1), result);
}

TEST(UnitConversion, DistanceConverter_ChineseUnit_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "米", "千米", "测试");
    Assert::AreEqual(D("0.001"), result);

    result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "米", "厘米", "测试");
    Assert::AreEqual(Decimal::FromInt(100), result);

    result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "米", "毫米", "测试");
    Assert::AreEqual(Decimal::FromInt(1000), result);
}

TEST(UnitConversion, DistanceConverter_ChiCun_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "m", "尺", "测试");
    Assert::AreEqual(Decimal::FromInt(3), result);

    result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "m", "寸", "测试");
    Assert::AreEqual(Decimal::FromInt(30), result);

    result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(6), "尺", "m", "测试");
    Assert::AreEqual(Decimal::FromInt(2), result);

    result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(3), "尺", "cm", "测试");
    Assert::AreEqual(Decimal::FromInt(100), result);
}

TEST(UnitConversion, DistanceConverter_FootToMeter_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "ft", "m", "测试");
    Assert::AreEqual(0.3048, result.Float64(), 0.0001);
}

TEST(UnitConversion, DistanceConverter_InchToMeter_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "in", "m", "测试");
    Assert::AreEqual(0.0254, result.Float64(), 0.0001);
}

TEST(UnitConversion, DistanceConverter_YardToMeter_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "yd", "m", "测试");
    Assert::AreEqual(0.9144, result.Float64(), 0.0001);
}

TEST(UnitConversion, DistanceConverter_MileToMeter_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "mile", "m", "测试");
    Assert::AreEqual(1609.344, result.Float64(), 0.1);
}

// ---------------- 面积单位 ----------------

TEST(UnitConversion, AreaConverter_SquareMeterToSquareKilometer_Test) {
    Decimal result =
        AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1000000), "m2", "km2", "测试");
    Assert::AreEqual(Decimal::FromInt(1), result);
}

TEST(UnitConversion, AreaConverter_SquareMeterToSquareCentimeter_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "m2", "cm2", "测试");
    Assert::AreEqual(Decimal::FromInt(10000), result);
}

TEST(UnitConversion, AreaConverter_SquareMeterToSquareMillimeter_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "m2", "mm2", "测试");
    Assert::AreEqual(Decimal::FromInt(1000000), result);
}

TEST(UnitConversion, AreaConverter_SquareMeterToSquareDecimeter_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "m2", "dm2", "测试");
    Assert::AreEqual(Decimal::FromInt(100), result);
}

TEST(UnitConversion, AreaConverter_ChineseUnit_Test) {
    Decimal result =
        AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "平方米", "平方厘米", "测试");
    Assert::AreEqual(Decimal::FromInt(10000), result);

    result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "平方米", "平方毫米", "测试");
    Assert::AreEqual(Decimal::FromInt(1000000), result);
}

TEST(UnitConversion, AreaConverter_HectareToSquareMeter_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "ha", "m2", "测试");
    Assert::AreEqual(Decimal::FromInt(10000), result);
}

TEST(UnitConversion, AreaConverter_MuToSquareMeter_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "亩", "m2", "测试");
    Assert::AreEqual(666.667, result.Float64(), 0.1);
}

TEST(UnitConversion, AreaConverter_SquareFootToSquareMeter_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "ft2", "m2", "测试");
    Assert::AreEqual(0.092903, result.Float64(), 0.0001);
}

// ---------------- 体积单位 ----------------

TEST(UnitConversion, VolumeConverter_CubicMeterToLiter_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "m3", "L", "测试");
    Assert::AreEqual(Decimal::FromInt(1000), result);
}

TEST(UnitConversion, VolumeConverter_LiterToMilliliter_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "L", "cm3", "测试");
    Assert::AreEqual(Decimal::FromInt(1000), result);
}

TEST(UnitConversion, VolumeConverter_CubicMeterToCubicCentimeter_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "m3", "cm3", "测试");
    Assert::AreEqual(Decimal::FromInt(1000000), result);
}

TEST(UnitConversion, VolumeConverter_CubicMeterToCubicMillimeter_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "m3", "mm3", "测试");
    Assert::AreEqual(Decimal::FromInt(1000000000), result);
}

TEST(UnitConversion, VolumeConverter_ChineseUnit_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "立方米", "升", "测试");
    Assert::AreEqual(Decimal::FromInt(1000), result);

    result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "升", "毫升", "测试");
    Assert::AreEqual(Decimal::FromInt(1000), result);
}

TEST(UnitConversion, VolumeConverter_CubicFootToLiter_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "ft3", "L", "测试");
    Assert::AreEqual(28.3168, result.Float64(), 0.1);
}

TEST(UnitConversion, VolumeConverter_USGallonToLiter_Test) {
    Decimal result =
        AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "US gallon", "L", "测试");
    Assert::AreEqual(3.78541, result.Float64(), 0.1);
}

// ---------------- 质量单位 ----------------

TEST(UnitConversion, MassConverter_KilogramToGram_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "kg", "g", "测试");
    Assert::AreEqual(Decimal::FromInt(1000), result);
}

TEST(UnitConversion, MassConverter_KilogramToTon_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1000), "kg", "t", "测试");
    Assert::AreEqual(Decimal::FromInt(1), result);
}

TEST(UnitConversion, MassConverter_TonToKilogram_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "t", "kg", "测试");
    Assert::AreEqual(Decimal::FromInt(1000), result);
}

TEST(UnitConversion, MassConverter_GramToKilogram_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1000), "g", "kg", "测试");
    Assert::AreEqual(Decimal::FromInt(1), result);
}

TEST(UnitConversion, MassConverter_ChineseUnit_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "千克", "克", "测试");
    Assert::AreEqual(Decimal::FromInt(1000), result);

    result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "吨", "千克", "测试");
    Assert::AreEqual(Decimal::FromInt(1000), result);
}

TEST(UnitConversion, MassConverter_Milligram_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "kg", "mg", "测试");
    Assert::AreEqual(Decimal::FromInt(1000000), result);

    result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "kg", "毫克", "测试");
    Assert::AreEqual(Decimal::FromInt(1000000), result);

    result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(500), "mg", "kg", "测试");
    Assert::AreEqual(D("0.0005"), result);
}

TEST(UnitConversion, MassConverter_JinLiang_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "kg", "斤", "测试");
    Assert::AreEqual(Decimal::FromInt(2), result);

    result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "kg", "两", "测试");
    Assert::AreEqual(Decimal::FromInt(20), result);

    result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(5), "斤", "kg", "测试");
    Assert::AreEqual(D("2.5"), result);

    result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(2), "两", "g", "测试");
    Assert::AreEqual(Decimal::FromInt(100), result);
}

TEST(UnitConversion, MassConverter_PoundToKilogram_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "lb", "kg", "测试");
    Assert::AreEqual(0.453592, result.Float64(), 0.0001);
}

TEST(UnitConversion, MassConverter_OunceToKilogram_Test) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "oz", "kg", "测试");
    Assert::AreEqual(0.0283495, result.Float64(), 0.0001);
}

// ---------------- 无效单位 ----------------

TEST(UnitConversion, UnitConversion_InvalidUnit_Test) {
    bool hasException = false;
    try {
        AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "invalid", "m", "测试");
    } catch (...) {
        hasException = true;
    }
    Assert::IsTrue(hasException);

    hasException = false;
    try {
        AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "m", "invalid", "测试");
    } catch (...) {
        hasException = true;
    }
    Assert::IsTrue(hasException);
}

}  // namespace
