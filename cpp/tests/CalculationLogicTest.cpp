// 移植自 csharp/ToolGood.Algorithm.Test/CalculationLogic/CalculationLogicTest.cs
//
// 说明：
//   * C# 的 SetUp/TearDown 把 Thread.CurrentThread.CurrentCulture 设为
//     InvariantCulture，此处按移植指南删除（C++ 侧不做区域设置，数值格式化
//     始终与 InvariantCulture 一致）。
//   * new CalculationLogicEngine(new FunctionCache(), useCalculationLogicInfo)
//     -> FunctionCache cache; CalculationLogicEngine logic(cache, useCalculationLogicInfo);
//        C++ 侧构造函数以引用持有缓存，故用 TestLogic 保证 cache 生命周期覆盖 logic。
//        C++ 构造函数的 useCalculationLogicInfo 默认值为 false，而 C# 默认为 true，
//        因此这里总是显式传入。
//   * decimal 字面量 -> Decimal::FromString(...)；new DateTime(...) -> MyDate(...)
//   * FormatException -> std::runtime_error，ex.Message -> e.what()
//   * 字符串实参显式写成 std::string(...)，避免 const char* 被解析到 bool 重载
#include "cpptest.h"

#include <exception>
#include <string>

#include "toolgood/AlgorithmEngineHelper.h"
#include "toolgood/CalculationLogic/CalculationLogic.h"

using namespace toolgood;

namespace {

/// 对应 C# 的 CreateEngine(useCalculationLogicInfo)（缓存与引擎同生命周期）
struct TestLogic {
    FunctionCache cache;
    CalculationLogicEngine logic;

