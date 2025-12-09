//using BenchmarkDotNet.Attributes;
//using Microsoft.VSDiagnostics;
//using ToolGood.Algorithm;
//using static System.Runtime.InteropServices.JavaScript.JSType;

//[CPUUsageDiagnoser]
//public class AlgorithmEngineBenchmarks
//{
//    private AlgorithmEngine engine;
//    [GlobalSetup]
//    public void Setup()
//    {
//        engine = new AlgorithmEngine();
//    }


//    [Benchmark]
//    public string UrlEncodeBenchmark()
//    {
//        return engine.TryEvaluate("UrlEncode('&=我中国人 >||')", "");
//    }

//    [Benchmark]
//    public string UrlDecodeBenchmark()
//    {
//        return engine.TryEvaluate("UrlDecode('%26%3d%e6%88%91%e4%b8%ad%e5%9b%bd%e4%ba%ba+%3e%7c%7c')", "");
//    }

//    [Benchmark]
//    public string HtmlEncodeBenchmark()
//    {
//        return engine.TryEvaluate("HtmlEncode('&=我中国人 >||')", "");
//    }

//    [Benchmark]
//    public string HtmlDecodeBenchmark()
//    {
//        return engine.TryEvaluate("HtmlDecode('&amp;=我中国人 &gt;||')", "");
//    }

//    [Benchmark]
//    public string Md5Benchmark()
//    {
//        return engine.TryEvaluate("Md5('&=我中国人 >||')", "");
//    }

//    [Benchmark]
//    public string Sha1Benchmark()
//    {
//        return engine.TryEvaluate("Sha1('&=我中国人 >||')", "");
//    }

//    [Benchmark]
//    public string Sha256Benchmark()
//    {
//        return engine.TryEvaluate("Sha256('&=我中国人 >||')", "");
//    }

//    [Benchmark]
//    public string Sha512Benchmark()
//    {
//        return engine.TryEvaluate("Sha512('&=我中国人 >||')", "");
//    }

//    [Benchmark]
//    public string Crc32Benchmark()
//    {
//        return engine.TryEvaluate("Crc32('&=我中国人 >||')", "");
//    }

//    [Benchmark]
//    public string TextToBase64Benchmark()
//    {
//        return engine.TryEvaluate("TextToBase64('&=我中国人 >||')", "");
//    }

//    [Benchmark]
//    public string Base64ToTextBenchmark()
//    {
//        return engine.TryEvaluate("Base64ToText('Jj3miJHkuK3lm73kurogPnx8')", "");
//    }

//    [Benchmark]
//    public string RegexBenchmark()
//    {
//        return engine.TryEvaluate("Regex('abcd','a.*c')", "");
//    }

//    [Benchmark]
//    public bool IsRegexBenchmark()
//    {
//        return engine.TryEvaluate("IsRegex('abcd','a.*c')", false);
//    }

//    [Benchmark]
//    public string GuidBenchmark()
//    {
//        return engine.TryEvaluate("Guid()", "");
//    }

//    [Benchmark]
//    public string HmacMd5Benchmark()
//    {
//        return engine.TryEvaluate("HmacMd5('&=我中国人 >||','12')", "");
//    }

//    [Benchmark]
//    public string HmacSha1Benchmark()
//    {
//        return engine.TryEvaluate("HmacSha1('&=我中国人 >||','12')", "");
//    }

//    [Benchmark]
//    public string HmacSha256Benchmark()
//    {
//        return engine.TryEvaluate("HmacSha256('&=我中国人 >||','12')", "");
//    }

//    [Benchmark]
//    public string HmacSha512Benchmark()
//    {
//        return engine.TryEvaluate("HmacSha512('&=我中国人 >||','12')", "");
//    }

//    [Benchmark]
//    public string TrimStartBenchmark()
//    {
//        return engine.TryEvaluate("TrimStart(' 123 ')", "");
//    }

//    [Benchmark]
//    public string TrimEndBenchmark()
//    {
//        return engine.TryEvaluate("TrimEnd(' 123 ')", "");
//    }

//    [Benchmark]
//    public int IndexOfBenchmark()
//    {
//        return engine.TryEvaluate("IndexOf('abcd','cd')", -1);
//    }

//    [Benchmark]
//    public int LastIndexOfBenchmark()
//    {
//        return engine.TryEvaluate("LastIndexOf('abcd','cd')", -1);
//    }

//    [Benchmark]
//    public string SplitBenchmark()
//    {
//        return engine.TryEvaluate("Split('1,2,3,4',',')[3]", "");
//    }

//    [Benchmark]
//    public string JoinBenchmark()
//    {
//        return engine.TryEvaluate("Join(',',1,2,5,6)", "");
//    }

//    [Benchmark]
//    public string SubstringBenchmark()
//    {
//        return engine.TryEvaluate("Substring('123456789',1,2)", "");
//    }

//    [Benchmark]
//    public bool StartsWithBenchmark()
//    {
//        return engine.TryEvaluate("StartsWith('123456789','12')", false);
//    }

//    [Benchmark]
//    public bool EndsWithBenchmark()
//    {
//        return engine.TryEvaluate("EndsWith('123456789','89')", false);
//    }

//    [Benchmark]
//    public string RemoveStartBenchmark()
//    {
//        return engine.TryEvaluate("RemoveStart('123456789','12')", "");
//    }

//    [Benchmark]
//    public string RemoveEndBenchmark()
//    {
//        return engine.TryEvaluate("RemoveEnd('123456789','89')", "");
//    }

//    [Benchmark]
//    public string JsonBenchmark()
//    {
//        return engine.TryEvaluate("json('{\"Name\":\"William Shakespeare\",\"Age\":51}').Age", "");
//    }

//    [Benchmark]
//    public bool HasBenchmark()
//    {
//        return engine.TryEvaluate("Has(json('{\"Name\":\"William\"}'), 'Name')", false);
//    }
//}