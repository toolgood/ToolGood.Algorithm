"""Function_BASE64URLTOTEXT (C#: Internals/Functions/CsharpWeb/Function_BASE64URLTOTEXT.cs)."""
from __future__ import annotations
import base64
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_1 import Function_1
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("BASE64URLTOTEXT")
class Function_BASE64URLTOTEXT(Function_1):
    def __init__(self, funcs):
        _require_args(funcs, "Base64UrlToText", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "Base64UrlToText"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        try:
            base64_url = args1.text_value().replace("-", "+").replace("_", "/")
            padding = 4 - (len(base64_url) % 4)
            if padding < 4:
                base64_url += "=" * padding
            decoded = base64.b64decode(base64_url, validate=True)
            return Operand.create(decoded.decode("utf-8", "replace"))
        except Exception:  # noqa: BLE001 - mirrors the catch-all in the original
            return self._parameter_error(1)

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