    explicit TestLogic(bool useCalculationLogicInfo = true) : cache(), logic(cache, useCalculationLogicInfo) {}
};

/// 对应 C# 的 NormalizeLine：C++ 侧换行固定为 "\n"，此处仅做等价归一化
std::string NormalizeLine(const std::string& text) {
    std::string r;
    r.reserve(text.size());
    for (size_t i = 0; i < text.size(); i++) {
        if (text[i] == '\r' && i + 1 < text.size() && text[i + 1] == '\n') continue;
        r += text[i];
    }
    return r;
}

// -------------------------------- SetSceneName --------------------------------

TEST(CalcLogic, SetSceneName_Test) {
    TestLogic tl(true);
    CalculationLogicEngine& logic = tl.logic;
    logic.SetSceneName("场景1");
    Assert::AreEqual(std::string("===== 场景1 =====\n"), NormalizeLine(logic.ToInfoString()));
}

TEST(CalcLogic, SetSceneName_NoInfo_Test) {
    TestLogic tl(false);
    CalculationLogicEngine& logic = tl.logic;
    logic.SetSceneName("场景1");
    Assert::AreEqual(std::string(""), logic.ToInfoString());
}

// --------------------------------- InitValue ---------------------------------

TEST(CalcLogic, InitValue_Int_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.InitValue("a", 1);
    logic.InitValue("b", 2);
    Assert::AreEqual(std::string("[初始] a=1;b=2;\n"), NormalizeLine(logic.ToInfoString()));
}

TEST(CalcLogic, InitValue_String_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.InitValue("s", std::string("hello"));
    Assert::AreEqual(std::string("[初始] s=hello;\n"), NormalizeLine(logic.ToInfoString()));
}

TEST(CalcLogic, InitValue_Decimal_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.InitValue("d", Decimal::FromString("1.5"));
    Assert::AreEqual(std::string("[初始] d=1.5;\n"), NormalizeLine(logic.ToInfoString()));
}

TEST(CalcLogic, InitValue_Double_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.InitValue("dd", 1.5);
    Assert::AreEqual(std::string("[初始] dd=1.5;\n"), NormalizeLine(logic.ToInfoString()));
}

TEST(CalcLogic, InitValue_Bool_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.InitValue("flag", true);
    Assert::AreEqual(std::string("[初始] flag=True;\n"), NormalizeLine(logic.ToInfoString()));
}

TEST(CalcLogic, InitValue_LayerRemark_Test) {
    // InitValue 的层级与备注不参与信息输出
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.InitValue("i", 100, 2, "备注");
    Assert::AreEqual(std::string("[初始] i=100;\n"), NormalizeLine(logic.ToInfoString()));
}

TEST(CalcLogic, InitValue_CanBeUsedInCondition_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.InitValue("a", 1);
    logic.InitValue("b", 2);
    Assert::IsTrue(logic.CheckCondition("a<b"));
    Assert::IsFalse(logic.CheckCondition("a>b"));
}

TEST(CalcLogic, InitValue_DateTime_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.InitValue("dt", MyDate(2026, 8, 31, 12, 0, 0));
    Assert::AreEqual(std::string("[初始] dt=\"2026-08-31 12:00:00\";\n"),
                     NormalizeLine(logic.ToInfoString()));
}

TEST(CalcLogic, InitValue_DateTime_LayerRemark_Test) {
    // InitValue 的层级与备注不参与信息输出
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.InitValue("dt", MyDate(2026, 8, 31, 12, 0, 0), 2, "备注");
    Assert::AreEqual(std::string("[初始] dt=\"2026-08-31 12:00:00\";\n"),
                     NormalizeLine(logic.ToInfoString()));
}

TEST(CalcLogic, InitValue_DateTime_CanBeUsedInCondition_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.InitValue("dt", MyDate(2026, 8, 31, 12, 0, 0));
    Assert::IsTrue(logic.CheckCondition("YEAR(dt)=2026"));
    Assert::IsTrue(logic.CheckCondition("MONTH(dt)=8"));
    Assert::IsTrue(logic.CheckCondition("dt>DATEVALUE('2026-08-30')"));
}

// ------------------------------- CheckCondition -------------------------------

TEST(CalcLogic, CheckCondition_True_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.InitValue("a", 1);
    logic.InitValue("b", 2);
    Assert::IsTrue(logic.CheckCondition("a<b"));
    Assert::AreEqual(std::string("[初始] a=1;b=2;\n[成功] if a<b: // 1<2\n"),
                     NormalizeLine(logic.ToInfoString()));
}

TEST(CalcLogic, CheckCondition_False_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.InitValue("a", 1);
    logic.InitValue("b", 2);
    Assert::IsFalse(logic.CheckCondition("a>b"));
    Assert::AreEqual(std::string("[初始] a=1;b=2;\n[失败] if a>b: // 1>2\n"),
                     NormalizeLine(logic.ToInfoString()));
}

TEST(CalcLogic, CheckCondition_LayerRemark_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.InitValue("a", 1);
    logic.InitValue("b", 2);
    Assert::IsTrue(logic.CheckCondition("a<b", 1, "备注"));
    Assert::AreEqual(std::string("[初始] a=1;b=2;\n[成功]    if a<b: // 1<2 // 备注\n"),
                     NormalizeLine(logic.ToInfoString()));
}

TEST(CalcLogic, CheckCondition_StringCompare_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.InitValue("s", std::string("abc"));
    Assert::IsTrue(logic.CheckCondition("s='abc'"));
    Assert::IsFalse(logic.CheckCondition("s='ab'"));
    Assert::AreEqual(
        std::string("[初始] s=abc;\n[成功] if s='abc': // \"abc\"='abc'\n[失败] if s='ab': // "
                    "\"abc\"='ab'\n"),
        NormalizeLine(logic.ToInfoString()));
}

TEST(CalcLogic, CheckCondition_NonBoolean_Throws_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.InitValue("a", 1);
    logic.InitValue("b", 2);
    bool thrown = false;
    std::string msg;
    try {
        logic.CheckCondition("a+b");
    } catch (const std::exception& ex) {
        thrown = true;
        msg = ex.what();
    }
    Assert::IsTrue(thrown);
    Assert::AreEqual(std::string("The condition must be a boolean value!"), msg);
    Assert::AreEqual(std::string("[初始] a=1;b=2;\n[条件][错误] if a+b: // 1+2 = The condition must "
                                 "be a boolean value!\n"),
                     NormalizeLine(logic.ToInfoString()));
}

TEST(CalcLogic, CheckCondition_Error_Throws_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    bool thrown = false;
    std::string msg;
    try {
        logic.CheckCondition("1/0=0");
    } catch (const std::exception& ex) {
        thrown = true;
        msg = ex.what();
    }
    Assert::IsTrue(thrown);
    Assert::AreEqual(std::string("Function '/' Div 0 error!"), msg);
    Assert::AreEqual(std::string("[条件][错误] if 1/0=0: // 1/0=0 = Function '/' Div 0 error!\n"),
                     NormalizeLine(logic.ToInfoString()));
}

// --------------------------------- SetFormula ---------------------------------

TEST(CalcLogic, SetFormula_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.InitValue("a", 1);
    logic.InitValue("b", 2);
    logic.SetFormula("x", "a*3+b", 1, "备注");
    Assert::AreEqual(std::string("[初始] a=1;b=2;\n[赋值]    x = a*3+b = 1*3+2 = 5 // 备注\n"),
                     NormalizeLine(logic.ToInfoString()));
    // 公式结果应已写入参数
    Assert::IsTrue(logic.CheckCondition("x=5"));
}

TEST(CalcLogic, SetFormula_StringConcat_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.InitValue("s", std::string("hello"));
    logic.SetFormula("y", "s&'!'");
    Assert::AreEqual(std::string("[初始] s=hello;\n[赋值] y = s&'!' = \"hello\"&'!' = hello!\n"),
                     NormalizeLine(logic.ToInfoString()));
    Assert::IsTrue(logic.CheckCondition("y='hello!'"));
}

TEST(CalcLogic, SetFormula_TextAndNumber_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.InitValue("a", 1);
    logic.SetFormula("z", "'text'&a");
    Assert::AreEqual(std::string("[初始] a=1;\n[赋值] z = 'text'&a = 'text'&1 = text1\n"),
                     NormalizeLine(logic.ToInfoString()));
}

TEST(CalcLogic, SetFormula_Error_Throws_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    bool thrown = false;
    std::string msg;
    try {
        logic.SetFormula("x", "1/0");
    } catch (const std::exception& ex) {
        thrown = true;
        msg = ex.what();
    }
    Assert::IsTrue(thrown);
    Assert::AreEqual(std::string("Function '/' Div 0 error!"), msg);
    Assert::AreEqual(
        std::string("[赋值][错误] x = 1/0 = 1/0 // Function '/' Div 0 error!\n"),
        NormalizeLine(logic.ToInfoString()));
}

// ---------------------------------- SetValue ----------------------------------

TEST(CalcLogic, SetValue_String_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.SetValue("s", std::string("文本"));
    Assert::AreEqual(std::string("[赋值] s = \"文本\"\n"), NormalizeLine(logic.ToInfoString()));
    Assert::IsTrue(logic.CheckCondition("s='文本'"));
}

TEST(CalcLogic, SetValue_StringEscape_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.SetValue("sq", std::string("a\"b"));
    Assert::AreEqual(std::string("[赋值] sq = \"a\\\"b\"\n"), NormalizeLine(logic.ToInfoString()));
}

TEST(CalcLogic, SetValue_Decimal_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.SetValue("d", Decimal::FromString("1.5"));
    Assert::AreEqual(std::string("[赋值] d = 1.5\n"), NormalizeLine(logic.ToInfoString()));
}

TEST(CalcLogic, SetValue_Double_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.SetValue("dd", 1.5);
    Assert::AreEqual(std::string("[赋值] dd = 1.5\n"), NormalizeLine(logic.ToInfoString()));
}

TEST(CalcLogic, SetValue_Bool_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.SetValue("flag", true);
    Assert::AreEqual(std::string("[赋值] flag = True\n"), NormalizeLine(logic.ToInfoString()));
}

TEST(CalcLogic, SetValue_LayerRemark_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.SetValue("i", 100, 1, "备注");
    Assert::AreEqual(std::string("[赋值]    i = 100 // 备注\n"), NormalizeLine(logic.ToInfoString()));
}

TEST(CalcLogic, SetValue_DateTime_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.SetValue("dt", MyDate(2026, 8, 31, 12, 0, 0));
    Assert::AreEqual(std::string("[赋值] dt = \"2026-08-31 12:00:00\"\n"),
                     NormalizeLine(logic.ToInfoString()));
}

TEST(CalcLogic, SetValue_DateTime_LayerRemark_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.SetValue("dt", MyDate(2026, 8, 31, 12, 0, 0), 1, "备注");
    Assert::AreEqual(std::string("[赋值]    dt = \"2026-08-31 12:00:00\" // 备注\n"),
                     NormalizeLine(logic.ToInfoString()));
}

TEST(CalcLogic, SetValue_DateTime_CanBeUsedInCondition_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.SetValue("dt", MyDate(2026, 8, 31, 12, 0, 0));
    Assert::IsTrue(logic.CheckCondition("YEAR(dt)=2026"));
    Assert::IsTrue(logic.CheckCondition("MONTH(dt)=8"));
    Assert::IsTrue(logic.CheckCondition("dt>DATEVALUE('2026-08-30')"));
}

// ---------------------------------- GetValue ----------------------------------

TEST(CalcLogic, GetValue_InitValue_Int_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.InitValue("a", 1);
    Operand operand = logic.GetValue("a");
    Assert::IsTrue(operand.IsNumber());
    Assert::AreEqual(static_cast<int>(OperandType::NUMBER), static_cast<int>(operand.Type()));
    Assert::AreEqual(Decimal::FromInt(1), operand.NumberValue());
}

TEST(CalcLogic, GetValue_InitValue_String_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.InitValue("s", std::string("hello"));
    Operand operand = logic.GetValue("s");
    Assert::IsTrue(operand.IsText());
    Assert::AreEqual(static_cast<int>(OperandType::TEXT), static_cast<int>(operand.Type()));
    Assert::AreEqual(std::string("hello"), operand.TextValue());
}

TEST(CalcLogic, GetValue_InitValue_Decimal_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.InitValue("d", Decimal::FromString("1.5"));
    Operand operand = logic.GetValue("d");
    Assert::IsTrue(operand.IsNumber());
    Assert::AreEqual(Decimal::FromString("1.5"), operand.NumberValue());
}

TEST(CalcLogic, GetValue_InitValue_Double_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.InitValue("dd", 1.5);
    Operand operand = logic.GetValue("dd");
    Assert::IsTrue(operand.IsNumber());
    Assert::AreEqual(Decimal::FromString("1.5"), operand.NumberValue());
}

TEST(CalcLogic, GetValue_InitValue_Bool_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.InitValue("flag", true);
    Operand operand = logic.GetValue("flag");
    Assert::IsTrue(operand.IsBoolean());
    Assert::AreEqual(static_cast<int>(OperandType::BOOLEAN), static_cast<int>(operand.Type()));
    Assert::IsTrue(operand.BooleanValue());
}

TEST(CalcLogic, GetValue_InitValue_DateTime_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.InitValue("dt", MyDate(2026, 8, 31, 12, 0, 0));
    Operand operand = logic.GetValue("dt");
    Assert::IsTrue(operand.IsDate());
    Assert::AreEqual(static_cast<int>(OperandType::DATE), static_cast<int>(operand.Type()));
    // C#: operand.DateValue.ToDateTime()
    Assert::AreEqual(MyDate(2026, 8, 31, 12, 0, 0), operand.DateValue());
}

TEST(CalcLogic, GetValue_SetValue_DateTime_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.SetValue("dt", MyDate(2026, 8, 31, 12, 0, 0));
    Operand operand = logic.GetValue("dt");
    Assert::IsTrue(operand.IsDate());
    Assert::AreEqual(static_cast<int>(OperandType::DATE), static_cast<int>(operand.Type()));
    // C#: operand.DateValue.ToDateTime()
    Assert::AreEqual(MyDate(2026, 8, 31, 12, 0, 0), operand.DateValue());
}

TEST(CalcLogic, GetValue_SetValue_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.SetValue("v", std::string("text"));
    Operand operand = logic.GetValue("v");
    Assert::IsTrue(operand.IsText());
    Assert::AreEqual(std::string("text"), operand.TextValue());
}

TEST(CalcLogic, GetValue_SetFormula_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.InitValue("a", 1);
    logic.InitValue("b", 2);
    logic.SetFormula("x", "a*3+b");
    Operand operand = logic.GetValue("x");
    Assert::IsTrue(operand.IsNumber());
    Assert::AreEqual(Decimal::FromInt(5), operand.NumberValue());
}

TEST(CalcLogic, GetValue_AfterSetFormulaOverwrite_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.InitValue("a", 1);
    logic.SetFormula("a", "a+10");
    Operand operand = logic.GetValue("a");
    Assert::IsTrue(operand.IsNumber());
    Assert::AreEqual(Decimal::FromInt(11), operand.NumberValue());
}

TEST(CalcLogic, GetValue_NotFound_ReturnsError_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    Operand operand = logic.GetValue("missing");
    Assert::IsTrue(operand.IsError());
    Assert::AreEqual(std::string("Parameter [missing] is missing."), operand.ErrorMsg());
}

TEST(CalcLogic, GetValue_NoInfo_Test) {
    // 关闭信息记录不影响取值
    TestLogic tl(false);
    CalculationLogicEngine& logic = tl.logic;
    logic.InitValue("a", 1);
    Assert::AreEqual(Decimal::FromInt(1), logic.GetValue("a").NumberValue());
}

// --------------------------------- BlankLine ---------------------------------

TEST(CalcLogic, BlankLine_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.BlankLine();
    Assert::AreEqual(std::string("\n"), NormalizeLine(logic.ToInfoString()));

    logic.BlankLine();
    logic.BlankLine();
    Assert::AreEqual(std::string("\n\n\n"), NormalizeLine(logic.ToInfoString()));
}

// -------------------------------- ToInfoString --------------------------------

TEST(CalcLogic, ToInfoString_Empty_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    Assert::AreEqual(std::string(""), logic.ToInfoString());
}

TEST(CalcLogic, ToInfoString_Disabled_Test) {
    // 关闭信息记录后，所有操作都不产生信息
    TestLogic tl(false);
    CalculationLogicEngine& logic = tl.logic;
    logic.SetSceneName("场景1");
    logic.InitValue("a", 1);
    logic.CheckCondition("a>0");
    logic.SetValue("b", 2);
    logic.SetFormula("c", "a+1");
    logic.BlankLine();
    Assert::AreEqual(std::string(""), logic.ToInfoString());
}

TEST(CalcLogic, ToInfoString_FullFlow_Test) {
    TestLogic tl;
    CalculationLogicEngine& logic = tl.logic;
    logic.SetSceneName("场景1");
    logic.InitValue("a", 1);
    logic.InitValue("b", 2);
    if (logic.CheckCondition("a>b")) {
        logic.SetValue("c", 3, 1);
        logic.SetFormula("c", "a*5+b", 1);
    } else if (logic.CheckCondition("a<b")) {
        logic.SetValue("c", 4, 1);
        logic.SetFormula("c", "a*3+b", 1);
    }
    logic.BlankLine();
    logic.SetValue("e", 5);

    const std::string expected = std::string("[初始] a=1;b=2;\n") + "===== 场景1 =====\n" +
                                 "[失败] if a>b: // 1>2\n" + "[成功] if a<b: // 1<2\n" +
                                 "[赋值]    c = 4\n" + "[赋值]    c = a*3+b = 1*3+2 = 5\n" + "\n" +
                                 "[赋值] e = 5\n";
    Assert::AreEqual(expected, NormalizeLine(logic.ToInfoString()));
}

}  // namespace
