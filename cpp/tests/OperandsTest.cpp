// 移植自 csharp/ToolGood.Algorithm.Test/Operands/OperandsTest.cs
//
// 类型对应：
//   Operand.True/False/One/Zero/Null/Version -> Operand::True() 等（返回 const 引用）
//   operand.IsXxx / ErrorMsg                 -> operand.IsXxx() / operand.ErrorMsg()
//   Operand.Error("...{0}", arg)             -> Operand::ErrorFormat("...{0}", {arg})
//   new DateTime(...)                        -> MyDate(y, M, d, h, m, s)
//   new TimeSpan(d, h, m, s)                 -> MyDate(0, 0, d, h, m, s)（字段一一对应）
//   List<T> / ICollection<T>                 -> std::vector<Operand>
//   decimal 字面量 1.5m                       -> Decimal::FromString("1.5")
//   C# 隐式转换（Operand op = 42;）            -> 显式 Operand::Create(...)
//   OperandType（enum class）比较              -> 转 int 后比较（框架 Show 无枚举重载）
#include "cpptest.h"

#include <string>
#include <vector>

using namespace toolgood;

namespace {

/// enum class 无法直接参与 AreEqual 的字符串化，统一转 int 比较
int TypeInt(OperandType t) { return static_cast<int>(t); }

bool StartsWith(const std::string& s, const std::string& prefix) {
    return s.size() >= prefix.size() && s.compare(0, prefix.size(), prefix) == 0;
}

// ---------------- Operand 静态属性 ----------------

TEST(Operands, Operand_StaticProperties_Test) {
    Assert::AreEqual("ToolGood.Algorithm 6.3", Operand::Version().TextValue());
    Assert::AreEqual(true, Operand::True().BooleanValue());
    Assert::AreEqual(false, Operand::False().BooleanValue());
    Assert::AreEqual(1, Operand::One().IntValue());
    Assert::AreEqual(0, Operand::Zero().IntValue());
    Assert::IsTrue(Operand::Null().IsNull());
}

// ---------------- Operand Create ----------------

TEST(Operands, Operand_Create_Bool_Test) {
    Operand op = Operand::Create(true);
    Assert::IsTrue(op.IsBoolean());
    Assert::AreEqual(true, op.BooleanValue());

    op = Operand::Create(false);
    Assert::IsTrue(op.IsBoolean());
    Assert::AreEqual(false, op.BooleanValue());
}

TEST(Operands, Operand_Create_Int_Test) {
    Operand op = Operand::Create(100);
    Assert::IsTrue(op.IsNumber());
    Assert::AreEqual(100, op.IntValue());
    Assert::AreEqual(Decimal::FromInt(100), op.NumberValue());

    op = Operand::Create(-100);
    Assert::IsTrue(op.IsNumber());
    Assert::AreEqual(-100, op.IntValue());

    op = Operand::Create(0);
    Assert::IsTrue(op.IsNumber());
    Assert::AreEqual(0, op.IntValue());

    op = Operand::Create(1000);
    Assert::IsTrue(op.IsNumber());
    Assert::AreEqual(1000, op.IntValue());

    op = Operand::Create(-1000);
    Assert::IsTrue(op.IsNumber());
    Assert::AreEqual(-1000, op.IntValue());
}

TEST(Operands, Operand_Create_Long_Test) {
    Operand op = Operand::Create(static_cast<int64_t>(100000));
    Assert::IsTrue(op.IsNumber());
    Assert::AreEqual(static_cast<int64_t>(100000), op.LongValue());
}

TEST(Operands, Operand_Create_Double_Test) {
    Operand op = Operand::Create(3.14159);
    Assert::IsTrue(op.IsNumber());
    Assert::AreEqual(3.14159, op.DoubleValue(), 0.00001);
}

TEST(Operands, Operand_Create_Decimal_Test) {
    Operand op = Operand::Create(Decimal::FromString("3.14159"));
    Assert::IsTrue(op.IsNumber());
    Assert::AreEqual(Decimal::FromString("3.14159"), op.NumberValue());
}

TEST(Operands, Operand_Create_String_Test) {
    Operand op = Operand::Create(std::string("hello"));
    Assert::IsTrue(op.IsText());
    Assert::AreEqual("hello", op.TextValue());

    op = Operand::Create(std::string(""));
    Assert::IsTrue(op.IsText());
    Assert::AreEqual("", op.TextValue());

    // C# 的 (string)null -> C++ 的 const char* nullptr，Create 返回 Null 操作数
    op = Operand::Create(static_cast<const char*>(nullptr));
    Assert::IsTrue(op.IsNull());
}

TEST(Operands, Operand_Create_DateTime_Test) {
    MyDate dt(2024, 1, 15, 10, 30, 45);
    Operand op = Operand::Create(dt);
    Assert::IsTrue(op.IsDate());
    Assert::AreEqual(2024, op.DateValue().Year);
    Assert::AreEqual(1, op.DateValue().Month);
    Assert::AreEqual(15, op.DateValue().Day);
    Assert::AreEqual(10, op.DateValue().Hour);
    Assert::AreEqual(30, op.DateValue().Minute);
    Assert::AreEqual(45, op.DateValue().Second);
}

TEST(Operands, Operand_Create_TimeSpan_Test) {
    // new TimeSpan(1, 2, 3, 4) = 1 天 2 时 3 分 4 秒
    MyDate ts(0, 0, 1, 2, 3, 4);
    Operand op = Operand::Create(ts);
    Assert::IsTrue(op.IsDate());
    Assert::AreEqual(1, op.DateValue().Day);
    Assert::AreEqual(2, op.DateValue().Hour);
    Assert::AreEqual(3, op.DateValue().Minute);
    Assert::AreEqual(4, op.DateValue().Second);
}

TEST(Operands, Operand_Create_List_Test) {
    std::vector<Operand> list{Operand::Create(1), Operand::Create(2), Operand::Create(3)};
    Operand op = Operand::Create(list);
    Assert::IsTrue(op.IsArray());
    std::vector<Operand> arr = op.ArrayValue();
    Assert::AreEqual(3, static_cast<int>(arr.size()));
    Assert::AreEqual(1, arr[0].IntValue());
    Assert::AreEqual(2, arr[1].IntValue());
    Assert::AreEqual(3, arr[2].IntValue());
}

TEST(Operands, Operand_Create_Collection_String_Test) {
    std::vector<Operand> list{Operand::Create(std::string("a")), Operand::Create(std::string("b")),
                              Operand::Create(std::string("c"))};
    Operand op = Operand::Create(list);
    Assert::IsTrue(op.IsArray());
    std::vector<Operand> arr = op.ArrayValue();
    Assert::AreEqual(3, static_cast<int>(arr.size()));
    Assert::AreEqual("a", arr[0].TextValue());
    Assert::AreEqual("b", arr[1].TextValue());
    Assert::AreEqual("c", arr[2].TextValue());
}

TEST(Operands, Operand_Create_Collection_Int_Test) {
    std::vector<Operand> list{Operand::Create(1), Operand::Create(2), Operand::Create(3)};
    Operand op = Operand::Create(list);
    Assert::IsTrue(op.IsArray());
    std::vector<Operand> arr = op.ArrayValue();
    Assert::AreEqual(3, static_cast<int>(arr.size()));
    Assert::AreEqual(1, arr[0].IntValue());
    Assert::AreEqual(2, arr[1].IntValue());
    Assert::AreEqual(3, arr[2].IntValue());
}

TEST(Operands, Operand_Create_Collection_Double_Test) {
    std::vector<Operand> list{Operand::Create(1.1), Operand::Create(2.2), Operand::Create(3.3)};
    Operand op = Operand::Create(list);
    Assert::IsTrue(op.IsArray());
    std::vector<Operand> arr = op.ArrayValue();
    Assert::AreEqual(3, static_cast<int>(arr.size()));
    Assert::AreEqual(1.1, arr[0].DoubleValue(), 0.001);
    Assert::AreEqual(2.2, arr[1].DoubleValue(), 0.001);
    Assert::AreEqual(3.3, arr[2].DoubleValue(), 0.001);
}

TEST(Operands, Operand_Create_Collection_Bool_Test) {
    std::vector<Operand> list{Operand::Create(true), Operand::Create(false), Operand::Create(true)};
    Operand op = Operand::Create(list);
    Assert::IsTrue(op.IsArray());
    std::vector<Operand> arr = op.ArrayValue();
    Assert::AreEqual(3, static_cast<int>(arr.size()));
    Assert::AreEqual(true, arr[0].BooleanValue());
    Assert::AreEqual(false, arr[1].BooleanValue());
    Assert::AreEqual(true, arr[2].BooleanValue());
}

TEST(Operands, Operand_CreateJson_Test) {
    Operand op = Operand::CreateJson("{\"name\":\"test\",\"value\":123}");
    Assert::IsTrue(op.IsJson());

    op = Operand::CreateJson("[1,2,3]");
    Assert::IsTrue(op.IsJson());

    op = Operand::CreateJson("invalid json");
    Assert::IsTrue(op.IsError());
}

TEST(Operands, Operand_Error_Test) {
    Operand op = Operand::Error("test error");
    Assert::IsTrue(op.IsError());
    Assert::AreEqual("test error", op.ErrorMsg());

    // C#: Operand.Error("error {0}", "param")
    op = Operand::ErrorFormat("error {0}", {"param"});
    Assert::IsTrue(op.IsError());
    Assert::AreEqual("error param", op.ErrorMsg());
}

TEST(Operands, Operand_CreateNull_Test) {
    Operand op = Operand::Null();
    Assert::IsTrue(op.IsNull());
}

// ---------------- OperandInt ----------------

TEST(Operands, OperandInt_Properties_Test) {
    Operand op = Operand::Create(42);
    Assert::AreEqual(TypeInt(OperandType::NUMBER), TypeInt(op.Type()));
    Assert::IsTrue(op.IsNumber());
    Assert::IsFalse(op.IsNull());
    Assert::IsFalse(op.IsText());
    Assert::IsFalse(op.IsBoolean());
    Assert::IsFalse(op.IsArray());
    Assert::IsFalse(op.IsDate());
    Assert::IsFalse(op.IsJson());
    Assert::IsFalse(op.IsError());
}

TEST(Operands, OperandInt_ToNumber_Test) {
    Operand op = Operand::Create(42);
    Operand result = op.ToNumber();
    Assert::IsTrue(result.IsNumber());
    Assert::AreEqual(42, result.IntValue());
}

TEST(Operands, OperandInt_ToBoolean_Test) {
    Operand op = Operand::Create(0);
    Operand result = op.ToBoolean();
    Assert::IsTrue(result.IsBoolean());
    Assert::AreEqual(false, result.BooleanValue());

    op = Operand::Create(1);
    result = op.ToBoolean();
    Assert::IsTrue(result.IsBoolean());
    Assert::AreEqual(true, result.BooleanValue());

    op = Operand::Create(2);
    result = op.ToBoolean();
    Assert::IsTrue(result.IsError());
}

TEST(Operands, OperandInt_ToText_Test) {
    Operand op = Operand::Create(42);
    Operand result = op.ToText();
    Assert::IsTrue(result.IsText());
    Assert::AreEqual("42", result.TextValue());
}

TEST(Operands, OperandInt_ToString_Test) {
    Operand op = Operand::Create(42);
    Assert::AreEqual("42", op.ToString());
}

// ---------------- OperandDouble ----------------

TEST(Operands, OperandDouble_Properties_Test) {
    Operand op = Operand::Create(3.14);
    Assert::AreEqual(TypeInt(OperandType::NUMBER), TypeInt(op.Type()));
    Assert::IsTrue(op.IsNumber());
    Assert::AreEqual(3.14, op.DoubleValue(), 0.001);
    Assert::AreEqual(Decimal::FromString("3.14"), op.NumberValue());
    Assert::AreEqual(3, op.IntValue());
    Assert::AreEqual(static_cast<int64_t>(3), op.LongValue());
}

TEST(Operands, OperandDouble_ToBoolean_Test) {
    Operand op = Operand::Create(0.0);
    Operand result = op.ToBoolean();
    Assert::IsTrue(result.IsBoolean());
    Assert::AreEqual(false, result.BooleanValue());

    op = Operand::Create(1.0);
    result = op.ToBoolean();
    Assert::IsTrue(result.IsBoolean());
    Assert::AreEqual(true, result.BooleanValue());

    op = Operand::Create(2.5);
    result = op.ToBoolean();
    Assert::IsTrue(result.IsError());
}

TEST(Operands, OperandDouble_ToText_Test) {
    Operand op = Operand::Create(3.14);
    Operand result = op.ToText();
    Assert::IsTrue(result.IsText());
    Assert::AreEqual("3.14", result.TextValue());
}

// ---------------- OperandDecimal ----------------

TEST(Operands, OperandDecimal_Properties_Test) {
    Operand op = Operand::Create(Decimal::FromString("123.456"));
    Assert::AreEqual(TypeInt(OperandType::NUMBER), TypeInt(op.Type()));
    Assert::IsTrue(op.IsNumber());
    Assert::AreEqual(Decimal::FromString("123.456"), op.NumberValue());
    Assert::AreEqual(123.456, op.DoubleValue(), 0.001);
    Assert::AreEqual(123, op.IntValue());
}

TEST(Operands, OperandDecimal_ToBoolean_Test) {
    Operand op = Operand::Create(Decimal::FromInt(0));
    Operand result = op.ToBoolean();
    Assert::IsTrue(result.IsBoolean());
    Assert::AreEqual(false, result.BooleanValue());

    op = Operand::Create(Decimal::FromInt(1));
    result = op.ToBoolean();
    Assert::IsTrue(result.IsBoolean());
    Assert::AreEqual(true, result.BooleanValue());
}

TEST(Operands, OperandDecimal_ToText_Test) {
    Operand op = Operand::Create(Decimal::FromString("123.45"));
    Operand result = op.ToText();
    Assert::IsTrue(result.IsText());
    Assert::AreEqual("123.45", result.TextValue());
}

// ---------------- OperandBoolean ----------------

TEST(Operands, OperandBoolean_Properties_Test) {
    Operand op = Operand::Create(true);
    Assert::AreEqual(TypeInt(OperandType::BOOLEAN), TypeInt(op.Type()));
    Assert::IsTrue(op.IsBoolean());
    Assert::IsFalse(op.IsNumber());
    Assert::AreEqual(true, op.BooleanValue());

    op = Operand::Create(false);
    Assert::AreEqual(TypeInt(OperandType::BOOLEAN), TypeInt(op.Type()));
    Assert::IsTrue(op.IsBoolean());
    Assert::AreEqual(false, op.BooleanValue());
}

TEST(Operands, OperandBoolean_ToNumber_Test) {
    Operand op = Operand::Create(true);
    Operand result = op.ToNumber();
    Assert::IsTrue(result.IsNumber());
    Assert::AreEqual(1, result.IntValue());

    op = Operand::Create(false);
    result = op.ToNumber();
    Assert::IsTrue(result.IsNumber());
    Assert::AreEqual(0, result.IntValue());
}

TEST(Operands, OperandBoolean_ToText_Test) {
    Operand op = Operand::Create(true);
    Operand result = op.ToText();
    Assert::IsTrue(result.IsText());
    Assert::AreEqual("TRUE", result.TextValue());

    op = Operand::Create(false);
    result = op.ToText();
    Assert::IsTrue(result.IsText());
    Assert::AreEqual("FALSE", result.TextValue());
}

TEST(Operands, OperandBoolean_ToString_Test) {
    Operand op = Operand::Create(true);
    Assert::AreEqual("true", op.ToString());

    op = Operand::Create(false);
    Assert::AreEqual("false", op.ToString());
}

// ---------------- OperandString ----------------

TEST(Operands, OperandString_Properties_Test) {
    Operand op = Operand::Create(std::string("hello world"));
    Assert::AreEqual(TypeInt(OperandType::TEXT), TypeInt(op.Type()));
    Assert::IsTrue(op.IsText());
    Assert::IsFalse(op.IsNumber());
    Assert::IsFalse(op.IsBoolean());
    Assert::AreEqual("hello world", op.TextValue());
}

TEST(Operands, OperandString_ToNumber_Test) {
    Operand op = Operand::Create(std::string("42"));
    Operand result = op.ToNumber();
    Assert::IsTrue(result.IsNumber());
    Assert::AreEqual(42, result.IntValue());

    op = Operand::Create(std::string("3.14"));
    result = op.ToNumber();
    Assert::IsTrue(result.IsNumber());
    Assert::AreEqual(3.14, result.DoubleValue(), 0.001);

    op = Operand::Create(std::string("not a number"));
    result = op.ToNumber();
    Assert::IsTrue(result.IsError());
}

TEST(Operands, OperandString_ToBoolean_Test) {
    Operand op = Operand::Create(std::string("true"));
    Operand result = op.ToBoolean();
    Assert::IsTrue(result.IsBoolean());
    Assert::AreEqual(true, result.BooleanValue());

    op = Operand::Create(std::string("false"));
    result = op.ToBoolean();
    Assert::IsTrue(result.IsBoolean());
    Assert::AreEqual(false, result.BooleanValue());

    op = Operand::Create(std::string("TRUE"));
    result = op.ToBoolean();
    Assert::IsTrue(result.IsBoolean());
    Assert::AreEqual(true, result.BooleanValue());

    op = Operand::Create(std::string("not bool"));
    result = op.ToBoolean();
    Assert::IsTrue(result.IsError());
}

TEST(Operands, OperandString_ToMyDate_Test) {
    Operand op = Operand::Create(std::string("2024-01-15"));
    Operand result = op.ToMyDate();
    Assert::IsTrue(result.IsDate());
    Assert::AreEqual(2024, result.DateValue().Year);
    Assert::AreEqual(1, result.DateValue().Month);
    Assert::AreEqual(15, result.DateValue().Day);

    op = Operand::Create(std::string("10:30:45"));
    result = op.ToMyDate();
    Assert::IsTrue(result.IsDate());
    Assert::AreEqual(10, result.DateValue().Hour);
    Assert::AreEqual(30, result.DateValue().Minute);
    Assert::AreEqual(45, result.DateValue().Second);

    op = Operand::Create(std::string("not a date"));
    result = op.ToMyDate();
    Assert::IsTrue(result.IsError());
}

TEST(Operands, OperandString_ToJson_Test) {
    Operand op = Operand::Create(std::string("{\"name\":\"test\"}"));
    Operand result = op.ToJson();
    Assert::IsTrue(result.IsJson());

    op = Operand::Create(std::string("[1,2,3]"));
    result = op.ToJson();
    Assert::IsTrue(result.IsJson());

    op = Operand::Create(std::string("not json"));
    result = op.ToJson();
    Assert::IsTrue(result.IsError());
}

TEST(Operands, OperandString_ToArray_Test) {
    Operand op = Operand::Create(std::string("test"));
    Operand result = op.ToArray();
    Assert::IsTrue(result.IsError());
}

TEST(Operands, OperandString_ToString_Test) {
    Operand op = Operand::Create(std::string("hello"));
    Assert::AreEqual("\"hello\"", op.ToString());

    op = Operand::Create(std::string("hello\"world"));
    Assert::AreEqual("\"hello\\\"world\"", op.ToString());

    op = Operand::Create(std::string("hello\nworld"));
    Assert::AreEqual("\"hello\\nworld\"", op.ToString());

    op = Operand::Create(std::string("hello\tworld"));
    Assert::AreEqual("\"hello\\tworld\"", op.ToString());
}

// ---------------- OperandNull ----------------

TEST(Operands, OperandNull_Properties_Test) {
    Operand op = Operand::Null();
    Assert::AreEqual(TypeInt(OperandType::NULL_VALUE), TypeInt(op.Type()));
    Assert::IsTrue(op.IsNull());
    Assert::IsFalse(op.IsNumber());
    Assert::IsFalse(op.IsText());
    Assert::IsFalse(op.IsBoolean());
    Assert::IsFalse(op.IsError());
}

TEST(Operands, OperandNull_ToString_Test) {
    Operand op = Operand::Null();
    Assert::AreEqual("null", op.ToString());
}

// ---------------- OperandError ----------------

TEST(Operands, OperandError_Properties_Test) {
    Operand op = Operand::Error("test error message");
    Assert::AreEqual(TypeInt(OperandType::ERROR), TypeInt(op.Type()));
    Assert::IsTrue(op.IsError());
    Assert::IsFalse(op.IsNull());
    Assert::IsFalse(op.IsNumber());
    Assert::IsFalse(op.IsText());
    Assert::AreEqual("test error message", op.ErrorMsg());
}

TEST(Operands, OperandError_Conversions_Test) {
    Operand op = Operand::Error("error");

    Assert::IsTrue(op.ToNumber().IsError());
    Assert::IsTrue(op.ToBoolean().IsError());
    Assert::IsTrue(op.ToText().IsError());
    Assert::IsTrue(op.ToArray().IsError());
    Assert::IsTrue(op.ToMyDate().IsError());
    Assert::IsTrue(op.ToJson().IsError());
}

// ---------------- OperandArray ----------------

TEST(Operands, OperandArray_Properties_Test) {
    std::vector<Operand> list{Operand::Create(1), Operand::Create(2), Operand::Create(3)};
    Operand op = Operand::Create(list);
    Assert::AreEqual(TypeInt(OperandType::ARRAY), TypeInt(op.Type()));
    Assert::IsTrue(op.IsArray());
    Assert::IsFalse(op.IsNumber());
    Assert::IsFalse(op.IsText());
    Assert::AreEqual(3, static_cast<int>(op.ArrayValue().size()));
}

TEST(Operands, OperandArray_ToText_Test) {
    std::vector<Operand> list{Operand::Create(1), Operand::Create(2), Operand::Create(3)};
    Operand op = Operand::Create(list);
    Operand result = op.ToText();
    Assert::IsTrue(result.IsText());
    Assert::AreEqual("[1,2,3]", result.TextValue());
}

TEST(Operands, OperandArray_ToArray_Test) {
    std::vector<Operand> list{Operand::Create(1), Operand::Create(2)};
    Operand op = Operand::Create(list);
    Operand result = op.ToArray("");
    Assert::IsTrue(result.IsArray());
    Assert::AreEqual(2, static_cast<int>(result.ArrayValue().size()));
}

TEST(Operands, OperandArray_ToJson_Test) {
    std::vector<Operand> list{Operand::Create(1), Operand::Create(2)};
    Operand op = Operand::Create(list);
    Operand result = op.ToJson();
    Assert::IsTrue(result.IsJson());
}

TEST(Operands, OperandArray_ToString_Test) {
    std::vector<Operand> list{Operand::Create(1), Operand::Create(std::string("test")),
                              Operand::Create(true)};
    Operand op = Operand::Create(list);
    Assert::AreEqual("[1,\"test\",true]", op.ToString());
}

// ---------------- OperandMyDate ----------------

TEST(Operands, OperandMyDate_Properties_Test) {
    MyDate dt(2024, 6, 15, 10, 30, 45);
    Operand op = Operand::Create(dt);
    Assert::AreEqual(TypeInt(OperandType::DATE), TypeInt(op.Type()));
    Assert::IsTrue(op.IsDate());
    Assert::IsFalse(op.IsNumber());
    Assert::IsFalse(op.IsText());
    Assert::AreEqual(2024, op.DateValue().Year);
    Assert::AreEqual(6, op.DateValue().Month);
    Assert::AreEqual(15, op.DateValue().Day);
    Assert::AreEqual(10, op.DateValue().Hour);
    Assert::AreEqual(30, op.DateValue().Minute);
    Assert::AreEqual(45, op.DateValue().Second);
}

TEST(Operands, OperandMyDate_ToText_Test) {
    MyDate dt(2024, 6, 15, 10, 30, 45);
    Operand op = Operand::Create(dt);
    Operand result = op.ToText();
    Assert::IsTrue(result.IsText());
}

TEST(Operands, OperandMyDate_ToMyDate_Test) {
    MyDate dt(2024, 6, 15, 10, 30, 45);
    Operand op = Operand::Create(dt);
    Operand result = op.ToMyDate();
    Assert::IsTrue(result.IsDate());
    Assert::AreEqual(2024, result.DateValue().Year);
}

TEST(Operands, OperandMyDate_ToString_Test) {
    MyDate dt(2024, 6, 15, 10, 30, 45);
    Operand op = Operand::Create(dt);
    Assert::IsTrue(StartsWith(op.ToString(), "\"2024-06-15"));
}

// ---------------- 隐式转换（C++ 用显式 Create 表达）----------------

TEST(Operands, Operand_Implicit_Int_Test) {
    Operand op = Operand::Create(42);
    Assert::IsTrue(op.IsNumber());
    Assert::AreEqual(42, op.IntValue());
}

TEST(Operands, Operand_Implicit_Double_Test) {
    Operand op = Operand::Create(3.14);
    Assert::IsTrue(op.IsNumber());
    Assert::AreEqual(3.14, op.DoubleValue(), 0.001);
}

TEST(Operands, Operand_Implicit_Decimal_Test) {
    Operand op = Operand::Create(Decimal::FromString("123.45"));
    Assert::IsTrue(op.IsNumber());
    Assert::AreEqual(Decimal::FromString("123.45"), op.NumberValue());
}

TEST(Operands, Operand_Implicit_Bool_Test) {
    Operand op = Operand::Create(true);
    Assert::IsTrue(op.IsBoolean());
    Assert::AreEqual(true, op.BooleanValue());

    op = Operand::Create(false);
    Assert::IsTrue(op.IsBoolean());
    Assert::AreEqual(false, op.BooleanValue());
}

TEST(Operands, Operand_Implicit_String_Test) {
    Operand op = Operand::Create(std::string("hello"));
    Assert::IsTrue(op.IsText());
    Assert::AreEqual("hello", op.TextValue());
}

TEST(Operands, Operand_Implicit_DateTime_Test) {
    MyDate dt(2024, 1, 15, 0, 0, 0);
    Operand op = Operand::Create(dt);
    Assert::IsTrue(op.IsDate());
    Assert::AreEqual(2024, op.DateValue().Year);
}

TEST(Operands, Operand_Implicit_TimeSpan_Test) {
    // new TimeSpan(10, 30, 0) = 10 时 30 分
    MyDate ts(0, 0, 0, 10, 30, 0);
    Operand op = Operand::Create(ts);
    Assert::IsTrue(op.IsDate());
    Assert::AreEqual(10, op.DateValue().Hour);
    Assert::AreEqual(30, op.DateValue().Minute);
}

TEST(Operands, Operand_Implicit_List_Test) {
    Operand op = Operand::Create(std::vector<Operand>{Operand::Create(1), Operand::Create(2),
                                                      Operand::Create(3)});
    Assert::IsTrue(op.IsArray());
    Assert::AreEqual(3, static_cast<int>(op.ArrayValue().size()));

    op = Operand::Create(
        std::vector<Operand>{Operand::Create(std::string("a")), Operand::Create(std::string("b"))});
    Assert::IsTrue(op.IsArray());
    Assert::AreEqual(2, static_cast<int>(op.ArrayValue().size()));

    op = Operand::Create(std::vector<Operand>{Operand::Create(1.1), Operand::Create(2.2)});
    Assert::IsTrue(op.IsArray());
    Assert::AreEqual(2, static_cast<int>(op.ArrayValue().size()));

    op = Operand::Create(std::vector<Operand>{Operand::Create(true), Operand::Create(false)});
    Assert::IsTrue(op.IsArray());
    Assert::AreEqual(2, static_cast<int>(op.ArrayValue().size()));
}

}  // namespace
