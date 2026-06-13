package toolgood.algorithm.AlgorithmEngine;

import org.junit.Test;
import static org.junit.Assert.*;

import toolgood.algorithm.AlgorithmEngine;

public class CsharpTest {
    @Test
    public void Regex_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.SetUseExcelIndex(false);
        String r = engine.TryEvaluate("Regex('abcd','a.*c')", (String)null);
        assertEquals("abc", r);
    }

    @Test
    public void REGEXREPLACE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String r = engine.TryEvaluate("REGEXREPLACE('abc123def', '\\\\d+', 'X')", "");
        assertEquals("abcXdef", r);

        r = engine.TryEvaluate("REGEXREPLACE('hello world', 'world', 'there')", "");
        assertEquals("hello there", r);

        r = engine.TryEvaluate("REGEXREPLACE('123-456-789', '-', '')", "");
        assertEquals("123456789", r);
    }

    @Test
    public void MethodStyle_REGEX_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String r = engine.TryEvaluate("'abcd'.REGEX('a.*c')", (String)null);
        assertEquals("abc", r);
    }

    @Test
    public void MethodStyle_REGEXREPLACE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String r = engine.TryEvaluate("'abc123def'.REGEXREPLACE('\\\\d+', 'X')", "");
        assertEquals("abcXdef", r);
    }

    @Test
    public void MethodStyle_ISREGEX_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean r = engine.TryEvaluate("'abcd'.ISREGEX('a.*c')", false);
        assertEquals(true, r);

        r = engine.TryEvaluate("'abcd'.ISREGEX('x.*z')", true);
        assertEquals(false, r);
    }

    @Test
    public void IsRegex_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean r = engine.TryEvaluate("IsRegex('abcd','a.*c')", false);
        assertEquals(true, r);
        r = engine.TryEvaluate("IsRegex('abcd','da.*c')", true);
        assertEquals(false, r);
    }

    @Test
    public void IsRegex_ALIAS_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean r = engine.TryEvaluate("ISMATCH('abcd','a.*c')", false);
        assertEquals(true, r);
        r = engine.TryEvaluate("ISMATCH('abcd','da.*c')", true);
        assertEquals(false, r);
    }

    @Test
    public void IndexOf() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.SetUseExcelIndex(false);
        int dt = engine.TryEvaluate("IndexOf('abcd','cd')", -1);
        assertEquals(2, dt);
        dt = engine.TryEvaluate("LastIndexOf('abcd','cd')", -1);
        assertEquals(2, dt);
    }

    @Test
    public void Split() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("Split('1,2,3,4',',')[3]", (String)null);
        assertEquals("3", dt);
    }

    @Test
    public void TrimStart() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("TrimStart(' 123 ')", (String)null);
        assertEquals("123 ", dt);

        dt = engine.TryEvaluate("TrimStart(' 123 ',' 1')", (String)null);
        assertEquals("23 ", dt);
    }

    @Test
    public void TrimStart_ALIAS_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("LTRIM(' 123 ')", (String)null);
        assertEquals("123 ", dt);

        dt = engine.TryEvaluate("LTRIM(' 123 ',' 1')", (String)null);
        assertEquals("23 ", dt);
    }

    @Test
    public void TrimEnd() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("TrimEnd(' 123 ')", (String)null);
        assertEquals(" 123", dt);

        dt = engine.TryEvaluate("TrimEnd(' 123 ','3 ')", (String)null);
        assertEquals(" 12", dt);
    }

    @Test
    public void TrimEnd_ALIAS_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("RTRIM(' 123 ')", (String)null);
        assertEquals(" 123", dt);

        dt = engine.TryEvaluate("RTRIM(' 123 ','3 ')", (String)null);
        assertEquals(" 12", dt);
    }

    @Test
    public void Join() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("Join(',',1,2,5,6)", (String)null);
        assertEquals("1,2,5,6", dt);
        dt = engine.TryEvaluate("Join(',',1,2,5,6,split('7,8,9',','))", (String)null);
        assertEquals("1,2,5,6,7,8,9", dt);

        dt = engine.TryEvaluate("Join(',',1,2,5,6，'tt')", (String)null);
        assertEquals("1,2,5,6,tt", dt);
    }

    @Test
    public void Substring() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("Substring('123456789',1,2)", (String)null);
        assertEquals("12", dt);
    }

    @Test
    public void StartsWith() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean dt = engine.TryEvaluate("StartsWith('123456789','12')", false);
        assertEquals(true, dt);
        dt = engine.TryEvaluate("StartsWith('123456789','127')", true);
        assertEquals(false, dt);
    }

    @Test
    public void EndsWith() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean dt = engine.TryEvaluate("EndsWith('123456789','89')", false);
        assertEquals(true, dt);
        dt = engine.TryEvaluate("EndsWith('123456789','127')", true);
        assertEquals(false, dt);
    }

    @Test
    public void IsNullOrEmpty() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean dt = engine.TryEvaluate("IsNullOrEmpty('')", false);
        assertEquals(true, dt);
        dt = engine.TryEvaluate("IsNullOrEmpty(' ')", true);
        assertEquals(false, dt);
    }

    @Test
    public void IsNullOrWhiteSpace() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean dt = engine.TryEvaluate("IsNullOrWhiteSpace('')", false);
        assertEquals(true, dt);
        dt = engine.TryEvaluate("IsNullOrWhiteSpace('   ')", false);
        assertEquals(true, dt);
        dt = engine.TryEvaluate("IsNullOrWhiteSpace(' f ')", true);
        assertEquals(false, dt);
    }

    @Test
    public void RemoveStart() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("RemoveStart('123456789','12')", (String)null);
        assertEquals("3456789", dt);
        dt = engine.TryEvaluate("RemoveStart('123456789','127')", (String)null);
        assertEquals("123456789", dt);
    }

    @Test
    public void RemoveEnd() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("RemoveEnd('123456789','89')", (String)null);
        assertEquals("1234567", dt);
        dt = engine.TryEvaluate("RemoveEnd('123456789','127')", (String)null);
        assertEquals("123456789", dt);
    }

    @Test
    public void Json_Object_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        Object dt = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare\",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}').Age", (String)null);
        assertEquals("51", dt.toString());
        dt = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare\",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Birthday']", (String)null);
        assertEquals("04/26/1564 00:00:00", dt);
    }

    @Test
    public void Json_Array_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("json('[1,2,3]')[1]", (String)null);
        assertEquals("1", dt);
    }

    @Test
    public void Json_MethodChain_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare   \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Name'].Trim()", (String)null);
        assertEquals("William Shakespeare", dt);

        dt = engine.TryEvaluate("json('{\"Name1\":\"William Shakespeare \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Name'& 1].Trim().substring(2,3)", (String)null);
        assertEquals("ill", dt);
    }

    @Test
    public void Json_Invalid_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("json('12346{\"Name1\":\"William Shakespeare \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Name'& 1].Trim().substring(2,3)", (String)null);
        assertNull(dt);

        dt = engine.TryEvaluate("json('[1,2,3,4,5,6]')[1].Trim()", (String)null);
        assertEquals("1", dt);

        dt = engine.TryEvaluate("json('[1,2,3,4,5,6]22')[1].Trim()", (String)null);
        assertNull(dt);

        dt = engine.TryEvaluate("json('22[1,2,3,4,5,6]')[1].Trim()", (String)null);
        assertNull(dt);
    }

    @Test
    public void Json_Boolean_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean dt2 = engine.TryEvaluate("json('{\"w11\":true}')['w11']", false);
        assertEquals(true, dt2);
    }

    @Test
    public void Json_Raw_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt3 = engine.TryEvaluate("{\"w11\":false}", "");
        assertEquals("{\"w11\":false}", dt3.toString());

        dt3 = engine.TryEvaluate("[1,2,3,4]", "");
        assertEquals("[1,2,3,4]", dt3.toString());
    }

    // #region 方法式调用测试 - 字符串扩展方法

    @Test
    public void MethodStyle_TRIMSTART_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("' 123 '.TRIMSTART()", (String)null);
        assertEquals("123 ", dt);

        dt = engine.TryEvaluate("' 123 '.TRIMSTART(' 1')", (String)null);
        assertEquals("23 ", dt);
    }

    @Test
    public void MethodStyle_TRIMEND_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("' 123 '.TRIMEND()", (String)null);
        assertEquals(" 123", dt);

        dt = engine.TryEvaluate("' 123 '.TRIMEND('3 ')", (String)null);
        assertEquals(" 12", dt);
    }

    @Test
    public void MethodStyle_INDEXOF_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.SetUseExcelIndex(false);
        int dt = engine.TryEvaluate("'abcd'.INDEXOF('cd')", -1);
        assertEquals(2, dt);

        dt = engine.TryEvaluate("'abcd'.INDEXOF('cd',0)", -1);
        assertEquals(2, dt);
    }

    @Test
    public void MethodStyle_LASTINDEXOF_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.SetUseExcelIndex(false);
        int dt = engine.TryEvaluate("'abcdcd'.LASTINDEXOF('cd')", -1);
        assertEquals(4, dt);
    }

    @Test
    public void MethodStyle_SPLIT_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("'1,2,3,4'.SPLIT(',')[3]", (String)null);
        assertEquals("3", dt);
    }

    @Test
    public void MethodStyle_JOIN_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("','.JOIN(1,2,3)", (String)null);
        assertEquals("1,2,3", dt);
    }

    @Test
    public void MethodStyle_SUBSTRING_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("'123456789'.SUBSTRING(1,2)", (String)null);
        assertEquals("12", dt);

        dt = engine.TryEvaluate("'123456789'.SUBSTRING(3)", (String)null);
        assertEquals("3456789", dt);
    }

    @Test
    public void MethodStyle_STARTSWITH_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean dt = engine.TryEvaluate("'123456789'.STARTSWITH('12')", false);
        assertEquals(true, dt);

        dt = engine.TryEvaluate("'123456789'.STARTSWITH('127')", true);
        assertEquals(false, dt);
    }

    @Test
    public void MethodStyle_ENDSWITH_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean dt = engine.TryEvaluate("'123456789'.ENDSWITH('89')", false);
        assertEquals(true, dt);

        dt = engine.TryEvaluate("'123456789'.ENDSWITH('127')", true);
        assertEquals(false, dt);
    }

    @Test
    public void MethodStyle_ISNULLOREMPTY_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean dt = engine.TryEvaluate("''.ISNULLOREMPTY()", false);
        assertEquals(true, dt);

        dt = engine.TryEvaluate("' '.ISNULLOREMPTY()", true);
        assertEquals(false, dt);
    }

    @Test
    public void MethodStyle_ISNULLORWHITESPACE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean dt = engine.TryEvaluate("''.ISNULLORWHITESPACE()", false);
        assertEquals(true, dt);

        dt = engine.TryEvaluate("'   '.ISNULLORWHITESPACE()", false);
        assertEquals(true, dt);

        dt = engine.TryEvaluate("' f '.ISNULLORWHITESPACE()", true);
        assertEquals(false, dt);
    }

    @Test
    public void MethodStyle_REMOVESTART_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("'123456789'.REMOVESTART('12')", (String)null);
        assertEquals("3456789", dt);

        dt = engine.TryEvaluate("'123456789'.REMOVESTART('127')", (String)null);
        assertEquals("123456789", dt);
    }

    @Test
    public void MethodStyle_REMOVEEND_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("'123456789'.REMOVEEND('89')", (String)null);
        assertEquals("1234567", dt);

        dt = engine.TryEvaluate("'123456789'.REMOVEEND('127')", (String)null);
        assertEquals("123456789", dt);
    }

    @Test
    public void MethodStyle_JSON_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("'{\"Name\":\"William Shakespeare\",\"Age\":51}'.JSON()['Age']", (String)null);
        assertEquals("51", dt.toString());
    }

    @Test
    public void MethodStyle_HAS_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean dt = engine.TryEvaluate("{\"name\":\"toolgood\",\"age\":\"12\"}.HAS('age')", false);
        assertEquals(true, dt);

        dt = engine.TryEvaluate("{\"name\":\"toolgood\",\"age\":\"12\"}.HASKEY('age')", false);
        assertEquals(true, dt);

        dt = engine.TryEvaluate("{\"name\":\"toolgood\",\"age\":\"12\"}.CONTAINS('age')", false);
        assertEquals(true, dt);

        dt = engine.TryEvaluate("{\"name\":\"toolgood\",\"age\":\"12\"}.CONTAINSKEY('age')", false);
        assertEquals(true, dt);
    }

    @Test
    public void MethodStyle_HASVALUE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean dt = engine.TryEvaluate("{\"name\":\"toolgood\",\"age\":\"12\"}.HASVALUE('toolgood')", false);
        assertEquals(true, dt);

        dt = engine.TryEvaluate("{\"name\":\"toolgood\",\"age\":\"12\"}.CONTAINSVALUE('toolgood')", false);
        assertEquals(true, dt);
    }

    // #endregion 方法式调用测试 - 字符串扩展方法

    @Test
    public void LookFloor_test() {
        AlgorithmEngine engine = new AlgorithmEngine();

        int num = engine.TryEvaluate("LookFloor(2,[0,1,2,3,4])", 0);
        assertEquals(2, num);

        num = engine.TryEvaluate("LookFloor(2.1,[0,1,2,3,4])", 0);
        assertEquals(2, num);

        num = engine.TryEvaluate("LookFloor(-2.1,[0,1,2,3,4])", 0);
        assertEquals(0, num);

        num = engine.TryEvaluate("LookFloor(5,[0,1,2,3,4])", 0);
        assertEquals(4, num);
    }

    @Test
    public void LookCeiling_test() {
        AlgorithmEngine engine = new AlgorithmEngine();

        int num = engine.TryEvaluate("LookCeiling(2,[0,1,2,3,4])", 0);
        assertEquals(2, num);

        num = engine.TryEvaluate("LookCeiling(2.1,[0,1,2,3,4])", 0);
        assertEquals(3, num);

        num = engine.TryEvaluate("LookCeiling(-2.1,[0,1,2,3,4])", 0);
        assertEquals(0, num);

        num = engine.TryEvaluate("LookCeiling(5,[0,1,2,3,4])", 0);
        assertEquals(4, num);
    }
}
