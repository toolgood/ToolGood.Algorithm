"""Ported from ``conditionTrees/ConditionTreeTest.java``."""
from __future__ import annotations

from toolgood_algorithm import AlgorithmEngineHelper, ConditionTreeType


def _slice(txt: str, tree) -> str:
    """Java: txt.substring(tree.Start, tree.End + 1)."""
    return txt[tree.start:tree.end + 1]


def test_test1():
    txt = "AA.IsText() = bb"
    t1 = AlgorithmEngineHelper.parse_condition(txt)
    assert t1.type == ConditionTreeType.String
    assert txt[t1.start:t1.end + 1] == "AA.IsText() = bb"
    assert t1.get_text() == "AA.IsText() = bb"

    txt = "[bbb]=bb"
    t1 = AlgorithmEngineHelper.parse_condition(txt)
    assert t1.type == ConditionTreeType.String
    assert txt[t1.start:t1.end + 1] == txt


def test_test2():
    txt = "AA.IsText()=bb && dd=ss"
    tree = AlgorithmEngineHelper.parse_condition(txt)

    assert tree.type == ConditionTreeType.And
    t1 = tree.nodes[0]
    t2 = tree.nodes[1]
    assert _slice(txt, t1) == "AA.IsText()=bb"
    assert _slice(txt, t2) == "dd=ss"


def test_test3():
    txt = "AA.IsText()=bb || dd=ss"
    tree = AlgorithmEngineHelper.parse_condition(txt)

    assert tree.type == ConditionTreeType.Or
    t1 = tree.nodes[0]
    t2 = tree.nodes[1]
    assert _slice(txt, t1) == "AA.IsText()=bb"
    assert _slice(txt, t2) == "dd=ss"


def test_test4():
    txt = "AA.IsText()=bb || (dd=ss && tt=22)"
    tree = AlgorithmEngineHelper.parse_condition(txt)

    assert tree.type == ConditionTreeType.Or
    t1 = tree.nodes[0]
    t2 = tree.nodes[1]
    t3 = t2.nodes[0]
    t4 = t2.nodes[1]
    assert _slice(txt, t1) == "AA.IsText()=bb"
    assert _slice(txt, t3) == "dd=ss"
    assert _slice(txt, t4) == "tt=22"


def test_test5():
    txt = "AA.IsText()=bb || AND(dd=ss , tt=22)"
    tree = AlgorithmEngineHelper.parse_condition(txt)

    assert tree.type == ConditionTreeType.Or
    t1 = tree.nodes[0]
    t2 = tree.nodes[1]
    assert _slice(txt, t1) == "AA.IsText()=bb"
    assert _slice(txt, t2) == "AND(dd=ss , tt=22)"


def test_test6():
    txt = "AA.IsText()==bb && (dd=ss || tt=22)"
    tree = AlgorithmEngineHelper.parse_condition(txt)

    assert tree.type == ConditionTreeType.And
    t1 = tree.nodes[0]
    t2 = tree.nodes[1]
    assert t2.type == ConditionTreeType.Or
    t3 = t2.nodes[0]
    t4 = t2.nodes[1]

    assert _slice(txt, t1) == "AA.IsText()==bb"
    assert _slice(txt, t3) == "dd=ss"
    assert _slice(txt, t4) == "tt=22"


def test_test7():
    txt = "AA.IsText()==bb ? 1:2"
    t1 = AlgorithmEngineHelper.parse_condition(txt)
    assert t1.type == ConditionTreeType.String
    assert _slice(txt, t1) == txt


def test_test8():
    txt = "AA.IsText()==bb && (dd=ss || [tt]=22)"
    tree = AlgorithmEngineHelper.parse_condition(txt)

    assert tree.type == ConditionTreeType.And
    t1 = tree.nodes[0]
    t2 = tree.nodes[1]
    assert t2.type == ConditionTreeType.Or
    t3 = t2.nodes[0]
    t4 = t2.nodes[1]

    assert _slice(txt, t1) == "AA.IsText()==bb"
    assert _slice(txt, t3) == "dd=ss"
    assert _slice(txt, t4) == "[tt]=22"


