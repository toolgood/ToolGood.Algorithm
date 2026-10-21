"""Tests ported from ``java/.../AlgorithmEngine/StrictModeTest.java``.

One Java ``@Test`` maps to one pytest function (snake_case).
"""
from __future__ import annotations

from toolgood_algorithm import AlgorithmEngine

# region 二元 && 运算符


def test_and_strict_should_error_when_right_is_error():
    engine = AlgorithmEngine()
    engine.UseStrictMode = True
    t = engine.try_evaluate("(1>2) && ERROR('test')", True)
    # 严格模式下，即使左边为 false，右边错误也会传播
    assert t is True
    assert engine.LastError is not None


def test_and_strict_all_valid():
    engine = AlgorithmEngine()
    engine.UseStrictMode = True
    t = engine.try_evaluate("2>1 && 3>2", False)
    assert t is True
    assert engine.LastError is None


def test_and_not_strict_short_circuit():
    engine = AlgorithmEngine()
    engine.UseStrictMode = False
    # 非严格模式：1>2 为 false，短路不执行右边
    t = engine.try_evaluate("(1>2) && ERROR('test')", True)
    assert t is False
    assert engine.LastError is None


def test_and_not_strict_error_on_left():
    engine = AlgorithmEngine()
    engine.UseStrictMode = False
    # 非严格模式：左边错误无法短路，应报错
    t = engine.try_evaluate("ERROR('left') && (2>1)", True)
    assert t is True
    assert engine.LastError is not None


def test_and_not_strict_continue_when_left_true():
    engine = AlgorithmEngine()
    engine.UseStrictMode = False
    # 非严格模式：左边为 true，继续执行右边
    t = engine.try_evaluate("(2>1) && ERROR('right')", True)
    assert t is True
    assert engine.LastError is not None


def test_and_not_strict_all_valid():
    engine = AlgorithmEngine()
    engine.UseStrictMode = False
    t = engine.try_evaluate("2>1 && 3>2", False)
    assert t is True
    assert engine.LastError is None


# endregion

# region 二元 || 运算符


def test_or_strict_should_error_when_right_is_error():
    engine = AlgorithmEngine()
    engine.UseStrictMode = True
    # 严格模式下，即使左边为 true，右边错误也会传播
    t = engine.try_evaluate("(2>1) || ERROR('test')", False)
    assert t is False
    assert engine.LastError is not None


def test_or_strict_all_valid():
    engine = AlgorithmEngine()
    engine.UseStrictMode = True
    t = engine.try_evaluate("1>2 || 3>2", False)
    assert t is True
    assert engine.LastError is None


def test_or_not_strict_short_circuit():
    engine = AlgorithmEngine()
    engine.UseStrictMode = False
    # 非严格模式：2>1 为 true，短路不执行右边
    t = engine.try_evaluate("(2>1) || ERROR('test')", False)
    assert t is True
    assert engine.LastError is None


def test_or_not_strict_error_on_left():
    engine = AlgorithmEngine()
    engine.UseStrictMode = False
    # 非严格模式：左边错误无法短路，应报错
    t = engine.try_evaluate("ERROR('left') || (2>1)", False)
    assert t is False
    assert engine.LastError is not None


def test_or_not_strict_continue_when_left_false():
    engine = AlgorithmEngine()
    engine.UseStrictMode = False
    # 非严格模式：左边为 false，继续执行右边
    t = engine.try_evaluate("(1>2) || ERROR('right')", True)
    assert t is True
    assert engine.LastError is not None


def test_or_not_strict_all_valid():
    engine = AlgorithmEngine()
    engine.UseStrictMode = False
    t = engine.try_evaluate("1>2 || 3>2", False)
    assert t is True
    assert engine.LastError is None


# endregion

# region n 元 AND() 函数


def test_and_n_strict_should_error_when_any_param_is_error():
    engine = AlgorithmEngine()
    engine.UseStrictMode = True
    t = engine.try_evaluate("AND(true(), true(), ERROR('test'))", True)
    assert t is True
    assert engine.LastError is not None


