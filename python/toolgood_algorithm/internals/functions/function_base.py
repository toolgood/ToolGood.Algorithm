"""``FunctionBase`` - base class of every expression node.

C# splits the hierarchy into ``FunctionBase.cs`` and ``Function_0.cs`` ...
``Function_N.cs``; the ``Function_0`` ... ``Function_N`` family lives in
:mod:`toolgood_algorithm.internals.functions.function_n`.
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
        from toolgood_algorithm.internals.functions.none_engine import NoneEngine

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


def _make_alias(name):
    def method(self, *args, **kwargs):
        return getattr(self, name)(*args, **kwargs)

    return method
