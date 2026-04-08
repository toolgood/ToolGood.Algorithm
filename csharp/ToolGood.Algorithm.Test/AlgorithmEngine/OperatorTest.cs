using PetaTest;

namespace ToolGood.Algorithm.Test.Operator
{
    [TestFixture]
    internal class OperatorTest
    {
        [Test]
        public void arithmetic_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("1+(3*2+2)/2", 0);
            Assert.AreEqual(5, t);

            t = engine.TryEvaluate("(8-3)*(3+2)", 0);
            Assert.AreEqual(25, t);

            t = engine.TryEvaluate("(8-3)*(3+2) % 7", 0);
            Assert.AreEqual(4, t);

            var c = engine.TryEvaluate("2+3", 0);
            Assert.AreEqual(5, c);
            c = engine.TryEvaluate("(2)+3", 0);
            Assert.AreEqual(5, c);
            c = engine.TryEvaluate("2+3*2+10/2*4", 0);
            Assert.AreEqual(28, c);

            c = engine.TryEvaluate("2.1e3 + 10", 0);
            Assert.AreEqual(2110, c);

            c = engine.TryEvaluate("2.1e+03 + 10", 0);
            Assert.AreEqual(2110, c);

            c = engine.TryEvaluate("2.1e+3 + 10", 0);
            Assert.AreEqual(2110, c);

            var d = engine.TryEvaluate("2.1e-3 + 10", 0.0);
            Assert.AreEqual(10.0021, d);
        }

        [Test]
        public void connect_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var s = engine.TryEvaluate("'aa'&'bb'", "");
            Assert.AreEqual("aabb", s);

            s = engine.TryEvaluate("'3'+2", "");
            Assert.AreEqual("5", s);
        }

        [Test]
        public void conditional_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t1 = engine.TryEvaluate("-7 < -2 ?1 : 2", 0);
            Assert.AreEqual(t1, 1);
            t1 = engine.TryEvaluate("-7 < -2 ?1 ： 2", 0);
            Assert.AreEqual(t1, 1);

			t1 = engine.TryEvaluate("-7 < -2 ?1 ： (7>1?3:2)", 0);
			Assert.AreEqual(t1, 1);

			t1 = engine.TryEvaluate("-7 < -2 ?1 ：2", 0);
            Assert.AreEqual(t1, 1);
            t1 = engine.TryEvaluate("-7 < -2 ？ 1 : 2", 0);
            Assert.AreEqual(t1, 1);
            t1 = engine.TryEvaluate("-7 < -2 ？1 : 2", 0);
            Assert.AreEqual(t1, 1);

            t1 = engine.TryEvaluate("-7 < -2 ？1 ： 2", 0);
            Assert.AreEqual(t1, 1);

            t1 = engine.TryEvaluate("(!(-7 < -2))？1：2", 0);
            Assert.AreEqual(t1, 2);
            t1 = engine.TryEvaluate("1>2？1：2", 0);
            Assert.AreEqual(t1, 2);

            t1 = engine.TryEvaluate("1！=2？1：2", 0);
            Assert.AreEqual(t1, 1);
        }

        [Test]
        public void percentage_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("100%", 0.0);
            Assert.AreEqual(1.0, t);

            t = engine.TryEvaluate("50%", 0.0);
            Assert.AreEqual(0.5, t);

            t = engine.TryEvaluate("200%", 0.0);
            Assert.AreEqual(2.0, t);

            t = engine.TryEvaluate("100*50%", 0.0);
            Assert.AreEqual(50.0, t);

            t = engine.TryEvaluate("100+50%", 0.0);
            Assert.AreEqual(100.5, t);
        }

        [Test]
        public void null_operation_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var tbb2 = engine.TryEvaluate("'111'*null", 0);
            Assert.AreEqual(tbb2, 0);
        }
    }
}
