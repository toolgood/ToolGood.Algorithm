"""Calculation logic info (``CalculationLogicInfo.java``)."""
from __future__ import annotations

from typing import Optional

from toolgood_algorithm.operands.operand import Operand

from .calculation_logic_type import CalculationLogicType


class CalculationLogicInfo:
    """One recorded step of the calculation logic."""

    __slots__ = ("logic_type", "layer", "name", "exp", "exp2", "remark", "value")

    def __init__(
        self,
        logic_type: Optional[CalculationLogicType] = None,
        layer: int = 0,
        name: Optional[str] = None,
        exp: Optional[str] = None,
        exp2: Optional[str] = None,
        remark: Optional[str] = None,
        value: Optional[Operand] = None,
    ):
        self.logic_type = logic_type
        self.layer = layer
        self.name = name
        self.exp = exp
        self.exp2 = exp2
        self.remark = remark
        self.value = value

    def to_info_string(self) -> str:
        """Format this entry the way ``ToInfoString()`` does."""
        remark = "" if not self.remark else " // " + self.remark
        layer_str = " " * (min(max(0, self.layer), 1000) * 3)
        logic_type = self.logic_type

        if logic_type == CalculationLogicType.BlankLine:
            return ""
        if logic_type == CalculationLogicType.Scene:
            return "===== " + str(self.name) + " ====="
        if logic_type == CalculationLogicType.InitValue:
            return str(self.name) + "=" + str(self.exp) + ";"
        if logic_type == CalculationLogicType.Condition:
            if self.value.is_error():
                return (
                    "[条件][错误] " + layer_str + "if " + str(self.exp) + ": // "
                    + str(self.exp2) + " = " + self.value.error_msg() + remark
                )
            if self.value.boolean_value():
                return "[成功] " + layer_str + "if " + str(self.exp) + ": // " + str(self.exp2) + remark
            return "[失败] " + layer_str + "if " + str(self.exp) + ": // " + str(self.exp2) + remark
        if logic_type == CalculationLogicType.SetFormula:
            if self.value.is_error():
                return (
                    "[赋值][错误] " + layer_str + str(self.name) + " = " + str(self.exp) + " = "
                    + str(self.exp2) + " // " + self.value.error_msg() + remark
                )
            return (
                "[赋值] " + layer_str + str(self.name) + " = " + str(self.exp) + " = "
                + str(self.exp2) + " = " + self.value.to_text(None).text_value() + remark
            )
        if logic_type == CalculationLogicType.SetValue:
            return "[赋值] " + layer_str + str(self.name) + " = " + str(self.exp) + remark
        if logic_type == CalculationLogicType.Error:
            return "[错误] " + layer_str + str(self.exp)
        return "[错误]: " + layer_str + str(self.name) + str(self.exp) + remark

    # ------------------------------------------------------------- camel fields
    @property
    def LogicType(self) -> Optional[CalculationLogicType]:  # noqa: N802
        return self.logic_type

    @LogicType.setter
    def LogicType(self, value: CalculationLogicType) -> None:  # noqa: N802
        self.logic_type = value

    @property
    def Layer(self) -> int:  # noqa: N802
        return self.layer

    @Layer.setter
    def Layer(self, value: int) -> None:  # noqa: N802
        self.layer = value

    @property
    def Name(self) -> Optional[str]:  # noqa: N802
        return self.name

    @Name.setter
    def Name(self, value: str) -> None:  # noqa: N802
        self.name = value

    @property
    def Exp(self) -> Optional[str]:  # noqa: N802
        return self.exp

    @Exp.setter
    def Exp(self, value: str) -> None:  # noqa: N802
        self.exp = value

    @property
    def Exp2(self) -> Optional[str]:  # noqa: N802
        return self.exp2

    @Exp2.setter
    def Exp2(self, value: str) -> None:  # noqa: N802
        self.exp2 = value

    @property
    def Remark(self) -> Optional[str]:  # noqa: N802
        return self.remark

    @Remark.setter
    def Remark(self, value: str) -> None:  # noqa: N802
        self.remark = value

    @property
    def Value(self) -> Optional[Operand]:  # noqa: N802
        return self.value

    @Value.setter
    def Value(self, value: Operand) -> None:  # noqa: N802
        self.value = value

    ToInfoString = to_info_string
