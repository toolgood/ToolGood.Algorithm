using PetaTest;
using System;

namespace ToolGood.Algorithm.Test
{
    [TestFixture]
    internal class EngineTest
    {
        [Test]
        public void Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();

            var c = engine.TryEvaluate("2+3", 0);
            Assert.AreEqual(5, c);
            c = engine.TryEvaluate("(2)+3", 0);
            Assert.AreEqual(5, c);
            c = engine.TryEvaluate("2+3*2+10/2*4", 0);
            Assert.AreEqual(28, c);

            c = engine.TryEvaluate("if(2+3*2+10/2*4,1", 0);
            Assert.AreEqual(0, c);

            var b = engine.TryEvaluate("true", false);
            Assert.AreEqual(true, b);
            b = engine.TryEvaluate("false", true);
            Assert.AreEqual(false, b);

            var b1 = engine.TryEvaluate("if(true,1,2)", 0);
            Assert.AreEqual(1, b1);

            b1 = engine.TryEvaluate("if(false,1,2)", 0);
            Assert.AreEqual(2, b1);

            var b2 = engine.TryEvaluate("pi*4", 0.0);
            Assert.AreEqual(Math.PI * 4, b2, 10);
            b2 = engine.TryEvaluate("e*4", 0.0);
            Assert.AreEqual(Math.E * 4, b2, 10);

            var t2 = engine.TryEvaluate("Ａsc('ａｂｃＡＢＣ１２３')", "");
            Assert.AreEqual(t2, "abcABC123");
        }

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
