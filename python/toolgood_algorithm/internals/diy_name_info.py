"""``DiyNameInfo`` / ``DiyNameKeyInfo`` (C#: Internals/DiyNameInfo.cs)."""
from __future__ import annotations

from typing import List


class DiyNameKeyInfo:
    """A user supplied parameter or function name (``DiyNameKeyInfo.cs``)."""

    __slots__ = ("name", "start", "end")

    def __init__(self, name: str = None, start: int = 0, end: int = 0):
        self.name = name
        self.start = start
        self.end = end

    def __str__(self) -> str:
        return str(self.name)

    def __repr__(self) -> str:  # pragma: no cover
        return f"DiyNameKeyInfo({self.name!r}, {self.start}, {self.end})"

    @property
    def Name(self):  # noqa: N802
        return self.name

    @property
    def Start(self):  # noqa: N802
        return self.start

    @property
    def End(self):  # noqa: N802
        return self.end


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
