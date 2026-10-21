"""Recursive descent parser (``mathParser`` + ``MathFunctionVisitor``).

The original project uses ANTLR (``g4/math.g4``); this module implements the
same grammar by hand and produces an :class:`AstNode` tree.  ``to_function``
converts that tree into executable ``FunctionBase`` nodes.
"""
from __future__ import annotations

import re
from dataclasses import dataclass, field
from decimal import Decimal, InvalidOperation
from typing import List

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.math.lexer import KEYWORDS, ParseError, Token, Tokenizer

@dataclass
class AstNode:
    kind: str
    start: int
    end: int
    text: str = ""
    value: object = None
    op: str = None
    keyword: str = None
    children: List["AstNode"] = field(default_factory=list)
    key_is_parameter: bool = False

    # ------------------------------------------------------------------ build
    def to_function(self):
        from toolgood_algorithm.internals import functions  # noqa: F401  (ensures registration)
        from toolgood_algorithm.internals.functions.registry import FUNCTIONS

        kind = self.kind
        if kind == "num":
            from toolgood_algorithm.internals.functions.value import Function_Number, Function_Number2

            from toolgood_algorithm.operands.operand import Operand

            if isinstance(self.value, tuple):
                return Function_Number2(self.value[0], self.value[1])
            return Function_Number(Operand.create(self.value))
        if kind == "string":
            from toolgood_algorithm.internals.functions.value import Function_ValueText
            from toolgood_algorithm.operands.operand import Operand

            return Function_ValueText(Operand.create(self.value))
        if kind == "param":
            from toolgood_algorithm.internals.functions.value import Function_PARAMETER

            return Function_PARAMETER(self.text)
        if kind == "const2":
            from toolgood_algorithm.internals.functions.value import Function_NULL, Function_ValueBoolean, Function_Version

            if self.keyword == "TRUE":
                return Function_ValueBoolean(True)
            if self.keyword == "FALSE":
                return Function_ValueBoolean(False)
            if self.keyword == "ALGORITHMVERSION":
                return Function_Version()
            return Function_NULL()
        if kind == "const":
            from toolgood_algorithm.internals.functions.value import Function_Number_E, Function_Number_PI, Function_GUID, Function_RAND, Function_NOW, Function_TODAY

            return {
                "E": Function_Number_E,
                "PI": Function_Number_PI,
                "RAND": Function_RAND,
                "GUID": Function_GUID,
                "NOW": Function_NOW,
                "TODAY": Function_TODAY,
            }[self.keyword]()
        if kind == "call":
            args = [child.to_function() for child in self.children]
            if self.keyword == "PARAMETER":
                from toolgood_algorithm.internals.functions.value import Function_DiyFunction

                return Function_DiyFunction(self.text, args)
            cls = FUNCTIONS.get(self.keyword)
            if cls is None:
                from toolgood_algorithm.internals.functions.value import Function_DiyFunction

                return Function_DiyFunction(self.text, args)
            return cls(args)
        if kind == "method":
            args = [child.to_function() for child in self.children]
            if self.keyword != "PARAMETER":
                cls = FUNCTIONS.get(self.keyword)
                if cls is not None:
                    return cls(args)
            from toolgood_algorithm.internals.functions.value import Function_DiyFunction

            return Function_DiyFunction(self.text, args)
        if kind == "index":
            from toolgood_algorithm.internals.functions.value import Function_GetJsonValue, Function_PARAMETER
            from toolgood_algorithm.internals.functions.value import Function_ValueText
            from toolgood_algorithm.operands.operand import Operand

            obj_func = self.children[0].to_function()
            if self.key_is_parameter:
                key_func = Function_PARAMETER(self.children[1].text)
            else:
                key_func = self.children[1].to_function()
            return Function_GetJsonValue(obj_func, key_func)
        if kind == "dotkey":
            from toolgood_algorithm.internals.functions.value import Function_GetJsonValue, Function_ValueText
            from toolgood_algorithm.operands.operand import Operand

            return Function_GetJsonValue(self.children[0].to_function(), Function_ValueText(Operand.create(self.text)))
        if kind == "bracket":
            return self.children[0].to_function()
        if kind == "binary":
            return self._binary_function()
        if kind == "unary":
            from toolgood_algorithm.internals.functions.flow import Function_NOT

            return Function_NOT(self.children[0].to_function())
        if kind == "percent":
            from toolgood_algorithm.internals.functions.math_base import Function_Percentage

            return Function_Percentage(self.children[0].to_function())
        if kind == "ternary":
            from toolgood_algorithm.internals.functions.flow import Function_IF

            return Function_IF([c.to_function() for c in self.children])
        if kind == "array":
            from toolgood_algorithm.internals.functions.value import Function_ARRAY

            return Function_ARRAY([c.to_function() for c in self.children])
        if kind == "arrayjson":
            from toolgood_algorithm.internals.functions.value import Function_ArrayJson

            return Function_ArrayJson([c.to_function() for c in self.children])
        if kind == "arrayjsonitem":
            from toolgood_algorithm.internals.functions.value import Function_ArrayJsonItem

            return Function_ArrayJsonItem(self.text, self.children[0].to_function())
        raise ParseError(f"unsupported node '{kind}'")

    def _binary_function(self):
        from toolgood_algorithm.internals.functions import compare, operator

        op = self.op
        if op == "OPMUL":
            return operator.Function_Mul([c.to_function() for c in self.children])
        if op == "OPDIV":
            return operator.Function_Div([c.to_function() for c in self.children])
        if op == "OPMOD":
            return operator.Function_Mod([c.to_function() for c in self.children])
        if op == "OPADD":
            return operator.Function_Add([c.to_function() for c in self.children])
        if op == "OPSUB":
            return operator.Function_Sub([c.to_function() for c in self.children])
        if op == "OPCAT":
            return operator.Function_Connect([c.to_function() for c in self.children])
        if op == "OPGT":
            return compare.Function_GT([c.to_function() for c in self.children])
        if op == "OPGE":
            return compare.Function_GE([c.to_function() for c in self.children])
        if op == "OPLT":
            return compare.Function_LT([c.to_function() for c in self.children])
        if op == "OPLE":
            return compare.Function_LE([c.to_function() for c in self.children])
        if op == "OPEQ":
            return compare.Function_EQ([c.to_function() for c in self.children])
        if op == "OPNE":
            return compare.Function_NE([c.to_function() for c in self.children])
        if op == "OPAND":
            from toolgood_algorithm.internals.functions.operator import Function_AND

            return Function_AND([c.to_function() for c in self.children])
        if op == "OPOR":
            from toolgood_algorithm.internals.functions.operator import Function_OR

            return Function_OR([c.to_function() for c in self.children])
        raise ParseError(f"unsupported operator '{op}'")

    # --------------------------------------------------------------- traversal
    def walk(self):
        yield self
        for child in self.children:
            yield from child.walk()


