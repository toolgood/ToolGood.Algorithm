"""Port of ``StringTest.java``."""
from __future__ import annotations

from toolgood_algorithm import AlgorithmEngine


def test_asc():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("asc('ａｂｃＡＢＣ１２３')", "")
    assert t == "abcABC123"


def test_jis():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("jis('abcABC123')", "")
    assert t == "ａｂｃＡＢＣ１２３"
    t = engine.try_evaluate("WIDECHAR('abcABC123')", "")
    assert t == "ａｂｃＡＢＣ１２３"


def test_char():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("char(49)", "")
    assert t == "1"


def test_clean():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("clean('\\r112\\t')", "")
    assert t == "112"


def test_clean_control_chars():
    engine = AlgorithmEngine()
    # 应移除所有 0~31 控制字符(修复前仅移除 \f \n \r \t \v)
    t = engine.try_evaluate("clean(char(7)&char(8)&'abc')", "")
    assert t == "abc"


def test_code():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("code('1')", 0)
    assert t == 49


def test_unichar():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("unichar(65)", "")
    assert t == "A"
    t = engine.try_evaluate("unichar(20013)", "")
    assert t == "中"
    t = engine.try_evaluate("unichar(128512)", "")
    assert t == "😀"


def test_unicode():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("unicode('A')", 0)
    assert t == 65
    t = engine.try_evaluate("unicode('中')", 0)
    assert t == 20013
    t = engine.try_evaluate("unicode('😀')", 0)
    assert t == 128512


def test_concatenate():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("CONCATENATE('tt','33')", "")
    assert t == "tt33"


def test_concatenate_alias():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("CONCAT('tt','33')", "")
    assert t == "tt33"


def test_exact():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("EXACT('tt','33')", True)
    assert t is False
    t = engine.try_evaluate("EXACT('tt','tt')", False)
    assert t is True

    t = engine.try_evaluate("EXACT('33',33)", False)
    assert t is True
    t = engine.try_evaluate("EXACT('331.1',331.1)", False)
    assert t is True
    t = engine.try_evaluate("EXACT('TRUE',TRUE())", False)
    assert t is True
    t = engine.try_evaluate("EXACT('1',TRUE())", True)
    assert t is False


def test_find():
    engine = AlgorithmEngine()
    t = engine.try_evaluate('FIND("11","12221122")', 0)
    assert t == 5

    t = engine.try_evaluate('FIND("12","123456789123456789",6)', 0)
    assert t == 10

    t = engine.try_evaluate('FIND("bc","aaabc",5)', -1)
    assert t == -1

    t = engine.try_evaluate('FIND("bc","aaabc",4)', -1)
    assert t == 4


def test_fixed():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("FIXED(4567.89,1)", "")
    assert t == "4,567.9"
    t = engine.try_evaluate("FIXED(-4567.89, 1, TRUE())", "")
    assert t == "-4567.9"
    t = engine.try_evaluate("FIXED(77.888)", "")
    assert t == "77.89"


def test_fixed_negative_digits():
    engine = AlgorithmEngine()
    # Excel 支持负数 decimals(向左取整),如 FIXED(1234.567,-1)="1,230"
    t = engine.try_evaluate("FIXED(1234.567,-1)", "")
    assert t == "1,230"

    t = engine.try_evaluate("FIXED(1234.567,-2)", "")
    assert t == "1,200"

    t = engine.try_evaluate("FIXED(1234.567,-3)", "")
    assert t == "1,000"

    t = engine.try_evaluate("FIXED(1234.567,-4)", "")
    assert t == "0"

    # 负数取整后保留千分位
    t = engine.try_evaluate("FIXED(-1234.567,-1)", "")
    assert t == "-1,230"

    # 超出 -15 报错
    t = engine.try_evaluate("FIXED(1234.567,-16)", "")
    assert engine.LastError is not None


def test_left():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("LEFT('123222',3)", "")
    assert t == "123"

    t = engine.try_evaluate("LEFT('123222',30)", "")
    assert t == "123222"


def test_len():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("LEN('123222')", 0)
    assert t == 6


def test_lower():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("LOWER('ABC')", "")
    assert t == "abc"


def test_lower_alias():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("TOLOWER('ABC')", "")
    assert t == "abc"


