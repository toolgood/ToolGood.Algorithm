"""Function_JOIN (C#: Internals/Functions/Csharp/Function_JOIN.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_n import Function_N
from toolgood_algorithm.internals.functions.function_util import flatten_to_string_list
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("JOIN")
class Function_JOIN(Function_N):
    def __init__(self, funcs):
        _require_args(funcs, "Join", 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Join"

    def evaluate(self, engine, tp) -> Operand:
        args: list[Operand] = []
        error = self.try_evaluate_all(engine, tp, args)
        if error is not None:
            return error

        args1 = args[0]
        if args1.is_json():
            converted = args1.to_array(None)
            if not converted.is_error_or_none():
                args1 = converted

        if args1.is_array():
            values: list[str] = []
            if not flatten_to_string_list(args1, values):
                return self._parameter_error(1)
            separator = self._convert_to_text(args[1], 2)
            if separator.is_error_or_none():
                return separator
            return Operand.create(separator.text_value().join(values))

        args1 = self._convert_to_text(args1, 1)
        if args1.is_error_or_none():
            return args1
        values = []
        for i in range(1, len(args)):
            if not flatten_to_string_list(args[i], values):
                return self._parameter_error(i + 1)
        return Operand.create(args1.text_value().join(values))

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        for func in self.funcs:
            func._get_parameter_types(none_engine, result, OperandType.NONE)
