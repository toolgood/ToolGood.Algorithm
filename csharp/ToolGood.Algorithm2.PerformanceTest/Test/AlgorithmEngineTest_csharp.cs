using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using ToolGood.Algorithm;

namespace ToolGood.Algorithm2.PerformanceTest.Test
{
    public class AlgorithmEngineTest_csharp
    {
        [Benchmark]
        public void UrlDecode()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("UrlDecode('%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c')", null);
        }
        [Benchmark]
        public void UrlEncode()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("UrlEncode('&=我中国人 >||')", null);
        }
        [Benchmark]
        public void HtmlEncode()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("HtmlEncode('&=我中国人 >||')", null);
        }
        [Benchmark]
        public void HtmlDecode()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("HtmlDecode('&amp;=我中国人 &gt;||')", null);
        }

        [Benchmark]
        public void TextToBase64()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var r = engine.TryEvaluate("TextToBase64('&=我中国人 >||')", null);

        }

        [Benchmark]
        public void TextToBase64_2()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var r = engine.TryEvaluate("TextToBase64('&=我中国人 >||','GBK')", null);
        }

        [Benchmark]
        public void TextToBase64Url()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var r = engine.TryEvaluate("TextToBase64Url('&=我中国人 >||')", null);
        }
        [Benchmark]
        public void TextToBase64Url_2()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var r = engine.TryEvaluate("TextToBase64Url('&=我中国人 >||','GBK')", null);
        }


        [Benchmark]
        public void Base64ToText()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var r = engine.TryEvaluate("Base64ToText('Jj3miJHkuK3lm73kurogPnx8')", null);
        }

        [Benchmark]
        public void Base64ToText_2()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var r = engine.TryEvaluate("Base64ToText('Jj3O0tbQufrIyyA+fHw=','GBK')", null);
        }

        [Benchmark]
        public void Base64UrlToText()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var r = engine.TryEvaluate("Base64UrlToText('Jj3miJHkuK3lm73kurogPnx8')", null);
        }
        [Benchmark]
        public void Base64UrlToText_2()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var r = engine.TryEvaluate("Base64UrlToText('Jj3O0tbQufrIyyA-fHw','GBK')", null);
        }

        [Benchmark]
        public void Regex_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseExcelIndex = false;
            var r = engine.TryEvaluate("Regex('abcd','a.*c')", null);
        }
        [Benchmark]
        public void Regex_test_2()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseExcelIndex = false;
            var r = engine.TryEvaluate("Regex('abcd,abbcd','a.*?c',1)", null);
        }
        [Benchmark]
        public void Regex_test_3()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseExcelIndex = false;
            var r = engine.TryEvaluate("Regex('abcd,abbcd','a(.*?)c',1,1)", null);
        }

        [Benchmark]
        public void IsRegex_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var r = engine.TryEvaluate("IsRegex('abcd','a.*c')", false);
        }

        [Benchmark]
        public void IsRegex_test_2()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var r = engine.TryEvaluate("IsRegex('abcd','da.*c')", false);
        }



        [Benchmark]
        public void Guid()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var r = engine.TryEvaluate("Guid()", "");
        }

        [Benchmark]
        public void Hash()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("Md5('&=我中国人 >||')", null);
            engine.TryEvaluate("Sha1('&=我中国人 >||')", null);
            engine.TryEvaluate("Sha256('&=我中国人 >||')", null);
            engine.TryEvaluate("Sha512('&=我中国人 >||')", null);
            engine.TryEvaluate("Crc8('&=我中国人 >||')", null);
            engine.TryEvaluate("Crc16('&=我中国人 >||')", null);
            engine.TryEvaluate("Crc32('&=我中国人 >||')", null);
            engine.TryEvaluate("HmacMd5('&=我中国人 >||','12')", null);
            engine.TryEvaluate("HmacSha1('&=我中国人 >||','12')", null);
            engine.TryEvaluate("HmacSha256('&=我中国人 >||','12')", null);
            engine.TryEvaluate("HmacSha512('&=我中国人 >||','12')", null);
        }

        //IndexOf
        [Benchmark]
        public void IndexOf()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseExcelIndex = false;
            engine.TryEvaluate("IndexOf('abcd','cd')", -1);

        }
        [Benchmark]
        public void LastIndexOf()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseExcelIndex = false;
            engine.TryEvaluate("LastIndexOf('abcd','cd')", -1);
        }

        [Benchmark]
        public void Split()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("Split('1,2,3,4',',')[3]", null);

        }

        [Benchmark]
        public void TrimStart()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("TrimStart(' 123 ')", null);

        }
        [Benchmark]
        public void TrimStart_2()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("TrimStart(' 123 ',' 1')", null);
        }

        [Benchmark]
        public void TrimEnd()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("TrimEnd(' 123 ')", null);
        }
        [Benchmark]
        public void TrimEnd_2()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("TrimEnd(' 123 ','3 ')", null);
        }

        [Benchmark]
        public void Join()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("Join(',',1,2,5,6)", null);
        }
                [Benchmark]
        public void Substring()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("Substring('123456789',1,2)", null);

        }
        [Benchmark]
        public void StartsWith()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("StartsWith('123456789','12')", false);
        }
        [Benchmark]
        public void EndsWith()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("EndsWith('123456789','89')", false);
        }

        [Benchmark]
        public void IsNullOrEmpty()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("IsNullOrEmpty('')", false);
        }
        [Benchmark]
        public void IsNullOrEmpty_2()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("IsNullOrEmpty(' ')", false);
        }
        [Benchmark]
        public void IsNullOrWhiteSpace()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("IsNullOrWhiteSpace('')", false);
        }
        [Benchmark]
        public void IsNullOrWhiteSpace_2()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("IsNullOrWhiteSpace('   ')", false);
        }
        [Benchmark]
        public void IsNullOrWhiteSpace_3()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("IsNullOrWhiteSpace(' f ')", false);
        }

        [Benchmark]
        public void RemoveStart()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("RemoveStart('123456789','12')", null);
        }
        [Benchmark]
        public void RemoveEnd()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.TryEvaluate("RemoveEnd('123456789','89')", null);
        }

    }
}
