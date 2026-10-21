"""csharp_web functions (C#: Internals/Functions/csharp_web/).

One module per function class; ``_common.py`` holds shared helpers and bases.
"""
from __future__ import annotations

from ._common import *  # noqa: F401,F403  (re-export shared helpers)
from ._common import _HEX, _HTML_ENTITIES, _decode_entity, _html_decode, _html_encode, _url_encode  # noqa: F401

from .function_base64_totext import Function_BASE64TOTEXT
from .function_base64_urltotext import Function_BASE64URLTOTEXT
from .function_htmldecode import Function_HTMLDECODE
from .function_htmlencode import Function_HTMLENCODE
from .function_texttobase64 import Function_TEXTTOBASE64
from .function_texttobase64_url import Function_TEXTTOBASE64URL
from .function_urldecode import Function_URLDECODE
from .function_urlencode import Function_URLENCODE

__all__ = [
    "Function_BASE64TOTEXT",
    "Function_BASE64URLTOTEXT",
    "Function_HTMLDECODE",
    "Function_HTMLENCODE",
    "Function_TEXTTOBASE64",
    "Function_TEXTTOBASE64URL",
    "Function_URLDECODE",
    "Function_URLENCODE",
]
