"""operator functions (C#: Internals/Functions/operator/).

One module per function class; ``_common.py`` holds shared helpers and bases.
"""
from __future__ import annotations

from ._common import *  # noqa: F401,F403  (re-export shared helpers)
from ._common import _Arithmetic  # noqa: F401

from .function_add import Function_Add
from .function_sub import Function_Sub
from .function_mul import Function_Mul
from .function_div import Function_Div
from .function_mod import Function_Mod
from .function_connect import Function_Connect
from .function_and import Function_AND
from .function_or import Function_OR
from .function_and_n import Function_AND_N
from .function_or_n import Function_OR_N
from .function_xor import Function_XOR

__all__ = [
    "Function_Add",
    "Function_Sub",
    "Function_Mul",
    "Function_Div",
    "Function_Mod",
    "Function_Connect",
    "Function_AND",
    "Function_OR",
    "Function_AND_N",
    "Function_OR_N",
    "Function_XOR",
]
