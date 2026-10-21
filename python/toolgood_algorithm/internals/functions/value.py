"""Value nodes: literals, parameters, arrays, JSON and constants."""
from __future__ import annotations

import datetime as _dt
import math
import random
import uuid
from decimal import Decimal

from toolgood_algorithm.enums import (
    AreaUnitType,
    DistanceUnitType,
    MassUnitType,
    NumberUnitType,
    OperandType,
    VolumeUnitType,
)
from toolgood_algorithm.internals.functions.function_base import Function_0, Function_1, Function_2, Function_N, _require_args
from toolgood_algorithm.lit_json.json_data import JsonData
from toolgood_algorithm.lit_json.json_mapper import JsonMapper
from toolgood_algorithm.operands.my_date import MyDate
from toolgood_algorithm.operands.operand import KeyValue, Operand, OperandKeyValue, OperandKeyValueList, json_to_operand
from toolgood_algorithm.internals.functions.registry import register

# ---------------------------------------------------------------------------
# literals
# ---------------------------------------------------------------------------


class Function_Number(Function_0):
    def __init__(self, value: Operand):
        self._value = value

    def name(self) -> str:
        return "Num"

    def evaluate(self, engine, tp) -> Operand:
        return self._value

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append(str(self._value.number_value()))


class _Distance:
    FACTORS = {
        NumberUnitType.MM: Decimal("0.001"),
        NumberUnitType.CM: Decimal("0.01"),
        NumberUnitType.DM: Decimal("0.1"),
        NumberUnitType.M: Decimal("1"),
        NumberUnitType.KM: Decimal("1000"),
    }


_AREA_FACTORS = {
    NumberUnitType.MM2: Decimal("0.000001"),
    NumberUnitType.CM2: Decimal("0.0001"),
    NumberUnitType.DM2: Decimal("0.01"),
    NumberUnitType.M2: Decimal("1"),
    NumberUnitType.KM2: Decimal("1000000"),
}

_VOLUME_FACTORS = {
    NumberUnitType.MM3: Decimal("0.000000001"),
    NumberUnitType.CM3: Decimal("0.000001"),
    NumberUnitType.DM3: Decimal("0.001"),
    NumberUnitType.M3: Decimal("1"),
    NumberUnitType.KM3: Decimal("1000000000"),
}

_MASS_FACTORS = {
    NumberUnitType.G: Decimal("1"),
    NumberUnitType.KG: Decimal("1000"),
    NumberUnitType.T: Decimal("1000000"),
}

_UNIT_DICT = {
    "KM": NumberUnitType.KM,
    "M": NumberUnitType.M,
    "DM": NumberUnitType.DM,
    "CM": NumberUnitType.CM,
    "MM": NumberUnitType.MM,
    "KM2": NumberUnitType.KM2,
    "M2": NumberUnitType.M2,
    "DM2": NumberUnitType.DM2,
    "CM2": NumberUnitType.CM2,
    "MM2": NumberUnitType.MM2,
    "M3": NumberUnitType.M3,
    "DM3": NumberUnitType.DM3,
    "CM3": NumberUnitType.CM3,
    "MM3": NumberUnitType.MM3,
    "KM3": NumberUnitType.KM3,
    "L": NumberUnitType.DM3,
    "ML": NumberUnitType.CM3,
    "T": NumberUnitType.T,
    "KG": NumberUnitType.KG,
    "G": NumberUnitType.G,
}

_DISTANCE_ALIASES = {
    1: DistanceUnitType.MM,
    2: DistanceUnitType.CM,
    3: DistanceUnitType.DM,
    4: DistanceUnitType.M,
    5: DistanceUnitType.KM,
}
_AREA_ALIASES = {
    11: AreaUnitType.MM2,
    12: AreaUnitType.CM2,
    13: AreaUnitType.DM2,
    14: AreaUnitType.M2,
    15: AreaUnitType.KM2,
}
_VOLUME_ALIASES = {
    21: VolumeUnitType.MM3,
    22: VolumeUnitType.CM3,
    23: VolumeUnitType.DM3,
    24: VolumeUnitType.M3,
    25: VolumeUnitType.KM3,
}
_MASS_ALIASES = {31: MassUnitType.G, 32: MassUnitType.KG, 33: MassUnitType.T}


