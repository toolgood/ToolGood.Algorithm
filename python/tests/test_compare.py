"""Tests ported from ``java/.../AlgorithmEngine/CompareTest.java``.

One Java ``@Test`` maps to one pytest function (snake_case).
"""
from __future__ import annotations

from toolgood_algorithm import AlgorithmEngine


def test_base():
    engine = AlgorithmEngine()
    b = engine.try_evaluate("1=1", False)
    assert b is True

    b = engine.try_evaluate("1=2", True)
    assert b is False

    b = engine.try_evaluate("1<>2", False)
    assert b is True

    b = engine.try_evaluate("1!=2", False)
    assert b is True

    b = engine.try_evaluate("1>2", True)
    assert b is False

    b = engine.try_evaluate("1<2", False)
    assert b is True

    b = engine.try_evaluate("1<=2", False)
    assert b is True

    b = engine.try_evaluate("1>=2", True)
    assert b is False

    b = engine.try_evaluate("'1'='1'", False)
    assert b is True
    b = engine.try_evaluate("'e'='e'", False)
    assert b is True
    b = engine.try_evaluate("'1'='2'", True)
    assert b is False
    b = engine.try_evaluate("'我'='你'", True)
    assert b is False
    b = engine.try_evaluate("'1'!='2'", False)
    assert b is True


def test_strict_equality():
    engine = AlgorithmEngine()
    b = engine.try_evaluate("1===1", False)
    assert b is True

    b = engine.try_evaluate("1===2", True)
    assert b is False

    b = engine.try_evaluate("'1'==='1'", False)
    assert b is True

    b = engine.try_evaluate("'1'==='2'", True)
    assert b is False

    b = engine.try_evaluate("1!==2", False)
    assert b is True

    b = engine.try_evaluate("1!==1", True)
    assert b is False

    b = engine.try_evaluate("'1'!=='2'", False)
    assert b is True

    b = engine.try_evaluate("'1'!=='1'", True)
    assert b is False


def test_null_compare():
    engine = AlgorithmEngine()

    bb2 = engine.try_evaluate("1>null", True)
    assert bb2 is False

    bb2 = engine.try_evaluate("1>=null", True)
    assert bb2 is False

    bb2 = engine.try_evaluate("1<=null", True)
    assert bb2 is False

    bb2 = engine.try_evaluate("1<null", True)
    assert bb2 is False

    bb2 = engine.try_evaluate("1==null", True)
    assert bb2 is False

    bb2 = engine.try_evaluate("1!=null", False)
    assert bb2 is True

    # 两侧同为 null 属于异常数据，返回错误，TryEvaluate 返回默认值
    bb2 = engine.try_evaluate("null=null", False)
    assert bb2 is False
    assert engine.LastError is not None

    bb2 = engine.try_evaluate("null!=null", True)
    assert bb2 is True
    assert engine.LastError is not None

    bb2 = engine.try_evaluate("'111'=null", True)
    assert bb2 is False

    bb2 = engine.try_evaluate("'111'!=null", False)
    assert bb2 is True


def test_negative_compare():
    engine = AlgorithmEngine()
    value = engine.try_evaluate("1 > (-2)", False)
    assert value is True

    value = engine.try_evaluate("(-1) > (-2）", False)
    assert value is True

    value = engine.try_evaluate("-1 > (-2)", False)
    assert value is True

    value = engine.try_evaluate("-1 > -2", False)
    assert value is True

    value3 = engine.try_evaluate("-7 < -2", False)
    assert value3 is True

    value3 = engine.try_evaluate("-7*Yes < -2", False)
    assert value3 is True

    value3 = engine.try_evaluate("-7*No > -2", False)
    assert value3 is True


def test_null_same_type_compare():
    engine = AlgorithmEngine()
    # 两侧同为 null 属于异常数据，所有比较运算符均返回错误，TryEvaluate 返回默认值
    b = engine.try_evaluate("null>null", True)
    assert b is True
    assert engine.LastError is not None

    b = engine.try_evaluate("null>=null", True)
    assert b is True
    assert engine.LastError is not None

    b = engine.try_evaluate("null<=null", True)
    assert b is True
    assert engine.LastError is not None

    b = engine.try_evaluate("null<null", True)
    assert b is True
    assert engine.LastError is not None
