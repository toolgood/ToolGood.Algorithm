package toolgood.algorithm.Tests;

import org.junit.Test;
import static org.junit.Assert.*;

import toolgood.algorithm.AlgorithmEngine;

public class CsharpWebTest {
    @Test
    public void UrlDecode() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("UrlDecode('%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c')", "");
        assertEquals("&=我中国人 >||", dt);
    }

    @Test
    public void UrlEncode() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("UrlEncode('&=我中国人 >||')", "");
        assertEquals("%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c", dt);
    }

    @Test
    public void HtmlEncode() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("HtmlEncode('&=我中国人 >||')", "");
        assertEquals("&amp;=我中国人 &gt;||", dt);
    }

    @Test
    public void HtmlDecode() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("HtmlDecode('&amp;=我中国人 &gt;||')", "");
        assertEquals("&=我中国人 >||", dt);
    }

    @Test
    public void TextToBase64() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String r = engine.TryEvaluate("TextToBase64('&=我中国人 >||')", "");
        assertEquals("Jj3miJHkuK3lm73kurogPnx8", r);
    }

    @Test
    public void TextToBase64Url() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String r = engine.TryEvaluate("TextToBase64Url('&=我中国人 >||')", "");
        assertEquals("Jj3miJHkuK3lm73kurogPnx8", r);
    }

    @Test
    public void Base64ToText() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String r = engine.TryEvaluate("Base64ToText('Jj3miJHkuK3lm73kurogPnx8')", "");
        assertEquals("&=我中国人 >||", r);
    }

    @Test
    public void Base64UrlToText() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String r = engine.TryEvaluate("Base64UrlToText('Jj3miJHkuK3lm73kurogPnx8')", "");
        assertEquals("&=我中国人 >||", r);
    }

    // 方法式调用测试 - 编码类
    @Test
    public void MethodStyle_URLENCODE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("'&=我中国人 >||'.URLENCODE()", "");
        assertEquals("%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c", dt);
    }

    @Test
    public void MethodStyle_URLDECODE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("'%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c'.URLDECODE()", "");
        assertEquals("&=我中国人 >||", dt);
    }
}
