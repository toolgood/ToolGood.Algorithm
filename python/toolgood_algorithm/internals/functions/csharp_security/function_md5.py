"""Function_MD5 (C#: Internals/Functions/CsharpSecurity/Function_MD5.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_1 import Function_1
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import digest


@register("MD5")
class Function_MD5(Function_1):
    def __init__(self, funcs):
        _require_args(funcs, "MD5", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "MD5"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        return Operand.create(digest("md5", args1.text_value()))

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
