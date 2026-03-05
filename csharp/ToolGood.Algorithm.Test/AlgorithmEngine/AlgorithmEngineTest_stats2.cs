using PetaTest;
using System;

namespace ToolGood.Algorithm.Test
{
    [TestFixture]
    internal class AlgorithmEngineTest_stats2
    {
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
    }
}
