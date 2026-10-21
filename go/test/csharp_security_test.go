package test

import (
	"testing"
)

// TestCsharpSecurityTest 对应 csharp/ToolGood.Algorithm.Test/AlgorithmEngine/CsharpSecurityTest.cs。
func TestCsharpSecurityTest(t *testing.T) {
	t.Run("Md5_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "2E1CEFBDFA3677725B7856E02D225819", evalText(engine, "Md5('&=我中国人 >||')"))
		assertText(t, "202CB962AC59075B964B07152D234B70", evalText(engine, "Md5('123')"))
		// 两个参数在解析期即被拒（对应 C# 返回 null）
		assertNull(t, engine.TryEvaluate("Md5('&=我中国人 >||','GGG')", nil))
	})

	t.Run("Sha1_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "F2C250C58F3A40DC54B5A47F0F6B1187AD5AC2EE", evalText(engine, "Sha1('&=我中国人 >||')"))
		assertText(t, "40BD001563085FC35165329EA1FF5C5ECBDBBEEF", evalText(engine, "Sha1('123')"))
	})

	t.Run("Sha256_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "FA5BF04D13AEF750D62040E492479A16B6B10888D0B19923A1E7B9339990632A", evalText(engine, "Sha256('&=我中国人 >||')"))
		assertText(t, "A665A45920422F9D417E4867EFDC4FB8A04A1F3FFF1FA07E998E86F7F7A27AE3", evalText(engine, "Sha256('123')"))
	})

	t.Run("Sha512_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "FFEAC98C39D76CD86A3AB8ECEF16BE23166F68E1A3C5C9809A8AD2CE417170465286E4CF6FFA17924613CD7477533B9109A5DD504A2462F9DB693D56AD365C14", evalText(engine, "Sha512('&=我中国人 >||')"))
		assertText(t, "3C9909AFEC25354D551DAE21590BB26E38D53F2173B8D3DC3EEE4C047E7AB1C1EB8B85103E3BE7BA613B31BB5C9C36214DC9F14A42FD7A2FDB84856BCA5C44C2", evalText(engine, "Sha512('123')"))
	})

	t.Run("HmacMd5_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "CF3923196E21B1E270FD72B089B092BB", evalText(engine, "HmacMd5('&=我中国人 >||','12')"))
		assertText(t, "B2A1EC0F3E0607099D7F39791C04E9A4", evalText(engine, "HmacMd5('123','123')"))
	})

	t.Run("HmacSha1_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "EB4D4FC2AA5637060FD12004DF845801D8902105", evalText(engine, "HmacSha1('&=我中国人 >||','12')"))
		assertText(t, "A3C024F01CCCB3B63457D848B0D2F89C1F744A3D", evalText(engine, "HmacSha1('123','123')"))
	})

	t.Run("HmacSha256_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "3E25E0D14039E8258BBBBD15F7E3B91BB497A8966C12E1DEA3D651BF03CB4B97", evalText(engine, "HmacSha256('&=我中国人 >||','12')"))
		assertText(t, "3CAFE40F92BE6AC77D2792B4B267C2DA11E3F3087B93BB19C6C5133786984B44", evalText(engine, "HmacSha256('123','123')"))
	})

	t.Run("HmacSha512_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "4E9CE785C46569965C7C712A841EC7382C64D918D49F992EDB3504BED9C3A5EFBB1C8F712968F6B904417E07F9D72E707FCF148D55A4D3EDF1A9866B7BAC2049", evalText(engine, "HmacSha512('&=我中国人 >||','12')"))
		assertText(t, "0634FD04380BBAF5069C8C46A74C7D21DF7414888D980C27A16D5E262CB8C9059139C212D0926000FAF026E483904CEFAE2F5E9D9BD5F51FBC2AC4C4DE518115", evalText(engine, "HmacSha512('123','123')"))
	})

	t.Run("Guid", func(t *testing.T) {
		engine := newEngine()
		// C# 版本只调用不断言；此处同样只验证能够求值。
		_ = evalText(engine, "Guid()")
	})

	// ------------------------------------------------------------------ 方法式调用测试 - 加密类

	t.Run("MethodStyle_MD5_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "202CB962AC59075B964B07152D234B70", evalText(engine, "'123'.MD5()"))
	})

	t.Run("MethodStyle_SHA1_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "40BD001563085FC35165329EA1FF5C5ECBDBBEEF", evalText(engine, "'123'.SHA1()"))
	})

	t.Run("MethodStyle_SHA256_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "A665A45920422F9D417E4867EFDC4FB8A04A1F3FFF1FA07E998E86F7F7A27AE3", evalText(engine, "'123'.SHA256()"))
	})

	t.Run("MethodStyle_SHA512_test", func(t *testing.T) {
		engine := newEngine()
		assertText(t, "3C9909AFEC25354D551DAE21590BB26E38D53F2173B8D3DC3EEE4C047E7AB1C1EB8B85103E3BE7BA613B31BB5C9C36214DC9F14A42FD7A2FDB84856BCA5C44C2", evalText(engine, "'123'.SHA512()"))
	})
}
