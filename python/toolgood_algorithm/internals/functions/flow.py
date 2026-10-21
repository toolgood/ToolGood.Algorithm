"""Flow control / type predicates: IF, IFS, SWITCH, IS* functions, NOT."""
from __future__ import annotations

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import Function_1, Function_2, Function_3, Function_N, _require_args
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("IF")
class Function_IF(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "IF", 2, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "IF"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_boolean_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        if args1.boolean_value():
            return self.func2.evaluate(engine, tp)
        if self.func3 is None:
            return Operand.FALSE
        return self.func3.evaluate(engine, tp)

    def get_result_type(self) -> OperandType:
        t2 = self.func2.get_result_type()
        if t2 != OperandType.NONE:
            return t2
        if self.func3 is not None:
            t3 = self.func3.get_result_type()
            if t3 != OperandType.NONE:
                return t3
        return OperandType.NONE

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.BOOLEAN)
        self.func2._get_parameter_types(none_engine, result, OperandType.NONE)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.NONE)


@register("IFERROR")
class Function_IFERROR(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "IFERROR", 2, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "IFERROR"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self.func1.evaluate(engine, tp)
        if args1.is_none():
            return args1
        if args1.is_error():
            return self.func2.evaluate(engine, tp)
        if self.func3 is None:
            return args1
        return self.func3.evaluate(engine, tp)

    def get_result_type(self) -> OperandType:
        t2 = self.func2.get_result_type()
        if t2 != OperandType.NONE:
            return t2
        if self.func3 is not None:
            t3 = self.func3.get_result_type()
            if t3 != OperandType.NONE:
                return t3
        return OperandType.NONE

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NONE)
        self.func2._get_parameter_types(none_engine, result, OperandType.NONE)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.NONE)


@register("IFS")
class Function_IFS(Function_N):
    def __init__(self, funcs):
        _require_args(funcs, "IFS", 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "IFS"

    def evaluate(self, engine, tp) -> Operand:
        for i in range(0, len(self.funcs) - 1, 2):
            condition = self._get_boolean(engine, tp, i)
            if condition.is_error_or_none():
                return condition
            if condition.boolean_value():
                return self.funcs[i + 1].evaluate(engine, tp)
        return self._function_error()

    def get_result_type(self) -> OperandType:
        for i in range(0, len(self.funcs) - 1, 2):
            t = self.funcs[i + 1].get_result_type()
            if t != OperandType.NONE:
                return t
        return OperandType.NONE

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        for i in range(0, len(self.funcs) - 1, 2):
            self.funcs[i]._get_parameter_types(none_engine, result, OperandType.BOOLEAN)
            self.funcs[i + 1]._get_parameter_types(none_engine, result, OperandType.NONE)


@register("SWITCH")
class Function_SWITCH(Function_N):
    def __init__(self, funcs):
        _require_args(funcs, "SWITCH", 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "SWITCH"

    def evaluate(self, engine, tp) -> Operand:
        expr_value = self.funcs[0].evaluate(engine, tp)
        if expr_value.is_error_or_none():
            return expr_value
        i = 1
        while i < len(self.funcs) - 1:
            compare_value = self.funcs[i].evaluate(engine, tp)
            if compare_value.is_error_or_none():
                return compare_value
            if _equals_operand(expr_value, compare_value):
                return self.funcs[i + 1].evaluate(engine, tp)
            i += 2
        if len(self.funcs) % 2 == 0:
            return self.funcs[-1].evaluate(engine, tp)
        return self._function_error()

    def get_result_type(self) -> OperandType:
        i = 1
        while i < len(self.funcs) - 1:
            t = self.funcs[i + 1].get_result_type()
            if t != OperandType.NONE:
                return t
            i += 2
        if len(self.funcs) % 2 == 0:
            t = self.funcs[-1].get_result_type()
            if t != OperandType.NONE:
                return t
        return OperandType.NONE

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        for i, func in enumerate(self.funcs):
            func._get_parameter_types(none_engine, result, OperandType.NONE if i % 2 == 0 else OperandType.NONE)


def _equals_operand(a: Operand, b: Operand) -> bool:
    if a.is_number() and b.is_number():
        return a.number_value() == b.number_value()
    if a.is_text() and b.is_text():
        return a.text_value() == b.text_value()
    if a.is_boolean() and b.is_boolean():
        return a.boolean_value() == b.boolean_value()
    if a.is_date() and b.is_date():
        return a.date_value().to_long() == b.date_value().to_long()
    if a.is_null() and b.is_null():
        return True
    return False


class _IsBase(Function_1):
    def __init__(self, funcs):
        _require_args(funcs, "IS", 1, 1)
        super().__init__(funcs)

    def get_result_type(self) -> OperandType:
        return OperandType.BOOLEAN

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NONE)


@register("ISERROR")
class Function_ISERROR(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "ISERROR", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "ISERROR"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self.func1.evaluate(engine, tp)
        if args1.is_none():
            return args1
        if self.func2 is not None:
            if args1.is_error():
                return self.func2.evaluate(engine, tp)
            return args1
        return Operand.TRUE if args1.is_error() else Operand.FALSE

    def get_result_type(self) -> OperandType:
        return self.func2.get_result_type() if self.func2 is not None else OperandType.BOOLEAN

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NONE)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NONE)


