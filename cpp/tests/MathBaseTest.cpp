// 移植自 csharp/ToolGood.Algorithm.Test/AlgorithmEngine/MathBaseTest.cs
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

TEST(MathBase, Pi_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("pi()", 0.0);
    Assert::AreEqual(3.141592654, cs_round(t, 9));
}

TEST(MathBase, abs_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("abs(-1.2)", 0.0);
    Assert::AreEqual(1.2, t);
}

TEST(MathBase, QUOTIENT_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("QUOTIENT(7,3)", 0.0);
    Assert::AreEqual(2.0, t);
}

TEST(MathBase, MOD_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("MOD(7,3)", 0.0);
    Assert::AreEqual(1.0, t);
}

TEST(MathBase, SIGN_test) {
    AlgorithmEngine engine;
    int t = engine.TryEvaluate("SIGN(0)", 0);
    Assert::AreEqual(0, t);
    t = engine.TryEvaluate("SIGN(9)", 0);
    Assert::AreEqual(1, t);
    t = engine.TryEvaluate("SIGN(-9)", 0);
    Assert::AreEqual(-1, t);
}

TEST(MathBase, SQRT_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("SQRT(9)", 0.0);
    Assert::AreEqual(3.0, t);
}

TEST(MathBase, SUM_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("SUM(1,2,3,4)", 0.0);
    Assert::AreEqual(10.0, t);
}

TEST(MathBase, TRUNC_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("TRUNC(9.222)", 0.0);
    Assert::AreEqual(9.0, t);
    t = engine.TryEvaluate("TRUNC(-9.222)", 0.0);
    Assert::AreEqual(-9.0, t);
}

TEST(MathBase, TRUNC_ExcelCompatible_test) {
    AlgorithmEngine engine;

    double t = engine.TryEvaluate("TRUNC(8.9)", 0.0);
    Assert::AreEqual(8.0, t);

    t = engine.TryEvaluate("TRUNC(8.912, 2)", 0.0);
    Assert::AreEqual(8.91, t);

    t = engine.TryEvaluate("TRUNC(8.912, 1)", 0.0);
    Assert::AreEqual(8.9, t);

    t = engine.TryEvaluate("TRUNC(-5.5)", 0.0);
    Assert::AreEqual(-5.0, t);

    t = engine.TryEvaluate("TRUNC(123.456, -2)", 0.0);
    Assert::AreEqual(100.0, t);

    t = engine.TryEvaluate("TRUNC(123.456, -1)", 0.0);
    Assert::AreEqual(120.0, t);

    t = engine.TryEvaluate("TRUNC(-123.456, -2)", 0.0);
    Assert::AreEqual(-100.0, t);

    t = engine.TryEvaluate("TRUNC(0)", 0.0);
    Assert::AreEqual(0.0, t);

    t = engine.TryEvaluate("TRUNC(PI(), 4)", 0.0);
    Assert::AreEqual(3.1415, cs_round(t, 4));
}

TEST(MathBase, int_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("int(9.222)", 0.0);
    Assert::AreEqual(9.0, t);
    t = engine.TryEvaluate("int(-9.222)", 0.0);
    Assert::AreEqual(-10.0, t);
}

TEST(MathBase, GCD_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("GCD(3,5,7)", 0.0);
    Assert::AreEqual(1.0, t);
    t = engine.TryEvaluate("GCD(30,21)", 0.0);
    Assert::AreEqual(3.0, t);
}

TEST(MathBase, LCM_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("LCM(3,5,7)", 0.0);
    Assert::AreEqual(105.0, t);
}

TEST(MathBase, combin_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("combin(10,2)", 0.0);
    Assert::AreEqual(45.0, t);
}

TEST(MathBase, PERMUT_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("PERMUT(10,2)", 0.0);
    Assert::AreEqual(90.0, t);
}

// ---------- 四舍五入 ----------

TEST(MathBase, ROUND_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("ROUND(4.333,2)", 0.0);
    Assert::AreEqual(4.33, t);
}

