"""Algorithm engine - the public entry point.

Ported from ``toolgood.algorithm.AlgorithmEngine`` / ``AlgorithmEngineEx``.
"""
from __future__ import annotations

import datetime as _dt
from decimal import Decimal
from typing import Optional

from toolgood_algorithm.enums import AreaUnitType, DistanceUnitType, MassUnitType, OperandType, VolumeUnitType
from toolgood_algorithm.internals.functions.function_base import FunctionBase
from toolgood_algorithm.lit_json.json_mapper import JsonMapper
from toolgood_algorithm.operands.my_date import MyDate
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.math.parser import ParseError, parse_function


class AlgorithmEngine:
    """Compiles and evaluates formulas."""

    def __init__(self, ignore_case: bool = False):
        self.ExcelIndex = 1
        self.IgnoreCase = ignore_case
        self.UseLocalTime = True
        self.UseStrictMode = True
        self.DistanceUnit = DistanceUnitType.M
        self.AreaUnit = AreaUnitType.M2
        self.VolumeUnit = VolumeUnitType.M3
        self.MassUnit = MassUnitType.KG
        self.LastError: Optional[str] = None
        self.UseParseCache = False
        self._parse_cache: dict[str, FunctionBase] = {}

    # ------------------------------------------------------------------ config
    def set_use_excel_index(self, value: bool) -> None:
        self.ExcelIndex = 1 if value else 0

    # -------------------------------------------------------------- overridable
    def get_parameter(self, parameter: str) -> Operand:
        """Return the value of ``parameter``. Override for custom variables."""
        return Operand.error(f"Parameter [{parameter}] is missing.")

    def execute_diy_function(self, parameter: str, args) -> Operand:
        """Execute a user defined function. Override for custom functions."""
        return Operand.error(f"DiyFunction [{parameter}] is missing.")

    # ------------------------------------------------------------------- parse
    def parse(self, exp: str) -> FunctionBase:
        self.LastError = None
        if exp is None or not exp.strip():
            self.LastError = "Parameter exp invalid !"
            raise ValueError(self.LastError)
        exp = exp.strip()
        if self.UseParseCache:
            cached = self._parse_cache.get(exp)
            if cached is not None:
                return cached
            func = self._parse_internal(exp, True)
            self._parse_cache[exp] = func
            return func
        return self._parse_internal(exp, True)

    def parse_without_error(self, exp: str) -> Optional[FunctionBase]:
        self.LastError = None
        if exp is None or not exp.strip():
            self.LastError = "Parameter exp invalid !"
            return None
        exp = exp.strip()
        if self.UseParseCache:
            cached = self._parse_cache.get(exp)
            if cached is not None:
                return cached
            func = self._parse_internal(exp, False)
            if func is not None:
                self._parse_cache[exp] = func
            return func
        return self._parse_internal(exp, False)

    def _parse_internal(self, exp: str, throw_on_error: bool) -> Optional[FunctionBase]:
        try:
            return parse_function(exp)
        except ParseError as exc:
            self.LastError = str(exc)
            if throw_on_error:
                raise ValueError(self.LastError) from exc
            return None

    # ---------------------------------------------------------------- evaluate
    def evaluate(self, function: FunctionBase) -> Operand:
        return function.evaluate(self, lambda engine, name: engine.get_parameter(name))

    # ------------------------------------------------------------- try evaluate
    def try_evaluate(self, exp: str, default=None):
        """Evaluate ``exp``; on error return ``default``.

        The default value's type selects the conversion (int/float/Decimal ->
        number, str -> text, bool -> bool, MyDate/datetime -> date, None ->
        the raw :class:`Operand`).
        """
        try:
            function = self.parse_without_error(exp)
            if function is None:
                return default
            obj = self.evaluate(function)
            if default is None:
                if obj.is_error():
                    self.LastError = obj.error_msg()
                    return None
                return None if obj.is_none() else obj
            if isinstance(default, bool):
                converted = obj if obj.is_boolean() else obj.to_boolean("It can't be converted to bool!")
                if converted.is_error():
                    self.LastError = converted.error_msg()
                    return default
                return converted.boolean_value()
            if isinstance(default, int):
                converted = obj if obj.is_number() else obj.to_number("It can't be converted to number!")
                if converted.is_error():
                    self.LastError = converted.error_msg()
                    return default
                return converted.int_value()
            if isinstance(default, (float, Decimal)):
                converted = obj if obj.is_number() else obj.to_number("It can't be converted to number!")
                if converted.is_error():
                    self.LastError = converted.error_msg()
                    return default
                return float(converted.double_value()) if isinstance(default, float) else converted.number_value()
            if isinstance(default, str):
                converted = obj if obj.is_text() else obj.to_text("It can't be converted to string!")
                if converted.is_error():
                    self.LastError = converted.error_msg()
                    return default
                return converted.text_value()
            if isinstance(default, (_dt.datetime, _dt.date, _dt.timedelta)):
                converted = obj if obj.is_date() else obj.to_my_date("It can't be converted to DateTime!")
                if converted.is_error():
                    self.LastError = converted.error_msg()
                    return default
                return converted.date_value().to_datetime()
            converted = obj if obj.is_date() else obj.to_my_date("It can't be converted to DateTime!")
            if converted.is_error():
                self.LastError = converted.error_msg()
                return default
            return converted.date_value()
        except Exception as exc:  # noqa: BLE001
            self.LastError = str(exc)
            return default

    def try_evaluate_my_date(self, exp: str, default: MyDate = None) -> Optional[MyDate]:
        return self._try_typed(
            exp,
            default,
            lambda obj: obj if obj.is_date() else obj.to_my_date("It can't be converted to DateTime!"),
            lambda obj: obj.date_value(),
        )

    # ------------------------------------------------------- typed overloads
    def _try_typed(self, exp: str, default, convert, extract):
        try:
            function = self.parse_without_error(exp)
            if function is None:
                return default
            obj = self.evaluate(function)
            converted = convert(obj)
            if converted.is_error() or converted.is_none():
                if converted.is_error():
                    self.LastError = converted.error_msg()
                return default
            return extract(converted)
        except Exception as exc:  # noqa: BLE001
            self.LastError = str(exc)
            return default

    def try_evaluate_text(self, exp: str, default: str = None) -> Optional[str]:
        """``TryEvaluate(string, string)`` - convert the result to text."""
        return self._try_typed(
            exp,
            default,
            lambda obj: obj if obj.is_text() else obj.to_text("It can't be converted to string!"),
            lambda obj: obj.text_value(),
        )

    def try_evaluate_int(self, exp: str, default: int = 0) -> int:
        """``TryEvaluate(string, int)``."""
        return self._try_typed(
            exp,
            default,
            lambda obj: obj if obj.is_number() else obj.to_number("It can't be converted to number!"),
            lambda obj: obj.int_value(),
        )

    def try_evaluate_bool(self, exp: str, default: bool = False) -> bool:
        """``TryEvaluate(string, bool)``."""
        return self._try_typed(
            exp,
            default,
            lambda obj: obj if obj.is_boolean() else obj.to_boolean("It can't be converted to bool!"),
            lambda obj: obj.boolean_value(),
        )

    def try_evaluate_number(self, exp: str, default: Decimal = None) -> Optional[Decimal]:
        """``TryEvaluate(string, decimal)``."""
        if default is None:
            default = Decimal(0)
        return self._try_typed(
            exp,
            default,
            lambda obj: obj if obj.is_number() else obj.to_number("It can't be converted to number!"),
            lambda obj: obj.number_value(),
        )

    def try_evaluate_double(self, exp: str, default: float = 0.0) -> float:
        """``TryEvaluate(string, double)``."""
        return self._try_typed(
            exp,
            default,
            lambda obj: obj if obj.is_number() else obj.to_number("It can't be converted to number!"),
            lambda obj: obj.double_value(),
        )

    def try_evaluate_datetime(self, exp: str, default=None):
        """``TryEvaluate(string, DateTime)``."""
        def extract(obj):
            date = obj.date_value()
            return date.to_datetime() if self.UseLocalTime else date.to_datetime()

        return self._try_typed(
            exp,
            default,
            lambda obj: obj if obj.is_date() else obj.to_my_date("It can't be converted to DateTime!"),
            extract,
        )

    # --------------------------------------------------------------- camel api
    # Wrappers (not direct assignments) so that subclass overrides are honoured.
    def GetParameter(self, parameter):
        return self.get_parameter(parameter)

    def ExecuteDiyFunction(self, parameter, args):
        return self.execute_diy_function(parameter, args)

    def Parse(self, exp):
        return self.parse(exp)

    def ParseWithoutError(self, exp):
        return self.parse_without_error(exp)

    def Evaluate(self, function):
        return self.evaluate(function)

    def TryEvaluate(self, exp, default=None):
        return self.try_evaluate(exp, default)

    def TryEvaluate_MyDate(self, exp, default=None):
        return self.try_evaluate_my_date(exp, default)

    def TryEvaluateText(self, exp, default=None):
        return self.try_evaluate_text(exp, default)

    def TryEvaluateInt(self, exp, default=0):
        return self.try_evaluate_int(exp, default)

    def TryEvaluateBool(self, exp, default=False):
        return self.try_evaluate_bool(exp, default)

    def SetUseExcelIndex(self, value):
        return self.set_use_excel_index(value)


class NoneEngine(AlgorithmEngine):
    """Engine used while inspecting parameter types (all values are ``NULL``)."""

    def __init__(self, engine: AlgorithmEngine):
        super().__init__(engine.IgnoreCase)
        self.AreaUnit = engine.AreaUnit
        self.DistanceUnit = engine.DistanceUnit
        self.LastError = engine.LastError
        self.MassUnit = engine.MassUnit
        self.UseLocalTime = engine.UseLocalTime
        self.ExcelIndex = engine.ExcelIndex

    def get_parameter(self, parameter: str) -> Operand:
        return Operand.NULL

    def execute_diy_function(self, parameter: str, args) -> Operand:
        return Operand.NULL
