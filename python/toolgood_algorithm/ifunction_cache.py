"""IFunctionCache (C#: IFunctionCache.cs)."""
from __future__ import annotations

from typing import Protocol

from toolgood_algorithm.internals.functions.function_base import FunctionBase
from toolgood_algorithm.internals.diy_name_info import DiyNameInfo


class IFunctionCache(Protocol):
    """Cache contract, mirroring ``IFunctionCache``."""

    def get_diy_names_with_cache(self, exp: str) -> DiyNameInfo:  # pragma: no cover - protocol
        ...

    def parse_with_cache(self, fun_exp: str) -> FunctionBase:  # pragma: no cover - protocol
        ...

    def parse_condition_with_cache(self, fun_exp: str) -> FunctionBase:  # pragma: no cover - protocol
        ...
