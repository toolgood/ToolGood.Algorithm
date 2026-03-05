using PetaTest;
using System;

namespace ToolGood.Algorithm.Test
{
    [TestFixture]
    internal class AlgorithmEngineTest_math2
    {
        [Test]
        public void ARABIC_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("ARABIC('I')", 0);
            Assert.AreEqual(t, 1);

            t = engine.TryEvaluate("ARABIC('IV')", 0);
            Assert.AreEqual(t, 4);

            t = engine.TryEvaluate("ARABIC('IX')", 0);
            Assert.AreEqual(t, 9);

            t = engine.TryEvaluate("ARABIC('X')", 0);
            Assert.AreEqual(t, 10);

            t = engine.TryEvaluate("ARABIC('XL')", 0);
            Assert.AreEqual(t, 40);

            t = engine.TryEvaluate("ARABIC('L')", 0);
            Assert.AreEqual(t, 50);

            t = engine.TryEvaluate("ARABIC('XC')", 0);
            Assert.AreEqual(t, 90);

            t = engine.TryEvaluate("ARABIC('C')", 0);
            Assert.AreEqual(t, 100);

            t = engine.TryEvaluate("ARABIC('CD')", 0);
            Assert.AreEqual(t, 400);

            t = engine.TryEvaluate("ARABIC('D')", 0);
            Assert.AreEqual(t, 500);

            t = engine.TryEvaluate("ARABIC('CM')", 0);
            Assert.AreEqual(t, 900);

            t = engine.TryEvaluate("ARABIC('M')", 0);
            Assert.AreEqual(t, 1000);

            t = engine.TryEvaluate("ARABIC('MMXXIII')", 0);
            Assert.AreEqual(t, 2023);
        }

        [Test]
        public void ROMAN_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("ROMAN(1)", "");
            Assert.AreEqual(t, "I");

            t = engine.TryEvaluate("ROMAN(4)", "");
            Assert.AreEqual(t, "IV");

            t = engine.TryEvaluate("ROMAN(9)", "");
            Assert.AreEqual(t, "IX");

            t = engine.TryEvaluate("ROMAN(10)", "");
            Assert.AreEqual(t, "X");

            t = engine.TryEvaluate("ROMAN(40)", "");
            Assert.AreEqual(t, "XL");

            t = engine.TryEvaluate("ROMAN(50)", "");
            Assert.AreEqual(t, "L");

            t = engine.TryEvaluate("ROMAN(90)", "");
            Assert.AreEqual(t, "XC");

            t = engine.TryEvaluate("ROMAN(100)", "");
            Assert.AreEqual(t, "C");

            t = engine.TryEvaluate("ROMAN(400)", "");
            Assert.AreEqual(t, "CD");

            t = engine.TryEvaluate("ROMAN(500)", "");
            Assert.AreEqual(t, "D");

            t = engine.TryEvaluate("ROMAN(900)", "");
            Assert.AreEqual(t, "CM");

            t = engine.TryEvaluate("ROMAN(1000)", "");
            Assert.AreEqual(t, "M");

            t = engine.TryEvaluate("ROMAN(2023)", "");
            Assert.AreEqual(t, "MMXXIII");
        }

        [Test]
        public void SERIESSUM_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("SERIESSUM(2, 0, 1, array(1, 1, 1, 1))", 0.0);
            Assert.AreEqual(t, 15.0);
        }

        [Test]
        public void SUMPRODUCT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("SUMPRODUCT(array(1, 2, 3), array(4, 5, 6))", 0.0);
            Assert.AreEqual(t, 32.0);
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
    }
}
