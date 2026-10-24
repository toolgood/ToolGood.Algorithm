// 移植自 csharp/ToolGood.Algorithm.Test/AlgorithmEngine/CsharpTest.cs
//
// 说明：C# 中 `TryEvaluate(exp, null)` 绑定到 `TryEvaluate(string, string)` 重载，
// 出错时返回 null。移植版按本仓库约定用空串表示 string null，故默认值写 ""，
// 期望 null 处断言空串。
#include <string>

#include "cpptest.h"

#include "toolgood/AlgorithmEngine.h"

using namespace toolgood;

namespace {

TEST(Csharp, Regex_test) {
    AlgorithmEngine engine;
    engine.SetUseExcelIndex(false);
    auto r = engine.TryEvaluate("Regex('abcd','a.*c')", "");
    Assert::AreEqual(r, "abc");
}

TEST(Csharp, REGEXREPLACE_test) {
    AlgorithmEngine engine;
    auto r = engine.TryEvaluate("REGEXREPLACE('abc123def', '\\\\d+', 'X')", "");
    Assert::AreEqual(r, "abcXdef");

    r = engine.TryEvaluate("REGEXREPLACE('hello world', 'world', 'there')", "");
    Assert::AreEqual(r, "hello there");

    r = engine.TryEvaluate("REGEXREPLACE('123-456-789', '-', '')", "");
    Assert::AreEqual(r, "123456789");
}

TEST(Csharp, MethodStyle_REGEX_test) {
    AlgorithmEngine engine;
    auto r = engine.TryEvaluate("'abcd'.REGEX('a.*c')", "");
    Assert::AreEqual(r, "abc");
}

TEST(Csharp, MethodStyle_REGEXREPLACE_test) {
    AlgorithmEngine engine;
    auto r = engine.TryEvaluate("'abc123def'.REGEXREPLACE('\\\\d+', 'X')", "");
    Assert::AreEqual(r, "abcXdef");
}

TEST(Csharp, MethodStyle_ISREGEX_test) {
    AlgorithmEngine engine;
    auto r = engine.TryEvaluate("'abcd'.ISREGEX('a.*c')", false);
    Assert::AreEqual(r, true);

    r = engine.TryEvaluate("'abcd'.ISREGEX('x.*z')", true);
    Assert::AreEqual(r, false);
}

TEST(Csharp, IsRegex_test) {
    AlgorithmEngine engine;
    auto r = engine.TryEvaluate("IsRegex('abcd','a.*c')", false);
    Assert::AreEqual(r, true);
    r = engine.TryEvaluate("IsRegex('abcd','da.*c')", true);
    Assert::AreEqual(r, false);
}

TEST(Csharp, IsRegex_ALIAS_test) {
    AlgorithmEngine engine;
    auto r = engine.TryEvaluate("ISMATCH('abcd','a.*c')", false);
    Assert::AreEqual(r, true);
    r = engine.TryEvaluate("ISMATCH('abcd','da.*c')", true);
    Assert::AreEqual(r, false);
}

TEST(Csharp, IndexOf) {
    AlgorithmEngine engine;
    engine.SetUseExcelIndex(false);
    auto dt = engine.TryEvaluate("IndexOf('abcd','cd')", -1);
    Assert::AreEqual(dt, 2);
    dt = engine.TryEvaluate("LastIndexOf('abcd','cd')", -1);
    Assert::AreEqual(dt, 2);
}

TEST(Csharp, Split) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("Split('1,2,3,4',',')[3]", "");
    Assert::AreEqual(dt, "3");
}

TEST(Csharp, TrimStart) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("TrimStart(' 123 ')", "");
    Assert::AreEqual(dt, "123 ");

    dt = engine.TryEvaluate("TrimStart(' 123 ',' 1')", "");
    Assert::AreEqual(dt, "23 ");
}

TEST(Csharp, TrimStart_ALIAS_test) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("LTRIM(' 123 ')", "");
    Assert::AreEqual(dt, "123 ");

    dt = engine.TryEvaluate("LTRIM(' 123 ',' 1')", "");
    Assert::AreEqual(dt, "23 ");
}

TEST(Csharp, TrimEnd) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("TrimEnd(' 123 ')", "");
    Assert::AreEqual(dt, " 123");

    dt = engine.TryEvaluate("TrimEnd(' 123 ','3 ')", "");
    Assert::AreEqual(dt, " 12");
}

