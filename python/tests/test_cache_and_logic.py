"""Tests for ``FunctionCache`` and ``CalculationLogicEngine``."""
from __future__ import annotations

from decimal import Decimal

import pytest

from toolgood_algorithm import AlgorithmEngine
from toolgood_algorithm.calculation_logic import CalculationLogicEngine, CalculationLogicType
from toolgood_algorithm.function_cache import FunctionCache


@pytest.fixture()
def cache():
    return FunctionCache()


def test_parse_with_cache(cache):
    first = cache.parse_with_cache("1+2*3")
    second = cache.parse_with_cache("1+2*3")
    assert first is second
    engine = AlgorithmEngine()
    assert engine.evaluate(first).number_value() == Decimal(7)


def test_condition_cache(cache):
    function = cache.parse_condition_with_cache("1=1 && 2=2")
    engine = AlgorithmEngine()
    assert engine.evaluate(function).boolean_value() is True

    function = cache.parse_condition_with_cache("1=1 || 2=3")
    assert engine.evaluate(function).boolean_value() is True


def test_diy_names_cache(cache):
    first = cache.get_diy_names_with_cache("a+1")
    second = cache.get_diy_names_with_cache("a+1")
    assert first is second
    assert [p.name for p in first.parameters] == ["a"]


def test_calculation_logic_scene_and_values():
    engine = CalculationLogicEngine(FunctionCache(), True)
    engine.set_scene_name("demo")
    engine.init_value("a", 2)
    engine.init_value("b", 3)
    engine.set_formula("c", "a*b")
    engine.set_value("d", "ok")
    assert engine.get_value("c").number_value() == Decimal(6)
    assert engine.get_value("d").text_value() == "ok"


def test_calculation_logic_condition():
    engine = CalculationLogicEngine(FunctionCache(), False)
    engine.init_value("score", 80)
    engine.set_formula("level", "if(score>=60,'PASS','FAIL')")
    assert engine.get_value("level").text_value() == "PASS"
    # SetFormula evaluates immediately and stores the operand (like the C#/Java engines)
    engine.init_value("score", 50)
    assert engine.get_value("level").text_value() == "PASS"
    engine.set_formula("level", "if(score>=60,'PASS','FAIL')")
    assert engine.get_value("level").text_value() == "FAIL"


def test_calculation_logic_info_log():
    engine = CalculationLogicEngine(FunctionCache(), True)
    engine.set_scene_name("s")
    engine.init_value("a", 1)
    engine.set_formula("b", "a+1")
    text = engine.to_info_string()
    assert "s" in text
    assert "a" in text
