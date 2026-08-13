using PetaTest;
using System;

namespace ToolGood.Algorithm.Test.MathSum
{
    [TestFixture]
    internal class MathSumTest
    {
        #region 简单统计

        [Test]
        public void MAX_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("max(1,2,3,4,2,2,1,4)", 0.0);
            Assert.AreEqual(t, 4.0);
        }

        [Test]
        public void MEDIAN_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("MEDIAN(1,2,3,4,2,2,1,4)", 0.0);
            Assert.AreEqual(t, 2.0);

            t = engine.TryEvaluate("MEDIAN(1,1,2,2)", 0.0);
            Assert.AreEqual(t, 1.5);
        }

        [Test]
        public void MIN_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("MIN(1,2,3,4,2,2,1,4)", 0.0);
            Assert.AreEqual(t, 1.0);
        }

        [Test]
        public void QUARTILE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),0)", 0.0);
            Assert.AreEqual(t, 1.0);
            t = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),1)", 0.0);
            Assert.AreEqual(t, 1.75);
            t = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),2)", 0.0);
            Assert.AreEqual(t, 2.0);
            t = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),3)", 0.0);
            Assert.AreEqual(t, 3.25);
            t = engine.TryEvaluate("QUARTILE(array(1,2,3,4,2,2,1,4),4)", 0.0);
            Assert.AreEqual(t, 4.0);
        }

        [Test]
        public void MODE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("MODE(1,2,3,4,2,2,1,4)", 0.0);
            Assert.AreEqual(t, 2.0);
        }

        [Test]
        public void PERCENTILE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("PERCENTILE(array(1,2,3,4,2,2,1,4)，0.4)", 0.0);
            Assert.AreEqual(t, 2.0);
        }

        [Test]
        public void PERCENTRANK_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("PERCENTRANK(array(1,2,3,4,2,2,1,4)，3)", 0.0);
            Assert.AreEqual(t, 0.714);
        }

        [Test]
        public void PERCENTRANK_significance_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            // significance 指定保留小数位数(原始值 5/7≈0.7142857)
            var t = engine.TryEvaluate("PERCENTRANK(array(1,2,3,4,2,2,1,4),3,4)", 0.0);
            Assert.AreEqual(0.7143, t, 1e-9);

            // 28 为允许的最大位数
            t = engine.TryEvaluate("PERCENTRANK(array(1,2,3,4,2,2,1,4),3,28)", 0.0);
            Assert.AreEqual(5.0 / 7.0, t, 1e-12);

            // 超过 28 报错(Math.Round(decimal,int) 上限,修复前会抛 ArgumentOutOfRangeException)
            t = engine.TryEvaluate("PERCENTRANK(array(1,2,3,4,2,2,1,4),3,29)", 0.0);
            Assert.IsTrue(engine.LastError != null);

            // 负数位数报错
            t = engine.TryEvaluate("PERCENTRANK(array(1,2,3,4,2,2,1,4),3,-1)", 0.0);
            Assert.IsTrue(engine.LastError != null);
        }

        [Test]
        public void AVERAGE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("AVERAGE(1,2,3,4,2,2,1,4)", 0.0);
            Assert.AreEqual(t, 2.375);
        }

        [Test]
        public void GEOMEAN_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("GEOMEAN(1,2,3,4)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(2.213363839, 6));
        }

        [Test]
        public void HARMEAN_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("HARMEAN(1,2,3,4)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(1.92, 6));
        }

        [Test]
        public void COUNT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("COUNT(1,2,3,4,2,2,1,4)", 0.0);
            Assert.AreEqual(t, 8.0);
        }

        [Test]
        public void AVEDEV_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("AVEDEV(1,2,3,4,2,2,1,4)", 0.0);
            Assert.AreEqual(t, 0.96875);
        }

        [Test]
        public void STDEV_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("STDEV(1,2,3,4,2,2,1,4)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(1.187734939, 6));
        }

        [Test]
        public void STDEVP_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("STDEVP(1,2,3,4,2,2,1,4)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(1.111024302, 6));
        }

        [Test]
        public void DEVSQ_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("DEVSQ(1,2,3,4,2,2,1,4)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(9.875, 6));
        }

        [Test]
        public void VAR_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("VAR(1,2,3,4,2,2,1,4)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(1.410714286, 6));
        }

        [Test]
        public void VARP_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("VARP(1,2,3,4,2,2,1,4)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(1.234375, 6));
        }

        #endregion 简单统计

        #region 统计函数

        [Test]
        public void RANK_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("RANK(3, array(1, 2, 3, 4, 5))", 0);
            Assert.AreEqual(t, 3);

            t = engine.TryEvaluate("RANK(3, array(1, 2, 3, 4, 5), 0)", 0);
            Assert.AreEqual(t, 3);

            t = engine.TryEvaluate("RANK(3, array(1, 2, 3, 4, 5), 1)", 0);
            Assert.AreEqual(t, 3);

            t = engine.TryEvaluate("RANK(5, array(1, 2, 3, 4, 5))", 0);
            Assert.AreEqual(t, 1);

            t = engine.TryEvaluate("RANK(1, array(1, 2, 3, 4, 5))", 0);
            Assert.AreEqual(t, 5);
        }

        [Test]
        public void FORECAST_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("FORECAST(30, array(6, 7, 9, 15, 21), array(20, 28, 31, 38, 40))", 0.0);
            Assert.AreEqual(Math.Round(t, 4), Math.Round(10.6073, 4));
        }

        [Test]
        public void INTERCEPT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("INTERCEPT(array(2, 3, 9, 1, 8), array(6, 5, 11, 7, 5))", 0.0);
            Assert.AreEqual(Math.Round(t, 4), Math.Round(0.0484, 4));
        }

        [Test]
        public void SLOPE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("SLOPE(array(2, 3, 9, 1, 8), array(6, 5, 11, 7, 5))", 0.0);
            Assert.AreEqual(Math.Round(t, 4), Math.Round(0.6694, 4));
        }

        [Test]
        public void CORREL_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("CORREL(array(3, 2, 4, 5, 6), array(9, 7, 12, 15, 17))", 0.0);
            Assert.AreEqual(Math.Round(t, 4), Math.Round(0.9971, 4));
        }

        [Test]
        public void PEARSON_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("PEARSON(array(3, 2, 4, 5, 6), array(9, 7, 12, 15, 17))", 0.0);
            Assert.AreEqual(Math.Round(t, 4), Math.Round(0.9971, 4));
        }

        [Test]
        public void LARGE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("LARGE(array(1,2,3,4,2,2,1,4),3)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(3.0, 6));
        }

        [Test]
        public void SMALL_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("SMALL(array(1,2,3,4,2,2,1,4),3)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(2.0, 6));
        }

        [Test]
        public void COVAR_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("COVAR(array(3,7,6,11),array(5,15,13,9))", 0.0);
            var t1 = engine.TryEvaluate("COVARIANCE.P(array(3,7,6,11),array(5,15,13,9))", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(3.375, 6));
            Assert.AreEqual(Math.Round(t1, 6), Math.Round(3.375, 6));
        }

        [Test]
        public void COVARIANCES_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("COVARIANCE.S(array(3,7,6,11),array(5,15,13,9))", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(4.5, 6));
        }

        #endregion 统计函数

        #region 条件统计

        [Test]
        public void COUNTIF_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'>1')", 0.0);
            Assert.AreEqual(t, 6.0);
            t = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'>=1')", 0.0);
            Assert.AreEqual(t, 8.0);
            t = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'!=1')", 0.0);
            Assert.AreEqual(t, 6.0);
            t = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'=1')", 0.0);
            Assert.AreEqual(t, 2.0);
            t = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'<=1')", 0.0);
            Assert.AreEqual(t, 2.0);
            t = engine.TryEvaluate("COUNTIF(array(1,2,3,4,2,2,1,4),'<2')", 0.0);
            Assert.AreEqual(t, 2.0);
        }

        [Test]
        public void COUNTIF_boolean_criteria_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            // 布尔 criteria 既非数字也非文本,应返回参数错误(修复前抛 NotImplementedException)
            var t = engine.TryEvaluate("COUNTIF(array(1,2,3), true)", 0.0);
            Assert.IsTrue(engine.LastError != null);
        }

        [Test]
        public void SUMIF_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("SUMIF(array(1,2,3,4,2,2,1,4),'>1')", 0.0);
            Assert.AreEqual(t, 17);
            t = engine.TryEvaluate("SUMIF(array(1,2,3,4,2,2,1,4),'>1',array(1,1,1,1,1,1,1,1))", 0.0);
            Assert.AreEqual(t, 6);
            t = engine.TryEvaluate("SUMIF(array(1,2,3,4,2,2,1,4),'>=2',array(1,1,1,1,1,1,1,1))", 0.0);
            Assert.AreEqual(t, 6);
            t = engine.TryEvaluate("SUMIF(array(1,2,3,4,2,2,1,4),'<1',array(1,1,1,1,1,1,1,1))", 0.0);
            Assert.AreEqual(t, 0);
            t = engine.TryEvaluate("SUMIF(array(1,2,3,4,2,2,1,4),'==1',array(1,1,1,1,1,1,1,1))", 0.0);
            Assert.AreEqual(t, 2);
        }

        [Test]
        public void SUMIF_boolean_criteria_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            // 布尔 criteria 既非数字也非文本,应返回参数错误(修复前抛 NotImplementedException)
            var t = engine.TryEvaluate("SUMIF(array(1,2,3), true)", 0.0);
            Assert.IsTrue(engine.LastError != null);
        }

        [Test]
        public void AVERAGEIF_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("AVERAGEIF(array(1,2,3,4,2,2,1,4),'>1')", 0.0);
            Assert.AreEqual(t, 2.833333333, 6);

            t = engine.TryEvaluate("AVERAGEIF(array(1,2,3,4,2,2,1,4),'>1',array(1,1,1,1,1,1,1,1))", 0.0);
            Assert.AreEqual(t, 1);
        }

        [Test]
        public void AVERAGEIF_boolean_criteria_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            // 布尔 criteria 既非数字也非文本,应返回参数错误(修复前抛 NotImplementedException)
            var t = engine.TryEvaluate("AVERAGEIF(array(1,2,3), true)", 0.0);
            Assert.IsTrue(engine.LastError != null);
        }

        #endregion 条件统计

        #region 数组运算

        [Test]
        public void SERIESSUM_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("SERIESSUM(2, 0, 1, array(1, 1, 1, 1))", 0.0);
            Assert.AreEqual(t, 15.0);
        }

        [Test]
        public void SERIESSUM_non_numeric_coefficient_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            // 系数数组含非数字元素时跳过但保持索引位置,避免后续指数 n + i*m 错位
            // 2*2^0 + 3*2^2 = 2 + 12 = 14(修复前会算成 2*2^0 + 3*2^1 = 8)
            var t = engine.TryEvaluate("SERIESSUM(2, 0, 1, array(2, 'x', 3))", 0.0);
            Assert.AreEqual(14.0, t);
        }

        [Test]
        public void SUMPRODUCT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("SUMPRODUCT(array(1, 2, 3), array(4, 5, 6))", 0.0);
            Assert.AreEqual(t, 32.0);
        }

        [Test]
        public void SUMPRODUCT_single_array_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            // 单数组参数等价于求和(修复前被误判为参数错误)
            var t = engine.TryEvaluate("SUMPRODUCT(array(1, 2, 3))", 0.0);
            Assert.AreEqual(6.0, t);
        }

        [Test]
        public void SUMX2MY2_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("SUMX2MY2(array(1, 2, 3), array(4, 5, 6))", 0.0);
            Assert.AreEqual(t, -63.0);
        }

        [Test]
        public void SUMX2PY2_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("SUMX2PY2(array(1, 2, 3), array(4, 5, 6))", 0.0);
            Assert.AreEqual(t, 91.0);
        }

        [Test]
        public void SUMXMY2_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("SUMXMY2(array(1, 2, 3), array(4, 5, 6))", 0.0);
            Assert.AreEqual(t, 27.0);
        }

        #endregion 数组运算
    }
}
