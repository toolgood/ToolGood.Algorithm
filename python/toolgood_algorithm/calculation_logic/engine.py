"""Calculation logic engine (``CalculationLogicEngine.java``)."""
from __future__ import annotations

import datetime as _dt
from decimal import Decimal
from typing import List, Optional

from toolgood_algorithm.algorithm_engine_ex import AlgorithmEngineEx
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.function_cache import IFunctionCache
from toolgood_algorithm.internals.numeric import plain
from toolgood_algorithm.operands.my_date import MyDate
from toolgood_algorithm.operands.operand import Operand

from .calculation_logic_type import CalculationLogicType
from .info import CalculationLogicInfo


def _operand_and_exp(value, quoted_text: bool):
    """Build the operand plus the literal that is written into the info log."""
    if isinstance(value, Operand):
        return value, str(value)
    if isinstance(value, bool):
        return Operand.create(value), "True" if value else "False"
    if isinstance(value, int):
        return Operand.create(value), str(value)
    if isinstance(value, float):
        return Operand.create(value), str(value)
    if isinstance(value, Decimal):
        return Operand.create(value), plain(value)
    if isinstance(value, str):
        if quoted_text:
            return Operand.create(value), '"' + value.replace('"', '\\"') + '"'
        return Operand.create(value), value
    if isinstance(value, MyDate):
        return Operand.create(value), '"' + value.to_string() + '"'
    if isinstance(value, (_dt.datetime, _dt.date)):
        return Operand.create(value), '"' + MyDate(value).to_string() + '"'
    raise TypeError(f"Unsupported value type {type(value)!r}.")


