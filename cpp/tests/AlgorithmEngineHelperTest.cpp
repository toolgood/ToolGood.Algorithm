// 移植自 csharp/ToolGood.Algorithm.Test/AlgorithmEngineHelper/AlgorithmEngineHelperTest.cs
//
// 类型对应：
//   DiyNameInfo.Parameters[i].ToString() -> 同（C++ 的 DiyNameKeyInfo::ToString 返回 Name）
//   decimal 字面量 1M / 0.001M           -> Decimal::FromInt(1) / Decimal::FromString("0.001")
//   Assert.Throws<T>(() => ...)          -> Assert::Throws([&]{ ... })
//   Assert.IsNotNull(ParseFormula(...))  -> C++ 返回 Function(shared_ptr)，用 != nullptr 判定
//   异常类型：ArgumentException -> std::invalid_argument
#include "cpptest.h"

#include <string>
#include <vector>

#include "toolgood/AlgorithmEngineHelper.h"

using namespace toolgood;

namespace {

TEST(Helper, GetDiyNames_test) {
    DiyNameInfo p = AlgorithmEngineHelper::GetDiyNames("dd");
    Assert::AreEqual(std::string("dd"), p.Parameters[0].ToString());

    DiyNameInfo p3 = AlgorithmEngineHelper::GetDiyNames("dd");
    Assert::AreEqual(std::string("dd"), p3.Parameters[0].ToString());

    DiyNameInfo p5 = AlgorithmEngineHelper::GetDiyNames("ddd(d1,22)");
    Assert::AreEqual(std::string("ddd"), p5.Functions[0].Name);
    Assert::AreEqual(std::string("d1"), p5.Parameters[0].ToString());

    DiyNameInfo p6 = AlgorithmEngineHelper::GetDiyNames("长");
    Assert::AreEqual(std::string("长"), p6.Parameters[0].ToString());

    DiyNameInfo p7 = AlgorithmEngineHelper::GetDiyNames("ddd+2");
    Assert::AreEqual(std::string("ddd"), p7.Parameters[0].ToString());

    DiyNameInfo p8 = AlgorithmEngineHelper::GetDiyNames(
        "{\"A\": 0.6,\"B\": 0.4,\"C\": 0.6,\"E\": 0.33,\"F\": 0.29,\"Z\": 0.15"
        ",\"EB\": 0.7,\"EE\": 0.65,\"EA\": 0.85,\"AB\": 1.0,\"BC\": 1.0,\"AA\":1.0"
        ",\"EBC\": 1.15,\"BAB\": 1.25,\"BCB\": 1.25,\"BBC\": 1.25,\"CBB\": 1.25,\"EBA\": 1.2,"
        "\"AAA\": 1.4}[瓦楞]");
    Assert::AreEqual(std::string("瓦楞"), p8.Parameters[0].ToString());
}

TEST(Helper, IsParameter_Test) {
    bool b = AlgorithmEngineHelper::IsParameter("false");
    Assert::IsFalse(b);

    b = AlgorithmEngineHelper::IsParameter("f11");
    Assert::IsTrue(b);

    b = AlgorithmEngineHelper::IsParameter("f11+1");
    Assert::IsFalse(b);

    b = AlgorithmEngineHelper::IsParameter("f11++");
    Assert::IsFalse(b);
}

TEST(Helper, UnitConversion_test) {
    Decimal b = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "米", "千米", "测试");
    Assert::AreEqual(Decimal::FromString("0.001"), b);
    b = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "米", "分米", "测试");
    Assert::AreEqual(Decimal::FromInt(10), b);
    b = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "米", "厘米", "测试");
    Assert::AreEqual(Decimal::FromInt(100), b);
    b = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "米", "mm", "测试");
    Assert::AreEqual(Decimal::FromInt(1000), b);

    b = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "m2", "dm2", "测试");
    Assert::AreEqual(Decimal::FromInt(100), b);
    b = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "m2", "cm2", "测试");
    Assert::AreEqual(Decimal::FromInt(10000), b);
    b = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "m2", "mm2", "测试");
    Assert::AreEqual(Decimal::FromInt(1000000), b);

    b = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "m3", "dm3", "测试");
    Assert::AreEqual(Decimal::FromInt(1000), b);
    b = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "m3", "cm3", "测试");
    Assert::AreEqual(Decimal::FromInt(1000000), b);
    b = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "m3", "mm3", "测试");
    Assert::AreEqual(Decimal::FromInt(1000000000), b);

    b = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "t", "kg", "测试");
    Assert::AreEqual(Decimal::FromInt(1000), b);
    b = AlgorithmEngineHelper::UnitConversion(Decimal::FromInt(1), "t", "g", "测试");
    Assert::AreEqual(Decimal::FromInt(1000000), b);
}

TEST(Helper, CheckFormula_test) {
    bool b = AlgorithmEngineHelper::CheckFormula("1+1");
    Assert::IsTrue(b);

    b = AlgorithmEngineHelper::CheckFormula("1+");
    Assert::AreEqual(false, b);

    b = AlgorithmEngineHelper::CheckFormula("@123+1");
    Assert::AreEqual(false, b);
}

TEST(Helper, ParseFormula_NumberOverflow_Test) {
    // 超出 decimal 范围(约 29 位)的数字应抛 ArgumentException（C++: std::invalid_argument）
    Assert::Throws([&] { AlgorithmEngineHelper::ParseFormula("12345678901234567890123456789012345"); });
    Assert::Throws([&] { AlgorithmEngineHelper::ParseFormula("12345678901234567890123456789012345M"); });
    Assert::Throws([&] { AlgorithmEngineHelper::ParseFormula("9999999999999999999999999999999"); });

    // 正常数字（含单位、科学计数法、负数）不受影响
    Function f1 = AlgorithmEngineHelper::ParseFormula("12CM");
    Assert::IsTrue(f1 != nullptr);
    Function f2 = AlgorithmEngineHelper::ParseFormula("10M2");
    Assert::IsTrue(f2 != nullptr);
    Function f3 = AlgorithmEngineHelper::ParseFormula("-5");
    Assert::IsTrue(f3 != nullptr);
    Function f4 = AlgorithmEngineHelper::ParseFormula("-5M");
    Assert::IsTrue(f4 != nullptr);
    Function f5 = AlgorithmEngineHelper::ParseFormula("3E5");
    Assert::IsTrue(f5 != nullptr);
    Function f6 = AlgorithmEngineHelper::ParseFormula("1234567890123456789012345678");
    Assert::IsTrue(f6 != nullptr);
}

}  // namespace
