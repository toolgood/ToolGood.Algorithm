using PetaTest;
using System;

namespace ToolGood.Algorithm.Test.MathSum2
{
    [TestFixture]
    internal class MathSum2Test
    {
        [Test]
        public void NORMSDIST_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("NORMSDIST(1)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.841344746, 6));
        }

        [Test]
        public void NORMDIST_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("NORMDIST(3,8,4,1)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.105649774, 6));
            t = engine.TryEvaluate("NORMDIST(3,8,4,0)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.045662271, 6));
        }

        [Test]
        public void NORMINV_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("NORMINV(0.8,8,3)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(10.5248637, 6));
        }

        [Test]
        public void NORMSINV_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("NORMSINV(0.3)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(-0.524400513, 6));
        }

        [Test]
        public void BETADIST_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("BETADIST(0.5,11,22)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.97494877, 6));
        }

        [Test]
        public void BETAINV_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("BETAINV(0.5,23,45)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.336640759, 6));
        }

        [Test]
        public void BETAINV_param_count_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            // 参数个数不足时,构造函数抛出异常并被引擎捕获,返回默认值而非空引用
            var t = engine.TryEvaluate("BETAINV(0.5,23)", 0.0);
            Assert.IsTrue(engine.LastError != null);
        }

        [Test]
        public void BINOMDIST_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("BINOMDIST(12,45,0.5,0)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.000817409, 6));
            t = engine.TryEvaluate("BINOMDIST(12,45,0.5,1)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.00122945, 6));
        }

        [Test]
        public void EXPONDIST_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("EXPONDIST(3,1,0)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.049787068, 6));
            t = engine.TryEvaluate("EXPONDIST(3,1,1)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.950212932, 6));
        }

        [Test]
        public void FDIST_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("FDIST(0.4,2,3)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.701465776, 6));
        }

        [Test]
        public void FINV_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("FINV(0.7,2,3)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.402651432, 6));
        }

        [Test]
        public void GAMMADIST_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("GAMMADIST(0.5,3,4,0)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.001723627, 6));
            t = engine.TryEvaluate("GAMMADIST(0.5,3,4,1)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.000296478, 6));
        }

        [Test]
        public void GAMMAINV_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("GAMMAINV(0.2,3,4)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(6.140176811, 6));
        }

        [Test]
        public void GAMMALN_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("GAMMALN(4)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(1.791759469, 6));
        }

        [Test]
        public void HYPGEOMDIST_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("HYPGEOMDIST(23,45,45,100)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.08715016, 6));
        }

        [Test]
        public void LOGINV_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("LOGINV(0.1,45,33)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(15.01122624, 6));
        }

        [Test]
        public void LOGNORMDIST_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("LOGNORMDIST(15,23,45)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.326019201, 6));
        }

        [Test]
        public void NEGBINOMDIST_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("NEGBINOMDIST(23,45,0.7)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.053463314, 6));
        }

        [Test]
        public void POISSON_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("POISSON(23,23,0)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.082884384, 6));
            t = engine.TryEvaluate("POISSON(23,23,1)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.555149936, 6));
        }

        [Test]
        public void TDIST_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("TDIST(1.2,24,1)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.120925677, 6));
            t = engine.TryEvaluate("TDIST(1.2,24,2)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.241851353, 6));
        }

        [Test]
        public void TINV_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("TINV(0.12,23)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(1.614756561, 6));
        }

        [Test]
        public void WEIBULL_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("WEIBULL(1,2,3,1)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.105160683, 6));
            t = engine.TryEvaluate("WEIBULL(1,2,3,0)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.198853182, 6));
            t = engine.TryEvaluate("WEIBULL(-1,2,3,0)", 0.0);
            t = engine.TryEvaluate("WEIBULL(-1,-2,3,0)", 0.0);
            t = engine.TryEvaluate("WEIBULL(-1,-2,-3,0)", 0.0);
            t = engine.TryEvaluate("WEIBULL(-1,-2,-3,-1)", 0.0);
        }

        [Test]
        public void WEIBULL_tiny_x_cdf_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            // 极小 x 时 CDF 应趋近于 0 且非负(修复前 ExponentialMinusOne 符号错误导致负值)
            var t = engine.TryEvaluate("WEIBULL(1E-28,1,1,1)", 0.0);
            Assert.IsTrue(engine.LastError == null);
            Assert.IsTrue(t >= 0.0);
        }

        [Test]
        public void FISHER_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("FISHER(0.68)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.8291140383, 6));
        }

        [Test]
        public void FISHERINV_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("FISHERINV(0.6)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.537049567, 6));
        }

        [Test]
        public void FISHERINV_overflow_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            // 2x 超出 decimal 的 e 指数范围(约 ±66)时,修复前抛 OverflowException
            // 修复后 tanh 趋近于 ±1,与 Java 版 double 计算一致
            var t = engine.TryEvaluate("FISHERINV(40)", 0.0);
            Assert.AreEqual(1.0, t, 1e-9);

            t = engine.TryEvaluate("FISHERINV(-40)", 0.0);
            Assert.AreEqual(-1.0, t, 1e-9);

            // 边界内仍正常计算
            t = engine.TryEvaluate("FISHERINV(0.6)", 0.0);
            Assert.AreEqual(Math.Round(t, 6), Math.Round(0.537049567, 6));
        }

        #region Bessel函数

        [Test]
        public void BESSELI_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("BESSELI(1.5, 1)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(0.981666, t);

            t = engine.TryEvaluate("BESSELI(1.5, 0)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(1.646723, t);
        }

        [Test]
        public void BESSELJ_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("BESSELJ(1.5, 1)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(0.557937, t);

            t = engine.TryEvaluate("BESSELJ(1.5, 0)", 0.0);
            t = Math.Round(t, 6);
            Assert.AreEqual(0.511828, t);
        }

        [Test]
        public void BESSELK_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("BESSELK(1.5, 1)", 0.0);
            t = Math.Round(t, 3);
            Assert.AreEqual(0.277, t, 0.01);

            t = engine.TryEvaluate("BESSELK(1.5, 0)", 0.0);
            t = Math.Round(t, 3);
            Assert.AreEqual(0.214, t, 0.01);
        }

        [Test]
        public void BESSELY_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("BESSELY(1.5, 1)", 0.0);
            t = Math.Round(t, 3);
            Assert.AreEqual(-0.412, t, 0.01);

            t = engine.TryEvaluate("BESSELY(2.5, 0)", 0.0);
            t = Math.Round(t, 3);
            Assert.AreEqual(0.498, t, 0.01);
        }

        [Test]
        public void BESSELK_overflow_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            // x 过小时 K_n(x) 结果超出 decimal 范围,应返回错误而非抛异常
            var t = engine.TryEvaluate("BESSELK(1E-28,2)", 0.0);
            Assert.IsTrue(engine.LastError != null);
        }

        [Test]
        public void BESSELY_overflow_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            // x 过小时 Y_n(x) 结果超出 decimal 范围,应返回错误而非抛异常
            var t = engine.TryEvaluate("BESSELY(1E-28,2)", 0.0);
            Assert.IsTrue(engine.LastError != null);
        }

        [Test]
        public void BESSELJ_overflow_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            // Miller 递推中间值超出 decimal 范围,应返回错误而非抛异常
            var t = engine.TryEvaluate("BESSELJ(1E-10,100)", 0.0);
            Assert.IsTrue(engine.LastError != null);
        }

        #endregion Bessel函数
    }
}
