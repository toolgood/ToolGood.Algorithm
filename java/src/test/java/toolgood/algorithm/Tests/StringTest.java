package toolgood.algorithm.Tests;

import org.junit.Test;
import toolgood.algorithm.AlgorithmEngine;

import static org.junit.Assert.assertEquals;

public class StringTest {

    @Test
    void ASC_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("asc('ａｂｃＡＢＣ１２３')", "");
        assertEquals(t, "abcABC123");
    }

    @Test
    void Jis_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("jis('abcABC123')", "");
        assertEquals(t, "ａｂｃＡＢＣ１２３");
        t = engine.TryEvaluate("WIDECHAR('abcABC123')", "");
        assertEquals(t, "ａｂｃＡＢＣ１２３");
    }

    @Test
    void CHAR_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("char(49)", "");
        assertEquals(t, "1");
    }

    @Test
    void CLEAN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("clean('\r112\t')", "");
        assertEquals(t, "112");
    }

    @Test
    void code_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("code('1')", "");
        assertEquals(t, 49);
    }

    @Test
    void unichar_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("unichar(65)", "");
        assertEquals(t, "A");
        t = engine.TryEvaluate("unichar(20013)", "");
        assertEquals(t, "中");
        t = engine.TryEvaluate("unichar(128512)", "");
        assertEquals(t, "😀");
    }

    @Test
    void unicode_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("unicode('A')", "");
        assertEquals(t, 65);
        t = engine.TryEvaluate("unicode('中')", "");
        assertEquals(t, 20013);
        t = engine.TryEvaluate("unicode('😀')", "");
        assertEquals(t, 128512);
    }

    @Test
    void CONCATENATE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("CONCATENATE('tt','33')", "");
        assertEquals(t, "tt33");
    }

    @Test
    void CONCATENATE_ALIAS_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("CONCAT('tt','33')", "");
        assertEquals(t, "tt33");
    }

    @Test
    void EXACT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("EXACT('tt','33')", "");
        assertEquals(t, false);
        t = engine.TryEvaluate("EXACT('tt','tt')", "");
        assertEquals(t, true);

        t = engine.TryEvaluate("EXACT('33',33)", "");
        assertEquals(t, true);
        t = engine.TryEvaluate("EXACT('331.1',331.1)", "");
        assertEquals(t, true);
        t = engine.TryEvaluate("EXACT('TRUE',TRUE())", "");
        assertEquals(t, true);
        t = engine.TryEvaluate("EXACT('1',TRUE())", "");
        assertEquals(t, false);
    }

    @Test
    void FIND_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("FIND(\"11\",\"12221122\")", "");
        assertEquals(t, 5);

        t = engine.TryEvaluate("FIND(\"12\",\"123456789123456789\",6)", "");
        assertEquals(t, 10);

        t = engine.TryEvaluate("FIND(\"bc\",\"aaabc\",5)", "");
        assertEquals(t, -1);

        t = engine.TryEvaluate("FIND(\"bc\",\"aaabc\",4)", "");
        assertEquals(t, 4);
    }

    @Test
    void FIXED_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("FIXED(4567.89,1)", "");
        assertEquals(t, "4,567.9");
        t = engine.TryEvaluate("FIXED(-4567.89, 1, TRUE())", "");
        assertEquals(t, "-4567.9");
        t = engine.TryEvaluate("FIXED(77.888)", "");
        assertEquals(t, "77.89");
    }

    @Test
    void LEFT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("LEFT('123222',3)", "");
        assertEquals(t, "123");
    }

    @Test
    void LEN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("LEN('123222')", "");
        assertEquals(t, 6);
    }

    @Test
    void LOWER_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("LOWER('ABC')", "");
        assertEquals(t, "abc");
    }

    @Test
    void LOWER_ALIAS_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("TOLOWER('ABC')", "");
        assertEquals(t, "abc");
    }

    @Test
    void MID_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("MID('ABCDEF',2,3)", "");
        assertEquals(t, "BCD");
    }

    @Test
    void PROPER_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("PROPER('abc abc')", "");
        assertEquals(t, "Abc Abc");
    }

    @Test
    void REPLACE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("REPLACE(\"abccd\",2,3,\"2\")", "");
        assertEquals(t, "a2d");

        Object t1 = engine.TryEvaluate("REPLACE(\"abccd\",'bc',\"2\")", "");
        assertEquals(t1, "a2cd");
    }

    @Test
    void REPT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("REPT(\"q\",3)", "");
        assertEquals(t, "qqq");
    }

    @Test
    void RIGHT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("RIGHT(\"123q\",3)", "");
        assertEquals(t, "23q");
    }

    @Test
    void RMB_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("rmb(12.3)", "");
        assertEquals(t, "壹拾贰元叁角");
    }

    @Test
    void SEARCH_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("SEARCH(\"aa\",\"abbAaddd\")", "");
        assertEquals(t, 4);
    }

    @Test
    void SUBSTITUTE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("SUBSTITUTE(\"ababcc\",\"ab\",\"12\")", "");
        assertEquals(t, "1212cc");
        t = engine.TryEvaluate("SUBSTITUTE(\"ababcc\",\"ab\",\"12\",2)", "");
        assertEquals(t, "ab12cc");

        t = engine.TryEvaluate("SUBSTITUTE(\"123456789\",\"123\",\"1111111111111111111111\")", "");
        assertEquals(t, "1111111111111111111111456789");

        t = engine.TryEvaluate("SUBSTITUTE(\"123456789\",\"1239\",\"1111111111111111111111\")", "");
        assertEquals(t, "123456789");

        t = engine.TryEvaluate("SUBSTITUTE(\"123456789\",\"9\",\"1111111111111111111111\")", "");
        assertEquals(t, "123456781111111111111111111111");
    }

    @Test
    void T_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("T(12)", "");
        assertEquals(t, "");
        t = engine.TryEvaluate("T('123')", "");
        assertEquals(t, "123");
    }

    @Test
    void TEXT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("TEXT(123,\"0.00\")", "");
        assertEquals(t, "123.00");
    }

    @Test
    void TRIM_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("TRIM(\" 123 123 \")", "");
        assertEquals(t, "123 123");
    }

    @Test
    void UPPER_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("UPPER(\"abc\")", "");
        assertEquals(t, "ABC");
    }

    @Test
    void UPPER_ALIAS_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("TOUPPER('abc')", "");
        assertEquals(t, "ABC");
    }

    @Test
    void VALUE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("VALUE(\"123\")", "");
        assertEquals(t, 123);
    }

    // 方法式调用测试
    @Test
    void MethodStyle_LEN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("'abcdef'.LEN()", "");
        assertEquals(t, 6);
    }

    @Test
    void MethodStyle_LEFT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("'abcdef'.LEFT(3)", "");
        assertEquals(t, "abc");

        t = engine.TryEvaluate("'abcdef'.LEFT()", "");
        assertEquals(t, "a");
    }

    @Test
    void MethodStyle_RIGHT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("'abcdef'.RIGHT(3)", "");
        assertEquals(t, "def");

        t = engine.TryEvaluate("'abcdef'.RIGHT()", "");
        assertEquals(t, "f");
    }

    @Test
    void MethodStyle_MID_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("'abcdef'.MID(2,3)", "");
        assertEquals(t, "bcd");
    }

    @Test
    void MethodStyle_LOWER_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("'ABC'.LOWER()", "");
        assertEquals(t, "abc");
    }

    @Test
    void MethodStyle_UPPER_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("'abc'.UPPER()", "");
        assertEquals(t, "ABC");
    }

    @Test
    void MethodStyle_TRIM_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("'  abc  '.TRIM()", "");
        assertEquals(t, "abc");
    }

    @Test
    void MethodStyle_REPLACE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("'abcdef'.REPLACE(2,3,'x')", "");
        assertEquals(t, "axef");

        t = engine.TryEvaluate("'abcdef'.REPLACE('bc','x')", "");
        assertEquals(t, "axdef");
    }

    @Test
    void MethodStyle_EXACT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("'abc'.EXACT('abc')", "");
        assertEquals(t, true);

        t = engine.TryEvaluate("'abc'.EXACT('ABC')", "");
        assertEquals(t, false);
    }

    @Test
    void MethodStyle_T_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("'abc'.T()", "");
        assertEquals(t, "abc");

        t = engine.TryEvaluate("123.T()", "");
        assertEquals(t, "");
    }

    @Test
    void MethodStyle_TEXT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("123.456.TEXT('0.00')", "");
        assertEquals(t, "123.46");
    }

    @Test
    void MethodStyle_VALUE_test2() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("'123'.VALUE()", "");
        assertEquals(t, 123);
    }

    @Test
    void MethodStyle_RMB_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("123.45.RMB()", "");
        assertEquals(t, "壹佰贰拾叁元肆角伍分");
    }

    // 边界值测试
    @Test
    void EmptyString_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("LEN('')", "");
        assertEquals(t, 0);

        Object t2 = engine.TryEvaluate("LEFT('', 1)", "");
        assertEquals(t2, "");

        t2 = engine.TryEvaluate("RIGHT('', 1)", "");
        assertEquals(t2, "");

        t2 = engine.TryEvaluate("MID('', 1, 1)", "");
        assertEquals(t2, "");
    }

    @Test
    void UnicodeBoundary_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("LEN('中文')", "");
        assertEquals(t, 2);

        Object t2 = engine.TryEvaluate("LEFT('中文测试', 2)", "");
        assertEquals(t2, "中文");

        t2 = engine.TryEvaluate("MID('中文测试', 1, 2)", "");
        assertEquals(t2, "中文");
    }

    @Test
    void SubstringBoundary_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("LEFT('ABC', 100)", "");
        assertEquals(t, "ABC");

        Object t2 = engine.TryEvaluate("RIGHT('ABC', 100)", "");
        assertEquals(t2, "ABC");

        t2 = engine.TryEvaluate("MID('ABC', 1, 100)", "");
        assertEquals(t2, "ABC");

        t2 = engine.TryEvaluate("MID('ABC', 5, 2)", "");
        assertEquals(t2, "");
    }

    @Test
    void SubstituteBoundary_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("SUBSTITUTE('aaa', 'a', 'b')", "");
        assertEquals(t, "bbb");

        Object t2 = engine.TryEvaluate("SUBSTITUTE('aaa', 'x', 'b')", "");
        assertEquals(t2, "aaa");

        t2 = engine.TryEvaluate("SUBSTITUTE('aabaa', 'a', 'b', 1)", "");
        assertEquals(t2, "babaa");

        t2 = engine.TryEvaluate("SUBSTITUTE('aabaa', 'a', 'b', 2)", "");
        assertEquals(t2, "abbaa");
    }
}
