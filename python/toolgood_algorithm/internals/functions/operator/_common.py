"""operator: shared helpers and base classes (kept out of the per-class files)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_2 import Function_2
from toolgood_algorithm.operands.operand import Operand


class _Arithmetic(Function_2):
    OP_TEXT = "?"
    RESULT = OperandType.NUMBER

    def __init__(self, funcs):
        super().__init__(funcs)

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        return self._apply(args1.number_value(), args2.number_value())

    def _apply(self, a: Decimal, b: Decimal) -> Operand:
        raise NotImplementedError

    def to_string(self, sb: list, add_brackets: bool) -> None:
        if add_brackets:
            sb.append("(")
        self.func1.to_string(sb, False)
        sb.append(self.OP_TEXT)
        self.func2.to_string(sb, False)
        if add_brackets:
            sb.append(")")

    def get_result_type(self) -> OperandType:
        return self.RESULT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, self.RESULT)
        self.func2._get_parameter_types(none_engine, result, self.RESULT)