TEST(Csharp, TrimEnd_ALIAS_test) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("RTRIM(' 123 ')", "");
    Assert::AreEqual(dt, " 123");

    dt = engine.TryEvaluate("RTRIM(' 123 ','3 ')", "");
    Assert::AreEqual(dt, " 12");
}

TEST(Csharp, Join) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("Join(',',1,2,5,6)", "");
    Assert::AreEqual(dt, "1,2,5,6");
    dt = engine.TryEvaluate("Join(',',1,2,5,6,split('7,8,9',','))", "");
    Assert::AreEqual(dt, "1,2,5,6,7,8,9");

    dt = engine.TryEvaluate("Join(',',1,2,5,6，'tt')", "");
    Assert::AreEqual(dt, "1,2,5,6,tt");
}

TEST(Csharp, Substring) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("Substring('123456789',1,2)", "");
    Assert::AreEqual(dt, "12");
}

TEST(Csharp, StartsWith) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("StartsWith('123456789','12')", false);
    Assert::AreEqual(dt, true);
    dt = engine.TryEvaluate("StartsWith('123456789','127')", true);
    Assert::AreEqual(dt, false);
}

TEST(Csharp, EndsWith) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("EndsWith('123456789','89')", false);
    Assert::AreEqual(dt, true);
    dt = engine.TryEvaluate("EndsWith('123456789','127')", true);
    Assert::AreEqual(dt, false);
}

TEST(Csharp, IsNullOrEmpty) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("IsNullOrEmpty('')", false);
    Assert::AreEqual(dt, true);
    dt = engine.TryEvaluate("IsNullOrEmpty(' ')", true);
    Assert::AreEqual(dt, false);
}

TEST(Csharp, IsNullOrWhiteSpace) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("IsNullOrWhiteSpace('')", false);
    Assert::AreEqual(dt, true);
    dt = engine.TryEvaluate("IsNullOrWhiteSpace('   ')", false);
    Assert::AreEqual(dt, true);
    dt = engine.TryEvaluate("IsNullOrWhiteSpace(' f ')", true);
    Assert::AreEqual(dt, false);
}

TEST(Csharp, RemoveStart) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("RemoveStart('123456789','12')", "");
    Assert::AreEqual(dt, "3456789");
    dt = engine.TryEvaluate("RemoveStart('123456789','127')", "");
    Assert::AreEqual(dt, "123456789");
}

TEST(Csharp, RemoveEnd) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("RemoveEnd('123456789','89')", "");
    Assert::AreEqual(dt, "1234567");
    dt = engine.TryEvaluate("RemoveEnd('123456789','127')", "");
    Assert::AreEqual(dt, "123456789");
}

TEST(Csharp, Json_Object_test) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare\",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}').Age", "");
    Assert::AreEqual(dt, "51");
    dt = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare\",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Birthday']", "");
    Assert::AreEqual(dt, "04/26/1564 00:00:00");
}

TEST(Csharp, Json_Array_test) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("json('[1,2,3]')[1]", "");
    Assert::AreEqual(dt, "1");
}

TEST(Csharp, Json_MethodChain_test) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare   \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Name'].Trim()", "");
    Assert::AreEqual(dt, "William Shakespeare");

    dt = engine.TryEvaluate("json('{\"Name1\":\"William Shakespeare \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Name'& 1].Trim().substring(2,3)", "");
    Assert::AreEqual(dt, "ill");
}

TEST(Csharp, Json_Invalid_test) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("json('12346{\"Name1\":\"William Shakespeare \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Name'& 1].Trim().substring(2,3)", "");
    Assert::AreEqual(dt, "");

    dt = engine.TryEvaluate("json('[1,2,3,4,5,6]')[1].Trim()", "");
    Assert::AreEqual(dt, "1");

    dt = engine.TryEvaluate("json('[1,2,3,4,5,6]22')[1].Trim()", "");
    Assert::AreEqual(dt, "");

    dt = engine.TryEvaluate("json('22[1,2,3,4,5,6]')[1].Trim()", "");
    Assert::AreEqual(dt, "");
}

TEST(Csharp, Json_Boolean_test) {
    AlgorithmEngine engine;
    auto dt2 = engine.TryEvaluate("json('{\"w11\":true}')['w11']", false);
    Assert::AreEqual(dt2, true);
}

TEST(Csharp, Json_Raw_test) {
    AlgorithmEngine engine;
    auto dt3 = engine.TryEvaluate("{\"w11\":false}", "");
    Assert::AreEqual(dt3, "{\"w11\":false}");

    dt3 = engine.TryEvaluate("[1,2,3,4]", "");
    Assert::AreEqual(dt3, "[1,2,3,4]");
}

