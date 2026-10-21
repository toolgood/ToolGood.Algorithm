"""C# style extension functions (Split, Join, IndexOf, Regex, Trim...).

Ported from ``toolgood.algorithm.internals.functions.csharp``.  ``Guid`` is not
defined here: the parser maps the ``GUID`` constant to
:class:`toolgood_algorithm.functions.value.Function_GUID`.
"""
from __future__ import annotations

import bisect
import re

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import (
    Function_2,
    Function_3,
    Function_4,
    Function_N,
    _require_args,
)
from toolgood_algorithm.internals.functions.function_util import flatten_to_number_list, flatten_to_string_list
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


# ---------------------------------------------------------------------------
# helpers
# ---------------------------------------------------------------------------
def _json_item_matches(value, text: str) -> bool:
    """Compare a ``JsonData`` leaf with the text argument (``Has`` / ``HasValue``)."""
    if value.is_string():
        return value.string_value() == text
    if value.is_double():
        return str(value.number_value()) == text
    if value.is_boolean():
        return ("true" if value.boolean_value() else "false") == text.lower()
    return False


def _trim_start(text: str) -> str:
    start = 0
    while start < len(text) and text[start].isspace():
        start += 1
    return text[start:]


def _trim_end(text: str) -> str:
    end = len(text)
    while end > 0 and text[end - 1].isspace():
        end -= 1
    return text[:end]


def _to_unicode_replacement(text: str) -> str:
    """Translate Java/.NET ``$1`` group references into Python ``\\g<1>``."""
    return re.sub(r"\$(\d+)", r"\\g<\1>", text)


# ---------------------------------------------------------------------------
# functions
# ---------------------------------------------------------------------------
@register("ENDSWITH")
class Function_ENDSWITH(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "EndsWith", 2, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "EndsWith"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        text = args1.text_value()
        if self.func3 is None:
            return Operand.create(text.endswith(args2.text_value()))
        args3 = self._get_boolean_3(engine, tp)
        if args3.is_error_or_none():
            return args3
        if args3.boolean_value():
            return Operand.create(text.lower().endswith(args2.text_value().lower()))
        return Operand.create(text.endswith(args2.text_value()))

    def get_result_type(self) -> OperandType:
        return OperandType.BOOLEAN

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.BOOLEAN)


@register("HAS")
class Function_HAS(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Has", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Has"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self.func1.evaluate(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        if args1.is_array_json():
            return Operand.create(args1.contains_key(args2))
        if args1.is_json():
            json = args1.json_value()
            if json.is_array():
                for i in range(json.count()):
                    if _json_item_matches(json.get_child(i), args2.text_value()):
                        return Operand.TRUE
            elif json.get_child(args2.text_value()) is not None:
                return Operand.TRUE
            return Operand.FALSE
        if args1.is_array():
            for item in args1.array_value():
                text = item.to_text(None)
                if text.is_error_or_none():
                    continue
                if text.text_value() == args2.text_value():
                    return Operand.TRUE
            return Operand.FALSE
        return self._parameter_error(1)

    def get_result_type(self) -> OperandType:
        return OperandType.BOOLEAN

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.JSON)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)


@register("HASVALUE")
class Function_HASVALUE(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "HasValue", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "HasValue"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self.func1.evaluate(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        if args1.is_array_json():
            return Operand.create(args1.contains_value(args2))
        if args1.is_json():
            json = args1.json_value()
            if json.is_array():
                for i in range(json.count()):
                    if _json_item_matches(json.get_child(i), args2.text_value()):
                        return Operand.TRUE
            else:
                for value in json.inst_object.values():
                    if _json_item_matches(value, args2.text_value()):
                        return Operand.TRUE
            return Operand.FALSE
        if args1.is_array():
            for item in args1.array_value():
                text = item.to_text(None)
                if text.is_error_or_none():
                    continue
                if text.text_value() == args2.text_value():
                    return Operand.TRUE
            return Operand.FALSE
        return self._parameter_error(1)

    def get_result_type(self) -> OperandType:
        return OperandType.BOOLEAN

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.JSON)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)


