// 移植自 csharp/ToolGood.Algorithm.Test/UnitConversion/UnitConversionPrecisionTest.cs
//
// 精确分数系数防回归测试：相对误差需 < 1e-9。
// 类型对应：
//   AlgorithmEngineHelper.UnitConversion(decimal, src, tar, name)
//     -> AlgorithmEngineHelper::UnitConversion(const Decimal&, src, tar, name) -> Decimal
//   decimal 字面量 1m -> Decimal::FromInt(1)
//   (double)result    -> result.Float64()
//   Math.Max          -> std::max
#include "cpptest.h"

#include <algorithm>
#include <cmath>
#include <string>

#include "toolgood/AlgorithmEngineHelper.h"

using namespace toolgood;

namespace {

/// 对应 C# 的 AssertClose(src, tar, value, expected)
void AssertClose(const std::string& src, const std::string& tar, const Decimal& value,
                 double expected) {
    Decimal result = AlgorithmEngineHelper::UnitConversion(value, src, tar, "精度测试");
    double d = result.Float64();
    double rel = std::fabs(d - expected) / std::max(std::fabs(expected), 1e-30);
    Assert::IsTrue(rel < 1e-9);
}

// ---------------- 质量 ----------------

TEST(UnitConversionPrecision, Mass_ShortTon_Exact) {
    AssertClose("short ton", "kg", Decimal::FromInt(1), 907.18474);
    AssertClose("kg", "short ton", Decimal::FromInt(1), 1.0 / 907.18474);
    AssertClose("short ton", "lb", Decimal::FromInt(1), 2000);
}

TEST(UnitConversionPrecision, Mass_LongTon_Exact) {
    AssertClose("long ton", "kg", Decimal::FromInt(1), 1016.0469088);
    AssertClose("kg", "long ton", Decimal::FromInt(1), 1.0 / 1016.0469088);
    AssertClose("long ton", "lb", Decimal::FromInt(1), 2240);
}

// ---------------- 面积 ----------------

TEST(UnitConversionPrecision, Area_Mu_Exact) {
    AssertClose("亩", "m²", Decimal::FromInt(1), 666.6666666666667);
    AssertClose("m²", "亩", Decimal::FromInt(1), 0.0015);
}

// ---------------- 体积 ----------------

TEST(UnitConversionPrecision, Volume_CubicFoot_Exact) {
    AssertClose("ft³", "l", Decimal::FromInt(1), 28.316846592);
    AssertClose("l", "ft³", Decimal::FromInt(1), 0.03531466672148859);
    AssertClose("ft³", "in³", Decimal::FromInt(1), 1728);
}

TEST(UnitConversionPrecision, Volume_CubicInch_Exact) {
    AssertClose("in³", "l", Decimal::FromInt(1), 0.016387064);
    AssertClose("l", "in³", Decimal::FromInt(1), 61.0237440947323);
}

TEST(UnitConversionPrecision, Volume_ImperialPint_Exact) {
    AssertClose("imperial pint", "l", Decimal::FromInt(1), 0.56826125);
    AssertClose("l", "imperial pint", Decimal::FromInt(1), 1.7597539863927023);
}

TEST(UnitConversionPrecision, Volume_ImperialGallon_Exact) {
    AssertClose("imperial gallon", "l", Decimal::FromInt(1), 4.54609);
    AssertClose("l", "imperial gallon", Decimal::FromInt(1), 0.2199692482990878);
    AssertClose("imperial gallon", "imperial pint", Decimal::FromInt(1), 8);
    AssertClose("imperial gallon", "imperial quart", Decimal::FromInt(1), 4);
}

TEST(UnitConversionPrecision, Volume_ImperialQuart_Exact) {
    AssertClose("imperial quart", "l", Decimal::FromInt(1), 1.1365225);
    AssertClose("l", "imperial quart", Decimal::FromInt(1), 0.87987699319635115);
}

TEST(UnitConversionPrecision, Volume_USPint_Exact) {
    AssertClose("US pint", "l", Decimal::FromInt(1), 0.473176473);
    AssertClose("l", "US pint", Decimal::FromInt(1), 2.1133764188651873);
}

TEST(UnitConversionPrecision, Volume_USGallon_Exact) {
    AssertClose("US gallon", "l", Decimal::FromInt(1), 3.785411784);
    AssertClose("l", "US gallon", Decimal::FromInt(1), 0.2641720523581484);
    AssertClose("US gallon", "US quart", Decimal::FromInt(1), 4);
}

}  // namespace
