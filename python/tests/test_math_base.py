"""Port of ``MathBaseTest.java``."""
from __future__ import annotations

import pytest

from toolgood_algorithm import AlgorithmEngine


def test_pi():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("pi()", 0.0)
    assert t == pytest.approx(3.141592654, abs=1e-9)


def test_abs():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("abs(-1.2)", 0.0)
    assert t == pytest.approx(1.2, abs=0.0)


def test_quotient():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("QUOTIENT(7,3)", 0.0)
    assert t == pytest.approx(2.0, abs=0.0)


def test_mod():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("MOD(7,3)", 0.0)
    assert t == pytest.approx(1.0, abs=0.0)


def test_sign():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("SIGN(0)", 0)
    assert t == 0
    t = engine.try_evaluate("SIGN(9)", 0)
    assert t == 1
    t = engine.try_evaluate("SIGN(-9)", 0)
    assert t == -1


def test_sqrt():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("SQRT(9)", 0.0)
    assert t == pytest.approx(3.0, abs=0.0)


def test_sum():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("SUM(1,2,3,4)", 0.0)
    assert t == pytest.approx(10.0, abs=0.0)


def test_trunc():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("TRUNC(9.222)", 0.0)
    assert t == pytest.approx(9.0, abs=0.0)
    t = engine.try_evaluate("TRUNC(-9.222)", 0.0)
    assert t == pytest.approx(-9.0, abs=0.0)


def test_trunc_excel_compatible():
    engine = AlgorithmEngine()

    t = engine.try_evaluate("TRUNC(8.9)", 0.0)
    assert t == pytest.approx(8.0, abs=0.0)

    t = engine.try_evaluate("TRUNC(8.912, 2)", 0.0)
    assert t == pytest.approx(8.91, abs=0.0)

    t = engine.try_evaluate("TRUNC(8.912, 1)", 0.0)
    assert t == pytest.approx(8.9, abs=0.0)

    t = engine.try_evaluate("TRUNC(-5.5)", 0.0)
    assert t == pytest.approx(-5.0, abs=0.0)

    t = engine.try_evaluate("TRUNC(123.456, -2)", 0.0)
    assert t == pytest.approx(100.0, abs=0.0)

    t = engine.try_evaluate("TRUNC(123.456, -1)", 0.0)
    assert t == pytest.approx(120.0, abs=0.0)

    t = engine.try_evaluate("TRUNC(-123.456, -2)", 0.0)
    assert t == pytest.approx(-100.0, abs=0.0)

    t = engine.try_evaluate("TRUNC(0)", 0.0)
    assert t == pytest.approx(0.0, abs=0.0)

    t = engine.try_evaluate("TRUNC(PI(), 4)", 0.0)
    assert t == pytest.approx(3.1415, abs=1e-4)


def test_int():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("int(9.222)", 0.0)
    assert t == pytest.approx(9.0, abs=0.0)
    t = engine.try_evaluate("int(-9.222)", 0.0)
    assert t == pytest.approx(-10.0, abs=0.0)


def test_gcd():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("GCD(3,5,7)", 0.0)
    assert t == pytest.approx(1.0, abs=0.0)
    t = engine.try_evaluate("GCD(30,21)", 0.0)
    assert t == pytest.approx(3.0, abs=0.0)


def test_lcm():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("LCM(3,5,7)", 0.0)
    assert t == pytest.approx(105.0, abs=0.0)


def test_combin():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("combin(10,2)", 0.0)
    assert t == pytest.approx(45.0, abs=0.0)


def test_permut():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("PERMUT(10,2)", 0.0)
    assert t == pytest.approx(90.0, abs=0.0)


def test_round():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("ROUND(4.333,2)", 0.0)
    assert t == pytest.approx(4.33, abs=0.0)


def test_round_single_param():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("ROUND(4.5)", 0.0)
    assert t == pytest.approx(5.0, abs=0.0)

    t = engine.try_evaluate("ROUND(4.4)", 0.0)
    assert t == pytest.approx(4.0, abs=0.0)

    t = engine.try_evaluate("ROUND(-4.5)", 0.0)
    assert t == pytest.approx(-5.0, abs=0.0)


def test_rounddown():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("ROUNDDOWN(4.333,2)", 0.0)
    assert t == pytest.approx(4.33, abs=0.0)

    t = engine.try_evaluate("ROUNDDOWN(-3.141592, 3)", 0.0)
    assert t == pytest.approx(-3.141, abs=0.0)


