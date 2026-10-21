"""Tokenizer + keyword table for the formula language (``mathLexer``).

Characters are normalised with
:func:`toolgood_algorithm.internals.visitors.char_util.standard_char` so the
language is case insensitive and accepts full-width punctuation.  Keywords are
matched with ANTLR's rule: longest match wins, keyword beats identifier on
equal length.
"""
from __future__ import annotations

import re
from dataclasses import dataclass
from typing import List

from toolgood_algorithm.internals.visitors.char_util import standard_char, standard_string


_KEYWORD_SPELLINGS = {
    # constants
    "ALGORITHMVERSION": ("ALGORITHMVERSION", "ENGINEVERSION"),
    "FALSE": ("FALSE", "NO"),
    "TRUE": ("TRUE", "YES"),
    "NULL": ("NULL",),
    "E": ("E",),
    "GUID": ("GUID",),
    "NOW": ("NOW",),
    "PI": ("PI",),
    "RAND": ("RAND",),
    "TODAY": ("TODAY",),
    "BETADIST": ("BETADIST", "BETA.DIST"),
    "BETAINV": ("BETAINV", "BETA.INV"),
    "BINOMDIST": ("BINOMDIST", "BINOM.DIST"),
    "CEILING": ("CEIL", "CEILING"),
    "CONCATENATE": ("CONCAT", "CONCATENATE"),
    "COVAR": ("COVAR", "COVARIANCE.P"),
    "COVARIANCES": ("COVARIANCE.S",),
    "EXPONDIST": ("EXPONDIST", "EXPON.DIST"),
    "FDIST": ("FDIST", "F.DIST"),
    "FINV": ("FINV", "F.INV"),
    "GAMMADIST": ("GAMMADIST", "GAMMA.DIST"),
    "GAMMAINV": ("GAMMAINV", "GAMMA.INV"),
    "GAMMALN": ("GAMMALN", "GAMMALN.PRECISE"),
    "HAS": ("HAS", "HASKEY", "CONTAINS", "CONTAINSKEY"),
    "HASVALUE": ("HASVALUE", "CONTAINSVALUE"),
    "HYPGEOMDIST": ("HYPGEOMDIST", "HYPGEOM.DIST"),
    "ISREGEX": ("ISREGEX", "ISMATCH"),
    "JIS": ("JIS", "WIDECHAR"),
    "LOGINV": ("LOGINV", "LOGNORM.INV"),
    "LOGNORMDIST": ("LOGNORMDIST", "LOGNORM.DIST"),
    "LOOKCEILING": ("LOOKCEIL", "LOOKCEILING"),
    "LOWER": ("LOWER", "TOLOWER"),
    "NEGBINOMDIST": ("NEGBINOMDIST", "NEGBINOM.DIST"),
    "NORMDIST": ("NORMDIST", "NORM.DIST"),
    "NORMINV": ("NORMINV", "NORM.INV"),
    "NORMSDIST": ("NORMSDIST", "NORM.S.DIST"),
    "NORMSINV": ("NORMSINV", "NORM.S.INV"),
    "PARAM": ("PARAM", "PARAMETER", "GETPARAMETER"),
    "PERCENTILE": ("PERCENTILE", "PERCENTILE.INC"),
    "PERCENTRANK": ("PERCENTRANK", "PERCENTRANK.INC"),
    "POISSON": ("POISSON", "POISSON.DIST"),
    "STDEV": ("STDEV", "STDEV.S"),
    "STDEVP": ("STDEVP", "STDEV.P"),
    "TDIST": ("TDIST", "T.DIST"),
    "TINV": ("TINV", "T.INV"),
    "TRIMEND": ("TRIMEND", "RTRIM"),
    "TRIMSTART": ("TRIMSTART", "LTRIM"),
    "UPPER": ("UPPER", "TOUPPER"),
    "VAR": ("VAR", "VAR.S"),
    "VARP": ("VARP", "VAR.P"),
}
#: upper-case spelling -> canonical token name
KEYWORDS: dict[str, str] = {}

# spelling variants first (HAS / HASKEY / CONTAINS ...), canonical name wins on ties
for _canonical, _spellings in _KEYWORD_SPELLINGS.items():
    for _spelling in _spellings:
        KEYWORDS[_spelling] = _canonical

