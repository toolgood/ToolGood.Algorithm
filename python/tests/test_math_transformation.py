"""Port of ``MathTransformationTest.java``."""
from __future__ import annotations

from toolgood_algorithm import AlgorithmEngine


def _places_error(formula: str) -> bool:
    engine = AlgorithmEngine()
    engine.try_evaluate(formula, "")
    return engine.LastError is not None


def test_bin2dec():
    engine = AlgorithmEngine()
    num = engine.try_evaluate("BIN2DEC(10101)", 0)
    assert num == 21


def test_oct2dec():
    engine = AlgorithmEngine()
    num = engine.try_evaluate("OCT2DEC(12456)", 0)
    assert num == 5422


def test_hex2dec():
    engine = AlgorithmEngine()
    num = engine.try_evaluate("HEX2DEC('213adf')", 0)
    assert num == 2177759


def test_dec2bin():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("DEC2BIN(10)", "")
    assert t == "1010"
    t = engine.try_evaluate("DEC2BIN(10,8)", "")
    assert t == "00001010"


def test_oct2bin():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("OCT2BIN('721')", "")
    assert t == "111010001"


def test_hex2bin():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("HEX2BIN('fa')", "")
    assert t == "11111010"


def test_bin2oct():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("BIN2OCT(10)", "")
    assert t == "2"


def test_dec2oct():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("DEC2OCT('75')", "")
    assert t == "113"


def test_hex2oct():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("HEX2OCT('f5')", "")
    assert t == "365"


def test_bin2hex():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("BIN2HEX(101010100)", "")
    assert t == "154"


def test_oct2hex():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("OCT2HEX(75212)", "")
    assert t == "7A8A"


def test_dec2hex():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("DEC2HEX(952)", "")
    assert t == "3B8"


def test_arabic():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("ARABIC('I')", 0)
    assert t == 1

    t = engine.try_evaluate("ARABIC('IV')", 0)
    assert t == 4

    t = engine.try_evaluate("ARABIC('IX')", 0)
    assert t == 9

    t = engine.try_evaluate("ARABIC('X')", 0)
    assert t == 10

    t = engine.try_evaluate("ARABIC('XL')", 0)
    assert t == 40

    t = engine.try_evaluate("ARABIC('L')", 0)
    assert t == 50

    t = engine.try_evaluate("ARABIC('XC')", 0)
    assert t == 90

    t = engine.try_evaluate("ARABIC('C')", 0)
    assert t == 100

    t = engine.try_evaluate("ARABIC('CD')", 0)
    assert t == 400

    t = engine.try_evaluate("ARABIC('D')", 0)
    assert t == 500

    t = engine.try_evaluate("ARABIC('CM')", 0)
    assert t == 900

    t = engine.try_evaluate("ARABIC('M')", 0)
    assert t == 1000

    t = engine.try_evaluate("ARABIC('MMXXIII')", 0)
    assert t == 2023


def test_roman():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("ROMAN(1)", "")
    assert t == "I"

    t = engine.try_evaluate("ROMAN(4)", "")
    assert t == "IV"

    t = engine.try_evaluate("ROMAN(9)", "")
    assert t == "IX"

    t = engine.try_evaluate("ROMAN(10)", "")
    assert t == "X"

    t = engine.try_evaluate("ROMAN(40)", "")
    assert t == "XL"

    t = engine.try_evaluate("ROMAN(50)", "")
    assert t == "L"

    t = engine.try_evaluate("ROMAN(90)", "")
    assert t == "XC"

    t = engine.try_evaluate("ROMAN(100)", "")
    assert t == "C"

    t = engine.try_evaluate("ROMAN(400)", "")
    assert t == "CD"

    t = engine.try_evaluate("ROMAN(500)", "")
    assert t == "D"

    t = engine.try_evaluate("ROMAN(900)", "")
    assert t == "CM"

    t = engine.try_evaluate("ROMAN(1000)", "")
    assert t == "M"

    t = engine.try_evaluate("ROMAN(2023)", "")
    assert t == "MMXXIII"


