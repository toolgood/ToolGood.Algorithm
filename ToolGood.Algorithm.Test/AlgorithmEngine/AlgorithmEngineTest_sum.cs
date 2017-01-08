using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaTest;

namespace ToolGood.Algorithm
{
    [TestFixture]
    class AlgorithmEngineTest_sum
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
            var t = engine.TryEvaluate("QUARTILE({1,2,3,4,2,2,1,4},0)", 0.0);
            Assert.AreEqual(t, 1.0);
            t = engine.TryEvaluate("QUARTILE({1,2,3,4,2,2,1,4},1)", 0.0);
            Assert.AreEqual(t, 1.75);
            t = engine.TryEvaluate("QUARTILE({1,2,3,4,2,2,1,4},2)", 0.0);
            Assert.AreEqual(t, 2.0);
            t = engine.TryEvaluate("QUARTILE({1,2,3,4,2,2,1,4},3)", 0.0);
            Assert.AreEqual(t, 3.25);
            t = engine.TryEvaluate("QUARTILE({1,2,3,4,2,2,1,4},4)", 0.0);
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
            var t = engine.TryEvaluate("PERCENTILE({1,2,3,4,2,2,1,4}，0.4)", 0.0);
            Assert.AreEqual(t, 2.0);
        }
        [Test]
        public void PERCENTRANK_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("PERCENTRANK({1,2,3,4,2,2,1,4}，3)", 0.0);
            Assert.AreEqual(t, 0.714);
        }

        [Test]
        public void AVERAGE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("AVERAGE(1,2,3,4,2,2,1,4)", 0.0);
            Assert.AreEqual(t, 2.375);
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
        #endregion

        #region 统计
        [Test]
        public void NORMSDIST_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("NORMSDIST(1)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.841344746, 6));
        }
        [Test]
        public void NORMDIST_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("NORMDIST(3,8,4,1)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.105649774, 6));
            t = engine.TryEvaluate("NORMDIST(3,8,4,0)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.045662271, 6));
        }
        [Test]
        public void NORMINV_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("NORMINV(0.8,8,3)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(10.5248637, 6));
        }
        [Test]
        public void NORMSINV_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("NORMSINV(0.3)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(-0.524400513, 6));
        }
        [Test]
        public void BETADIST_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("BETADIST(0.5,11,22)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.97494877, 6));
        }
        [Test]
        public void BETAINV_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("BETAINV(0.5,23,45)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.336640759, 6));
        }
        [Test]
        public void BINOMDIST_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("BINOMDIST(12,45,0.5,0)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.000817409, 6));
            t = engine.TryEvaluate("BINOMDIST(12,45,0.5,1)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.00122945, 6));
        }
        [Test]
        public void EXPONDIST_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("EXPONDIST(3,1,0)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.049787068, 6));
            t = engine.TryEvaluate("EXPONDIST(3,1,1)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.950212932, 6));
        }
        [Test]
        public void FDIST_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("FDIST(0.4,2,3)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.701465776, 6));
        }
        [Test]
        public void FINV_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("FINV(0.7,2,3)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.402651432, 6));
        }
        [Test]
        public void GAMMADIST_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("GAMMADIST(0.5,3,4,0)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.001723627, 6));
            t = engine.TryEvaluate("GAMMADIST(0.5,3,4,1)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.000296478, 6));
        }
        [Test]
        public void GAMMAINV_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("GAMMAINV(0.2,3,4)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(6.140176811, 6));
        }
        [Test]
        public void HYPGEOMDIST_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("HYPGEOMDIST(23,45,45,100)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.08715016, 6));
        }
        [Test]
        public void LOGINV_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("LOGINV(0.1,45,33)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(15.01122624, 6));
        }
        [Test]
        public void LOGNORMDIST_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("LOGNORMDIST(15,23,45)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.326019201, 6));
        }
        [Test]
        public void NEGBINOMDIST_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("NEGBINOMDIST(23,45,0.7)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.053463314, 6));
        }
        [Test]
        public void POISSON_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("POISSON(23,23,0)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.082884384, 6));
            t = engine.TryEvaluate("POISSON(23,23,1)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.555149936, 6));
        }
        [Test]
        public void TDIST_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("TDIST(1.2,24,1)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.120925677, 6));
            t = engine.TryEvaluate("TDIST(1.2,24,2)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.241851353, 6));
        }
        [Test]
        public void TINV_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("TINV(0.12,23)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(1.614756561, 6));
        }
        [Test]
        public void WEIBULL_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("WEIBULL(1,2,3,1)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.105160683, 6));
            t = engine.TryEvaluate("WEIBULL(1,2,3,0)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.198853182, 6));
        }

        [Test]
        public void FISHER_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("FISHER(0.68)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.8291140383, 6));
        }
        [Test]
        public void FISHERINV_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("FISHERINV(0.6)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.537049567, 6));
        }

        [Test]
        public void LARGE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("LARGE({1,2,3,4,2,2,1,4},3)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(3.0, 6));
        }
        [Test]
        public void SMALL_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("SMALL({1,2,3,4,2,2,1,4},3)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(2.0, 6));
        }


        #endregion

        #region if
        [Test]
        public void COUNTIF_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("COUNTIF({1,2,3,4,2,2,1,4},'>1')", 0.0);
            Assert.AreEqual(t, 6.0);
        }
        
       [Test]
        public void SUMIF_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("SUMIF({1,2,3,4,2,2,1,4},'>1')", 0.0);
            Assert.AreEqual(t, 17);
            t = engine.TryEvaluate("SUMIF({1,2,3,4,2,2,1,4},'>1',{1,1,1,1,1,1,1,1})", 0.0);
            Assert.AreEqual(t, 6);
        }
        [Test]
        public void AVERAGEIF_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("AVERAGEIF({1,2,3,4,2,2,1,4},'>1')", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(2.833333333, 6));
            t = engine.TryEvaluate("AVERAGEIF({1,2,3,4,2,2,1,4},'>1',{1,1,1,1,1,1,1,1})", 0.0);
            Assert.AreEqual(t, 1);
        }



        #endregion
    }
}
