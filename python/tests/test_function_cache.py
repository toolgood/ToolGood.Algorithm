"""Ported from ``FunctionCacheTest.java``."""
from __future__ import annotations

from toolgood_algorithm import FunctionCache


def test_parse_with_cache_formula_expression_returns_function_base():
    cache = FunctionCache()
    result = cache.parse_with_cache("abs(1)")
    assert result is not None


def test_parse_with_cache_same_formula_returns_same_instance():
    cache = FunctionCache()
    result1 = cache.parse_with_cache("abs(1)")
    result2 = cache.parse_with_cache("abs(1)")
    assert result1 is result2


def test_parse_with_cache_different_formulas_returns_different_instances():
    cache = FunctionCache()
    result1 = cache.parse_with_cache("abs(1)")
    result2 = cache.parse_with_cache("abs(2)")
    assert result1 is not result2


def test_parse_with_cache_multiple_calls_same_formula_returns_consistent_instance():
    cache = FunctionCache()
    r1 = cache.parse_with_cache("if(true,1,2)")
    r2 = cache.parse_with_cache("if(true,1,2)")
    r3 = cache.parse_with_cache("if(true,1,2)")
    assert r1 is r2
    assert r2 is r3
    assert r1 is not None


def test_parse_with_cache_diy_expression_returns_function_base():
    cache = FunctionCache()
    result = cache.parse_with_cache("myVar")
    assert result is not None


def test_parse_with_cache_nested_function_returns_function_base():
    cache = FunctionCache()
    result = cache.parse_with_cache("max(1,2,3)")
    assert result is not None


def test_parse_with_cache_formula_correct_name():
    cache = FunctionCache()
    result = cache.parse_with_cache("abs(1)")
    assert result is not None
    assert result.name() is not None


def test_parse_condition_with_cache_simple_condition_returns_function_base():
    cache = FunctionCache()
    result = cache.parse_condition_with_cache("1=1")
    assert result is not None


def test_parse_condition_with_cache_and_condition_returns_function_base():
    cache = FunctionCache()
    result = cache.parse_condition_with_cache("1=1 && 2=2")
    assert result is not None


def test_parse_condition_with_cache_or_condition_returns_function_base():
    cache = FunctionCache()
    result = cache.parse_condition_with_cache("1=1 || 2=2")
    assert result is not None


def test_parse_condition_with_cache_same_condition_returns_same_instance():
    cache = FunctionCache()
    result1 = cache.parse_condition_with_cache("1=1")
    result2 = cache.parse_condition_with_cache("1=1")
    assert result1 is result2


def test_parse_condition_with_cache_different_conditions_returns_different_instances():
    cache = FunctionCache()
    result1 = cache.parse_condition_with_cache("1=1")
    result2 = cache.parse_condition_with_cache("2=2")
    assert result1 is not result2


def test_parse_condition_with_cache_complex_condition_returns_function_base():
    cache = FunctionCache()
    result = cache.parse_condition_with_cache("(1=1 || 2=2) && 3=3")
    assert result is not None


def test_parse_condition_with_cache_string_condition_delegates_to_parse_with_cache():
    cache = FunctionCache()
    result = cache.parse_condition_with_cache("abs(1) > 0")
    assert result is not None


def test_cross_check_parse_with_cache_and_parse_condition_with_cache_independent():
    cache = FunctionCache()
    f1 = cache.parse_with_cache("abs(1)")
    f2 = cache.parse_condition_with_cache("1=1")
    assert f1 is not None
    assert f2 is not None
    assert f1 is not f2
