"""financial functions (C#: Internals/Functions/financial/).

One module per function class; ``_common.py`` holds shared helpers and bases.
"""
from __future__ import annotations

from ._common import *  # noqa: F401,F403  (re-export shared helpers)
from ._common import _DAYS_IN_YEAR, _EPS_SLOPE, _EPS_STEP, _MAX_ITERATIONS, _arg_array, _arg_number, _dates_from_array, _numbers_from_array, _whole_days  # noqa: F401

from .function_fv import Function_FV
from .function_pv import Function_PV
from .function_pmt import Function_PMT
from .function_ipmt import Function_IPMT
from .function_ppmt import Function_PPMT
from .function_nper import Function_NPER
from .function_rate import Function_RATE
from .function_npv import Function_NPV
from .function_irr import Function_IRR
from .function_mirr import Function_MIRR
from .function_xirr import Function_XIRR
from .function_xnpv import Function_XNPV
from .function_sln import Function_SLN
from .function_syd import Function_SYD
from .function_db import Function_DB
from .function_ddb import Function_DDB

__all__ = [
    "Function_FV",
    "Function_PV",
    "Function_PMT",
    "Function_IPMT",
    "Function_PPMT",
    "Function_NPER",
    "Function_RATE",
    "Function_NPV",
    "Function_IRR",
    "Function_MIRR",
    "Function_XIRR",
    "Function_XNPV",
    "Function_SLN",
    "Function_SYD",
    "Function_DB",
    "Function_DDB",
]