TEST(MathBase, ROUND_single_param_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("ROUND(4.5)", 0.0);
    Assert::AreEqual(5.0, t);

    t = engine.TryEvaluate("ROUND(4.4)", 0.0);
    Assert::AreEqual(4.0, t);

    t = engine.TryEvaluate("ROUND(-4.5)", 0.0);
    Assert::AreEqual(-5.0, t);
}

TEST(MathBase, ROUND_negative_digits_test) {
    AlgorithmEngine engine;
    // 负数位数向左取整(Math.Round(decimal,int) 不支持负数位数,修复前抛 ArgumentOutOfRangeException)
    double t = engine.TryEvaluate("ROUND(1234.567,-1)", 0.0);
    Assert::AreEqual(1230.0, t, 1e-6);

    t = engine.TryEvaluate("ROUND(1234.567,-2)", 0.0);
    Assert::AreEqual(1200.0, t, 1e-6);

    t = engine.TryEvaluate("ROUND(1234.567,-3)", 0.0);
    Assert::AreEqual(1000.0, t, 1e-6);

    t = engine.TryEvaluate("ROUND(1234.567,-4)", 0.0);
    Assert::AreEqual(0.0, t, 1e-6);

    // AwayFromZero 舍入
    t = engine.TryEvaluate("ROUND(15,-1)", 0.0);
    Assert::AreEqual(20.0, t, 1e-6);

    // 超出 -15 报错
    t = engine.TryEvaluate("ROUND(1234.567,-16)", 0.0);
    Assert::IsTrue(!engine.LastError.empty());
    (void)t;
}

TEST(MathBase, FACTDOUBLE_limit_test) {
    AlgorithmEngine engine;
    // 基础值
    double t = engine.TryEvaluate("FACTDOUBLE(6)", 0.0);
    Assert::AreEqual(48.0, t, 1e-9);
    t = engine.TryEvaluate("FACTDOUBLE(7)", 0.0);
    Assert::AreEqual(105.0, t, 1e-9);

    // 修复前 29~45 被误判溢出,现在 decimal 范围内正常(45!!≈2.537e28)
    t = engine.TryEvaluate("FACTDOUBLE(29)", 0.0);
    Assert::IsTrue(engine.LastError.empty() && t > 0);

    t = engine.TryEvaluate("FACTDOUBLE(45)", 0.0);
    Assert::IsTrue(engine.LastError.empty() && t > 0);

    // 46!! 超出 decimal 溢出,报错
    t = engine.TryEvaluate("FACTDOUBLE(46)", 0.0);
    Assert::IsTrue(!engine.LastError.empty());

    // 负数报错
    t = engine.TryEvaluate("FACTDOUBLE(-1)", 0.0);
    Assert::IsTrue(!engine.LastError.empty());
    (void)t;
}

TEST(MathBase, ROUNDDOWN_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("ROUNDDOWN(4.333,2)", 0.0);
    Assert::AreEqual(4.33, t);

    t = engine.TryEvaluate("ROUNDDOWN(-3.141592, 3)", 0.0);
    Assert::AreEqual(-3.141, t);
}

TEST(MathBase, ROUNDUP_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("ROUNDUP(4.333,2)", 0.0);
    Assert::AreEqual(4.34, t);

    t = engine.TryEvaluate("ROUNDUP(-3.141592, 3)", 0.0);
    Assert::AreEqual(-3.142, t);
}

TEST(MathBase, CEILING_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("CEILING(4.333,0.1)", 0.0);
    Assert::AreEqual(4.4, t);

    t = engine.TryEvaluate("CEILING(4.333)", 0.0);
    Assert::AreEqual(5, t);
}

TEST(MathBase, FLOOR_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("FLOOR(4.363,0.1)", 0.0);
    Assert::AreEqual(4.3, t);

    t = engine.TryEvaluate("FLOOR(4.333)", 0.0);
    Assert::AreEqual(4, t);
}

