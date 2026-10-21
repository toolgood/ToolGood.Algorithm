"""Function_HEX2OCT (C#: Internals/Functions/MathTransformation/Function_HEX2OCT.cs)."""
from __future__ import annotations
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _FromBase, _HEX_HALF, _HEX_MODULUS, _MAX_PLACES, _OCT_MASK, _is_hex, _pad, _parse_complement


@register("HEX2OCT")
class Function_HEX2OCT(_FromBase):
    def name(self) -> str:
        return "Hex2Oct"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        if not _is_hex(args1.text_value()):
            return self._parameter_error(1)
        text = args1.text_value()
        if len(text) > _MAX_PLACES:
            return self._parameter_error(1)
        # 10 位十六进制补码解析
        num = _parse_complement(text, 16, _HEX_HALF, _HEX_MODULUS)
        # Excel HEX2OCT 结果范围为 -536870912~536870911
        if num < -536870912 or num > 536870911:
            return self._parameter_error(1)
        if num < 0:
            # 负数:10 位八进制补码
            oct_text = _pad(format(num & _OCT_MASK, "o"), _MAX_PLACES)
        else:
            oct_text = format(num, "o")
        return self._apply_places(oct_text, engine, tp)
