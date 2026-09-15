using PetaTest;
using System;

namespace ToolGood.Algorithm.Test.CsharpWeb
{
    [TestFixture]
    internal partial class CsharpWebTest
    {
        [Test]
        public void UrlDecode()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("UrlDecode('%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c')", null);
            Assert.AreEqual(dt, "&=我中国人 >||");
        }

        [Test]
        public void UrlEncode()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("UrlEncode('&=我中国人 >||')", null);
            Assert.AreEqual(dt, "%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c");
        }

        [Test]
        public void HtmlEncode()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("HtmlEncode('&=我中国人 >||')", null);
            Assert.AreEqual(dt, "&amp;=我中国人 &gt;||");
        }

        [Test]
        public void HtmlDecode()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("HtmlDecode('&amp;=我中国人 &gt;||')", null);
            Assert.AreEqual(dt, "&=我中国人 >||");
        }

        [Test]
        public void TextToBase64()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var r = engine.TryEvaluate("TextToBase64('&=我中国人 >||')", null);
            Assert.AreEqual(r, "Jj3miJHkuK3lm73kurogPnx8");
        }

        [Test]
        public void TextToBase64Url()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var r = engine.TryEvaluate("TextToBase64Url('&=我中国人 >||')", null);
            Assert.AreEqual(r, "Jj3miJHkuK3lm73kurogPnx8");
        }

        [Test]
        public void Base64ToText()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var r = engine.TryEvaluate("Base64ToText('Jj3miJHkuK3lm73kurogPnx8')", null);
            Assert.AreEqual(r, "&=我中国人 >||");
        }

        [Test]
        public void Base64UrlToText()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var r = engine.TryEvaluate("Base64UrlToText('Jj3miJHkuK3lm73kurogPnx8')", null);
            Assert.AreEqual(r, "&=我中国人 >||");
        }

        #region 方法式调用测试 - 编码类

        [Test]
        public void MethodStyle_URLENCODE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'&=我中国人 >||'.URLENCODE()", null);
            Assert.AreEqual(dt, "%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c");
        }

        [Test]
        public void MethodStyle_URLDECODE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c'.URLDECODE()", null);
            Assert.AreEqual(dt, "&=我中国人 >||");
        }

        #endregion 方法式调用测试 - 编码类

        #region 边界行为测试

        [Test]
        public void ZeroArgument_ThrowsFormatException()
        {
            // 0 个参数在 ANTLR 解析期即被拒（与 Abs()、Regex() 等全库函数行为一致）
            Assert.Throws<FormatException>(() => new AlgorithmEngine().Parse("UrlEncode()"));
            Assert.Throws<FormatException>(() => new AlgorithmEngine().Parse("UrlDecode()"));
            Assert.Throws<FormatException>(() => new AlgorithmEngine().Parse("HtmlEncode()"));
            Assert.Throws<FormatException>(() => new AlgorithmEngine().Parse("HtmlDecode()"));
            Assert.Throws<FormatException>(() => new AlgorithmEngine().Parse("TextToBase64()"));
            Assert.Throws<FormatException>(() => new AlgorithmEngine().Parse("TextToBase64Url()"));
            Assert.Throws<FormatException>(() => new AlgorithmEngine().Parse("Base64ToText()"));
            Assert.Throws<FormatException>(() => new AlgorithmEngine().Parse("Base64UrlToText()"));
        }

        [Test]
        public void TooManyArguments_ThrowsArgumentException()
        {
            // 超过 1 个参数由构造函数校验拦截
            Assert.Throws<ArgumentException>(() => new AlgorithmEngine().Parse("UrlEncode('a','b')"));
            Assert.Throws<ArgumentException>(() => new AlgorithmEngine().Parse("UrlDecode('a','b')"));
            Assert.Throws<ArgumentException>(() => new AlgorithmEngine().Parse("HtmlEncode('a','b')"));
            Assert.Throws<ArgumentException>(() => new AlgorithmEngine().Parse("HtmlDecode('a','b')"));
            Assert.Throws<ArgumentException>(() => new AlgorithmEngine().Parse("TextToBase64('a','b')"));
            Assert.Throws<ArgumentException>(() => new AlgorithmEngine().Parse("TextToBase64Url('a','b')"));
            Assert.Throws<ArgumentException>(() => new AlgorithmEngine().Parse("Base64ToText('a','b')"));
            Assert.Throws<ArgumentException>(() => new AlgorithmEngine().Parse("Base64UrlToText('a','b')"));
        }

        [Test]
        public void NullArgument_ReturnsDefaultWithLastError()
        {
            var e1 = new AlgorithmEngine();
            var v1 = e1.TryEvaluate("UrlEncode(NULL())", "<DEF>");
            Assert.AreEqual(v1, "<DEF>");
            Assert.IsNotNull(e1.LastError);

            var e2 = new AlgorithmEngine();
            var v2 = e2.TryEvaluate("TextToBase64(NULL())", "<DEF>");
            Assert.AreEqual(v2, "<DEF>");
            Assert.IsNotNull(e2.LastError);
        }

        [Test]
        public void EmptyString_ReturnsEmptyString()
        {
            foreach (var fn in new[] { "UrlEncode", "UrlDecode", "HtmlEncode", "HtmlDecode",
                                       "TextToBase64", "TextToBase64Url", "Base64ToText", "Base64UrlToText" }) {
                var e = new AlgorithmEngine();
                var v = e.TryEvaluate($"{fn}('')", "<DEF>");
                Assert.AreEqual(v, "");
                Assert.IsNull(e.LastError);
            }
        }

        [Test]
        public void Base64WithPadding_ReturnsParameterError()
        {
            // 解码实现在转换前拒绝含 '=' 填充的输入
            var e1 = new AlgorithmEngine();
            var v1 = e1.TryEvaluate("Base64ToText('Jj3miJHkuK3lm73kurogPnx8=')", "<DEF>");
            Assert.AreEqual(v1, "<DEF>");
            Assert.IsNotNull(e1.LastError);

            var e2 = new AlgorithmEngine();
            var v2 = e2.TryEvaluate("Base64UrlToText('Jj3miJHkuK3lm73kurogPnx8=')", "<DEF>");
            Assert.AreEqual(v2, "<DEF>");
            Assert.IsNotNull(e2.LastError);
        }

        [Test]
        public void TextToBase64Url_PaddingVariants()
        {
            // 源字节数 3/6 时无填充；7/8 时标准 Base64 分别有 1/2 个 '='，Base64Url 应被截断
            Assert.AreEqual(new AlgorithmEngine().TryEvaluate("TextToBase64Url('中')", null), "5Lit");
            Assert.AreEqual(new AlgorithmEngine().TryEvaluate("TextToBase64Url('中国')", null), "5Lit5Zu9");
            Assert.AreEqual(new AlgorithmEngine().TryEvaluate("TextToBase64Url('中国A')", null), "5Lit5Zu9QQ");
            Assert.AreEqual(new AlgorithmEngine().TryEvaluate("TextToBase64Url('中国AB')", null), "5Lit5Zu9QUI");
            // 对照：标准 Base64 保留填充
            Assert.AreEqual(new AlgorithmEngine().TryEvaluate("TextToBase64('中国A')", null), "5Lit5Zu9QQ==");
        }

        [Test]
        public void Base64Url_FullRoundTrip()
        {
            Assert.AreEqual(new AlgorithmEngine().TryEvaluate("Base64UrlToText(TextToBase64Url('中国'))", null), "中国");
            // 两套字符集在对方解码器下均可解析
            Assert.AreEqual(new AlgorithmEngine().TryEvaluate("Base64UrlToText(TextToBase64('中国'))", null), "中国");
            Assert.AreEqual(new AlgorithmEngine().TryEvaluate("Base64ToText(TextToBase64Url('中国'))", null), "中国");
        }

        #endregion 边界行为测试
    }
}
