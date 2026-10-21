// 本文件移植 csharp/ToolGood.Algorithm.Test/AlgorithmEngine/CsharpTest.cs。
package test

import (
	"testing"
)

// TestCsharpTest 对应 C# CsharpTest 测试类。
func TestCsharpTest(t *testing.T) {
	t.Run("Regex_test", func(t *testing.T) {
		engine := newEngine()
		engine.SetUseExcelIndex(false)
		assertText(t, "abc", evalText(engine, "Regex('abcd','a.*c')"))
	})

	t.Run("REGEXREPLACE_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "abcXdef", evalText(engine, `REGEXREPLACE('abc123def', '\\d+', 'X')`))
		assertText(t, "hello there", evalText(engine, "REGEXREPLACE('hello world', 'world', 'there')"))
		assertText(t, "123456789", evalText(engine, "REGEXREPLACE('123-456-789', '-', '')"))
	})

	t.Run("MethodStyle_REGEX_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "abc", evalText(engine, "'abcd'.REGEX('a.*c')"))
	})

	t.Run("MethodStyle_REGEXREPLACE_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "abcXdef", evalText(engine, `'abc123def'.REGEXREPLACE('\\d+', 'X')`))
	})

	t.Run("MethodStyle_ISREGEX_test", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, "'abcd'.ISREGEX('a.*c')"))
		assertBool(t, false, evalBoolDef(engine, "'abcd'.ISREGEX('x.*z')", true))
	})

	t.Run("IsRegex_test", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, "IsRegex('abcd','a.*c')"))
		assertBool(t, false, evalBoolDef(engine, "IsRegex('abcd','da.*c')", true))
	})

	t.Run("IsRegex_ALIAS_test", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, "ISMATCH('abcd','a.*c')"))
		assertBool(t, false, evalBoolDef(engine, "ISMATCH('abcd','da.*c')", true))
	})

	t.Run("IndexOf", func(t *testing.T) {
		engine := newEngine()
		engine.SetUseExcelIndex(false)
		assertInt(t, 2, evalIntDef(engine, "IndexOf('abcd','cd')", -1))
		assertInt(t, 2, evalIntDef(engine, "LastIndexOf('abcd','cd')", -1))
	})

	t.Run("Split", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "3", evalText(engine, "Split('1,2,3,4',',')[3]"))
	})

	t.Run("TrimStart", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "123 ", evalText(engine, "TrimStart(' 123 ')"))
		assertText(t, "23 ", evalText(engine, "TrimStart(' 123 ',' 1')"))
	})

	t.Run("TrimStart_ALIAS_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "123 ", evalText(engine, "LTRIM(' 123 ')"))
		assertText(t, "23 ", evalText(engine, "LTRIM(' 123 ',' 1')"))
	})

	t.Run("TrimEnd", func(t *testing.T) {
		engine := newEngine()
		assertText(t, " 123", evalText(engine, "TrimEnd(' 123 ')"))
		assertText(t, " 12", evalText(engine, "TrimEnd(' 123 ','3 ')"))
	})

	t.Run("TrimEnd_ALIAS_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, " 123", evalText(engine, "RTRIM(' 123 ')"))
		assertText(t, " 12", evalText(engine, "RTRIM(' 123 ','3 ')"))
	})

	t.Run("Join", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "1,2,5,6", evalText(engine, "Join(',',1,2,5,6)"))
		assertText(t, "1,2,5,6,7,8,9", evalText(engine, "Join(',',1,2,5,6,split('7,8,9',','))"))
		assertText(t, "1,2,5,6,tt", evalText(engine, "Join(',',1,2,5,6，'tt')"))
	})

	t.Run("Substring", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "12", evalText(engine, "Substring('123456789',1,2)"))
	})

	t.Run("StartsWith", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, "StartsWith('123456789','12')"))
		assertBool(t, false, evalBoolDef(engine, "StartsWith('123456789','127')", true))
	})

	t.Run("EndsWith", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, "EndsWith('123456789','89')"))
		assertBool(t, false, evalBoolDef(engine, "EndsWith('123456789','127')", true))
	})

	t.Run("IsNullOrEmpty", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, "IsNullOrEmpty('')"))
		assertBool(t, false, evalBoolDef(engine, "IsNullOrEmpty(' ')", true))
	})

	t.Run("IsNullOrWhiteSpace", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, "IsNullOrWhiteSpace('')"))
		assertBool(t, true, evalBool(engine, "IsNullOrWhiteSpace('   ')"))
		assertBool(t, false, evalBoolDef(engine, "IsNullOrWhiteSpace(' f ')", true))
	})

	t.Run("RemoveStart", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "3456789", evalText(engine, "RemoveStart('123456789','12')"))
		assertText(t, "123456789", evalText(engine, "RemoveStart('123456789','127')"))
	})

	t.Run("RemoveEnd", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "1234567", evalText(engine, "RemoveEnd('123456789','89')"))
		assertText(t, "123456789", evalText(engine, "RemoveEnd('123456789','127')"))
	})

	t.Run("Json_Object_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "51", evalText(engine, `json('{"Name":"William Shakespeare","Age":51,"Birthday":"04/26/1564 00:00:00"}').Age`))
		assertText(t, "04/26/1564 00:00:00", evalText(engine, `json('{"Name":"William Shakespeare","Age":51,"Birthday":"04/26/1564 00:00:00"}')['Birthday']`))
	})

	t.Run("Json_Array_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "1", evalText(engine, "json('[1,2,3]')[1]"))
	})

	t.Run("Json_MethodChain_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "William Shakespeare", evalText(engine, `json('{"Name":"William Shakespeare   ","Age":51,"Birthday":"04/26/1564 00:00:00"}')['Name'].Trim()`))
		assertText(t, "ill", evalText(engine, `json('{"Name1":"William Shakespeare ","Age":51,"Birthday":"04/26/1564 00:00:00"}')['Name'& 1].Trim().substring(2,3)`))
	})

	t.Run("Json_Invalid_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "", evalText(engine, `json('12346{"Name1":"William Shakespeare ","Age":51,"Birthday":"04/26/1564 00:00:00"}')['Name'& 1].Trim().substring(2,3)`))
		assertText(t, "1", evalText(engine, "json('[1,2,3,4,5,6]')[1].Trim()"))
		assertText(t, "", evalText(engine, "json('[1,2,3,4,5,6]22')[1].Trim()"))
		assertText(t, "", evalText(engine, "json('22[1,2,3,4,5,6]')[1].Trim()"))
	})

	t.Run("Json_Boolean_test", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, `json('{"w11":true}')['w11']`))
	})

	t.Run("Json_Raw_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, `{"w11":false}`, evalText(engine, `{"w11":false}`))
		assertText(t, "[1,2,3,4]", evalText(engine, "[1,2,3,4]"))
	})

	// ---------------- 方法式调用测试 - 字符串扩展方法

	t.Run("MethodStyle_TRIMSTART_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "123 ", evalText(engine, "' 123 '.TRIMSTART()"))
		assertText(t, "23 ", evalText(engine, "' 123 '.TRIMSTART(' 1')"))
	})

	t.Run("MethodStyle_TRIMEND_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, " 123", evalText(engine, "' 123 '.TRIMEND()"))
		assertText(t, " 12", evalText(engine, "' 123 '.TRIMEND('3 ')"))
	})

	t.Run("MethodStyle_INDEXOF_test", func(t *testing.T) {
		engine := newEngine()
		engine.SetUseExcelIndex(false)
		assertInt(t, 2, evalIntDef(engine, "'abcd'.INDEXOF('cd')", -1))
		assertInt(t, 2, evalIntDef(engine, "'abcd'.INDEXOF('cd',0)", -1))
	})

	t.Run("MethodStyle_LASTINDEXOF_test", func(t *testing.T) {
		engine := newEngine()
		engine.SetUseExcelIndex(false)
		assertInt(t, 4, evalIntDef(engine, "'abcdcd'.LASTINDEXOF('cd')", -1))
	})

	t.Run("MethodStyle_SPLIT_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "3", evalText(engine, "'1,2,3,4'.SPLIT(',')[3]"))
	})

	t.Run("MethodStyle_JOIN_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "1,2,3", evalText(engine, "','.JOIN(1,2,3)"))
	})

	t.Run("MethodStyle_SUBSTRING_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "12", evalText(engine, "'123456789'.SUBSTRING(1,2)"))
		assertText(t, "3456789", evalText(engine, "'123456789'.SUBSTRING(3)"))
	})

	t.Run("MethodStyle_STARTSWITH_test", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, "'123456789'.STARTSWITH('12')"))
		assertBool(t, false, evalBoolDef(engine, "'123456789'.STARTSWITH('127')", true))
	})

	t.Run("MethodStyle_ENDSWITH_test", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, "'123456789'.ENDSWITH('89')"))
		assertBool(t, false, evalBoolDef(engine, "'123456789'.ENDSWITH('127')", true))
	})

	t.Run("MethodStyle_ISNULLOREMPTY_test", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, "''.ISNULLOREMPTY()"))
		assertBool(t, false, evalBoolDef(engine, "' '.ISNULLOREMPTY()", true))
	})

	t.Run("MethodStyle_ISNULLORWHITESPACE_test", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, "''.ISNULLORWHITESPACE()"))
		assertBool(t, true, evalBool(engine, "'   '.ISNULLORWHITESPACE()"))
		assertBool(t, false, evalBoolDef(engine, "' f '.ISNULLORWHITESPACE()", true))
	})

	t.Run("MethodStyle_REMOVESTART_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "3456789", evalText(engine, "'123456789'.REMOVESTART('12')"))
		assertText(t, "123456789", evalText(engine, "'123456789'.REMOVESTART('127')"))
	})

	t.Run("MethodStyle_REMOVEEND_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "1234567", evalText(engine, "'123456789'.REMOVEEND('89')"))
		assertText(t, "123456789", evalText(engine, "'123456789'.REMOVEEND('127')"))
	})

	t.Run("MethodStyle_JSON_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "51", evalText(engine, `'{"Name":"William Shakespeare","Age":51}'.JSON()['Age']`))
	})

	t.Run("MethodStyle_HAS_test", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, `{"name":"toolgood","age":"12"}.HAS('age')`))
		assertBool(t, true, evalBool(engine, `{"name":"toolgood","age":"12"}.HASKEY('age')`))
		assertBool(t, true, evalBool(engine, `{"name":"toolgood","age":"12"}.CONTAINS('age')`))
		assertBool(t, true, evalBool(engine, `{"name":"toolgood","age":"12"}.CONTAINSKEY('age')`))
	})

	t.Run("MethodStyle_HASVALUE_test", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, `{"name":"toolgood","age":"12"}.HASVALUE('toolgood')`))
		assertBool(t, true, evalBool(engine, `{"name":"toolgood","age":"12"}.CONTAINSVALUE('toolgood')`))
	})

	t.Run("LookFloor_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 2, evalInt(engine, "LookFloor(2,[0,1,2,3,4])"))
		assertInt(t, 2, evalInt(engine, "LookFloor(2.1,[0,1,2,3,4])"))
		assertInt(t, 0, evalInt(engine, "LookFloor(-2.1,[0,1,2,3,4])"))
		assertInt(t, 4, evalInt(engine, "LookFloor(5,[0,1,2,3,4])"))
	})

	t.Run("LookCeiling_test", func(t *testing.T) {
		engine := newEngine()
		assertInt(t, 2, evalInt(engine, "LookCeiling(2,[0,1,2,3,4])"))
		assertInt(t, 3, evalInt(engine, "LookCeiling(2.1,[0,1,2,3,4])"))
		assertInt(t, 0, evalInt(engine, "LookCeiling(-2.1,[0,1,2,3,4])"))
		assertInt(t, 4, evalInt(engine, "LookCeiling(5,[0,1,2,3,4])"))
	})

	t.Run("IndexOf_not_found_test", func(t *testing.T) {
		engine := newEngine()
		engine.SetUseExcelIndex(false)
		// 修复点: 指定 startIndex 但未找到时应返回 -1, 而不是 startIndex-1
		assertInt(t, -1, evalIntDef(engine, "IndexOf('abcd','xyz',2)", -1))
		assertInt(t, 2, evalIntDef(engine, "IndexOf('abcd','cd',2)", -1))
	})

	t.Run("RemoveEnd_RemoveStart_parameter_test", func(t *testing.T) {
		engine := newEngine()
		// 3 参数 ignoreCase=true
		assertText(t, "abc", evalText(engine, "RemoveEnd('abcABC','abc',true)"))
		assertText(t, "ABC", evalText(engine, "RemoveStart('abcABC','abc',true)"))

		// 1 个参数应被参数校验拒绝, 返回默认值而非崩溃
		assertText(t, "", evalText(engine, "RemoveEnd('abc')"))
		assertText(t, "", evalText(engine, "RemoveStart('abc')"))
	})

	t.Run("HasValue_test", func(t *testing.T) {
		engine := newEngine()
		assertBool(t, true, evalBool(engine, `HasValue({"name":"toolgood","age":"12"},'toolgood')`))
		assertBool(t, true, evalBool(engine, `HasValue({"name":"toolgood","age":"12"},'12')`))
		assertBool(t, false, evalBoolDef(engine, `HasValue({"name":"toolgood","age":"12"},'nonexist')`, true))
		assertBool(t, true, evalBool(engine, "HasValue(json('[1,2,3]'),'2')"))
	})

	t.Run("LastIndexOf_with_startIndex_test", func(t *testing.T) {
		engine := newEngine()
		engine.SetUseExcelIndex(false)

		// 单字符在 startIndex 位置能被找到（修复 AsSpan off-by-one bug）
		assertInt(t, 2, evalIntDef(engine, "LastIndexOf('abc','c',2)", -1))

		// 多字符匹配的最后一个字符恰好在 startIndex 位置
		assertInt(t, 2, evalIntDef(engine, "LastIndexOf('abcd','cd',3)", -1))

		// startIndex 之前有更早的匹配
		assertInt(t, 2, evalIntDef(engine, "LastIndexOf('abcdcd','cd',3)", -1))

		// startIndex == text.Length 允许（搜索到末尾）
		assertInt(t, 4, evalIntDef(engine, "LastIndexOf('abcdcd','cd',6)", -1))

		// startIndex 窗口内无匹配（匹配延伸到 startIndex 之外）
		assertInt(t, 0, evalIntDef(engine, "LastIndexOf('ababa','aba',2)", -1))

		// 完全未找到
		assertInt(t, -1, evalIntDef(engine, "LastIndexOf('abcd','xyz',2)", -1))

		// 1-based Excel index: 参数 startIndex=3 → 0-based=2
		engine.SetUseExcelIndex(true)
		assertInt(t, 3, evalIntDef(engine, "LastIndexOf('abc','c',3)", -1))
	})

	t.Run("LastIndexOf_with_startIndex_and_count_test", func(t *testing.T) {
		engine := newEngine()
		engine.SetUseExcelIndex(false)

		// 4参数：文本、搜索串、起始位置、count —— 窗口内匹配
		assertInt(t, 3, evalIntDef(engine, "LastIndexOf('abcabcabc','abc',5,3)", -1))

		// count 覆盖整个字符串（startIndex=8, count=9 → 窗口[0,8]）
		assertInt(t, 6, evalIntDef(engine, "LastIndexOf('abcabcabc','abc',8,9)", -1))

		// count 受限，找不到更近的匹配
		assertInt(t, -1, evalIntDef(engine, "LastIndexOf('abcabcabc','abc',5,2)", -1))
	})

	t.Run("LastIndexOf_not_found_with_startIndex_test", func(t *testing.T) {
		engine := newEngine()
		engine.SetUseExcelIndex(false)

		// startIndex 窗口内没有匹配项
		assertInt(t, -1, evalIntDef(engine, "LastIndexOf('abcdef','xyz',3)", -1))
	})

	t.Run("Guid_test", func(t *testing.T) {
		engine := newEngine()
		dt := evalText(engine, "Guid()")
		assertNoError(t, engine)
		// GUID 格式: 8-4-4-4-12
		assertInt(t, 36, len(dt))
	})

	t.Run("Has_json_array_types_test", func(t *testing.T) {
		engine := newEngine()

		// JSON 数组包含 double 类型值
		assertBool(t, true, evalBool(engine, "Has(json('[1,2,3]'),'2')"))

		// JSON 数组包含 boolean 类型值
		assertBool(t, true, evalBool(engine, "Has(json('[true,false]'),'true')"))
		assertBool(t, true, evalBool(engine, "Has(json('[true,false]'),'false')"))

		// JSON 对象按键查找
		assertBool(t, true, evalBool(engine, `Has(json('{"a":1,"b":2}'),'a')`))
		assertBool(t, false, evalBool(engine, `Has(json('{"a":1,"b":2}'),'c')`))
	})

	t.Run("HasValue_json_array_types_test", func(t *testing.T) {
		engine := newEngine()

		// JSON 数组包含 double 类型值
		assertBool(t, true, evalBool(engine, "HasValue(json('[1,2,3]'),'2')"))

		// JSON 对象包含 boolean 类型值
		assertBool(t, true, evalBool(engine, `HasValue(json('{"a":true,"b":false}'),'true')`))
		assertBool(t, true, evalBool(engine, `HasValue(json('{"a":true,"b":false}'),'false')`))
	})
}
