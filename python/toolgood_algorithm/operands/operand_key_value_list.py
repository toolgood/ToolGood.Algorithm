"""OperandKeyValueList (C#: Operands/OperandKeyValueList.cs)."""
from __future__ import annotations

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.numeric import plain
from toolgood_algorithm.operands.key_value import KeyValue
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.operands.operand_array import OperandArray
from toolgood_algorithm.operands.operand_string import OperandString


class OperandKeyValueList(Operand):
    __slots__ = ("_list",)

    def __init__(self):
        self._list: list[KeyValue] = []

    def is_array_json(self) -> bool:
        return True

    def type(self) -> OperandType:
        return OperandType.ARRAYJSON

    def array_value(self) -> list[Operand]:
        return [kv.value for kv in self._list]

    def to_text(self, error_message: str = None) -> Operand:
        return OperandString(str(self))

    def to_array(self, error_message: str = None) -> Operand:
        return OperandArray(self.array_value())

    def to_json(self, error_message: str = None) -> Operand:
        try:
            return Operand.create_json(str(self))
        except Exception:  # noqa: BLE001
            if error_message is None:
                error_message = "Convert to json error!"
            return Operand.error(error_message)

    def add_value(self, key_value: "KeyValue") -> None:
        self._list.append(key_value)

    def get_value(self, key: str) -> Optional[Operand]:
        for item in self._list:
            if item.key == key:
                return item.value
        return None

    def try_get_value(self, key: str):
        for item in self._list:
            if item.key == key:
                return item.value
        return None

    def contains_key(self, value: Operand) -> bool:
        text = value.text_value()
        return any(item.key == text for item in self._list)

    def contains_value(self, value: Operand) -> bool:
        text = value.text_value()
        for item in self._list:
            op = item.value
            if op.is_text():
                if op.text_value() == text:
                    return True
            elif op.is_number():
                if plain(op.number_value()) == text:
                    return True
            elif op.is_boolean():
                if str(op.boolean_value()).lower() == text.lower():
                    return True
        return False

    def __str__(self) -> str:
        return "{" + ",".join(OperandString(kv.key).__str__() + ":" + str(kv.value) for kv in self._list) + "}"
