"""``DiyNameVisitor`` - collects parameters / function names from the AST."""
from __future__ import annotations

from typing import List

from toolgood_algorithm.internals.diy_name_info import DiyNameInfo, DiyNameKeyInfo


def collect_diy_names(node, parameters: List[DiyNameKeyInfo], functions: List[DiyNameKeyInfo]) -> None:
    kind = node.kind
    if kind == "index" and node.key_is_parameter:
        key = node.children[1]
        parameters.append(DiyNameKeyInfo(key.text, key.start, key.end))
        collect_diy_names(node.children[0], parameters, functions)
        return
    if kind == "call" and node.keyword == "PARAMETER":
        functions.append(DiyNameKeyInfo(node.text, node.start, node.start + len(node.text) - 1))
    elif kind == "param":
        parameters.append(DiyNameKeyInfo(node.text, node.start, node.end))
    for child in node.children:
        collect_diy_names(child, parameters, functions)
