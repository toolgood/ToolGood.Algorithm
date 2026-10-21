"""Tests ported from ``java/.../AlgorithmEngine/CsharpTest.java``.

One Java ``@Test`` maps to one pytest function (snake_case).

Java ``TryEvaluate(exp, (String)null)`` 对应 ``try_evaluate_text(exp, None)``；
``TryEvaluate(exp, "")`` 对应 ``try_evaluate_text(exp, "")``。
"""
from __future__ import annotations

from toolgood_algorithm import AlgorithmEngine


def test_regex():
    # Java: CsharpTest.java:12
    engine = AlgorithmEngine()
    engine.set_use_excel_index(False)
    r = engine.try_evaluate_text("Regex('abcd','a.*c')", None)
    assert r == "abc"


def test_regexreplace():
    # Java: CsharpTest.java:20
    engine = AlgorithmEngine()
    r = engine.try_evaluate_text("REGEXREPLACE('abc123def', '\\\\d+', 'X')", "")
    assert r == "abcXdef"

    r = engine.try_evaluate_text("REGEXREPLACE('hello world', 'world', 'there')", "")
    assert r == "hello there"

    r = engine.try_evaluate_text("REGEXREPLACE('123-456-789', '-', '')", "")
    assert r == "123456789"


def test_method_style_regex():
    # Java: CsharpTest.java:33
    engine = AlgorithmEngine()
    r = engine.try_evaluate_text("'abcd'.REGEX('a.*c')", None)
    assert r == "abc"


def test_method_style_regexreplace():
    # Java: CsharpTest.java:40
    engine = AlgorithmEngine()
    r = engine.try_evaluate_text("'abc123def'.REGEXREPLACE('\\\\d+', 'X')", "")
    assert r == "abcXdef"


def test_method_style_isregex():
    # Java: CsharpTest.java:47
    engine = AlgorithmEngine()
    r = engine.try_evaluate("'abcd'.ISREGEX('a.*c')", False)
    assert r is True

    r = engine.try_evaluate("'abcd'.ISREGEX('x.*z')", True)
    assert r is False


def test_is_regex():
    # Java: CsharpTest.java:57
    engine = AlgorithmEngine()
    r = engine.try_evaluate("IsRegex('abcd','a.*c')", False)
    assert r is True
    r = engine.try_evaluate("IsRegex('abcd','da.*c')", True)
    assert r is False


def test_is_regex_alias():
    # Java: CsharpTest.java:66
    engine = AlgorithmEngine()
    r = engine.try_evaluate("ISMATCH('abcd','a.*c')", False)
    assert r is True
    r = engine.try_evaluate("ISMATCH('abcd','da.*c')", True)
    assert r is False


def test_index_of():
    # Java: CsharpTest.java:75
    engine = AlgorithmEngine()
    engine.set_use_excel_index(False)
    dt = engine.try_evaluate("IndexOf('abcd','cd')", -1)
    assert dt == 2
    dt = engine.try_evaluate("LastIndexOf('abcd','cd')", -1)
    assert dt == 2


def test_split():
    # Java: CsharpTest.java:85
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("Split('1,2,3,4',',')[3]", None)
    assert dt == "3"


def test_trim_start():
    # Java: CsharpTest.java:92
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("TrimStart(' 123 ')", None)
    assert dt == "123 "

    dt = engine.try_evaluate_text("TrimStart(' 123 ',' 1')", None)
    assert dt == "23 "


def test_trim_start_alias():
    # Java: CsharpTest.java:102
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("LTRIM(' 123 ')", None)
    assert dt == "123 "

    dt = engine.try_evaluate_text("LTRIM(' 123 ',' 1')", None)
    assert dt == "23 "


def test_trim_end():
    # Java: CsharpTest.java:112
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("TrimEnd(' 123 ')", None)
    assert dt == " 123"

    dt = engine.try_evaluate_text("TrimEnd(' 123 ','3 ')", None)
    assert dt == " 12"


