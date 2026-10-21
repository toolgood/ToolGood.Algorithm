"""Comparison operators: ``= == != <> > >= < <=``."""
from __future__ import annotations

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import Function_2
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


class _CompareBase(Function_2):
    OP_TEXT = " ? "
    OP_NAME = "?"

    def name(self) -> str:
        return self.OP_NAME

    # ---------------------------------------------------------------- compare
    def _compare_numbers(self, a, b) -> bool:
        raise NotImplementedError

    def _compare_texts(self, a: str, b: str) -> bool:
        raise NotImplementedError

    def _null_result_when_mixed(self) -> Operand:
        raise NotImplementedError

    def evaluate(self, engine, tp) -> Operand:
        args1 = self.func1.evaluate(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self.func2.evaluate(engine, tp)
        if args2.is_error_or_none():
            return args2

        if args1.type() == args2.type():
            if args1.is_number():
                return Operand.create(self._compare_numbers(args1.number_value(), args2.number_value()))
            if args1.is_text():
                return Operand.create(self._compare_texts(args1.text_value(), args2.text_value()))
            if args1.is_boolean():
                return Operand.create(self._compare_numbers(args1.to_number(None).number_value(), args2.to_number(None).number_value()))
            if args1.is_date():
                return Operand.create(
                    self._compare_numbers(args1.date_value().to_long(), args2.date_value().to_long())
                )
            if args1.is_null():
                # null is invalid data: comparing two nulls is an error
                return self._compare_error()
            return self._compare_error()
        if args1.is_null() or args2.is_null():
            return self._null_result_when_mixed()
        if (
            args1.is_date()
            or args2.is_date()
            or args1.is_json()
            or args2.is_json()
            or args1.is_array()
            or args2.is_array()
            or args1.is_array_json()
            or args2.is_array_json()
        ):
            return self._compare_error()
        args1 = self._convert_to_number(args1, 1)
        if args1.is_error_or_none():
            return args1
        args2 = self._convert_to_number(args2, 2)
        if args2.is_error_or_none():
            return args2
        return Operand.create(self._compare_numbers(args1.number_value(), args2.number_value()))

    def _compare_int(self, a: int, b: int) -> bool:
        return self._compare_numbers(a, b)

    # --------------------------------------------------------------- printing
    def to_string(self, sb: list, add_brackets: bool) -> None:
        if add_brackets:
            sb.append("(")
        self.func1.to_string(sb, False)
        sb.append(self.OP_TEXT)
        self.func2.to_string(sb, False)
        if add_brackets:
            sb.append(")")

    def get_result_type(self) -> OperandType:
        return OperandType.BOOLEAN

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        t1 = self.func1.get_result_type()
        t2 = self.func2.get_result_type()
        if t1 == OperandType.NONE:
            p = self.func2.evaluate(none_engine, None).to_text(None)
            if t2 != OperandType.ERROR and not p.is_error_or_none():
                self.func1._get_parameter_types(none_engine, result, t2, self.name(), p.text_value())
                self.func2._get_parameter_types(none_engine, result, t2)
                return
        elif t2 == OperandType.NONE:
            p = self.func1.evaluate(none_engine, None).to_text(None)
            if t1 != OperandType.ERROR and not p.is_error_or_none():
                self.func2._get_parameter_types(none_engine, result, t1, self.name(), p.text_value())
                self.func1._get_parameter_types(none_engine, result, t1)
                return
        self.func1._get_parameter_types(none_engine, result, OperandType.NONE)
        self.func2._get_parameter_types(none_engine, result, OperandType.NONE)


@register("EQ")
class Function_EQ(_CompareBase):
    OP_TEXT = " == "
    OP_NAME = "=="

    def _compare_numbers(self, a, b) -> bool:
        return a == b

    def _compare_texts(self, a, b) -> bool:
        return a == b

    def _null_result_when_mixed(self) -> Operand:
        return Operand.FALSE


@register("NE")
class Function_NE(_CompareBase):
    OP_TEXT = " != "
    OP_NAME = "!="

    def _compare_numbers(self, a, b) -> bool:
        return a != b

    def _compare_texts(self, a, b) -> bool:
        return a != b

    def _null_result_when_mixed(self) -> Operand:
        return Operand.TRUE


@register("GT")
class Function_GT(_CompareBase):
    OP_TEXT = " > "
    OP_NAME = ">"

    def _compare_numbers(self, a, b) -> bool:
        return a > b

    def _compare_texts(self, a, b) -> bool:
        return a > b

    def _null_result_when_mixed(self) -> Operand:
        return Operand.FALSE


@register("GE")
class Function_GE(_CompareBase):
    OP_TEXT = " >= "
    OP_NAME = ">="

    def _compare_numbers(self, a, b) -> bool:
        return a >= b

    def _compare_texts(self, a, b) -> bool:
        return a >= b

    def _null_result_when_mixed(self) -> Operand:
        return Operand.FALSE


@register("LT")
class Function_LT(_CompareBase):
    OP_TEXT = " < "
    OP_NAME = "<"

    def _compare_numbers(self, a, b) -> bool:
        return a < b

    def _compare_texts(self, a, b) -> bool:
        return a < b

    def _null_result_when_mixed(self) -> Operand:
        return Operand.FALSE


@register("LE")
class Function_LE(_CompareBase):
    OP_TEXT = " <= "
    OP_NAME = "<="

    def _compare_numbers(self, a, b) -> bool:
        return a <= b

    def _compare_texts(self, a, b) -> bool:
        return a <= b

    def _null_result_when_mixed(self) -> Operand:
        return Operand.FALSE
