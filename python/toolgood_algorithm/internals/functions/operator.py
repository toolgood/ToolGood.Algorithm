"""Arithmetic / logical operators (``+ - * / % & && || and or xor``)."""
from __future__ import annotations

from decimal import Decimal

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import Function_2, Function_N
from toolgood_algorithm.internals.numeric import DECIMAL_MAX, ONE, ZERO
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


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


@register("ADD")
class Function_Add(_Arithmetic):
    NAME = "+"
    OP_TEXT = " + "

    def name(self) -> str:
        return "+"

    def _apply(self, a: Decimal, b: Decimal) -> Operand:
        result = a + b
        if abs(result) > DECIMAL_MAX:
            return self._num_error()
        return Operand.create(result)


@register("SUB")
class Function_Sub(_Arithmetic):
    OP_TEXT = " - "

    def name(self) -> str:
        return "-"

    def _apply(self, a: Decimal, b: Decimal) -> Operand:
        if b == ZERO:
            return Operand.create(a)
        result = a - b
        if abs(result) > DECIMAL_MAX:
            return self._num_error()
        return Operand.create(result)


@register("MUL")
class Function_Mul(_Arithmetic):
    OP_TEXT = " * "

    def name(self) -> str:
        return "*"

    def _apply(self, a: Decimal, b: Decimal) -> Operand:
        if a == ONE:
            return Operand.create(b)
        if b == ONE:
            return Operand.create(a)
        result = a * b
        if abs(result) > DECIMAL_MAX:
            return self._num_error()
        return Operand.create(result)

    def to_string(self, sb: list, add_brackets: bool) -> None:
        if add_brackets:
            sb.append("(")
        self.func1.to_string(sb, True)
        sb.append(" * ")
        self.func2.to_string(sb, True)
        if add_brackets:
            sb.append(")")


@register("DIV")
class Function_Div(_Arithmetic):
    OP_TEXT = " / "

    def name(self) -> str:
        return "/"

    def _apply(self, a: Decimal, b: Decimal) -> Operand:
        from toolgood_algorithm.internals.numeric import divide

        if b == ZERO:
            return self._div0_error()
        if b == ONE:
            return Operand.create(a)
        result = divide(a, b)
        if abs(result) > DECIMAL_MAX:
            return self._num_error()
        return Operand.create(result)

    def to_string(self, sb: list, add_brackets: bool) -> None:
        if add_brackets:
            sb.append("(")
        self.func1.to_string(sb, True)
        sb.append(" / ")
        self.func2.to_string(sb, True)
        if add_brackets:
            sb.append(")")


@register("MOD")
class Function_Mod(_Arithmetic):
    OP_TEXT = " % "

    def name(self) -> str:
        return "%"

    def _apply(self, a: Decimal, b: Decimal) -> Operand:
        if b == ZERO:
            return self._div0_error()
        # Excel MOD: the result takes the sign of the divisor
        r = a % b
        if r != ZERO and (r < 0) != (b < 0):
            r += b
        return Operand.create(r)

    def to_string(self, sb: list, add_brackets: bool) -> None:
        if add_brackets:
            sb.append("(")
        self.func1.to_string(sb, True)
        sb.append(" % ")
        self.func2.to_string(sb, True)
        if add_brackets:
            sb.append(")")


@register("CONNECT")
class Function_Connect(_Arithmetic):
    OP_TEXT = " & "
    RESULT = OperandType.TEXT

    def name(self) -> str:
        return "&"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        return Operand.create(args1.text_value() + args2.text_value())


class Function_AND(Function_2):
    def name(self) -> str:
        return "And"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_boolean_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        if not args1.boolean_value():
            if engine.UseStrictMode:
                args2 = self._get_boolean_2(engine, tp)
                if args2.is_error_or_none():
                    return args2
            return Operand.FALSE
        return self._get_boolean_2(engine, tp)

    def to_string(self, sb: list, add_brackets: bool) -> None:
        if add_brackets:
            sb.append("(")
        self.func1.to_string(sb, False)
        sb.append(" && ")
        self.func2.to_string(sb, False)
        if add_brackets:
            sb.append(")")

    def get_result_type(self) -> OperandType:
        return OperandType.BOOLEAN

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.BOOLEAN)
        self.func2._get_parameter_types(none_engine, result, OperandType.BOOLEAN)


class Function_OR(Function_2):
    def name(self) -> str:
        return "Or"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_boolean_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        if args1.boolean_value():
            if engine.UseStrictMode:
                args2 = self._get_boolean_2(engine, tp)
                if args2.is_error_or_none():
                    return args2
            return Operand.TRUE
        return self._get_boolean_2(engine, tp)

    def to_string(self, sb: list, add_brackets: bool) -> None:
        if add_brackets:
            sb.append("(")
        self.func1.to_string(sb, False)
        sb.append(" || ")
        self.func2.to_string(sb, False)
        if add_brackets:
            sb.append(")")

    def get_result_type(self) -> OperandType:
        return OperandType.BOOLEAN

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.BOOLEAN)
        self.func2._get_parameter_types(none_engine, result, OperandType.BOOLEAN)


@register("AND")
class Function_AND_N(Function_N):
    def name(self) -> str:
        return "And"

    def evaluate(self, engine, tp) -> Operand:
        value = True
        for i in range(len(self.funcs)):
            arg = self._get_boolean(engine, tp, i)
            if arg.is_error_or_none():
                return arg
            if not arg.boolean_value():
                value = False
                if not engine.UseStrictMode:
                    break
        return Operand.TRUE if value else Operand.FALSE

    def get_result_type(self) -> OperandType:
        return OperandType.BOOLEAN

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        for func in self.funcs:
            func._get_parameter_types(none_engine, result, OperandType.BOOLEAN)


@register("OR")
class Function_OR_N(Function_N):
    def name(self) -> str:
        return "Or"

    def evaluate(self, engine, tp) -> Operand:
        value = False
        for i in range(len(self.funcs)):
            arg = self._get_boolean(engine, tp, i)
            if arg.is_error_or_none():
                return arg
            if arg.boolean_value():
                value = True
                if not engine.UseStrictMode:
                    break
        return Operand.TRUE if value else Operand.FALSE

    def get_result_type(self) -> OperandType:
        return OperandType.BOOLEAN

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        for func in self.funcs:
            func._get_parameter_types(none_engine, result, OperandType.BOOLEAN)


@register("XOR")
class Function_XOR(Function_N):
    def name(self) -> str:
        return "Xor"

    def evaluate(self, engine, tp) -> Operand:
        true_count = 0
        for i in range(len(self.funcs)):
            arg = self._get_boolean(engine, tp, i)
            if arg.is_error_or_none():
                return arg
            if arg.boolean_value():
                true_count += 1
        return Operand.TRUE if true_count % 2 == 1 else Operand.FALSE

    def get_result_type(self) -> OperandType:
        return OperandType.BOOLEAN

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        for func in self.funcs:
            func._get_parameter_types(none_engine, result, OperandType.BOOLEAN)
