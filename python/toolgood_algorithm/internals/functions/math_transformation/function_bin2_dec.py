"""Function_BIN2DEC (C#: Internals/Functions/MathTransformation/Function_BIN2DEC.cs)."""
from __future__ import annotations
from decimal import Decimal
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import _BIN_HALF, _BIN_MODULUS, _FromBase, _MAX_PLACES, _format_decimal, _is_bin, _parse_complement


@register("BIN2DEC")
class Function_BIN2DEC(_FromBase):
    def name(self) -> str:
        return "Bin2Dec"

    def get_result_type(self) -> OperandType:
        if self.func2 is not None:
            return OperandType.TEXT
        return OperandType.NUMBER

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_text_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        if not _is_bin(args1.text_value()):
            return self._parameter_error(1)
        text = args1.text_value()
        if len(text) > _MAX_PLACES:
            return self._parameter_error(1)
        # 10 位二进制补码解析:最高位为 1 时表示负数
        num = _parse_complement(text, 2, _BIN_HALF, _BIN_MODULUS)
        if self.func2 is not None:
            args2 = self._get_number_2(engine, tp)
            if args2.is_error_or_none():
                return args2
            places = args2.int_value()
            if places < 0:
                return self._parameter_error(2)
            n = str(num)
            if len(n) <= places:
                return Operand.create(_format_decimal(num, places))
            return self._parameter_error(2)
        return Operand.create(Decimal(num))
