"""Function_UNICHAR (C#: Internals/Functions/String/Function_UNICHAR.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_1 import Function_1
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("UNICHAR")
class Function_UNICHAR(Function_1):
    def __init__(self, funcs):
        if not isinstance(funcs, (list, tuple)):
            funcs = [funcs]
        _require_args(funcs, "UniChar", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "UniChar"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        code = args1.int_value()
        if code < 0 or code > 0x10FFFF or 0xD800 <= code <= 0xDFFF:
            return self._parameter_error(1)
        try:
            return Operand.create(chr(code))
        except (ValueError, OverflowError):
            return self._parameter_error(1)

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