#: every other keyword is its own spelling (ABS, ACOS, ...)
for _line in (
    "ABS ACOS ACOSH ACOT ACOTH ADDDAYS ADDHOURS ADDMINUTES ADDMONTHS ADDSECONDS ADDYEARS "
    "AND ARABIC ARRAY ASC ASIN ASINH ATAN ATAN2 ATANH AVEDEV AVERAGE AVERAGEIF "
    "BASE64TOTEXT BASE64URLTOTEXT BESSELI BESSELJ BESSELK BESSELY "
    "BIN2DEC BIN2HEX BIN2OCT CEILING CHAR CLEAN CODE COMBIN CORREL COS COSH COT COTH "
    "COUNT COUNTIF CSC CSCH DATE DATEDIF DATEVALUE DAY DAYS DAYS360 DB DDB DEC2BIN DEC2HEX "
    "DEC2OCT DEGREES DELTA DEVSQ EDATE ENDSWITH EOMONTH ERF ERFC ERROR EVEN EXACT EXP FACT "
    "FACTDOUBLE FIND FISHER FISHERINV FIXED FLOOR FORECAST FV GCD GEOMEAN GESTEP HARMEAN "
    "HEX2BIN HEX2DEC HEX2OCT HMACMD5 HMACSHA1 HMACSHA256 HMACSHA512 HOUR HTMLDECODE HTMLENCODE "
    "IF IFERROR IFS INDEXOF INT INTERCEPT IPMT IRR ISERROR ISEVEN ISLOGICAL ISNONTEXT ISNULL "
    "ISNULLOREMPTY ISNULLORERROR ISNULLORWHITESPACE ISNUMBER ISODD ISTEXT JOIN JSON LARGE "
    "LASTINDEXOF LCM LEFT LEN LN LOG LOG10 LOOKFLOOR MAX MD5 MEDIAN MID MIN MINUTE MIRR MOD "
    "MODE MONTH MROUND MULTINOMIAL NETWORKDAYS NOT NPER NPV OCT2BIN OCT2DEC OCT2HEX ODD OR "
    "PEARSON PERMUT PMT POWER PPMT PRODUCT PROPER PV QUARTILE QUOTIENT RADIANS RANDBETWEEN "
    "RANK RATE REGEX REGEXREPLACE REMOVEEND REMOVESTART REPLACE REPT RIGHT RMB ROMAN ROUND "
    "ROUNDDOWN ROUNDUP SEARCH SEC SECH SECOND SERIESSUM SHA1 SHA256 SHA512 SIGN SIN SINH SLN "
    "SLOPE SMALL SPLIT SQRT SQRTPI STARTSWITH SUBSTITUTE SUBSTRING SUM SUMIF SUMPRODUCT SUMSQ "
    "SUMX2MY2 SUMX2PY2 SUMXMY2 SWITCH SYD T TAN TANH TEXT TEXTTOBASE64 TEXTTOBASE64URL TIME "
    "TIMESTAMP TIMEVALUE TRIM TRUNC UNICHAR UNICODE URLDECODE URLENCODE VALUE WEEKDAY WEEKNUM "
    "WEIBULL WORKDAY XIRR XNPV XOR YEAR YEARFRAC"
).split():
    KEYWORDS.setdefault(_line, _line)
    _KEYWORD_SPELLINGS.setdefault(_line, (_line,))
_MAX_KEYWORD_LEN = max(len(k) for k in KEYWORDS)
_PARAMETER2_KEYWORDS = set(KEYWORDS)


class ParseError(ValueError):
    """Raised when the formula cannot be parsed."""


@dataclass
class Token:
    kind: str
    text: str
    start: int
    end: int


"""Tokenizer for the formula language (equivalent of ``mathLexer``)."""

_PUNCT = {
    "(": "LPAREN",
    ")": "RPAREN",
    "[": "LBRACKET",
    "]": "RBRACKET",
    "{": "LBRACE",
    "}": "RBRACE",
    ",": "COMMA",
    ":": "COLON",
    "?": "QUESTION",
    ".": "DOT",
    "+": "OPADD",
    "-": "OPSUB",
    "*": "OPMUL",
    "/": "OPDIV",
    "%": "OPMOD",
    "&": "OPCAT",
}

_NUM_SUFFIXES = tuple(
    re.compile(pattern) for pattern in (r"E[+-]?\d+", r"[KDCM]?M[23]?", r"M?L", r"K?G", r"T")
)

def _match_num_suffix(text: str, position: int) -> str:
    best = ""
    for pattern in _NUM_SUFFIXES:
        match = pattern.match(text, position)
        if match and len(match.group(0)) > len(best):
            best = match.group(0)
    return best

def _is_ident_start(c: str) -> bool:
    return ("A" <= c <= "Z") or c == "_"

def _is_ident_part(c: str) -> bool:
    return ("A" <= c <= "Z") or ("0" <= c <= "9") or c == "_"

