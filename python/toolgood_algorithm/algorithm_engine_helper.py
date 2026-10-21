"""``AlgorithmEngineHelper`` - parsing / conversion utilities.

Ported from ``toolgood.algorithm.AlgorithmEngineHelper`` (C#/Java).  The tree
builders replace the ANTLR visitors ``MathSplitVisitor`` / ``MathSplitVisitor2``
/ ``DiyNameVisitor``.
"""
from __future__ import annotations

from decimal import Decimal
from typing import List, Optional

from toolgood_algorithm.enums import CalculateTreeType, CombineCalculateType, ConditionTreeType, OperandType
from toolgood_algorithm.internals.functions import compare, operator
from toolgood_algorithm.internals.functions.function_base import FunctionBase
from toolgood_algorithm.internals.functions.flow import Function_IF
from toolgood_algorithm.math.parser import ParseError, parse_ast
from toolgood_algorithm.unit_conversion import unit_conversion


from toolgood_algorithm.internals.calculate_tree import CalculateTree
from toolgood_algorithm.internals.condition_tree import ConditionTree
from toolgood_algorithm.internals.diy_name_info import DiyNameInfo, DiyNameKeyInfo
from toolgood_algorithm.internals.visitors.diy_name_visitor import collect_diy_names
from toolgood_algorithm.internals.visitors.math_split_visitor import build_calculate_tree, build_condition_tree

# ---------------------------------------------------------------------------
class AlgorithmEngineHelper:
    """Static helpers: parse formulas/conditions and inspect expressions."""

    # ---------------------------------------------------------------- parsing
    @staticmethod
    def parse_formula(exp: str) -> FunctionBase:
        if exp is None or not exp.strip():
            raise ValueError("Parameter exp invalid !")
        return parse_function_checked(exp)

    @staticmethod
    def check_formula(exp: str) -> bool:
        if exp is None or not exp.strip():
            return False
        try:
            parse_ast(exp)
            return True
        except ParseError:
            return False
        except ValueError:
            return False

    # ------------------------------------------------------------- diy names
    @staticmethod
    def get_diy_names(exp: str) -> DiyNameInfo:
        if exp is None or not exp.strip():
            raise ValueError("Parameter exp invalid !")
        node = parse_ast(exp)
        parameters: List[DiyNameKeyInfo] = []
        functions: List[DiyNameKeyInfo] = []
        collect_diy_names(node, parameters, functions)
        return DiyNameInfo(parameters, functions)

    @staticmethod
    def is_parameter(parameter: str) -> bool:
        if parameter is None or not parameter.strip():
            return False
        try:
            diy = AlgorithmEngineHelper.get_diy_names(parameter)
            if diy.functions:
                return False
            if len(diy.parameters) == 1:
                return diy.parameters[0].name == parameter
        except Exception:  # noqa: BLE001
            return False
        return False

    # -------------------------------------------------------- unit conversion
    @staticmethod
    def unit_conversion(src: Decimal, old_src_unit: str, old_tar_unit: str, name: str = None) -> Decimal:
        return unit_conversion(src, old_src_unit, old_tar_unit, name)

    # ------------------------------------------------------------- conditions
    @staticmethod
    def parse_condition(condition: str) -> ConditionTree:
        tree = ConditionTree()
        if condition is None or not condition.strip():
            tree.type = ConditionTreeType.Error
            tree.error_message = "condition is null"
            return tree
        try:
            node = parse_ast(condition)
            return build_condition_tree(node, condition)
        except Exception as exc:  # noqa: BLE001
            tree.type = ConditionTreeType.Error
            tree.error_message = str(exc)
            return tree

    @staticmethod
    def condition_and(left: FunctionBase, right: FunctionBase) -> FunctionBase:
        return operator.Function_AND([left, right])

    @staticmethod
    def condition_or(left: FunctionBase, right: FunctionBase) -> FunctionBase:
        return operator.Function_OR([left, right])

    @staticmethod
    def error(txt: FunctionBase) -> FunctionBase:
        from toolgood_algorithm.internals.functions.value import Function_ERROR

        return Function_ERROR([txt])

    # ------------------------------------------------------------ calculation
    @staticmethod
    def parse_calculate(exp: str) -> CalculateTree:
        tree = CalculateTree()
        if exp is None or not exp.strip():
            tree.type = CalculateTreeType.Error
            tree.error_message = "exp is null"
            return tree
        try:
            node = parse_ast(exp)
            return build_calculate_tree(node, exp)
        except Exception as exc:  # noqa: BLE001
            tree.type = CalculateTreeType.Error
            tree.error_message = str(exc)
            return tree

    @staticmethod
    def combine_calculate(left: FunctionBase, type_: CombineCalculateType, right: FunctionBase) -> FunctionBase:
        mapping = {
            CombineCalculateType.Add: operator.Function_Add,
            CombineCalculateType.Sub: operator.Function_Sub,
            CombineCalculateType.Mul: operator.Function_Mul,
            CombineCalculateType.Div: operator.Function_Div,
            CombineCalculateType.Mod: operator.Function_Mod,
            CombineCalculateType.Connect: operator.Function_Connect,
            CombineCalculateType.And: operator.Function_AND,
            CombineCalculateType.Or: operator.Function_OR,
            CombineCalculateType.OpGt: compare.Function_GT,
            CombineCalculateType.OpLt: compare.Function_LT,
            CombineCalculateType.OpGe: compare.Function_GE,
            CombineCalculateType.OpLe: compare.Function_LE,
            CombineCalculateType.OpEq: compare.Function_EQ,
            CombineCalculateType.OpNe: compare.Function_NE,
        }
        cls = mapping.get(type_)
        if cls is None:
            raise ValueError(f"unknown combine type {type_}")
        return cls([left, right])

    # CamelCase API
    ParseFormula = parse_formula
    CheckFormula = check_formula
    GetDiyNames = get_diy_names
    IsParameter = is_parameter
    UnitConversion = unit_conversion
    ParseCondition = parse_condition
    Condition_And = condition_and
    Condition_Or = condition_or
    Error = error
    ParseCalculate = parse_calculate
    CombineCalculate = combine_calculate


def parse_function_checked(exp: str) -> FunctionBase:
    from toolgood_algorithm.math.parser import parse_function

    return parse_function(exp)
