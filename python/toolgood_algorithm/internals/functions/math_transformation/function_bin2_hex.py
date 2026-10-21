"""Function_BIN2HEX (C#: Internals/Functions/MathTransformation/Function_BIN2HEX.cs)."""
from __future__ import annotations
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _BIN_HALF, _BIN_MODULUS, _FromBase, _HEX_MODULUS, _MAX_PLACES, _is_bin, _pad, _parse_complement


@register("BIN2HEX")
class Function_BIN2HEX(_FromBase):
    def name(self) -> str:
        return "Bin2Hex"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        if not _is_bin(args1.text_value()):
            return self._parameter_error(1)
        text = args1.text_value()
        if len(text) > _MAX_PLACES:
            return self._parameter_error(1)
        # 10 位二进制补码解析
        num = _parse_complement(text, 2, _BIN_HALF, _BIN_MODULUS)
        if num < 0:
            # 负数:10 位十六进制补码
            hex_text = _pad(format(num + _HEX_MODULUS, "X"), _MAX_PLACES)
        else:
            hex_text = format(num, "X")
        return self._apply_places(hex_text, engine, tp)
