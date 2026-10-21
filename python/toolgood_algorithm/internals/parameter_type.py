"""``ParameterType`` - describes one parameter discovered by the engine.

Used by the ``GetParameterTypes`` API so that callers can find out which
parameters an expression expects (and with which operator / value).
"""
from __future__ import annotations

from toolgood_algorithm.enums import OperandType


class ParameterType:
    __slots__ = ("name", "type", "operator", "value")

    def __init__(self, name: str = None, type_: OperandType = OperandType.NONE, operator: str = None, value: str = None):
        self.name = name
        self.type = type_
        self.operator = operator
        self.value = value

    def __repr__(self) -> str:  # pragma: no cover - debugging helper
        return f"ParameterType({self.name!r}, {self.type!r}, {self.operator!r}, {self.value!r})"

    # CamelCase API
    @property
    def Name(self):  # noqa: N802
        return self.name

    @Name.setter
    def Name(self, v):  # noqa: N802
        self.name = v

    @property
    def Type(self):  # noqa: N802
        return self.type

    @Type.setter
    def Type(self, v):  # noqa: N802
        self.type = v

    @property
    def Operator(self):  # noqa: N802
        return self.operator

    @Operator.setter
    def Operator(self, v):  # noqa: N802
        self.operator = v

    @property
    def Value(self):  # noqa: N802
        return self.value

    @Value.setter
    def Value(self, v):  # noqa: N802
        self.value = v
