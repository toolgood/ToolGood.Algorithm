// 移植自 csharp/ToolGood.Algorithm.Test/AlgorithmEngine/MathSumTest.cs
#include "cpptest.h"

#include "toolgood/AlgorithmEngine.h"

using namespace toolgood;

namespace {

/// 对应 C# Math.Round(double, int)（银行家舍入）
double Round(double v, int digits) {
    return Decimal::FromDouble(v).RoundHalfEven(digits).Float64();
}

TEST(MathSum, MAX_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("max(1,2,3,4,2,2,1,4)", 0.0);
    Assert::AreEqual(t, 4.0);
}

TEST(MathSum, MEDIAN_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("MEDIAN(1,2,3,4,2,2,1,4)", 0.0);
    Assert::AreEqual(t, 2.0);

    t = engine.TryEvaluate("MEDIAN(1,1,2,2)", 0.0);
    Assert::AreEqual(t, 1.5);
}

TEST(MathSum, MIN_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("MIN(1,2,3,4,2,2,1,4)", 0.0);
    Assert::AreEqual(t, 1.0);
}

TEST(MathSum, QUARTILE_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),0)", 0.0);
    Assert::AreEqual(t, 1.0);
    t = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),1)", 0.0);
    Assert::AreEqual(t, 1.75);
    t = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),2)", 0.0);
    Assert::AreEqual(t, 2.0);
    t = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),3)", 0.0);
    Assert::AreEqual(t, 3.25);
    t = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),4)", 0.0);
    Assert::AreEqual(t, 4.0);
}

TEST(MathSum, MODE_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("MODE(1,2,3,4,2,2,1,4)", 0.0);
    Assert::AreEqual(t, 2.0);
}

TEST(MathSum, PERCENTILE_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("PERCENTILE(array(1,2,3,4,2,2,1,4)，0.4)", 0.0);
    Assert::AreEqual(t, 2.0);
}

TEST(MathSum, PERCENTRANK_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("PERCENTRANK(array(1,2,3,4,2,2,1,4)，3)", 0.0);
    Assert::AreEqual(t, 0.714);
}

TEST(MathSum, PERCENTRANK_significance_test) {
    AlgorithmEngine engine;
    // significance 指定保留小数位数(原始值 5/7≈0.7142857)
    double t = engine.TryEvaluate("PERCENTRANK(array(1,2,3,4,2,2,1,4),3,4)", 0.0);
    Assert::AreEqual(0.7143, t, 1e-9);

    // 28 为允许的最大位数
    t = engine.TryEvaluate("PERCENTRANK(array(1,2,3,4,2,2,1,4),3,28)", 0.0);
    Assert::AreEqual(5.0 / 7.0, t, 1e-12);

    // 超过 28 报错(Math.Round(decimal,int) 上限,修复前会抛 ArgumentOutOfRangeException)
    t = engine.TryEvaluate("PERCENTRANK(array(1,2,3,4,2,2,1,4),3,29)", 0.0);
    Assert::IsNotNull(engine.LastError);

    // 负数位数报错
    t = engine.TryEvaluate("PERCENTRANK(array(1,2,3,4,2,2,1,4),3,-1)", 0.0);
    Assert::IsNotNull(engine.LastError);
}

TEST(MathSum, AVERAGE_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("AVERAGE(1,2,3,4,2,2,1,4)", 0.0);
    Assert::AreEqual(t, 2.375);
}

TEST(MathSum, GEOMEAN_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("GEOMEAN(1,2,3,4)", 0.0);
    Assert::AreEqual(Round(t, 6), Round(2.213363839, 6));
}

TEST(MathSum, HARMEAN_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("HARMEAN(1,2,3,4)", 0.0);
    Assert::AreEqual(Round(t, 6), Round(1.92, 6));
}

TEST(MathSum, COUNT_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("COUNT(1,2,3,4,2,2,1,4)", 0.0);
    Assert::AreEqual(t, 8.0);
}

TEST(MathSum, AVEDEV_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("AVEDEV(1,2,3,4,2,2,1,4)", 0.0);
    Assert::AreEqual(t, 0.96875);
}

TEST(MathSum, STDEV_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("STDEV(1,2,3,4,2,2,1,4)", 0.0);
    Assert::AreEqual(Round(t, 6), Round(1.187734939, 6));
}

TEST(MathSum, STDEVP_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("STDEVP(1,2,3,4,2,2,1,4)", 0.0);
    Assert::AreEqual(Round(t, 6), Round(1.111024302, 6));
}

