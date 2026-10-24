// 移植自 csharp/ToolGood.Algorithm.Test/AlgorithmEngineEx/AlgorithmEngineExTest.cs
//
// 类型对应：
//   new AlgorithmEngineEx()/ (bool)   -> AlgorithmEngineEx engine; / AlgorithmEngineEx engine(true);
//   engine.IgnoreCase                 -> engine.IgnoreCase()
//   engine.UseTempDict                -> 同名字段
//   List<Operand>                     -> std::vector<Operand>
//   Operand.Create(new List<T>{...})  -> Operand::Create(std::vector<Operand>{...})
//   new TimeSpan(10, 30, 0)           -> MyDate(0, 0, 0, 10, 30, 0)（C++ 无 TimeSpan，
//                                        MyDate 以 Year/Month=0 表示"仅时间"，与 C# 的
//                                        MyDate(TimeSpan) 语义一致：Day=0, Hour=10, Minute=30）
#include "cpptest.h"

#include <cstdint>
#include <exception>
#include <string>
#include <vector>

#include "toolgood/AlgorithmEngineEx.h"

using namespace toolgood;

namespace {

// ---------------------------------- 构造函数 ----------------------------------

TEST(EngineEx, Constructor_Default_Test) {
    AlgorithmEngineEx engine;
    Assert::IsFalse(engine.IgnoreCase());
    Assert::IsFalse(engine.UseTempDict);
}

TEST(EngineEx, Constructor_IgnoreCase_Test) {
    AlgorithmEngineEx engine(true);
    Assert::IsTrue(engine.IgnoreCase());

    engine = AlgorithmEngineEx(false);
    Assert::IsFalse(engine.IgnoreCase());
}

// ---------------------------------- AddParameter ----------------------------------

TEST(EngineEx, AddParameter_Bool_Test) {
    AlgorithmEngineEx engine;
    engine.AddParameter("flag", true);
    bool result = engine.TryEvaluate("flag", false);
    Assert::AreEqual(true, result);

    engine.AddParameter("flag", false);
    result = engine.TryEvaluate("flag", true);
    Assert::AreEqual(false, result);
}

TEST(EngineEx, AddParameter_Int_Test) {
    AlgorithmEngineEx engine;
    engine.AddParameter("num", 42);
    int result = engine.TryEvaluate("num", 0);
    Assert::AreEqual(42, result);
}

TEST(EngineEx, AddParameter_Long_Test) {
    AlgorithmEngineEx engine;
    engine.AddParameter("num", static_cast<int64_t>(123456789));
    int64_t result = engine.TryEvaluate("num", static_cast<int64_t>(0));
    Assert::AreEqual(static_cast<int64_t>(123456789), result);
}

TEST(EngineEx, AddParameter_Double_Test) {
    AlgorithmEngineEx engine;
    engine.AddParameter("num", 3.14159);
    double result = engine.TryEvaluate("num", 0.0);
    Assert::AreEqual(3.14159, result, 0.00001);
}

TEST(EngineEx, AddParameter_Decimal_Test) {
    AlgorithmEngineEx engine;
    engine.AddParameter("num", Decimal::FromString("123.45"));
    Decimal result = engine.TryEvaluate("num", Decimal::FromInt(0));
    Assert::AreEqual(Decimal::FromString("123.45"), result);
}

TEST(EngineEx, AddParameter_String_Test) {
    AlgorithmEngineEx engine;
    engine.AddParameter("name", "ToolGood");
    std::string result = engine.TryEvaluate("name", "");
    Assert::AreEqual(std::string("ToolGood"), result);
}

TEST(EngineEx, AddParameter_DateTime_Test) {
    AlgorithmEngineEx engine;
    MyDate dt(2024, 6, 15, 10, 30, 0);
    engine.AddParameter("dt", dt);
    int result = engine.TryEvaluate("year(dt)", 0);
    Assert::AreEqual(2024, result);
    result = engine.TryEvaluate("month(dt)", 0);
    Assert::AreEqual(6, result);
    result = engine.TryEvaluate("day(dt)", 0);
    Assert::AreEqual(15, result);
}

TEST(EngineEx, AddParameter_TimeSpan_Test) {
    AlgorithmEngineEx engine;
    // C# 的 new TimeSpan(10, 30, 0)：C++ 无 TimeSpan 类型，用 MyDate 的"仅时间"形式表达
    MyDate ti(0, 0, 0, 10, 30, 0);
    engine.AddParameter("ti", ti);
    int result = engine.TryEvaluate("hour(ti)", 0);
    Assert::AreEqual(10, result);
    result = engine.TryEvaluate("minute(ti)", 0);
    Assert::AreEqual(30, result);
}

TEST(EngineEx, AddParameter_List_Test) {
    AlgorithmEngineEx engine;
    std::vector<Operand> list{Operand::Create(1), Operand::Create(2), Operand::Create(3)};
    engine.AddParameter("arr", list);
    int result = engine.TryEvaluate("count(arr)", 0);
    Assert::AreEqual(3, result);
    result = engine.TryEvaluate("arr[1]", 0);
    Assert::AreEqual(1, result);
}

TEST(EngineEx, AddParameter_Collection_String_Test) {
    AlgorithmEngineEx engine;
    engine.AddParameter("names", Operand::Create(std::vector<Operand>{
                                     Operand::Create("a"), Operand::Create("b"),
                                     Operand::Create("c")}));
    // COUNT 只统计可转为数值的项,文本数组计数为 0
    int result = engine.TryEvaluate("count(names)", 0);
    Assert::AreEqual(0, result);
    std::string text = engine.TryEvaluate("names[1]", "");
    Assert::AreEqual(std::string("a"), text);
}

TEST(EngineEx, AddParameter_Collection_Int_Test) {
    AlgorithmEngineEx engine;
    engine.AddParameter("nums", Operand::Create(std::vector<Operand>{
                                    Operand::Create(1), Operand::Create(2), Operand::Create(3),
                                    Operand::Create(4), Operand::Create(5)}));
    int result = engine.TryEvaluate("sum(nums)", 0);
    Assert::AreEqual(15, result);
}

TEST(EngineEx, AddParameter_Collection_Double_Test) {
    AlgorithmEngineEx engine;
    engine.AddParameter("nums", Operand::Create(std::vector<Operand>{
                                    Operand::Create(1.1), Operand::Create(2.2),
                                    Operand::Create(3.3)}));
    double result = engine.TryEvaluate("sum(nums)", 0.0);
    Assert::AreEqual(6.6, result, 0.01);
}

TEST(EngineEx, AddParameter_Collection_Bool_Test) {
    AlgorithmEngineEx engine;
    engine.AddParameter("flags", Operand::Create(std::vector<Operand>{
                                     Operand::Create(true), Operand::Create(false),
                                     Operand::Create(true)}));
    int result = engine.TryEvaluate("count(flags)", 0);
    Assert::AreEqual(3, result);
}

TEST(EngineEx, AddParameter_Operand_Test) {
    AlgorithmEngineEx engine;
    engine.AddParameter("val", Operand::Create(100));
    int result = engine.TryEvaluate("val", 0);
    Assert::AreEqual(100, result);
}

// ------------------------------ AddParameterFromJson ------------------------------

TEST(EngineEx, AddParameterFromJson_Test) {
    AlgorithmEngineEx engine;
    engine.AddParameterFromJson("{\"name\":\"test\",\"age\":25,\"active\":true}");

    std::string result = engine.TryEvaluate("name", "");
    Assert::AreEqual(std::string("test"), result);

    int result2 = engine.TryEvaluate("age", 0);
    Assert::AreEqual(25, result2);

    bool boolResult = engine.TryEvaluate("active", false);
    Assert::AreEqual(true, boolResult);
}

TEST(EngineEx, AddParameterFromJson_Complex_Test) {
    AlgorithmEngineEx engine;
    engine.AddParameterFromJson("{\"灰色\":\"L\",\"canBookCount\":905,\"saleCount\":91}");

    std::string result = engine.TryEvaluate("灰色", "");
    Assert::AreEqual(std::string("L"), result);

    int result2 = engine.TryEvaluate("canBookCount", 0);
    Assert::AreEqual(905, result2);
}

TEST(EngineEx, AddParameterFromJson_Invalid_Test) {
    AlgorithmEngineEx engine;
    bool hasException = false;
    try {
        engine.AddParameterFromJson("not a json");
    } catch (const std::exception&) {
        hasException = true;
    }
    Assert::IsTrue(hasException);
}

// ------------------------------- ClearParameters -------------------------------

TEST(EngineEx, ClearParameters_Test) {
    AlgorithmEngineEx engine;
    engine.AddParameter("num", 42);
    int result = engine.TryEvaluate("num", 0);
    Assert::AreEqual(42, result);

    engine.ClearParameters();
    result = engine.TryEvaluate("num", -1);
    Assert::AreEqual(-1, result);
}

// -------------------------------- UseTempDict --------------------------------

TEST(EngineEx, UseTempDict_Test) {
    AlgorithmEngineEx engine;
    engine.UseTempDict = true;
    engine.AddParameter("num", 42);

    int result = engine.TryEvaluate("num", 0);
    Assert::AreEqual(42, result);
}

// -------------------------------- IgnoreCase --------------------------------

TEST(EngineEx, IgnoreCase_True_Test) {
    AlgorithmEngineEx engine(true);
    engine.AddParameter("Name", "Test");

    std::string result = engine.TryEvaluate("name", "");
    Assert::AreEqual(std::string("Test"), result);

    result = engine.TryEvaluate("NAME", "");
    Assert::AreEqual(std::string("Test"), result);

    result = engine.TryEvaluate("Name", "");
    Assert::AreEqual(std::string("Test"), result);
}

TEST(EngineEx, IgnoreCase_False_Test) {
    AlgorithmEngineEx engine(false);
    engine.AddParameter("Name", "Test");

    std::string result = engine.TryEvaluate("Name", "");
    Assert::AreEqual(std::string("Test"), result);

    result = engine.TryEvaluate("name", "default");
    Assert::AreEqual(std::string("default"), result);
}

// ------------------------------- GetParameterEx -------------------------------

TEST(EngineEx, GetParameterEx_Missing_Test) {
    AlgorithmEngineEx engine;
    std::string result = engine.TryEvaluate("missing_param", "default");
    Assert::AreEqual(std::string("default"), result);
}

// --------------------------------- 综合测试 ---------------------------------

TEST(EngineEx, Complex_Expression_Test) {
    AlgorithmEngineEx engine;
    engine.AddParameter("a", 10);
    engine.AddParameter("b", 20);
    engine.AddParameter("c", 30);

    int result = engine.TryEvaluate("a + b + c", 0);
    Assert::AreEqual(60, result);

    result = engine.TryEvaluate("a * b - c", 0);
    Assert::AreEqual(170, result);

    result = engine.TryEvaluate("if(a > 5, b, c)", 0);
    Assert::AreEqual(20, result);
}

TEST(EngineEx, Nested_Json_Test) {
    AlgorithmEngineEx engine;
    engine.AddParameterFromJson("{\"data\":{\"name\":\"test\",\"value\":123}}");

    std::string result = engine.TryEvaluate("json(data)['name']", "");
    Assert::AreEqual(std::string("test"), result);
}

TEST(EngineEx, Array_Operations_Test) {
    AlgorithmEngineEx engine;
    engine.AddParameter("arr", Operand::Create(std::vector<Operand>{
                                   Operand::Create(1), Operand::Create(2), Operand::Create(3),
                                   Operand::Create(4), Operand::Create(5)}));

    int result = engine.TryEvaluate("sum(arr)", 0);
    Assert::AreEqual(15, result);

    double result2 = engine.TryEvaluate("average(arr)", 0.0);
    Assert::AreEqual(3.0, result2, 0.01);

    result = engine.TryEvaluate("max(arr)", 0);
    Assert::AreEqual(5, result);

    result = engine.TryEvaluate("min(arr)", 0);
    Assert::AreEqual(1, result);

    result = engine.TryEvaluate("count(arr)", 0);
    Assert::AreEqual(5, result);
}

}  // namespace