// ---------------- 方法式调用测试 - 字符串扩展方法 ----------------

TEST(Csharp, MethodStyle_TRIMSTART_test) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("' 123 '.TRIMSTART()", "");
    Assert::AreEqual(dt, "123 ");

    dt = engine.TryEvaluate("' 123 '.TRIMSTART(' 1')", "");
    Assert::AreEqual(dt, "23 ");
}

TEST(Csharp, MethodStyle_TRIMEND_test) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("' 123 '.TRIMEND()", "");
    Assert::AreEqual(dt, " 123");

    dt = engine.TryEvaluate("' 123 '.TRIMEND('3 ')", "");
    Assert::AreEqual(dt, " 12");
}

TEST(Csharp, MethodStyle_INDEXOF_test) {
    AlgorithmEngine engine;
    engine.SetUseExcelIndex(false);
    auto dt = engine.TryEvaluate("'abcd'.INDEXOF('cd')", -1);
    Assert::AreEqual(dt, 2);

    dt = engine.TryEvaluate("'abcd'.INDEXOF('cd',0)", -1);
    Assert::AreEqual(dt, 2);
}

TEST(Csharp, MethodStyle_LASTINDEXOF_test) {
    AlgorithmEngine engine;
    engine.SetUseExcelIndex(false);
    auto dt = engine.TryEvaluate("'abcdcd'.LASTINDEXOF('cd')", -1);
    Assert::AreEqual(dt, 4);
}

TEST(Csharp, MethodStyle_SPLIT_test) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("'1,2,3,4'.SPLIT(',')[3]", "");
    Assert::AreEqual(dt, "3");
}

TEST(Csharp, MethodStyle_JOIN_test) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("','.JOIN(1,2,3)", "");
    Assert::AreEqual(dt, "1,2,3");
}

TEST(Csharp, MethodStyle_SUBSTRING_test) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("'123456789'.SUBSTRING(1,2)", "");
    Assert::AreEqual(dt, "12");

    dt = engine.TryEvaluate("'123456789'.SUBSTRING(3)", "");
    Assert::AreEqual(dt, "3456789");
}

TEST(Csharp, MethodStyle_STARTSWITH_test) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("'123456789'.STARTSWITH('12')", false);
    Assert::AreEqual(dt, true);

    dt = engine.TryEvaluate("'123456789'.STARTSWITH('127')", true);
    Assert::AreEqual(dt, false);
}

TEST(Csharp, MethodStyle_ENDSWITH_test) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("'123456789'.ENDSWITH('89')", false);
    Assert::AreEqual(dt, true);

    dt = engine.TryEvaluate("'123456789'.ENDSWITH('127')", true);
    Assert::AreEqual(dt, false);
}

TEST(Csharp, MethodStyle_ISNULLOREMPTY_test) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("''.ISNULLOREMPTY()", false);
    Assert::AreEqual(dt, true);

    dt = engine.TryEvaluate("' '.ISNULLOREMPTY()", true);
    Assert::AreEqual(dt, false);
}

TEST(Csharp, MethodStyle_ISNULLORWHITESPACE_test) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("''.ISNULLORWHITESPACE()", false);
    Assert::AreEqual(dt, true);

    dt = engine.TryEvaluate("'   '.ISNULLORWHITESPACE()", false);
    Assert::AreEqual(dt, true);

    dt = engine.TryEvaluate("' f '.ISNULLORWHITESPACE()", true);
    Assert::AreEqual(dt, false);
}

TEST(Csharp, MethodStyle_REMOVESTART_test) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("'123456789'.REMOVESTART('12')", "");
    Assert::AreEqual(dt, "3456789");

    dt = engine.TryEvaluate("'123456789'.REMOVESTART('127')", "");
    Assert::AreEqual(dt, "123456789");
}

TEST(Csharp, MethodStyle_REMOVEEND_test) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("'123456789'.REMOVEEND('89')", "");
    Assert::AreEqual(dt, "1234567");

    dt = engine.TryEvaluate("'123456789'.REMOVEEND('127')", "");
    Assert::AreEqual(dt, "123456789");
}

TEST(Csharp, MethodStyle_JSON_test) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("'{\"Name\":\"William Shakespeare\",\"Age\":51}'.JSON()['Age']", "");
    Assert::AreEqual(dt, "51");
}

