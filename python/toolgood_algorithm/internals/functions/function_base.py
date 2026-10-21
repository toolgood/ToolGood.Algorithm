"""Expression node base classes.

Mirrors ``FunctionBase`` / ``Function_0`` ... ``Function_N`` from the C# and
Java implementations.  Every node knows how to
  * evaluate itself (``evaluate``),
  * report its result type (``get_result_type``),
  * print itself back to text (``to_string``),
  * describe its parameter types (``get_parameter_types``).
"""
from __future__ import annotations

from decimal import Decimal
from typing import Callable, List

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.parameter_type import ParameterType


class FunctionBase:
    """Base class for all expression nodes."""

    # ------------------------------------------------------------------ core
    def name(self) -> str:
        raise NotImplementedError

    def evaluate(self, engine, temp_parameter: Callable) -> Operand:
        raise NotImplementedError

    def get_result_type(self) -> OperandType:
        raise NotImplementedError

    def get_parameter_types(self, engine) -> List[ParameterType]:
        from toolgood_algorithm.algorithm_engine import NoneEngine

        result: List[ParameterType] = []
        self._get_parameter_types(NoneEngine(engine), result, OperandType.NONE)
        return result

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        raise NotImplementedError

    def to_string(self, sb: list, add_brackets: bool) -> None:
        raise NotImplementedError

    def __str__(self) -> str:
        sb: list = []
        self.to_string(sb, False)
        return "".join(sb)

    # ------------------------------------------------------------ conversions
    def _convert_to_text(self, arg: Operand, index: int) -> Operand:
        return arg.to_text(f"Function '{self.name()}' parameter {index} is error!")

    def _convert_to_boolean(self, arg: Operand, index: int) -> Operand:
        return arg.to_boolean(f"Function '{self.name()}' parameter {index} is error!")

    def _convert_to_number(self, arg: Operand, index: int) -> Operand:
        return arg.to_number(f"Function '{self.name()}' parameter {index} is error!")

    def _convert_to_array(self, arg: Operand, index: int) -> Operand:
        return arg.to_array(f"Function '{self.name()}' parameter {index} is error!")

    def _convert_to_date(self, arg: Operand, index: int) -> Operand:
        return arg.to_my_date(f"Function '{self.name()}' parameter {index} is error!")

    # ------------------------------------------------------------------ errors
    def _parameter_error(self, index: int) -> Operand:
        return Operand.error(f"Function '{self.name()}' parameter {index} is error!")

    def _function_error(self) -> Operand:
        return Operand.error(f"Function '{self.name()}' parameter is error!")

    def _compare_error(self) -> Operand:
        return Operand.error(f"Function '{self.name()}' compare is error.")

    def _div0_error(self) -> Operand:
        return Operand.error(f"Function '{self.name()}' Div 0 error!")

    def _num_error(self) -> Operand:
        return Operand.error(f"Function '{self.name()}' Num error!")

    # --------------------------------------------------------------- try_eval
    def try_evaluate(self, engine, default, temp_parameter: Callable):
        """Evaluate and convert, returning ``default`` on any error."""
        try:
            obj = self.evaluate(engine, temp_parameter)
            if isinstance(default, bool):
                converted = obj if obj.is_boolean() else obj.to_boolean("It can't be converted to bool!")
                if converted.is_error():
                    engine.LastError = converted.error_msg()
                    return default
                return converted.boolean_value()
            if isinstance(default, int) and not isinstance(default, bool):
                converted = obj if obj.is_number() else obj.to_number("It can't be converted to number!")
                if converted.is_error():
                    engine.LastError = converted.error_msg()
                    return default
                return converted.int_value()
            if isinstance(default, (float, Decimal)):
                converted = obj if obj.is_number() else obj.to_number("It can't be converted to number!")
                if converted.is_error():
                    engine.LastError = converted.error_msg()
                    return default
                return float(converted.double_value()) if isinstance(default, float) else converted.number_value()
            if isinstance(default, str):
                converted = obj if obj.is_text() else obj.to_text("It can't be converted to string!")
                if converted.is_error():
                    engine.LastError = converted.error_msg()
                    return default
                return converted.text_value()
            converted = obj if obj.is_date() else obj.to_my_date("It can't be converted to DateTime!")
            if converted.is_error():
                engine.LastError = converted.error_msg()
                return default
            return converted.date_value()
        except Exception as exc:  # noqa: BLE001 - mirrors the original catch-all
            engine.LastError = str(exc)
            return default

    # CamelCase API (wrappers so subclass overrides are honoured)
    def Name(self):
        return self.name()

    def Evaluate(self, engine, temp_parameter):
        return self.evaluate(engine, temp_parameter)

    def GetResultType(self):
        return self.get_result_type()

    def GetParameterTypes(self, engine):
        return self.get_parameter_types(engine)

    def ToString(self, string_builder=None, add_brackets=False):
        if string_builder is None:
            return str(self)
        return self.to_string(string_builder, add_brackets)

    def TryEvaluate(self, engine, default, temp_parameter):
        return self.try_evaluate(engine, default, temp_parameter)