@register("INDEXOF")
class Function_INDEXOF(Function_4):
    def __init__(self, funcs):
        _require_args(funcs, "IndexOf", 2, 4)
        super().__init__(funcs)

    def name(self) -> str:
        return "IndexOf"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        text = args1.text_value()
        search_str = args2.text_value()
        excel_index = engine.ExcelIndex

        if self.func3 is None:
            return Operand.create(text.find(search_str) + excel_index)

        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3
        start_index = args3.int_value() - excel_index
        if start_index < 0 or start_index > len(text):
            return self._parameter_error(3)

        if self.func4 is None:
            return Operand.create(text.find(search_str, start_index) + excel_index)

        args4 = self._get_number_4(engine, tp)
        if args4.is_error_or_none():
            return args4
        count = args4.int_value()
        if count < 0 or start_index + count > len(text):
            return self._parameter_error(4)

        # the match has to fit completely inside [startIndex, startIndex + count)
        local_index = text[start_index:start_index + count].find(search_str)
        index = -1 if local_index < 0 else local_index + start_index
        return Operand.create(index + excel_index)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func4 is not None:
            self.func4._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("ISREGEX")
class Function_ISREGEX(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "IsRegex", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "IsRegex"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        try:
            found = re.search(args2.text_value(), args1.text_value()) is not None
        except re.error:
            return self._parameter_error(2)
        return Operand.create(found)

    def get_result_type(self) -> OperandType:
        return OperandType.BOOLEAN

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)


