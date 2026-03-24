using PetaTest;

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
    }
}