def test_arabic_error():
    engine = AlgorithmEngine()
    # 空文本报错(修复前返回 0)
    t = engine.try_evaluate("ARABIC('')", 0)
    assert engine.LastError is not None

    engine = AlgorithmEngine()
    # 非法字符报错(修复前静默忽略非法字符)
    t = engine.try_evaluate("ARABIC('ABC')", 0)
    assert engine.LastError is not None

    # 小写输入支持(内部转大写)
    t = engine.try_evaluate("ARABIC('mcm')", 0)
    assert t == 1900

    # Excel 官方例子
    t = engine.try_evaluate("ARABIC('MCMLXXII')", 0)
    assert t == 1972


def test_roman_form():
    engine = AlgorithmEngine()
    # Excel 官方 form 0~4 对 499 的不同精简形式
    t = engine.try_evaluate("ROMAN(499,0)", "")
    assert t == "CDXCIX"

    t = engine.try_evaluate("ROMAN(499,1)", "")
    assert t == "LDVLIV"

    t = engine.try_evaluate("ROMAN(499,2)", "")
    assert t == "XDIX"

    t = engine.try_evaluate("ROMAN(499,3)", "")
    assert t == "VDIV"

    t = engine.try_evaluate("ROMAN(499,4)", "")
    assert t == "ID"

    # ROMAN(0) 返回空文本
    t = engine.try_evaluate("ROMAN(0)", "")
    assert t == ""

    # number 超出 0~3999 报错(修复前返回空文本)
    t = engine.try_evaluate("ROMAN(-1)", "")
    assert engine.LastError is not None

    engine = AlgorithmEngine()
    t = engine.try_evaluate("ROMAN(4000)", "")
    assert engine.LastError is not None

    engine = AlgorithmEngine()
    # form 超出 0~4 报错
    t = engine.try_evaluate("ROMAN(499,5)", "")
    assert engine.LastError is not None

    engine = AlgorithmEngine()
    t = engine.try_evaluate("ROMAN(499,-1)", "")
    assert engine.LastError is not None


def test_arabic_invalid():
    engine = AlgorithmEngine()
    # 非法重复:IIII 不是标准罗马数字
    t = engine.try_evaluate("ARABIC('IIII')", 0)
    assert engine.LastError is not None

    engine = AlgorithmEngine()
    # 非法减法:IC 不是标准罗马数字
    t = engine.try_evaluate("ARABIC('IC')", 0)
    assert engine.LastError is not None

    engine = AlgorithmEngine()
    # 非法重复:VV 不是标准罗马数字
    t = engine.try_evaluate("ARABIC('VV')", 0)
    assert engine.LastError is not None


def test_places_out_of_range():
    # places 超过 10 应返回 #NUM!
    assert _places_error("DEC2BIN(10,20)") is True
    assert _places_error("DEC2HEX(952,20)") is True
    assert _places_error("DEC2OCT(75,20)") is True
    assert _places_error("BIN2OCT(10,20)") is True
    assert _places_error("BIN2HEX(101010100,20)") is True
    assert _places_error("OCT2BIN(721,20)") is True
    assert _places_error("OCT2HEX(75212,20)") is True
    assert _places_error("HEX2BIN('fa',20)") is True
    assert _places_error("HEX2OCT('f5',20)") is True


def test_negative_places_padding():
    engine = AlgorithmEngine()
    # 10 位补码解析为 -1,places=3 时应输出 "-01" 而非 "0-1"
    t = engine.try_evaluate("BIN2DEC('1111111111',3)", "")
    assert t == "-01"

    t = engine.try_evaluate("OCT2DEC('7777777777',3)", "")
    assert t == "-01"

    t = engine.try_evaluate("HEX2DEC('FFFFFFFFFF',3)", "")
    assert t == "-01"
