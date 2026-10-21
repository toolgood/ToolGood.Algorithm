"""Ported from ``conditionTrees/CalculateTreeTest.java``."""
from __future__ import annotations

from toolgood_algorithm import AlgorithmEngineHelper, CalculateTreeType


def test_test1():
    txt = "A1+1"
    t1 = AlgorithmEngineHelper.parse_calculate(txt)
    assert t1.type == CalculateTreeType.Add
    assert txt[t1.start:t1.end + 1] == "A1+1"
    assert t1.get_text() == "A1+1"

    txt = "A1-(1+1)"
    t1 = AlgorithmEngineHelper.parse_calculate(txt)
    assert t1.type == CalculateTreeType.Sub
    assert t1.nodes[1].get_text() == "1+1"

    txt = "A1*(1+1)"
    t1 = AlgorithmEngineHelper.parse_calculate(txt)
    assert t1.type == CalculateTreeType.Mul

    txt = "A1/(1+1)"
    t1 = AlgorithmEngineHelper.parse_calculate(txt)
    assert t1.type == CalculateTreeType.Div

    txt = "A1%(1+1)"
    t1 = AlgorithmEngineHelper.parse_calculate(txt)
    assert t1.type == CalculateTreeType.Mod

    txt = "A1&(1+1)"
    t1 = AlgorithmEngineHelper.parse_calculate(txt)
    assert t1.type == CalculateTreeType.Connect

    txt = "A1(1+1)"
    t1 = AlgorithmEngineHelper.parse_calculate(txt)
    assert t1.type == CalculateTreeType.String

    txt = "A1(1+1)-"
    t1 = AlgorithmEngineHelper.parse_calculate(txt)
    assert t1.type == CalculateTreeType.Error

    txt = "-1+(1+1)"
    t1 = AlgorithmEngineHelper.parse_calculate(txt)
    assert t1.type == CalculateTreeType.Add

    txt = "-1"
    t1 = AlgorithmEngineHelper.parse_calculate(txt)
    assert t1.type == CalculateTreeType.String


def test_test_error():
    txt = ""
    tree = AlgorithmEngineHelper.parse_calculate(txt)
    assert tree.type == CalculateTreeType.Error
    assert tree.error_message == "exp is null"


def test_test_complex_expressions():
    txt = "A1+B1*C1"
    tree = AlgorithmEngineHelper.parse_calculate(txt)
    assert tree.type == CalculateTreeType.Add

    txt = "A1*(B1+C1)/D1"
    tree = AlgorithmEngineHelper.parse_calculate(txt)
    assert tree.type == CalculateTreeType.Div

    txt = "A1+B1-C1*D1/E1"
    tree = AlgorithmEngineHelper.parse_calculate(txt)
    assert tree.type == CalculateTreeType.Sub


def test_has_bracket():
    # 无括号时，二元节点的 HasBracket 均为 false
    tree = AlgorithmEngineHelper.parse_calculate("a + b * c")
    assert tree.type == CalculateTreeType.Add
    assert tree.nodes[1].has_bracket is False  # Mul 节点

    # 括号包裹叶子后，后续兄弟节点不应被误标记为 HasBracket（Bug 修复验证）
    tree = AlgorithmEngineHelper.parse_calculate("(a) + b * c")
    assert tree.type == CalculateTreeType.Add
    assert tree.nodes[0].has_bracket is True   # (a) 叶子
    assert tree.nodes[1].has_bracket is False  # Mul 节点，无括号

    # 括号包裹二元节点时，该节点 HasBracket = true
    tree = AlgorithmEngineHelper.parse_calculate("a + (b * c)")
    assert tree.type == CalculateTreeType.Add
    assert tree.nodes[1].has_bracket is True  # (b * c) Mul 节点

    tree = AlgorithmEngineHelper.parse_calculate("(a + b) * c")
    assert tree.type == CalculateTreeType.Mul
    assert tree.nodes[0].has_bracket is True  # (a + b) Add 节点

    # 与条件树同样修复了 || && 的泄漏
    tree = AlgorithmEngineHelper.parse_calculate("(a) || b && c")
    assert tree.type == CalculateTreeType.Or
    assert tree.nodes[0].has_bracket is True   # (a) 叶子
    assert tree.nodes[1].has_bracket is False  # And 节点，无括号
