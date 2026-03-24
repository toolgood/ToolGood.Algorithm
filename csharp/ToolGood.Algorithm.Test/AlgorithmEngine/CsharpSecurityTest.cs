using PetaTest;

namespace ToolGood.Algorithm.Test.CsharpSecurity
{
    [TestFixture]
    internal class CsharpSecurityTest
    {
        [Test]
        public void Md5_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("Md5('&=我中国人 >||')", null);
            Assert.AreEqual(dt, "2E1CEFBDFA3677725B7856E02D225819");
            dt = engine.TryEvaluate("Md5('123')", null);
            Assert.AreEqual(dt, "202CB962AC59075B964B07152D234B70");
            dt = engine.TryEvaluate("Md5('&=我中国人 >||','GGG')", null);
            Assert.AreEqual(dt, null);
        }

        [Test]
        public void Sha1_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("Sha1('&=我中国人 >||')", null);
            Assert.AreEqual(dt, "F2C250C58F3A40DC54B5A47F0F6B1187AD5AC2EE");
            dt = engine.TryEvaluate("Sha1('123')", null);
            Assert.AreEqual(dt, "40BD001563085FC35165329EA1FF5C5ECBDBBEEF");
        }

        [Test]
        public void Sha256_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("Sha256('&=我中国人 >||')", null);
            Assert.AreEqual(dt, "FA5BF04D13AEF750D62040E492479A16B6B10888D0B19923A1E7B9339990632A");
            dt = engine.TryEvaluate("Sha256('123')", null);
            Assert.AreEqual(dt, "A665A45920422F9D417E4867EFDC4FB8A04A1F3FFF1FA07E998E86F7F7A27AE3");
        }

        [Test]
        public void Sha512_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("Sha512('&=我中国人 >||')", null);
            Assert.AreEqual(dt, "FFEAC98C39D76CD86A3AB8ECEF16BE23166F68E1A3C5C9809A8AD2CE417170465286E4CF6FFA17924613CD7477533B9109A5DD504A2462F9DB693D56AD365C14");
            dt = engine.TryEvaluate("Sha512('123')", null);
            Assert.AreEqual(dt, "3C9909AFEC25354D551DAE21590BB26E38D53F2173B8D3DC3EEE4C047E7AB1C1EB8B85103E3BE7BA613B31BB5C9C36214DC9F14A42FD7A2FDB84856BCA5C44C2");
        }

        [Test]
        public void HmacMd5_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("HmacMd5('&=我中国人 >||','12')", null);
            Assert.AreEqual(dt, "CF3923196E21B1E270FD72B089B092BB");
            dt = engine.TryEvaluate("HmacMd5('123','123')", null);
            Assert.AreEqual(dt, "B2A1EC0F3E0607099D7F39791C04E9A4");
        }

        [Test]
        public void HmacSha1_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("HmacSha1('&=我中国人 >||','12')", null);
            Assert.AreEqual(dt, "EB4D4FC2AA5637060FD12004DF845801D8902105");
            dt = engine.TryEvaluate("HmacSha1('123','123')", null);
            Assert.AreEqual(dt, "A3C024F01CCCB3B63457D848B0D2F89C1F744A3D");
        }

        [Test]
        public void HmacSha256_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("HmacSha256('&=我中国人 >||','12')", null);
            Assert.AreEqual(dt, "3E25E0D14039E8258BBBBD15F7E3B91BB497A8966C12E1DEA3D651BF03CB4B97");
            dt = engine.TryEvaluate("HmacSha256('123','123')", null);
            Assert.AreEqual(dt, "3CAFE40F92BE6AC77D2792B4B267C2DA11E3F3087B93BB19C6C5133786984B44");
        }

        [Test]
        public void HmacSha512_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("HmacSha512('&=我中国人 >||','12')", null);
            Assert.AreEqual(dt, "4E9CE785C46569965C7C712A841EC7382C64D918D49F992EDB3504BED9C3A5EFBB1C8F712968F6B904417E07F9D72E707FCF148D55A4D3EDF1A9866B7BAC2049");
            dt = engine.TryEvaluate("HmacSha512('123','123')", null);
            Assert.AreEqual(dt, "0634FD04380BBAF5069C8C46A74C7D21DF7414888D980C27A16D5E262CB8C9059139C212D0926000FAF026E483904CEFAE2F5E9D9BD5F51FBC2AC4C4DE518115");
        }

        [Test]
        public void Guid()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var r = engine.TryEvaluate("Guid()", "");
        }

        #region 方法式调用测试 - 加密类

        [Test]
        public void MethodStyle_MD5_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'123'.MD5()", null);
            Assert.AreEqual(dt, "202CB962AC59075B964B07152D234B70");
        }

        [Test]
        public void MethodStyle_SHA1_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'123'.SHA1()", null);
            Assert.AreEqual(dt, "40BD001563085FC35165329EA1FF5C5ECBDBBEEF");
        }

        [Test]
        public void MethodStyle_SHA256_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'123'.SHA256()", null);
            Assert.AreEqual(dt, "A665A45920422F9D417E4867EFDC4FB8A04A1F3FFF1FA07E998E86F7F7A27AE3");
        }

        [Test]
        public void MethodStyle_SHA512_test()
        {
            AlgorithmEngine engine = new AlgorithmEngine();
            var dt = engine.TryEvaluate("'123'.SHA512()", null);
            Assert.AreEqual(dt, "3C9909AFEC25354D551DAE21590BB26E38D53F2173B8D3DC3EEE4C047E7AB1C1EB8B85103E3BE7BA613B31BB5C9C36214DC9F14A42FD7A2FDB84856BCA5C44C2");
        }

        #endregion 方法式调用测试 - 加密类
    }
}