TEST(Csharp, MethodStyle_HAS_test) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("{\"name\":\"toolgood\",\"age\":\"12\"}.HAS('age')", false);
    Assert::AreEqual(dt, true);

    dt = engine.TryEvaluate("{\"name\":\"toolgood\",\"age\":\"12\"}.HASKEY('age')", false);
    Assert::AreEqual(dt, true);

    dt = engine.TryEvaluate("{\"name\":\"toolgood\",\"age\":\"12\"}.CONTAINS('age')", false);
    Assert::AreEqual(dt, true);

    dt = engine.TryEvaluate("{\"name\":\"toolgood\",\"age\":\"12\"}.CONTAINSKEY('age')", false);
    Assert::AreEqual(dt, true);
}

TEST(Csharp, MethodStyle_HASVALUE_test) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("{\"name\":\"toolgood\",\"age\":\"12\"}.HASVALUE('toolgood')", false);
    Assert::AreEqual(dt, true);

    dt = engine.TryEvaluate("{\"name\":\"toolgood\",\"age\":\"12\"}.CONTAINSVALUE('toolgood')", false);
    Assert::AreEqual(dt, true);
}

TEST(Csharp, LookFloor_test) {
    AlgorithmEngine engine;

    auto num = engine.TryEvaluate("LookFloor(2,[0,1,2,3,4])", 0);
    Assert::AreEqual(num, 2);

    num = engine.TryEvaluate("LookFloor(2.1,[0,1,2,3,4])", 0);
    Assert::AreEqual(num, 2);

    num = engine.TryEvaluate("LookFloor(-2.1,[0,1,2,3,4])", 0);
    Assert::AreEqual(num, 0);

    num = engine.TryEvaluate("LookFloor(5,[0,1,2,3,4])", 0);
    Assert::AreEqual(num, 4);
}

TEST(Csharp, LookCeiling_test) {
    AlgorithmEngine engine;

    auto num = engine.TryEvaluate("LookCeiling(2,[0,1,2,3,4])", 0);
    Assert::AreEqual(num, 2);

    num = engine.TryEvaluate("LookCeiling(2.1,[0,1,2,3,4])", 0);
    Assert::AreEqual(num, 3);

    num = engine.TryEvaluate("LookCeiling(-2.1,[0,1,2,3,4])", 0);
    Assert::AreEqual(num, 0);

    num = engine.TryEvaluate("LookCeiling(5,[0,1,2,3,4])", 0);
    Assert::AreEqual(num, 4);
}

TEST(Csharp, IndexOf_not_found_test) {
    AlgorithmEngine engine;
    engine.SetUseExcelIndex(false);
    // 修复点: 指定 startIndex 但未找到时应返回 -1, 而不是 startIndex-1
    auto dt = engine.TryEvaluate("IndexOf('abcd','xyz',2)", -1);
    Assert::AreEqual(dt, -1);

    dt = engine.TryEvaluate("IndexOf('abcd','cd',2)", -1);
    Assert::AreEqual(dt, 2);
}

TEST(Csharp, RemoveEnd_RemoveStart_parameter_test) {
    AlgorithmEngine engine;
    // 3 参数 ignoreCase=true
    auto dt = engine.TryEvaluate("RemoveEnd('abcABC','abc',true)", "");
    Assert::AreEqual(dt, "abc");

    dt = engine.TryEvaluate("RemoveStart('abcABC','abc',true)", "");
    Assert::AreEqual(dt, "ABC");

    // 1 个参数应被参数校验拒绝, 返回默认值而非崩溃（C# 中默认值为 null）
    dt = engine.TryEvaluate("RemoveEnd('abc')", "");
    Assert::AreEqual(dt, "");

    dt = engine.TryEvaluate("RemoveStart('abc')", "");
    Assert::AreEqual(dt, "");
}

TEST(Csharp, HasValue_test) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("HasValue({\"name\":\"toolgood\",\"age\":\"12\"},'toolgood')", false);
    Assert::AreEqual(dt, true);

    dt = engine.TryEvaluate("HasValue({\"name\":\"toolgood\",\"age\":\"12\"},'12')", false);
    Assert::AreEqual(dt, true);

    dt = engine.TryEvaluate("HasValue({\"name\":\"toolgood\",\"age\":\"12\"},'nonexist')", true);
    Assert::AreEqual(dt, false);

    dt = engine.TryEvaluate("HasValue(json('[1,2,3]'),'2')", false);
    Assert::AreEqual(dt, true);
}