def _as_enum(value, mapping):
    if isinstance(value, int):
        return mapping[int(value)]
    return value


def transformation_unit(src: Decimal, type1, distance, area, volume, mass) -> Decimal:
    """Convert ``src`` expressed in ``type1`` into the engine configured unit."""
    if type1 is None:
        raise ValueError("Number unit is error!")
    if int(type1) in (1, 2, 3, 4, 5):
        target = _as_enum(distance, _DISTANCE_ALIASES)
        return src * _Distance.FACTORS[type1] / _Distance.FACTORS[target]
    if 11 <= int(type1) <= 15:
        target = _as_enum(area, _AREA_ALIASES)
        return src * _AREA_FACTORS[type1] / _AREA_FACTORS[target]
    if 21 <= int(type1) <= 25:
        target = _as_enum(volume, _VOLUME_ALIASES)
        return src * _VOLUME_FACTORS[type1] / _VOLUME_FACTORS[target]
    if 31 <= int(type1) <= 33:
        target = _as_enum(mass, _MASS_ALIASES)
        return src * _MASS_FACTORS[type1] / _MASS_FACTORS[target]
    raise ValueError("Number unit is error!")


class Function_Number2(Function_0):
    def __init__(self, value: Decimal, unit: str):
        self.d = value
        self.unit = unit

    def name(self) -> str:
        return "Num"

    def evaluate(self, engine, tp) -> Operand:
        unit_type = _UNIT_DICT.get(self.unit.upper())
        value = transformation_unit(self.d, unit_type, engine.DistanceUnit, engine.AreaUnit, engine.VolumeUnit, engine.MassUnit)
        return Operand.create(value)

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append(str(self.d))
        sb.append(self.unit)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER


class Function_Number_E(Function_0):
    _OPERAND = Operand.create(math.e)

    def name(self) -> str:
        return "E"

    def evaluate(self, engine, tp) -> Operand:
        return self._OPERAND

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append("E()")


class Function_Number_PI(Function_0):
    _OPERAND = Operand.create(math.pi)

    def name(self) -> str:
        return "PI"

    def evaluate(self, engine, tp) -> Operand:
        return self._OPERAND

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append("PI()")


class Function_ValueText(Function_0):
    def __init__(self, value: Operand):
        self._value = value

    def name(self) -> str:
        return "Value"

    def evaluate(self, engine, tp) -> Operand:
        return self._value

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append('"')
        text = self._value.text_value()
        for old, new in (
            ("\\", "\\\\"),
            ("\r", "\\r"),
            ("\n", "\\n"),
            ("\t", "\\t"),
            ("\0", "\\0"),
            ("\v", "\\v"),
            ("\a", "\\a"),
            ("\b", "\\b"),
            ("\f", "\\f"),
            ('"', '\\"'),
        ):
            text = text.replace(old, new)
        sb.append(text)
        sb.append('"')


class Function_ValueBoolean(Function_0):
    def __init__(self, value: bool):
        self._value = value

    def name(self) -> str:
        return "True" if self._value else "False"

    def evaluate(self, engine, tp) -> Operand:
        return Operand.TRUE if self._value else Operand.FALSE

    def get_result_type(self) -> OperandType:
        return OperandType.BOOLEAN

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append(self.name())


class Function_NULL(Function_0):
    def name(self) -> str:
        return "NULL"

    def evaluate(self, engine, tp) -> Operand:
        return Operand.NULL

    def get_result_type(self) -> OperandType:
        return OperandType.NULL

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append("NULL")


class Function_Version(Function_0):
    def name(self) -> str:
        return "ALGORITHMVERSION"

    def evaluate(self, engine, tp) -> Operand:
        return Operand.VERSION

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append(self.name())


@register("ERROR")
class Function_ERROR(Function_1):
    def __init__(self, funcs):
        if isinstance(funcs, (list, tuple)):
            _require_args(funcs, "Error", 1, 1)
            super().__init__(funcs)
        else:
            super().__init__([funcs])

    def name(self) -> str:
        return "Error"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        return Operand.error(args1.text_value())

    def get_result_type(self) -> OperandType:
        return OperandType.ERROR

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        if self.func1 is not None:
            self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)


