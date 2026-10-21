"""Tests ported from ``java/.../AlgorithmEngine/CsharpSecurityTest.java``.

安全哈希测试：MD5, SHA1, SHA256, SHA512, HMAC, Guid。
注意：Java 实现的哈希输出为**大写十六进制**，断言保持大写。
"""
from __future__ import annotations

from toolgood_algorithm import AlgorithmEngine


def test_md5():
    # Java: CsharpSecurityTest.java:14
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("Md5('&=我中国人 >||')", None)
    assert dt == "2E1CEFBDFA3677725B7856E02D225819"
    dt = engine.try_evaluate_text("Md5('123')", None)
    assert dt == "202CB962AC59075B964B07152D234B70"
    dt = engine.try_evaluate_text("Md5('&=我中国人 >||','GGG')", None)
    assert dt is None


def test_sha1():
    # Java: CsharpSecurityTest.java:25
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("Sha1('&=我中国人 >||')", None)
    assert dt == "F2C250C58F3A40DC54B5A47F0F6B1187AD5AC2EE"
    dt = engine.try_evaluate_text("Sha1('123')", None)
    assert dt == "40BD001563085FC35165329EA1FF5C5ECBDBBEEF"


def test_sha256():
    # Java: CsharpSecurityTest.java:34
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("Sha256('&=我中国人 >||')", None)
    assert dt == "FA5BF04D13AEF750D62040E492479A16B6B10888D0B19923A1E7B9339990632A"
    dt = engine.try_evaluate_text("Sha256('123')", None)
    assert dt == "A665A45920422F9D417E4867EFDC4FB8A04A1F3FFF1FA07E998E86F7F7A27AE3"


def test_sha512():
    # Java: CsharpSecurityTest.java:43
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("Sha512('&=我中国人 >||')", None)
    assert dt == (
        "FFEAC98C39D76CD86A3AB8ECEF16BE23166F68E1A3C5C9809A8AD2CE41717046"
        "5286E4CF6FFA17924613CD7477533B9109A5DD504A2462F9DB693D56AD365C14"
    )
    dt = engine.try_evaluate_text("Sha512('123')", None)
    assert dt == (
        "3C9909AFEC25354D551DAE21590BB26E38D53F2173B8D3DC3EEE4C047E7AB1C1"
        "EB8B85103E3BE7BA613B31BB5C9C36214DC9F14A42FD7A2FDB84856BCA5C44C2"
    )


def test_hmac_md5():
    # Java: CsharpSecurityTest.java:52
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("HmacMd5('&=我中国人 >||','12')", None)
    assert dt == "CF3923196E21B1E270FD72B089B092BB"
    dt = engine.try_evaluate_text("HmacMd5('123','123')", None)
    assert dt == "B2A1EC0F3E0607099D7F39791C04E9A4"


def test_hmac_sha1():
    # Java: CsharpSecurityTest.java:61
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("HmacSha1('&=我中国人 >||','12')", None)
    assert dt == "EB4D4FC2AA5637060FD12004DF845801D8902105"
    dt = engine.try_evaluate_text("HmacSha1('123','123')", None)
    assert dt == "A3C024F01CCCB3B63457D848B0D2F89C1F744A3D"


def test_hmac_sha256():
    # Java: CsharpSecurityTest.java:70
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("HmacSha256('&=我中国人 >||','12')", None)
    assert dt == "3E25E0D14039E8258BBBBD15F7E3B91BB497A8966C12E1DEA3D651BF03CB4B97"
    dt = engine.try_evaluate_text("HmacSha256('123','123')", None)
    assert dt == "3CAFE40F92BE6AC77D2792B4B267C2DA11E3F3087B93BB19C6C5133786984B44"


def test_hmac_sha512():
    # Java: CsharpSecurityTest.java:79
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("HmacSha512('&=我中国人 >||','12')", None)
    assert dt == (
        "4E9CE785C46569965C7C712A841EC7382C64D918D49F992EDB3504BED9C3A5EF"
        "BB1C8F712968F6B904417E07F9D72E707FCF148D55A4D3EDF1A9866B7BAC2049"
    )
    dt = engine.try_evaluate_text("HmacSha512('123','123')", None)
    assert dt == (
        "0634FD04380BBAF5069C8C46A74C7D21DF7414888D980C27A16D5E262CB8C905"
        "9139C212D0926000FAF026E483904CEFAE2F5E9D9BD5F51FBC2AC4C4DE518115"
    )


def test_guid():
    # Java: CsharpSecurityTest.java:88
    engine = AlgorithmEngine()
    r = engine.try_evaluate_text("Guid()", "")
    # Guid 每次调用返回不同的值
    assert r is not None


# region 方法式调用测试 - 加密类


def test_method_style_md5():
    # Java: CsharpSecurityTest.java:98
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("'123'.MD5()", None)
    assert dt == "202CB962AC59075B964B07152D234B70"


def test_method_style_sha1():
    # Java: CsharpSecurityTest.java:105
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("'123'.SHA1()", None)
    assert dt == "40BD001563085FC35165329EA1FF5C5ECBDBBEEF"


def test_method_style_sha256():
    # Java: CsharpSecurityTest.java:112
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("'123'.SHA256()", None)
    assert dt == "A665A45920422F9D417E4867EFDC4FB8A04A1F3FFF1FA07E998E86F7F7A27AE3"


def test_method_style_sha512():
    # Java: CsharpSecurityTest.java:119
    engine = AlgorithmEngine()
    dt = engine.try_evaluate_text("'123'.SHA512()", None)
    assert dt == (
        "3C9909AFEC25354D551DAE21590BB26E38D53F2173B8D3DC3EEE4C047E7AB1C1"
        "EB8B85103E3BE7BA613B31BB5C9C36214DC9F14A42FD7A2FDB84856BCA5C44C2"
    )


# endregion
