"""Tests for ``AlgorithmEngineHelper`` (diy names, trees, unit conversion)."""
from __future__ import annotations

from decimal import Decimal

import pytest

from toolgood_algorithm import (
    AlgorithmEngine,
    AlgorithmEngineHelper,
    CalculateTreeType,
    ConditionTreeType,
    OperandType,
)


def test_get_diy_names():
    info = AlgorithmEngineHelper.get_diy_names("dd")
    assert [p.name for p in info.parameters] == ["dd"]

    info = AlgorithmEngineHelper.get_diy_names("ddd(d1,22)")
    assert [f.name for f in info.functions] == ["ddd"]
    assert [p.name for p in info.parameters] == ["d1"]

    info = AlgorithmEngineHelper.get_diy_names("长")
    assert [p.name for p in info.parameters] == ["长"]

    info = AlgorithmEngineHelper.get_diy_names("ddd+2")
    assert [p.name for p in info.parameters] == ["ddd"]

    info = AlgorithmEngineHelper.get_diy_names("{'A': 1, 'BC': 1.0}[瓦楞]")
    assert [p.name for p in info.parameters] == ["瓦楞"]


def test_is_parameter():
    assert AlgorithmEngineHelper.is_parameter("false") is False
    assert AlgorithmEngineHelper.is_parameter("f11") is True
    assert AlgorithmEngineHelper.is_parameter("f11+1") is False
    assert AlgorithmEngineHelper.is_parameter("f11++") is False
    assert AlgorithmEngineHelper.is_parameter("ABS(1)") is False


def test_check_formula():
    assert AlgorithmEngineHelper.check_formula("1+1") is True
    assert AlgorithmEngineHelper.check_formula("1+") is False
    assert AlgorithmEngineHelper.check_formula("") is False


def test_parse_condition_tree():
    tree = AlgorithmEngineHelper.parse_condition("a>1 && b<2")
    assert tree.type == ConditionTreeType.And
    assert len(tree.nodes) == 2
    assert tree.nodes[0].type == ConditionTreeType.String
    assert tree.get_text() == "a>1 && b<2"

    tree = AlgorithmEngineHelper.parse_condition("(a>1 && b<2) || c=3")
    assert tree.type == ConditionTreeType.Or
    assert tree.nodes[0].type == ConditionTreeType.And
    assert tree.nodes[0].has_bracket is True
    assert tree.nodes[1].get_text() == "c=3"

    tree = AlgorithmEngineHelper.parse_condition("a>1")
    assert tree.type == ConditionTreeType.String

    tree = AlgorithmEngineHelper.parse_condition("")
    assert tree.type == ConditionTreeType.Error
    assert tree.error_message


def test_parse_calculate_tree():
    tree = AlgorithmEngineHelper.parse_calculate("1+2*3")
    assert tree.type == CalculateTreeType.Add
    assert tree.nodes[1].type == CalculateTreeType.Mul
    assert tree.get_text() == "1+2*3"

    tree = AlgorithmEngineHelper.parse_calculate("(1+2)*3")
    assert tree.type == CalculateTreeType.Mul
    assert tree.nodes[0].has_bracket is True

    tree = AlgorithmEngineHelper.parse_calculate("1>0")
    assert tree.type == CalculateTreeType.OpGt

    tree = AlgorithmEngineHelper.parse_calculate("'a'&'b'")
    assert tree.type == CalculateTreeType.Connect


def test_combine_calculate():
    helper = AlgorithmEngineHelper
    left = helper.parse_formula("1")
    right = helper.parse_formula("2")
    from toolgood_algorithm import CombineCalculateType

    function = helper.combine_calculate(left, CombineCalculateType.Add, right)
    assert AlgorithmEngine().evaluate(function).number_value() == Decimal(3)


def test_unit_conversion():
    convert = AlgorithmEngineHelper.unit_conversion
    assert convert(Decimal(1), "米", "千米", "测试") == Decimal("0.001")
    assert convert(Decimal(1), "米", "分米", "测试") == Decimal(10)
    assert convert(Decimal(1), "米", "厘米", "测试") == Decimal(100)
    assert convert(Decimal(1), "米", "mm", "测试") == Decimal(1000)
    assert convert(Decimal(1), "kg", "g", "测试") == Decimal(1000)
    assert convert(Decimal(1000), "平方米", "亩", "测试") == Decimal("1.5")
    with pytest.raises(ValueError):
        convert(Decimal(1), "米", "千克", "测试")
    # unknown unit with empty name still raises
    with pytest.raises(ValueError):
        convert(Decimal(1), "xx", "yy")
    # same unit short circuits
    assert convert(Decimal(5), "米", "米") == Decimal(5)


def test_condition_helpers():
    engine = AlgorithmEngine()
    left = AlgorithmEngineHelper.parse_formula("1=1")
    right = AlgorithmEngineHelper.parse_formula("1=2")
    assert engine.evaluate(AlgorithmEngineHelper.condition_and(left, right)).boolean_value() is False
    assert engine.evaluate(AlgorithmEngineHelper.condition_or(left, right)).boolean_value() is True
    error = AlgorithmEngineHelper.error(AlgorithmEngineHelper.parse_formula("'boom'"))
    result = engine.evaluate(error)
    assert result.is_error()
    assert result.error_msg() == "boom"
