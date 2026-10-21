"""flow functions (C#: Internals/Functions/flow/).

One module per function class; ``_common.py`` holds shared helpers and bases.
"""
from __future__ import annotations

from ._common import *  # noqa: F401,F403  (re-export shared helpers)
from ._common import _IsBase, _equals_operand  # noqa: F401

from .function_if import Function_IF
from .function_iferror import Function_IFERROR
from .function_ifs import Function_IFS
from .function_switch import Function_SWITCH
from .function_iserror import Function_ISERROR
from .function_isnull import Function_ISNULL
from .function_isnullorerror import Function_ISNULLORERROR
from .function_isnullorempty import Function_ISNULLOREMPTY
from .function_isnullorwhitespace import Function_ISNULLORWHITESPACE
from .function_isnumber import Function_ISNUMBER
from .function_istext import Function_ISTEXT
from .function_isnontext import Function_ISNONTEXT
from .function_islogical import Function_ISLOGICAL
from .function_iseven import Function_ISEVEN
from .function_isodd import Function_ISODD
from .function_not import Function_NOT

__all__ = [
    "Function_IF",
    "Function_IFERROR",
    "Function_IFS",
    "Function_SWITCH",
    "Function_ISERROR",
    "Function_ISNULL",
    "Function_ISNULLORERROR",
    "Function_ISNULLOREMPTY",
    "Function_ISNULLORWHITESPACE",
    "Function_ISNUMBER",
    "Function_ISTEXT",
    "Function_ISNONTEXT",
    "Function_ISLOGICAL",
    "Function_ISEVEN",
    "Function_ISODD",
    "Function_NOT",
]