class CalculationLogicEngine:
    """Runs a stack of conditions / assignments and records every step."""

    def __init__(self, function_cache: IFunctionCache, use_calculation_logic_info: bool = True):
        self._function_cache = function_cache
        self._use_calculation_logic_info = use_calculation_logic_info
        self._engine = AlgorithmEngineEx()
        self._init_value_infos: List[CalculationLogicInfo] = []
        self._calculation_logic_infos: List[CalculationLogicInfo] = []

    # ------------------------------------------------------------------ scene
    def set_scene_name(self, scene: str) -> None:
        """Set the scene name."""
        if self._use_calculation_logic_info:
            self._calculation_logic_infos.append(
                CalculationLogicInfo(CalculationLogicType.Scene, name=scene)
            )

    # -------------------------------------------------------------- init value
    def init_value(self, key: str, value, layer: int = 0, remark: Optional[str] = None) -> None:
        """Define an initial value (string / bool / int / float / Decimal / date)."""
        operand, exp = _operand_and_exp(value, False)
        self._engine.add_parameter(key, operand)
        if self._use_calculation_logic_info:
            self._init_value_infos.append(
                CalculationLogicInfo(
                    CalculationLogicType.InitValue,
                    layer=layer,
                    name=key,
                    exp=exp,
                    remark=remark,
                )
            )

    # ----------------------------------------------------------- check condition
    def check_condition(self, condition: str, layer: int = 0, remark: Optional[str] = None) -> bool:
        """Evaluate ``condition``; raises ``ValueError`` when it is not a boolean."""
        func = self._function_cache.parse_with_cache(condition)
        operand = self._engine.evaluate(func)
        operand = operand.to_boolean("The condition must be a boolean value!")
        if operand.is_error():
            if self._use_calculation_logic_info:
                exp_str = self._exp_analysis(condition)
                self._calculation_logic_infos.append(
                    CalculationLogicInfo(
                        CalculationLogicType.Condition,
                        layer=layer,
                        exp=condition,
                        exp2=exp_str,
                        remark=remark,
                        value=operand,
                    )
                )
            raise ValueError(operand.error_msg())
        if self._use_calculation_logic_info:
            exp_str = self._exp_analysis(condition)
            self._calculation_logic_infos.append(
                CalculationLogicInfo(
                    CalculationLogicType.Condition,
                    layer=layer,
                    exp=condition,
                    exp2=exp_str,
                    remark=remark,
                    value=operand,
                )
            )
        return operand.boolean_value()

    # -------------------------------------------------------------- set formula
    def set_formula(self, key: str, exp: str, layer: int = 0, remark: Optional[str] = None) -> None:
        """Assign ``key`` the result of ``exp``."""
        func = self._function_cache.parse_with_cache(exp)
        operand = self._engine.evaluate(func)
        if operand.is_error():
            if self._use_calculation_logic_info:
                exp_str = self._exp_analysis(exp)
                self._calculation_logic_infos.append(
                    CalculationLogicInfo(
                        CalculationLogicType.SetFormula,
                        layer=layer,
                        name=key,
                        exp=exp,
                        exp2=exp_str,
                        remark=remark,
                        value=operand,
                    )
                )
            raise ValueError(operand.error_msg())
        self._engine.add_parameter(key, operand)

        if self._use_calculation_logic_info:
            exp_str = self._exp_analysis(exp)
            self._calculation_logic_infos.append(
                CalculationLogicInfo(
                    CalculationLogicType.SetFormula,
                    layer=layer,
                    name=key,
                    exp=exp,
                    exp2=exp_str,
                    remark=remark,
                    value=operand,
                )
            )

    # ---------------------------------------------------------------- set value
    def set_value(self, key: str, value, layer: int = 0, remark: Optional[str] = None) -> None:
        """Assign ``key`` a plain value."""
        operand, exp = _operand_and_exp(value, True)
        self._engine.add_parameter(key, operand)
        if self._use_calculation_logic_info:
            self._calculation_logic_infos.append(
                CalculationLogicInfo(
                    CalculationLogicType.SetValue,
                    layer=layer,
                    name=key,
                    exp=exp,
                    remark=remark,
                )
            )

    # ------------------------------------------------------------ exp analysis
    def _exp_analysis(self, exp: str) -> str:
        try:
            diy_name_info = self._function_cache.get_diy_names_with_cache(exp)
            index = 0
            parts: List[str] = []
            for parameter in diy_name_info.parameters:
                if index < parameter.start:
                    parts.append(exp[index:parameter.start])
                parts.append(self._get_operand(self._engine, parameter.name))
                index = parameter.end + 1
            if index < len(exp):
                parts.append(exp[index:])
            return "".join(parts)
        except Exception as exc:  # noqa: BLE001
            raise RuntimeError(str(exc)) from exc

    @staticmethod
    def _get_operand(engine: AlgorithmEngineEx, name: str) -> str:
        operand = engine.get_parameter(name)
        if operand.is_error():
            message = operand.error_msg().replace("\\", "\\\\").replace('"', '\\"')
            return 'error("' + message + '")'
        if operand.type() == OperandType.TEXT:
            return str(operand)
        return operand.to_text(None).text_value()

    # ---------------------------------------------------------------- get value
    def get_value(self, key: str) -> Operand:
        """Return the current value of ``key``."""
        return self._engine.get_parameter(key)

    # --------------------------------------------------------------- blank line
    def blank_line(self) -> None:
        """Append an empty line to the info log."""
        if self._use_calculation_logic_info:
            self._calculation_logic_infos.append(CalculationLogicInfo(CalculationLogicType.BlankLine))

    # ------------------------------------------------------------- info string
    def to_info_string(self) -> str:
        """Render the whole log."""
        if self._use_calculation_logic_info:
            sb: List[str] = []
            if self._init_value_infos:
                sb.append("[初始] ")
                for item in self._init_value_infos:
                    sb.append(item.to_info_string())
                sb.append("\n")
            for item in self._calculation_logic_infos:
                sb.append(item.to_info_string())
                sb.append("\n")
            return "".join(sb)
        return ""

    # --------------------------------------------------------------- camel api
    SetSceneName = set_scene_name
    InitValue = init_value
    CheckCondition = check_condition
    SetFormula = set_formula
    SetValue = set_value
    GetValue = get_value
    BlankLine = blank_line
    ToInfoString = to_info_string
