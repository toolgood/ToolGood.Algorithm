"""``AlgorithmEngineEx`` - engine with a parameter dictionary (C#: AlgorithmEngineEx.cs)."""
from __future__ import annotations

from toolgood_algorithm.algorithm_engine import AlgorithmEngine
from toolgood_algorithm.lit_json.json_mapper import JsonMapper
from toolgood_algorithm.operands.my_date import MyDate
from toolgood_algorithm.operands.operand import Operand, json_to_operand


class AlgorithmEngineEx(AlgorithmEngine):
    """``AlgorithmEngine`` with a parameter dictionary and optional caching."""

    def __init__(self, ignore_case: bool = False):
        super().__init__(ignore_case)
        self.IgnoreCase = ignore_case
        self.UseTempDict = False
        self._tempdict: dict[str, Operand] = {}

    def get_parameter(self, parameter: str) -> Operand:
        if self.IgnoreCase:
            for key, value in self._tempdict.items():
                if key.lower() == parameter.lower():
                    return value
        else:
            if parameter in self._tempdict:
                return self._tempdict[parameter]
        result = self.get_parameter_ex(parameter)
        if self.UseTempDict:
            self._tempdict[parameter] = result
        return result

    def get_parameter_ex(self, parameter: str) -> Operand:
        return Operand.error(f"Parameter [{parameter}] is missing.")

    # -------------------------------------------------------------- parameters
    def clear_parameters(self) -> None:
        self._tempdict.clear()

    def add_parameter(self, key: str, obj) -> None:
        if isinstance(obj, Operand):
            self._tempdict[key] = obj
        elif isinstance(obj, MyDate):
            self._tempdict[key] = Operand.create(obj)
        else:
            self._tempdict[key] = Operand.create(obj)

    def add_parameter_from_json(self, json_text: str) -> None:
        if json_text.startswith("{") and json_text.endswith("}"):
            jo = JsonMapper.to_object(json_text)
            if jo.is_object():
                from toolgood_algorithm.operands.operand import json_to_operand

                for key, value in jo.inst_object.items():
                    if value.is_string():
                        self._tempdict[key] = Operand.create(value.string_value())
                    elif value.is_boolean():
                        self._tempdict[key] = Operand.create(value.boolean_value())
                    elif value.is_double():
                        self._tempdict[key] = Operand.create(value.number_value())
                    elif value.is_null():
                        self._tempdict[key] = Operand.NULL
                    else:
                        self._tempdict[key] = json_to_operand(value)
                return
        raise ValueError("Parameter is not json string.")

    def ClearParameters(self):
        return self.clear_parameters()

    def AddParameter(self, key, obj):
        return self.add_parameter(key, obj)

    def AddParameterFromJson(self, json_text):
        return self.add_parameter_from_json(json_text)

    def GetParameterEx(self, parameter):
        return self.get_parameter_ex(parameter)