def test_trim_end_alias():
    # Java: CsharpTest.java:122
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("RTRIM(' 123 ')", None)
    assert dt == " 123"

    dt = engine.try_evaluate_text("RTRIM(' 123 ','3 ')", None)
    assert dt == " 12"


def test_join():
    # Java: CsharpTest.java:132
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("Join(',',1,2,5,6)", None)
    assert dt == "1,2,5,6"
    dt = engine.try_evaluate_text("Join(',',1,2,5,6,split('7,8,9',','))", None)
    assert dt == "1,2,5,6,7,8,9"

    dt = engine.try_evaluate_text("Join(',',1,2,5,6，'tt')", None)
    assert dt == "1,2,5,6,tt"


def test_substring():
    # Java: CsharpTest.java:144
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("Substring('123456789',1,2)", None)
    assert dt == "12"


def test_starts_with():
    # Java: CsharpTest.java:151
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("StartsWith('123456789','12')", False)
    assert dt is True
    dt = engine.try_evaluate("StartsWith('123456789','127')", True)
    assert dt is False


def test_ends_with():
    # Java: CsharpTest.java:160
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("EndsWith('123456789','89')", False)
    assert dt is True
    dt = engine.try_evaluate("EndsWith('123456789','127')", True)
    assert dt is False


def test_is_null_or_empty():
    # Java: CsharpTest.java:169
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("IsNullOrEmpty('')", False)
    assert dt is True
    dt = engine.try_evaluate("IsNullOrEmpty(' ')", True)
    assert dt is False


def test_is_null_or_white_space():
    # Java: CsharpTest.java:178
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("IsNullOrWhiteSpace('')", False)
    assert dt is True
    dt = engine.try_evaluate("IsNullOrWhiteSpace('   ')", False)
    assert dt is True
    dt = engine.try_evaluate("IsNullOrWhiteSpace(' f ')", True)
    assert dt is False


def test_remove_start():
    # Java: CsharpTest.java:189
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("RemoveStart('123456789','12')", None)
    assert dt == "3456789"
    dt = engine.try_evaluate_text("RemoveStart('123456789','127')", None)
    assert dt == "123456789"


def test_remove_end():
    # Java: CsharpTest.java:198
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("RemoveEnd('123456789','89')", None)
    assert dt == "1234567"
    dt = engine.try_evaluate_text("RemoveEnd('123456789','127')", None)
    assert dt == "123456789"


def test_json_object():
    # Java: CsharpTest.java:207
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text(
        "json('{\"Name\":\"William Shakespeare\",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}').Age",
        None,
    )
    assert str(dt) == "51"
    dt = engine.try_evaluate_text(
        "json('{\"Name\":\"William Shakespeare\",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Birthday']",
        None,
    )
    assert dt == "04/26/1564 00:00:00"


def test_json_array():
    # Java: CsharpTest.java:216
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("json('[1,2,3]')[1]", None)
    assert dt == "1"


def test_json_method_chain():
    # Java: CsharpTest.java:223
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text(
        "json('{\"Name\":\"William Shakespeare   \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Name'].Trim()",
        None,
    )
    assert dt == "William Shakespeare"

    dt = engine.try_evaluate_text(
        "json('{\"Name1\":\"William Shakespeare \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Name'& 1].Trim().substring(2,3)",
        None,
    )
    assert dt == "ill"


def test_json_invalid():
    # Java: CsharpTest.java:233
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text(
        "json('12346{\"Name1\":\"William Shakespeare \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Name'& 1].Trim().substring(2,3)",
        None,
    )
    assert dt is None

    dt = engine.try_evaluate_text("json('[1,2,3,4,5,6]')[1].Trim()", None)
    assert dt == "1"

    dt = engine.try_evaluate_text("json('[1,2,3,4,5,6]22')[1].Trim()", None)
    assert dt is None

    dt = engine.try_evaluate_text("json('22[1,2,3,4,5,6]')[1].Trim()", None)
    assert dt is None