@register("JOIN")
class Function_JOIN(Function_N):
    def __init__(self, funcs):
        _require_args(funcs, "Join", 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Join"

    def evaluate(self, engine, tp) -> Operand:
        args: list[Operand] = []
        error = self.try_evaluate_all(engine, tp, args)
        if error is not None:
            return error

        args1 = args[0]
        if args1.is_json():
            converted = args1.to_array(None)
            if not converted.is_error_or_none():
                args1 = converted

        if args1.is_array():
            values: list[str] = []
            if not flatten_to_string_list(args1, values):
                return self._parameter_error(1)
            separator = self._convert_to_text(args[1], 2)
            if separator.is_error_or_none():
                return separator
            return Operand.create(separator.text_value().join(values))

        args1 = self._convert_to_text(args1, 1)
        if args1.is_error_or_none():
            return args1
        values = []
        for i in range(1, len(args)):
            if not flatten_to_string_list(args[i], values):
                return self._parameter_error(i + 1)
        return Operand.create(args1.text_value().join(values))

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        for func in self.funcs:
            func._get_parameter_types(none_engine, result, OperandType.NONE)


@register("LASTINDEXOF")
class Function_LASTINDEXOF(Function_4):
    def __init__(self, funcs):
        _require_args(funcs, "LastIndexOf", 2, 4)
        super().__init__(funcs)

    def name(self) -> str:
        return "LastIndexOf"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        text = args1.text_value()
        search_str = args2.text_value()
        excel_index = engine.ExcelIndex

        if self.func3 is None:
            return Operand.create(text.rfind(search_str) + excel_index)

        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3
        start_index = args3.int_value() - excel_index
        if start_index < 0 or start_index > len(text):
            return self._parameter_error(3)

        if self.func4 is None:
            # the last matching character has to be at position <= startIndex
            from_index = start_index - len(search_str) + 1
            if from_index < 0:
                return Operand.create(-1 + excel_index)
            index = text.rfind(search_str, 0, from_index + len(search_str))
            return Operand.create(index + excel_index)

        args4 = self._get_number_4(engine, tp)
        if args4.is_error_or_none():
            return args4
        count = args4.int_value()
        if count < 0 or count > start_index + 1:
            return self._parameter_error(4)

        # search inside the [startIndex - count + 1, startIndex] window
        offset = start_index - count + 1
        end_pos = min(start_index + 1, len(text))
        window = text[offset:end_pos] if offset <= end_pos else ""
        local_index = window.rfind(search_str)
        index = -1 if local_index < 0 else local_index + offset
        return Operand.create(index + excel_index)

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func4 is not None:
            self.func4._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("LOOKCEILING")
class Function_LOOKCEILING(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "LookCeiling", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "LookCeiling"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_array_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        values: list = []
        if not flatten_to_number_list(args2, values):
            return self._parameter_error(2)
        if not values:
            return self._parameter_error(2)
        values.sort()

        value = args1.number_value()
        index = bisect.bisect_left(values, value)
        if index < len(values) and values[index] == value:
            return args1
        if index == len(values):
            # every element is smaller than value: return the biggest one
            return Operand.create(values[-1])
        return Operand.create(values[index])

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.ARRAY)


@register("LOOKFLOOR")
class Function_LOOKFLOOR(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "LookFloor", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "LookFloor"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_array_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        values: list = []
        if not flatten_to_number_list(args2, values):
            return self._parameter_error(2)
        if not values:
            return self._parameter_error(2)
        values.sort()

        value = args1.number_value()
        index = bisect.bisect_left(values, value)
        if index < len(values) and values[index] == value:
            return args1
        if index == 0:
            # every element is greater than value: return the smallest one
            return Operand.create(values[0])
        return Operand.create(values[index - 1])

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.ARRAY)


@register("REGEX")
class Function_REGEX(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Regex", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Regex"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        try:
            match = re.search(args2.text_value(), args1.text_value())
        except re.error:
            return self._parameter_error(2)
        if match is None:
            return self._function_error()
        return Operand.create(match.group(0))

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)


@register("REGEXREPLACE")
class Function_REGEXREPLACE(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "RegexReplace", 3, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "RegexReplace"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        args3 = self._get_text_3(engine, tp)
        if args3.is_error_or_none():
            return args3
        try:
            result = re.sub(args2.text_value(), _to_unicode_replacement(args3.text_value()), args1.text_value())
        except (re.error, IndexError):
            return self._parameter_error(2)
        return Operand.create(result)

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func3._get_parameter_types(none_engine, result, OperandType.TEXT)


@register("REMOVEEND")
class Function_REMOVEEND(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "RemoveEnd", 2, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "RemoveEnd"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        ignore_case = False
        if self.func3 is not None:
            args3 = self._get_boolean_3(engine, tp)
            if args3.is_error_or_none():
                return args3
            ignore_case = args3.boolean_value()

        text = args1.text_value()
        remove_text = args2.text_value()
        if ignore_case:
            if text.lower().endswith(remove_text.lower()):
                return Operand.create(text[:len(text) - len(remove_text)])
        elif text.endswith(remove_text):
            return Operand.create(text[:len(text) - len(remove_text)])
        return args1

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.BOOLEAN)


@register("REMOVESTART")
class Function_REMOVESTART(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "RemoveStart", 2, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "RemoveStart"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        ignore_case = False
        if self.func3 is not None:
            args3 = self._get_boolean_3(engine, tp)
            if args3.is_error_or_none():
                return args3
            ignore_case = args3.boolean_value()

        text = args1.text_value()
        remove_text = args2.text_value()
        if ignore_case:
            if text.lower().startswith(remove_text.lower()):
                return Operand.create(text[len(remove_text):])
        elif text.startswith(remove_text):
            return Operand.create(text[len(remove_text):])
        return args1

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.BOOLEAN)


@register("SPLIT")
class Function_SPLIT(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "Split", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "Split"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        separator = args2.text_value()
        if not separator:
            return self._parameter_error(2)
        return Operand.create([Operand.create(s) for s in args1.text_value().split(separator)])

    def get_result_type(self) -> OperandType:
        return OperandType.ARRAY

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)


@register("STARTSWITH")
class Function_STARTSWITH(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "StartsWith", 2, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "StartsWith"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        text = args1.text_value()
        if self.func3 is None:
            return Operand.create(text.startswith(args2.text_value()))
        args3 = self._get_boolean_3(engine, tp)
        if args3.is_error_or_none():
            return args3
        if args3.boolean_value():
            return Operand.create(text.lower().startswith(args2.text_value().lower()))
        return Operand.create(text.startswith(args2.text_value()))

    def get_result_type(self) -> OperandType:
        return OperandType.BOOLEAN

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.BOOLEAN)


@register("SUBSTRING")
class Function_SUBSTRING(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "Substring", 2, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "Substring"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        text = args1.text_value()
        start_index = args2.int_value() - engine.ExcelIndex

        if start_index < 0:
            return self._parameter_error(2)
        if start_index >= len(text):
            return Operand.create("")

        if self.func3 is None:
            return Operand.create(text[start_index:])

        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3

        length = args3.int_value()
        if length < 0:
            return self._parameter_error(3)
        if start_index + length > len(text):
            length = len(text) - start_index
        return Operand.create(text[start_index:start_index + length])

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("TRIMEND")
class Function_TRIMEND(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "TrimEnd", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "TrimEnd"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        text = args1.text_value()

        if self.func2 is None:
            return Operand.create(_trim_end(text))

        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        trim_chars = args2.text_value()
        end = len(text)
        while end > 0 and text[end - 1] in trim_chars:
            end -= 1
        return Operand.create(text[:end])

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)


@register("TRIMSTART")
class Function_TRIMSTART(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "TrimStart", 1, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "TrimStart"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        text = args1.text_value()

        if self.func2 is None:
            return Operand.create(_trim_start(text))

        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        trim_chars = args2.text_value()
        start = 0
        while start < len(text) and text[start] in trim_chars:
            start += 1
        return Operand.create(text[start:])

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        if self.func2 is not None:
            self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)
