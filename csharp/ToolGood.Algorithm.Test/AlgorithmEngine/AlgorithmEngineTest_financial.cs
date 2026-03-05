using PetaTest;
using System;

namespace ToolGood.Algorithm.Test
{
    [TestFixture]
    internal class AlgorithmEngineTest_financial
    {
        [Test]
        public void PMT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("PMT(0.08/12, 10, 10000)", 0.0);
            Assert.AreEqual(Math.Round(t, 4), Math.Round(-1037.0321, 4));
        }

        [Test]
        public void PPMT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("PPMT(0.08/12, 1, 10, 10000)", 0.0);
            Assert.AreEqual(Math.Round(t, 4), Math.Round(-970.3654, 4));
        }

        [Test]
        public void IPMT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("IPMT(0.08/12, 1, 10, 10000)", 0.0);
            Assert.AreEqual(Math.Round(t, 4), Math.Round(-66.6667, 4));
        }

        [Test]
        public void PV_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("PV(0.08/12, 10, -1000)", 0.0);
            Assert.AreEqual(Math.Round(t, 2), Math.Round(9642.90, 2));
        }

        [Test]
        public void FV_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("FV(0.08/12, 10, -1000)", 0.0);
            Assert.AreEqual(Math.Round(t, 2), Math.Round(10305.40 , 2));
        }

        [Test]
        public void NPER_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("NPER(0.08/12, -1000, 10000)", 0.0);
            Assert.AreEqual(Math.Round(t, 2), Math.Round(10.38 , 2));
        }

        [Test]
        public void RATE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("RATE(12,-100,400,0,0,0.1)", 0.0);
            Assert.AreEqual(Math.Round(t, 4), Math.Round(0.2289 , 4));
        }

        [Test]
        public void NPV_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("NPV(0.1, -10000, 3000, 4200, 6800)", 0.0);
            Assert.AreEqual(Math.Round(t, 2), Math.Round(1188.44, 2));
        }

        [Test]
        public void IRR_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("IRR(array(-70000, 12000, 15000, 18000, 21000, 26000))", 0.0);
            Assert.AreEqual(Math.Round(t, 4), Math.Round(0.0866, 4));
        }

        [Test]
        public void MIRR_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("MIRR(array(-70000, 12000, 15000, 18000, 21000, 26000), 0.1, 0.12)", 0.0);
            Assert.AreEqual(Math.Round(t, 4), Math.Round(0.0987, 4));
        }

        [Test]
        public void SLN_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("SLN(30000, 7500, 10)", 0.0);
            Assert.AreEqual(t, 2250.0);
        }

        [Test]
        public void SYD_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("SYD(30000, 7500, 10, 1)", 0.0);
            Assert.AreEqual(Math.Round(t, 2), Math.Round(4090.91, 2));
        }

        [Test]
        public void DDB_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("DDB(2400, 300, 10, 2)", 0.0);
            Assert.AreEqual(Math.Round(t, 2), Math.Round(384.0, 2));
        }

        [Test]
        public void DB_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("DB(1000000, 100000, 6, 1)", 0.0);
            Assert.AreEqual(Math.Round(t, 2), Math.Round(319000.0, 2));
        }

        [Test]
        public void XNPV_test()
        {
            AlgorithmEngineEx engine = new AlgorithmEngineEx();
            engine.AddParameter("values", new double[] { -10000, 2750, 4250, 3250, 2750 });
            engine.AddParameter("dates2", new string[] { "2008-1-1", "2008-3-1", "2008-10-30", "2009-2-15", "2009-4-1" });
            var t = engine.TryEvaluate("XNPV(0.09, values, dates2)", 0.0);
            Assert.AreEqual(Math.Round(t, 2), Math.Round(2086.65, 2));
        }

        [Test]
        public void XIRR_test()
        {
			AlgorithmEngineEx engine = new AlgorithmEngineEx();
            engine.AddParameter("values", new double[] { -10000, 2750, 4250, 3250, 2750 });
            engine.AddParameter("dates2", new string[] { "2008-1-1", "2008-3-1", "2008-10-30", "2009-2-15", "2009-4-1" });
            var t = engine.TryEvaluate("XIRR(values, dates2)", 0.0);
            Assert.AreEqual(Math.Round(t, 4), Math.Round(0.3734, 4));
        }
    }
}
