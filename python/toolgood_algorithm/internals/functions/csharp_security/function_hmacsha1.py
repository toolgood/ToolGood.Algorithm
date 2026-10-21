"""Function_HMACSHA1 (C#: Internals/Functions/CsharpSecurity/Function_HMACSHA1.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import hmac_digest


@register("HMACSHA1")
class Function_HMACSHA1(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "HmacSHA1", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "HmacSHA1"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        return Operand.create(hmac_digest("sha1", args1.text_value(), args2.text_value()))

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)
