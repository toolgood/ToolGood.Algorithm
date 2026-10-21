"""Function_EXACT (C#: Internals/Functions/String/Function_EXACT.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("EXACT")
class Function_EXACT(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Exact", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Exact"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        return Operand.create(args1.text_value() == args2.text_value())

    def get_result_type(self) -> OperandType:
        return OperandType.BOOLEAN

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        t1 = self.func1.get_result_type()
        t2 = self.func2.get_result_type()
        if t1 == OperandType.NONE:
            p = none_engine.evaluate(self.func2).to_text(None)
            if t2 != OperandType.ERROR and not p.is_error_or_none():
                self.func1._get_parameter_types(none_engine, result, OperandType.TEXT, "==", p.text_value())
                self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)
                return
        elif t2 == OperandType.NONE:
            p = none_engine.evaluate(self.func1).to_text(None)
            if t1 != OperandType.ERROR and not p.is_error_or_none():
                self.func2._get_parameter_types(none_engine, result, OperandType.TEXT, "==", p.text_value())
                self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
                return
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)