@register("PARAM")
class Function_PARAM(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Param", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Param"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        if tp is not None:
            result = tp(engine, args1.text_value())
            if result is not None:
                return result
        if self.func2 is not None:
            return self.func2.evaluate(engine, tp)
        return Operand.error(f"Parameter '{args1.text_value()}' is missing.")

    def get_result_type(self) -> OperandType:
        return OperandType.NONE

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.NONE)


class Function_PARAMETER(Function_0):
    def __init__(self, name: str):
        self.name_text = name

    def name(self) -> str:
        return "Parameter"

    def evaluate(self, engine, tp) -> Operand:
        if tp is not None:
            result = tp(engine, self.name_text)
            if result is not None:
                return result
        return Operand.error(f"Parameter '{self.name_text}' is missing.")

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append(self.name_text)

    def get_result_type(self) -> OperandType:
        return OperandType.NONE

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        from toolgood_algorithm.internals.parameter_type import ParameterType

        result.append(ParameterType(self.name_text, operand_type, op, val))


class Function_DiyFunction(Function_N):
    def __init__(self, fun_name: str, funcs):
        super().__init__(funcs)
        self.fun_name = fun_name

    def name(self) -> str:
        return "DiyFunction"

    def evaluate(self, engine, tp) -> Operand:
        args = [func.evaluate(engine, tp) for func in self.funcs]
        return engine.execute_diy_function(self.fun_name, args)

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append(self.fun_name + "(")
        for i, func in enumerate(self.funcs):
            if i:
                sb.append(", ")
            func.to_string(sb, False)
        sb.append(")")

    def get_result_type(self) -> OperandType:
        return OperandType.NONE

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        for func in self.funcs:
            func._get_parameter_types(none_engine, result, OperandType.NONE)


@register("ARRAY")
class Function_ARRAY(Function_N):
    def __init__(self, funcs):
        _require_args(funcs, "Array", 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "Array"

    def evaluate(self, engine, tp) -> Operand:
        args: list[Operand] = []
        error = self.try_evaluate_all(engine, tp, args)
        if error is not None:
            return error
        return Operand.create(args)

    def get_result_type(self) -> OperandType:
        return OperandType.ARRAY

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        for func in self.funcs:
            func._get_parameter_types(none_engine, result, OperandType.NONE)


@register("ARRAYJSON")
class Function_ArrayJson(Function_N):
    def name(self) -> str:
        return "ArrayJson"

    def evaluate(self, engine, tp) -> Operand:
        result = OperandKeyValueList()
        for func in self.funcs:
            value = func.evaluate(engine, tp)
            if value.is_error_or_none():
                return value
            if isinstance(value, OperandKeyValue):
                result.add_value(value.value())
            else:
                return self._parameter_error(1)
        return result

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append("{")
        for i, func in enumerate(self.funcs):
            if i:
                sb.append(", ")
            func.to_string(sb, False)
        sb.append("}")

    def get_result_type(self) -> OperandType:
        return OperandType.ARRAYJSON

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        for func in self.funcs:
            func._get_parameter_types(none_engine, result, OperandType.NONE)


class Function_ArrayJsonItem(Function_1):
    def __init__(self, key: str, func1):
        super().__init__([func1])
        self.key = key

    def name(self) -> str:
        return "ArrayJsonItem"

    def evaluate(self, engine, tp) -> Operand:
        return OperandKeyValue(KeyValue(self.key, self.func1.evaluate(engine, tp)))

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append(str(self.key))
        sb.append(":")
        self.func1.to_string(sb, False)

    def get_result_type(self) -> OperandType:
        return OperandType.ARRAYJSON

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NONE)


