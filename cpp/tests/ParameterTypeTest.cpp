// 移植自 csharp/ToolGood.Algorithm.Test/ParameterTypes/ParameterTypeTest.cs
//
// 说明：C# 的 `Assert.IsNotNull(list)` 在此略去——C++ 的 GetParameterTypes 返回
// `std::vector<ParameterType>`（值类型），恒非 null；其余断言与 C# 一一对应。
#include "cpptest.h"

#include "toolgood/AlgorithmEngine.h"
#include "toolgood/AlgorithmEngineHelper.h"

using namespace toolgood;

namespace {

// ======================= 基础测试 =======================

TEST(ParameterType, Test_Basic_NumberParameter) {
    auto fb = AlgorithmEngineHelper::ParseFormula("test+1");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual("test", list[0].Name);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
}

TEST(ParameterType, Test_Basic_BooleanParameter) {
    auto fb = AlgorithmEngineHelper::ParseFormula("if(test,2,3)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual("test", list[0].Name);
    Assert::AreEqual((int)OperandType::BOOLEAN, (int)list[0].Type);
}

TEST(ParameterType, Test_Basic_TextParameter) {
    auto fb = AlgorithmEngineHelper::ParseFormula("left(test,ww)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(2, (int)list.size());
    Assert::AreEqual("test", list[0].Name);
    Assert::AreEqual("ww", list[1].Name);
    Assert::AreEqual((int)OperandType::TEXT, (int)list[0].Type);
}

// ======================= 比较操作符测试 =======================

TEST(ParameterType, Test_Compare_Equal_Number) {
    auto fb = AlgorithmEngineHelper::ParseFormula("if(test=1,2,3)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual("test", list[0].Name);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
    Assert::AreEqual("==", list[0].Operator);
    Assert::AreEqual("1", list[0].Value);
}

TEST(ParameterType, Test_Compare_Equal_Text) {
    auto fb = AlgorithmEngineHelper::ParseFormula("if(test='123tt',2,3)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual("test", list[0].Name);
    Assert::AreEqual((int)OperandType::TEXT, (int)list[0].Type);
    Assert::AreEqual("==", list[0].Operator);
    Assert::AreEqual("123tt", list[0].Value);
}

TEST(ParameterType, Test_Compare_GreaterThanOrEqual) {
    auto fb = AlgorithmEngineHelper::ParseFormula("if(test>=1,2,3)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual("test", list[0].Name);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
    Assert::AreEqual(">=", list[0].Operator);
    Assert::AreEqual("1", list[0].Value);
}

TEST(ParameterType, Test_Compare_GreaterThanOrEqual_Expression) {
    auto fb = AlgorithmEngineHelper::ParseFormula("if(test>=1+1,2,3)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual("test", list[0].Name);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
    Assert::AreEqual(">=", list[0].Operator);
    Assert::AreEqual("2", list[0].Value);
}

TEST(ParameterType, Test_Compare_LessThan) {
    auto fb = AlgorithmEngineHelper::ParseFormula("if(test<100,1,0)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual("test", list[0].Name);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
    Assert::AreEqual("<", list[0].Operator);
    Assert::AreEqual("100", list[0].Value);
}

TEST(ParameterType, Test_Compare_LessThanOrEqual) {
    auto fb = AlgorithmEngineHelper::ParseFormula("if(test<=50,1,0)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual("test", list[0].Name);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
    Assert::AreEqual("<=", list[0].Operator);
    Assert::AreEqual("50", list[0].Value);
}

TEST(ParameterType, Test_Compare_GreaterThan) {
    auto fb = AlgorithmEngineHelper::ParseFormula("if(test>0,1,0)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual("test", list[0].Name);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
    Assert::AreEqual(">", list[0].Operator);
    Assert::AreEqual("0", list[0].Value);
}

TEST(ParameterType, Test_Compare_NotEqual) {
    auto fb = AlgorithmEngineHelper::ParseFormula("if(test!=0,1,0)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual("test", list[0].Name);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
    Assert::AreEqual("!=", list[0].Operator);
    Assert::AreEqual("0", list[0].Value);
}

// ======================= 逻辑运算符测试 =======================

TEST(ParameterType, Test_Logic_And) {
    auto fb = AlgorithmEngineHelper::ParseFormula("if(a && b,1,0)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(2, (int)list.size());
    Assert::AreEqual("a", list[0].Name);
    Assert::AreEqual((int)OperandType::BOOLEAN, (int)list[0].Type);
    Assert::AreEqual("b", list[1].Name);
    Assert::AreEqual((int)OperandType::BOOLEAN, (int)list[1].Type);
}

TEST(ParameterType, Test_Logic_Or) {
    auto fb = AlgorithmEngineHelper::ParseFormula("if(a || b,1,0)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(2, (int)list.size());
    Assert::AreEqual("a", list[0].Name);
    Assert::AreEqual((int)OperandType::BOOLEAN, (int)list[0].Type);
    Assert::AreEqual("b", list[1].Name);
    Assert::AreEqual((int)OperandType::BOOLEAN, (int)list[1].Type);
}

TEST(ParameterType, Test_Logic_Not) {
    auto fb = AlgorithmEngineHelper::ParseFormula("if(!a,1,0)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual("a", list[0].Name);
    Assert::AreEqual((int)OperandType::BOOLEAN, (int)list[0].Type);
}

TEST(ParameterType, Test_Logic_Complex) {
    auto fb = AlgorithmEngineHelper::ParseFormula("if(a && b || c,1,0)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(3, (int)list.size());
    Assert::AreEqual((int)OperandType::BOOLEAN, (int)list[0].Type);
    Assert::AreEqual((int)OperandType::BOOLEAN, (int)list[1].Type);
    Assert::AreEqual((int)OperandType::BOOLEAN, (int)list[2].Type);
}

TEST(ParameterType, Test_Logic_And_Function) {
    auto fb = AlgorithmEngineHelper::ParseFormula("and(a,b,c)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(3, (int)list.size());
    Assert::AreEqual((int)OperandType::BOOLEAN, (int)list[0].Type);
    Assert::AreEqual((int)OperandType::BOOLEAN, (int)list[1].Type);
    Assert::AreEqual((int)OperandType::BOOLEAN, (int)list[2].Type);
}

TEST(ParameterType, Test_Logic_Or_Function) {
    auto fb = AlgorithmEngineHelper::ParseFormula("or(a,b)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(2, (int)list.size());
    Assert::AreEqual((int)OperandType::BOOLEAN, (int)list[0].Type);
    Assert::AreEqual((int)OperandType::BOOLEAN, (int)list[1].Type);
}

// ======================= 数学运算测试 =======================

TEST(ParameterType, Test_Math_Add) {
    auto fb = AlgorithmEngineHelper::ParseFormula("a+b");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(2, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[1].Type);
}

TEST(ParameterType, Test_Math_Subtract) {
    auto fb = AlgorithmEngineHelper::ParseFormula("a-b");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(2, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[1].Type);
}

TEST(ParameterType, Test_Math_Multiply) {
    auto fb = AlgorithmEngineHelper::ParseFormula("a*b");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(2, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[1].Type);
}

TEST(ParameterType, Test_Math_Divide) {
    auto fb = AlgorithmEngineHelper::ParseFormula("a/b");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(2, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[1].Type);
}

TEST(ParameterType, Test_Math_Mod) {
    auto fb = AlgorithmEngineHelper::ParseFormula("a%b");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(2, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[1].Type);
}

TEST(ParameterType, Test_Math_ABS) {
    auto fb = AlgorithmEngineHelper::ParseFormula("abs(a)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
}

TEST(ParameterType, Test_Math_SQRT) {
    auto fb = AlgorithmEngineHelper::ParseFormula("sqrt(a)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
}

TEST(ParameterType, Test_Math_ROUND) {
    auto fb = AlgorithmEngineHelper::ParseFormula("round(a,b)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(2, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[1].Type);
}

TEST(ParameterType, Test_Math_CEILING) {
    auto fb = AlgorithmEngineHelper::ParseFormula("ceiling(a,0.5)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
}

TEST(ParameterType, Test_Math_FLOOR) {
    auto fb = AlgorithmEngineHelper::ParseFormula("floor(a,1)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
}

TEST(ParameterType, Test_Math_POWER) {
    auto fb = AlgorithmEngineHelper::ParseFormula("power(a,b)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(2, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[1].Type);
}

TEST(ParameterType, Test_Math_SUM) {
    auto fb = AlgorithmEngineHelper::ParseFormula("sum(a,b,c)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(3, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[1].Type);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[2].Type);
}

TEST(ParameterType, Test_Math_MAX) {
    auto fb = AlgorithmEngineHelper::ParseFormula("max(a,b,c)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(3, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
}

TEST(ParameterType, Test_Math_MIN) {
    auto fb = AlgorithmEngineHelper::ParseFormula("min(a,b)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(2, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
}

TEST(ParameterType, Test_Math_AVERAGE) {
    auto fb = AlgorithmEngineHelper::ParseFormula("average(a,b,c)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(3, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
}

// ======================= 字符串函数测试 =======================

TEST(ParameterType, Test_String_LEFT) {
    auto fb = AlgorithmEngineHelper::ParseFormula("left(a,n)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(2, (int)list.size());
    Assert::AreEqual((int)OperandType::TEXT, (int)list[0].Type);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[1].Type);
}

TEST(ParameterType, Test_String_RIGHT) {
    auto fb = AlgorithmEngineHelper::ParseFormula("right(a,n)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(2, (int)list.size());
    Assert::AreEqual((int)OperandType::TEXT, (int)list[0].Type);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[1].Type);
}

TEST(ParameterType, Test_String_MID) {
    auto fb = AlgorithmEngineHelper::ParseFormula("mid(a,p1,p2)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(3, (int)list.size());
    Assert::AreEqual((int)OperandType::TEXT, (int)list[0].Type);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[1].Type);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[2].Type);
}

TEST(ParameterType, Test_String_LEN) {
    auto fb = AlgorithmEngineHelper::ParseFormula("len(a)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::TEXT, (int)list[0].Type);
}

TEST(ParameterType, Test_String_UPPER) {
    auto fb = AlgorithmEngineHelper::ParseFormula("upper(a)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::TEXT, (int)list[0].Type);
}

TEST(ParameterType, Test_String_LOWER) {
    auto fb = AlgorithmEngineHelper::ParseFormula("lower(a)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::TEXT, (int)list[0].Type);
}

TEST(ParameterType, Test_String_TRIM) {
    auto fb = AlgorithmEngineHelper::ParseFormula("trim(a)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::TEXT, (int)list[0].Type);
}

TEST(ParameterType, Test_String_REPLACE) {
    auto fb = AlgorithmEngineHelper::ParseFormula("replace(a,p1,p2,p3)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(4, (int)list.size());
    Assert::AreEqual((int)OperandType::TEXT, (int)list[0].Type);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[1].Type);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[2].Type);
}

TEST(ParameterType, Test_String_SUBSTITUTE) {
    auto fb = AlgorithmEngineHelper::ParseFormula("substitute(a,'old','new')");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::TEXT, (int)list[0].Type);
}

TEST(ParameterType, Test_String_FIND) {
    auto fb = AlgorithmEngineHelper::ParseFormula("find('test',a)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::TEXT, (int)list[0].Type);
}

TEST(ParameterType, Test_String_CONCATENATE) {
    auto fb = AlgorithmEngineHelper::ParseFormula("concatenate(a,b,c)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(3, (int)list.size());
    Assert::AreEqual((int)OperandType::TEXT, (int)list[0].Type);
}

TEST(ParameterType, Test_String_CONNECT) {
    auto fb = AlgorithmEngineHelper::ParseFormula("a & b");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(2, (int)list.size());
    Assert::AreEqual((int)OperandType::TEXT, (int)list[0].Type);
    Assert::AreEqual((int)OperandType::TEXT, (int)list[1].Type);
}

// ======================= 日期函数测试 =======================

TEST(ParameterType, Test_Date_DATE) {
    auto fb = AlgorithmEngineHelper::ParseFormula("date(yr,mon,dy)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(3, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[1].Type);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[2].Type);
}

TEST(ParameterType, Test_Date_DATE_WithTime) {
    auto fb = AlgorithmEngineHelper::ParseFormula("date(yr,mon,dy,hr,m1,s1)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(6, (int)list.size());
    for (int i = 0; i < 6; i++) {
        Assert::AreEqual((int)OperandType::NUMBER, (int)list[i].Type);
    }
}

TEST(ParameterType, Test_Date_YEAR) {
    auto fb = AlgorithmEngineHelper::ParseFormula("year(dt)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::DATE, (int)list[0].Type);
}

TEST(ParameterType, Test_Date_MONTH) {
    auto fb = AlgorithmEngineHelper::ParseFormula("month(dt)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::DATE, (int)list[0].Type);
}

TEST(ParameterType, Test_Date_DAY) {
    auto fb = AlgorithmEngineHelper::ParseFormula("day(dt)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::DATE, (int)list[0].Type);
}

TEST(ParameterType, Test_Date_HOUR) {
    auto fb = AlgorithmEngineHelper::ParseFormula("hour(dt)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::DATE, (int)list[0].Type);
}

TEST(ParameterType, Test_Date_MINUTE) {
    auto fb = AlgorithmEngineHelper::ParseFormula("minute(dt)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::DATE, (int)list[0].Type);
}

TEST(ParameterType, Test_Date_SECOND) {
    auto fb = AlgorithmEngineHelper::ParseFormula("second(dt)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::DATE, (int)list[0].Type);
}

TEST(ParameterType, Test_Date_ADDDAYS) {
    auto fb = AlgorithmEngineHelper::ParseFormula("adddays(dt,num)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(2, (int)list.size());
    Assert::AreEqual((int)OperandType::DATE, (int)list[0].Type);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[1].Type);
}

TEST(ParameterType, Test_Date_DATEDIF) {
    auto fb = AlgorithmEngineHelper::ParseFormula("datedif(startDt,endDt,'d')");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(2, (int)list.size());
    Assert::AreEqual((int)OperandType::DATE, (int)list[0].Type);
    Assert::AreEqual((int)OperandType::DATE, (int)list[1].Type);
}

// ======================= 类型检查函数测试 =======================

TEST(ParameterType, Test_TypeCheck_ISNUMBER) {
    auto fb = AlgorithmEngineHelper::ParseFormula("isnumber(a)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::NONE, (int)list[0].Type);
}

TEST(ParameterType, Test_TypeCheck_ISTEXT) {
    auto fb = AlgorithmEngineHelper::ParseFormula("istext(a)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::NONE, (int)list[0].Type);
}

TEST(ParameterType, Test_TypeCheck_ISNULL) {
    auto fb = AlgorithmEngineHelper::ParseFormula("isnull(a)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::NONE, (int)list[0].Type);
}

TEST(ParameterType, Test_TypeCheck_ISNULLOREMPTY) {
    auto fb = AlgorithmEngineHelper::ParseFormula("isnullorempty(a)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::NONE, (int)list[0].Type);
}

TEST(ParameterType, Test_TypeCheck_ISERROR) {
    auto fb = AlgorithmEngineHelper::ParseFormula("iserror(a)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::NONE, (int)list[0].Type);
}

TEST(ParameterType, Test_TypeCheck_ISEVEN) {
    auto fb = AlgorithmEngineHelper::ParseFormula("iseven(a)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
}

TEST(ParameterType, Test_TypeCheck_ISODD) {
    auto fb = AlgorithmEngineHelper::ParseFormula("isodd(a)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
}

TEST(ParameterType, Test_TypeCheck_ISLOGICAL) {
    auto fb = AlgorithmEngineHelper::ParseFormula("islogical(a)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::NONE, (int)list[0].Type);
}

// ======================= 流程控制函数测试 =======================

TEST(ParameterType, Test_Flow_IF_WithCondition) {
    auto fb = AlgorithmEngineHelper::ParseFormula("if(score>=60,'Pass','Fail')");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual("score", list[0].Name);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
    Assert::AreEqual(">=", list[0].Operator);
    Assert::AreEqual("60", list[0].Value);
}

TEST(ParameterType, Test_Flow_IFS) {
    auto fb = AlgorithmEngineHelper::ParseFormula("ifs(a=1,'One',a=2,'Two',a=3,'Three')");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(3, (int)list.size());
    Assert::AreEqual("a", list[0].Name);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
}

TEST(ParameterType, Test_Flow_IFERROR) {
    auto fb = AlgorithmEngineHelper::ParseFormula("iferror(a/b,0)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(2, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[1].Type);
}

TEST(ParameterType, Test_Flow_SWITCH) {
    auto fb = AlgorithmEngineHelper::ParseFormula("switch(a,b,c)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(3, (int)list.size());
    Assert::AreEqual("a", list[0].Name);
    Assert::AreEqual((int)OperandType::NONE, (int)list[0].Type);
}

// ======================= 条件统计函数测试 =======================

TEST(ParameterType, Test_Conditional_SUMIF) {
    auto fb = AlgorithmEngineHelper::ParseFormula("sumif(values,'>10')");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::ARRAY, (int)list[0].Type);
}

TEST(ParameterType, Test_Conditional_SUMIF_WithSumRange) {
    auto fb = AlgorithmEngineHelper::ParseFormula("sumif(range,'>10',sumRange)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(2, (int)list.size());
    Assert::AreEqual((int)OperandType::ARRAY, (int)list[0].Type);
    Assert::AreEqual((int)OperandType::ARRAY, (int)list[1].Type);
}

TEST(ParameterType, Test_Conditional_COUNTIF) {
    auto fb = AlgorithmEngineHelper::ParseFormula("countif(values,'>5')");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::ARRAY, (int)list[0].Type);
}

TEST(ParameterType, Test_Conditional_AVERAGEIF) {
    auto fb = AlgorithmEngineHelper::ParseFormula("averageif(values,'>0')");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::ARRAY, (int)list[0].Type);
}

// ======================= 三角函数测试 =======================

TEST(ParameterType, Test_Trig_SIN) {
    auto fb = AlgorithmEngineHelper::ParseFormula("sin(angle)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
}

TEST(ParameterType, Test_Trig_COS) {
    auto fb = AlgorithmEngineHelper::ParseFormula("cos(angle)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
}

TEST(ParameterType, Test_Trig_TAN) {
    auto fb = AlgorithmEngineHelper::ParseFormula("tan(angle)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
}

TEST(ParameterType, Test_Trig_ASIN) {
    auto fb = AlgorithmEngineHelper::ParseFormula("asin(val)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
}

TEST(ParameterType, Test_Trig_ACOS) {
    auto fb = AlgorithmEngineHelper::ParseFormula("acos(val)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
}

TEST(ParameterType, Test_Trig_ATAN2) {
    auto fb = AlgorithmEngineHelper::ParseFormula("atan2(y,x)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(2, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[1].Type);
}

TEST(ParameterType, Test_Trig_DEGREES) {
    auto fb = AlgorithmEngineHelper::ParseFormula("degrees(rad)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
}

TEST(ParameterType, Test_Trig_RADIANS) {
    auto fb = AlgorithmEngineHelper::ParseFormula("radians(deg)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
}

// ======================= 进制转换函数测试 =======================

TEST(ParameterType, Test_Conversion_BIN2DEC) {
    auto fb = AlgorithmEngineHelper::ParseFormula("bin2dec(binary)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
}

TEST(ParameterType, Test_Conversion_DEC2BIN) {
    auto fb = AlgorithmEngineHelper::ParseFormula("dec2bin(decimal)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
}

TEST(ParameterType, Test_Conversion_HEX2DEC) {
    auto fb = AlgorithmEngineHelper::ParseFormula("hex2dec(hex)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(1, (int)list.size());
}

// ======================= 复杂表达式测试 =======================

TEST(ParameterType, Test_Complex_NestedIF) {
    auto fb = AlgorithmEngineHelper::ParseFormula("if(a>100,'High',if(a>50,'Medium','Low'))");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(2, (int)list.size());
    Assert::AreEqual("a", list[0].Name);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
}

TEST(ParameterType, Test_Complex_MultipleParameters) {
    auto fb = AlgorithmEngineHelper::ParseFormula("(a+b)*(c-d)/e()");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(4, (int)list.size());
    for (const auto& p : list) {
        Assert::AreEqual((int)OperandType::NUMBER, (int)p.Type);
    }
}

TEST(ParameterType, Test_Complex_MixedTypes) {
    auto fb = AlgorithmEngineHelper::ParseFormula("if(amount>1000, 'High: ' & name, 'Low')");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(2, (int)list.size());
    Assert::AreEqual("amount", list[0].Name);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
    Assert::AreEqual("name", list[1].Name);
    Assert::AreEqual((int)OperandType::TEXT, (int)list[1].Type);
}

TEST(ParameterType, Test_Complex_DateCalculation) {
    auto fb = AlgorithmEngineHelper::ParseFormula("adddays(startDate, duration)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(2, (int)list.size());
    Assert::AreEqual((int)OperandType::DATE, (int)list[0].Type);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[1].Type);
}

TEST(ParameterType, Test_Complex_ScoreGrade) {
    auto fb = AlgorithmEngineHelper::ParseFormula(
        "ifs(score>=90,'A',score>=80,'B',score>=70,'C',score>=60,'D',true,'F')");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(4, (int)list.size());
    Assert::AreEqual("score", list[0].Name);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
}

TEST(ParameterType, Test_Complex_StringManipulation) {
    auto fb = AlgorithmEngineHelper::ParseFormula(
        "upper(left(name,1)) & lower(mid(name,2,len(name)-1))");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(3, (int)list.size());
    Assert::AreEqual("name", list[0].Name);
    Assert::AreEqual((int)OperandType::TEXT, (int)list[0].Type);
}

TEST(ParameterType, Test_Complex_ConditionalSum) {
    auto fb = AlgorithmEngineHelper::ParseFormula("sumif(amounts,'>0')/countif(amounts,'>0')");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(2, (int)list.size());
    Assert::AreEqual((int)OperandType::ARRAY, (int)list[0].Type);
}

TEST(ParameterType, Test_Complex_BooleanExpression) {
    auto fb = AlgorithmEngineHelper::ParseFormula(
        "if((age>=18 && age<=65) || isVIP, 'Eligible', 'Not Eligible')");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(3, (int)list.size());
    Assert::AreEqual("age", list[0].Name);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
    Assert::AreEqual("isVIP", list[2].Name);
    Assert::AreEqual((int)OperandType::BOOLEAN, (int)list[2].Type);
}

// ======================= 金融函数测试 =======================

TEST(ParameterType, Test_Financial_PMT) {
    auto fb = AlgorithmEngineHelper::ParseFormula("pmt(arg1,arg2,arg3)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(3, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[1].Type);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[2].Type);
}

TEST(ParameterType, Test_Financial_PV) {
    auto fb = AlgorithmEngineHelper::ParseFormula("pv(arg1,arg2,arg3)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(3, (int)list.size());
    for (const auto& p : list) {
        Assert::AreEqual((int)OperandType::NUMBER, (int)p.Type);
    }
}

TEST(ParameterType, Test_Financial_FV) {
    auto fb = AlgorithmEngineHelper::ParseFormula("fv(arg1,arg2,arg3)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(3, (int)list.size());
    for (const auto& p : list) {
        Assert::AreEqual((int)OperandType::NUMBER, (int)p.Type);
    }
}

TEST(ParameterType, Test_Financial_NPV) {
    auto fb = AlgorithmEngineHelper::ParseFormula("npv(arg1,arg2)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(2, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
}

// ======================= 数组函数测试 =======================

TEST(ParameterType, Test_Array_Array) {
    auto fb = AlgorithmEngineHelper::ParseFormula("array(a,b,c)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(3, (int)list.size());
}

TEST(ParameterType, Test_Array_Large) {
    auto fb = AlgorithmEngineHelper::ParseFormula("large(arr,k)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(2, (int)list.size());
    Assert::AreEqual((int)OperandType::ARRAY, (int)list[0].Type);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[1].Type);
}

TEST(ParameterType, Test_Array_Small) {
    auto fb = AlgorithmEngineHelper::ParseFormula("small(arr,k)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(2, (int)list.size());
    Assert::AreEqual((int)OperandType::ARRAY, (int)list[0].Type);
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[1].Type);
}

TEST(ParameterType, Test_Array_Rank) {
    auto fb = AlgorithmEngineHelper::ParseFormula("rank(num,ref)");
    AlgorithmEngine engine;
    auto list = fb->GetParameterTypes(engine);
    Assert::AreEqual(2, (int)list.size());
    Assert::AreEqual((int)OperandType::NUMBER, (int)list[0].Type);
    Assert::AreEqual((int)OperandType::ARRAY, (int)list[1].Type);
}

}  // namespace