TEST(MathSum, DEVSQ_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("DEVSQ(1,2,3,4,2,2,1,4)", 0.0);
    Assert::AreEqual(Round(t, 6), Round(9.875, 6));
}

TEST(MathSum, VAR_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("VAR(1,2,3,4,2,2,1,4)", 0.0);
    Assert::AreEqual(Round(t, 6), Round(1.410714286, 6));
}

TEST(MathSum, VARP_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("VARP(1,2,3,4,2,2,1,4)", 0.0);
    Assert::AreEqual(Round(t, 6), Round(1.234375, 6));
}

TEST(MathSum, RANK_test) {
    AlgorithmEngine engine;
    int t = engine.TryEvaluate("RANK(3, array(1, 2, 3, 4, 5))", 0);
    Assert::AreEqual(t, 3);

    t = engine.TryEvaluate("RANK(3, array(1, 2, 3, 4, 5), 0)", 0);
    Assert::AreEqual(t, 3);

    t = engine.TryEvaluate("RANK(3, array(1, 2, 3, 4, 5), 1)", 0);
    Assert::AreEqual(t, 3);

    t = engine.TryEvaluate("RANK(5, array(1, 2, 3, 4, 5))", 0);
    Assert::AreEqual(t, 1);

    t = engine.TryEvaluate("RANK(1, array(1, 2, 3, 4, 5))", 0);
    Assert::AreEqual(t, 5);
}

TEST(MathSum, FORECAST_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("FORECAST(30, array(6, 7, 9, 15, 21), array(20, 28, 31, 38, 40))", 0.0);
    Assert::AreEqual(Round(t, 4), Round(10.6073, 4));
}

TEST(MathSum, INTERCEPT_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("INTERCEPT(array(2, 3, 9, 1, 8), array(6, 5, 11, 7, 5))", 0.0);
    Assert::AreEqual(Round(t, 4), Round(0.0484, 4));
}

TEST(MathSum, SLOPE_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("SLOPE(array(2, 3, 9, 1, 8), array(6, 5, 11, 7, 5))", 0.0);
    Assert::AreEqual(Round(t, 4), Round(0.6694, 4));
}

TEST(MathSum, CORREL_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("CORREL(array(3, 2, 4, 5, 6), array(9, 7, 12, 15, 17))", 0.0);
    Assert::AreEqual(Round(t, 4), Round(0.9971, 4));
}

TEST(MathSum, PEARSON_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("PEARSON(array(3, 2, 4, 5, 6), array(9, 7, 12, 15, 17))", 0.0);
    Assert::AreEqual(Round(t, 4), Round(0.9971, 4));
}

TEST(MathSum, LARGE_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("LARGE(array(1,2,3,4,2,2,1,4),3)", 0.0);
    Assert::AreEqual(Round(t, 6), Round(3.0, 6));
}

TEST(MathSum, SMALL_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("SMALL(array(1,2,3,4,2,2,1,4),3)", 0.0);
    Assert::AreEqual(Round(t, 6), Round(2.0, 6));
}

TEST(MathSum, COVAR_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("COVAR(array(3,7,6,11),array(5,15,13,9))", 0.0);
    double t1 = engine.TryEvaluate("COVARIANCE.P(array(3,7,6,11),array(5,15,13,9))", 0.0);
    Assert::AreEqual(Round(t, 6), Round(3.375, 6));
    Assert::AreEqual(Round(t1, 6), Round(3.375, 6));
}

TEST(MathSum, COVARIANCES_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("COVARIANCE.S(array(3,7,6,11),array(5,15,13,9))", 0.0);
    Assert::AreEqual(Round(t, 6), Round(4.5, 6));
}

TEST(MathSum, COUNTIF_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'>1')", 0.0);
    Assert::AreEqual(t, 6.0);
    t = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'>=1')", 0.0);
    Assert::AreEqual(t, 8.0);
    t = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'!=1')", 0.0);
    Assert::AreEqual(t, 6.0);
    t = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'=1')", 0.0);
    Assert::AreEqual(t, 2.0);
    t = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'<=1')", 0.0);
    Assert::AreEqual(t, 2.0);
    t = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'<2')", 0.0);
    Assert::AreEqual(t, 2.0);
}

