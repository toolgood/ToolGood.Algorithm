"""csharp: shared helpers and base classes (kept out of the per-class files)."""
from __future__ import annotations
import re


def _json_item_matches(value, text: str) -> bool:
    """Compare a ``JsonData`` leaf with the text argument (``Has`` / ``HasValue``)."""
    if value.is_string():
        return value.string_value() == text
    if value.is_double():
        return str(value.number_value()) == text
    if value.is_boolean():
        return ("true" if value.boolean_value() else "false") == text.lower()
    return False


def _trim_start(text: str) -> str:
    start = 0
    while start < len(text) and text[start].isspace():
        start += 1
    return text[start:]


def _trim_end(text: str) -> str:
    end = len(text)
    while end > 0 and text[end - 1].isspace():
        end -= 1
    return text[:end]


def _to_unicode_replacement(text: str) -> str:
    """Translate Java/.NET ``$1`` group references into Python ``\\g<1>``."""
    return re.sub(r"\$(\d+)", r"\\g<\1>", text)