@register("JSON")
class Function_JSON(Function_1):
    def __init__(self, funcs):
        _require_args(funcs, "Json", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "Json"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self.func1.evaluate(engine, tp)
        if args1.is_error_or_none():
            return args1
        if args1.is_json():
            return args1
        if args1.is_array_json():
            args1 = args1.to_text(None)
        if not args1.is_text():
            return self._parameter_error(1)
        text = args1.text_value()
        if (text.startswith("{") and text.endswith("}")) or (text.startswith("[") and text.endswith("]")):
            try:
                return Operand.create(JsonMapper.to_object(text))
            except Exception:  # noqa: BLE001
                return self._parameter_error(1)
        return self._parameter_error(1)

    def get_result_type(self) -> OperandType:
        return OperandType.JSON

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NONE)


class Function_GetJsonValue(Function_2):
    def __init__(self, func1, func2):
        super().__init__([func1, func2])

    def name(self) -> str:
        return "GetJsonValue"

    def evaluate(self, engine, tp) -> Operand:
        obj = self.func1.evaluate(engine, tp)
        if obj.is_error_or_none():
            return obj
        op = self.func2.evaluate(engine, tp)
        if op.is_error_or_none():
            return op

        if obj.is_array():
            op = self._convert_to_number(op, 2)
            if op.is_error_or_none():
                return op
            index = op.int_value() - engine.ExcelIndex
            array = obj.array_value()
            if 0 <= index < len(array):
                return array[index]
            return Operand.error(f"Function 'GetJsonValue' ARRAY index {index} out of range!")

        if obj.is_array_json():
            if op.is_number():
                key = op.number_value()
                result = obj.get_value(format(key, "f"))
                if result is not None:
                    return result
                return Operand.error(f"Function 'GetJsonValue' Parameter name '{format(key, 'f')}' is missing!")
            if op.is_text():
                result = obj.get_value(op.text_value())
                if result is not None:
                    return result
                return Operand.error(f"Function 'GetJsonValue' Parameter name '{op.text_value()}' is missing!")
            return Operand.error("Function 'GetJsonValue' Parameter name is missing!")

        if obj.is_json():
            json = obj.json_value()
            if json.is_array():
                op = self._convert_to_number(op, 2)
                if op.is_error_or_none():
                    return op
                index = op.int_value() - engine.ExcelIndex
                if 0 <= index < json.count():
                    return json_to_operand(json.get_child(index))
                return Operand.error(f"Function 'GetJsonValue' JSON index {index} out of range!")
            op = self._convert_to_text(op, 2)
            if op.is_error_or_none():
                return op
            value = json.get_child(op.text_value())
            if value is not None:
                return json_to_operand(value)
        return Operand.error("Function 'GetJsonValue' Operator is error!")

    def to_string(self, sb: list, add_brackets: bool) -> None:
        self.func1.to_string(sb, False)
        sb.append("[")
        self.func2.to_string(sb, False)
        sb.append("]")

    def get_result_type(self) -> OperandType:
        return OperandType.NONE

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.JSON)
        self.func2._get_parameter_types(none_engine, result, OperandType.NONE)


# ---------------------------------------------------------------------------
# constants
# ---------------------------------------------------------------------------


class Function_RAND(Function_0):
    def name(self) -> str:
        return "Rand"

    def evaluate(self, engine, tp) -> Operand:
        return Operand.create(Decimal(str(random.random())))

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append("Rand()")

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER


class Function_GUID(Function_0):
    def name(self) -> str:
        return "Guid"

    def evaluate(self, engine, tp) -> Operand:
        return Operand.create(str(uuid.uuid4()))

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append("Guid()")

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT


class Function_NOW(Function_0):
    def name(self) -> str:
        return "Now"

    def evaluate(self, engine, tp) -> Operand:
        if engine.UseLocalTime:
            return Operand.create(_dt.datetime.now())
        return Operand.create(_dt.datetime.utcnow())

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append("Now()")

    def get_result_type(self) -> OperandType:
        return OperandType.DATE


class Function_TODAY(Function_0):
    def name(self) -> str:
        return "Today"

    def evaluate(self, engine, tp) -> Operand:
        now = _dt.datetime.now() if engine.UseLocalTime else _dt.datetime.utcnow()
        return Operand.create(MyDate(now.year, now.month, now.day, 0, 0, 0))

    def to_string(self, sb: list, add_brackets: bool) -> None:
        sb.append("Today()")

    def get_result_type(self) -> OperandType:
        return OperandType.DATE
