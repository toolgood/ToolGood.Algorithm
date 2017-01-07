using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaTest;

namespace ToolGood.Algorithm
{
    [TestFixture]
    class AlgorithmEngineTest_string
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
        public void CONCATENATE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("CONCATENATE('tt','33')", "");
            Assert.AreEqual(t, "tt33");
        }
        [Test]
        public void EXACT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("EXACT('tt','33')", false);
            Assert.AreEqual(t, false);
            t = engine.TryEvaluate("EXACT('tt','tt')", true);
            Assert.AreEqual(t, true);
        }
        [Test]
        public void FIND_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("FIND(\"11\",\"12221122\")", 0);
            Assert.AreEqual(t, 5);
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
        public void VALUE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var t = engine.TryEvaluate("VALUE(\"123\")", 0);
            Assert.AreEqual(t, 123);
        }


    }
}
