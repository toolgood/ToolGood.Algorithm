"""Split visitors: build ``ConditionTree`` / ``CalculateTree`` from the AST.

Replaces the ANTLR based ``MathSplitVisitor.cs`` / ``MathSplitVisitor2.cs``.
"""
from __future__ import annotations

from toolgood_algorithm.enums import CalculateTreeType, ConditionTreeType
from toolgood_algorithm.internals.calculate_tree import CalculateTree
from toolgood_algorithm.internals.condition_tree import ConditionTree


_CALCULATE_TYPES = {
    "OPMUL": CalculateTreeType.Mul,
    "OPDIV": CalculateTreeType.Div,
    "OPMOD": CalculateTreeType.Mod,
    "OPADD": CalculateTreeType.Add,
    "OPSUB": CalculateTreeType.Sub,
    "OPCAT": CalculateTreeType.Connect,
    "OPAND": CalculateTreeType.And,
    "OPOR": CalculateTreeType.Or,
    "OPGT": CalculateTreeType.OpGt,
    "OPLT": CalculateTreeType.OpLt,
    "OPGE": CalculateTreeType.OpGe,
    "OPLE": CalculateTreeType.OpLe,
    "OPEQ": CalculateTreeType.OpEq,
    "OPNE": CalculateTreeType.OpNe,
}


def build_condition_tree(node, source: str, has_bracket: bool = False) -> ConditionTree:
    from toolgood_algorithm.math.parser import AstNode  # noqa: F401

    if node.kind == "bracket":
        return build_condition_tree(node.children[0], source, True)
    tree = ConditionTree()
    tree.set_source(source)
    tree.start = node.start
    tree.end = node.end
    tree.has_bracket = has_bracket
    if node.kind == "binary" and node.op in ("OPAND", "OPOR"):
        tree.type = ConditionTreeType.And if node.op == "OPAND" else ConditionTreeType.Or
        tree.nodes = [
            build_condition_tree(node.children[0], source),
            build_condition_tree(node.children[1], source),
        ]
    else:
        tree.type = ConditionTreeType.String
        tree.nodes = []
    return tree


def build_calculate_tree(node, source: str, has_bracket: bool = False) -> CalculateTree:
    if node.kind == "bracket":
        return build_calculate_tree(node.children[0], source, True)
    tree = CalculateTree()
    tree.set_source(source)
    tree.start = node.start
    tree.end = node.end
    tree.has_bracket = has_bracket
    if node.kind == "binary":
        tree.type = _CALCULATE_TYPES.get(node.op, CalculateTreeType.String)
        tree.nodes = [
            build_calculate_tree(node.children[0], source),
            build_calculate_tree(node.children[1], source),
        ]
    else:
        tree.type = CalculateTreeType.String
        tree.nodes = []
    return tree
