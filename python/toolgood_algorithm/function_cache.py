"""FunctionCache (C#: FunctionCache.cs)."""
from __future__ import annotations

from toolgood_algorithm.algorithm_engine_helper import AlgorithmEngineHelper, CalculateTree, ConditionTree, DiyNameInfo
from toolgood_algorithm.enums import CalculateTreeType, CombineCalculateType, ConditionTreeType
from toolgood_algorithm.ifunction_cache import IFunctionCache
from toolgood_algorithm.internals.functions.function_base import FunctionBase


class FunctionCache(IFunctionCache):
    """Caches compiled expressions, conditions and parameter name lookups."""

    def __init__(self):
        self.calculate_cache: dict[str, FunctionBase] = {}
        self.condition_cache: dict[str, FunctionBase] = {}
        self.diy_name_cache: dict[str, DiyNameInfo] = {}

    # ------------------------------------------------------------------ names
    def get_diy_names_with_cache(self, exp: str) -> DiyNameInfo:
        result = self.diy_name_cache.get(exp)
        if result is not None:
            return result
        result = AlgorithmEngineHelper.get_diy_names(exp)
        self.diy_name_cache[exp] = result
        return result

    # -------------------------------------------------------------- calculate
    def parse_with_cache(self, fun_exp: str) -> FunctionBase:
        result = self.calculate_cache.get(fun_exp)
        if result is not None:
            return result
        tree = AlgorithmEngineHelper.parse_calculate(fun_exp)
        function = self._create_calculate(tree, fun_exp)
        self.calculate_cache[fun_exp] = function
        return function

    def _create_calculate(self, tree: CalculateTree, exp: str) -> FunctionBase:
        if tree.type == CalculateTreeType.String:
            key = exp[tree.start:tree.end + 1]
            cached = self.calculate_cache.get(key)
            if cached is not None:
                return cached
            function = AlgorithmEngineHelper.parse_formula(key)
            self.calculate_cache[key] = function
            return function
        if tree.type == CalculateTreeType.Error:
            raise ValueError(tree.error_message)

        left = self._create_calculate(tree.nodes[0], exp)
        right = self._create_calculate(tree.nodes[1], exp)
        combine = CombineCalculateType.int_to_enum(int(tree.type))
        return AlgorithmEngineHelper.combine_calculate(left, combine, right)

    # -------------------------------------------------------------- condition
    def parse_condition_with_cache(self, fun_exp: str) -> FunctionBase:
        result = self.condition_cache.get(fun_exp)
        if result is not None:
            return result
        tree = AlgorithmEngineHelper.parse_condition(fun_exp)
        function = self._create_condition(tree, fun_exp)
        self.condition_cache[fun_exp] = function
        return function

    def _create_condition(self, tree: ConditionTree, exp: str) -> FunctionBase:
        if tree.type == ConditionTreeType.String:
            return self.parse_with_cache(exp[tree.start:tree.end + 1])
        if tree.type == ConditionTreeType.Error:
            raise ValueError(tree.error_message)
        left = self._create_condition(tree.nodes[0], exp)
        right = self._create_condition(tree.nodes[1], exp)
        if tree.type == ConditionTreeType.And:
            return AlgorithmEngineHelper.condition_and(left, right)
        if tree.type == ConditionTreeType.Or:
            return AlgorithmEngineHelper.condition_or(left, right)
        raise ValueError(tree.error_message)

    # CamelCase API
    def GetDiyNamesWithCache(self, exp):
        return self.get_diy_names_with_cache(exp)

    def ParseWithCache(self, fun_exp):
        return self.parse_with_cache(fun_exp)

    def ParseConditionWithCache(self, fun_exp):
        return self.parse_condition_with_cache(fun_exp)
