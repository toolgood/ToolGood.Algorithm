package toolgood.algorithm.Tests;

import static org.junit.Assert.assertEquals;

import org.junit.Test;

import toolgood.algorithm.AlgorithmEngine;

public class AlgorithmEngineTest_string {
    @Test
        public void ASC_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            String t = engine.TryEvaluate("asc('ａｂｃＡＢＣ１２３')", "");
            assertEquals(t, "abcABC123");
        }
        @Test
        public void Jis_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            String t = engine.TryEvaluate("jis('abcABC123')", "");
            assertEquals(t, "ａｂｃＡＢＣ１２３");
            t = engine.TryEvaluate("WIDECHAR('abcABC123')", "");
            assertEquals(t, "ａｂｃＡＢＣ１２３");
        }

        @Test
        public void CHAR_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            String t = engine.TryEvaluate("char(49)", "");
            assertEquals(t, "1");
        }

        @Test
        public void CLEAN_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            String t = engine.TryEvaluate("clean('\\r112\\t')", "");
            assertEquals(t, "112");
        }
        @Test
        public void code_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            int t = engine.TryEvaluate("code('1')", 0);
            assertEquals(t, 49);
        }

        @Test
        public void CONCATENATE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            String t = engine.TryEvaluate("CONCATENATE('tt','33')", "");
            assertEquals(t, "tt33");
        }
        @Test
        public void EXACT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            boolean t = engine.TryEvaluate("EXACT('tt','33')", true);
            assertEquals(t, false);
            t = engine.TryEvaluate("EXACT('tt','tt')", false);
            assertEquals(t, true);
            t = engine.TryEvaluate("EXACT('33',33)", false);
            assertEquals(t, true);
            t = engine.TryEvaluate("EXACT('331.1',331.1)", false);
            assertEquals(t, true);
            t = engine.TryEvaluate("EXACT('TRUE',TRUE())", false);
            assertEquals(t, true);
            t = engine.TryEvaluate("EXACT('1',TRUE())", true);
            assertEquals(t, false);
        }
        @Test
        public void FIND_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            int t = engine.TryEvaluate("FIND(\"11\",\"12221122\")", 0);
            assertEquals(t, 5);
        }

        @Test
        public void FIXED_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            String t = engine.TryEvaluate("FIXED(4567.89,1)", "");
            assertEquals(t, "4,567.9");
            //t = engine.TryEvaluate(" FIXED(4567.89,-1)", "");//iserror
            //assertEquals(t, "4,570.0");
            t = engine.TryEvaluate("FIXED(-4567.89, 1, TRUE())", "");
            assertEquals(t, "-4567.9");
            t = engine.TryEvaluate("FIXED(77.888)", "");
            assertEquals(t, "77.89");
        }

        @Test
        public void LEFT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            String t = engine.TryEvaluate("LEFT('123222',3)", "");
            assertEquals(t, "123");
        }
        @Test
        public void LEN_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            int t = engine.TryEvaluate("LEN('123222')", 0);
            assertEquals(t, 6);
        }
        @Test
        public void LOWER_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            String t = engine.TryEvaluate("LOWER('ABC')", "");
            assertEquals(t, "abc");
        }
        @Test
        public void MID_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            String t = engine.TryEvaluate("MID('ABCDEF',2,3)", "");
            assertEquals(t, "BCD");
        }
        @Test
        public void PROPER_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            String t = engine.TryEvaluate("PROPER('abc abc')", "");
            assertEquals(t, "Abc Abc");
        }
        @Test
        public void REPLACE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            String t = engine.TryEvaluate("REPLACE(\"abccd\",2,3,\"2\")", "");
            assertEquals(t, "a2d");

            String t1 = engine.TryEvaluate("REPLACE(\"abccd\",'bc',\"2\")", "");
            assertEquals(t1, "a2cd");
        }
        @Test
        public void REPT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            String t = engine.TryEvaluate("REPT(\"q\",3)", "");
            assertEquals(t, "qqq");
        }
        @Test
        public void RIGHT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            String t = engine.TryEvaluate("RIGHT(\"123q\",3)", "");
            assertEquals(t, "23q");
        }
        @Test
        public void RMB_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            String t = engine.TryEvaluate("rmb(12.3)", "");
            assertEquals(t, "壹拾贰元叁角");
        }
        @Test
        public void SEARCH_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            int t = engine.TryEvaluate("SEARCH(\"aa\",\"abbAaddd\")", 0);
            assertEquals(t, 4);
        }
        @Test
        public void SUBSTITUTE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            String t = engine.TryEvaluate("SUBSTITUTE(\"ababcc\",\"ab\",\"12\")", "");
            assertEquals(t, "1212cc");
            t = engine.TryEvaluate("SUBSTITUTE(\"ababcc\",\"ab\",\"12\",2)", "");
            assertEquals(t, "ab12cc");
        }
        @Test
        public void T_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            String t = engine.TryEvaluate("T(12)", "");
            assertEquals(t, "");
            t = engine.TryEvaluate("T('123')", "");
            assertEquals(t, "123");
        }
        @Test
        public void TEXT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            String t = engine.TryEvaluate("TEXT(123,\"0.00\")", "");
            assertEquals(t, "123.00");
        }
        @Test
        public void TRIM_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            String t = engine.TryEvaluate("TRIM(\" 123 123 \")", "");
            assertEquals(t, "123 123");
        }
        @Test
        public void UPPER_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            String t = engine.TryEvaluate("UPPER(\"abc\")", "");
            assertEquals(t, "ABC");
        }

        @Test
        public void VALUE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            int t = engine.TryEvaluate("VALUE(\"123\")", 0);
            assertEquals(t, 123);
        }
}