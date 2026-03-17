using PetaTest;

namespace ToolGood.Algorithm.Test.Csharp
{
    [TestFixture]
    internal partial class CsharpTest
    {
        [Test]
        public void Regex_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseExcelIndex = false;
            var r = engine.TryEvaluate("Regex('abcd','a.*c')", null);
            Assert.AreEqual(r, "abc");
        }

        [Test]
        public void REGEXREPLACE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var r = engine.TryEvaluate("REGEXREPLACE('abc123def', '\\\\d+', 'X')", "");
            Assert.AreEqual(r, "abcXdef");

            r = engine.TryEvaluate("REGEXREPLACE('hello world', 'world', 'there')", "");
            Assert.AreEqual(r, "hello there");

            r = engine.TryEvaluate("REGEXREPLACE('123-456-789', '-', '')", "");
            Assert.AreEqual(r, "123456789");
        }

        [Test]
        public void MethodStyle_REGEX_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var r = engine.TryEvaluate("'abcd'.REGEX('a.*c')", null);
            Assert.AreEqual(r, "abc");
        }

        [Test]
        public void MethodStyle_REGEXREPLACE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var r = engine.TryEvaluate("'abc123def'.REGEXREPLACE('\\\\d+', 'X')", "");
            Assert.AreEqual(r, "abcXdef");
        }

        [Test]
        public void MethodStyle_ISREGEX_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var r = engine.TryEvaluate("'abcd'.ISREGEX('a.*c')", false);
            Assert.AreEqual(r, true);

            r = engine.TryEvaluate("'abcd'.ISREGEX('x.*z')", true);
            Assert.AreEqual(r, false);
        }

        [Test]
        public void IsRegex_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var r = engine.TryEvaluate("IsRegex('abcd','a.*c')", false);
            Assert.AreEqual(r, true);
            r = engine.TryEvaluate("IsRegex('abcd','da.*c')", true);
            Assert.AreEqual(r, false);
        }

        [Test]
        public void IsRegex_ALIAS_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var r = engine.TryEvaluate("ISMATCH('abcd','a.*c')", false);
            Assert.AreEqual(r, true);
            r = engine.TryEvaluate("ISMATCH('abcd','da.*c')", true);
            Assert.AreEqual(r, false);
        }

        [Test]
        public void IndexOf()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseExcelIndex = false;
            var dt = engine.TryEvaluate("IndexOf('abcd','cd')", -1);
            Assert.AreEqual(dt, 2);
            dt = engine.TryEvaluate("LastIndexOf('abcd','cd')", -1);
            Assert.AreEqual(dt, 2);
        }

        [Test]
        public void Split()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("Split('1,2,3,4',',')[3]", null);
            Assert.AreEqual(dt, "3");
        }

        [Test]
        public void TrimStart()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("TrimStart(' 123 ')", null);
            Assert.AreEqual(dt, "123 ");

            dt = engine.TryEvaluate("TrimStart(' 123 ',' 1')", null);
            Assert.AreEqual(dt, "23 ");
        }

        [Test]
        public void TrimStart_ALIAS_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("LTRIM(' 123 ')", null);
            Assert.AreEqual(dt, "123 ");

            dt = engine.TryEvaluate("LTRIM(' 123 ',' 1')", null);
            Assert.AreEqual(dt, "23 ");
        }

        [Test]
        public void TrimEnd()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("TrimEnd(' 123 ')", null);
            Assert.AreEqual(dt, " 123");

            dt = engine.TryEvaluate("TrimEnd(' 123 ','3 ')", null);
            Assert.AreEqual(dt, " 12");
        }

        [Test]
        public void TrimEnd_ALIAS_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("RTRIM(' 123 ')", null);
            Assert.AreEqual(dt, " 123");

            dt = engine.TryEvaluate("RTRIM(' 123 ','3 ')", null);
            Assert.AreEqual(dt, " 12");
        }

        [Test]
        public void Join()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("Join(',',1,2,5,6)", null);
            Assert.AreEqual(dt, "1,2,5,6");
            dt = engine.TryEvaluate("Join(',',1,2,5,6,split('7,8,9',','))", null);
            Assert.AreEqual(dt, "1,2,5,6,7,8,9");

            dt = engine.TryEvaluate("Join(',',1,2,5,6，'tt')", null);
            Assert.AreEqual(dt, "1,2,5,6,tt");
        }

        [Test]
        public void Substring()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("Substring('123456789',1,2)", null);
            Assert.AreEqual(dt, "12");
        }

        [Test]
        public void StartsWith()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("StartsWith('123456789','12')", false);
            Assert.AreEqual(dt, true);
            dt = engine.TryEvaluate("StartsWith('123456789','127')", true);
            Assert.AreEqual(dt, false);
        }

        [Test]
        public void EndsWith()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("EndsWith('123456789','89')", false);
            Assert.AreEqual(dt, true);
            dt = engine.TryEvaluate("EndsWith('123456789','127')", true);
            Assert.AreEqual(dt, false);
        }

        [Test]
        public void IsNullOrEmpty()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("IsNullOrEmpty('')", false);
            Assert.AreEqual(dt, true);
            dt = engine.TryEvaluate("IsNullOrEmpty(' ')", true);
            Assert.AreEqual(dt, false);
        }

        [Test]
        public void IsNullOrWhiteSpace()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("IsNullOrWhiteSpace('')", false);
            Assert.AreEqual(dt, true);
            dt = engine.TryEvaluate("IsNullOrWhiteSpace('   ')", false);
            Assert.AreEqual(dt, true);
            dt = engine.TryEvaluate("IsNullOrWhiteSpace(' f ')", true);
            Assert.AreEqual(dt, false);
        }

        [Test]
        public void RemoveStart()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("RemoveStart('123456789','12')", null);
            Assert.AreEqual(dt, "3456789");
            dt = engine.TryEvaluate("RemoveStart('123456789','127')", null);
            Assert.AreEqual(dt, "123456789");
        }

        [Test]
        public void RemoveEnd()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("RemoveEnd('123456789','89')", null);
            Assert.AreEqual(dt, "1234567");
            dt = engine.TryEvaluate("RemoveEnd('123456789','127')", null);
            Assert.AreEqual(dt, "123456789");
        }

        [Test]
        public void Json_Object_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare\",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}').Age", null);
            Assert.AreEqual(dt.ToString(), "51");
            dt = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare\",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Birthday']", null);
            Assert.AreEqual(dt, "04/26/1564 00:00:00");
        }

        [Test]
        public void Json_Array_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("json('[1,2,3]')[1]", null);
            Assert.AreEqual(dt, "1");
        }

        [Test]
        public void Json_MethodChain_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare   \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Name'].Trim()", null);
            Assert.AreEqual(dt, "William Shakespeare");

            dt = engine.TryEvaluate("json('{\"Name1\":\"William Shakespeare \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Name'& 1].Trim().substring(2,3)", null);
            Assert.AreEqual(dt, "ill");
        }

        [Test]
        public void Json_Invalid_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("json('12346{\"Name1\":\"William Shakespeare \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Name'& 1].Trim().substring(2,3)", null);
            Assert.AreEqual(dt, null);

            dt = engine.TryEvaluate("json('[1,2,3,4,5,6]')[1].Trim()", null);
            Assert.AreEqual(dt, "1");

            dt = engine.TryEvaluate("json('[1,2,3,4,5,6]22')[1].Trim()", null);
            Assert.AreEqual(dt, null);

            dt = engine.TryEvaluate("json('22[1,2,3,4,5,6]')[1].Trim()", null);
            Assert.AreEqual(dt, null);
        }

        [Test]
        public void Json_Boolean_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt2 = engine.TryEvaluate("json('{\"w11\":true}')['w11']", false);
            Assert.AreEqual(dt2, true);
        }

        [Test]
        public void Json_Raw_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt3 = engine.TryEvaluate("{\"w11\":false}", "");
            Assert.AreEqual(dt3.ToString(), "{\"w11\":false}");

            dt3 = engine.TryEvaluate("[1,2,3,4]", "");
            Assert.AreEqual(dt3.ToString(), "[1,2,3,4]");
        }

        #region 方法式调用测试 - 字符串扩展方法

        [Test]
        public void MethodStyle_TRIMSTART_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("' 123 '.TRIMSTART()", null);
            Assert.AreEqual(dt, "123 ");

            dt = engine.TryEvaluate("' 123 '.TRIMSTART(' 1')", null);
            Assert.AreEqual(dt, "23 ");
        }

        [Test]
        public void MethodStyle_TRIMEND_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("' 123 '.TRIMEND()", null);
            Assert.AreEqual(dt, " 123");

            dt = engine.TryEvaluate("' 123 '.TRIMEND('3 ')", null);
            Assert.AreEqual(dt, " 12");
        }

        [Test]
        public void MethodStyle_INDEXOF_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseExcelIndex = false;
            var dt = engine.TryEvaluate("'abcd'.INDEXOF('cd')", -1);
            Assert.AreEqual(dt, 2);

            dt = engine.TryEvaluate("'abcd'.INDEXOF('cd',0)", -1);
            Assert.AreEqual(dt, 2);
        }

        [Test]
        public void MethodStyle_LASTINDEXOF_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseExcelIndex = false;
            var dt = engine.TryEvaluate("'abcdcd'.LASTINDEXOF('cd')", -1);
            Assert.AreEqual(dt, 4);
        }

        [Test]
        public void MethodStyle_SPLIT_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'1,2,3,4'.SPLIT(',')[3]", null);
            Assert.AreEqual(dt, "3");
        }

        [Test]
        public void MethodStyle_JOIN_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("','.JOIN(1,2,3)", null);
            Assert.AreEqual(dt, "1,2,3");
        }

        [Test]
        public void MethodStyle_SUBSTRING_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'123456789'.SUBSTRING(1,2)", null);
            Assert.AreEqual(dt, "12");

            dt = engine.TryEvaluate("'123456789'.SUBSTRING(3)", null);
            Assert.AreEqual(dt, "3456789");
        }

        [Test]
        public void MethodStyle_STARTSWITH_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'123456789'.STARTSWITH('12')", false);
            Assert.AreEqual(dt, true);

            dt = engine.TryEvaluate("'123456789'.STARTSWITH('127')", true);
            Assert.AreEqual(dt, false);
        }

        [Test]
        public void MethodStyle_ENDSWITH_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'123456789'.ENDSWITH('89')", false);
            Assert.AreEqual(dt, true);

            dt = engine.TryEvaluate("'123456789'.ENDSWITH('127')", true);
            Assert.AreEqual(dt, false);
        }

        [Test]
        public void MethodStyle_ISNULLOREMPTY_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("''.ISNULLOREMPTY()", false);
            Assert.AreEqual(dt, true);

            dt = engine.TryEvaluate("' '.ISNULLOREMPTY()", true);
            Assert.AreEqual(dt, false);
        }

        [Test]
        public void MethodStyle_ISNULLORWHITESPACE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("''.ISNULLORWHITESPACE()", false);
            Assert.AreEqual(dt, true);

            dt = engine.TryEvaluate("'   '.ISNULLORWHITESPACE()", false);
            Assert.AreEqual(dt, true);

            dt = engine.TryEvaluate("' f '.ISNULLORWHITESPACE()", true);
            Assert.AreEqual(dt, false);
        }

        [Test]
        public void MethodStyle_REMOVESTART_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'123456789'.REMOVESTART('12')", null);
            Assert.AreEqual(dt, "3456789");

            dt = engine.TryEvaluate("'123456789'.REMOVESTART('127')", null);
            Assert.AreEqual(dt, "123456789");
        }

        [Test]
        public void MethodStyle_REMOVEEND_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'123456789'.REMOVEEND('89')", null);
            Assert.AreEqual(dt, "1234567");

            dt = engine.TryEvaluate("'123456789'.REMOVEEND('127')", null);
            Assert.AreEqual(dt, "123456789");
        }

        [Test]
        public void MethodStyle_JSON_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'{\"Name\":\"William Shakespeare\",\"Age\":51}'.JSON()['Age']", null);
            Assert.AreEqual(dt.ToString(), "51");
        }

        [Test]
        public void MethodStyle_HAS_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("{\"name\":\"toolgood\",\"age\":\"12\"}.HAS('age')", false);
            Assert.AreEqual(dt, true);

            dt = engine.TryEvaluate("{\"name\":\"toolgood\",\"age\":\"12\"}.HASKEY('age')", false);
            Assert.AreEqual(dt, true);

            dt = engine.TryEvaluate("{\"name\":\"toolgood\",\"age\":\"12\"}.CONTAINS('age')", false);
            Assert.AreEqual(dt, true);

            dt = engine.TryEvaluate("{\"name\":\"toolgood\",\"age\":\"12\"}.CONTAINSKEY('age')", false);
            Assert.AreEqual(dt, true);
        }

        [Test]
        public void MethodStyle_HASVALUE_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("{\"name\":\"toolgood\",\"age\":\"12\"}.HASVALUE('toolgood')", false);
            Assert.AreEqual(dt, true);

            dt = engine.TryEvaluate("{\"name\":\"toolgood\",\"age\":\"12\"}.CONTAINSVALUE('toolgood')", false);
            Assert.AreEqual(dt, true);
        }

        #endregion 方法式调用测试 - 字符串扩展方法

      [Test]
        public void LookFloor_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();

            var num = engine.TryEvaluate("LookFloor(2,[0,1,2,3,4])", 0);
            Assert.AreEqual(num, 2);

            num = engine.TryEvaluate("LookFloor(2.1,[0,1,2,3,4])", 0);
            Assert.AreEqual(num, 2);

            num = engine.TryEvaluate("LookFloor(-2.1,[0,1,2,3,4])", 0);
            Assert.AreEqual(num, 0);

            num = engine.TryEvaluate("LookFloor(5,[0,1,2,3,4])", 0);
            Assert.AreEqual(num, 4);
        }

        [Test]
        public void LookCeiling_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();

            var num = engine.TryEvaluate("LookCeiling(2,[0,1,2,3,4])", 0);
            Assert.AreEqual(num, 2);

            num = engine.TryEvaluate("LookCeiling(2.1,[0,1,2,3,4])", 0);
            Assert.AreEqual(num, 3);

            num = engine.TryEvaluate("LookCeiling(-2.1,[0,1,2,3,4])", 0);
            Assert.AreEqual(num, 0);

            num = engine.TryEvaluate("LookCeiling(5,[0,1,2,3,4])", 0);
            Assert.AreEqual(num, 4);
        }
    
    }
}
