"""Tests ported from ``java/.../AlgorithmEngine/CsharpWebTest.java``.

Web 编码解码测试：URL编码/解码、HTML编码/解码、Base64编码/解码。
"""
from __future__ import annotations

from toolgood_algorithm import AlgorithmEngine


def test_url_decode():
    # Java: CsharpWebTest.java:14
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("UrlDecode('%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c')", None)
    assert dt == "&=我中国人 >||"


def test_url_encode():
    # Java: CsharpWebTest.java:21
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("UrlEncode('&=我中国人 >||')", None)
    assert dt == "%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c"


def test_html_encode():
    # Java: CsharpWebTest.java:28
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("HtmlEncode('&=我中国人 >||')", None)
    assert dt == "&amp;=我中国人 &gt;||"


def test_html_decode():
    # Java: CsharpWebTest.java:35
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("HtmlDecode('&amp;=我中国人 &gt;||')", None)
    assert dt == "&=我中国人 >||"


def test_text_to_base64():
    # Java: CsharpWebTest.java:42
    engine = AlgorithmEngine()
    r = engine.try_evaluate_text("TextToBase64('&=我中国人 >||')", None)
    assert r == "Jj3miJHkuK3lm73kurogPnx8"


def test_text_to_base64_url():
    # Java: CsharpWebTest.java:49
    engine = AlgorithmEngine()
    r = engine.try_evaluate_text("TextToBase64Url('&=我中国人 >||')", None)
    assert r == "Jj3miJHkuK3lm73kurogPnx8"


def test_base64_to_text():
    # Java: CsharpWebTest.java:56
    engine = AlgorithmEngine()
    r = engine.try_evaluate_text("Base64ToText('Jj3miJHkuK3lm73kurogPnx8')", None)
    assert r == "&=我中国人 >||"


def test_base64_url_to_text():
    # Java: CsharpWebTest.java:63
    engine = AlgorithmEngine()
    r = engine.try_evaluate_text("Base64UrlToText('Jj3miJHkuK3lm73kurogPnx8')", None)
    assert r == "&=我中国人 >||"


# region 方法式调用测试 - 编码类


def test_method_style_urlencode():
    # Java: CsharpWebTest.java:72
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("'&=我中国人 >||'.URLENCODE()", None)
    assert dt == "%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c"


def test_method_style_urldecode():
    # Java: CsharpWebTest.java:79
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("'%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c'.URLDECODE()", None)
    assert dt == "&=我中国人 >||"


def test_html_decode_entities():
    # Java: CsharpWebTest.java:86
    engine = AlgorithmEngine()
    # 新增实体表: &copy; ©, &nbsp; 为不换行空格(修复前 nbsp 被替换为普通空格)
    dt = engine.try_evaluate_text("HtmlDecode('&copy;&reg;&trade;')", None)
    assert dt == "\u00A9\u00AE\u2122"

    dt = engine.try_evaluate_text("HtmlDecode('a&nbsp;b')", None)
    assert dt == "a\u00A0b"

    # 数字实体(修复前不支持)
    dt = engine.try_evaluate_text("HtmlDecode('&#65;&#x42;')", None)
    assert dt == "AB"

    # 基础实体行为保持
    dt = engine.try_evaluate_text("HtmlDecode('&amp;&lt;&gt;&quot;&apos;')", None)
    assert dt == "&<>\"'"


def test_url_encode_space():
    # Java: CsharpWebTest.java:105
    engine = AlgorithmEngine()
    # 空格编码为 +(修复前 URLEncoder.encode 同样为 +, 但保留字符集不同)
    dt = engine.try_evaluate_text("UrlEncode('a b')", None)
    assert dt == "a+b"

    # 保留字符 ~(RFC 3986 unreserved)
    dt = engine.try_evaluate_text("UrlEncode('a~b')", None)
    assert dt == "a~b"


# endregion
