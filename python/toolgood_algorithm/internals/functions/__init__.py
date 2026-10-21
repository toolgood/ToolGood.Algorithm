"""Function implementations grouped like the C# ``Internals/Functions`` folders.

Importing this package registers every implementation in
``toolgood_algorithm.internals.functions.registry``.
"""
from __future__ import annotations

# --- node types used by the parser -------------------------------------------------
from . import _aliases  # noqa: F401  (CamelCase helpers, must run after the bases)
from . import value  # noqa: F401  (literals / parameters / JSON / constants)
from . import operator  # noqa: F401
from . import compare  # noqa: F401
from . import flow  # noqa: F401

# --- function families (one module per C# folder) ----------------------------------
from . import math_base  # noqa: F401           MathBase/
from . import math_trigonometric  # noqa: F401  MathTrigonometric/
from . import math_transformation  # noqa: F401 MathTransformation/
from . import math_sum  # noqa: F401            MathSum/
from . import math_sum2  # noqa: F401           MathSum2/
from . import string  # noqa: F401              String/
from . import date_times  # noqa: F401          DateTimes/
from . import financial  # noqa: F401           Financial/
from . import csharp  # noqa: F401              Csharp/
from . import csharp_security  # noqa: F401     CsharpSecurity/
from . import csharp_web  # noqa: F401          CsharpWeb/

__all__ = [
    "value",
    "operator",
    "compare",
    "flow",
    "math_base",
    "math_trigonometric",
    "math_transformation",
    "math_sum",
    "math_sum2",
    "string",
    "date_times",
    "financial",
    "csharp",
    "csharp_security",
    "csharp_web",
]
