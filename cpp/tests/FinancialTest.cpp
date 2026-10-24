// 移植自 csharp/ToolGood.Algorithm.Test/AlgorithmEngine/FinancialTest.cs
#include <string>
#include <vector>

#include "cpptest.h"

#include "toolgood/AlgorithmEngine.h"
#include "toolgood/AlgorithmEngineEx.h"

using namespace toolgood;

namespace {

/// 对应 C# Math.Round(double, int)（银行家舍入）
double Round(double v, int digits) {
    return Decimal::FromDouble(v).RoundHalfEven(digits).Float64();
}

TEST(Financial, PMT_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("PMT(0.08/12, 10, 10000)", 0.0);
    Assert::AreEqual(Round(t, 4), Round(-1037.0321, 4));
}

TEST(Financial, PPMT_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("PPMT(0.08/12, 1, 10, 10000)", 0.0);
    Assert::AreEqual(Round(t, 4), Round(-970.3654, 4));
}

TEST(Financial, IPMT_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("IPMT(0.08/12, 1, 10, 10000)", 0.0);
    Assert::AreEqual(Round(t, 4), Round(-66.6667, 4));
}

TEST(Financial, PV_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("PV(0.08/12, 10, -1000)", 0.0);
    Assert::AreEqual(Round(t, 2), Round(9642.90, 2));
}

TEST(Financial, FV_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("FV(0.08/12, 10, -1000)", 0.0);
    Assert::AreEqual(Round(t, 2), Round(10305.40, 2));
}

TEST(Financial, NPER_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("NPER(0.08/12, -1000, 10000)", 0.0);
    Assert::AreEqual(Round(t, 2), Round(10.38, 2));
}

TEST(Financial, RATE_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("RATE(12,-100,400,0,0,0.1)", 0.0);
    Assert::AreEqual(Round(t, 4), Round(0.2289, 4));
}

TEST(Financial, NPV_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("NPV(0.1, -10000, 3000, 4200, 6800)", 0.0);
    Assert::AreEqual(Round(t, 2), Round(1188.44, 2));
}

TEST(Financial, IRR_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("IRR(array(-70000, 12000, 15000, 18000, 21000, 26000))", 0.0);
    Assert::AreEqual(Round(t, 4), Round(0.0866, 4));
}

TEST(Financial, MIRR_test) {
    AlgorithmEngine engine;
    double t =
        engine.TryEvaluate("MIRR(array(-70000, 12000, 15000, 18000, 21000, 26000), 0.1, 0.12)", 0.0);
    Assert::AreEqual(Round(t, 4), Round(0.0987, 4));
}

TEST(Financial, SLN_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("SLN(30000, 7500, 10)", 0.0);
    Assert::AreEqual(t, 2250.0);
}

TEST(Financial, SYD_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("SYD(30000, 7500, 10, 1)", 0.0);
    Assert::AreEqual(Round(t, 2), Round(4090.91, 2));
}

TEST(Financial, DDB_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("DDB(2400, 300, 10, 2)", 0.0);
    Assert::AreEqual(Round(t, 2), Round(384.0, 2));
}

TEST(Financial, DB_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("DB(1000000, 100000, 6, 1)", 0.0);
    Assert::AreEqual(Round(t, 2), Round(319000.0, 2));
}

TEST(Financial, DB_month_test) {
    AlgorithmEngine engine;
    // Excel: month<12 时折旧跨越 life+1 个期间(第 1 年部分月 + life-1 个整年 + 最后部分月)
    // 修复前 period>life 直接报错, 现在允许 period=life+1
    double t = engine.TryEvaluate("DB(1000000, 100000, 6, 7, 7)", 0.0);
    Assert::IsTrue(engine.LastError.empty() && t > 0);

    // 超过总期间数仍报错
    t = engine.TryEvaluate("DB(1000000, 100000, 6, 8, 7)", 0.0);
    Assert::IsNotNull(engine.LastError);

    // month=12 时总期间数等于 life, 最后一年折旧为整年
    t = engine.TryEvaluate("DB(1000000, 100000, 6, 6, 12)", 0.0);
    Assert::IsTrue(engine.LastError.empty() && t > 0);

    // period=1 首期按部分月折旧
    t = engine.TryEvaluate("DB(1000000, 100000, 6, 1, 7)", 0.0);
    Assert::IsTrue(engine.LastError.empty() && t > 0);
}

TEST(Financial, XNPV_test) {
    AlgorithmEngineEx engine;
    engine.AddParameter("values", std::vector<double>{-10000, 2750, 4250, 3250, 2750});
    engine.AddParameter("dates2", std::vector<std::string>{"2008-1-1", "2008-3-1", "2008-10-30",
                                                           "2009-2-15", "2009-4-1"});
    double t = engine.TryEvaluate("XNPV(0.09, values, dates2)", 0.0);
    Assert::AreEqual(Round(t, 2), Round(2086.65, 2));
}

TEST(Financial, XIRR_test) {
    AlgorithmEngineEx engine;
    engine.AddParameter("values", std::vector<double>{-10000, 2750, 4250, 3250, 2750});
    engine.AddParameter("dates2", std::vector<std::string>{"2008-1-1", "2008-3-1", "2008-10-30",
                                                           "2009-2-15", "2009-4-1"});
    double t = engine.TryEvaluate("XIRR(values, dates2)", 0.0);
    Assert::AreEqual(Round(t, 4), Round(0.3734, 4));
}

TEST(Financial, IPMT_PPMT_per_out_of_range_test) {
    AlgorithmEngine engine;
    // per < 1 越界, 应报错
    double t = engine.TryEvaluate("IPMT(0.08/12, 0, 10, 10000)", 0.0);
    Assert::IsNotNull(engine.LastError);

    // per > nper 越界, 应报错
    t = engine.TryEvaluate("PPMT(0.08/12, 11, 10, 10000)", 0.0);
    (void)t;
    Assert::IsNotNull(engine.LastError);
}

TEST(Financial, NPV_parameter_test) {
    AlgorithmEngine engine;
    // 仅 rate、无 value, 参数校验拒绝
    double t = engine.TryEvaluate("NPV(0.1)", 0.0);
    (void)t;
    Assert::IsNotNull(engine.LastError);
}

TEST(Financial, MIRR_no_positive_test) {
    AlgorithmEngine engine;
    // 全负现金流、无正现金流, 应返回 Div0 错误
    double t = engine.TryEvaluate("MIRR(array(-70000, -12000, -15000), 0.1, 0.12)", 0.0);
    (void)t;
    Assert::IsNotNull(engine.LastError);
}

TEST(Financial, PV_type_test) {
    AlgorithmEngine engine;
    // type=0(期末) 与 type=1(期初) 的关系: PV(type=1) = PV(type=0) * (1+rate)
    double pv0 = engine.TryEvaluate("PV(0.08/12, 10, -1000)", 0.0);
    double pv1 = engine.TryEvaluate("PV(0.08/12, 10, -1000, 0, 1)", 0.0);
    Assert::AreEqual(Round(pv1, 4), Round(pv0 * (1 + 0.08 / 12), 4));
}

}  // namespace
