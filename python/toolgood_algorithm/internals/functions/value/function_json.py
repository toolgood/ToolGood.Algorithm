"""Function_JSON (C#: Internals/Functions/Value/Function_JSON.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_1 import Function_1
from toolgood_algorithm.lit_json.json_mapper import JsonMapper
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("JSON")
class Function_JSON(Function_1):
    def __init__(self, funcs):
        _require_args(funcs, "Json", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "Json"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self.func1.evaluate(engine, tp)
        if args1.is_error_or_none():
            return args1
        if args1.is_json():
            return args1
        if args1.is_array_json():
            args1 = args1.to_text(None)
        if not args1.is_text():
            return self._parameter_error(1)
        text = args1.text_value()
        if (text.startswith("{") and text.endswith("}")) or (text.startswith("[") and text.endswith("]")):
            try:
                return Operand.create(JsonMapper.to_object(text))
            except Exception:  # noqa: BLE001
                return self._parameter_error(1)
        return self._parameter_error(1)

    def get_result_type(self) -> OperandType:
        return OperandType.JSON

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NONE)