def _require_args(funcs, name: str, minimum: int, maximum: int = None) -> None:
    count = len(funcs)
    if maximum is None:
        if count < minimum:
            raise ValueError(f"Function '{name}' requires at least {minimum} parameter.")
    elif minimum == maximum:
        if count != minimum:
            raise ValueError(f"Function '{name}' requires exactly {minimum} parameter.")
    else:
        if count < minimum or count > maximum:
            raise ValueError(f"Function '{name}' requires {minimum} to {maximum} parameters.")


class Function_0(FunctionBase):
    def __init__(self, funcs=None):
        pass

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append(self.name() + "()")

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        pass


class _FuncsBase(FunctionBase):
    def _add_function(self, sb: list, function_name: str) -> None:
        sb.append(function_name)
        sb.append("(")
        for i, func in enumerate(self.funcs):
            if i:
                sb.append(", ")
            func.to_string(sb, False)
        sb.append(")")


class Function_1(FunctionBase):
    def __init__(self, funcs):
        self.func1 = funcs[0] if isinstance(funcs, (list, tuple)) else funcs

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append(self.name())
        sb.append("(")
        self.func1.to_string(sb, False)
        sb.append(")")

    def _add_function(self, sb: list, function_name: str) -> None:
        sb.append(function_name + "(")
        self.func1.to_string(sb, False)
        sb.append(")")

    def _get_text_1(self, engine, tp) -> Operand:
        a = self.func1.evaluate(engine, tp)
        return a if a.is_text() else self._convert_to_text(a, 1)

    def _get_number_1(self, engine, tp) -> Operand:
        a = self.func1.evaluate(engine, tp)
        return a if a.is_number() else self._convert_to_number(a, 1)

    def _get_date_1(self, engine, tp) -> Operand:
        a = self.func1.evaluate(engine, tp)
        return a if a.is_date() else self._convert_to_date(a, 1)

    def _get_boolean_1(self, engine, tp) -> Operand:
        a = self.func1.evaluate(engine, tp)
        return a if a.is_boolean() else self._convert_to_boolean(a, 1)

    def _get_array_1(self, engine, tp) -> Operand:
        a = self.func1.evaluate(engine, tp)
        return a if a.is_array() else self._convert_to_array(a, 1)

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, operand_type, op, val)


