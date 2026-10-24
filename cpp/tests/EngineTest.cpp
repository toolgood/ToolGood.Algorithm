// 移植自 csharp/ToolGood.Algorithm.Test/AlgorithmEngine/EngineTest.cs
//       与 csharp/ToolGood.Algorithm.Test/AlgorithmEngine/Cylinder.cs（自定义引擎）
//
// 类型对应：
//   Cylinder : AlgorithmEngineEx          -> class Cylinder : public AlgorithmEngineEx
//   GetParameterEx(string)                -> GetParameterEx(const std::string&) override
//   ExecuteDiyFunction(string, List<Operand>)
//                                         -> ExecuteDiyFunction(const std::string&,
//                                                               const std::vector<Operand>&) override
//   (double)operands[0].ToNumber().NumberValue -> .NumberValue().Float64()
//   Math.PI                                -> 3.14159265358979323846
#include "cpptest.h"

#include <string>
#include <vector>

#include "toolgood/AlgorithmEngineEx.h"

using namespace toolgood;

namespace {

/// 对应 C# 的 Cylinder : AlgorithmEngineEx（半径/直径/高 参数 + 求面积 DIY 函数）
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
        return AlgorithmEngineEx::GetParameter(parameter);
    }

    Operand ExecuteDiyFunction(const std::string& funcName,
                               const std::vector<Operand>& operands) override {
        if (funcName == "求面积") {
            if (operands.size() == 1) {
                const double r = operands[0].ToNumber().NumberValue().Float64();
                return Operand::Create(r * r * 3.14159265358979323846);
            }
        }
        return AlgorithmEngineEx::ExecuteDiyFunction(funcName, operands);
    }

private:
    int radius_;
    int height_;
};

TEST(Engine, base_test2) {
    AlgorithmEngine engine;
    std::string t = engine.TryEvaluate("1+(3*2+2)/2 & '11' & '11:20'*9 & isnumber(22)*3", "");
    (void)t;
}

TEST(Engine, base_test3) {
    AlgorithmEngine engine;

    int c = engine.TryEvaluate("(2)+/*123456*/3", 0);
    Assert::AreEqual(5, c);

    c = engine.TryEvaluate("2+3//eee", 0);
    Assert::AreEqual(5, c);

    c = engine.TryEvaluate("(2)+/*123456*/3 ee22+22", 0);
    Assert::AreEqual(0, c);
}

TEST(Engine, base_test4) {
    AlgorithmEngine engine;

    std::string c = engine.TryEvaluate("'4dd'&'55' rr", "");
    Assert::AreEqual(std::string(""), c);
}

TEST(Engine, base_test5) {
    AlgorithmEngine engine;

    std::string c = engine.TryEvaluate("'4dd'&'55'.left(1)", "");
    Assert::AreEqual(std::string("4dd5"), c);
}

TEST(Engine, Cylinder_Test) {
    Cylinder c(3, 10);
    double t2 = c.TryEvaluate("半径*半径*pi()", 0.0);
    (void)t2;

    double t = c.TryEvaluate("直径*pi()", 0.0);
    t = c.TryEvaluate("半径*半径*pi()*高", 0.0);

    t = c.TryEvaluate("'半径'*半径*pi()*高", 0.0);

    t = c.TryEvaluate("求面积（10）", 0.0);
    Assert::AreEqual(10 * 10 * 3.14159265358979323846, t, 10.0);

    std::string json =
        "{'灰色':'L','canBookCount':905,'saleCount':91,'specId':'43b0e72e98731aed69e1f0cc7d64bf4d'}";
    c.AddParameterFromJson(json);

    std::string tt = c.TryEvaluate("灰色", "");
    Assert::AreEqual(std::string("L"), tt);
}

}  // namespace