TEST(MathBase, even_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("even(4.363)", 0.0);
    Assert::AreEqual(6.0, t);
}

TEST(MathBase, odd_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("odd(4.363)", 0.0);
    Assert::AreEqual(5, t);
}

TEST(MathBase, MROUND_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("MROUND(4.363,2)", 0.0);
    Assert::AreEqual(4, t);
    t = engine.TryEvaluate("MROUND(5.363,2)", 0.0);
    Assert::AreEqual(6, t);
}

// ---------- 随机数 ----------

TEST(MathBase, Rand_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("RAND()", 0.0);
    Assert::GreaterOrEqual(t, 0);
    Assert::Less(t, 1);
}

TEST(MathBase, RANDBETWEEN_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("RANDBETWEEN(2,99)", 0.0);
    Assert::GreaterOrEqual(t, 2);
    Assert::LessOrEqual(t, 99);
}

// ---------- 幂/对数/阶乘 ----------

TEST(MathBase, fact_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("fact(6)", 0.0);
    Assert::AreEqual(720.0, t);
    t = engine.TryEvaluate("fact(3)", 0.0);
    Assert::AreEqual(6.0, t);
}

TEST(MathBase, factdouble_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("factdouble(10)", 0.0);
    Assert::AreEqual(3840.0, t);
}

TEST(MathBase, POWER_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("POWER(10,2)", 0.0);
    Assert::AreEqual(100.0, t);
}

