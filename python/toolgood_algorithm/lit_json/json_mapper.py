"""``JsonMapper`` - JSON text parser (LitJson/JsonMapper.cs).

LitJson is more permissive than the JSON standard: it accepts single quoted
strings and trailing commas, which the original test suites rely on.  This
module implements a small recursive descent parser with the same tolerance.
"""
from __future__ import annotations

from decimal import Decimal, InvalidOperation

from toolgood_algorithm.lit_json.json_data import JsonData
from toolgood_algorithm.lit_json.json_exception import JsonException

_WHITESPACE = " \t\r\n\f"
_NUMBER_CHARS = "0123456789+-.eE"


class _Parser:
    def __init__(self, text: str):
        self.text = text
        self.pos = 0

    def error(self, message: str):
        raise JsonException(f"{message} at position {self.pos}")

    def skip_ws(self) -> None:
        while self.pos < len(self.text) and self.text[self.pos] in _WHITESPACE:
            self.pos += 1

    def peek(self) -> str:
        return self.text[self.pos] if self.pos < len(self.text) else ""

    def parse_value(self):
        self.skip_ws()
        ch = self.peek()
        if ch == "":
            self.error("unexpected end of json")
        if ch == "{":
            return self.parse_object()
        if ch == "[":
            return self.parse_array()
        if ch in ("'", '"'):
            return self.parse_string()
        if ch == "-" or ch.isdigit():
            return self.parse_number()
        if self.text.startswith("true", self.pos):
            self.pos += 4
            return True
        if self.text.startswith("false", self.pos):
            self.pos += 5
            return False
        if self.text.startswith("null", self.pos):
            self.pos += 4
            return None
        self.error(f"unexpected character '{ch}'")

    def parse_object(self):
        self.pos += 1  # {
        result = {}
        while True:
            self.skip_ws()
            if self.peek() == "}":
                self.pos += 1
                return result
            if self.peek() in ("'", '"'):
                key = self.parse_string()
            else:
                start = self.pos
                while self.pos < len(self.text) and self.text[self.pos] not in ":,}" + _WHITESPACE:
                    self.pos += 1
                key = self.text[start:self.pos]
            self.skip_ws()
            if self.peek() != ":":
                self.error("expected ':'")
            self.pos += 1
            result[key] = self.parse_value()
            self.skip_ws()
            if self.peek() == ",":
                self.pos += 1
                continue
            if self.peek() == "}":
                self.pos += 1
                return result
            self.error("expected ',' or '}'")

    def parse_array(self):
        self.pos += 1  # [
        result = []
        while True:
            self.skip_ws()
            if self.peek() == "]":
                self.pos += 1
                return result
            result.append(self.parse_value())
            self.skip_ws()
            if self.peek() == ",":
                self.pos += 1
                continue
            if self.peek() == "]":
                self.pos += 1
                return result
            self.error("expected ',' or ']'")

    def parse_string(self) -> str:
        quote = self.text[self.pos]
        self.pos += 1
        out = []
        while self.pos < len(self.text):
            ch = self.text[self.pos]
            if ch == "\\":
                self.pos += 1
                if self.pos >= len(self.text):
                    self.error("unterminated escape")
                esc = self.text[self.pos]
                mapping = {
                    "n": "\n",
                    "r": "\r",
                    "t": "\t",
                    "b": "\b",
                    "f": "\f",
                    '"': '"',
                    "'": "'",
                    "\\": "\\",
                    "/": "/",
                }
                if esc == "u":
                    hex_text = self.text[self.pos + 1:self.pos + 5]
                    try:
                        out.append(chr(int(hex_text, 16)))
                    except ValueError:
                        self.error("invalid unicode escape")
                    self.pos += 5
                    continue
                out.append(mapping.get(esc, esc))
                self.pos += 1
                continue
            if ch == quote:
                self.pos += 1
                return "".join(out)
            out.append(ch)
            self.pos += 1
        self.error("unterminated string")

    def parse_number(self):
        start = self.pos
        while self.pos < len(self.text) and self.text[self.pos] in _NUMBER_CHARS:
            self.pos += 1
        text = self.text[start:self.pos]
        try:
            return Decimal(text)
        except InvalidOperation:
            self.error(f"invalid number '{text}'")


class JsonMapper:
    """Small replacement for LitJson's ``JsonMapper``."""

    @staticmethod
    def to_object(text: str) -> JsonData:
        parser = _Parser(text)
        value = parser.parse_value()
        parser.skip_ws()
        if parser.pos != len(text):
            raise JsonException(f"additional text at position {parser.pos}")
        return JsonData(value)


JsonMapper.ToObject = JsonMapper.to_object
