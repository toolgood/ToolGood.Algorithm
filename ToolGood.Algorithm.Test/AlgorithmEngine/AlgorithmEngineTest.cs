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

            var b1 = engine.TryEvaluate("if(true,1,2)", 0);
            Assert.AreEqual(1, b1);

            b1 = engine.TryEvaluate("if(false,1,2)", 0);
            Assert.AreEqual(2, b1);

            var b2 = engine.TryEvaluate("pi*4", 0.0);
            Assert.AreEqual(Math.PI * 4, b2);
            b2 = engine.TryEvaluate("e*4", 0.0);
            Assert.AreEqual(Math.E * 4, b2);

            var s = engine.TryEvaluate("'aa'&'bb'", "");
            Assert.AreEqual("aabb", s);

            var r = engine.TryEvaluate("count({ 1,2,3,4})", 0);
            Assert.AreEqual(4, r);


            r = engine.TryEvaluate("(1=1)*9+2", 0);
            Assert.AreEqual(11, r); ;
            r = engine.TryEvaluate("(1=2)*9+2", 0);
            Assert.AreEqual(2, r); ;

            var dt = engine.TryEvaluate("'2016-1-1'+1", DateTime.MinValue);
            Assert.AreEqual(DateTime.Parse("2016-1-2"), dt); ;
            dt = engine.TryEvaluate("'2016-1-1'+9*'1:0'", DateTime.MinValue);
            Assert.AreEqual(DateTime.Parse("2016-1-1 9:0"), dt); ;
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

        [Test]
        public void name_Test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            if (engine.Parse("a", "1+(3*2+2)/2")) {
                var t = engine.Evaluate("a");
                Assert.AreEqual(5, (int)(double)t);
                return;
            }
            throw new Exception("");
        }



    }
}
