"""``ConditionTree`` (C#: Internals/ConditionTree.cs)."""
from __future__ import annotations

from toolgood_algorithm.enums import ConditionTreeType
from toolgood_algorithm.internals.tree_base import _TreeBase


class ConditionTree(_TreeBase):
    """Tree of ``&&`` / ``||`` nodes used by ``FunctionCache``."""

    def __init__(self):
        super().__init__(ConditionTreeType.String)

    # CamelCase API
    @property
    def Nodes(self):  # noqa: N802
        return self.nodes

    @Nodes.setter
    def Nodes(self, v):  # noqa: N802
        self.nodes = v

    @property
    def Start(self):  # noqa: N802
        return self.start

    @Start.setter
    def Start(self, v):  # noqa: N802
        self.start = v

    @property
    def End(self):  # noqa: N802
        return self.end

    @End.setter
    def End(self, v):  # noqa: N802
        self.end = v

    @property
    def Type(self):  # noqa: N802
        return self.type

    @Type.setter
    def Type(self, v):  # noqa: N802
        self.type = v

    @property
    def HasBracket(self):  # noqa: N802
        return self.has_bracket

    @HasBracket.setter
    def HasBracket(self, v):  # noqa: N802
        self.has_bracket = v

    @property
    def ErrorMessage(self):  # noqa: N802
        return self.error_message

    @ErrorMessage.setter
    def ErrorMessage(self, v):  # noqa: N802
        self.error_message = v

    def get_text(self) -> Optional[str]:
        return self.text

    def setSource(self, source: str) -> None:  # noqa: N802
        self.set_source(source)