class Function_2(FunctionBase):
    def __init__(self, funcs):
        if isinstance(funcs, (list, tuple)):
            self.func1 = funcs[0]
            self.func2 = funcs[1] if len(funcs) > 1 else None
        else:
            self.func1 = funcs
            self.func2 = None

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append(self.name() + "(")
        self.func1.to_string(sb, False)
        if self.func2 is not None:
            sb.append(", ")
            self.func2.to_string(sb, False)
        sb.append(")")

    def _args(self):
        return [f for f in (self.func1, self.func2) if f is not None]

    def _get_text_1(self, engine, tp) -> Operand:
        a = self.func1.evaluate(engine, tp)
        return a if a.is_text() else self._convert_to_text(a, 1)

    def _get_number_1(self, engine, tp) -> Operand:
        a = self.func1.evaluate(engine, tp)
        return a if a.is_number() else self._convert_to_number(a, 1)

    def _get_date_1(self, engine, tp) -> Operand:
        a = self.func1.evaluate(engine, tp)
        return a if a.is_date() else self._convert_to_date(a, 1)

    def _get_boolean_1(self, engine, tp) -> Operand:
        a = self.func1.evaluate(engine, tp)
        return a if a.is_boolean() else self._convert_to_boolean(a, 1)

    def _get_array_1(self, engine, tp) -> Operand:
        a = self.func1.evaluate(engine, tp)
        return a if a.is_array() else self._convert_to_array(a, 1)

    def _get_text_2(self, engine, tp) -> Operand:
        a = self.func2.evaluate(engine, tp)
        return a if a.is_text() else self._convert_to_text(a, 2)

    def _get_number_2(self, engine, tp) -> Operand:
        a = self.func2.evaluate(engine, tp)
        return a if a.is_number() else self._convert_to_number(a, 2)

    def _get_date_2(self, engine, tp) -> Operand:
        a = self.func2.evaluate(engine, tp)
        return a if a.is_date() else self._convert_to_date(a, 2)

    def _get_boolean_2(self, engine, tp) -> Operand:
        a = self.func2.evaluate(engine, tp)
        return a if a.is_boolean() else self._convert_to_boolean(a, 2)

    def _get_array_2(self, engine, tp) -> Operand:
        a = self.func2.evaluate(engine, tp)
        return a if a.is_array() else self._convert_to_array(a, 2)

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, operand_type, op, val)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, operand_type, op, val)


class Function_3(Function_2):
    def __init__(self, funcs):
        super().__init__(funcs)
        self.func3 = funcs[2] if len(funcs) > 2 else None

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append(self.name() + "(")
        self.func1.to_string(sb, False)
        for f in (self.func2, self.func3):
            if f is not None:
                sb.append(", ")
                f.to_string(sb, False)
        sb.append(")")

    def _get_text_3(self, engine, tp) -> Operand:
        a = self.func3.evaluate(engine, tp)
        return a if a.is_text() else self._convert_to_text(a, 3)

    def _get_number_3(self, engine, tp) -> Operand:
        a = self.func3.evaluate(engine, tp)
        return a if a.is_number() else self._convert_to_number(a, 3)

    def _get_date_3(self, engine, tp) -> Operand:
        a = self.func3.evaluate(engine, tp)
        return a if a.is_date() else self._convert_to_date(a, 3)

    def _get_boolean_3(self, engine, tp) -> Operand:
        a = self.func3.evaluate(engine, tp)
        return a if a.is_boolean() else self._convert_to_boolean(a, 3)

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        super()._get_parameter_types(none_engine, result, operand_type, op, val)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, operand_type, op, val)


class Function_4(Function_3):
    def __init__(self, funcs):
        super().__init__(funcs)
        self.func4 = funcs[3] if len(funcs) > 3 else None

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append(self.name() + "(")
        self.func1.to_string(sb, False)
        for f in (self.func2, self.func3, self.func4):
            if f is not None:
                sb.append(", ")
                f.to_string(sb, False)
        sb.append(")")

    def _get_number_4(self, engine, tp) -> Operand:
        a = self.func4.evaluate(engine, tp)
        return a if a.is_number() else self._convert_to_number(a, 4)

    def _get_text_4(self, engine, tp) -> Operand:
        a = self.func4.evaluate(engine, tp)
        return a if a.is_text() else self._convert_to_text(a, 4)

    def _get_boolean_4(self, engine, tp) -> Operand:
        a = self.func4.evaluate(engine, tp)
        return a if a.is_boolean() else self._convert_to_boolean(a, 4)

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        super()._get_parameter_types(none_engine, result, operand_type, op, val)
        if self.func4 is not None:
            self.func4._get_parameter_types(none_engine, result, operand_type, op, val)


class Function_5(Function_4):
    def __init__(self, funcs):
        super().__init__(funcs)
        self.func5 = funcs[4] if len(funcs) > 4 else None

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append(self.name() + "(")
        self.func1.to_string(sb, False)
        for f in (self.func2, self.func3, self.func4, self.func5):
            if f is not None:
                sb.append(", ")
                f.to_string(sb, False)
        sb.append(")")

    def _get_number_5(self, engine, tp) -> Operand:
        a = self.func5.evaluate(engine, tp)
        return a if a.is_number() else self._convert_to_number(a, 5)

    def _get_text_5(self, engine, tp) -> Operand:
        a = self.func5.evaluate(engine, tp)
        return a if a.is_text() else self._convert_to_text(a, 5)

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        super()._get_parameter_types(none_engine, result, operand_type, op, val)
        if self.func5 is not None:
            self.func5._get_parameter_types(none_engine, result, operand_type, op, val)


