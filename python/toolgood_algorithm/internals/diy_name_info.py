"""DiyNameInfo (C#: Internals/DiyNameInfo.cs)."""
from __future__ import annotations

from typing import List

from toolgood_algorithm.internals.diy_name_key_info import DiyNameKeyInfo


class DiyNameInfo:
    """Result of :meth:`AlgorithmEngineHelper.get_diy_names`."""

    __slots__ = ("parameters", "functions")

    def __init__(self, parameters: List[DiyNameKeyInfo] = None, functions: List[DiyNameKeyInfo] = None):
        self.parameters = parameters if parameters is not None else []
        self.functions = functions if functions is not None else []

    @property
    def Parameters(self):  # noqa: N802
        return self.parameters

    @property
    def Functions(self):  # noqa: N802
        return self.functions
