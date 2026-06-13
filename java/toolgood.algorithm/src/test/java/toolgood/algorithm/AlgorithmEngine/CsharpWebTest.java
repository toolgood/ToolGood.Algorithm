package toolgood.algorithm.algorithmengine;

import org.junit.Test;
import static org.junit.Assert.*;

import toolgood.algorithm.AlgorithmEngine;

/**
 * Web 编码解码测试：URL编码/解码、HTML编码/解码、Base64编码/解码
 */
public class CsharpWebTest {

    @Test
    public void UrlDecode() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("UrlDecode('%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c')", (String)null);
        assertEquals("&=我中国人 >||", dt);
    }

    @Test
    public void UrlEncode() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("UrlEncode('&=我中国人 >||')", (String)null);
        assertEquals("%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c", dt);
    }

    @Test
    public void HtmlEncode() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("HtmlEncode('&=我中国人 >||')", (String)null);
        assertEquals("&amp;=我中国人 &gt;||", dt);
    }

    @Test
    public void HtmlDecode() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("HtmlDecode('&amp;=我中国人 &gt;||')", (String)null);
        assertEquals("&=我中国人 >||", dt);
    }

    @Test
    public void TextToBase64() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String r = engine.TryEvaluate("TextToBase64('&=我中国人 >||')", (String)null);
        assertEquals("Jj3miJHkuK3lm73kurogPnx8", r);
    }

    @Test
    public void TextToBase64Url() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String r = engine.TryEvaluate("TextToBase64Url('&=我中国人 >||')", (String)null);
        assertEquals("Jj3miJHkuK3lm73kurogPnx8", r);
    }

    @Test
    public void Base64ToText() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String r = engine.TryEvaluate("Base64ToText('Jj3miJHkuK3lm73kurogPnx8')", (String)null);
        assertEquals("&=我中国人 >||", r);
    }

    @Test
    public void Base64UrlToText() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String r = engine.TryEvaluate("Base64UrlToText('Jj3miJHkuK3lm73kurogPnx8')", (String)null);
        assertEquals("&=我中国人 >||", r);
    }

    // ===== 方法式调用测试 - 编码类 =====

    @Test
    public void MethodStyle_URLENCODE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("'&=我中国人 >||'.URLENCODE()", (String)null);
        assertEquals("%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c", dt);
    }

    @Test
    public void MethodStyle_URLDECODE_test() {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("'%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c'.URLDECODE()", (String)null);
        assertEquals("&=我中国人 >||", dt);
    }
}
