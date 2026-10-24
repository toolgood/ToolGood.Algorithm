// 移植自 csharp/ToolGood.Algorithm.Test/AlgorithmEngine/ValueTest.cs
//
// 类型对应：
//   decimal 字面量 1000m -> Decimal::FromInt(1000)
//   Math.E / Math.PI       -> kE / kPi（避免 M_E/M_PI 在 -std=c++17 下不可用）
//   Assert.IsNull(Function) -> 用 `== nullptr` 断言（C++ Function 是 shared_ptr）
#include "cpptest.h"

#include <cmath>
#include <stdexcept>
#include <string>
#include <typeinfo>
#include <vector>

#include "toolgood/AlgorithmEngine.h"
#include "toolgood/AlgorithmEngineEx.h"

using namespace toolgood;

namespace {

const double kE = 2.71828182845904523536;
const double kPi = 3.14159265358979323846;

/// 对应 C# 测试中的 Cylinder（继承 AlgorithmEngineEx，重写 GetParameterEx /
/// ExecuteDiyFunction）。C++ 中 AlgorithmEngineEx::GetParameter 会调用
/// GetParameterEx，故兜底时直接调用 AlgorithmEngine::GetParameter。
class Cylinder : public AlgorithmEngineEx {
public:
    Cylinder(int radius, int height) : radius_(radius), height_(height) {}

    Operand GetParameterEx(const std::string& parameter) override {
        if (parameter == "半径") {
            return Operand::Create(radius_);
        }
        if (parameter == "直径") {
            return Operand::Create(radius_ * 2);
        }
        if (parameter == "高") {
            return Operand::Create(height_);
        }
        return AlgorithmEngine::GetParameter(parameter);
    }

