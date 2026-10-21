"""Port of ``MathTrigonometricTest.java``."""
from __future__ import annotations

import math

import pytest

from toolgood_algorithm import AlgorithmEngine


def test_degrees():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("degrees(pi())", 0.0)
    assert t == pytest.approx(180.0, abs=0.0)


def test_radians():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("RADIANS(180)", 0.0)
    assert t == pytest.approx(math.pi, abs=1e-10)


def test_cos():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("cos(1)", 0.0)
    assert t == pytest.approx(0.540302306, abs=1e-6)


def test_cosh():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("cosh(1)", 0.0)
    assert t == pytest.approx(1.543080635, abs=1e-6)


def test_sin():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("sin(1)", 0.0)
    assert t == pytest.approx(0.841470985, abs=1e-6)


def test_sinh():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("sinh(1)", 0.0)
    assert t == pytest.approx(1.175201194, abs=1e-6)


def test_tan():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("tan(1)", 0.0)
    assert t == pytest.approx(1.557407725, abs=1e-6)


def test_tanh():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("tanh(1)", 0.0)
    assert t == pytest.approx(0.761594156, abs=1e-6)


def test_acos():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("acos(0.5)", 0.0)
    assert t == pytest.approx(1.047197551, abs=1e-6)


def test_acosh():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("acosh(1.5)", 0.0)
    assert t == pytest.approx(0.96242365, abs=1e-6)


def test_asin():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("asin(0.5)", 0.0)
    assert t == pytest.approx(0.523598776, abs=1e-6)


def test_asinh():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("asinh(1.5)", 0.0)
    assert t == pytest.approx(1.194763217, abs=1e-6)


def test_atan():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("atan(1)", 0.0)
    assert t == pytest.approx(0.785398163, abs=1e-6)


def test_atanh():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("atanh(0.5)", 0.0)
    assert t == pytest.approx(0.549306144, abs=1e-6)


def test_atan2():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("atan2(1,2)", 0.0)
    assert t == pytest.approx(1.107148718, abs=1e-6)


def test_cot():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("cot(1)", 0.0)
    assert t == pytest.approx(0.642092616, abs=1e-6)


def test_coth():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("coth(1)", 0.0)
    assert t == pytest.approx(1.313035285, abs=1e-6)


def test_csc():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("csc(1)", 0.0)
    assert t == pytest.approx(1.188395106, abs=1e-6)


def test_csch():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("csch(1)", 0.0)
    assert t == pytest.approx(0.850918128, abs=1e-6)


def test_sec():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("sec(1)", 0.0)
    assert t == pytest.approx(1.850815718, abs=1e-6)


def test_sech():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("sech(1)", 0.0)
    assert t == pytest.approx(0.648054274, abs=1e-6)


def test_acot():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("acot(1)", 0.0)
    assert t == pytest.approx(0.785398163, abs=1e-6)


def test_acoth():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("acoth(2)", 0.0)
    assert t == pytest.approx(0.549306144, abs=1e-6)


def test_hyperbolic_overflow():
    engine = AlgorithmEngine()

    # tanh/coth 大参数趋近 ±1
    assert engine.try_evaluate("tanh(1000)", 0.0) == pytest.approx(1.0, abs=0.0)
    assert engine.try_evaluate("tanh(-1000)", 0.0) == pytest.approx(-1.0, abs=0.0)
    assert engine.try_evaluate("coth(1000)", 0.0) == pytest.approx(1.0, abs=0.0)
    assert engine.try_evaluate("coth(-1000)", 0.0) == pytest.approx(-1.0, abs=0.0)

    # sech/csch 大参数趋近 0
    assert engine.try_evaluate("sech(1000)", 1.0) == pytest.approx(0.0, abs=0.0)
    assert engine.try_evaluate("csch(1000)", 1.0) == pytest.approx(0.0, abs=0.0)

    # sinh/cosh 大参数超出 double 范围,返回错误
    engine.try_evaluate("sinh(1000)", 0.0)
    assert engine.LastError is not None

    engine = AlgorithmEngine()
    engine.try_evaluate("cosh(1000)", 0.0)
    assert engine.LastError is not None
