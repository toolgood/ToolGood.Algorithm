"""Function_SEARCH (C#: Internals/Functions/String/Function_SEARCH.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_3 import Function_3
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _upper_char


@register("SEARCH")
class Function_SEARCH(Function_3):
    LITERAL = 0
    ANY_ONE = 1
    ANY_SEQ = 2

    def __init__(self, funcs):
        _require_args(funcs, "Search", 2, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "Search"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        excel_index = engine.ExcelIndex

        if self.func3 is None:
            index = self._wildcard_index_of(args2.text_value(), args1.text_value(), 0)
            if index < 0:
                # not found: Excel mode (1 based) is an error, C# mode returns -1
                return self._function_error() if engine.ExcelIndex == 1 else Operand.create(-1)
            return Operand.create(index + excel_index)
        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3
        start_index = args3.int_value() - excel_index
        if start_index < 0 or start_index >= len(args2.text_value()):
            return self._parameter_error(3)
        p2 = self._wildcard_index_of(args2.text_value(), args1.text_value(), start_index)
        if p2 < 0:
            return self._function_error() if engine.ExcelIndex == 1 else Operand.create(-1)
        return Operand.create(p2 + excel_index)

    # case insensitive wildcard search supporting Excel ``?`` / ``*`` / ``~``
    @classmethod
    def _wildcard_index_of(cls, text: str, pattern: str, start_index: int) -> int:
        if start_index < 0:
            start_index = 0
        if start_index > len(text):
            return -1
        tokens = cls._parse_wildcard(pattern)
        if not tokens:
            return start_index
        for start in range(start_index, len(text) + 1):
            if cls._match_wildcard(text, tokens, start):
                return start
        return -1

    @classmethod
    def _parse_wildcard(cls, pattern: str) -> list[tuple[int, str]]:
        tokens: list[tuple[int, str]] = []
        i = 0
        n = len(pattern)
        while i < n:
            c = pattern[i]
            if c == "~":
                if i + 1 < n and pattern[i + 1] in ("?", "*", "~"):
                    tokens.append((cls.LITERAL, _upper_char(pattern[i + 1])))
                    i += 1
                else:
                    tokens.append((cls.LITERAL, "~"))
            elif c == "*":
                tokens.append((cls.ANY_SEQ, "\0"))
            elif c == "?":
                tokens.append((cls.ANY_ONE, "\0"))
            else:
                tokens.append((cls.LITERAL, _upper_char(c)))
            i += 1
        return tokens

    @classmethod
    def _match_wildcard(cls, text: str, tokens: list[tuple[int, str]], start: int) -> bool:
        ti = 0
        si = start
        star_token = -1
        star_match = 0
        size = len(tokens)

        while ti < size:
            if si < len(text):
                kind, value = tokens[ti]
                if kind == cls.LITERAL:
                    if _upper_char(text[si]) == value:
                        ti += 1
                        si += 1
                    elif star_token != -1:
                        ti = star_token + 1
                        star_match += 1
                        si = star_match
                    else:
                        return False
                elif kind == cls.ANY_ONE:
                    ti += 1
                    si += 1
                else:
                    star_token = ti
                    star_match = si
                    ti += 1
            else:
                break

        while ti < size and tokens[ti][0] == cls.ANY_SEQ:
            ti += 1
        return ti == size

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)
        if self.func3 is not None:
            self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
