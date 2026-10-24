// cpp/tests/tools/smoke.cpp - 端到端冒烟测试
#include <cstdio>
#include <string>
#include <vector>

#include "toolgood/AlgorithmEngine.h"

using namespace toolgood;

static int g_fail = 0;
static int g_total = 0;

/// 自定义引擎：提供参数
class TestEngine : public AlgorithmEngine {
public:
    Operand GetParameter(const std::string& name) override {
        if (name == "数量") return Operand::Create(3);
        if (name == "单价") return Operand::Create(Decimal::FromString("2.5"));
        if (name == "名称") return Operand::Create("苹果");
        if (name == "列表") return Operand::Create(std::vector<Operand>{
            Operand::Create(1), Operand::Create(2), Operand::Create(3)});
        if (name == "数据") return Operand::CreateJson("{\"a\":10,\"b\":[1,2,3]}");
        return AlgorithmEngine::GetParameter(name);
    }
    Operand ExecuteDiyFunction(const std::string& name, const std::vector<Operand>& args) override {
        if (name == "myadd") {
            Decimal s;
            for (auto& a : args) s = s.Add(a.NumberValue());
            return Operand::Create(s);
        }
        return AlgorithmEngine::ExecuteDiyFunction(name, args);
    }
};

static void Check(TestEngine& engine, const std::string& exp, const std::string& want) {
    g_total++;
    std::string got;
    try {
        Operand o = engine.Calculate(exp);
        if (o.IsError()) got = "ERROR:" + o.ErrorMsg();
        else if (o.IsNumber()) got = o.NumberValue().PlainStrip();
        else if (o.IsText()) got = o.TextValue();
        else if (o.IsBoolean()) got = o.BooleanValue() ? "true" : "false";
        else if (o.IsDate()) got = o.DateValue().ToString();
        else got = o.ToString();
    } catch (const std::exception& ex) {
        got = std::string("EXCEPTION:") + ex.what();
    }
    if (got != want) {
        printf("[FAIL] %-34s got '%s' want '%s'\n", exp.c_str(), got.c_str(), want.c_str());
        g_fail++;
    }
}

int main() {
    TestEngine engine;

    // 数字与运算优先级
    Check(engine, "1+2*3", "7");
    Check(engine, "(1+2)*3", "9");
    Check(engine, "10/4", "2.5");
    Check(engine, "7%3", "1");
    Check(engine, "-7%3", "2");
    Check(engine, "2^2", "EXCEPTION:token recognition error at: '^'");
    Check(engine, "1<2", "true");
    Check(engine, "1>=2", "false");
    Check(engine, "TRUE && FALSE", "false");
    Check(engine, "TRUE || FALSE", "true");
    Check(engine, "!TRUE", "false");
    Check(engine, "1>2?3:4", "4");
    Check(engine, "\"abc\"&\"de\"", "abcde");
    Check(engine, "1&2", "12");

    // 常用函数
    Check(engine, "ABS(-3)", "3");
    Check(engine, "ROUND(2.345,2)", "2.35");
    Check(engine, "ROUND(2.5,0)", "3");
    Check(engine, "SUM(1,2,3)", "6");
    Check(engine, "MAX(1,9,3)", "9");
    Check(engine, "AVERAGE(1,2,3)", "2");
    Check(engine, "IF(1>0,10,20)", "10");
    Check(engine, "IFERROR(1/0,99)", "99");
    Check(engine, "LEN(\"hello\")", "5");
    Check(engine, "LEFT(\"hello\",2)", "he");
    Check(engine, "CONCATENATE(\"a\",\"b\",1)", "ab1");
    Check(engine, "UPPER(\"abc\")", "ABC");
    Check(engine, "SUBSTITUTE(\"a-b-c\",\"-\",\"+\")", "a+b+c");
    Check(engine, "ISNUMBER(1)", "true");
    Check(engine, "ISNUMBER(\"a\")", "false");

    // 数组与 JSON
    Check(engine, "SUM([1,2,3])", "6");
    Check(engine, "ARRAY(1,2,3)[2]", "2");
    Check(engine, "JSON(\"{\\\"a\\\":1}\")[\"a\"]", "1");
    Check(engine, "{\"x\":1,\"y\":2}[\"y\"]", "2");
    Check(engine, "ARRAY(1,2,3)[2]", "2");

    // 参数
    Check(engine, "数量*单价", "7.5");
    Check(engine, "名称", "苹果");
    Check(engine, "SUM(列表)", "6");
    Check(engine, "数据[\"a\"]", "10");
    Check(engine, "数据[\"b\"][3]", "3");
    Check(engine, "PARAM(\"数量\")", "3");
    Check(engine, "PARAM(\"不存在\",100)", "100");

    // 自定义函数
    Check(engine, "myadd(1,2,3.5)", "6.5");

    // 日期
    Check(engine, "YEAR(DATE(2024,2,29))", "2024");
    Check(engine, "MONTH(DATE(2024,2,29))", "2");
    Check(engine, "DAY(DATE(2024,2,29))", "29");
    Check(engine, "DATE(2024,2,29)", "2024-02-29 00:00:00");
    Check(engine, "DAYS(DATE(2024,3,1),DATE(2024,2,1))", "29");
    Check(engine, "ADDDAYS(DATE(2024,1,31),1)", "2024-02-01 00:00:00");

    // 字符串函数
    Check(engine, "MID(\"abcdef\",2,3)", "bcd");
    Check(engine, "RIGHT(\"abcdef\",2)", "ef");
    Check(engine, "FIND(\"cd\",\"abcdef\")", "3");
    Check(engine, "TRIM(\"  a b  \")", "a b");
    Check(engine, "REPT(\"ab\",3)", "ababab");

    // 统计分布
    Check(engine, "NORMSDIST(0)", "0.5");
    Check(engine, "ROUND(NORMSINV(0.975),6)", "1.959964");

    // 进制/单位
    Check(engine, "DEC2HEX(255)", "FF");
    Check(engine, "HEX2DEC(\"FF\")", "255");
    Check(engine, "ROMAN(2024)", "MMXXIV");

    // 错误传播
    Check(engine, "NOSUCHFUNC(1)", "ERROR:DiyFunction [NOSUCHFUNC] is missing.");
    Check(engine, "ABS()", "EXCEPTION:Function 'Abs' requires exactly 1 parameter.");

    printf("--- %d/%d passed\n", g_total - g_fail, g_total);
    return g_fail == 0 ? 0 : 1;
}
