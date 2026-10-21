"""Function_SWITCH (C#: Internals/Functions/Flow/Function_SWITCH.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_n import Function_N
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _equals_operand


@register("SWITCH")
class Function_SWITCH(Function_N):
    def __init__(self, funcs):
        _require_args(funcs, "SWITCH", 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "SWITCH"

    def evaluate(self, engine, tp) -> Operand:
        expr_value = self.funcs[0].evaluate(engine, tp)
        if expr_value.is_error_or_none():
            return expr_value
        i = 1
        while i < len(self.funcs) - 1:
            compare_value = self.funcs[i].evaluate(engine, tp)
            if compare_value.is_error_or_none():
                return compare_value
            if _equals_operand(expr_value, compare_value):
                return self.funcs[i + 1].evaluate(engine, tp)
            i += 2
        if len(self.funcs) % 2 == 0:
            return self.funcs[-1].evaluate(engine, tp)
        return self._function_error()

    def get_result_type(self) -> OperandType:
        i = 1
        while i < len(self.funcs) - 1:
            t = self.funcs[i + 1].get_result_type()
            if t != OperandType.NONE:
                return t
            i += 2
        if len(self.funcs) % 2 == 0:
            t = self.funcs[-1].get_result_type()
            if t != OperandType.NONE:
                return t
        return OperandType.NONE

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        for i, func in enumerate(self.funcs):
            func._get_parameter_types(none_engine, result, OperandType.NONE if i % 2 == 0 else OperandType.NONE)
