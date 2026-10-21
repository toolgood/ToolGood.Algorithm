"""``_SumFunctionN`` - variadic shell used by the SUM family (Python shared base)."""
from __future__ import annotations

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_n import Function_N


class _SumFunctionN(Function_N):
    """``Function_N`` shell that validates arity and evaluates every argument."""

    def __init__(self, funcs, name: str, minimum=1, maximum=None):
        _require_args(funcs, name, minimum, maximum)
        super().__init__(funcs)

    def _collect(self, engine, tp):
        args = []
        error = self.try_evaluate_all(engine, tp, args)
        if error is not None:
            return error, None
        return None, args

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER


__all__ = ["_SumFunctionN"]