"""Recursive descent parser (equivalent of ``mathParser``).

Produces :class:`toolgood_algorithm.math.ast_node.AstNode` trees which are
converted to executable ``FunctionBase`` nodes by ``AstNode.to_function``.
"""

_PRECEDENCE = {
    "OPOR": 2,
    "OPAND": 3,
    "OPNE": 4,
    "OPEQ": 4,
    "OPGT": 5,
    "OPGE": 5,
    "OPLT": 5,
    "OPLE": 5,
    "OPCAT": 6,
    "OPADD": 7,
    "OPSUB": 7,
    "OPMUL": 8,
    "OPDIV": 8,
    "OPMOD": 8,
}

_TERMINATORS = {"EOF", "RPAREN", "RBRACKET", "RBRACE", "COMMA", "COLON"}

class Parser:
    def __init__(self, text: str):
        self.text = text
        self.tokens = Tokenizer(text).tokenize()
        self.pos = 0

    # ------------------------------------------------------------- token utils
    @property
    def current(self) -> Token:
        return self.tokens[self.pos]

    def peek(self, offset: int = 1) -> Token:
        index = min(self.pos + offset, len(self.tokens) - 1)
        return self.tokens[index]

    def advance(self) -> Token:
        token = self.tokens[self.pos]
        if token.kind != "EOF":
            self.pos += 1
        return token

    def expect(self, kind: str) -> Token:
        token = self.current
        if token.kind != kind:
            raise ParseError(
                f"mismatched input '{token.text}' expecting {kind} at position {token.start}"
            )
        return self.advance()

    # ---------------------------------------------------------------- entry
    def parse(self) -> AstNode:
        node = self.parse_expr(0)
        if self.current.kind != "EOF":
            token = self.current
            raise ParseError(f"extraneous input '{token.text}' at position {token.start}")
        return node

    # ------------------------------------------------------------ expressions
    def parse_expr(self, min_precedence: int) -> AstNode:
        left = self.parse_prefix()

        while True:
            token = self.current
            kind = token.kind

            # ternary is right associative and has the lowest precedence
            if kind == "QUESTION" and min_precedence <= 1:
                self.advance()
                when_true = self.parse_expr(0)
                self.expect("COLON")
                when_false = self.parse_expr(1)
                left = AstNode("ternary", left.start, when_false.end, children=[left, when_true, when_false])
                continue

            # postfix percentage: only when the '%' is not followed by an operand
            if kind == "OPMOD" and min_precedence <= 9 and not self._starts_operand(self.peek()):
                self.advance()
                left = AstNode("percent", left.start, token.end, children=[left])
                continue

            precedence = _PRECEDENCE.get(kind)
            if precedence is None or precedence < min_precedence:
                break
            self.advance()
            right = self.parse_expr(precedence + 1)
            left = AstNode("binary", left.start, right.end, op=kind, children=[left, right])

        return left

    @staticmethod
    def _starts_operand(token: Token) -> bool:
        if token.kind in ("NUM", "STRING", "PARAMETER", "LPAREN", "LBRACKET", "LBRACE", "NOT"):
            return True
        if token.kind in ("OPADD", "OPSUB"):
            return True
        if token.kind in KEYWORDS.values() or token.kind in ("TRUE", "FALSE", "NULL"):
            return True
        return False

    def parse_prefix(self) -> AstNode:
        token = self.current

        if token.kind == "NOT":
            self.advance()
            operand = self.parse_expr(9)
            return AstNode("unary", token.start, operand.end, children=[operand])

        if token.kind == "OPSUB" and self.peek().kind == "NUM":
            sign = self.advance()
            number = self.advance()
            return self._number_node(sign.start, number)

        node = self.parse_primary()
        return self.parse_postfix(node)

    def parse_primary(self) -> AstNode:
        token = self.current

        if token.kind == "NUM":
            self.advance()
            return self._number_node(token.start, token)

        if token.kind == "STRING":
            self.advance()
            return AstNode("string", token.start, token.end, value=_unescape(token.text))

        if token.kind == "LPAREN":
            self.advance()
            inner = self.parse_expr(0)
            close = self.expect("RPAREN")
            return AstNode("bracket", token.start, close.end, children=[inner])

        if token.kind == "LBRACKET":
            self.advance()
            items: List[AstNode] = []
            if self.current.kind != "RBRACKET":
                items.append(self.parse_expr(0))
                while self.current.kind == "COMMA":
                    self.advance()
                    if self.current.kind == "RBRACKET":
                        break
                    items.append(self.parse_expr(0))
            close = self.expect("RBRACKET")
            return AstNode("array", token.start, close.end, children=items)

        if token.kind == "LBRACE":
            self.advance()
            items: List[AstNode] = []
            while self.current.kind != "RBRACE":
                items.append(self.parse_array_json_item())
                if self.current.kind == "COMMA":
                    self.advance()
                    continue
                break
            close = self.expect("RBRACE")
            return AstNode("arrayjson", token.start, close.end, children=items)

        if token.kind == "PARAMETER":
            # a bare identifier is a parameter reference; a following '(' makes it a diy function
            self.advance()
            if self.current.kind == "LPAREN":
                args, close = self._parse_arguments()
                return AstNode("call", token.start, close.end, text=token.text, keyword="PARAMETER", children=args)
            return AstNode("param", token.start, token.end, text=token.text)

        if token.kind in ("TRUE", "FALSE", "NULL", "ALGORITHMVERSION"):
            self.advance()
            if self.current.kind == "LPAREN" and self.peek().kind == "RPAREN":
                self.advance()
                self.advance()
            return AstNode("const2", token.start, token.end, keyword=token.kind)

        if token.kind in ("E", "PI", "RAND", "GUID", "NOW", "TODAY"):
            self.advance()
            if self.current.kind == "LPAREN" and self.peek().kind == "RPAREN":
                self.advance()
                self.advance()
                return AstNode("const", token.start, self.tokens[self.pos - 1].end, keyword=token.kind)
            # ``E`` / ``PI`` used as bare identifiers are not valid constants, treat as diy function
            return AstNode("call", token.start, token.end, text=token.text, keyword=token.kind, children=[])

        if token.kind in KEYWORDS.values():
            keyword = token.kind
            self.advance()
            if self.current.kind == "LPAREN":
                args, close = self._parse_arguments()
                start = token.start
                return AstNode("call", start, close.end, text=token.text, keyword=keyword, children=args)
            # constant-style keywords without parentheses
            if keyword in ("TRUE", "FALSE", "NULL", "ALGORITHMVERSION"):
                return AstNode("const2", token.start, token.end, keyword=keyword)
            if keyword in ("E", "PI", "RAND", "GUID", "NOW", "TODAY"):
                return AstNode("const", token.start, token.end, keyword=keyword)
            raise ParseError(f"mismatched input '{token.text}' expecting '(' at position {token.start}")

        raise ParseError(f"mismatched input '{token.text or '<EOF>'}' at position {token.start}")

    def parse_postfix(self, node: AstNode) -> AstNode:
        while True:
            token = self.current
            if token.kind == "DOT":
                self.advance()
                name_token = self.current
                if name_token.kind != "PARAMETER" and name_token.kind not in KEYWORDS.values():
                    raise ParseError(f"unknown property '{name_token.text}' at position {name_token.start}")
                self.advance()
                if self.current.kind == "LPAREN":
                    args, close = self._parse_arguments()
                    keyword = "PARAMETER" if name_token.kind == "PARAMETER" else name_token.kind
                    node = AstNode(
                        "method",
                        node.start,
                        close.end,
                        text=name_token.text,
                        keyword=keyword,
                        children=[node] + args,
                    )
                else:
                    node = AstNode(
                        "dotkey", node.start, name_token.end, text=name_token.text, children=[node]
                    )
                continue
            if token.kind == "LBRACKET":
                self.advance()
                key_is_parameter = self.current.kind == "PARAMETER" and self.peek().kind == "RBRACKET"
                key = self.parse_expr(0)
                close = self.expect("RBRACKET")
                node = AstNode(
                    "index",
                    node.start,
                    close.end,
                    children=[node, key],
                    key_is_parameter=key_is_parameter,
                )
                continue
            break
        return node

    def _number_node(self, start: int, token: Token) -> AstNode:
        text = token.text if start == token.start else self.text[start:token.end + 1]
        try:
            return AstNode("num", start, token.end, value=Decimal(text))
        except (InvalidOperation, ValueError):
            pass
        body, unit = _split_number_unit(text)
        try:
            value = Decimal(body)
        except (InvalidOperation, ValueError) as exc:
            raise ParseError(f"invalid number '{text}'") from exc
        return AstNode("num", start, token.end, value=(value, unit))

    def _parse_arguments(self):
        self.expect("LPAREN")
        args: List[AstNode] = []
        if self.current.kind != "RPAREN":
            args.append(self.parse_expr(0))
            while self.current.kind == "COMMA":
                self.advance()
                args.append(self.parse_expr(0))
        close = self.expect("RPAREN")
        return args, close

    def parse_array_json_item(self) -> AstNode:
        token = self.current
        if token.kind in ("NUM", "STRING") or (token.kind == "PARAMETER" and self.peek().kind == "COLON"):
            self.advance()
            key = _unescape(token.text) if token.kind == "STRING" else token.text.strip("'\"` ")
            self.expect("COLON")
            value = self.parse_expr(0)
            return AstNode("arrayjsonitem", token.start, value.end, text=key, children=[value])
        if token.kind in KEYWORDS.values():
            self.advance()
            if self.current.kind == "COLON":
                self.expect("COLON")
                value = self.parse_expr(0)
                return AstNode("arrayjsonitem", token.start, value.end, text=token.text, children=[value])
            # keyword used as the value of a shorthand item - fall through to expression
            self.pos -= 1
        value = self.parse_expr(0)
        return AstNode("arrayjsonitem", value.start, value.end, text=None, children=[value])

def _split_number_unit(text: str):
    n = len(text)
    if n > 3 and text[n - 3] >= "A":
        return text[:n - 3], text[n - 3:]
    if n > 2 and text[n - 2] >= "A":
        return text[:n - 2], text[n - 2:]
    return text[:n - 1], text[n - 1:]

#: escape sequences accepted inside string literals (see ``visitSTRING_fun``)
_ESCAPES = {
    "n": "\n",
    "r": "\r",
    "t": "\t",
    "0": "\0",
    "v": "\v",
    "a": "\a",
    "b": "\b",
    "f": "\f",
}


def _unescape(text: str) -> str:
    body = text[1:-1]
    if "\\" not in body:
        return body
    out: List[str] = []
    i = 0
    n = len(body)
    while i < n:
        ch = body[i]
        if ch == "\\" and i + 1 < n:
            nxt = body[i + 1]
            out.append(_ESCAPES.get(nxt, nxt))
            i += 2
            continue
        out.append(ch)
        i += 1
    return "".join(out)

def parse_ast(text: str) -> AstNode:
    return Parser(text).parse()

def parse_function(text: str):
    return parse_ast(text).to_function()