TEST(MathSum, COUNTIF_boolean_criteria_test) {
    AlgorithmEngine engine;
    // 布尔 criteria 既非数字也非文本,应返回参数错误(修复前抛 NotImplementedException)
    double t = engine.TryEvaluate("COUNTIF(array(1,2,3), true)", 0.0);
    (void)t;
    Assert::IsNotNull(engine.LastError);
}

TEST(MathSum, SUMIF_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("SUMIF(array(1,2,3,4,2,2,1,4),'>1')", 0.0);
    Assert::AreEqual(t, 17.0);
    t = engine.TryEvaluate("SUMIF(array(1,2,3,4,2,2,1,4),'>1',array(1,1,1,1,1,1,1,1))", 0.0);
    Assert::AreEqual(t, 6.0);
    t = engine.TryEvaluate("SUMIF(array(1,2,3,4,2,2,1,4),'>=2',array(1,1,1,1,1,1,1,1))", 0.0);
    Assert::AreEqual(t, 6.0);
    t = engine.TryEvaluate("SUMIF(array(1,2,3,4,2,2,1,4),'<1',array(1,1,1,1,1,1,1,1))", 0.0);
    Assert::AreEqual(t, 0.0);
    t = engine.TryEvaluate("SUMIF(array(1,2,3,4,2,2,1,4),'==1',array(1,1,1,1,1,1,1,1))", 0.0);
    Assert::AreEqual(t, 2.0);
}

TEST(MathSum, SUMIF_boolean_criteria_test) {
    AlgorithmEngine engine;
    // 布尔 criteria 既非数字也非文本,应返回参数错误(修复前抛 NotImplementedException)
    double t = engine.TryEvaluate("SUMIF(array(1,2,3), true)", 0.0);
    (void)t;
    Assert::IsNotNull(engine.LastError);
}

TEST(MathSum, AVERAGEIF_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("AVERAGEIF(array(1,2,3,4,2,2,1,4),'>1')", 0.0);
    Assert::AreEqual(t, 2.833333333, 6.0);

    t = engine.TryEvaluate("AVERAGEIF(array(1,2,3,4,2,2,1,4),'>1',array(1,1,1,1,1,1,1,1))", 0.0);
    Assert::AreEqual(t, 1.0);
}

TEST(MathSum, AVERAGEIF_boolean_criteria_test) {
    AlgorithmEngine engine;
    // 布尔 criteria 既非数字也非文本,应返回参数错误(修复前抛 NotImplementedException)
    double t = engine.TryEvaluate("AVERAGEIF(array(1,2,3), true)", 0.0);
    (void)t;
    Assert::IsNotNull(engine.LastError);
}

TEST(MathSum, SERIESSUM_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("SERIESSUM(2, 0, 1, array(1, 1, 1, 1))", 0.0);
    Assert::AreEqual(t, 15.0);
}

TEST(MathSum, SERIESSUM_non_numeric_coefficient_test) {
    AlgorithmEngine engine;
    // 系数数组含非数字元素时跳过但保持索引位置,避免后续指数 n + i*m 错位
    // 2*2^0 + 3*2^2 = 2 + 12 = 14(修复前会算成 2*2^0 + 3*2^1 = 8)
    double t = engine.TryEvaluate("SERIESSUM(2, 0, 1, array(2, 'x', 3))", 0.0);
    Assert::AreEqual(14.0, t);
}

TEST(MathSum, SUMPRODUCT_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("SUMPRODUCT(array(1, 2, 3), array(4, 5, 6))", 0.0);
    Assert::AreEqual(t, 32.0);
}

TEST(MathSum, SUMPRODUCT_single_array_test) {
    AlgorithmEngine engine;
    // 单数组参数等价于求和(修复前被误判为参数错误)
    double t = engine.TryEvaluate("SUMPRODUCT(array(1, 2, 3))", 0.0);
    Assert::AreEqual(6.0, t);
}

TEST(MathSum, SUMX2MY2_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("SUMX2MY2(array(1, 2, 3), array(4, 5, 6))", 0.0);
    Assert::AreEqual(t, -63.0);
}

TEST(MathSum, SUMX2PY2_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("SUMX2PY2(array(1, 2, 3), array(4, 5, 6))", 0.0);
    Assert::AreEqual(t, 91.0);
}

TEST(MathSum, SUMXMY2_test) {
    AlgorithmEngine engine;
    double t = engine.TryEvaluate("SUMXMY2(array(1, 2, 3), array(4, 5, 6))", 0.0);
    Assert::AreEqual(t, 27.0);
}

}  // namespace