def test_test9():
    txt = "AA.IsText()==bb && (dd=ss || [tt]==22)"
    tree = AlgorithmEngineHelper.parse_condition(txt)

    assert tree.type == ConditionTreeType.And
    t1 = tree.nodes[0]
    t2 = tree.nodes[1]
    assert t2.type == ConditionTreeType.Or
    t3 = t2.nodes[0]
    t4 = t2.nodes[1]

    assert _slice(txt, t1) == "AA.IsText()==bb"
    assert _slice(txt, t3) == "dd=ss"
    assert _slice(txt, t4) == "[tt]==22"


def test_test10():
    txt = "AA && (dd=ss || [tt]==22)"
    tree = AlgorithmEngineHelper.parse_condition(txt)

    assert tree.type == ConditionTreeType.And
    t1 = tree.nodes[0]
    t2 = tree.nodes[1]
    assert t2.type == ConditionTreeType.Or
    t3 = t2.nodes[0]
    t4 = t2.nodes[1]

    assert _slice(txt, t1) == "AA"
    assert _slice(txt, t3) == "dd=ss"
    assert _slice(txt, t4) == "[tt]==22"


def test_test11():
    txt = "1 && (dd=ss || [tt]==22)"
    tree = AlgorithmEngineHelper.parse_condition(txt)

    assert tree.type == ConditionTreeType.And
    t1 = tree.nodes[0]
    t2 = tree.nodes[1]
    assert t2.type == ConditionTreeType.Or
    t3 = t2.nodes[0]
    t4 = t2.nodes[1]

    assert _slice(txt, t1) == "1"
    assert _slice(txt, t3) == "dd=ss"
    assert _slice(txt, t4) == "[tt]==22"


def test_test_error():
    txt = ""
    tree = AlgorithmEngineHelper.parse_condition(txt)
    assert tree.type == ConditionTreeType.Error
    assert tree.error_message == "condition is null"


def test_has_bracket():
    # 无括号时，二元节点的 HasBracket 均为 false
    tree = AlgorithmEngineHelper.parse_condition("a || b && c")
    assert tree.type == ConditionTreeType.Or
    assert tree.has_bracket is False
    assert tree.nodes[0].has_bracket is False
    assert tree.nodes[1].has_bracket is False  # And 节点

    # 括号包裹叶子后，后续兄弟节点不应被误标记为 HasBracket（Bug 修复验证）
    tree = AlgorithmEngineHelper.parse_condition("(a) || b && c")
    assert tree.type == ConditionTreeType.Or
    assert tree.nodes[0].has_bracket is True   # (a) 叶子
    assert tree.nodes[1].has_bracket is False  # And 节点，无括号

    # 括号包裹二元节点时，该节点 HasBracket = true
    tree = AlgorithmEngineHelper.parse_condition("(a || b) && c")
    assert tree.type == ConditionTreeType.And
    assert tree.nodes[0].has_bracket is True  # (a || b) Or 节点

    tree = AlgorithmEngineHelper.parse_condition("a && (b || c)")
    assert tree.type == ConditionTreeType.And
    assert tree.nodes[1].has_bracket is True  # (b || c) Or 节点

    # 括号包裹叶子与二元节点混合
    tree = AlgorithmEngineHelper.parse_condition("(a) && (b || c)")
    assert tree.type == ConditionTreeType.And
    assert tree.nodes[0].has_bracket is True  # (a) 叶子
    assert tree.nodes[1].has_bracket is True  # (b || c) Or 节点

    tree = AlgorithmEngineHelper.parse_condition("(a) || (b)")
    assert tree.type == ConditionTreeType.Or
    assert tree.nodes[0].has_bracket is True  # (a) 叶子
    assert tree.nodes[1].has_bracket is True  # (b) 叶子

    # 多重括号
    tree = AlgorithmEngineHelper.parse_condition("((a)) && b")
    assert tree.type == ConditionTreeType.And
    assert tree.nodes[0].has_bracket is True   # ((a)) 叶子
    assert tree.nodes[1].has_bracket is False  # b 叶子