    Operand ExecuteDiyFunction(const std::string& funcName,
                               const std::vector<Operand>& operands) override {
        if (funcName == "求面积") {
            if (operands.size() == 1) {
                const double r = operands[0].ToNumber().NumberValue().Float64();
                return Operand::Create(r * r * kPi);
            }
        }
        return AlgorithmEngine::ExecuteDiyFunction(funcName, operands);
    }

private:
    int radius_;
    int height_;
};

/// 往返一致性校验：Parse(exp) -> ToString 还原 -> 重新 Parse 求值，
/// 要求还原前后语义完全一致(结果值 + 是否错误)。
void AssertRoundTrip(const std::string& exp) {
    AlgorithmEngine engine1;
    Function function = engine1.Parse(exp);
    std::string text = function->ToString();
    Operand result1 = engine1.Evaluate(function);

    AlgorithmEngine engine2;
    Operand result2;
    try {
        result2 = engine2.Evaluate(engine2.Parse(text));
    } catch (const std::exception& ex) {
        throw std::runtime_error("往返测试失败: [" + exp + "] 还原为 [" + text + "] 后重新求值抛出 " +
                                 typeid(ex).name() + ": " + ex.what());
    }

    if (result1.IsError() != result2.IsError() || result1.ToString() != result2.ToString()) {
        throw std::runtime_error("往返测试失败: [" + exp + "] 还原为 [" + text + "]，原结果=[" +
                                 result1.ToString() + "](IsError=" +
                                 (result1.IsError() ? "true" : "false") + ")，往返结果=[" +
                                 result2.ToString() + "](IsError=" +
                                 (result2.IsError() ? "true" : "false") + ")");
    }
}

TEST(Value, constant_test) {
    AlgorithmEngine engine;
    double e = engine.TryEvaluate("e", 0.0);
    Assert::AreEqual(kE, e, 10);
    e = engine.TryEvaluate("pi", 0.0);
    Assert::AreEqual(kPi, e, 10);

    bool b = engine.TryEvaluate("true", false);
    Assert::AreEqual(true, b);
    b = engine.TryEvaluate("false", true);
    Assert::AreEqual(false, b);
}

TEST(Value, boolean_test) {
    AlgorithmEngine engine;
    int b1 = engine.TryEvaluate("if(true,1,2)", 0);
    Assert::AreEqual(1, b1);

    b1 = engine.TryEvaluate("if(false,1,2)", 0);
    Assert::AreEqual(2, b1);
}

TEST(Value, array_test) {
    AlgorithmEngine engine;
    int r = engine.TryEvaluate("count(Array(1,2,3,4))", 0);
    Assert::AreEqual(4, r);

    r = engine.TryEvaluate("(1=1)*9+2", 0);
    Assert::AreEqual(11, r);
    r = engine.TryEvaluate("(1=2)*9+2", 0);
    Assert::AreEqual(2, r);
}

TEST(Value, TestVersion) {
    AlgorithmEngine engine;
    std::string t25 = engine.TryEvaluate("Engineversion", "");
    Assert::AreEqual(std::string("ToolGood.Algorithm 6.3"), t25);
    std::string t26 = engine.TryEvaluate("Algorithmversion", "");
    Assert::AreEqual(std::string("ToolGood.Algorithm 6.3"), t26);
}

TEST(Value, Test_Json) {
    AlgorithmEngine engine;
    Function t = engine.Parse(
        "{'灰色':'L','canBookCount':905,'saleCount':91,'specId':'43b0e72e98731aed69e1f0cc7d64bf4d'}");
    std::string c = engine.Evaluate(t).ToString();
    Assert::AreEqual(
        std::string(
            "{\"灰色\":\"L\",\"canBookCount\":905,\"saleCount\":91,\"specId\":"
            "\"43b0e72e98731aed69e1f0cc7d64bf4d\"}"),
        c);
}

TEST(Value, Json_large_number_error_test) {
    AlgorithmEngine engine;
    // Bug A 回归: 超出 decimal 范围的超大数解析应报错(修复前静默返回 0)
    engine.TryEvaluate("json('{\"a\":99999999999999999999999999999999999}')['a']", 0.0);
    Assert::IsNotNull(engine.LastError);
}

TEST(Value, Json_string_indexer_no_exception_test) {
    AlgorithmEngine engine;
    // Bug B 回归: 非对象 JSON 值被字符串索引不应抛 NullReferenceException
    engine.TryEvaluate("json('123')['a']", "");
    Assert::IsNotNull(engine.LastError);

    // 不存在的键返回错误而非异常
    engine.TryEvaluate("json('{\"a\":1}')['b']", "");
    Assert::IsNotNull(engine.LastError);
}

TEST(Value, Json_backslash_escape_test) {
    AlgorithmEngine engine;
    // Bug C 回归: 序列化字符串应转义反斜杠(修复前漏转义, 输出为 C:\temp)
    Function t = engine.Parse("{'a':'C:\\\\temp'}");
    std::string c = engine.Evaluate(t).ToString();
    Assert::AreEqual(std::string("{\"a\":\"C:\\\\temp\"}"), c);

    // 反斜杠与引号同时转义
    t = engine.Parse("{'a':'a\\\"b\\\\c'}");
    c = engine.Evaluate(t).ToString();
    Assert::AreEqual(std::string("{\"a\":\"a\\\"b\\\\c\"}"), c);
}

TEST(Value, PARAM_test) {
    Cylinder engine(10, 15);
    int num = engine.TryEvaluate("PARAM('半径')", 0);
    Assert::AreEqual(num, 10);
    num = engine.TryEvaluate("PARAMETER('半径')", 0);
    Assert::AreEqual(num, 10);
    num = engine.TryEvaluate("GETPARAMETER('半径')", 0);
    Assert::AreEqual(num, 10);

    num = engine.TryEvaluate("半径", 0);
    Assert::AreEqual(num, 10);
}

TEST(Value, Error_test) {
    AlgorithmEngine engine;
    std::string num = engine.TryEvaluate("Error('出错了')", "");
    Assert::AreEqual(num, std::string(""));
    Assert::AreEqual(engine.LastError, std::string("出错了"));
}

TEST(Value, Json_test) {
    AlgorithmEngine engine;
    std::string str = engine.TryEvaluate("{name:'toolgood', age:'12',}['name']", "");
    Assert::AreEqual(str, std::string("toolgood"));

    str = engine.TryEvaluate("{name:'toolgood', age:'12',other:{work:'IT'}}['other']['work']", "");
    Assert::AreEqual(str, std::string("IT"));

    str = engine.TryEvaluate("json(\"{'name':'toolgood', 'age':'12','other':{'work':'IT'}}\")['name']",
                             "");
    Assert::AreEqual(str, std::string("toolgood"));

    str = engine.TryEvaluate(
        "json(\"{'name':'toolgood', 'age':'12','other':{'work':'IT'}}\")['other']['work']", "");
    Assert::AreEqual(str, std::string("IT"));

    bool b = engine.TryEvaluate("{name:'toolgood', age:'12',other:{work:'IT'}}.has('age')", false);
    Assert::AreEqual(b, true);
    b = engine.TryEvaluate("{name:'toolgood', age:'12',other:{work:'IT'}}.hasKey('age')", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("{e:'toolgood', pi:'12',other:{work:'IT'}}.hasKey('e')", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate(
        "json(\"{'name':'toolgood', 'age':'12','other':{'work':'IT'}}\").has('age')", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("{name:'toolgood', age:'12',other:{work:'IT'}}.has('work')", true);
    Assert::AreEqual(b, false);

    b = engine.TryEvaluate("{name:'toolgood', age:'12',other:{work:'IT'}}.hasValue('toolgood')",
                           false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate(
        "json(\"{'name':'toolgood', 'age':'12','other':{'work':'IT'}}\").hasValue('toolgood')", false);
    Assert::AreEqual(b, true);
}

TEST(Value, array_test2) {
    AlgorithmEngine engine;
    engine.SetUseExcelIndex(true);
    int num = engine.TryEvaluate("[1,2,3,4,][2]", 0);
    Assert::AreEqual(num, 2);

    std::string str = engine.TryEvaluate("[1,2,3,4,'555'][5]", "");
    Assert::AreEqual(str, std::string("555"));

    bool b = engine.TryEvaluate("[1,2,3,4,].has('1')", false);
    Assert::AreEqual(b, true);
    b = engine.TryEvaluate("['abc','age'].hasKey('age')", false);
    Assert::AreEqual(b, true);
    b = engine.TryEvaluate("['abc','age'].hasValue('age')", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("json(\"['abc','age']\").has('age')", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("json(\"['abc','age']\").hasValue('age')", false);
    Assert::AreEqual(b, true);
}

TEST(Value, Distance_M_Test) {
    AlgorithmEngine engine;
    bool b = engine.TryEvaluate("1=1m", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=10dm", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=100cm", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1000mm", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=0.001km", false);
    Assert::AreEqual(b, true);
}

TEST(Value, Distance_DM_Test) {
    AlgorithmEngine engine;
    engine.DistanceUnit = DistanceUnitType::DM;
    bool b = engine.TryEvaluate("1=0.1m", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1dm", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=10cm", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=100mm", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=0.0001km", false);
    Assert::AreEqual(b, true);
}

TEST(Value, Distance_CM_Test) {
    AlgorithmEngine engine;
    engine.DistanceUnit = DistanceUnitType::CM;
    bool b = engine.TryEvaluate("1=0.01m", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=0.1dm", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1cm", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=10mm", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=0.00001km", false);
    Assert::AreEqual(b, true);
}

TEST(Value, Distance_MM_Test) {
    AlgorithmEngine engine;
    engine.DistanceUnit = DistanceUnitType::MM;
    bool b = engine.TryEvaluate("1=0.001m", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=0.01dm", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=0.1cm", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1mm", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=0.000001km", false);
    Assert::AreEqual(b, true);
}

TEST(Value, Distance_KM_Test) {
    AlgorithmEngine engine;
    engine.DistanceUnit = DistanceUnitType::KM;

    bool b = engine.TryEvaluate("1=1m*1000", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=10dm*1000", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=100cm*1000", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1000mm*1000", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=0.001km*1000", false);
    Assert::AreEqual(b, true);
}

TEST(Value, Area_M2_Test) {
    AlgorithmEngine engine;
    bool b = engine.TryEvaluate("1=1m*1m", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1m2=1m*1m", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1m2=10dm*10dm", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1m2=100cm*100cm", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1m2=1000mm*1000mm", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1m2=0.001km*0.001km", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1m2", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=100dm2", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=10000cm2", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1000000mm2", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=0.000001km2", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1km2=1km*1km", false);
    Assert::AreEqual(b, true);
}

TEST(Value, Area_DM2_Test) {
    AlgorithmEngine engine;
    engine.AreaUnit = AreaUnitType::DM2;

    bool b = engine.TryEvaluate("1=1m2/100", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=100dm2/100", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=10000cm2/100", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1000000mm2/100", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=0.000001km2/100", false);
    Assert::AreEqual(b, true);
}

TEST(Value, Area_CM2_Test) {
    AlgorithmEngine engine;
    engine.AreaUnit = AreaUnitType::CM2;

    bool b = engine.TryEvaluate("1=1m2/100/100", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=100dm2/100/100", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=10000cm2/100/100", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1000000mm2/100/100", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=0.000001km2/100/100", false);
    Assert::AreEqual(b, true);
}

TEST(Value, Area_MM2_Test) {
    AlgorithmEngine engine;
    engine.AreaUnit = AreaUnitType::MM2;

    bool b = engine.TryEvaluate("1=1m2/100/100/100", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=100dm2/100/100/100", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=10000cm2/100/100/100", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1000000mm2/100/100/100", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=0.000001km2/100/100/100", false);
    Assert::AreEqual(b, true);
}

TEST(Value, Area_KM2_Test) {
    AlgorithmEngine engine;
    engine.AreaUnit = AreaUnitType::KM2;

    bool b = engine.TryEvaluate("1=1m2*1000*1000", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=100dm2*1000*1000", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=10000cm2*1000*1000", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1000000mm2*1000*1000", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=0.000001km2*1000*1000", false);
    Assert::AreEqual(b, true);
}

TEST(Value, Volume_M3_Test) {
    AlgorithmEngine engine;
    bool b = engine.TryEvaluate("1=1m*1m*1m", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1m3=1m*1m*1m", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1m3=1m2*1m", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1m3=1000L", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1L=1000ml", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1m3=1000ml*1000", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1m3=10dm*10dm*10dm", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1m3=100cm*100cm*100cm", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1m3=1000mm*1000mm*1000mm", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1m3=0.001km*0.001km*0.001km", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1m3", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1000dm3", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1000000cm3", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1000000000mm3", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1km3/1000/1000/1000", false);
    Assert::AreEqual(b, true);
}

TEST(Value, Volume_DM3_Test) {
    AlgorithmEngine engine;
    engine.VolumeUnit = VolumeUnitType::DM3;
    bool b = engine.TryEvaluate("1=1m3/1000", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1000dm3/1000", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1000000cm3/1000", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1000000000mm3/1000", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1km3/1000/1000/1000/1000", false);
    Assert::AreEqual(b, true);
}

TEST(Value, Volume_CM3_Test) {
    AlgorithmEngine engine;
    engine.VolumeUnit = VolumeUnitType::CM3;
    bool b = engine.TryEvaluate("1=1m3/1000/1000", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1000dm3/1000/1000", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1000000cm3/1000/1000", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1000000000mm3/1000/1000", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1km3/1000/1000/1000/1000/1000", false);
    Assert::AreEqual(b, true);
}

TEST(Value, Volume_MM3_Test) {
    AlgorithmEngine engine;
    engine.VolumeUnit = VolumeUnitType::MM3;
    bool b = engine.TryEvaluate("1=1m3/1000/1000/1000", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1000dm3/1000/1000/1000", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1000000cm3/1000/1000/1000", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1000000000mm3/1000/1000/1000", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1km3/1000/1000/1000/1000/1000/1000", false);
    Assert::AreEqual(b, true);
}

TEST(Value, Volume_KM3_Test) {
    AlgorithmEngine engine;
    engine.VolumeUnit = VolumeUnitType::KM3;
    bool b = engine.TryEvaluate("1=1m3*1000*1000*1000", false);
    Assert::AreEqual(b, true);
    b = engine.TryEvaluate("1=1000dm3*1000*1000*1000", false);
    Assert::AreEqual(b, true);
    b = engine.TryEvaluate("1=1000000cm3*1000*1000*1000", false);
    Assert::AreEqual(b, true);
    b = engine.TryEvaluate("1=1000000000mm3*1000*1000*1000", false);
    Assert::AreEqual(b, true);
    b = engine.TryEvaluate("1=1km3", false);
    Assert::AreEqual(b, true);
}

TEST(Value, Mass_KG_Test) {
    AlgorithmEngine engine;
    bool b = engine.TryEvaluate("1=1kg", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1000g", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=0.001t", false);
    Assert::AreEqual(b, true);
}

TEST(Value, Mass_G_Test) {
    AlgorithmEngine engine;
    engine.MassUnit = MassUnitType::G;

    bool b = engine.TryEvaluate("1=1kg/1000", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1000g/1000", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=0.001t/1000", false);
    Assert::AreEqual(b, true);
}

TEST(Value, Mass_T_Test) {
    AlgorithmEngine engine;
    engine.MassUnit = MassUnitType::T;

    bool b = engine.TryEvaluate("1=1kg*1000", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=1000g*1000", false);
    Assert::AreEqual(b, true);

    b = engine.TryEvaluate("1=0.001t*1000", false);
    Assert::AreEqual(b, true);
}

TEST(Value, Unit_Error_Test) {
    AlgorithmEngine engine;
    bool b = engine.TryEvaluate("1m=1kg", false);
    Assert::AreEqual(b, true);
    b = engine.TryEvaluate("1m=1m2", false);
    Assert::AreEqual(b, true);
    b = engine.TryEvaluate("1m=1m3", false);
    Assert::AreEqual(b, true);
}

TEST(Value, P0_1_number_tostring_invariant_culture_test) {
    // P0-1 回归: ToString 必须使用 InvariantCulture，
    // 否则在 de-DE/fr-FR 等区域下小数点会输出为 ','，导致结果无法被本引擎重新解析
    // （C# 中切换 CurrentCulture 的部分为区域相关逻辑，C++ 无对应机制，故省略）
    AlgorithmEngine engine;
    Assert::AreEqual(std::string("1.5"), engine.Parse("1.5")->ToString());
    Assert::AreEqual(std::string("1.5M"), engine.Parse("1.5M")->ToString());
    Assert::AreEqual(std::string("1234.5 + 0"), engine.Parse("1234.5 + 0")->ToString());

    AlgorithmEngine e2;
    double r = e2.TryEvaluate(engine.Parse("1.5")->ToString(), 0.0);
    Assert::AreEqual(1.5, r, 10);

    AssertRoundTrip("1.5");
    AssertRoundTrip("1.5 + 1");
    AssertRoundTrip("1.5M");
    AssertRoundTrip("1.5M + 1M");
    AssertRoundTrip("1234.5 + 0");
}

TEST(Value, P0_2_arrayjson_error_propagation_test) {
    AlgorithmEngine engine;
    // P0-2 回归: JSON 项内的错误必须向上传播，
    // 修复前错误操作数被静默包裹成 KeyValue，导致 IsError 为 false
    Operand r = engine.Evaluate(engine.Parse("{\"a\":1, \"b\":1/0}"));
    Assert::IsTrue(r.IsError());
    Assert::AreEqual(std::string("Function '/' Div 0 error!"), r.ErrorMsg());

    r = engine.Evaluate(engine.Parse("{\"a\":1/0}"));
    Assert::IsTrue(r.IsError());

    // 正常 JSON 不受影响
    r = engine.Evaluate(engine.Parse("{\"a\":1,\"b\":2}"));
    Assert::IsFalse(r.IsError());
    Assert::AreEqual(std::string("{\"a\":1,\"b\":2}"), r.ToString());

    AssertRoundTrip("{\"a\":1,\"b\":2}");
    AssertRoundTrip("{\"a\":1, \"b\":1/0}");
}

TEST(Value, P0_3_number_out_of_range_parse_test) {
    AlgorithmEngine engine;
    // P0-3 回归: 超出 decimal 范围的纯数字必须在 Parse 阶段抛出明确异常，
    // 而不是在 Evaluate 时抛 KeyNotFoundException(修复前末位数字被误当作单位)
    bool parsed = false;
    try {
        engine.Parse("99999999999999999999999999999");
        parsed = true;
    } catch (const std::exception& ex) {
        Assert::IsTrue(std::string(ex.what()).find("is invalid or out of range") !=
                       std::string::npos);
    }
    Assert::IsFalse(parsed);  // 29 位纯数字应在 Parse 阶段抛异常

    // ParseWithoutError 不抛异常，仅返回空指针
    Assert::IsTrue(engine.ParseWithoutError("99999999999999999999999999999") == nullptr);

    // 边界内数值仍可正常解析求值
    Operand r = engine.Evaluate(engine.Parse("9999999999999999999999999999"));
    Assert::IsFalse(r.IsError());
    Assert::AreEqual(Decimal::FromString("9999999999999999999999999999"), r.NumberValue());
}

TEST(Value, P0_4_number_unit_overflow_test) {
    AlgorithmEngine engine;
    // P0-4 回归: 单位换算放大溢出应返回 #NUM! 错误操作数，而不是抛出 OverflowException
    Operand r = engine.Evaluate(engine.Parse("9999999999999999999999999999KM3"));
    Assert::IsTrue(r.IsError());
    Assert::AreEqual(std::string("Function 'Num' Num error!"), r.ErrorMsg());

    // 溢出后 TryEvaluate 返回默认值，并填充 LastError
    double d = engine.TryEvaluate("9999999999999999999999999999KM3", -1.0);
    Assert::AreEqual(-1.0, d, 10);
    Assert::IsNotNull(engine.LastError);

    // 未溢出的单位换算不受影响
    r = engine.Evaluate(engine.Parse("1KM"));
    Assert::IsFalse(r.IsError());
    Assert::AreEqual(Decimal::FromInt(1000), r.NumberValue());

    // 单位数值同样支持往返
    AssertRoundTrip("1KM");
    AssertRoundTrip("1.5M");
}

TEST(Value, P1_1_arrayjson_key_escape_test) {
    AlgorithmEngine engine;
    // P1-1 回归: 含引号/空格的 key 必须按字符串字面量转义输出。
    // 修复前 ToString 输出 [{a\"b:1}]，再次 Parse 会抛 InvalidCastException
    Function f = engine.Parse("{\"a\\\"b\":1}");
    Assert::AreEqual(std::string("{\"a\\\"b\":1}"), f->ToString());
    AssertRoundTrip("{\"a\\\"b\":1}");

    f = engine.Parse("{\"a b\":1}");
    Assert::AreEqual(std::string("{\"a b\":1}"), f->ToString());
    AssertRoundTrip("{\"a b\":1}");

    // 含换行转义的 key
    AssertRoundTrip("{\"a\\nb\":1}");

    // 单引号字面量同样解转义，输出统一规范化为双引号
    Assert::AreEqual(std::string("{\"a'b\":1}"), engine.Parse("{'a\\'b':1}")->ToString());
}

TEST(Value, P1_2_diyfunction_error_propagation_test) {
    // P1-2 回归: 自定义函数的参数为错误操作数时必须在引擎内被拦截。
    // 修复前错误操作数被直接传入 ExecuteDiyFunction，访问 NumberValue 会抛 NotImplementedException
    Cylinder engine(10, 15);
    Operand r = engine.Evaluate(engine.Parse("求面积(1/0)"));
    Assert::IsTrue(r.IsError());
    Assert::AreEqual(std::string("Function '/' Div 0 error!"), r.ErrorMsg());

    // 溢出后 TryEvaluate 返回默认值，并填充 LastError
    double d = engine.TryEvaluate("求面积(1/0)", -1.0);
    Assert::AreEqual(-1.0, d, 10);
    Assert::IsNotNull(engine.LastError);

    // 正常参数不受影响
    r = engine.Evaluate(engine.Parse("求面积(2)"));
    Assert::IsFalse(r.IsError());
    Assert::IsTrue(std::fabs(r.NumberValue().Float64() - 4 * kPi) < 1e-9);
}

TEST(Value, P1_3_string_unicode_escape_test) {
    AlgorithmEngine engine;
    // P1-3 回归: \uXXXX 十六进制转义必须被解析。
    // 修复前 "\u000b" 被解析成 "u000b"(丢弃 \u 标记与后 4 位)
    Operand r = engine.Evaluate(engine.Parse("\"a\\u0041b\""));
    Assert::AreEqual(std::string("aAb"), r.TextValue());

    // 控制字符往返: 0x0B(\v) / 0x07(\a) 由 Function_ValueText 转义输出，重新解析必须还原
    AssertRoundTrip("\"a\\u000bb\"");
    AssertRoundTrip("\"a\\u0007b\"");

    // 不完整转义按原样保留，不吞字符
    r = engine.Evaluate(engine.Parse("\"a\\ub\""));
    Assert::AreEqual(std::string("aub"), r.TextValue());
}

TEST(Value, Visitors_P1_1_num_unit_split_test) {
    AlgorithmEngine engine;
    // Visitors P1-1 回归: 超出 decimal 范围的指数记法必须与 "1E+30" 一致地在 Parse 阶段失败。
    // 修复前单位切分只判断后缀首字母是否为字母，把 'E' 当作单位首字母切出
    // 数值 1 + 单位 "E29"，报出误导性的"Number unit 'E29' is invalid!"。
    const std::vector<std::string> exps = {"1E29", "12E29", "1e29", "1E+30", "1E300"};
    for (const std::string& exp : exps) {
        bool parsed = false;
        try {
            engine.Parse(exp);
            parsed = true;
        } catch (const std::exception& ex) {
            if (std::string(ex.what()).find("unit") != std::string::npos) {
                Assert::IsTrue(false);  // 报出了单位错误而非数值越界
            }
        }
        Assert::IsFalse(parsed);  // 应在 Parse 阶段抛出异常
        Assert::IsTrue(engine.ParseWithoutError(exp) == nullptr);
    }

    // 范围内的指数记法走数值分支，不受单位切分影响
    Operand r = engine.Evaluate(engine.Parse("1E2"));
    Assert::IsFalse(r.IsError());
    Assert::AreEqual(Decimal::FromInt(100), r.NumberValue());
    r = engine.Evaluate(engine.Parse("1.5e2"));
    Assert::IsFalse(r.IsError());
    Assert::AreEqual(Decimal::FromInt(150), r.NumberValue());

    // 长度 1/2/3 的单位后缀都必须按完整后缀切分(不能只按末 2/3 位)
    Assert::AreEqual(std::string("1KM2"), engine.Parse("1KM2")->ToString());
    Assert::AreEqual(std::string("1M2"), engine.Parse("1M2")->ToString());
    Assert::AreEqual(std::string("1ML"), engine.Parse("1ML")->ToString());
    Assert::AreEqual(std::string("1KG"), engine.Parse("1KG")->ToString());
    Assert::AreEqual(std::string("1T"), engine.Parse("1T")->ToString());
    Assert::AreEqual(std::string("1L"), engine.Parse("1L")->ToString());

    // 单位数值语义不变，大小写单位等价(token 文本保留原样，需按不区分大小写匹配)
    Assert::AreEqual(Decimal::FromInt(1000), engine.Evaluate(engine.Parse("1KM")).NumberValue());
    Assert::AreEqual(Decimal::FromInt(1000), engine.Evaluate(engine.Parse("1km")).NumberValue());
    Assert::AreEqual(Decimal::FromInt(1234), engine.Evaluate(engine.Parse("1234M")).NumberValue());
    Assert::AreEqual(Decimal::FromInt(12), engine.Evaluate(engine.Parse("12M")).NumberValue());
    Assert::AreEqual(Decimal::FromInt(1), engine.Evaluate(engine.Parse("1m")).NumberValue());

    AssertRoundTrip("1KM2");
    AssertRoundTrip("1ML");
    AssertRoundTrip("12M");
    AssertRoundTrip("1E2");
}

TEST(Value, Visitors_P1_2_function_branch_split_test) {
    // Visitors P1-2 回归: VisitFunction_fun 的 PARAMETER(自定义函数) 分支拆分后行为必须不变，
    // 未实现的内建函数 token 不再与 PARAMETER 共用分支而静默降级为自定义函数。
    Cylinder engine(10, 15);
    Operand r = engine.Evaluate(engine.Parse("求面积(2)"));
    Assert::IsFalse(r.IsError());
    Assert::IsTrue(std::fabs(r.NumberValue().Float64() - 4 * kPi) < 1e-9);

    // 自定义函数与内建函数混用时各走各的分支
    r = engine.Evaluate(engine.Parse("求面积(2) + ABS(-1)"));
    Assert::IsFalse(r.IsError());
    Assert::IsTrue(std::fabs(r.NumberValue().Float64() - (4 * kPi + 1)) < 1e-9);

    // 未注册的自定义函数在调用时给出明确错误，而不是静默返回结果
    AlgorithmEngine engine2;
    r = engine2.Evaluate(engine2.Parse("求面积(2)"));
    Assert::IsTrue(r.IsError());
    Assert::AreEqual(std::string("DiyFunction [求面积] is missing."), r.ErrorMsg());

    // 自定义函数名参与往返
    AssertRoundTrip("求面积(2)");
}

}  // namespace
