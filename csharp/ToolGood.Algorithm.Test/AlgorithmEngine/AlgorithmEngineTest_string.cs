using PetaTest;

namespace ToolGood.Algorithm.Test
{
    [TestFixture]
    internal class AlgorithmEngineTest_string
    {
        [Test]
        public void ASC_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("asc('ａｂｃＡＢＣ１２３')", "");
            Assert.AreEqual(t, "abcABC123");
        }

        [Test]
        public void Jis_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("jis('abcABC123')", "");
            Assert.AreEqual(t, "ａｂｃＡＢＣ１２３");
            t = engine.TryEvaluate("WIDECHAR('abcABC123')", "");
            Assert.AreEqual(t, "ａｂｃＡＢＣ１２３");
        }

        [Test]
        public void CHAR_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("char(49)", "");
            Assert.AreEqual(t, "1");
        }

        [Test]
        public void CLEAN_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("clean('\\r112\\t')", "");
            Assert.AreEqual(t, "112");
        }

        [Test]
        public void code_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("code('1')", 0);
            Assert.AreEqual(t, 49);
        }

        [Test]
        public void unichar_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("unichar(65)", "");
            Assert.AreEqual(t, "A");
            t = engine.TryEvaluate("unichar(20013)", "");
            Assert.AreEqual(t, "中");
            t = engine.TryEvaluate("unichar(128512)", "");
            Assert.AreEqual(t, "😀");
        }

        [Test]
        public void unicode_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("unicode('A')", 0);
            Assert.AreEqual(t, 65);
            t = engine.TryEvaluate("unicode('中')", 0);
            Assert.AreEqual(t, 20013);
            t = engine.TryEvaluate("unicode('😀')", 0);
            Assert.AreEqual(t, 128512);
        }

        [Test]
        public void CONCATENATE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("CONCATENATE('tt','33')", "");
            Assert.AreEqual(t, "tt33");
        }

        [Test]
        public void CONCATENATE_ALIAS_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("CONCAT('tt','33')", "");
            Assert.AreEqual(t, "tt33");
        }

        [Test]
        public void EXACT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("EXACT('tt','33')", true);
            Assert.AreEqual(t, false);
            t = engine.TryEvaluate("EXACT('tt','tt')", false);
            Assert.AreEqual(t, true);

            t = engine.TryEvaluate("EXACT('33',33)", false);
            Assert.AreEqual(t, true);
            t = engine.TryEvaluate("EXACT('331.1',331.1)", false);
            Assert.AreEqual(t, true);
            t = engine.TryEvaluate("EXACT('TRUE',TRUE())", false);
            Assert.AreEqual(t, true);
            t = engine.TryEvaluate("EXACT('1',TRUE())", true);
            Assert.AreEqual(t, false);
        }

        [Test]
        public void FIND_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("FIND(\"11\",\"12221122\")", 0);
            Assert.AreEqual(t, 5);

			t = engine.TryEvaluate("FIND(\"12\",\"123456789123456789\",6)", 0);
			Assert.AreEqual(t, 10);
		}

        [Test]
        public void FIXED_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("FIXED(4567.89,1)", "");
            Assert.AreEqual(t, "4,567.9");
            //t = engine.TryEvaluate(" FIXED(4567.89,-1)", "");//iserror
            //Assert.AreEqual(t, "4,570.0");
            t = engine.TryEvaluate("FIXED(-4567.89, 1, TRUE())", "");
            Assert.AreEqual(t, "-4567.9");
            t = engine.TryEvaluate("FIXED(77.888)", "");
            Assert.AreEqual(t, "77.89");
        }

        [Test]
        public void LEFT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("LEFT('123222',3)", "");
            Assert.AreEqual(t, "123");
        }

        [Test]
        public void LEN_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("LEN('123222')", 0);
            Assert.AreEqual(t, 6);
        }

        [Test]
        public void LOWER_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("LOWER('ABC')", "");
            Assert.AreEqual(t, "abc");
        }

        [Test]
        public void LOWER_ALIAS_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("TOLOWER('ABC')", "");
            Assert.AreEqual(t, "abc");
        }

        [Test]
        public void MID_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("MID('ABCDEF',2,3)", "");
            Assert.AreEqual(t, "BCD");
        }

        [Test]
        public void PROPER_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("PROPER('abc abc')", "");
            Assert.AreEqual(t, "Abc Abc");
        }

        [Test]
        public void REPLACE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("REPLACE(\"abccd\",2,3,\"2\")", "");
            Assert.AreEqual(t, "a2d");

            var t1 = engine.TryEvaluate("REPLACE(\"abccd\",'bc',\"2\")", "");
            Assert.AreEqual(t1, "a2cd");
        }

        [Test]
        public void REPT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("REPT(\"q\",3)", "");
            Assert.AreEqual(t, "qqq");
        }

        [Test]
        public void RIGHT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("RIGHT(\"123q\",3)", "");
            Assert.AreEqual(t, "23q");
        }

        [Test]
        public void RMB_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("rmb(12.3)", "");
            Assert.AreEqual(t, "壹拾贰元叁角");
        }

        [Test]
        public void SEARCH_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("SEARCH(\"aa\",\"abbAaddd\")", 0);
            Assert.AreEqual(t, 4);
        }

        [Test]
        public void SUBSTITUTE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("SUBSTITUTE(\"ababcc\",\"ab\",\"12\")", "");
            Assert.AreEqual(t, "1212cc");
            t = engine.TryEvaluate("SUBSTITUTE(\"ababcc\",\"ab\",\"12\",2)", "");
            Assert.AreEqual(t, "ab12cc");

            t = engine.TryEvaluate("SUBSTITUTE(\"123456789\",\"123\",\"1111111111111111111111\")", "");
            Assert.AreEqual(t, "1111111111111111111111456789");

            t = engine.TryEvaluate("SUBSTITUTE(\"123456789\",\"1239\",\"1111111111111111111111\")", "");
            Assert.AreEqual(t, "123456789");

            t = engine.TryEvaluate("SUBSTITUTE(\"123456789\",\"9\",\"1111111111111111111111\")", "");
            Assert.AreEqual(t, "123456781111111111111111111111");
        }

        [Test]
        public void T_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("T(12)", "");
            Assert.AreEqual(t, "");
            t = engine.TryEvaluate("T('123')", "");
            Assert.AreEqual(t, "123");
        }

        [Test]
        public void TEXT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("TEXT(123,\"0.00\")", "");
            Assert.AreEqual(t, "123.00");
        }

        [Test]
        public void TRIM_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("TRIM(\" 123 123 \")", "");
            Assert.AreEqual(t, "123 123");
        }

        [Test]
        public void UPPER_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("UPPER(\"abc\")", "");
            Assert.AreEqual(t, "ABC");
        }

        [Test]
        public void UPPER_ALIAS_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("TOUPPER('abc')", "");
            Assert.AreEqual(t, "ABC");
        }

        [Test]
        public void VALUE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("VALUE(\"123\")", 0);
            Assert.AreEqual(t, 123);
        }

        #region 方法式调用测试

        [Test]
        public void MethodStyle_LEN_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("'abcdef'.LEN()", 0);
            Assert.AreEqual(t, 6);
        }

        [Test]
        public void MethodStyle_LEFT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("'abcdef'.LEFT(3)", "");
            Assert.AreEqual(t, "abc");

            t = engine.TryEvaluate("'abcdef'.LEFT()", "");
            Assert.AreEqual(t, "a");
        }

        [Test]
        public void MethodStyle_RIGHT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("'abcdef'.RIGHT(3)", "");
            Assert.AreEqual(t, "def");

            t = engine.TryEvaluate("'abcdef'.RIGHT()", "");
            Assert.AreEqual(t, "f");
        }

        [Test]
        public void MethodStyle_MID_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("'abcdef'.MID(2,3)", "");
            Assert.AreEqual(t, "bcd");
        }

        [Test]
        public void MethodStyle_LOWER_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("'ABC'.LOWER()", "");
            Assert.AreEqual(t, "abc");
        }

        [Test]
        public void MethodStyle_UPPER_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("'abc'.UPPER()", "");
            Assert.AreEqual(t, "ABC");
        }

        [Test]
        public void MethodStyle_TRIM_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("'  abc  '.TRIM()", "");
            Assert.AreEqual(t, "abc");
        }


        [Test]
        public void MethodStyle_REPLACE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("'abcdef'.REPLACE(2,3,'x')", "");
            Assert.AreEqual(t, "axef");

            t = engine.TryEvaluate("'abcdef'.REPLACE('bc','x')", "");
            Assert.AreEqual(t, "axdef");
        }

        [Test]
        public void MethodStyle_SUBSTITUTE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("'abcabc'.SUBSTITUTE('ab','x')", "");
            Assert.AreEqual(t, "xcxc");

            t = engine.TryEvaluate("'abcabc'.SUBSTITUTE('ab','x',2)", "");
            Assert.AreEqual(t, "abcxc");
        }

        [Test]
        public void MethodStyle_CONCATENATE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("'abc'.CONCATENATE('def')", "");
            Assert.AreEqual(t, "abcdef");

            t = engine.TryEvaluate("'abc'.CONCATENATE('def','ghi')", "");
            Assert.AreEqual(t, "abcdefghi");
        }

        [Test]
        public void MethodStyle_EXACT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("'abc'.EXACT('abc')", false);
            Assert.AreEqual(t, true);

            t = engine.TryEvaluate("'abc'.EXACT('ABC')", true);
            Assert.AreEqual(t, false);
        }

        [Test]
        public void MethodStyle_CHAR_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("49.CHAR()", "");
            Assert.AreEqual(t, "1");
        }

        [Test]
        public void MethodStyle_CODE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("'A'.CODE()", 0);
            Assert.AreEqual(t, 65);
        }

        [Test]
        public void MethodStyle_ASC_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("'ａｂｃ'.ASC()", "");
            Assert.AreEqual(t, "abc");
        }

        [Test]
        public void MethodStyle_JIS_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("'abc'.JIS()", "");
            Assert.AreEqual(t, "ａｂｃ");
        }

        [Test]
        public void MethodStyle_PROPER_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("'abc abc'.PROPER()", "");
            Assert.AreEqual(t, "Abc Abc");
        }

        [Test]
        public void MethodStyle_CLEAN_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("'ab\\r\\tc'.CLEAN()", "");
            Assert.AreEqual(t, "abc");
        }

        [Test]
        public void MethodStyle_REPT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("'ab'.REPT(3)", "");
            Assert.AreEqual(t, "ababab");
        }

        [Test]
        public void MethodStyle_T_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("'abc'.T()", "");
            Assert.AreEqual(t, "abc");

            t = engine.TryEvaluate("123.T()", "default");
            Assert.AreEqual(t, "");
        }

        [Test]
        public void MethodStyle_TEXT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("123.456.TEXT('0.00')", "");
            Assert.AreEqual(t, "123.46");
        }

        [Test]
        public void MethodStyle_VALUE_test2()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("'123'.VALUE()", 0);
            Assert.AreEqual(t, 123);
        }

        [Test]
        public void MethodStyle_RMB_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("123.45.RMB()", "");
            Assert.AreEqual(t, "壹佰贰拾叁元肆角伍分");
        }

        #endregion 方法式调用测试
    }
}