class Function_6(Function_5):
    def __init__(self, funcs):
        super().__init__(funcs)
        self.func6 = funcs[5] if len(funcs) > 5 else None

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append(self.name() + "(")
        self.func1.to_string(sb, False)
        for f in (self.func2, self.func3, self.func4, self.func5, self.func6):
            if f is not None:
                sb.append(", ")
                f.to_string(sb, False)
        sb.append(")")

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        super()._get_parameter_types(none_engine, result, operand_type, op, val)
        if self.func6 is not None:
            self.func6._get_parameter_types(none_engine, result, operand_type, op, val)


class Function_N(FunctionBase):
    def __init__(self, funcs):
        self.funcs = list(funcs)

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append(self.name() + "(")
        for i, f in enumerate(self.funcs):
            if i:
                sb.append(", ")
            f.to_string(sb, False)
        sb.append(")")

    def try_evaluate_all(self, engine, tp, args: list) -> Operand | None:
        for func in self.funcs:
            value = func.evaluate(engine, tp)
            if value.is_error_or_none():
                return value
            args.append(value)
        return None

    def _get_text(self, engine, tp, idx: int) -> Operand:
        a = self.funcs[idx].evaluate(engine, tp)
        return a if a.is_text() else self._convert_to_text(a, idx)

    def _get_number(self, engine, tp, idx: int) -> Operand:
        a = self.funcs[idx].evaluate(engine, tp)
        return a if a.is_number() else self._convert_to_number(a, idx)

    def _get_date(self, engine, tp, idx: int) -> Operand:
        a = self.funcs[idx].evaluate(engine, tp)
        return a if a.is_date() else self._convert_to_date(a, idx)

    def _get_boolean(self, engine, tp, idx: int) -> Operand:
        a = self.funcs[idx].evaluate(engine, tp)
        return a if a.is_boolean() else self._convert_to_boolean(a, idx)

    def _get_array(self, engine, tp, idx: int) -> Operand:
        a = self.funcs[idx].evaluate(engine, tp)
        return a if a.is_array() else self._convert_to_array(a, idx)

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        for func in self.funcs:
            func._get_parameter_types(none_engine, result, operand_type, op, val)


# CamelCase aliases for callers migrating from the C#/Java API.
# Implemented as delegating wrappers so that overrides keep working.
def _make_alias(name):
    def method(self, *args, **kwargs):
        return getattr(self, name)(*args, **kwargs)

    return method


for _camel, _snake in (
    ("GetText_1", "_get_text_1"),
    ("GetNumber_1", "_get_number_1"),
    ("GetDate_1", "_get_date_1"),
    ("GetBoolean_1", "_get_boolean_1"),
    ("GetArray_1", "_get_array_1"),
    ("GetText_2", "_get_text_2"),
    ("GetNumber_2", "_get_number_2"),
    ("GetDate_2", "_get_date_2"),
    ("GetBoolean_2", "_get_boolean_2"),
    ("GetArray_2", "_get_array_2"),
    ("GetNumber_3", "_get_number_3"),
    ("GetText_3", "_get_text_3"),
    ("GetNumber_4", "_get_number_4"),
    ("GetText_4", "_get_text_4"),
    ("GetNumber_5", "_get_number_5"),
    ("GetText", "_get_text"),
    ("GetNumber", "_get_number"),
    ("GetDate", "_get_date"),
    ("GetBoolean", "_get_boolean"),
    ("GetArray", "_get_array"),
    ("TryEvaluateAll", "try_evaluate_all"),
    ("ParameterError", "_parameter_error"),
    ("FunctionError", "_function_error"),
    ("CompareError", "_compare_error"),
    ("Div0Error", "_div0_error"),
    ("NumError", "_num_error"),
):
    if hasattr(FunctionBase, _snake):
        setattr(FunctionBase, _camel, _make_alias(_snake))
