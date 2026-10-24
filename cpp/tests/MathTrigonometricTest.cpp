// 移植自 csharp/ToolGood.Algorithm.Test/AlgorithmEngine/MathTrigonometricTest.cs
#include "cpptest.h"

#include <cmath>

#include "toolgood/AlgorithmEngine.h"

using namespace toolgood;

namespace {

/// 对应 C# Math.Round(double, int)：先 scale，再银行家舍入，再 scale 回来。
double cs_round(double value, int digits) {
    if (digits >= 0) {
        double p = 1.0;
        for (int i = 0; i < digits; i++) p *= 10.0;
        return std::nearbyint(value * p) / p;
    }
    double p = 1.0;
    for (int i = 0; i < -digits; i++) p *= 10.0;
    return std::nearbyint(value / p) * p;
}

/// C# Math.PI 的双精度值
const double kPi = 3.14159265358979311600;

TEST(MathTrig, degrees_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("degrees(pi())", 0.0);
    Assert::AreEqual(180.0, t);
}

TEST(MathTrig, RADIANS_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("RADIANS(180)", 0.0);
    Assert::AreEqual(kPi, t, 10.0);
}

TEST(MathTrig, cos_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("cos(1)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(0.540302306, 6), t);
}

TEST(MathTrig, cosh_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("cosh(1)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(1.543080635, 6), t);
}

TEST(MathTrig, sin_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("sin(1)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(0.841470985, 6), t);
}

TEST(MathTrig, sinh_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("sinh(1)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(1.175201194, 6), t);
}

TEST(MathTrig, tan_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("tan(1)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(1.557407725, 6), t);
}

TEST(MathTrig, tanh_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("tanh(1)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(0.761594156, 6), t);
}

TEST(MathTrig, acos_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("acos(0.5)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(1.047197551, 6), t);
}

TEST(MathTrig, acosh_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("acosh(1.5)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(0.96242365, 6), t);
}

TEST(MathTrig, asin_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("asin(0.5)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(0.523598776, 6), t);
}

TEST(MathTrig, asinh_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("asinh(1.5)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(1.194763217, 6), t);
}

TEST(MathTrig, atan_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("atan(1)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(0.785398163, 6), t);
}

TEST(MathTrig, atanh_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("atanh(0.5)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(0.549306144, 6), t);
}

TEST(MathTrig, atan2_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("atan2(1,2)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(1.107148718, 6), t);
}

TEST(MathTrig, cot_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("cot(1)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(0.642092616, 6), t);
}

TEST(MathTrig, coth_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("coth(1)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(1.313035285, 6), t);
}

TEST(MathTrig, csc_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("csc(1)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(1.188395106, 6), t);
}

TEST(MathTrig, csch_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("csch(1)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(0.850918128, 6), t);
}

TEST(MathTrig, sec_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("sec(1)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(1.850815718, 6), t);
}

TEST(MathTrig, sech_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("sech(1)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(0.648054274, 6), t);
}

TEST(MathTrig, acot_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("acot(1)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(0.785398163, 6), t);
}

TEST(MathTrig, acoth_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("acoth(2)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(0.549306144, 6), t);
}

TEST(MathTrig, hyperbolic_overflow_test) {
    AlgorithmEngine engine;

    // tanh/coth 大参数趋近 ±1
    Assert::AreEqual(1.0, engine.TryEvaluate("tanh(1000)", 0.0));
    Assert::AreEqual(-1.0, engine.TryEvaluate("tanh(-1000)", 0.0));
    Assert::AreEqual(1.0, engine.TryEvaluate("coth(1000)", 0.0));
    Assert::AreEqual(-1.0, engine.TryEvaluate("coth(-1000)", 0.0));

    // sech/csch 大参数趋近 0
    Assert::AreEqual(0.0, engine.TryEvaluate("sech(1000)", 1.0));
    Assert::AreEqual(0.0, engine.TryEvaluate("csch(1000)", 1.0));

    // sinh/cosh 大参数超出 decimal 范围,返回错误
    engine.TryEvaluate("sinh(1000)", 0.0);
    Assert::IsTrue(!engine.LastError.empty());

    AlgorithmEngine engine2;
    engine2.TryEvaluate("cosh(1000)", 0.0);
    Assert::IsTrue(!engine2.LastError.empty());
}

}  // namespace