def test_json_boolean():
    # Java: CsharpTest.java:249
    engine = AlgorithmEngine()
    dt2 = engine.try_evaluate("json('{\"w11\":true}')['w11']", False)
    assert dt2 is True


def test_json_raw():
    # Java: CsharpTest.java:256
    engine = AlgorithmEngine()
    dt3 = engine.try_evaluate_text('{"w11":false}', "")
    assert str(dt3) == '{"w11":false}'

    dt3 = engine.try_evaluate_text("[1,2,3,4]", "")
    assert str(dt3) == "[1,2,3,4]"


# region 方法式调用测试 - 字符串扩展方法


def test_method_style_trimstart():
    # Java: CsharpTest.java:268
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("' 123 '.TRIMSTART()", None)
    assert dt == "123 "

    dt = engine.try_evaluate_text("' 123 '.TRIMSTART(' 1')", None)
    assert dt == "23 "


def test_method_style_trimend():
    # Java: CsharpTest.java:278
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("' 123 '.TRIMEND()", None)
    assert dt == " 123"

    dt = engine.try_evaluate_text("' 123 '.TRIMEND('3 ')", None)
    assert dt == " 12"


def test_trim_start_unicode_whitespace():
    # Java: CsharpTest.java:288
    engine = AlgorithmEngine()
    # U+00A0 不换行空格: isSpaceChar=true, 但 String.stripLeading() 只去除 <= U+0020 的空白
    # 修复前 '\u00A0abc'.TRIMSTART() 无法去除开头的不换行空格
    dt = engine.try_evaluate_text("'\u00A0abc'.TRIMSTART()", None)
    assert dt == "abc"

    dt = engine.try_evaluate_text("' \u00A0abc'.TRIMSTART()", None)
    assert dt == "abc"


def test_trim_end_unicode_whitespace():
    # Java: CsharpTest.java:300
    engine = AlgorithmEngine()
    # 同上, 修复前 '\u2003'(EM SPACE, isWhitespace=true) 无法从末尾去除
    dt = engine.try_evaluate_text("'abc\u00A0'.TRIMEND()", None)
    assert dt == "abc"

    dt = engine.try_evaluate_text("'abc\u00A0 '.TRIMEND()", None)
    assert dt == "abc"

    dt = engine.try_evaluate_text("'abc\u2003'.TRIMEND()", None)
    assert dt == "abc"


def test_method_style_indexof():
    # Java: CsharpTest.java:314
    engine = AlgorithmEngine()
    engine.set_use_excel_index(False)
    dt = engine.try_evaluate("'abcd'.INDEXOF('cd')", -1)
    assert dt == 2

    dt = engine.try_evaluate("'abcd'.INDEXOF('cd',0)", -1)
    assert dt == 2


def test_method_style_lastindexof():
    # Java: CsharpTest.java:325
    engine = AlgorithmEngine()
    engine.set_use_excel_index(False)
    dt = engine.try_evaluate("'abcdcd'.LASTINDEXOF('cd')", -1)
    assert dt == 4


def test_method_style_split():
    # Java: CsharpTest.java:333
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("'1,2,3,4'.SPLIT(',')[3]", None)
    assert dt == "3"


def test_method_style_join():
    # Java: CsharpTest.java:340
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("','.JOIN(1,2,3)", None)
    assert dt == "1,2,3"


def test_method_style_substring():
    # Java: CsharpTest.java:347
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("'123456789'.SUBSTRING(1,2)", None)
    assert dt == "12"

    dt = engine.try_evaluate_text("'123456789'.SUBSTRING(3)", None)
    assert dt == "3456789"


def test_method_style_startswith():
    # Java: CsharpTest.java:357
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("'123456789'.STARTSWITH('12')", False)
    assert dt is True

    dt = engine.try_evaluate("'123456789'.STARTSWITH('127')", True)
    assert dt is False


