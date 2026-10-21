"""Function_URLDECODE (C#: Internals/Functions/CsharpWeb/Function_URLDECODE.cs)."""
from __future__ import annotations
from urllib.parse import unquote_plus
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_1 import Function_1
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("URLDECODE")
class Function_URLDECODE(Function_1):
    def __init__(self, funcs):
        _require_args(funcs, "UrlDecode", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "UrlDecode"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        text = args1.text_value()
        try:
            return Operand.create(unquote_plus(text, encoding="utf-8", errors="replace"))
        except Exception:  # noqa: BLE001 - invalid escapes: keep the raw text
            return Operand.create(text.replace("+", " "))

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
