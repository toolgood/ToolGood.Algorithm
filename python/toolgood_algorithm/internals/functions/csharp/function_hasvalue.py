"""Function_HASVALUE (C#: Internals/Functions/Csharp/Function_HASVALUE.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _json_item_matches


@register("HASVALUE")
class Function_HASVALUE(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "HasValue", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "HasValue"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self.func1.evaluate(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        if args1.is_array_json():
            return Operand.create(args1.contains_value(args2))
        if args1.is_json():
            json = args1.json_value()
            if json.is_array():
                for i in range(json.count()):
                    if _json_item_matches(json.get_child(i), args2.text_value()):
                        return Operand.TRUE
            else:
                for value in json.inst_object.values():
                    if _json_item_matches(value, args2.text_value()):
                        return Operand.TRUE
            return Operand.FALSE
        if args1.is_array():
            for item in args1.array_value():
                text = item.to_text(None)
                if text.is_error_or_none():
                    continue
                if text.text_value() == args2.text_value():
                    return Operand.TRUE
            return Operand.FALSE
        return self._parameter_error(1)

    def get_result_type(self) -> OperandType:
        return OperandType.BOOLEAN

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.JSON)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)
