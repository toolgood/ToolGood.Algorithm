// 移植自 csharp/ToolGood.Algorithm.Test/AlgorithmEngine/CsharpWebTest.cs
//
// 说明：C# 中 `TryEvaluate(exp, null)` 绑定到 string 重载，出错返回 null；
// 移植版按约定用空串表示 string null，故默认值写 ""。
#include <string>

#include "cpptest.h"

#include "toolgood/AlgorithmEngine.h"

using namespace toolgood;

namespace {

TEST(CsharpWeb, UrlDecode) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("UrlDecode('%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c')", "");
    Assert::AreEqual(dt, "&=我中国人 >||");
}

TEST(CsharpWeb, UrlEncode) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("UrlEncode('&=我中国人 >||')", "");
    Assert::AreEqual(dt, "%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c");
}

TEST(CsharpWeb, HtmlEncode) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("HtmlEncode('&=我中国人 >||')", "");
    Assert::AreEqual(dt, "&amp;=我中国人 &gt;||");
}

TEST(CsharpWeb, HtmlDecode) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("HtmlDecode('&amp;=我中国人 &gt;||')", "");
    Assert::AreEqual(dt, "&=我中国人 >||");
}

TEST(CsharpWeb, TextToBase64) {
    AlgorithmEngine engine;
    auto r = engine.TryEvaluate("TextToBase64('&=我中国人 >||')", "");
    Assert::AreEqual(r, "Jj3miJHkuK3lm73kurogPnx8");
}

TEST(CsharpWeb, TextToBase64Url) {
    AlgorithmEngine engine;
    auto r = engine.TryEvaluate("TextToBase64Url('&=我中国人 >||')", "");
    Assert::AreEqual(r, "Jj3miJHkuK3lm73kurogPnx8");
}

TEST(CsharpWeb, Base64ToText) {
    AlgorithmEngine engine;
    auto r = engine.TryEvaluate("Base64ToText('Jj3miJHkuK3lm73kurogPnx8')", "");
    Assert::AreEqual(r, "&=我中国人 >||");
}

TEST(CsharpWeb, Base64UrlToText) {
    AlgorithmEngine engine;
    auto r = engine.TryEvaluate("Base64UrlToText('Jj3miJHkuK3lm73kurogPnx8')", "");
    Assert::AreEqual(r, "&=我中国人 >||");
}

// ---------------- 方法式调用测试 - 编码类 ----------------

TEST(CsharpWeb, MethodStyle_URLENCODE_test) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("'&=我中国人 >||'.URLENCODE()", "");
    Assert::AreEqual(dt, "%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c");
}

TEST(CsharpWeb, MethodStyle_URLDECODE_test) {
    AlgorithmEngine engine;
    auto dt = engine.TryEvaluate("'%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c'.URLDECODE()", "");
    Assert::AreEqual(dt, "&=我中国人 >||");
}

// ---------------- 边界行为测试 ----------------

TEST(CsharpWeb, ZeroArgument_ThrowsFormatException) {
    // 0 个参数在解析期即被拒（与 Abs()、Regex() 等全库函数行为一致）
    Assert::Throws([] { AlgorithmEngine e; e.Parse("UrlEncode()"); });
    Assert::Throws([] { AlgorithmEngine e; e.Parse("UrlDecode()"); });
    Assert::Throws([] { AlgorithmEngine e; e.Parse("HtmlEncode()"); });
    Assert::Throws([] { AlgorithmEngine e; e.Parse("HtmlDecode()"); });
    Assert::Throws([] { AlgorithmEngine e; e.Parse("TextToBase64()"); });
    Assert::Throws([] { AlgorithmEngine e; e.Parse("TextToBase64Url()"); });
    Assert::Throws([] { AlgorithmEngine e; e.Parse("Base64ToText()"); });
    Assert::Throws([] { AlgorithmEngine e; e.Parse("Base64UrlToText()"); });
}