def test_method_style_endswith():
    # Java: CsharpTest.java:367
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("'123456789'.ENDSWITH('89')", False)
    assert dt is True

    dt = engine.try_evaluate("'123456789'.ENDSWITH('127')", True)
    assert dt is False


def test_method_style_isnullorempty():
    # Java: CsharpTest.java:377
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("''.ISNULLOREMPTY()", False)
    assert dt is True

    dt = engine.try_evaluate("' '.ISNULLOREMPTY()", True)
    assert dt is False


def test_method_style_isnullorwhitespace():
    # Java: CsharpTest.java:387
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("''.ISNULLORWHITESPACE()", False)
    assert dt is True

    dt = engine.try_evaluate("'   '.ISNULLORWHITESPACE()", False)
    assert dt is True

    dt = engine.try_evaluate("' f '.ISNULLORWHITESPACE()", True)
    assert dt is False


def test_method_style_removestart():
    # Java: CsharpTest.java:400
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("'123456789'.REMOVESTART('12')", None)
    assert dt == "3456789"

    dt = engine.try_evaluate_text("'123456789'.REMOVESTART('127')", None)
    assert dt == "123456789"


def test_method_style_removeend():
    # Java: CsharpTest.java:410
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("'123456789'.REMOVEEND('89')", None)
    assert dt == "1234567"

    dt = engine.try_evaluate_text("'123456789'.REMOVEEND('127')", None)
    assert dt == "123456789"


def test_method_style_json():
    # Java: CsharpTest.java:420
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("'{\"Name\":\"William Shakespeare\",\"Age\":51}'.JSON()['Age']", None)
    assert str(dt) == "51"


def test_method_style_has():
    # Java: CsharpTest.java:427
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("{\"name\":\"toolgood\",\"age\":\"12\"}.HAS('age')", False)
    assert dt is True

    dt = engine.try_evaluate("{\"name\":\"toolgood\",\"age\":\"12\"}.HASKEY('age')", False)
    assert dt is True

    dt = engine.try_evaluate("{\"name\":\"toolgood\",\"age\":\"12\"}.CONTAINS('age')", False)
    assert dt is True

    dt = engine.try_evaluate("{\"name\":\"toolgood\",\"age\":\"12\"}.CONTAINSKEY('age')", False)
    assert dt is True


def test_method_style_hasvalue():
    # Java: CsharpTest.java:443
    engine = AlgorithmEngine()
    dt = engine.try_evaluate("{\"name\":\"toolgood\",\"age\":\"12\"}.HASVALUE('toolgood')", False)
    assert dt is True

    dt = engine.try_evaluate("{\"name\":\"toolgood\",\"age\":\"12\"}.CONTAINSVALUE('toolgood')", False)
    assert dt is True


# endregion 方法式调用测试 - 字符串扩展方法


def test_look_floor():
    # Java: CsharpTest.java:455
    engine = AlgorithmEngine()

    num = engine.try_evaluate("LookFloor(2,[0,1,2,3,4])", 0)
    assert num == 2

    num = engine.try_evaluate("LookFloor(2.1,[0,1,2,3,4])", 0)
    assert num == 2

    num = engine.try_evaluate("LookFloor(-2.1,[0,1,2,3,4])", 0)
    assert num == 0

    num = engine.try_evaluate("LookFloor(5,[0,1,2,3,4])", 0)
    assert num == 4


def test_look_ceiling():
    # Java: CsharpTest.java:472
    engine = AlgorithmEngine()

    num = engine.try_evaluate("LookCeiling(2,[0,1,2,3,4])", 0)
    assert num == 2

    num = engine.try_evaluate("LookCeiling(2.1,[0,1,2,3,4])", 0)
    assert num == 3

    num = engine.try_evaluate("LookCeiling(-2.1,[0,1,2,3,4])", 0)
    assert num == 0

    num = engine.try_evaluate("LookCeiling(5,[0,1,2,3,4])", 0)
    assert num == 4
