using PetaTest;
using System;

namespace ToolGood.Algorithm.Test
{
    [TestFixture]
    internal class EngineTest
    {
        [Test]
        public void base_test2()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("1+(3*2+2)/2 & '11' & '11:20'*9 & isnumber(22)*3", "");
        }

        [Test]
        public void base_test3()
        {
            AlgorithmEngine engine = new AlgorithmEngine();

            var c = engine.TryEvaluate("(2)+/*123456*/3", 0);
            Assert.AreEqual(5, c);

            c = engine.TryEvaluate("2+3//eee", 0);
            Assert.AreEqual(5, c);

            c = engine.TryEvaluate("(2)+/*123456*/3 ee22+22", 0);
            Assert.AreEqual(0, c);
        }

        [Test]
        public void base_test4()
        {
            AlgorithmEngine engine = new AlgorithmEngine();

            var c = engine.TryEvaluate("'4dd'&'55' rr", "");
            Assert.AreEqual("", c);
        }

        [Test]
        public void base_test5()
        {
            AlgorithmEngine engine = new AlgorithmEngine();

            var c = engine.TryEvaluate("'4dd'&'55'.left(1)", "");
            Assert.AreEqual("4dd5", c);
        }

        [Test]
        public void Cylinder_Test()
        {
            Cylinder c = new Cylinder(3, 10);
            var t2 = c.TryEvaluate("半径*半径*pi()", 0.0);

            var t = c.TryEvaluate("直径*pi()", 0.0);
            t = c.TryEvaluate("半径*半径*pi()*高", 0.0);

            t = c.TryEvaluate("'半径'*半径*pi()*高", 0.0);

            t = c.TryEvaluate("求面积（10）", 0.0);
            Assert.AreEqual(10 * 10 * Math.PI, t, 10);

            var json = "{'灰色':'L','canBookCount':905,'saleCount':91,'specId':'43b0e72e98731aed69e1f0cc7d64bf4d'}";
            c.AddParameterFromJson(json);

            var tt = c.TryEvaluate("灰色", "");
            Assert.AreEqual("L", tt);
        }
    }
}
