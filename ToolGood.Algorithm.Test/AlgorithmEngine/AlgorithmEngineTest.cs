using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaTest;

namespace ToolGood.Algorithm
{
    [TestFixture]
    class AlgorithmEngineTest
    {
        [Test]
        public void Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            double t = 0.0;
            if (engine.Parse("1+2")) {
                t = (double)engine.Evaluate();
            }
            Assert.AreEqual(3.0, t);

            var c = engine.TryEvaluate("2+3", 0);
            Assert.AreEqual(5, c);

            var e = engine.TryEvaluate("e", 0.0);
            Assert.AreEqual(Math.E, e);
            e = engine.TryEvaluate("pi", 0.0);
            Assert.AreEqual(Math.PI, e);

            var b = engine.TryEvaluate("true", true);
            Assert.AreEqual(true, b);
            b = engine.TryEvaluate("false", false);
            Assert.AreEqual(false, b);

        }

        [Test]
        public void base_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("1+(3*2+2)/2", 0);
            Assert.AreEqual(5, t);

            t = engine.TryEvaluate("(8-3)*(3+2)", 0);
            Assert.AreEqual(25, t);

            t = engine.TryEvaluate("(8-3)*(3+2) % 7", 0);
            Assert.AreEqual(4, t);

            var b = engine.TryEvaluate("1=1", false);
            Assert.AreEqual(true, b);

            b = engine.TryEvaluate("1=2", false);
            Assert.AreEqual(false, b);

            b = engine.TryEvaluate("1<>2", false);
            Assert.AreEqual(true, b);

            b = engine.TryEvaluate("1!=2", false);
            Assert.AreEqual(true, b);

            b = engine.TryEvaluate("1>2", false);
            Assert.AreEqual(false, b);

            b = engine.TryEvaluate("1<2", false);
            Assert.AreEqual(true, b);

            b = engine.TryEvaluate("1<=2", false);
            Assert.AreEqual(true, b);

            b = engine.TryEvaluate("1>=2", false);
            Assert.AreEqual(false, b);

        }




    }
}
