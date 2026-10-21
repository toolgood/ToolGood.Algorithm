"""Shared base for ``CalculateTree`` / ``ConditionTree`` (port helper)."""
from __future__ import annotations

from typing import List, Optional


class _TreeBase:
    __slots__ = ("nodes", "start", "end", "type", "has_bracket", "error_message", "_source", "_text")

    def __init__(self, type_=None):
        self.nodes: List["_TreeBase"] = []
        self.start = 0
        self.end = 0
        self.type = type_
        self.has_bracket = False
        self.error_message: Optional[str] = None
        self._source: Optional[str] = None
        self._text: Optional[str] = None

    def set_source(self, source: str) -> None:
        self._source = source

    @property
    def text(self) -> Optional[str]:
        if self._text is None and self._source is not None and self.end >= self.start:
            self._text = self._source[self.start:self.end + 1]
        return self._text
