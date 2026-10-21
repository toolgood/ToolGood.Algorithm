"""Character helpers.

Ported from ``toolgood.algorithm.internals.visitors.CharUtil``.

``standard_char`` is what makes the lexer case insensitive: every character
fed to the tokenizer is normalised first (lower case -> upper case, full width
forms -> ASCII, CJK -> ``_``) while the original text is preserved in tokens.
"""
from __future__ import annotations

_FULL_WIDTH_RANGES = (
    (0x00C0, 0x00D6),
    (0x00D8, 0x00F6),
    (0x00F8, 0x00FF),
    (0x0100, 0x1FFF),
    (0x2C00, 0x2FFF),
    (0x3040, 0x318F),
    (0x3300, 0x337F),
    (0x3400, 0x3FFF),
    (0x4E00, 0x9FFF),
    (0xA000, 0xD7FF),
    (0xF900, 0xFAFF),
    (0xFF00, 0xFFF0),
)

_SPECIAL_CHARS = {
    215: "*",
    247: "/",
    8216: "'",
    8217: "'",
    8220: '"',
    8221: '"',
    12288: " ",
    12304: "[",
    12305: "]",
    12308: "(",
    12309: ")",
}


def standard_char(c: str) -> str:
    if not c:
        return c
    o = ord(c)
    if o <= 0:
        return c
    if c == "\r":
        return "\n"
    if c in ("\t", "\f"):
        return " "
    if c < "a":
        return c
    if c <= "z":
        return chr(o - 32)
    if 65280 < o < 65375:
        c = chr(o - 65248)
        o = ord(c)
        c = c.upper()
        o = ord(c)
    for start, end in _FULL_WIDTH_RANGES:
        if start <= o <= end:
            return "_"
    if o < 127:
        return c
    mapped = _SPECIAL_CHARS.get(o)
    if mapped is not None:
        return mapped
    return c.upper()


def standard_string(s: str) -> str:
    return "".join(standard_char(c) for c in s)


def _equals_once(left: str, right: str) -> bool:
    if len(left) != len(right):
        return False
    for a, b in zip(left, right):
        if a != b and standard_char(a) != standard_char(b):
            return False
    return True


def equals(left: str | None, *args: str | None) -> bool:
    """Compare ``left`` with any of ``args`` using standardised characters."""
    if left is None:
        return False
    for arg in args:
        if arg is not None and _equals_once(left, arg):
            return True
    return False


def contains(values, c: str) -> bool:
    if isinstance(values, str):
        return c in values
    return c in values


def split_formula(formula: str, split_chars) -> list[str]:
    """Split a formula on top level ``split_chars`` (used by callers/tests)."""
    result: list[str] = []
    in_square = False
    in_brace = False
    brackets = 0
    in_text = False
    text_char = "\0"
    buf: list[str] = []
    i = 0
    n = len(formula)
    while i < n:
        c = formula[i]
        if in_square:
            buf.append(c)
            if c == "]":
                in_square = False
        elif in_brace:
            buf.append(c)
            if c == "}":
                in_brace = False
        elif in_text:
            buf.append(c)
            if c == "\\":
                i += 1
                if i < n:
                    buf.append(formula[i])
            elif c == text_char:
                in_text = False
        elif c in split_chars and brackets == 0:
            result.append("".join(buf))
            result.append(c)
            buf = []
        else:
            buf.append(c)
            if c in ("'", '"', "`"):
                text_char = c
                in_text = True
            elif c == "[":
                in_square = True
            elif c == "{":
                in_brace = True
            elif c == "(":
                brackets += 1
            elif c == ")":
                brackets -= 1
        i += 1
    if buf:
        result.append("".join(buf))
    return result


def split_formula_for_and(formula: str) -> list[str]:
    """Split on ``&&`` / ``||`` at top level, keeping ``&&`` chains grouped."""
    result: list[str] = []
    in_square = False
    in_brace = False
    brackets = 0
    in_text = False
    text_char = "\0"
    buf: list[str] = []
    i = 0
    n = len(formula)
    while i < n:
        c = formula[i]
        if in_square:
            buf.append(c)
            if c == "]":
                in_square = False
        elif in_brace:
            buf.append(c)
            if c == "}":
                in_brace = False
        elif in_text:
            buf.append(c)
            if c == "\\":
                i += 1
                if i < n:
                    buf.append(formula[i])
            elif c == text_char:
                in_text = False
        elif c == "&" and brackets == 0:
            if i + 1 < n and formula[i + 1] == "&":
                i += 1
                result.append("".join(buf))
                buf = []
            else:
                result.append("".join(buf))
        elif c == "|" and brackets == 0:
            if i + 1 < n and formula[i + 1] == "|":
                i += 1
                result.append("".join(buf))
                buf = []
                buf.append("&&".join(result))
                buf.append("||")
                result = []
            else:
                result.append("".join(buf))
        else:
            buf.append(c)
            if c in ("'", '"', "`"):
                text_char = c
                in_text = True
            elif c == "[":
                in_square = True
            elif c == "{":
                in_brace = True
            elif c == "(":
                brackets += 1
            elif c == ")":
                brackets -= 1
        i += 1
    if buf:
        result.append("".join(buf))
    return result
