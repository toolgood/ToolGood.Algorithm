using PetaTest;
using System;

namespace ToolGood.Algorithm.Test.MathTrigonometric
{
    [TestFixture]
    internal class MathTrigonometricTest
    {
        [Test]
        public void degrees_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("degrees(pi())", 0.0);
            Assert.AreEqual(180.0, t);
        }

        [Test]
        public void RADIANS_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("RADIANS(180)", 0.0);
            Assert.AreEqual(Math.PI, t, 10);
        }

        [Test]
        public void cos_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("cos(1)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(0.540302306, 6), t);
        }

        [Test]
        public void cosh_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("cosh(1)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(1.543080635, 6), t);
        }

        [Test]
        public void sin_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("sin(1)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(0.841470985, 6), t);
        }

        [Test]
        public void sinh_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("sinh(1)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(1.175201194, 6), t);
        }

        [Test]
        public void tan_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("tan(1)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(1.557407725, 6), t);
        }

        [Test]
        public void tanh_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("tanh(1)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(0.761594156, 6), t);
        }

        [Test]
        public void acos_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("acos(0.5)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(1.047197551, 6), t);
        }

        [Test]
        public void acosh_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("acosh(1.5)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(0.96242365, 6), t);
        }

        [Test]
        public void asin_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("asin(0.5)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(0.523598776, 6), t);
        }

        [Test]
        public void asinh_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("asinh(1.5)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(1.194763217, 6), t);
        }

        [Test]
        public void atan_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("atan(1)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(0.785398163, 6), t);
        }

        [Test]
        public void atanh_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("atanh(0.5)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(0.549306144, 6), t);
        }

        [Test]
        public void atan2_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("atan2(1,2)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(1.107148718, 6), t);
        }

        [Test]
        public void cot_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("cot(1)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(0.642092616, 6), t);
        }

        [Test]
        public void coth_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("coth(1)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(1.313035285, 6), t);
        }

        [Test]
        public void csc_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("csc(1)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(1.188395106, 6), t);
        }

        [Test]
        public void csch_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("csch(1)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(0.850918128, 6), t);
        }

        [Test]
        public void sec_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("sec(1)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(1.850815718, 6), t);
        }

        [Test]
        public void sech_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("sech(1)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(0.648054274, 6), t);
        }

        [Test]
        public void acot_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("acot(1)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(0.785398163, 6), t);
        }

        [Test]
        public void acoth_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("acoth(2)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(0.549306144, 6), t);
        }
    }
}
