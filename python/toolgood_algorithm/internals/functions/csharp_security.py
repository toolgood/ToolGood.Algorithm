"""Security functions (MD5 / SHA / HMAC) in the C# flavour.

Ported from ``toolgood.algorithm.internals.functions.csharpSecurity``.  Digests
are computed on the UTF-8 bytes of the text argument and rendered as upper-case
hexadecimal, exactly like the Java port does.
"""
from __future__ import annotations

import hashlib
import hmac

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import Function_1, Function_2, _require_args
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


def digest(algorithm: str, text: str) -> str:
    return hashlib.new(algorithm, text.encode("utf-8")).hexdigest().upper()


def hmac_digest(algorithm: str, text: str, secret: str) -> str:
    key = (secret or "").encode("utf-8")
    return hmac.new(key, text.encode("utf-8"), algorithm).hexdigest().upper()


@register("HMACMD5")
class Function_HMACMD5(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "HmacMD5", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "HmacMD5"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        return Operand.create(hmac_digest("md5", args1.text_value(), args2.text_value()))

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)


@register("HMACSHA1")
class Function_HMACSHA1(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "HmacSHA1", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "HmacSHA1"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        return Operand.create(hmac_digest("sha1", args1.text_value(), args2.text_value()))

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)


@register("HMACSHA256")
class Function_HMACSHA256(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "HmacSHA256", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "HmacSHA256"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        return Operand.create(hmac_digest("sha256", args1.text_value(), args2.text_value()))

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)


@register("HMACSHA512")
class Function_HMACSHA512(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "HmacSHA512", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "HmacSHA512"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_text_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        return Operand.create(hmac_digest("sha512", args1.text_value(), args2.text_value()))

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
        self.func2._get_parameter_types(none_engine, result, OperandType.TEXT)


@register("MD5")
class Function_MD5(Function_1):
    def __init__(self, funcs):
        _require_args(funcs, "MD5", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "MD5"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        return Operand.create(digest("md5", args1.text_value()))

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)


@register("SHA1")
class Function_SHA1(Function_1):
    def __init__(self, funcs):
        _require_args(funcs, "SHA1", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "SHA1"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        return Operand.create(digest("sha1", args1.text_value()))

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)


@register("SHA256")
class Function_SHA256(Function_1):
    def __init__(self, funcs):
        _require_args(funcs, "SHA256", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "SHA256"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        return Operand.create(digest("sha256", args1.text_value()))

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)


@register("SHA512")
class Function_SHA512(Function_1):
    def __init__(self, funcs):
        _require_args(funcs, "SHA512", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "SHA512"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        return Operand.create(digest("sha512", args1.text_value()))

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.TEXT)
