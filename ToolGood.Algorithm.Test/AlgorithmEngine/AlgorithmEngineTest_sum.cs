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





        #endregion


    }
}
