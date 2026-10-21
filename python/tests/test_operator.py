"""Port of ``OperatorTest.java``."""
from __future__ import annotations

import pytest

from toolgood_algorithm import AlgorithmEngine


def test_arithmetic():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("1+(3*2+2)/2", 0)
    assert t == 5

    t = engine.try_evaluate("(8-3)*(3+2)", 0)
    assert t == 25

    t = engine.try_evaluate("(8-3)*(3+2) % 7", 0)
    assert t == 4

    c = engine.try_evaluate("2+3", 0)
    assert c == 5
    c = engine.try_evaluate("(2)+3", 0)
    assert c == 5
    c = engine.try_evaluate("2+3*2+10/2*4", 0)
    assert c == 28

    c = engine.try_evaluate("2.1e3 + 10", 0)
    assert c == 2110

    c = engine.try_evaluate("2.1e+03 + 10", 0)
    assert c == 2110

    c = engine.try_evaluate("2.1e+3 + 10", 0)
    assert c == 2110

    d = engine.try_evaluate("2.1e-3 + 10", 0.0)
    assert d == pytest.approx(10.0021, abs=1e-10)


def test_connect():
    engine = AlgorithmEngine()
    s = engine.try_evaluate("'aa'&'bb'", "")
    assert s == "aabb"

    s = engine.try_evaluate("'3'+2", "")
    assert s == "5"


def test_conditional():
    engine = AlgorithmEngine()
    t1 = engine.try_evaluate("-7 < -2 ?1 : 2", 0)
    assert t1 == 1
    t1 = engine.try_evaluate("-7 < -2 ?1 ： 2", 0)
    assert t1 == 1

    t1 = engine.try_evaluate("-7 < -2 ?1 ： (7>1?3:2)", 0)
    assert t1 == 1

    t1 = engine.try_evaluate("-7 < -2 ?1 ：2", 0)
    assert t1 == 1
    t1 = engine.try_evaluate("-7 < -2 ？ 1 : 2", 0)
    assert t1 == 1
    t1 = engine.try_evaluate("-7 < -2 ？1 : 2", 0)
    assert t1 == 1

    t1 = engine.try_evaluate("-7 < -2 ？1 ： 2", 0)
    assert t1 == 1

    t1 = engine.try_evaluate("(!(-7 < -2))？1：2", 0)
    assert t1 == 2
    t1 = engine.try_evaluate("1>2？1：2", 0)
    assert t1 == 2

    t1 = engine.try_evaluate("1！=2？1：2", 0)
    assert t1 == 1


def test_percentage():
    engine = AlgorithmEngine()
    t = engine.try_evaluate("100%", 0.0)
    assert t == pytest.approx(1.0, abs=1e-10)

    t = engine.try_evaluate("50%", 0.0)
    assert t == pytest.approx(0.5, abs=1e-10)

    t = engine.try_evaluate("200%", 0.0)
    assert t == pytest.approx(2.0, abs=1e-10)

    t = engine.try_evaluate("100*50%", 0.0)
    assert t == pytest.approx(50.0, abs=1e-10)

    t = engine.try_evaluate("100+50%", 0.0)
    assert t == pytest.approx(100.5, abs=1e-10)


def test_null_operation():
    engine = AlgorithmEngine()
    tbb2 = engine.try_evaluate("'111'*null", 0)
    assert tbb2 == 0


def test_overflow():
    engine = AlgorithmEngine()
    engine.try_evaluate("79228162514264337593543950335 + 1", 0.0)
    assert engine.LastError is not None

    engine = AlgorithmEngine()
    engine.try_evaluate("-79228162514264337593543950335 - 1", 0.0)
    assert engine.LastError is not None

    engine = AlgorithmEngine()
    engine.try_evaluate("79228162514264337593543950335 * 2", 0.0)
    assert engine.LastError is not None

    engine = AlgorithmEngine()
    engine.try_evaluate("79228162514264337593543950335 / 0.1", 0.0)
    assert engine.LastError is not None
