"""Function_OCT2BIN (C#: Internals/Functions/MathTransformation/Function_OCT2BIN.cs)."""
from __future__ import annotations
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _FromBase, _MAX_PLACES, _OCT_HALF, _OCT_MODULUS, _is_oct, _pad, _parse_complement


@register("OCT2BIN")
class Function_OCT2BIN(_FromBase):
    def name(self) -> str:
        return "Oct2Bin"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        if not _is_oct(args1.text_value()):
            return self._parameter_error(1)
        text = args1.text_value()
        if len(text) > _MAX_PLACES:
            return self._parameter_error(1)
        # 10 位八进制补码解析
        num = _parse_complement(text, 8, _OCT_HALF, _OCT_MODULUS)
        # Excel OCT2BIN 结果范围为 -512~511
        if num < -512 or num > 511:
            return self._parameter_error(1)
        if num < 0:
            # 负数:10 位二进制补码
            bin_text = _pad(format(num & 1023, "b"), _MAX_PLACES)
        else:
            bin_text = format(num, "b")
        return self._apply_places(bin_text, engine, tp)
