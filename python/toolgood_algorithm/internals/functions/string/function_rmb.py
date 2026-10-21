"""Function_RMB (C#: Internals/Functions/String/Function_RMB.cs)."""
from __future__ import annotations
from decimal import ROUND_HALF_UP, Decimal, localcontext
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_1 import Function_1
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register


@register("RMB")
class Function_RMB(Function_1):
    CN_UPPER_NUMBER = ("零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖")
    CN_UPPER_MONETRAY_UNIT = (
        "分", "角", "元", "拾", "佰", "仟", "万", "拾", "佰", "仟", "亿", "拾", "佰",
        "仟", "兆", "拾", "佰", "仟",
    )
    CN_FULL = "整"
    CN_NEGATIVE = "负"
    MONEY_PRECISION = 2
    CN_ZEOR_FULL = "零元" + CN_FULL

    def __init__(self, funcs):
        if not isinstance(funcs, (list, tuple)):
            funcs = [funcs]
        _require_args(funcs, "Rmb", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "Rmb"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        return Operand.create(self._to_chinese_rmb(args1.number_value()))

    def _to_chinese_rmb(self, number_of_money: Decimal) -> str:
        signum = 1 if number_of_money > 0 else (-1 if number_of_money < 0 else 0)
        if signum == 0:
            return self.CN_ZEOR_FULL
        with localcontext() as ctx:
            ctx.prec = 60
            scaled = (number_of_money * 100).quantize(Decimal(1), rounding=ROUND_HALF_UP)
        number = abs(int(scaled))
        sb: list[str] = []
        scale = number % 100
        num_index = 0
        get_zero = False
        if not scale > 0:
            num_index = 2
            number = number // 100
            get_zero = True
        if scale > 0 and not scale % 10 > 0:
            num_index = 1
            number = number // 10
            get_zero = True
        zero_size = 0
        while True:
            if number <= 0:
                break
            num_unit = number % 10
            if num_unit > 0:
                if num_index == 9 and zero_size >= 3:
                    sb.insert(0, self.CN_UPPER_MONETRAY_UNIT[6])
                if num_index == 13 and zero_size >= 3:
                    sb.insert(0, self.CN_UPPER_MONETRAY_UNIT[10])
                sb.insert(0, self.CN_UPPER_MONETRAY_UNIT[num_index])
                sb.insert(0, self.CN_UPPER_NUMBER[num_unit])
                get_zero = False
                zero_size = 0
            else:
                zero_size += 1
                if not get_zero:
                    sb.insert(0, self.CN_UPPER_NUMBER[num_unit])
                if num_index == 2:
                    if number > 0:
                        sb.insert(0, self.CN_UPPER_MONETRAY_UNIT[num_index])
                elif (num_index - 2) % 4 == 0 and number % 1000 > 0:
                    sb.insert(0, self.CN_UPPER_MONETRAY_UNIT[num_index])
                get_zero = True
            number = number // 10
            num_index += 1
        if signum == -1:
            sb.insert(0, self.CN_NEGATIVE)
        if not scale > 0:
            sb.append(self.CN_FULL)
        return "".join(sb)

    def get_result_type(self) -> OperandType:
        return OperandType.TEXT

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
