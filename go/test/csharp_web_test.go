package test

import (
	"fmt"
	"testing"
)

// TestCsharpWebTest 对应 csharp/ToolGood.Algorithm.Test/AlgorithmEngine/CsharpWebTest.cs。
func TestCsharpWebTest(t *testing.T) {
	t.Run("UrlDecode", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "&=我中国人 >||", evalText(engine, "UrlDecode('%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c')"))
	})

	t.Run("UrlEncode", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c", evalText(engine, "UrlEncode('&=我中国人 >||')"))
	})

	t.Run("HtmlEncode", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "&amp;=我中国人 &gt;||", evalText(engine, "HtmlEncode('&=我中国人 >||')"))
	})

	t.Run("HtmlDecode", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "&=我中国人 >||", evalText(engine, "HtmlDecode('&amp;=我中国人 &gt;||')"))
	})

	t.Run("TextToBase64", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "Jj3miJHkuK3lm73kurogPnx8", evalText(engine, "TextToBase64('&=我中国人 >||')"))
	})

	t.Run("TextToBase64Url", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "Jj3miJHkuK3lm73kurogPnx8", evalText(engine, "TextToBase64Url('&=我中国人 >||')"))
	})

	t.Run("Base64ToText", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "&=我中国人 >||", evalText(engine, "Base64ToText('Jj3miJHkuK3lm73kurogPnx8')"))
	})

	t.Run("Base64UrlToText", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "&=我中国人 >||", evalText(engine, "Base64UrlToText('Jj3miJHkuK3lm73kurogPnx8')"))
	})

	// ------------------------------------------------------------------ 方法式调用测试 - 编码类

	t.Run("MethodStyle_URLENCODE_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c", evalText(engine, "'&=我中国人 >||'.URLENCODE()"))
	})

	t.Run("MethodStyle_URLDECODE_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "&=我中国人 >||", evalText(engine, "'%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c'.URLDECODE()"))
	})

	// ------------------------------------------------------------------ 边界行为测试

	t.Run("ZeroArgument_ThrowsFormatException", func(t *testing.T) {
		// 差异：C# 在 ANTLR 解析期抛 FormatException；Go 用 Parse 返回 error 表达同一行为。
		for _, exp := range []string{
			"UrlEncode()", "UrlDecode()", "HtmlEncode()", "HtmlDecode()",
			"TextToBase64()", "TextToBase64Url()", "Base64ToText()", "Base64UrlToText()",
		} {
			fn, err := newEngine().Parse(exp)
			assertTrue(t, fn == nil && err != nil, "%s 应在解析期报错，实际 fn=%v err=%v", exp, fn, err)
		}
	})

	t.Run("TooManyArguments_ThrowsArgumentException", func(t *testing.T) {
		// 差异：C# 抛 ArgumentException；Go 用 Parse 返回 error 表达。
		for _, exp := range []string{
			"UrlEncode('a','b')", "UrlDecode('a','b')", "HtmlEncode('a','b')", "HtmlDecode('a','b')",
			"TextToBase64('a','b')", "TextToBase64Url('a','b')", "Base64ToText('a','b')", "Base64UrlToText('a','b')",
		} {
			fn, err := newEngine().Parse(exp)
			assertTrue(t, fn == nil && err != nil, "%s 应因参数过多报错，实际 fn=%v err=%v", exp, fn, err)
		}
	})

	t.Run("NullArgument_ReturnsDefaultWithLastError", func(t *testing.T) {
		e1 := newEngine()
		v1 := evalTextDef(e1, "UrlEncode(NULL())", "<DEF>")
		assertText(t, "<DEF>", v1)
		assertLastError(t, e1)

		e2 := newEngine()
		v2 := evalTextDef(e2, "TextToBase64(NULL())", "<DEF>")
		assertText(t, "<DEF>", v2)
		assertLastError(t, e2)
	})

	t.Run("EmptyString_ReturnsEmptyString", func(t *testing.T) {
		for _, fn := range []string{
			"UrlEncode", "UrlDecode", "HtmlEncode", "HtmlDecode",
			"TextToBase64", "TextToBase64Url", "Base64ToText", "Base64UrlToText",
		} {
			e := newEngine()
			v := evalTextDef(e, fmt.Sprintf("%s('')", fn), "<DEF>")
			assertText(t, "", v)
			assertNoError(t, e)
		}
	})

	t.Run("Base64WithPadding_ReturnsParameterError", func(t *testing.T) {
		// 解码实现在转换前拒绝含 '=' 填充的输入
		e1 := newEngine()
		v1 := evalTextDef(e1, "Base64ToText('Jj3miJHkuK3lm73kurogPnx8=')", "<DEF>")
		assertText(t, "<DEF>", v1)
		assertLastError(t, e1)

		e2 := newEngine()
		v2 := evalTextDef(e2, "Base64UrlToText('Jj3miJHkuK3lm73kurogPnx8=')", "<DEF>")
		assertText(t, "<DEF>", v2)
		assertLastError(t, e2)
	})

	t.Run("TextToBase64Url_PaddingVariants", func(t *testing.T) {
		// 源字节数 3/6 时无填充；7/8 时标准 Base64 分别有 1/2 个 '='，Base64Url 应被截断
		assertText(t, "5Lit", evalText(newEngine(), "TextToBase64Url('中')"))
		assertText(t, "5Lit5Zu9", evalText(newEngine(), "TextToBase64Url('中国')"))
		assertText(t, "5Lit5Zu9QQ", evalText(newEngine(), "TextToBase64Url('中国A')"))
		assertText(t, "5Lit5Zu9QUI", evalText(newEngine(), "TextToBase64Url('中国AB')"))
		// 对照：标准 Base64 保留填充
		assertText(t, "5Lit5Zu9QQ==", evalText(newEngine(), "TextToBase64('中国A')"))
	})

	t.Run("Base64Url_FullRoundTrip", func(t *testing.T) {
		assertText(t, "中国", evalText(newEngine(), "Base64UrlToText(TextToBase64Url('中国'))"))
		// 两套字符集在对方解码器下均可解析
		assertText(t, "中国", evalText(newEngine(), "Base64UrlToText(TextToBase64('中国'))"))
		assertText(t, "中国", evalText(newEngine(), "Base64ToText(TextToBase64Url('中国'))"))
	})
}