def test_roundup():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("ROUNDUP(4.333,2)", 0.0)
    assert t == pytest.approx(4.34, abs=0.0)

    t = engine.try_evaluate("ROUNDUP(-3.141592, 3)", 0.0)
    assert t == pytest.approx(-3.142, abs=0.0)


def test_ceiling():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("CEILING(4.333,0.1)", 0.0)
    assert t == pytest.approx(4.4, abs=0.0)

    t = engine.try_evaluate("CEILING(4.333)", 0.0)
    assert t == pytest.approx(5.0, abs=0.0)


def test_floor():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("FLOOR(4.363,0.1)", 0.0)
    assert t == pytest.approx(4.3, abs=0.0)

    t = engine.try_evaluate("FLOOR(4.333)", 0.0)
    assert t == pytest.approx(4.0, abs=0.0)


def test_even():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("even(4.363)", 0.0)
    assert t == pytest.approx(6.0, abs=0.0)


def test_odd():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("odd(4.363)", 0.0)
    assert t == pytest.approx(5.0, abs=0.0)


def test_mround():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("MROUND(4.363,2)", 0.0)
    assert t == pytest.approx(4.0, abs=0.0)
    t = engine.try_evaluate("MROUND(5.363,2)", 0.0)
    assert t == pytest.approx(6.0, abs=0.0)


def test_rand():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("RAND()", 0.0)
    assert t >= 0
    assert t < 1


def test_randbetween():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("RANDBETWEEN(2,99)", 0.0)
    assert t >= 2
    assert t <= 99


def test_fact():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("fact(6)", 0.0)
    assert t == pytest.approx(720.0, abs=0.0)
    t = engine.try_evaluate("fact(3)", 0.0)
    assert t == pytest.approx(6.0, abs=0.0)


def test_factdouble():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("factdouble(10)", 0.0)
    assert t == pytest.approx(3840.0, abs=0.0)


def test_power():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("POWER(10,2)", 0.0)
    assert t == pytest.approx(100.0, abs=0.0)


def test_exp():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("exp(2)", 0.0)
    assert t == pytest.approx(7.389056099, abs=1e-6)


def test_ln():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("ln(4)", 0.0)
    assert t == pytest.approx(1.386294361, abs=1e-6)


def test_log():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("log(10)", 0.0)
    assert t == pytest.approx(1.0, abs=1e-6)

    t = engine.try_evaluate("log(8,2)", 0.0)
    assert t == pytest.approx(3.0, abs=1e-6)


def test_log10():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("log10(10)", 0.0)
    assert t == pytest.approx(1.0, abs=1e-6)


def test_multinomial():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("MULTINOMIAL(1,2,3)", 0.0)
    assert t == pytest.approx(60.0, abs=1e-6)
    t = engine.try_evaluate("MULTINOMIAL(1,2,3,4)", 0.0)
    assert t == pytest.approx(12600.0, abs=1e-6)
    t = engine.try_evaluate("MULTINOMIAL(1,2,3,4.1)", 0.0)
    assert t == pytest.approx(12600.0, abs=1e-6)


def test_product():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("PRODUCT(1,2,3,4)", 0.0)
    assert t == pytest.approx(24.0, abs=1e-6)


def test_sqrtpi():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("SQRTPI(3)", 0.0)
    assert t == pytest.approx(3.069980124, abs=1e-6)


def test_sumsq():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("SUMSQ(1,2)", 0.0)
    assert t == pytest.approx(5.0, abs=1e-6)


def test_erf():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("ERF(0)", 0.0)
    assert t == pytest.approx(0.0, abs=1e-6)

    t = engine.try_evaluate("ERF(1)", 0.0)
    assert t == pytest.approx(0.842701, abs=1e-6)

    t = engine.try_evaluate("ERF(-1)", 0.0)
    assert t == pytest.approx(-0.842701, abs=1e-6)

    t = engine.try_evaluate("ERF(0.5)", 0.0)
    assert t == pytest.approx(0.520499878, abs=1e-6)

    t = engine.try_evaluate("ERF(4)", 0.0)
    assert t == pytest.approx(0.999999985, abs=1e-6)

    t = engine.try_evaluate("ERF(5)", 0.0)
    assert t == pytest.approx(1.0, abs=1e-6)

    t = engine.try_evaluate("ERF(7)", 0.0)
    assert t == pytest.approx(1.0, abs=1e-6)


def test_erfc():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("ERFC(0)", 0.0)
    assert t == pytest.approx(1.0, abs=1e-6)

    t = engine.try_evaluate("ERFC(1)", 0.0)
    assert t == pytest.approx(0.157299, abs=1e-6)

    t = engine.try_evaluate("ERFC(-3)", 0.0)
    assert t == pytest.approx(1.99997791, abs=1e-6)

    t = engine.try_evaluate("ERFC(1.5)", 0.0)
    assert t == pytest.approx(0.033894854, abs=1e-6)