class Tokenizer:
    def __init__(self, text: str):
        self.text = text
        self.std = standard_string(text)
        self.pos = 0
        self.tokens: List[Token] = []

    def tokenize(self) -> List[Token]:
        text, std = self.text, self.std
        n = len(text)
        while self.pos < n:
            i = self.pos
            c = std[i]
            if c in " \t\r\n\f":
                self.pos += 1
                continue
            if c == "/" and i + 1 < n and std[i + 1] == "*":
                end = std.find("*/", i + 2)
                if end < 0:
                    raise ParseError(f"token recognition error at: '{text[i:]}'")
                self.pos = end + 2
                continue
            if c == "/" and i + 1 < n and std[i + 1] == "/":
                end = std.find("\n", i + 2)
                self.pos = n if end < 0 else end + 1
                continue
            if c.isdigit():
                self._read_number()
                continue
            if c in ("'", '"', "`"):
                self._read_string(c)
                continue
            if _is_ident_start(c):
                self._read_word()
                continue
            self._read_operator()
        self.tokens.append(Token("EOF", "", n, n))
        return self.tokens

    # ------------------------------------------------------------------ tokens
    def _read_number(self) -> None:
        text, std = self.text, self.std
        i = self.pos
        n = len(std)
        while i < n and std[i].isdigit():
            i += 1
        if i < n and std[i] == "." and i + 1 < n and std[i + 1].isdigit():
            i += 1
            while i < n and std[i].isdigit():
                i += 1
        i += len(_match_num_suffix(std, i))
        self.tokens.append(Token("NUM", text[self.pos:i], self.pos, i - 1))
        self.pos = i

    def _read_string(self, quote: str) -> None:
        text, std = self.text, self.std
        i = self.pos + 1
        n = len(std)
        while i < n:
            if std[i] == "\\":
                i += 2
                continue
            if std[i] == quote:
                i += 1
                self.tokens.append(Token("STRING", text[self.pos:i], self.pos, i - 1))
                self.pos = i
                return
            i += 1
        raise ParseError(f"token recognition error at: '{text[self.pos:]}'")

    def _read_word(self) -> None:
        std = self.std
        i = self.pos
        n = len(std)
        while i < n and _is_ident_part(std[i]):
            i += 1
        ident_len = i - self.pos

        best_kind = None
        best_len = 0
        upper = std[self.pos:self.pos + _MAX_KEYWORD_LEN]
        for size in range(min(_MAX_KEYWORD_LEN, len(upper)), 0, -1):
            candidate = upper[:size]
            kind = KEYWORDS.get(candidate)
            if kind is not None and size > best_len:
                best_kind = kind
                best_len = size
                break

        if best_kind is not None and best_len >= ident_len:
            end = self.pos + best_len
            self.tokens.append(Token(best_kind, self.text[self.pos:end], self.pos, end - 1))
            self.pos = end
            return
        self.tokens.append(Token("PARAMETER", self.text[self.pos:i], self.pos, i - 1))
        self.pos = i

    def _read_operator(self) -> None:
        text, std = self.text, self.std
        i = self.pos
        n = len(std)
        two = std[i:i + 2]
        if two == "&&":
            self.tokens.append(Token("OPAND", text[i:i + 2], i, i + 1))
            self.pos = i + 2
            return
        if two == "||":
            self.tokens.append(Token("OPOR", text[i:i + 2], i, i + 1))
            self.pos = i + 2
            return
        if two in (">=", "<=", "!=", "<>"):
            kind = {">=": "OPGE", "<=": "OPLE", "!=": "OPNE", "<>": "OPNE"}[two]
            end = i + 2
            if two == "!=" and end < n and std[end] == "=":
                end += 1
            self.tokens.append(Token(kind, text[i:end], i, end - 1))
            self.pos = end
            return
        if two == "==":
            j = i + 2
            if j < n and std[j] == "=":
                j += 1
            self.tokens.append(Token("OPEQ", text[i:j], i, j - 1))
            self.pos = j
            return
        if std[i] == "=":
            self.tokens.append(Token("OPEQ", text[i:i + 1], i, i))
            self.pos = i + 1
            return
        if std[i] == "!":
            self.tokens.append(Token("NOT", text[i:i + 1], i, i))
            self.pos = i + 1
            return
        if std[i] in (">", "<"):
            kind = "OPGT" if std[i] == ">" else "OPLT"
            self.tokens.append(Token(kind, text[i:i + 1], i, i))
            self.pos = i + 1
            return
        punct = _PUNCT.get(std[i])
        if punct is not None:
            self.tokens.append(Token(punct, text[i:i + 1], i, i))
            self.pos = i + 1
            return
        raise ParseError(f"token recognition error at: '{text[i]}'")
