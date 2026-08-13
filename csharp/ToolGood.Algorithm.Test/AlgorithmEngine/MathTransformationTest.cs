using PetaTest;

namespace ToolGood.Algorithm.Test.MathTransformation
{
    [TestFixture]
    internal class MathTransformationTest
    {
        [Test]
        public void BIN2DEC_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var num = engine.TryEvaluate("BIN2DEC(10101)", 0);
            Assert.AreEqual(num, 21);
        }

        [Test]
        public void OCT2DEC_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var num = engine.TryEvaluate("OCT2DEC(12456)", 0);
            Assert.AreEqual(num, 5422);
        }

        [Test]
        public void HEX2DEC_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var num = engine.TryEvaluate("HEX2DEC('213adf')", 0);
            Assert.AreEqual(num, 2177759);
        }

        [Test]
        public void DEC2BIN_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("DEC2BIN(10)", "");
            Assert.AreEqual(t, "1010");
            t = engine.TryEvaluate("DEC2BIN(10,8)", "");
            Assert.AreEqual(t, "00001010");
        }

        [Test]
        public void OCT2BIN_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("OCT2BIN('721')", "");
            Assert.AreEqual(t, "111010001");
        }

        [Test]
        public void HEX2BIN_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("HEX2BIN('fa')", "");
            Assert.AreEqual(t, "11111010");
        }

        [Test]
        public void BIN2OCT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("BIN2OCT(10)", "");
            Assert.AreEqual(t, "2");
        }

        [Test]
        public void DEC2OCT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("DEC2OCT('75')", "");
            Assert.AreEqual(t, "113");
        }

        [Test]
        public void HEX2OCT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("HEX2OCT('f5')", "");
            Assert.AreEqual(t, "365");
        }

        [Test]
        public void BIN2HEX_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("BIN2HEX(101010100)", "");
            Assert.AreEqual(t, "154");
        }

        [Test]
        public void OCT2HEX_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("OCT2HEX(75212)", "");
            Assert.AreEqual(t, "7A8A");
        }

        [Test]
        public void DEC2HEX_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("DEC2HEX(952)", "");
            Assert.AreEqual(t, "3B8");
        }

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
        public void ARABIC_error_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            // 空文本报错(Excel 返回 #VALUE!)
            var t = engine.TryEvaluate("ARABIC('')", 0);
            Assert.IsTrue(engine.LastError != null);

            // 非法字符报错
            t = engine.TryEvaluate("ARABIC('ABC')", 0);
            Assert.IsTrue(engine.LastError != null);

            // 支持小写
            t = engine.TryEvaluate("ARABIC('mcm')", 0);
            Assert.AreEqual(t, 1900);

            // Excel 官方例子: ARABIC("MCMLXXII") = 1972
            t = engine.TryEvaluate("ARABIC('MCMLXXII')", 0);
            Assert.AreEqual(t, 1972);
        }

        [Test]
        public void ROMAN_form_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            // form 0~4 使用不同的简化规则
            Assert.AreEqual("CDXCIX", engine.TryEvaluate("ROMAN(499)", ""));
            Assert.AreEqual("LDVLIV", engine.TryEvaluate("ROMAN(499,1)", ""));
            Assert.AreEqual("XDIX", engine.TryEvaluate("ROMAN(499,2)", ""));
            Assert.AreEqual("VDIV", engine.TryEvaluate("ROMAN(499,3)", ""));
            Assert.AreEqual("ID", engine.TryEvaluate("ROMAN(499,4)", ""));

            // 0 返回空文本
            Assert.AreEqual("", engine.TryEvaluate("ROMAN(0)", "x"));

            // 负数与大于 3999 报错
            var t = engine.TryEvaluate("ROMAN(-1)", "");
            Assert.IsTrue(engine.LastError != null);
            t = engine.TryEvaluate("ROMAN(4000)", "");
            Assert.IsTrue(engine.LastError != null);

            // form 超出 0~4 报错
            t = engine.TryEvaluate("ROMAN(499,5)", "");
            Assert.IsTrue(engine.LastError != null);
            t = engine.TryEvaluate("ROMAN(499,-1)", "");
            Assert.IsTrue(engine.LastError != null);
        }

        [Test]
        public void ARABIC_invalid_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            // 非法重复:IIII 不是标准罗马数字
            var t = engine.TryEvaluate("ARABIC('IIII')", 0);
            Assert.IsTrue(engine.LastError != null);

            engine = new AlgorithmEngine();
            // 非法减法:IC 不是标准罗马数字
            t = engine.TryEvaluate("ARABIC('IC')", 0);
            Assert.IsTrue(engine.LastError != null);

            engine = new AlgorithmEngine();
            // 非法重复:VV 不是标准罗马数字
            t = engine.TryEvaluate("ARABIC('VV')", 0);
            Assert.IsTrue(engine.LastError != null);
        }

        [Test]
        public void places_out_of_range_test()
        {
            // places 超过 10 应返回 #NUM!
            Assert.IsTrue(PlacesError("DEC2BIN(10,20)"));
            Assert.IsTrue(PlacesError("DEC2HEX(952,20)"));
            Assert.IsTrue(PlacesError("DEC2OCT(75,20)"));
            Assert.IsTrue(PlacesError("BIN2OCT(10,20)"));
            Assert.IsTrue(PlacesError("BIN2HEX(101010100,20)"));
            Assert.IsTrue(PlacesError("OCT2BIN(721,20)"));
            Assert.IsTrue(PlacesError("OCT2HEX(75212,20)"));
            Assert.IsTrue(PlacesError("HEX2BIN('fa',20)"));
            Assert.IsTrue(PlacesError("HEX2OCT('f5',20)"));
        }

        private static bool PlacesError(string formula)
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate(formula, "");
            return engine.LastError != null;
        }

        [Test]
        public void negative_places_padding_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            // 10 位补码解析为 -1,places=3 时应输出 "-01" 而非 "0-1"
            var t = engine.TryEvaluate("BIN2DEC('1111111111',3)", "");
            Assert.AreEqual(t, "-01");

            t = engine.TryEvaluate("OCT2DEC('7777777777',3)", "");
            Assert.AreEqual(t, "-01");

            t = engine.TryEvaluate("HEX2DEC('FFFFFFFFFF',3)", "");
            Assert.AreEqual(t, "-01");
        }
    }
}