def test_and_n_strict_all_valid():
    engine = AlgorithmEngine()
    engine.UseStrictMode = True
    t = engine.try_evaluate("AND(true(), 1=1, 3>2)", False)
    assert t is True
    assert engine.LastError is None


def test_and_n_not_strict_short_circuit():
    engine = AlgorithmEngine()
    engine.UseStrictMode = False
    # 非严格模式：遇到 false 就短路
    t = engine.try_evaluate("AND(true(), false(), ERROR('test'))", True)
    assert t is False
    assert engine.LastError is None


def test_and_n_not_strict_error_before_short_circuit():
    engine = AlgorithmEngine()
    engine.UseStrictMode = False
    # 非严格模式：错误在短路条件之前，应报错
    t = engine.try_evaluate("AND(ERROR('first'), false(), true())", True)
    assert t is True
    assert engine.LastError is not None


def test_and_n_not_strict_all_valid():
    engine = AlgorithmEngine()
    engine.UseStrictMode = False
    t = engine.try_evaluate("AND(true(), 1=1, 3>2)", False)
    assert t is True
    assert engine.LastError is None


# endregion

# region n 元 OR() 函数


def test_or_n_strict_should_error_when_any_param_is_error():
    engine = AlgorithmEngine()
    engine.UseStrictMode = True
    t = engine.try_evaluate("OR(false(), false(), ERROR('test'))", True)
    assert t is True
    assert engine.LastError is not None


def test_or_n_strict_all_valid():
    engine = AlgorithmEngine()
    engine.UseStrictMode = True
    t = engine.try_evaluate("OR(false(), 1=2, 3>2)", False)
    assert t is True
    assert engine.LastError is None


def test_or_n_not_strict_short_circuit():
    engine = AlgorithmEngine()
    engine.UseStrictMode = False
    # 非严格模式：遇到 true 就短路
    t = engine.try_evaluate("OR(false(), true(), ERROR('test'))", False)
    assert t is True
    assert engine.LastError is None


def test_or_n_not_strict_error_before_short_circuit():
    engine = AlgorithmEngine()
    engine.UseStrictMode = False
    # 非严格模式：错误在短路条件之前，应报错
    t = engine.try_evaluate("OR(ERROR('first'), true(), false())", True)
    assert t is True
    assert engine.LastError is not None


def test_or_n_not_strict_all_valid():
    engine = AlgorithmEngine()
    engine.UseStrictMode = False
    t = engine.try_evaluate("OR(false(), 1=2, 3>2)", False)
    assert t is True
    assert engine.LastError is None


# endregion

# region 链式短路


def test_chained_and_not_strict_short_circuit():
    engine = AlgorithmEngine()
    engine.UseStrictMode = False
    # (1>2) && ... 短路，跳过 ERROR
    t = engine.try_evaluate("(1>2) && ERROR('a') && ERROR('b')", True)
    assert t is False
    assert engine.LastError is None


def test_chained_or_not_strict_short_circuit():
    engine = AlgorithmEngine()
    engine.UseStrictMode = False
    # (2>1) || ... 短路，跳过 ERROR
    t = engine.try_evaluate("(2>1) || ERROR('a') || ERROR('b')", False)
    assert t is True
    assert engine.LastError is None


def test_chained_and_strict_all_evaluated():
    engine = AlgorithmEngine()
    engine.UseStrictMode = True
    # 严格模式：所有条件都要求值，遇到错误报错
    t = engine.try_evaluate("(1>2) && (3>2) && ERROR('test')", True)
    assert t is True
    assert engine.LastError is not None


def test_chained_or_strict_all_evaluated():
    engine = AlgorithmEngine()
    engine.UseStrictMode = True
    # 严格模式：所有条件都要求值，遇到错误报错
    t = engine.try_evaluate("(2>1) || (3>2) || ERROR('test')", False)
    assert t is False
    assert engine.LastError is not None


# endregion

# region 默认行为


def test_default_mode_is_strict():
    engine = AlgorithmEngine()
    # 默认 UseStrictMode = true
    t = engine.try_evaluate("(2>1) || ERROR('test')", False)
    assert t is False
    assert engine.LastError is not None


# endregion