TEST(CsharpWeb, TooManyArguments_ThrowsArgumentException) {
    // 超过 1 个参数由构造函数校验拦截
    Assert::Throws([] { AlgorithmEngine e; e.Parse("UrlEncode('a','b')"); });
    Assert::Throws([] { AlgorithmEngine e; e.Parse("UrlDecode('a','b')"); });
    Assert::Throws([] { AlgorithmEngine e; e.Parse("HtmlEncode('a','b')"); });
    Assert::Throws([] { AlgorithmEngine e; e.Parse("HtmlDecode('a','b')"); });
    Assert::Throws([] { AlgorithmEngine e; e.Parse("TextToBase64('a','b')"); });
    Assert::Throws([] { AlgorithmEngine e; e.Parse("TextToBase64Url('a','b')"); });
    Assert::Throws([] { AlgorithmEngine e; e.Parse("Base64ToText('a','b')"); });
    Assert::Throws([] { AlgorithmEngine e; e.Parse("Base64UrlToText('a','b')"); });
}

TEST(CsharpWeb, NullArgument_ReturnsDefaultWithLastError) {
    AlgorithmEngine e1;
    auto v1 = e1.TryEvaluate("UrlEncode(NULL())", "<DEF>");
    Assert::AreEqual(v1, "<DEF>");
    Assert::IsNotNull(e1.LastError);

    AlgorithmEngine e2;
    auto v2 = e2.TryEvaluate("TextToBase64(NULL())", "<DEF>");
    Assert::AreEqual(v2, "<DEF>");
    Assert::IsNotNull(e2.LastError);
}

TEST(CsharpWeb, EmptyString_ReturnsEmptyString) {
    const std::string fns[] = {"UrlEncode",  "UrlDecode",     "HtmlEncode", "HtmlDecode",
                               "TextToBase64", "TextToBase64Url", "Base64ToText", "Base64UrlToText"};
    for (const auto& fn : fns) {
        AlgorithmEngine e;
        auto v = e.TryEvaluate(fn + "('')", "<DEF>");
        Assert::AreEqual(v, "");
        Assert::IsNull(e.LastError);
    }
}

TEST(CsharpWeb, Base64WithPadding_ReturnsParameterError) {
    // 解码实现在转换前拒绝含 '=' 填充的输入
    AlgorithmEngine e1;
    auto v1 = e1.TryEvaluate("Base64ToText('Jj3miJHkuK3lm73kurogPnx8=')", "<DEF>");
    Assert::AreEqual(v1, "<DEF>");
    Assert::IsNotNull(e1.LastError);

    AlgorithmEngine e2;
    auto v2 = e2.TryEvaluate("Base64UrlToText('Jj3miJHkuK3lm73kurogPnx8=')", "<DEF>");
    Assert::AreEqual(v2, "<DEF>");
    Assert::IsNotNull(e2.LastError);
}

TEST(CsharpWeb, TextToBase64Url_PaddingVariants) {
    // 源字节数 3/6 时无填充；7/8 时标准 Base64 分别有 1/2 个 '='，Base64Url 应被截断
    Assert::AreEqual(AlgorithmEngine().TryEvaluate("TextToBase64Url('中')", ""), "5Lit");
    Assert::AreEqual(AlgorithmEngine().TryEvaluate("TextToBase64Url('中国')", ""), "5Lit5Zu9");
    Assert::AreEqual(AlgorithmEngine().TryEvaluate("TextToBase64Url('中国A')", ""), "5Lit5Zu9QQ");
    Assert::AreEqual(AlgorithmEngine().TryEvaluate("TextToBase64Url('中国AB')", ""), "5Lit5Zu9QUI");
    // 对照：标准 Base64 保留填充
    Assert::AreEqual(AlgorithmEngine().TryEvaluate("TextToBase64('中国A')", ""), "5Lit5Zu9QQ==");
}

TEST(CsharpWeb, Base64Url_FullRoundTrip) {
    Assert::AreEqual(AlgorithmEngine().TryEvaluate("Base64UrlToText(TextToBase64Url('中国'))", ""), "中国");
    // 两套字符集在对方解码器下均可解析
    Assert::AreEqual(AlgorithmEngine().TryEvaluate("Base64UrlToText(TextToBase64('中国'))", ""), "中国");
    Assert::AreEqual(AlgorithmEngine().TryEvaluate("Base64ToText(TextToBase64Url('中国'))", ""), "中国");
}

}  // namespace