TEST(MathBase, exp_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("exp(2)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(7.389056099, 6), t);
}

TEST(MathBase, ln_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("ln(4)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(1.386294361, 6), t);
}

TEST(MathBase, log_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("log(10)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(1.0, 6), t);

    t = engine.TryEvaluate("log(8,2)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(3.0, 6), t);
}

TEST(MathBase, log10_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("log10(10)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(1.0, 6), t);
}

TEST(MathBase, MULTINOMIAL_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("MULTINOMIAL(1,2,3)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(60.0, 6), t);
    t = engine.TryEvaluate("MULTINOMIAL(1,2,3,4)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(12600.0, 6), t);
    t = engine.TryEvaluate("MULTINOMIAL(1,2,3,4.1)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(12600.0, 6), t);
}

TEST(MathBase, PRODUCT_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("PRODUCT(1,2,3,4)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(24.0, 6), t);
}

TEST(MathBase, SQRTPI_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("SQRTPI(3)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(3.069980124, 6), t);
}

TEST(MathBase, SUMSQ_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("SUMSQ(1,2)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(cs_round(5.0, 6), t);
}

// ---------- 工程函数 ----------

TEST(MathBase, ERF_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("ERF(0)", 0.0);
    Assert::AreEqual(0.0, cs_round(t, 6));

    t = engine.TryEvaluate("ERF(1)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(0.842701, t);

    t = engine.TryEvaluate("ERF(-1)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(-0.842701, t);

    t = engine.TryEvaluate("ERF(0.5)", 0.0);
    Assert::AreEqual(0.520499878, t, 6.0);

    t = engine.TryEvaluate("ERF(4)", 0.0);
    Assert::AreEqual(0.999999985, t, 6.0);

    t = engine.TryEvaluate("ERF(5)", 0.0);
    Assert::AreEqual(1.0, t, 6.0);

    t = engine.TryEvaluate("ERF(7)", 0.0);
    Assert::AreEqual(1.0, t, 6.0);
}

TEST(MathBase, ERFC_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("ERFC(0)", 0.0);
    Assert::AreEqual(1.0, cs_round(t, 6));

    t = engine.TryEvaluate("ERFC(1)", 0.0);
    t = cs_round(t, 6);
    Assert::AreEqual(0.157299, t);

    t = engine.TryEvaluate("ERFC(-3)", 0.0);
    Assert::AreEqual(1.99997791, t, 6.0);

    t = engine.TryEvaluate("ERFC(1.5)", 0.0);
    Assert::AreEqual(0.033894854, t, 6.0);
}

TEST(MathBase, DELTA_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("DELTA(5, 5)", 0.0);
    Assert::AreEqual(1.0, t);

    t = engine.TryEvaluate("DELTA(5, 4)", 0.0);
    Assert::AreEqual(0.0, t);

    t = engine.TryEvaluate("DELTA(5)", 0.0);
    Assert::AreEqual(0.0, t);

    t = engine.TryEvaluate("DELTA(0)", 0.0);
    Assert::AreEqual(1.0, t);
}

TEST(MathBase, GESTEP_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("GESTEP(5, 4)", 0.0);
    Assert::AreEqual(1.0, t);

    t = engine.TryEvaluate("GESTEP(5, 5)", 0.0);
    Assert::AreEqual(1.0, t);

    t = engine.TryEvaluate("GESTEP(5, 6)", 0.0);
    Assert::AreEqual(0.0, t);

    t = engine.TryEvaluate("GESTEP(5)", 0.0);
    Assert::AreEqual(1.0, t);

    t = engine.TryEvaluate("GESTEP(-1)", 0.0);
    Assert::AreEqual(0.0, t);
}

// ---------- 百分比 ----------

TEST(MathBase, Percentage_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("50%", 0.0);
    Assert::AreEqual(0.5, t);

    t = engine.TryEvaluate("100%", 0.0);
    Assert::AreEqual(1.0, t);

    t = engine.TryEvaluate("1%", 0.0);
    Assert::AreEqual(0.01, t);

    t = engine.TryEvaluate("0%", 0.0);
    Assert::AreEqual(0.0, t);

    t = engine.TryEvaluate("200%", 0.0);
    Assert::AreEqual(2.0, t);

    t = engine.TryEvaluate("25.5%", 0.0);
    Assert::AreEqual(0.255, t);
}

// ---------- 边界值测试 ----------

TEST(MathBase, DivisionByZero_Test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("1/0", 0.0);
    Assert::AreEqual(t, 0);

    t = engine.TryEvaluate("0/0", 0.0);
    Assert::AreEqual(t, 0);
}

TEST(MathBase, Overflow_Test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("99999999999999999999*99999999999999999999", 0.0);
    Assert::AreEqual(t, 0);
}

TEST(MathBase, NullOperation_Test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("null+1", 0.0);
    Assert::AreEqual(t, 0);

    t = engine.TryEvaluate("null*100", 0.0);
    Assert::AreEqual(t, 0);

    t = engine.TryEvaluate("null-null", 0.0);
    Assert::AreEqual(t, 0);
}

TEST(MathBase, SquareRootNegative_Test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("sqrt(-1)", 0.0);
    Assert::AreEqual(t, 0);
}

TEST(MathBase, LogNegative_Test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("log(-1)", 0.0);
    Assert::AreEqual(t, 0);

    t = engine.TryEvaluate("ln(0)", 0.0);
    Assert::AreEqual(t, 0);
}

// ---------- Excel兼容性测试 ----------

TEST(MathBase, ODD_ExcelCompatible_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("ODD(1)", 0.0);
    Assert::AreEqual(1.0, t);

    t = engine.TryEvaluate("ODD(2)", 0.0);
    Assert::AreEqual(3.0, t);

    t = engine.TryEvaluate("ODD(3)", 0.0);
    Assert::AreEqual(3.0, t);

    t = engine.TryEvaluate("ODD(-1)", 0.0);
    Assert::AreEqual(-1.0, t);

    t = engine.TryEvaluate("ODD(-2)", 0.0);
    Assert::AreEqual(-3.0, t);

    t = engine.TryEvaluate("ODD(-3)", 0.0);
    Assert::AreEqual(-3.0, t);

    t = engine.TryEvaluate("ODD(-4)", 0.0);
    Assert::AreEqual(-5.0, t);

    t = engine.TryEvaluate("ODD(1.5)", 0.0);
    Assert::AreEqual(3.0, t);

    t = engine.TryEvaluate("ODD(-1.5)", 0.0);
    Assert::AreEqual(-3.0, t);
}

TEST(MathBase, EVEN_ExcelCompatible_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("EVEN(2)", 0.0);
    Assert::AreEqual(2.0, t);

    t = engine.TryEvaluate("EVEN(1)", 0.0);
    Assert::AreEqual(2.0, t);

    t = engine.TryEvaluate("EVEN(-1)", 0.0);
    Assert::AreEqual(-2.0, t);

    t = engine.TryEvaluate("EVEN(-2)", 0.0);
    Assert::AreEqual(-2.0, t);

    t = engine.TryEvaluate("EVEN(-3)", 0.0);
    Assert::AreEqual(-4.0, t);

    t = engine.TryEvaluate("EVEN(1.5)", 0.0);
    Assert::AreEqual(2.0, t);

    t = engine.TryEvaluate("EVEN(-1.5)", 0.0);
    Assert::AreEqual(-2.0, t);

    t = engine.TryEvaluate("EVEN(0)", 0.0);
    Assert::AreEqual(0.0, t);
}

TEST(MathBase, CEILING_ExcelCompatible_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("CEILING(2.5, 1)", 0.0);
    Assert::AreEqual(3.0, t);

    t = engine.TryEvaluate("CEILING(-2.5, -1)", 0.0);
    Assert::AreEqual(-3.0, t);

    t = engine.TryEvaluate("CEILING(-2.5, 1)", 0.0);
    Assert::AreEqual(-2.0, t);

    t = engine.TryEvaluate("CEILING(1.5, 0.1)", 0.0);
    Assert::AreEqual(1.5, t);

    t = engine.TryEvaluate("CEILING(0.234, 0.01)", 0.0);
    Assert::AreEqual(0.24, t);
}

TEST(MathBase, FLOOR_ExcelCompatible_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("FLOOR(3.7, 1)", 0.0);
    Assert::AreEqual(3.0, t);

    t = engine.TryEvaluate("FLOOR(-3.7, -1)", 0.0);
    Assert::AreEqual(-3.0, t);

    t = engine.TryEvaluate("FLOOR(-3.7, 1)", 0.0);
    Assert::AreEqual(-4.0, t);

    t = engine.TryEvaluate("FLOOR(3.7, 2)", 0.0);
    Assert::AreEqual(2.0, t);

    t = engine.TryEvaluate("FLOOR(0, 1)", 0.0);
    Assert::AreEqual(0.0, t);
}

TEST(MathBase, MROUND_ExcelCompatible_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("MROUND(10, 3)", 0.0);
    Assert::AreEqual(9.0, t);

    t = engine.TryEvaluate("MROUND(-10, -3)", 0.0);
    Assert::AreEqual(-9.0, t);

    t = engine.TryEvaluate("MROUND(1.3, 0.2)", 0.0);
    Assert::AreEqual(1.4, t);

    t = engine.TryEvaluate("MROUND(-1.3, -0.2)", 0.0);
    Assert::AreEqual(-1.4, t);
}

TEST(MathBase, ROUNDDOWN_ExcelCompatible_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("ROUNDDOWN(3.7, 0)", 0.0);
    Assert::AreEqual(3.0, t);

    t = engine.TryEvaluate("ROUNDDOWN(-3.7, 0)", 0.0);
    Assert::AreEqual(-3.0, t);

    t = engine.TryEvaluate("ROUNDDOWN(3.777, 2)", 0.0);
    Assert::AreEqual(3.77, t);

    t = engine.TryEvaluate("ROUNDDOWN(-3.777, 2)", 0.0);
    Assert::AreEqual(-3.77, t);

    t = engine.TryEvaluate("ROUNDDOWN(76.9, -1)", 0.0);
    Assert::AreEqual(70.0, t);

    t = engine.TryEvaluate("ROUNDDOWN(-76.9, -1)", 0.0);
    Assert::AreEqual(-70.0, t);
}

}  // namespace
