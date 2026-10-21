"""compare functions (C#: Internals/Functions/compare/).

One module per function class; ``_common.py`` holds shared helpers and bases.
"""
from __future__ import annotations

from ._common import *  # noqa: F401,F403  (re-export shared helpers)
from ._common import _CompareBase  # noqa: F401

from .function_eq import Function_EQ
from .function_ne import Function_NE
from .function_gt import Function_GT
from .function_ge import Function_GE
from .function_lt import Function_LT
from .function_le import Function_LE

__all__ = [
    "Function_EQ",
    "Function_NE",
    "Function_GT",
    "Function_GE",
    "Function_LT",
    "Function_LE",
]
