package toolgood.algorithm.algorithmengine;

import org.junit.Test;
import static org.junit.Assert.*;

import toolgood.algorithm.AlgorithmEngine;

import toolgood.algorithm.AlgorithmEngine;

public class StringTest {

    @Test
    public void ASC_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("asc('ａｂｃＡＢＣ１２３')", "");
        assertEquals("abcABC123", t);
    }

    @Test
    public void Jis_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("jis('abcABC123')", "");
        assertEquals("ａｂｃＡＢＣ１２３", t);
        t = engine.TryEvaluate("WIDECHAR('abcABC123')", "");
        assertEquals("ａｂｃＡＢＣ１２３", t);
    }

    @Test
    public void CHAR_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("char(49)", "");
        assertEquals("1", t);
    }

    @Test
    public void CLEAN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("clean('\\r112\\t')", "");
        assertEquals("112", t);
    }

    @Test
    public void code_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int t = engine.TryEvaluate("code('1')", 0);
        assertEquals(49, t);
    }

    @Test
    public void unichar_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("unichar(65)", "");
        assertEquals("A", t);
        t = engine.TryEvaluate("unichar(20013)", "");
        assertEquals("中", t);
        t = engine.TryEvaluate("unichar(128512)", "");
        assertEquals("😀", t);
    }

    @Test
    public void unicode_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int t = engine.TryEvaluate("unicode('A')", 0);
        assertEquals(65, t);
        t = engine.TryEvaluate("unicode('中')", 0);
        assertEquals(20013, t);
        t = engine.TryEvaluate("unicode('😀')", 0);
        assertEquals(128512, t);
    }

    @Test
    public void CONCATENATE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("CONCATENATE('tt','33')", "");
        assertEquals("tt33", t);
    }

    @Test
    public void CONCATENATE_ALIAS_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("CONCAT('tt','33')", "");
        assertEquals("tt33", t);
    }

    @Test
    public void EXACT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean t = engine.TryEvaluate("EXACT('tt','33')", true);
        assertEquals(false, t);
        t = engine.TryEvaluate("EXACT('tt','tt')", false);
        assertEquals(true, t);

        t = engine.TryEvaluate("EXACT('33',33)", false);
        assertEquals(true, t);
        t = engine.TryEvaluate("EXACT('331.1',331.1)", false);
        assertEquals(true, t);
        t = engine.TryEvaluate("EXACT('TRUE',TRUE())", false);
        assertEquals(true, t);
        t = engine.TryEvaluate("EXACT('1',TRUE())", true);
        assertEquals(false, t);
    }

    @Test
    public void FIND_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int t = engine.TryEvaluate("FIND(\"11\",\"12221122\")", 0);
        assertEquals(5, t);

        t = engine.TryEvaluate("FIND(\"12\",\"123456789123456789\",6)", 0);
        assertEquals(10, t);

        t = engine.TryEvaluate("FIND(\"bc\",\"aaabc\",5)", -1);
        assertEquals(-1, t);

        t = engine.TryEvaluate("FIND(\"bc\",\"aaabc\",4)", -1);
        assertEquals(4, t);
    }

    @Test
    public void FIXED_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("FIXED(4567.89,1)", "");
        assertEquals("4,567.9", t);
        t = engine.TryEvaluate("FIXED(-4567.89, 1, TRUE())", "");
        assertEquals("-4567.9", t);
        t = engine.TryEvaluate("FIXED(77.888)", "");
        assertEquals("77.89", t);
    }

    @Test
    public void LEFT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("LEFT('123222',3)", "");
        assertEquals("123", t);

        t = engine.TryEvaluate("LEFT('123222',30)", "");
        assertEquals("123222", t);
    }

    @Test
    public void LEN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int t = engine.TryEvaluate("LEN('123222')", 0);
        assertEquals(6, t);
    }

    @Test
    public void LOWER_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("LOWER('ABC')", "");
        assertEquals("abc", t);
    }

    @Test
    public void LOWER_ALIAS_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("TOLOWER('ABC')", "");
        assertEquals("abc", t);
    }

    @Test
    public void MID_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("MID('ABCDEF',2,3)", "");
        assertEquals("BCD", t);
    }

    @Test
    public void REPLACE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("REPLACE(\"abccd\",2,3,\"2\")", "");
        assertEquals("a2d", t);

        String t1 = engine.TryEvaluate("REPLACE(\"abccd\",'bc',\"2\")", "");
        assertEquals("a2cd", t1);
    }

    @Test
    public void REPT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("REPT(\"q\",3)", "");
        assertEquals("qqq", t);
    }

    @Test
    public void RIGHT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("RIGHT(\"123q\",3)", "");
        assertEquals("23q", t);

        t = engine.TryEvaluate("RIGHT(\"123q\",30)", "");
        assertEquals("123q", t);
    }

    @Test
    public void RMB_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("rmb(12.3)", "");
        assertEquals("壹拾贰元叁角", t);
    }

    @Test
    public void SEARCH_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int t = engine.TryEvaluate("SEARCH(\"aa\",\"abbAaddd\")", 0);
        assertEquals(4, t);
    }

    @Test
    public void SUBSTITUTE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("SUBSTITUTE(\"ababcc\",\"ab\",\"12\")", "");
        assertEquals("1212cc", t);
        t = engine.TryEvaluate("SUBSTITUTE(\"ababcc\",\"ab\",\"12\",2)", "");
        assertEquals("ab12cc", t);

        t = engine.TryEvaluate("SUBSTITUTE(\"123456789\",\"123\",\"1111111111111111111111\")", "");
        assertEquals("1111111111111111111111456789", t);

        t = engine.TryEvaluate("SUBSTITUTE(\"123456789\",\"1239\",\"1111111111111111111111\")", "");
        assertEquals("123456789", t);

        t = engine.TryEvaluate("SUBSTITUTE(\"123456789\",\"9\",\"1111111111111111111111\")", "");
        assertEquals("123456781111111111111111111111", t);
    }

    @Test
    public void T_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("T(12)", "");
        assertEquals("", t);
        t = engine.TryEvaluate("T('123')", "");
        assertEquals("123", t);
    }

    @Test
    public void TEXT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("TEXT(123,\"0.00\")", "");
        assertEquals("123.00", t);
    }

    @Test
    public void TRIM_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("TRIM(\" 123 123 \")", "");
        assertEquals("123 123", t);
    }

    @Test
    public void TRIM_ExcelCompatible_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("TRIM(\"  abc  \")", "");
        assertEquals("abc", t);

        t = engine.TryEvaluate("TRIM(\"a   b   c\")", "");
        assertEquals("a b c", t);

        t = engine.TryEvaluate("TRIM(\"  hello   world  \")", "");
        assertEquals("hello world", t);

        t = engine.TryEvaluate("TRIM(\"no_spaces\")", "");
        assertEquals("no_spaces", t);

        t = engine.TryEvaluate("TRIM(\"   \")", "");
        assertEquals("", t);
    }

    @Test
    public void PROPER_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("PROPER('abc abc')", "");
        assertEquals("Abc Abc", t);
    }

    @Test
    public void PROPER_ExcelCompatible_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("PROPER('hello world')", "");
        assertEquals("Hello World", t);

        t = engine.TryEvaluate("PROPER('HELLO WORLD')", "");
        assertEquals("Hello World", t);

        t = engine.TryEvaluate("PROPER('hELLO wORLD')", "");
        assertEquals("Hello World", t);

        t = engine.TryEvaluate("PROPER('mICROSOFT eXCEL')", "");
        assertEquals("Microsoft Excel", t);

        t = engine.TryEvaluate("PROPER('aPpLe')", "");
        assertEquals("Apple", t);

        t = engine.TryEvaluate("PROPER('test123test')", "");
        assertEquals("Test123Test", t);
    }

    @Test
    public void UPPER_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("UPPER(\"abc\")", "");
        assertEquals("ABC", t);
    }

    @Test
    public void UPPER_ALIAS_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("TOUPPER('abc')", "");
        assertEquals("ABC", t);
    }

    @Test
    public void VALUE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int t = engine.TryEvaluate("VALUE(\"123\")", 0);
        assertEquals(123, t);
    }

    // 方法式调用测试

    @Test
    public void MethodStyle_LEN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int t = engine.TryEvaluate("'abcdef'.LEN()", 0);
        assertEquals(6, t);
    }

    @Test
    public void MethodStyle_LEFT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("'abcdef'.LEFT(3)", "");
        assertEquals("abc", t);

        t = engine.TryEvaluate("'abcdef'.LEFT()", "");
        assertEquals("a", t);
    }

    @Test
    public void MethodStyle_RIGHT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("'abcdef'.RIGHT(3)", "");
        assertEquals("def", t);

        t = engine.TryEvaluate("'abcdef'.RIGHT()", "");
        assertEquals("f", t);
    }

    @Test
    public void MethodStyle_MID_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("'abcdef'.MID(2,3)", "");
        assertEquals("bcd", t);
    }

    @Test
    public void MethodStyle_LOWER_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("'ABC'.LOWER()", "");
        assertEquals("abc", t);
    }

    @Test
    public void MethodStyle_UPPER_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("'abc'.UPPER()", "");
        assertEquals("ABC", t);
    }

    @Test
    public void MethodStyle_TRIM_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("'  abc  '.TRIM()", "");
        assertEquals("abc", t);
    }

    @Test
    public void MethodStyle_REPLACE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("'abcdef'.REPLACE(2,3,'x')", "");
        assertEquals("axef", t);

        t = engine.TryEvaluate("'abcdef'.REPLACE('bc','x')", "");
        assertEquals("axdef", t);
    }

    @Test
    public void MethodStyle_EXACT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean t = engine.TryEvaluate("'abc'.EXACT('abc')", false);
        assertEquals(true, t);

        t = engine.TryEvaluate("'abc'.EXACT('ABC')", true);
        assertEquals(false, t);
    }

    @Test
    public void MethodStyle_T_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("'abc'.T()", "");
        assertEquals("abc", t);

        t = engine.TryEvaluate("123.T()", "default");
        assertEquals("", t);
    }

    @Test
    public void MethodStyle_TEXT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("123.456.TEXT('0.00')", "");
        assertEquals("123.46", t);
    }

    @Test
    public void MethodStyle_VALUE_test2() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int t = engine.TryEvaluate("'123'.VALUE()", 0);
        assertEquals(123, t);
    }

    @Test
    public void MethodStyle_RMB_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("123.45.RMB()", "");
        assertEquals("壹佰贰拾叁元肆角伍分", t);
    }

    // 边界值测试

    @Test
    public void EmptyString_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int t = engine.TryEvaluate("LEN('')", 0);
        assertEquals(0, t);

        String t2 = engine.TryEvaluate("LEFT('', 1)", "");
        assertEquals("", t2);

        t2 = engine.TryEvaluate("RIGHT('', 1)", "");
        assertEquals("", t2);

        t2 = engine.TryEvaluate("MID('', 1, 1)", "");
        assertEquals("", t2);
    }

    @Test
    public void UnicodeBoundary_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        int t = engine.TryEvaluate("LEN('中文')", 0);
        assertEquals(2, t);

        String t2 = engine.TryEvaluate("LEFT('中文测试', 2)", "");
        assertEquals("中文", t2);

        t2 = engine.TryEvaluate("MID('中文测试', 1, 2)", "");
        assertEquals("中文", t2);
    }

    @Test
    public void SubstringBoundary_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("LEFT('ABC', 100)", "");
        assertEquals("ABC", t);

        String t2 = engine.TryEvaluate("RIGHT('ABC', 100)", "");
        assertEquals("ABC", t2);

        t2 = engine.TryEvaluate("MID('ABC', 1, 100)", "");
        assertEquals("ABC", t2);

        t2 = engine.TryEvaluate("MID('ABC', 5, 2)", "");
        assertEquals("", t2);
    }

    @Test
    public void SubstituteBoundary_Test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String t = engine.TryEvaluate("SUBSTITUTE('aaa', 'a', 'b')", "");
        assertEquals("bbb", t);

        String t2 = engine.TryEvaluate("SUBSTITUTE('aaa', 'x', 'b')", "");
        assertEquals("aaa", t2);

        t2 = engine.TryEvaluate("SUBSTITUTE('aabaa', 'a', 'b', 1)", "");
        assertEquals("babaa", t2);

        t2 = engine.TryEvaluate("SUBSTITUTE('aabaa', 'a', 'b', 2)", "");
        assertEquals("abbaa", t2);
    }
}