def test_delta():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("DELTA(5, 5)", 0.0)
    assert t == pytest.approx(1.0, abs=0.0)

    t = engine.try_evaluate("DELTA(5, 4)", 0.0)
    assert t == pytest.approx(0.0, abs=0.0)

    t = engine.try_evaluate("DELTA(5)", 0.0)
    assert t == pytest.approx(0.0, abs=0.0)

    t = engine.try_evaluate("DELTA(0)", 0.0)
    assert t == pytest.approx(1.0, abs=0.0)


def test_gestep():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("GESTEP(5, 4)", 0.0)
    assert t == pytest.approx(1.0, abs=0.0)

    t = engine.try_evaluate("GESTEP(5, 5)", 0.0)
    assert t == pytest.approx(1.0, abs=0.0)

    t = engine.try_evaluate("GESTEP(5, 6)", 0.0)
    assert t == pytest.approx(0.0, abs=0.0)

    t = engine.try_evaluate("GESTEP(5)", 0.0)
    assert t == pytest.approx(1.0, abs=0.0)

    t = engine.try_evaluate("GESTEP(-1)", 0.0)
    assert t == pytest.approx(0.0, abs=0.0)


def test_percentage():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("50%", 0.0)
    assert t == pytest.approx(0.5, abs=0.0)

    t = engine.try_evaluate("100%", 0.0)
    assert t == pytest.approx(1.0, abs=0.0)

    t = engine.try_evaluate("1%", 0.0)
    assert t == pytest.approx(0.01, abs=0.0)

    t = engine.try_evaluate("0%", 0.0)
    assert t == pytest.approx(0.0, abs=0.0)

    t = engine.try_evaluate("200%", 0.0)
    assert t == pytest.approx(2.0, abs=0.0)

    t = engine.try_evaluate("25.5%", 0.0)
    assert t == pytest.approx(0.255, abs=0.0)


def test_division_by_zero():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("1/0", 0.0)
    assert t == pytest.approx(0.0, abs=0.0)

    t = engine.try_evaluate("0/0", 0.0)
    assert t == pytest.approx(0.0, abs=0.0)


def test_overflow():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("99999999999999999999*99999999999999999999", 0.0)
    assert t == pytest.approx(0.0, abs=0.0)


def test_null_operation():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("null+1", 0.0)
    assert t == pytest.approx(0.0, abs=0.0)

    t = engine.try_evaluate("null*100", 0.0)
    assert t == pytest.approx(0.0, abs=0.0)

    t = engine.try_evaluate("null-null", 0.0)
    assert t == pytest.approx(0.0, abs=0.0)


def test_square_root_negative():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("sqrt(-1)", 0.0)
    assert t == pytest.approx(0.0, abs=0.0)


def test_log_negative():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("log(-1)", 0.0)
    assert t == pytest.approx(0.0, abs=0.0)

    t = engine.try_evaluate("ln(0)", 0.0)
    assert t == pytest.approx(0.0, abs=0.0)


def test_odd_excel_compatible():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("ODD(1)", 0.0)
    assert t == pytest.approx(1.0, abs=0.0)

    t = engine.try_evaluate("ODD(2)", 0.0)
    assert t == pytest.approx(3.0, abs=0.0)

    t = engine.try_evaluate("ODD(3)", 0.0)
    assert t == pytest.approx(3.0, abs=0.0)

    t = engine.try_evaluate("ODD(-1)", 0.0)
    assert t == pytest.approx(-1.0, abs=0.0)

    t = engine.try_evaluate("ODD(-2)", 0.0)
    assert t == pytest.approx(-3.0, abs=0.0)

    t = engine.try_evaluate("ODD(-3)", 0.0)
    assert t == pytest.approx(-3.0, abs=0.0)

    t = engine.try_evaluate("ODD(-4)", 0.0)
    assert t == pytest.approx(-5.0, abs=0.0)

    t = engine.try_evaluate("ODD(1.5)", 0.0)
    assert t == pytest.approx(3.0, abs=0.0)

    t = engine.try_evaluate("ODD(-1.5)", 0.0)
    assert t == pytest.approx(-3.0, abs=0.0)


