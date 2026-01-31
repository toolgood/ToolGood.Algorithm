using PetaTest;

namespace ToolGood.Algorithm.Test
{
    [TestFixture]
    internal partial class AlgorithmEngineTest_csharp
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

            r = engine.TryEvaluate("TextToBase64('&=我中国人 >||','GBK')", null);
            Assert.AreEqual(r, "Jj3O0tbQufrIyyA+fHw=");
        }

        [Test]
        public void TextToBase64Url()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var r = engine.TryEvaluate("TextToBase64Url('&=我中国人 >||')", null);
            Assert.AreEqual(r, "Jj3miJHkuK3lm73kurogPnx8");

            r = engine.TryEvaluate("TextToBase64Url('&=我中国人 >||','GBK')", null);
            Assert.AreEqual(r, "Jj3O0tbQufrIyyA-fHw");
        }

        [Test]
        public void Base64ToText()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var r = engine.TryEvaluate("Base64ToText('Jj3miJHkuK3lm73kurogPnx8')", null);
            Assert.AreEqual(r, "&=我中国人 >||");

            r = engine.TryEvaluate("Base64ToText('Jj3O0tbQufrIyyA+fHw=','GBK')", null);
            Assert.AreEqual(r, "&=我中国人 >||");
        }

        [Test]
        public void Base64UrlToText()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var r = engine.TryEvaluate("Base64UrlToText('Jj3miJHkuK3lm73kurogPnx8')", null);
            Assert.AreEqual(r, "&=我中国人 >||");

            r = engine.TryEvaluate("Base64UrlToText('Jj3O0tbQufrIyyA-fHw','GBK')", null);
            Assert.AreEqual(r, "&=我中国人 >||");
        }

        [Test]
        public void Regex_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            engine.UseExcelIndex = false;
            var r = engine.TryEvaluate("Regex('abcd','a.*c')", null);
            Assert.AreEqual(r, "abc");
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
        public void Guid()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var r = engine.TryEvaluate("Guid()", "");
        }

        [Test]
        public void Hash()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("Md5('&=我中国人 >||')", null);
            Assert.AreEqual(dt, "2E1CEFBDFA3677725B7856E02D225819");
			dt = engine.TryEvaluate("Md5('123')", null);
			Assert.AreEqual(dt, "202CB962AC59075B964B07152D234B70");
			dt = engine.TryEvaluate("Md5('&=我中国人 >||','GGG')", null);
            Assert.AreEqual(dt, null);

            dt = engine.TryEvaluate("Sha1('&=我中国人 >||')", null);
            Assert.AreEqual(dt, "F2C250C58F3A40DC54B5A47F0F6B1187AD5AC2EE");
			dt = engine.TryEvaluate("Sha1('123')", null);
			Assert.AreEqual(dt, "40BD001563085FC35165329EA1FF5C5ECBDBBEEF");

			dt = engine.TryEvaluate("Sha256('&=我中国人 >||')", null);
            Assert.AreEqual(dt, "FA5BF04D13AEF750D62040E492479A16B6B10888D0B19923A1E7B9339990632A");
			dt = engine.TryEvaluate("Sha256('123')", null);
			Assert.AreEqual(dt, "A665A45920422F9D417E4867EFDC4FB8A04A1F3FFF1FA07E998E86F7F7A27AE3");

			dt = engine.TryEvaluate("Sha512('&=我中国人 >||')", null);
            Assert.AreEqual(dt, "FFEAC98C39D76CD86A3AB8ECEF16BE23166F68E1A3C5C9809A8AD2CE417170465286E4CF6FFA17924613CD7477533B9109A5DD504A2462F9DB693D56AD365C14");
			dt = engine.TryEvaluate("Sha512('123')", null);
			Assert.AreEqual(dt, "3C9909AFEC25354D551DAE21590BB26E38D53F2173B8D3DC3EEE4C047E7AB1C1EB8B85103E3BE7BA613B31BB5C9C36214DC9F14A42FD7A2FDB84856BCA5C44C2");


			dt = engine.TryEvaluate("HmacMd5('&=我中国人 >||','12')", null);
            Assert.AreEqual(dt, "CF3923196E21B1E270FD72B089B092BB");
			dt = engine.TryEvaluate("HmacMd5('123','123')", null);
			Assert.AreEqual(dt, "B2A1EC0F3E0607099D7F39791C04E9A4");


			dt = engine.TryEvaluate("HmacSha1('&=我中国人 >||','12')", null);
            Assert.AreEqual(dt, "EB4D4FC2AA5637060FD12004DF845801D8902105");
			dt = engine.TryEvaluate("HmacSha1('123','123')", null);
			Assert.AreEqual(dt, "A3C024F01CCCB3B63457D848B0D2F89C1F744A3D");

			dt = engine.TryEvaluate("HmacSha256('&=我中国人 >||','12')", null);
            Assert.AreEqual(dt, "3E25E0D14039E8258BBBBD15F7E3B91BB497A8966C12E1DEA3D651BF03CB4B97");
			dt = engine.TryEvaluate("HmacSha256('123','123')", null);
			Assert.AreEqual(dt, "3CAFE40F92BE6AC77D2792B4B267C2DA11E3F3087B93BB19C6C5133786984B44");

			dt = engine.TryEvaluate("HmacSha512('&=我中国人 >||','12')", null);
            Assert.AreEqual(dt, "4E9CE785C46569965C7C712A841EC7382C64D918D49F992EDB3504BED9C3A5EFBB1C8F712968F6B904417E07F9D72E707FCF148D55A4D3EDF1A9866B7BAC2049");
			dt = engine.TryEvaluate("HmacSha512('123','123')", null);
			Assert.AreEqual(dt, "0634FD04380BBAF5069C8C46A74C7D21DF7414888D980C27A16D5E262CB8C9059139C212D0926000FAF026E483904CEFAE2F5E9D9BD5F51FBC2AC4C4DE518115");
		}

        //IndexOf
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
        public void TrimEnd()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("TrimEnd(' 123 ')", null);
            Assert.AreEqual(dt, " 123");

            dt = engine.TryEvaluate("TrimEnd(' 123 ','3 ')", null);
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
        public void Json()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare\",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}').Age", null);
            Assert.AreEqual(dt.ToString(), "51");
            dt = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare\",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Birthday']", null);
            Assert.AreEqual(dt, "04/26/1564 00:00:00");

            dt = engine.TryEvaluate("json('[1,2,3]')[1]", null);
            Assert.AreEqual(dt, "1");

            dt = engine.TryEvaluate("json('{\"Name\":\"William Shakespeare   \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Name'].Trim()", null);
            Assert.AreEqual(dt, "William Shakespeare");

            dt = engine.TryEvaluate("json('{\"Name1\":\"William Shakespeare \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Name'& 1].Trim().substring(2,3)", null);
            Assert.AreEqual(dt, "ill");

            dt = engine.TryEvaluate("json('12346{\"Name1\":\"William Shakespeare \",\"Age\":51,\"Birthday\":\"04/26/1564 00:00:00\"}')['Name'& 1].Trim().substring(2,3)", null);
            Assert.AreEqual(dt, null);

            dt = engine.TryEvaluate("json('[1,2,3,4,5,6]')[1].Trim()", null);
            Assert.AreEqual(dt, "1");

            dt = engine.TryEvaluate("json('[1,2,3,4,5,6]22')[1].Trim()", null);
            Assert.AreEqual(dt, null);

            dt = engine.TryEvaluate("json('22[1,2,3,4,5,6]')[1].Trim()", null);
            Assert.AreEqual(dt, null);

			var dt2 = engine.TryEvaluate("json('{\"w11\":true}')['w11']", false);
			Assert.AreEqual(dt2, true);

            var dt3 = engine.TryEvaluate("{\"w11\":false}", "");
            Assert.AreEqual(dt3.ToString(), "{\"w11\":false}");

			dt3 = engine.TryEvaluate("[1,2,3,4]", "");
			Assert.AreEqual(dt3.ToString(), "[1,2,3,4]");


		}
	}
}