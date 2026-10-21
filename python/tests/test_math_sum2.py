"""Port of ``MathSum2Test.java``."""
from __future__ import annotations

import pytest

from toolgood_algorithm import AlgorithmEngine


def test_normsdist():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("NORMSDIST(1)", 0.0)
    assert t == pytest.approx(0.841344746, abs=1e-6)


def test_normdist():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("NORMDIST(3,8,4,1)", 0.0)
    assert t == pytest.approx(0.105649774, abs=1e-6)
    t = engine.try_evaluate("NORMDIST(3,8,4,0)", 0.0)
    assert t == pytest.approx(0.045662271, abs=1e-6)


def test_norminv():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("NORMINV(0.8,8,3)", 0.0)
    assert t == pytest.approx(10.5248637, abs=1e-6)


def test_normsinv():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("NORMSINV(0.3)", 0.0)
    assert t == pytest.approx(-0.524400513, abs=1e-6)


def test_betadist():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("BETADIST(0.5,11,22)", 0.0)
    assert t == pytest.approx(0.97494877, abs=1e-6)


def test_betainv():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("BETAINV(0.5,23,45)", 0.0)
    assert t == pytest.approx(0.336640759, abs=1e-6)


def test_betainv_param_count():
    engine = AlgorithmEngine()
    # 参数个数不足时,构造函数抛出异常并被引擎捕获,返回默认值而非空引用
    t = engine.try_evaluate("BETAINV(0.5,23)", 0.0)
    assert engine.LastError is not None


def test_binomdist():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("BINOMDIST(12,45,0.5,0)", 0.0)
    assert t == pytest.approx(0.000817409, abs=1e-6)
    t = engine.try_evaluate("BINOMDIST(12,45,0.5,1)", 0.0)
    assert t == pytest.approx(0.00122945, abs=1e-6)


def test_expondist():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("EXPONDIST(3,1,0)", 0.0)
    assert t == pytest.approx(0.049787068, abs=1e-6)
    t = engine.try_evaluate("EXPONDIST(3,1,1)", 0.0)
    assert t == pytest.approx(0.950212932, abs=1e-6)


def test_fdist():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("FDIST(0.4,2,3)", 0.0)
    assert t == pytest.approx(0.701465776, abs=1e-6)


def test_finv():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("FINV(0.7,2,3)", 0.0)
    assert t == pytest.approx(0.402651432, abs=1e-6)


def test_gammadist():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("GAMMADIST(0.5,3,4,0)", 0.0)
    assert t == pytest.approx(0.001723627, abs=1e-6)
    t = engine.try_evaluate("GAMMADIST(0.5,3,4,1)", 0.0)
    assert t == pytest.approx(0.000296478, abs=1e-6)


def test_gammainv():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("GAMMAINV(0.2,3,4)", 0.0)
    assert t == pytest.approx(6.140176811, abs=1e-6)


def test_gammaln():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("GAMMALN(4)", 0.0)
    assert t == pytest.approx(1.791759469, abs=1e-6)


def test_hypgeomdist():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("HYPGEOMDIST(23,45,45,100)", 0.0)
    assert t == pytest.approx(0.08715016, abs=1e-6)


def test_loginv():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("LOGINV(0.1,45,33)", 0.0)
    assert t == pytest.approx(15.01122624, abs=1e-6)


def test_lognormdist():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("LOGNORMDIST(15,23,45)", 0.0)
    assert t == pytest.approx(0.326019201, abs=1e-6)


def test_negbinomdist():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("NEGBINOMDIST(23,45,0.7)", 0.0)
    assert t == pytest.approx(0.053463314, abs=1e-6)


def test_poisson():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("POISSON(23,23,0)", 0.0)
    assert t == pytest.approx(0.082884384, abs=1e-6)
    t = engine.try_evaluate("POISSON(23,23,1)", 0.0)
    assert t == pytest.approx(0.555149936, abs=1e-6)


def test_tdist():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("TDIST(1.2,24,1)", 0.0)
    assert t == pytest.approx(0.120925677, abs=1e-6)
    t = engine.try_evaluate("TDIST(1.2,24,2)", 0.0)
    assert t == pytest.approx(0.241851353, abs=1e-6)


def test_tinv():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("TINV(0.12,23)", 0.0)
    assert t == pytest.approx(1.614756561, abs=1e-6)


def test_weibull():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("WEIBULL(1,2,3,1)", 0.0)
    assert t == pytest.approx(0.105160683, abs=1e-6)
    t = engine.try_evaluate("WEIBULL(1,2,3,0)", 0.0)
    assert t == pytest.approx(0.198853182, abs=1e-6)
    t = engine.try_evaluate("WEIBULL(-1,2,3,0)", 0.0)
    t = engine.try_evaluate("WEIBULL(-1,-2,3,0)", 0.0)
    t = engine.try_evaluate("WEIBULL(-1,-2,-3,0)", 0.0)
    t = engine.try_evaluate("WEIBULL(-1,-2,-3,-1)", 0.0)


def test_weibull_tiny_x_cdf():
    engine = AlgorithmEngine()
    # 极小 x 时 CDF 应趋近于 0 且非负(修复前 ExponentialMinusOne 符号错误导致负值)
    t = engine.try_evaluate("WEIBULL(1E-28,1,1,1)", 0.0)
    assert engine.LastError is None
    assert t >= 0.0


def test_fisher():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("FISHER(0.68)", 0.0)
    assert t == pytest.approx(0.8291140383, abs=1e-6)


def test_fisherinv():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("FISHERINV(0.6)", 0.0)
    assert t == pytest.approx(0.537049567, abs=1e-6)


def test_besseli():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("BESSELI(1.5, 1)", 0.0)
    assert t == pytest.approx(0.981666, abs=1e-6)

    t = engine.try_evaluate("BESSELI(1.5, 0)", 0.0)
    assert t == pytest.approx(1.646723, abs=1e-6)


def test_besselj():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("BESSELJ(1.5, 1)", 0.0)
    assert t == pytest.approx(0.557937, abs=1e-6)

    t = engine.try_evaluate("BESSELJ(1.5, 0)", 0.0)
    assert t == pytest.approx(0.511828, abs=1e-6)


def test_besselk():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("BESSELK(1.5, 1)", 0.0)
    assert t == pytest.approx(0.277, abs=0.01)

    t = engine.try_evaluate("BESSELK(1.5, 0)", 0.0)
    assert t == pytest.approx(0.214, abs=0.01)


def test_bessely():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("BESSELY(1.5, 1)", 0.0)
    assert t == pytest.approx(-0.412, abs=0.01)

    t = engine.try_evaluate("BESSELY(2.5, 0)", 0.0)
    assert t == pytest.approx(0.498, abs=0.01)


def test_besselk_overflow():
    engine = AlgorithmEngine()
    # x 过小时 K_n(x) 溢出为 Infinity,应返回错误而非抛异常
    t = engine.try_evaluate("BESSELK(1E-300,2)", 0.0)
    assert engine.LastError is not None


def test_bessely_overflow():
    engine = AlgorithmEngine()
    # x 过小时 Y_n(x) 溢出为 Infinity,应返回错误而非抛异常
    t = engine.try_evaluate("BESSELY(1E-300,2)", 0.0)
    assert engine.LastError is not None


def test_besselj_overflow():
    engine = AlgorithmEngine()
    # Miller 递推中间值溢出为 Infinity/NaN,应返回错误而非抛异常
    t = engine.try_evaluate("BESSELJ(1E-10,100)", 0.0)
    assert engine.LastError is not None