def test_mid():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("MID('ABCDEF',2,3)", "")
    assert t == "BCD"


def test_replace():
    engine = AlgorithmEngine()
    t = engine.try_evaluate('REPLACE("abccd",2,3,"2")', "")
    assert t == "a2d"

    t1 = engine.try_evaluate("REPLACE(\"abccd\",'bc',\"2\")", "")
    assert t1 == "a2cd"


def test_replace_num_chars_overflow():
    engine = AlgorithmEngine()
    # num_chars 超出剩余文本长度时,应截断到末尾而非崩溃
    t = engine.try_evaluate('REPLACE("hello",1,6,"")', "")
    assert t == ""

    t = engine.try_evaluate('REPLACE("hello",1,10,"X")', "")
    assert t == "X"

    t = engine.try_evaluate('REPLACE("hello",3,100,"ab")', "")
    assert t == "heab"


def test_rept():
    engine = AlgorithmEngine()
    t = engine.try_evaluate('REPT("q",3)', "")
    assert t == "qqq"


def test_right():
    engine = AlgorithmEngine()
    t = engine.try_evaluate('RIGHT("123q",3)', "")
    assert t == "23q"

    t = engine.try_evaluate('RIGHT("123q",30)', "")
    assert t == "123q"


def test_rmb():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("rmb(12.3)", "")
    assert t == "壹拾贰元叁角"


def test_search():
    engine = AlgorithmEngine()
    t = engine.try_evaluate('SEARCH("aa","abbAaddd")', 0)
    assert t == 4


def test_search_wildcard():
    engine = AlgorithmEngine()
    # ? 匹配任意单字符
    t = engine.try_evaluate('SEARCH("a?c","aXc")', 0)
    assert t == 1

    # * 匹配任意字符序列
    t = engine.try_evaluate('SEARCH("a*c","abbc")', 0)
    assert t == 1

    # ~ 转义通配符字面量
    t = engine.try_evaluate('SEARCH("~?","a?b")', 0)
    assert t == 2

    # 大小写不敏感
    t = engine.try_evaluate('SEARCH("ABC","xxabc")', 0)
    assert t == 3


def test_substitute():
    engine = AlgorithmEngine()
    t = engine.try_evaluate('SUBSTITUTE("ababcc","ab","12")', "")
    assert t == "1212cc"
    t = engine.try_evaluate('SUBSTITUTE("ababcc","ab","12",2)', "")
    assert t == "ab12cc"

    t = engine.try_evaluate('SUBSTITUTE("123456789","123","1111111111111111111111")', "")
    assert t == "1111111111111111111111456789"

    t = engine.try_evaluate('SUBSTITUTE("123456789","1239","1111111111111111111111")', "")
    assert t == "123456789"

    t = engine.try_evaluate('SUBSTITUTE("123456789","9","1111111111111111111111")', "")
    assert t == "123456781111111111111111111111"


def test_t():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("T(12)", "")
    assert t == ""
    t = engine.try_evaluate("T('123')", "")
    assert t == "123"


def test_text():
    engine = AlgorithmEngine()
    t = engine.try_evaluate('TEXT(123,"0.00")', "")
    assert t == "123.00"


def test_trim():
    engine = AlgorithmEngine()
    t = engine.try_evaluate('TRIM(" 123 123 ")', "")
    assert t == "123 123"


def test_trim_excel_compatible():
    engine = AlgorithmEngine()
    t = engine.try_evaluate('TRIM("  abc  ")', "")
    assert t == "abc"

    t = engine.try_evaluate('TRIM("a   b   c")', "")
    assert t == "a b c"

    t = engine.try_evaluate('TRIM("  hello   world  ")', "")
    assert t == "hello world"

    t = engine.try_evaluate('TRIM("no_spaces")', "")
    assert t == "no_spaces"

    t = engine.try_evaluate('TRIM("   ")', "")
    assert t == ""


def test_proper():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("PROPER('abc abc')", "")
    assert t == "Abc Abc"


