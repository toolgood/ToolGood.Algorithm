package test

import "testing"

// decimalMax 对应 C# StringTest.DecimalMax。
const decimalMax = "79228162514264337593543950335"

// TestStringTest 对应 C# ToolGood.Algorithm.Test.String.StringTest。
func TestStringTest(t *testing.T) {
	t.Run("ASC_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "abcABC123", evalText(e, "asc('ａｂｃＡＢＣ１２３')"))
	})

	t.Run("Jis_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "ａｂｃＡＢＣ１２３", evalText(e, "jis('abcABC123')"))
		assertText(t, "ａｂｃＡＢＣ１２３", evalText(e, "WIDECHAR('abcABC123')"))
	})

	t.Run("CHAR_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "1", evalText(e, "char(49)"))
	})

	t.Run("CLEAN_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "112", evalText(e, `clean('\r112\t')`))
	})

	t.Run("CLEAN_control_chars_test", func(t *testing.T) {
		e := newEngine()
		// 应移除所有 0~31 控制字符(修复前仅移除 \f \n \r \t \v)
		assertText(t, "abc", evalText(e, "clean(char(7)&char(8)&'abc')"))
	})

	t.Run("code_test", func(t *testing.T) {
		e := newEngine()
		assertInt(t, 49, evalInt(e, "code('1')"))
	})

	t.Run("unichar_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "A", evalText(e, "unichar(65)"))
		assertText(t, "中", evalText(e, "unichar(20013)"))
		assertText(t, "😀", evalText(e, "unichar(128512)"))
	})

	t.Run("unicode_test", func(t *testing.T) {
		e := newEngine()
		assertInt(t, 65, evalInt(e, "unicode('A')"))
		assertInt(t, 20013, evalInt(e, "unicode('中')"))
		assertInt(t, 128512, evalInt(e, "unicode('😀')"))
	})

	t.Run("CONCATENATE_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "tt33", evalText(e, "CONCATENATE('tt','33')"))
	})

	t.Run("CONCATENATE_ALIAS_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "tt33", evalText(e, "CONCAT('tt','33')"))
	})

	t.Run("EXACT_test", func(t *testing.T) {
		e := newEngine()
		assertBool(t, false, evalBoolDef(e, "EXACT('tt','33')", true))
		assertBool(t, true, evalBool(e, "EXACT('tt','tt')"))
		assertBool(t, true, evalBool(e, "EXACT('33',33)"))
		assertBool(t, true, evalBool(e, "EXACT('331.1',331.1)"))
		assertBool(t, true, evalBool(e, "EXACT('TRUE',TRUE())"))
		assertBool(t, false, evalBoolDef(e, "EXACT('1',TRUE())", true))
	})

	t.Run("FIND_test", func(t *testing.T) {
		e := newEngine()
		assertInt(t, 5, evalInt(e, `FIND("11","12221122")`))
		assertInt(t, 10, evalInt(e, `FIND("12","123456789123456789",6)`))
		assertInt(t, -1, evalIntDef(e, `FIND("bc","aaabc",5)`, -1))
		assertInt(t, 4, evalIntDef(e, `FIND("bc","aaabc",4)`, -1))
	})

	t.Run("FIXED_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "4,567.9", evalText(e, "FIXED(4567.89,1)"))
		assertText(t, "-4567.9", evalText(e, "FIXED(-4567.89, 1, TRUE())"))
		assertText(t, "77.89", evalText(e, "FIXED(77.888)"))
	})

	t.Run("FIXED_negative_digits_test", func(t *testing.T) {
		e := newEngine()
		// Excel 支持负数 decimals(向左取整),如 FIXED(1234.567,-1)="1,230"
		assertText(t, "1,230", evalText(e, "FIXED(1234.567,-1)"))
		assertText(t, "1,200", evalText(e, "FIXED(1234.567,-2)"))
		assertText(t, "1,000", evalText(e, "FIXED(1234.567,-3)"))
		assertText(t, "0", evalText(e, "FIXED(1234.567,-4)"))
		// 负数取整后保留千分位
		assertText(t, "-1,230", evalText(e, "FIXED(-1234.567,-1)"))
		// 超出 -15 报错
		evalText(e, "FIXED(1234.567,-16)")
		assertLastError(t, e)
	})

	t.Run("LEFT_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "123", evalText(e, "LEFT('123222',3)"))
		assertText(t, "123222", evalText(e, "LEFT('123222',30)"))
	})

	t.Run("LEN_test", func(t *testing.T) {
		e := newEngine()
		assertInt(t, 6, evalInt(e, "LEN('123222')"))
	})

	t.Run("LOWER_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "abc", evalText(e, "LOWER('ABC')"))
	})

	t.Run("LOWER_ALIAS_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "abc", evalText(e, "TOLOWER('ABC')"))
	})

	t.Run("MID_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "BCD", evalText(e, "MID('ABCDEF',2,3)"))
	})

	t.Run("REPLACE_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "a2d", evalText(e, `REPLACE("abccd",2,3,"2")`))
		assertText(t, "a2cd", evalText(e, `REPLACE("abccd",'bc',"2")`))
	})

	t.Run("REPLACE_num_chars_overflow_test", func(t *testing.T) {
		e := newEngine()
		// num_chars 超出剩余文本长度时,应截断到末尾而非崩溃
		assertText(t, "", evalText(e, `REPLACE("hello",1,6,"")`))
		assertText(t, "X", evalText(e, `REPLACE("hello",1,10,"X")`))
		assertText(t, "heab", evalText(e, `REPLACE("hello",3,100,"ab")`))
	})

	t.Run("REPT_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "qqq", evalText(e, `REPT("q",3)`))
	})

	t.Run("RIGHT_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "23q", evalText(e, `RIGHT("123q",3)`))
		assertText(t, "123q", evalText(e, `RIGHT("123q",30)`))
	})

	t.Run("RMB_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "壹拾贰元叁角", evalText(e, "rmb(12.3)"))
	})

	t.Run("SEARCH_test", func(t *testing.T) {
		e := newEngine()
		assertInt(t, 4, evalInt(e, `SEARCH("aa","abbAaddd")`))
	})

	t.Run("SEARCH_wildcard_test", func(t *testing.T) {
		e := newEngine()
		// ? 匹配任意单字符
		assertInt(t, 1, evalInt(e, `SEARCH("a?c","aXc")`))
		// * 匹配任意字符序列
		assertInt(t, 1, evalInt(e, `SEARCH("a*c","abbc")`))
		// ~ 转义通配符字面量
		assertInt(t, 2, evalInt(e, `SEARCH("~?","a?b")`))
		// 大小写不敏感
		assertInt(t, 3, evalInt(e, `SEARCH("ABC","xxabc")`))
	})

	t.Run("SEARCH_param_test", func(t *testing.T) {
		e := newEngine()
		// Excel SEARCH 要求 2~3 个参数(find_text, within_text, [start_num])
		assertInt(t, 2, evalInt(e, "SEARCH('a','banana')"))
		assertInt(t, 4, evalInt(e, "SEARCH('a','banana',3)"))
		// 单参数解析阶段即报错(修复前误允许 1~2 个参数)
		evalInt(e, "SEARCH('a')")
		assertLastError(t, e)
	})

	t.Run("SUBSTITUTE_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "1212cc", evalText(e, `SUBSTITUTE("ababcc","ab","12")`))
		assertText(t, "ab12cc", evalText(e, `SUBSTITUTE("ababcc","ab","12",2)`))
		assertText(t, "1111111111111111111111456789", evalText(e, `SUBSTITUTE("123456789","123","1111111111111111111111")`))
		assertText(t, "123456789", evalText(e, `SUBSTITUTE("123456789","1239","1111111111111111111111")`))
		assertText(t, "123456781111111111111111111111", evalText(e, `SUBSTITUTE("123456789","9","1111111111111111111111")`))
	})

	t.Run("T_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "", evalText(e, "T(12)"))
		assertText(t, "123", evalText(e, "T('123')"))
	})

	t.Run("TEXT_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "123.00", evalText(e, `TEXT(123,"0.00")`))
	})

	t.Run("TRIM_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "123 123", evalText(e, `TRIM(" 123 123 ")`))
	})

	t.Run("TRIM_ExcelCompatible_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "abc", evalText(e, `TRIM("  abc  ")`))
		assertText(t, "a b c", evalText(e, `TRIM("a   b   c")`))
		assertText(t, "hello world", evalText(e, `TRIM("  hello   world  ")`))
		assertText(t, "no_spaces", evalText(e, `TRIM("no_spaces")`))
		assertText(t, "", evalText(e, `TRIM("   ")`))
	})

	t.Run("PROPER_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "Abc Abc", evalText(e, "PROPER('abc abc')"))
	})

	t.Run("PROPER_ExcelCompatible_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "Hello World", evalText(e, "PROPER('hello world')"))
		assertText(t, "Hello World", evalText(e, "PROPER('HELLO WORLD')"))
		assertText(t, "Hello World", evalText(e, "PROPER('hELLO wORLD')"))
		assertText(t, "Microsoft Excel", evalText(e, "PROPER('mICROSOFT eXCEL')"))
		assertText(t, "Apple", evalText(e, "PROPER('aPpLe')"))
		assertText(t, "Test123Test", evalText(e, "PROPER('test123test')"))
	})

	t.Run("UPPER_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "ABC", evalText(e, `UPPER("abc")`))
	})

	t.Run("UPPER_ALIAS_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "ABC", evalText(e, "TOUPPER('abc')"))
	})

	t.Run("VALUE_test", func(t *testing.T) {
		e := newEngine()
		assertInt(t, 123, evalInt(e, `VALUE("123")`))
	})

	t.Run("MethodStyle_LEN_test", func(t *testing.T) {
		e := newEngine()
		assertInt(t, 6, evalInt(e, "'abcdef'.LEN()"))
	})

	t.Run("MethodStyle_LEFT_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "abc", evalText(e, "'abcdef'.LEFT(3)"))
		assertText(t, "a", evalText(e, "'abcdef'.LEFT()"))
	})

	t.Run("MethodStyle_RIGHT_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "def", evalText(e, "'abcdef'.RIGHT(3)"))
		assertText(t, "f", evalText(e, "'abcdef'.RIGHT()"))
	})

	t.Run("MethodStyle_MID_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "bcd", evalText(e, "'abcdef'.MID(2,3)"))
	})

	t.Run("MethodStyle_LOWER_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "abc", evalText(e, "'ABC'.LOWER()"))
	})

	t.Run("MethodStyle_UPPER_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "ABC", evalText(e, "'abc'.UPPER()"))
	})

	t.Run("MethodStyle_TRIM_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "abc", evalText(e, "'  abc  '.TRIM()"))
	})

	t.Run("MethodStyle_REPLACE_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "axef", evalText(e, "'abcdef'.REPLACE(2,3,'x')"))
		assertText(t, "axdef", evalText(e, "'abcdef'.REPLACE('bc','x')"))
	})

	t.Run("MethodStyle_EXACT_test", func(t *testing.T) {
		e := newEngine()
		assertBool(t, true, evalBool(e, "'abc'.EXACT('abc')"))
		assertBool(t, false, evalBoolDef(e, "'abc'.EXACT('ABC')", true))
	})

	t.Run("MethodStyle_T_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "abc", evalText(e, "'abc'.T()"))
		assertText(t, "", evalTextDef(e, "123.T()", "default"))
	})

	t.Run("MethodStyle_TEXT_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "123.46", evalText(e, "123.456.TEXT('0.00')"))
	})

	t.Run("MethodStyle_VALUE_test2", func(t *testing.T) {
		e := newEngine()
		assertInt(t, 123, evalInt(e, "'123'.VALUE()"))
	})

	t.Run("MethodStyle_RMB_test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "壹佰贰拾叁元肆角伍分", evalText(e, "123.45.RMB()"))
	})

	t.Run("EmptyString_Test", func(t *testing.T) {
		e := newEngine()
		assertInt(t, 0, evalInt(e, "LEN('')"))
		assertText(t, "", evalText(e, "LEFT('', 1)"))
		assertText(t, "", evalText(e, "RIGHT('', 1)"))
		assertText(t, "", evalText(e, "MID('', 1, 1)"))
	})

	t.Run("UnicodeBoundary_Test", func(t *testing.T) {
		e := newEngine()
		assertInt(t, 2, evalInt(e, "LEN('中文')"))
		assertText(t, "中文", evalText(e, "LEFT('中文测试', 2)"))
		assertText(t, "中文", evalText(e, "MID('中文测试', 1, 2)"))
	})

	t.Run("SubstringBoundary_Test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "ABC", evalText(e, "LEFT('ABC', 100)"))
		assertText(t, "ABC", evalText(e, "RIGHT('ABC', 100)"))
		assertText(t, "ABC", evalText(e, "MID('ABC', 1, 100)"))
		assertText(t, "", evalText(e, "MID('ABC', 5, 2)"))
	})

	t.Run("SubstituteBoundary_Test", func(t *testing.T) {
		e := newEngine()
		assertText(t, "bbb", evalText(e, "SUBSTITUTE('aaa', 'a', 'b')"))
		assertText(t, "aaa", evalText(e, "SUBSTITUTE('aaa', 'x', 'b')"))
		assertText(t, "babaa", evalText(e, "SUBSTITUTE('aabaa', 'a', 'b', 1)"))
		assertText(t, "abbaa", evalText(e, "SUBSTITUTE('aabaa', 'a', 'b', 2)"))
		// old_text 为空字符串时返回原文本
		assertText(t, "abc", evalText(e, "SUBSTITUTE('abc', '', 'x')"))
		// instance_num 小于 1 时返回 #VALUE! 错误
		assertText(t, "", evalText(e, "SUBSTITUTE('abc', 'a', 'x', 0)"))
		assertLastError(t, e)
	})

	t.Run("IntOverflowBoundary_Test", func(t *testing.T) {
		e := newEngine()
		// 参数超出 int 范围时应返回参数错误,而不是抛出 OverflowException
		expressions := []string{
			"LEFT('abc', " + decimalMax + ")",
			"RIGHT('abc', " + decimalMax + ")",
			"MID('abc', 1, " + decimalMax + ")",
			"MID('abc', " + decimalMax + ", 1)",
			"REPT('a', " + decimalMax + ")",
			"SUBSTITUTE('abc', 'b', 'x', " + decimalMax + ")",
			"REPLACE('abc', 1, " + decimalMax + ", 'x')",
			"REPLACE('abc', " + decimalMax + ", 1, 'x')",
			"FIND('a', 'abc', " + decimalMax + ")",
			"SEARCH('a', 'abc', " + decimalMax + ")",
			"CHAR(" + decimalMax + ")",
			"UNICHAR(" + decimalMax + ")",
		}
		for _, expression := range expressions {
			got := evalOperand(e, expression)
			assertTrue(t, got.IsError(), "%s: want error, got %s", expression, dumpOperand(got))
		}
	})

	t.Run("MidLengthMaxValue_Test", func(t *testing.T) {
		e := newEngine()
		// length 为 int.MaxValue 时 start_index + length 不得溢出为负数
		assertText(t, "b", evalText(e, "MID('ab', 2, 2147483647)"))
		assertText(t, "f", evalText(e, "MID('abcdef', 6, 2147483647)"))
	})

	t.Run("SurrogateBoundary_Test", func(t *testing.T) {
		e := newEngine()
		// 差异：C# 的 string 能保存孤立代理项，UNICODE 遇到时返回参数错误；
		// Go 的 string(rune) 无法表示 0xD800~0xDFFF，CHAR 会得到 U+FFFD，
		// 故此处返回替换字符的码点 65533（该偏差已在 Function_CHAR.go 中注明）。
		assertInt(t, 65533, evalInt(e, "UNICODE(CHAR(55296))"))
		assertInt(t, 65533, evalInt(e, "UNICODE(CHAR(56320))"))

		assertInt(t, 65, evalInt(e, "UNICODE(CHAR(65))"))
		// 合法代理对应返回完整码点
		assertInt(t, 128512, evalInt(e, "UNICODE(UNICHAR(128512))"))
	})

	t.Run("TrimWhitespace_Test", func(t *testing.T) {
		e := newEngine()
		// Excel 的 TRIM 只处理半角空格,Tab 等空白字符保留
		assertInt(t, 3, evalInt(e, "LEN(TRIM(CHAR(9) & 'a' & CHAR(9)))"))
		assertInt(t, 3, evalInt(e, "LEN(TRIM('  a  b  '))"))
		assertInt(t, 3, evalInt(e, "LEN(TRIM(CHAR(13) & 'a' & CHAR(10)))"))
	})

	t.Run("ProperCulture_Test", func(t *testing.T) {
		e := newEngine()
		// PROPER 使用 Invariant 转换,不受当前区域性影响（Go 无 CurrentCulture）
		assertText(t, "Istanbul", evalText(e, "PROPER('istanbul')"))
	})
}
