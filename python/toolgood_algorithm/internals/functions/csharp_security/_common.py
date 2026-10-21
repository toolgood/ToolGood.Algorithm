"""csharp_security: shared helpers and base classes (kept out of the per-class files)."""
from __future__ import annotations
import hashlib
import hmac


def digest(algorithm: str, text: str) -> str:
    return hashlib.new(algorithm, text.encode("utf-8")).hexdigest().upper()


def hmac_digest(algorithm: str, text: str, secret: str) -> str:
    key = (secret or "").encode("utf-8")
    return hmac.new(key, text.encode("utf-8"), algorithm).hexdigest().upper()