def test_proper_excel_compatible():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("PROPER('hello world')", "")
    assert t == "Hello World"

    t = engine.try_evaluate("PROPER('HELLO WORLD')", "")
    assert t == "Hello World"

    t = engine.try_evaluate("PROPER('hELLO wORLD')", "")
    assert t == "Hello World"

    t = engine.try_evaluate("PROPER('mICROSOFT eXCEL')", "")
    assert t == "Microsoft Excel"

    t = engine.try_evaluate("PROPER('aPpLe')", "")
    assert t == "Apple"

    t = engine.try_evaluate("PROPER('test123test')", "")
    assert t == "Test123Test"


def test_upper():
    engine = AlgorithmEngine()
    t = engine.try_evaluate('UPPER("abc")', "")
    assert t == "ABC"


def test_upper_alias():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("TOUPPER('abc')", "")
    assert t == "ABC"


def test_value():
    engine = AlgorithmEngine()
    t = engine.try_evaluate('VALUE("123")', 0)
    assert t == 123


def test_method_style_len():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("'abcdef'.LEN()", 0)
    assert t == 6


def test_method_style_left():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("'abcdef'.LEFT(3)", "")
    assert t == "abc"

    t = engine.try_evaluate("'abcdef'.LEFT()", "")
    assert t == "a"


def test_method_style_right():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("'abcdef'.RIGHT(3)", "")
    assert t == "def"

    t = engine.try_evaluate("'abcdef'.RIGHT()", "")
    assert t == "f"


def test_method_style_mid():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("'abcdef'.MID(2,3)", "")
    assert t == "bcd"


def test_method_style_lower():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("'ABC'.LOWER()", "")
    assert t == "abc"


def test_method_style_upper():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("'abc'.UPPER()", "")
    assert t == "ABC"


def test_method_style_trim():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("'  abc  '.TRIM()", "")
    assert t == "abc"


def test_method_style_replace():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("'abcdef'.REPLACE(2,3,'x')", "")
    assert t == "axef"

    t = engine.try_evaluate("'abcdef'.REPLACE('bc','x')", "")
    assert t == "axdef"


def test_method_style_exact():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("'abc'.EXACT('abc')", False)
    assert t is True

    t = engine.try_evaluate("'abc'.EXACT('ABC')", True)
    assert t is False


def test_method_style_t():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("'abc'.T()", "")
    assert t == "abc"

    t = engine.try_evaluate("123.T()", "default")
    assert t == ""


def test_method_style_text():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("123.456.TEXT('0.00')", "")
    assert t == "123.46"


def test_method_style_value2():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("'123'.VALUE()", 0)
    assert t == 123


def test_method_style_rmb():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("123.45.RMB()", "")
    assert t == "壹佰贰拾叁元肆角伍分"


def test_empty_string():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("LEN('')", 0)
    assert t == 0

    t2 = engine.try_evaluate("LEFT('', 1)", "")
    assert t2 == ""

    t2 = engine.try_evaluate("RIGHT('', 1)", "")
    assert t2 == ""

    t2 = engine.try_evaluate("MID('', 1, 1)", "")
    assert t2 == ""


def test_unicode_boundary():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("LEN('中文')", 0)
    assert t == 2

    t2 = engine.try_evaluate("LEFT('中文测试', 2)", "")
    assert t2 == "中文"

    t2 = engine.try_evaluate("MID('中文测试', 1, 2)", "")
    assert t2 == "中文"


def test_substring_boundary():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("LEFT('ABC', 100)", "")
    assert t == "ABC"

    t2 = engine.try_evaluate("RIGHT('ABC', 100)", "")
    assert t2 == "ABC"

    t2 = engine.try_evaluate("MID('ABC', 1, 100)", "")
    assert t2 == "ABC"

    t2 = engine.try_evaluate("MID('ABC', 5, 2)", "")
    assert t2 == ""


def test_substitute_boundary():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("SUBSTITUTE('aaa', 'a', 'b')", "")
    assert t == "bbb"

    t2 = engine.try_evaluate("SUBSTITUTE('aaa', 'x', 'b')", "")
    assert t2 == "aaa"

    t2 = engine.try_evaluate("SUBSTITUTE('aabaa', 'a', 'b', 1)", "")
    assert t2 == "babaa"

    t2 = engine.try_evaluate("SUBSTITUTE('aabaa', 'a', 'b', 2)", "")
    assert t2 == "abbaa"
