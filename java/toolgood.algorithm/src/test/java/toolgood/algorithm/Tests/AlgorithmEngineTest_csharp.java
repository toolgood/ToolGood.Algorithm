package toolgood.algorithm.Tests;

import static org.junit.Assert.assertEquals;

import org.junit.Test;

import toolgood.algorithm.AlgorithmEngine;

@SuppressWarnings("deprecation")
public class AlgorithmEngineTest_csharp {

    @Test
    public void UrlDecode()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("UrlDecode('%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c')", "");
        assertEquals(dt, "&=我中国人 >||");
    }
    @Test
    public void UrlEncode()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("UrlEncode('&=我中国人 >||')", "");
        assertEquals(dt, "%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c");
    }
    @Test
    public void HtmlEncode()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("HtmlEncode('&=我中国人 >||')", "");
        assertEquals(dt, "&amp;=我中国人 &gt;||");
    }
    @Test
    public void HtmlDecode()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("HtmlDecode('&amp;=我中国人 &gt;||')", "");
        assertEquals(dt, "&=我中国人 >||");
    }

    @Test
    public void TextToBase64()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        String r = engine.TryEvaluate("TextToBase64('&=我中国人 >||')", "");
        assertEquals(r, "Jj3miJHkuK3lm73kurogPnx8");

        r = engine.TryEvaluate("TextToBase64('&=我中国人 >||','GBK')", "");
        assertEquals(r, "Jj3O0tbQufrIyyA+fHw=");
    }

    @Test
    public void TextToBase64Url()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        String r = engine.TryEvaluate("TextToBase64Url('&=我中国人 >||')", "");
        assertEquals(r, "Jj3miJHkuK3lm73kurogPnx8");

        r = engine.TryEvaluate("TextToBase64Url('&=我中国人 >||','GBK')", "");
        assertEquals(r, "Jj3O0tbQufrIyyA-fHw");
    }

    @Test
    public void Base64ToText()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        String r = engine.TryEvaluate("Base64ToText('Jj3miJHkuK3lm73kurogPnx8')", "");
        assertEquals(r, "&=我中国人 >||");

        r = engine.TryEvaluate("Base64ToText('Jj3O0tbQufrIyyA+fHw=','GBK')", "");
        assertEquals(r, "&=我中国人 >||");
    }

    @Test
    public void Base64UrlToText()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        String r = engine.TryEvaluate("Base64UrlToText('Jj3miJHkuK3lm73kurogPnx8')", "");
        assertEquals(r, "&=我中国人 >||");

        r = engine.TryEvaluate("Base64UrlToText('Jj3O0tbQufrIyyA-fHw','GBK')", "");
        assertEquals(r, "&=我中国人 >||");
    }

    @Test
    public void Regex_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseExcelIndex = false;
        String r = engine.TryEvaluate("Regex('abcd','a.*c')", "");
        assertEquals(r, "abc");
        r = engine.TryEvaluate("Regex('abcd,abbcd','a.*?c',1)", "");
        assertEquals(r, "abbc");
        r = engine.TryEvaluate("Regex('abcd,abbcd','a(.*?)c',1,1)", "");
        assertEquals(r, "bb");
    }

    @Test
    public void IsRegex_test()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean r = engine.TryEvaluate("IsRegex('abcd','a.*c')", false);
        assertEquals(r, true);
        r = engine.TryEvaluate("IsRegex('abcd','da.*c')", false);
        assertEquals(r, false);
    }

    @Test
    public void Guid()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        String r = engine.TryEvaluate("Guid()", "");
        assertEquals(r.length()>0, true);
    }

    @Test
    public void Hash()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("Md5('&=我中国人 >||')", "");
        assertEquals(dt, "2E1CEFBDFA3677725B7856E02D225819");
        dt = engine.TryEvaluate("Sha1('&=我中国人 >||')", "");
        dt = engine.TryEvaluate("Sha256('&=我中国人 >||')", "");
        dt = engine.TryEvaluate("Sha512('&=我中国人 >||')", "");
        dt = engine.TryEvaluate("Crc8('&=我中国人 >||')", "");
        dt = engine.TryEvaluate("Crc16('&=我中国人 >||')", "");
        dt = engine.TryEvaluate("Crc32('&=我中国人 >||')", "");
        dt = engine.TryEvaluate("HmacMd5('&=我中国人 >||','12')", "");
        dt = engine.TryEvaluate("HmacSha1('&=我中国人 >||','12')", "");
        dt = engine.TryEvaluate("HmacSha256('&=我中国人 >||','12')", "");
        dt = engine.TryEvaluate("HmacSha512('&=我中国人 >||','12')", "");

    }

    //IndexOf
    @Test
    public void IndexOf()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        engine.UseExcelIndex = false;
        int dt = engine.TryEvaluate("IndexOf('abcd','cd')", -1);
        assertEquals(dt, 2);
        dt = engine.TryEvaluate("LastIndexOf('abcd','cd')", -1);
        assertEquals(dt, 2);
    }

    @Test
    public void Split()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("Split('1,2,3,4',',')[3]", "");
        assertEquals(dt, "3");
    }

    @Test
    public void TrimStart()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("TrimStart(' 123 ')", "");
        assertEquals(dt, "123 ");

        dt = engine.TryEvaluate("TrimStart(' 123 ',' 1')", "");
        assertEquals(dt, "23 ");
    }
    @Test
    public void TrimEnd()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("TrimEnd(' 123 ')", "");
        assertEquals(dt, " 123");

        dt = engine.TryEvaluate("TrimEnd(' 123 ','3 ')", "");
        assertEquals(dt, " 12");
    }

    @Test
    public void Join()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("Join(',',1,2,5,6)", "");
        assertEquals(dt, "1,2,5,6");
        dt = engine.TryEvaluate("Join(',',1,2,5,6,split('7,8,9',','))", "");
        assertEquals(dt, "1,2,5,6,7,8,9");
    }

    @Test
    public void Substring()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("Substring('123456789',1,2)", "");
        assertEquals(dt, "12");
    }
    @Test
    public void StartsWith()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean dt = engine.TryEvaluate("StartsWith('123456789','12')", false);
        assertEquals(dt, true);
        dt = engine.TryEvaluate("StartsWith('123456789','127')", false);
        assertEquals(dt, false);
    }
    @Test
    public void EndsWith()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean dt = engine.TryEvaluate("EndsWith('123456789','89')", false);
        assertEquals(dt, true);
        dt = engine.TryEvaluate("EndsWith('123456789','127')", false);
        assertEquals(dt, false);
    }

    @Test
    public void IsNullOrEmpty()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean dt = engine.TryEvaluate("IsNullOrEmpty('')", false);
        assertEquals(dt, true);
        dt = engine.TryEvaluate("IsNullOrEmpty(' ')", false);
        assertEquals(dt, false);
    }
    @Test
    public void IsNullOrWhiteSpace()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        boolean dt = engine.TryEvaluate("IsNullOrWhiteSpace('')", false);
        assertEquals(dt, true);
        dt = engine.TryEvaluate("IsNullOrWhiteSpace('   ')", false);
        assertEquals(dt, true);
        dt = engine.TryEvaluate("IsNullOrWhiteSpace(' f ')", false);
        assertEquals(dt, false);
    }

    @Test
    public void RemoveStart()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("RemoveStart('123456789','12')", "");
        assertEquals(dt, "3456789");
        dt = engine.TryEvaluate("RemoveStart('123456789','127')", "");
        assertEquals(dt, "123456789");
    }
    @Test
    public void RemoveEnd()
    {
        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("RemoveEnd('123456789','89')", "");
        assertEquals(dt, "1234567");
        dt = engine.TryEvaluate("RemoveEnd('123456789','127')", "");
        assertEquals(dt, "123456789");
    }

    @Test
    public void Json()
    {

        AlgorithmEngine engine = new AlgorithmEngine();
        String dt = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare\",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}').Age", "");
        assertEquals(dt.toString(), "51");
        dt = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare\",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')[Birthday]", "");
        assertEquals(dt, "04/26/1564 00:00:00");

        dt = engine.TryEvaluate("json('[1,2,3]')[1]", "");
        assertEquals(dt, "1");

        dt = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare   \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')[Name].Trim()", "");
        assertEquals(dt, "William Shakespeare");


        dt = engine.TryEvaluate("json('{\"Name1\":\"William Shakespeare \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Name'& 1].Trim().substring(2,3)", "");
        assertEquals(dt, "ill");

        dt = engine.TryEvaluate("json('12346{\"Name1\":\"William Shakespeare \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Name'& 1].Trim().substring(2,3)", "");
        assertEquals(dt, "");

        dt = engine.TryEvaluate("json('[1,2,3,4,5,6]')[1].Trim()", "");
        assertEquals(dt, "1");

        dt = engine.TryEvaluate("json('[1,2,3,4,5,6]22')[1].Trim()", "");
        assertEquals(dt, "");

        dt = engine.TryEvaluate("json('22[1,2,3,4,5,6]')[1].Trim()", "");
        assertEquals(dt, "");
    }
}