"""Function_URLENCODE (C#: Internals/Functions/CsharpWeb/Function_URLENCODE.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_1 import Function_1
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _url_encode


@register("URLENCODE")
class Function_URLENCODE(Function_1):
    def __init__(self, funcs):
        _require_args(funcs, "UrlEncode", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "UrlEncode"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        return Operand.create(_url_encode(args1.text_value()))

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
