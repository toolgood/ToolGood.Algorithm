"""Tests ported from ``java/.../AlgorithmEngine/FinancialTest.java``.

One Java ``@Test`` maps to one pytest function (snake_case).
``assertEquals(expected, actual, delta)`` -> ``pytest.approx(expected, abs=delta)``。
"""
from __future__ import annotations

import pytest

from toolgood_algorithm import AlgorithmEngine, AlgorithmEngineEx, Operand


def test_pmt():
    # Java: FinancialTest.java:16
    engine = AlgorithmEngine()
    t = engine.try_evaluate("PMT(0.08/12, 10, 10000)", 0.0)
    assert t == pytest.approx(-1037.0321, abs=1e-4)


def test_ppmt():
    # Java: FinancialTest.java:23
    engine = AlgorithmEngine()
    t = engine.try_evaluate("PPMT(0.08/12, 1, 10, 10000)", 0.0)
    assert t == pytest.approx(-970.3654, abs=1e-4)


def test_ipmt():
    # Java: FinancialTest.java:30
    engine = AlgorithmEngine()
    t = engine.try_evaluate("IPMT(0.08/12, 1, 10, 10000)", 0.0)
    assert t == pytest.approx(-66.6667, abs=1e-4)


def test_pv():
    # Java: FinancialTest.java:37
    engine = AlgorithmEngine()
    t = engine.try_evaluate("PV(0.08/12, 10, -1000)", 0.0)
    assert t == pytest.approx(9642.90, abs=1e-2)


def test_fv():
    # Java: FinancialTest.java:44
    engine = AlgorithmEngine()
    t = engine.try_evaluate("FV(0.08/12, 10, -1000)", 0.0)
    assert t == pytest.approx(10305.40, abs=1e-2)


def test_nper():
    # Java: FinancialTest.java:51
    engine = AlgorithmEngine()
    t = engine.try_evaluate("NPER(0.08/12, -1000, 10000)", 0.0)
    assert t == pytest.approx(10.38, abs=1e-2)


def test_rate():
    # Java: FinancialTest.java:58
    engine = AlgorithmEngine()
    t = engine.try_evaluate("RATE(12,-100,400,0,0,0.1)", 0.0)
    assert t == pytest.approx(0.2289, abs=1e-4)


def test_npv():
    # Java: FinancialTest.java:65
    engine = AlgorithmEngine()
    t = engine.try_evaluate("NPV(0.1, -10000, 3000, 4200, 6800)", 0.0)
    assert t == pytest.approx(1188.44, abs=1e-2)


def test_irr():
    # Java: FinancialTest.java:72
    engine = AlgorithmEngine()
    t = engine.try_evaluate("IRR(array(-70000, 12000, 15000, 18000, 21000, 26000))", 0.0)
    assert t == pytest.approx(0.0866, abs=1e-4)


def test_mirr():
    # Java: FinancialTest.java:79
    engine = AlgorithmEngine()
    t = engine.try_evaluate("MIRR(array(-70000, 12000, 15000, 18000, 21000, 26000), 0.1, 0.12)", 0.0)
    assert t == pytest.approx(0.0987, abs=1e-4)


def test_sln():
    # Java: FinancialTest.java:86
    engine = AlgorithmEngine()
    t = engine.try_evaluate("SLN(30000, 7500, 10)", 0.0)
    assert t == pytest.approx(2250.0, abs=1e-10)


def test_syd():
    # Java: FinancialTest.java:93
    engine = AlgorithmEngine()
    t = engine.try_evaluate("SYD(30000, 7500, 10, 1)", 0.0)
    assert t == pytest.approx(4090.91, abs=1e-2)


def test_ddb():
    # Java: FinancialTest.java:100
    engine = AlgorithmEngine()
    t = engine.try_evaluate("DDB(2400, 300, 10, 2)", 0.0)
    assert t == pytest.approx(384.0, abs=1e-2)


def test_db():
    # Java: FinancialTest.java:107
    engine = AlgorithmEngine()
    t = engine.try_evaluate("DB(1000000, 100000, 6, 1)", 0.0)
    assert t == pytest.approx(319000.0, abs=1e-2)


def test_db_month():
    # Java: FinancialTest.java:114
    engine = AlgorithmEngine()
    # Excel: month<12 时折旧跨越 life+1 个期间(第 1 年部分月 + life-1 个整年 + 最后部分月)
    # 修复前 period>life 直接报错, 现在允许 period=life+1
    t = engine.try_evaluate("DB(1000000, 100000, 6, 7, 7)", 0.0)
    assert engine.LastError is None
    assert t > 0

    # 超过总期间数仍报错
    t = engine.try_evaluate("DB(1000000, 100000, 6, 8, 7)", 0.0)
    assert engine.LastError is not None

    engine = AlgorithmEngine()
    # month=12 时总期间数等于 life, 最后一年折旧为整年
    t = engine.try_evaluate("DB(1000000, 100000, 6, 6, 12)", 0.0)
    assert engine.LastError is None
    assert t > 0

    engine = AlgorithmEngine()
    # period=1 首期按部分月折旧
    t = engine.try_evaluate("DB(1000000, 100000, 6, 1, 7)", 0.0)
    assert engine.LastError is None
    assert t > 0


def test_xnpv():
    # Java: FinancialTest.java:140
    engine = AlgorithmEngineEx()
    engine.add_parameter(
        "values",
        [Operand.create(-10000.0), Operand.create(2750.0), Operand.create(4250.0),
         Operand.create(3250.0), Operand.create(2750.0)],
    )
    engine.add_parameter(
        "dates2",
        [Operand.create("2008-1-1"), Operand.create("2008-3-1"), Operand.create("2008-10-30"),
         Operand.create("2009-2-15"), Operand.create("2009-4-1")],
    )
    t = engine.try_evaluate("XNPV(0.09, values, dates2)", 0.0)
    assert t == pytest.approx(2086.65, abs=1e-2)


def test_xirr():
    # Java: FinancialTest.java:151
    engine = AlgorithmEngineEx()
    engine.add_parameter(
        "values",
        [Operand.create(-10000.0), Operand.create(2750.0), Operand.create(4250.0),
         Operand.create(3250.0), Operand.create(2750.0)],
    )
    engine.add_parameter(
        "dates2",
        [Operand.create("2008-1-1"), Operand.create("2008-3-1"), Operand.create("2008-10-30"),
         Operand.create("2009-2-15"), Operand.create("2009-4-1")],
    )
    t = engine.try_evaluate("XIRR(values, dates2)", 0.0)
    assert t == pytest.approx(0.3734, abs=1e-4)
