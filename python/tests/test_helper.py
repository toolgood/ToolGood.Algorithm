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


# ---------------------------------------------------------------------------
# Ported from ``algorithmEngineHelper/AlgorithmEngineHelperTest.java``
# ---------------------------------------------------------------------------
def test_get_diy_names_test():
    p = AlgorithmEngineHelper.get_diy_names("dd")
    assert str(p.parameters[0]) == "dd"

    p3 = AlgorithmEngineHelper.get_diy_names("dd")
    assert str(p3.parameters[0]) == "dd"

    p5 = AlgorithmEngineHelper.get_diy_names("ddd(d1,22)")
    assert p5.functions[0].name == "ddd"
    assert str(p5.parameters[0]) == "d1"

    p6 = AlgorithmEngineHelper.get_diy_names("长")
    assert str(p6.parameters[0]) == "长"

    p7 = AlgorithmEngineHelper.get_diy_names("ddd+2")
    assert str(p7.parameters[0]) == "ddd"

    p8 = AlgorithmEngineHelper.get_diy_names(
        "{\"A\": 0.6,\"B\": 0.4,\"C\": 0.6,\"E\": 0.33,\"F\": 0.29,\"Z\": 0.15\n"
        ",\"EB\": 0.7,\"EE\": 0.65,\"EA\": 0.85,\"AB\": 1.0,\"BC\": 1.0,\"AA\":1.0\n"
        ",\"EBC\": 1.15,\"BAB\": 1.25,\"BCB\": 1.25,\"BBC\": 1.25,\"CBB\": 1.25,\"EBA\": 1.2,\"AAA\": 1.4}[瓦楞]"
    )
    assert str(p8.parameters[0]) == "瓦楞"


def test_is_parameter_test():
    b = AlgorithmEngineHelper.is_parameter("false")
    assert b is False

    b = AlgorithmEngineHelper.is_parameter("f11")
    assert b is True

    b = AlgorithmEngineHelper.is_parameter("f11+1")
    assert b is False

    b = AlgorithmEngineHelper.is_parameter("f11++")
    assert b is False


def test_unit_conversion_test():
    b = AlgorithmEngineHelper.unit_conversion(Decimal("1"), "米", "千米", "测试")
    assert b == Decimal("0.001")
    b = AlgorithmEngineHelper.unit_conversion(Decimal("1"), "米", "分米", "测试")
    assert b == Decimal("10")
    b = AlgorithmEngineHelper.unit_conversion(Decimal("1"), "米", "厘米", "测试")
    assert b == Decimal("100")
    b = AlgorithmEngineHelper.unit_conversion(Decimal("1"), "米", "mm", "测试")
    assert b == Decimal("1000")

    b = AlgorithmEngineHelper.unit_conversion(Decimal("1"), "m2", "dm2", "测试")
    assert b == Decimal("100")
    b = AlgorithmEngineHelper.unit_conversion(Decimal("1"), "m2", "cm2", "测试")
    assert b == Decimal("10000")
    b = AlgorithmEngineHelper.unit_conversion(Decimal("1"), "m2", "mm2", "测试")
    assert b == Decimal("1000000")

    b = AlgorithmEngineHelper.unit_conversion(Decimal("1"), "m3", "dm3", "测试")
    assert b == Decimal("1000")
    b = AlgorithmEngineHelper.unit_conversion(Decimal("1"), "m3", "cm3", "测试")
    assert b == Decimal("1000000")
    b = AlgorithmEngineHelper.unit_conversion(Decimal("1"), "m3", "mm3", "测试")
    assert b == Decimal("1000000000")

    b = AlgorithmEngineHelper.unit_conversion(Decimal("1"), "t", "kg", "测试")
    assert b == Decimal("1000")
    b = AlgorithmEngineHelper.unit_conversion(Decimal("1"), "t", "g", "测试")
    assert b == Decimal("1000000")


def test_check_formula_test():
    b = AlgorithmEngineHelper.check_formula("1+1")
    assert b is True

    b = AlgorithmEngineHelper.check_formula("1+")
    assert b is False

    b = AlgorithmEngineHelper.check_formula("@123+1")
    assert b is False


def test_parse_formula_large_number_test():
    # Java 使用 BigDecimal（任意精度），超大数字不会像 C# decimal 那样溢出
    assert AlgorithmEngineHelper.parse_formula("12345678901234567890123456789012345") is not None
    assert AlgorithmEngineHelper.parse_formula("12345678901234567890123456789012345M") is not None
    assert AlgorithmEngineHelper.parse_formula("9999999999999999999999999999999") is not None

    # 正常数字（含单位、科学计数法、负数）不受影响
    assert AlgorithmEngineHelper.parse_formula("12CM") is not None
    assert AlgorithmEngineHelper.parse_formula("10M2") is not None
    assert AlgorithmEngineHelper.parse_formula("-5") is not None
    assert AlgorithmEngineHelper.parse_formula("-5M") is not None
    assert AlgorithmEngineHelper.parse_formula("3E5") is not None
    assert AlgorithmEngineHelper.parse_formula("1234567890123456789012345678") is not None
