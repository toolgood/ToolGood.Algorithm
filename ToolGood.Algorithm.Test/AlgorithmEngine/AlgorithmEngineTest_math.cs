using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaTest;

namespace ToolGood.Algorithm
{
    [TestFixture]
    class AlgorithmEngineTest_math
    {
        #region 基础数学
        [Test]
        public void Pi_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("pi()", 0.0);
            Assert.AreEqual(3.141592654, Math.Round(t, 9));
        }
        [Test]
        public void abs_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("abs(-1.2)", 0.0);
            Assert.AreEqual(1.2, t);
        }
        [Test]
        public void QUOTIENT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("QUOTIENT(7,3)", 0.0);
            Assert.AreEqual(2.0, t);
        }
        [Test]
        public void MOD_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("MOD(7,3)", 0.0);
            Assert.AreEqual(1.0, t);
        }
        [Test]
        public void SIGN_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("SIGN(0)", 0);
            Assert.AreEqual(0, t);
            t = engine.TryEvaluate("SIGN(9)", 0);
            Assert.AreEqual(1, t);
            t = engine.TryEvaluate("SIGN(-9)", 0);
            Assert.AreEqual(-1, t);
        }
        [Test]
        public void SQRT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("SQRT(9)", 0.0);
            Assert.AreEqual(3.0, t);
        }
        [Test]
        public void SUM_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("SUM(1,2,3,4)", 0.0);
            Assert.AreEqual(10.0, t);
        }
        [Test]
        public void TRUNC_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("TRUNC(9.222)", 0.0);
            Assert.AreEqual(9.0, t);
            t = engine.TryEvaluate("TRUNC(-9.222)", 0.0);
            Assert.AreEqual(-9.0, t);
        }
        [Test]
        public void int_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("int(9.222)", 0.0);
            Assert.AreEqual(9.0, t);
            t = engine.TryEvaluate("int(-9.222)", 0.0);
            Assert.AreEqual(-9.0, t);
        }
        [Test]
        public void GCD_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("GCD(3,5,7)", 0.0);
            Assert.AreEqual(1.0, t);
            t = engine.TryEvaluate("GCD(30,21)", 0.0);
            Assert.AreEqual(3.0, t);
        }
        [Test]
        public void LCM_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("LCM(3,5,7)", 0.0);
            Assert.AreEqual(105.0, t);

        }

        #endregion

        #region 三角函数
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
            Assert.AreEqual(Math.PI, t);
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


        #endregion

        #region 四舍五入



        #endregion



    }
}