def test_even_excel_compatible():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("EVEN(2)", 0.0)
    assert t == pytest.approx(2.0, abs=0.0)

    t = engine.try_evaluate("EVEN(1)", 0.0)
    assert t == pytest.approx(2.0, abs=0.0)

    t = engine.try_evaluate("EVEN(-1)", 0.0)
    assert t == pytest.approx(-2.0, abs=0.0)

    t = engine.try_evaluate("EVEN(-2)", 0.0)
    assert t == pytest.approx(-2.0, abs=0.0)

    t = engine.try_evaluate("EVEN(-3)", 0.0)
    assert t == pytest.approx(-4.0, abs=0.0)

    t = engine.try_evaluate("EVEN(1.5)", 0.0)
    assert t == pytest.approx(2.0, abs=0.0)

    t = engine.try_evaluate("EVEN(-1.5)", 0.0)
    assert t == pytest.approx(-2.0, abs=0.0)

    t = engine.try_evaluate("EVEN(0)", 0.0)
    assert t == pytest.approx(0.0, abs=0.0)


def test_ceiling_excel_compatible():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("CEILING(2.5, 1)", 0.0)
    assert t == pytest.approx(3.0, abs=0.0)

    t = engine.try_evaluate("CEILING(-2.5, -1)", 0.0)
    assert t == pytest.approx(-3.0, abs=0.0)

    t = engine.try_evaluate("CEILING(-2.5, 1)", 0.0)
    assert t == pytest.approx(-2.0, abs=0.0)

    t = engine.try_evaluate("CEILING(1.5, 0.1)", 0.0)
    assert t == pytest.approx(1.5, abs=0.0)

    t = engine.try_evaluate("CEILING(0.234, 0.01)", 0.0)
    assert t == pytest.approx(0.24, abs=0.0)


def test_floor_excel_compatible():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("FLOOR(3.7, 1)", 0.0)
    assert t == pytest.approx(3.0, abs=0.0)

    t = engine.try_evaluate("FLOOR(-3.7, -1)", 0.0)
    assert t == pytest.approx(-3.0, abs=0.0)

    t = engine.try_evaluate("FLOOR(-3.7, 1)", 0.0)
    assert t == pytest.approx(-4.0, abs=0.0)

    t = engine.try_evaluate("FLOOR(3.7, 2)", 0.0)
    assert t == pytest.approx(2.0, abs=0.0)

    t = engine.try_evaluate("FLOOR(0, 1)", 0.0)
    assert t == pytest.approx(0.0, abs=0.0)


def test_mround_excel_compatible():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("MROUND(10, 3)", 0.0)
    assert t == pytest.approx(9.0, abs=0.0)

    t = engine.try_evaluate("MROUND(-10, -3)", 0.0)
    assert t == pytest.approx(-9.0, abs=0.0)

    t = engine.try_evaluate("MROUND(1.3, 0.2)", 0.0)
    assert t == pytest.approx(1.4, abs=0.0)

    t = engine.try_evaluate("MROUND(-1.3, -0.2)", 0.0)
    assert t == pytest.approx(-1.4, abs=0.0)


def test_rounddown_excel_compatible():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("ROUNDDOWN(3.7, 0)", 0.0)
    assert t == pytest.approx(3.0, abs=0.0)

    t = engine.try_evaluate("ROUNDDOWN(-3.7, 0)", 0.0)
    assert t == pytest.approx(-3.0, abs=0.0)

    t = engine.try_evaluate("ROUNDDOWN(3.777, 2)", 0.0)
    assert t == pytest.approx(3.77, abs=0.0)

    t = engine.try_evaluate("ROUNDDOWN(-3.777, 2)", 0.0)
    assert t == pytest.approx(-3.77, abs=0.0)

    t = engine.try_evaluate("ROUNDDOWN(76.9, -1)", 0.0)
    assert t == pytest.approx(70.0, abs=0.0)

    t = engine.try_evaluate("ROUNDDOWN(-76.9, -1)", 0.0)
    assert t == pytest.approx(-70.0, abs=0.0)


def test_factdouble_limit():
    engine = AlgorithmEngine()
    # 29 正常(修复前误报溢出)
    t = engine.try_evaluate("FACTDOUBLE(29)", 0.0)
    assert engine.LastError is None

    # 45 为最大允许值: 45!!≈2.537e28 仍在 decimal 范围
    t = engine.try_evaluate("FACTDOUBLE(45)", 0.0)
    assert engine.LastError is None

    # 46!! 超出 decimal 范围, 报参数错误
    t = engine.try_evaluate("FACTDOUBLE(46)", 0.0)
    assert engine.LastError is not None

    engine = AlgorithmEngine()
    # 负数报错
    t = engine.try_evaluate("FACTDOUBLE(-1)", 0.0)
    assert engine.LastError is not None
