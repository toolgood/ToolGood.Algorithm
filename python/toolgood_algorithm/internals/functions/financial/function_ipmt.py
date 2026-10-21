"""Function_IPMT (C#: Internals/Functions/Financial/Function_IPMT.cs)."""
from __future__ import annotations
from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import _require_args
from toolgood_algorithm.internals.functions.function_6 import Function_6
from toolgood_algorithm.internals.numeric import ONE, ZERO
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register

from ._common import calculate_ipmt


@register("IPMT")
class Function_IPMT(Function_6):
    def __init__(self, funcs):
        _require_args(funcs, "IPMT", 4, 6)
        super().__init__(funcs)

    def name(self) -> str:
        return "IPMT"

    def evaluate(self, engine, tp) -> Operand:
        rate_arg = self._get_number_1(engine, tp)
        if rate_arg.is_error_or_none():
            return rate_arg
        rate = rate_arg.number_value()

        per_arg = self._get_number_2(engine, tp)
        if per_arg.is_error_or_none():
            return per_arg
        per = per_arg.number_value()

        nper_arg = self._get_number_3(engine, tp)
        if nper_arg.is_error_or_none():
            return nper_arg
        nper = nper_arg.number_value()

        pv_arg = self._get_number_4(engine, tp)
        if pv_arg.is_error_or_none():
            return pv_arg
        pv = pv_arg.number_value()

        fv = ZERO
        if self.func5 is not None:
            fv_arg = self._get_number_5(engine, tp)
            if fv_arg.is_error_or_none():
                return fv_arg
            fv = fv_arg.number_value()

        type_ = 0
        if self.func6 is not None:
            type_arg = self._get_number_6(engine, tp)
            if type_arg.is_error_or_none():
                return type_arg
            type_ = type_arg.int_value()
            if type_ != 0 and type_ != 1:
                return self._parameter_error(6)

        if per < ONE or per > nper:
            return self._parameter_error(2)

        if rate == ZERO:
            return Operand.create(ZERO)

        return Operand.create(calculate_ipmt(rate, per, nper, pv, fv, type_))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func4._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func5 is not None:
            self.func5._get_parameter_types(none_engine, result, OperandType.NUMBER)
        if self.func6 is not None:
            self.func6._get_parameter_types(none_engine, result, OperandType.NUMBER)