@register("ISNULL")
class Function_ISNULL(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "ISNULL", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "ISNULL"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self.func1.evaluate(engine, tp)
        if self.func2 is not None:
            if args1.is_null():
                return self.func2.evaluate(engine, tp)
            return args1
        return Operand.TRUE if args1.is_null() else Operand.FALSE

    def get_result_type(self) -> OperandType:
        return self.func2.get_result_type() if self.func2 is not None else OperandType.BOOLEAN

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NONE)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NONE)


@register("ISNULLORERROR")
class Function_ISNULLORERROR(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "ISNULLORERROR", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "ISNULLORERROR"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self.func1.evaluate(engine, tp)
        if self.func2 is not None:
            if args1.is_null() or args1.is_error_or_none():
                return self.func2.evaluate(engine, tp)
            return args1
        return Operand.TRUE if (args1.is_null() or args1.is_error_or_none()) else Operand.FALSE

    def get_result_type(self) -> OperandType:
        return self.func2.get_result_type() if self.func2 is not None else OperandType.BOOLEAN

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NONE)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NONE)


@register("ISNULLOREMPTY")
class Function_ISNULLOREMPTY(_IsBase):
    def name(self) -> str:
        return "ISNULLOREMPTY"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self.func1.evaluate(engine, tp)
        if args1.is_null():
            return Operand.TRUE
        text_arg = self._convert_to_text(args1, 1)
        if text_arg.is_error_or_none():
            return text_arg
        return Operand.create(text_arg.text_value() == "")


@register("ISNULLORWHITESPACE")
class Function_ISNULLORWHITESPACE(_IsBase):
    def name(self) -> str:
        return "ISNULLORWHITESPACE"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self.func1.evaluate(engine, tp)
        if args1.is_null():
            return Operand.TRUE
        text_arg = self._convert_to_text(args1, 1)
        if text_arg.is_error_or_none():
            return text_arg
        return Operand.create(text_arg.text_value().strip() == "")


@register("ISNUMBER")
class Function_ISNUMBER(_IsBase):
    def name(self) -> str:
        return "ISNUMBER"

    def evaluate(self, engine, tp) -> Operand:
        return Operand.TRUE if self.func1.evaluate(engine, tp).is_number() else Operand.FALSE


@register("ISTEXT")
class Function_ISTEXT(_IsBase):
    def name(self) -> str:
        return "ISTEXT"

    def evaluate(self, engine, tp) -> Operand:
        return Operand.TRUE if self.func1.evaluate(engine, tp).is_text() else Operand.FALSE


@register("ISNONTEXT")
class Function_ISNONTEXT(_IsBase):
    def name(self) -> str:
        return "ISNONTEXT"

    def evaluate(self, engine, tp) -> Operand:
        return Operand.FALSE if self.func1.evaluate(engine, tp).is_text() else Operand.TRUE


@register("ISLOGICAL")
class Function_ISLOGICAL(_IsBase):
    def name(self) -> str:
        return "ISLOGICAL"

    def evaluate(self, engine, tp) -> Operand:
        return Operand.TRUE if self.func1.evaluate(engine, tp).is_boolean() else Operand.FALSE


@register("ISEVEN")
class Function_ISEVEN(_IsBase):
    def name(self) -> str:
        return "ISEVEN"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self.func1.evaluate(engine, tp)
        if args1.is_number():
            return Operand.TRUE if args1.number_value().to_integral_value() % 2 == 0 else Operand.FALSE
        return Operand.FALSE

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("ISODD")
class Function_ISODD(_IsBase):
    def name(self) -> str:
        return "ISODD"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self.func1.evaluate(engine, tp)
        if args1.is_number():
            return Operand.TRUE if args1.number_value().to_integral_value() % 2 != 0 else Operand.FALSE
        return Operand.FALSE

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("NOT")
class Function_NOT(Function_1):
    def __init__(self, funcs):
        super().__init__(funcs if isinstance(funcs, (list, tuple)) else [funcs])
        _require_args([self.func1], "NOT", 1, 1)

    def name(self) -> str:
        return "NOT"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_boolean_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        return Operand.FALSE if args1.boolean_value() else Operand.TRUE

    def get_result_type(self) -> OperandType:
        return OperandType.BOOLEAN

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.BOOLEAN)
