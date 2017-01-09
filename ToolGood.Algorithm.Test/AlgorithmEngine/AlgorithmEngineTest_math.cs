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
        [Test]
        public void combin_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("combin(10,2)", 0.0);
            Assert.AreEqual(45.0, t);
        }
        [Test]
        public void PERMUT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("PERMUT(10,2)", 0.0);
            Assert.AreEqual(90.0, t);
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
        [Test]
        public void ROUND_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("ROUND(4.333,2)", 0.0);
            Assert.AreEqual(4.33, t);
        }
        [Test]
        public void ROUNDDOWN_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("ROUNDDOWN(4.333,2)", 0.0);
            Assert.AreEqual(4.33, t);

            t = engine.TryEvaluate("ROUNDDOWN(-3.141592, 3)", 0.0);
            Assert.AreEqual(-3.141, t);
        }
        [Test]
        public void ROUNDUP_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("ROUNDUP(4.333,2)", 0.0);
            Assert.AreEqual(4.34, t);

            t = engine.TryEvaluate("ROUNDUP(-3.141592, 3)", 0.0);
            Assert.AreEqual(-3.142, t);
        }
        [Test]
        public void CEILING_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("CEILING(4.333,0.1)", 0.0);
            Assert.AreEqual(4.4, t);

            t = engine.TryEvaluate("CEILING(4.333)", 0.0);
            Assert.AreEqual(5, t);
        }
        [Test]
        public void FLOOR_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("FLOOR(4.363,0.1)", 0.0);
            Assert.AreEqual(4.3, t);

            t = engine.TryEvaluate("FLOOR(4.333)", 0.0);
            Assert.AreEqual(4, t);
        }
        [Test]
        public void even_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("even(4.363)", 0.0);
            Assert.AreEqual(6.0, t);
        }
        [Test]
        public void odd_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("odd(4.363)", 0.0);
            Assert.AreEqual(5, t);
        }
        [Test]
        public void MROUND_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("MROUND(4.363,2)", 0.0);
            Assert.AreEqual(4, t);
            t = engine.TryEvaluate("MROUND(5.363,2)", 0.0);
            Assert.AreEqual(6, t);
        }


        #endregion

        #region 随机数
        [Test]
        public void Rand_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("RAND()", 0.0);
            Assert.Greater(t, 0);
            Assert.LessOrEqual(t, 1);
        }
        [Test]
        public void RANDBETWEEN_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("RANDBETWEEN(2,99)", 0.0);
            Assert.Greater(t, 2);
            Assert.LessOrEqual(t, 99);
        }

        #endregion

        #region 幂/对数/阶乘
        [Test]
        public void fact_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("fact(6)", 0.0);
            Assert.AreEqual(720.0, t);
            t = engine.TryEvaluate("fact(3)", 0.0);
            Assert.AreEqual(6.0, t);
        }
        [Test]
        public void factdouble_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("factdouble(10)", 0.0);
            Assert.AreEqual(3840.0, t);
        }
        [Test]
        public void POWER_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("POWER(10,2)", 0.0);
            Assert.AreEqual(100.0, t);
        }
        [Test]
        public void exp_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("exp(2)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(7.389056099, 6), t);
        }
        [Test]
        public void ln_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("ln(4)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(1.386294361, 6), t);
        }
        [Test]
        public void log_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("log(10)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(1.0, 6), t);

            t = engine.TryEvaluate("log(8,2)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(3.0, 6), t);
        }
        [Test]
        public void log10_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("log10(10)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(1.0, 6), t);
        }
        [Test]
        public void MULTINOMIAL_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("MULTINOMIAL(1,2,3)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(60.0, 6), t);
            t = engine.TryEvaluate("MULTINOMIAL(1,2,3,4)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(12600.0, 6), t);
        }
        [Test]
        public void PRODUCT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("PRODUCT(1,2,3,4)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(24.0, 6), t);
        }
        [Test]
        public void SQRTPI_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("SQRTPI(3)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(3.069980124, 6), t);
        }
        [Test]
        public void SUMSQ_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("SUMSQ(1,2)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(Math.Round(5.0, 6), t);
        }

        #endregion




    }
}
