"""csharp functions (C#: Internals/Functions/csharp/).

One module per function class; ``_common.py`` holds shared helpers and bases.
"""
from __future__ import annotations

from ._common import *  # noqa: F401,F403  (re-export shared helpers)
from ._common import _json_item_matches, _to_unicode_replacement, _trim_end, _trim_start  # noqa: F401

from .function_endswith import Function_ENDSWITH
from .function_has import Function_HAS
from .function_hasvalue import Function_HASVALUE
from .function_indexof import Function_INDEXOF
from .function_isregex import Function_ISREGEX
from .function_join import Function_JOIN
from .function_lastindexof import Function_LASTINDEXOF
from .function_lookceiling import Function_LOOKCEILING
from .function_lookfloor import Function_LOOKFLOOR
from .function_regex import Function_REGEX
from .function_regexreplace import Function_REGEXREPLACE
from .function_removeend import Function_REMOVEEND
from .function_removestart import Function_REMOVESTART
from .function_split import Function_SPLIT
from .function_startswith import Function_STARTSWITH
from .function_substring import Function_SUBSTRING
from .function_trimend import Function_TRIMEND
from .function_trimstart import Function_TRIMSTART

__all__ = [
    "Function_ENDSWITH",
    "Function_HAS",
    "Function_HASVALUE",
    "Function_INDEXOF",
    "Function_ISREGEX",
    "Function_JOIN",
    "Function_LASTINDEXOF",
    "Function_LOOKCEILING",
    "Function_LOOKFLOOR",
    "Function_REGEX",
    "Function_REGEXREPLACE",
    "Function_REMOVEEND",
    "Function_REMOVESTART",
    "Function_SPLIT",
    "Function_STARTSWITH",
    "Function_SUBSTRING",
    "Function_TRIMEND",
    "Function_TRIMSTART",
]