TEST(Csharp, LastIndexOf_with_startIndex_test) {
    AlgorithmEngine engine;
    engine.SetUseExcelIndex(false);

    // 单字符在 startIndex 位置能被找到（修复 AsSpan off-by-one bug：
    // 原 AsSpan(0,startIndex) 缺少 startIndex 位置的字符）
    auto dt = engine.TryEvaluate("LastIndexOf('abc','c',2)", -1);
    Assert::AreEqual(dt, 2);

    // 多字符匹配的最后一个字符恰好在 startIndex 位置
    dt = engine.TryEvaluate("LastIndexOf('abcd','cd',3)", -1);
    Assert::AreEqual(dt, 2);

    // startIndex 之前有更早的匹配
    dt = engine.TryEvaluate("LastIndexOf('abcdcd','cd',3)", -1);
    Assert::AreEqual(dt, 2);

    // startIndex == text.Length 允许（搜索到末尾）
    dt = engine.TryEvaluate("LastIndexOf('abcdcd','cd',6)", -1);
    Assert::AreEqual(dt, 4);

    // startIndex 窗口内无匹配（匹配延伸到 startIndex 之外）
    dt = engine.TryEvaluate("LastIndexOf('ababa','aba',2)", -1);
    Assert::AreEqual(dt, 0);

    // 完全未找到
    dt = engine.TryEvaluate("LastIndexOf('abcd','xyz',2)", -1);
    Assert::AreEqual(dt, -1);

    // 1-based Excel index: 参数 startIndex=3 → 0-based=2
    engine.SetUseExcelIndex(true);
    dt = engine.TryEvaluate("LastIndexOf('abc','c',3)", -1);
    Assert::AreEqual(dt, 3);
}

TEST(Csharp, LastIndexOf_with_startIndex_and_count_test) {
    AlgorithmEngine engine;
    engine.SetUseExcelIndex(false);

    // 4参数：文本、搜索串、起始位置、count —— 窗口内匹配
    auto dt = engine.TryEvaluate("LastIndexOf('abcabcabc','abc',5,3)", -1);
    Assert::AreEqual(dt, 3);

    // count 覆盖整个字符串（startIndex=8, count=9 → 窗口[0,8]）
    dt = engine.TryEvaluate("LastIndexOf('abcabcabc','abc',8,9)", -1);
    Assert::AreEqual(dt, 6);

    // count 受限，找不到更近的匹配
    dt = engine.TryEvaluate("LastIndexOf('abcabcabc','abc',5,2)", -1);
    Assert::AreEqual(dt, -1);
}

TEST(Csharp, LastIndexOf_not_found_with_startIndex_test) {
    AlgorithmEngine engine;
    engine.SetUseExcelIndex(false);

    // startIndex 窗口内没有匹配项
    auto dt = engine.TryEvaluate("LastIndexOf('abcdef','xyz',3)", -1);
    Assert::AreEqual(dt, -1);
}

TEST(Csharp, Guid_test) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("Guid()", "");
    Assert::IsNotNull(dt);
    // C# 的 `dt is string` 在移植版中恒成立（std::string），此处只校验长度
    // GUID 格式: 8-4-4-4-12
    Assert::AreEqual(static_cast<int>(dt.size()), 36);
}

TEST(Csharp, Has_json_array_types_test) {
    AlgorithmEngine engine;

    // JSON 数组包含 double 类型值
    auto dt = engine.TryEvaluate("Has(json('[1,2,3]'),'2')", false);
    Assert::AreEqual(dt, true);

    // JSON 数组包含 boolean 类型值
    dt = engine.TryEvaluate("Has(json('[true,false]'),'true')", false);
    Assert::AreEqual(dt, true);

    dt = engine.TryEvaluate("Has(json('[true,false]'),'false')", false);
    Assert::AreEqual(dt, true);

    // JSON 对象按键查找
    dt = engine.TryEvaluate("Has(json('{\"a\":1,\"b\":2}'),'a')", false);
    Assert::AreEqual(dt, true);

    dt = engine.TryEvaluate("Has(json('{\"a\":1,\"b\":2}'),'c')", false);
    Assert::AreEqual(dt, false);
}

TEST(Csharp, HasValue_json_array_types_test) {
    AlgorithmEngine engine;

    // JSON 数组包含 double 类型值
    auto dt = engine.TryEvaluate("HasValue(json('[1,2,3]'),'2')", false);
    Assert::AreEqual(dt, true);

    // JSON 对象包含 boolean 类型值
    dt = engine.TryEvaluate("HasValue(json('{\"a\":true,\"b\":false}'),'true')", false);
    Assert::AreEqual(dt, true);

    dt = engine.TryEvaluate("HasValue(json('{\"a\":true,\"b\":false}'),'false')", false);
    Assert::AreEqual(dt, true);
}

}  // namespace
