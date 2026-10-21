"""Statistical distribution functions (NORMDIST, TDIST, BESSELI, ...)."""
from __future__ import annotations

import math
from decimal import Decimal

from toolgood_algorithm.enums import OperandType
from toolgood_algorithm.internals.functions.function_base import Function_1, Function_2, Function_3, Function_4, _require_args
from toolgood_algorithm.math_net.excel_functions import (
    beta_dist,
    beta_inv,
    binom_dist,
    f_dist,
    f_inv,
    gamma_dist,
    gamma_inv,
    gammln,
    hypgeom_dist,
    log_inv,
    lognorm_dist,
    negbinom_dist,
    norm_dist,
    norm_inv,
    norm_s_dist,
    norm_s_inv,
    poisson,
    t_dist,
    t_inv,
    weibull,
)
from toolgood_algorithm.math_net.special_functions import bessel_i, bessel_j, bessel_k, bessel_y, erf, erfc
from toolgood_algorithm.operands.operand import Operand
from toolgood_algorithm.internals.functions.registry import register
from toolgood_algorithm.internals.functions.math_base import from_float


def _is_nan_or_inf(value: float) -> bool:
    return value != value or value in (float("inf"), float("-inf"))


def _expon_dist(x: float, rate: float, cumulative: bool) -> float:
    if cumulative:
        return 0.0 if x < 0.0 else 1.0 - math.exp(-rate * x)
    return 0.0 if x < 0.0 else rate * math.exp(-rate * x)


@register("BESSELI")
class Function_BESSELI(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "BesselI", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "BesselI"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        x = float(args1.number_value())
        n = int(float(args2.number_value()))
        try:
            result = bessel_i(n, x)
        except (ValueError, OverflowError, ZeroDivisionError):
            return self._function_error()
        # Math.exp overflows to Infinity/NaN for large x
        if _is_nan_or_inf(result):
            return self._function_error()
        return Operand.create(from_float(result))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("BESSELJ")
class Function_BESSELJ(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "BesselJ", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "BesselJ"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        x = float(args1.number_value())
        n = int(float(args2.number_value()))
        try:
            result = bessel_j(n, x)
        except (ValueError, OverflowError, ZeroDivisionError):
            return self._function_error()
        # the Miller recursion overflows to Infinity/NaN for large x
        if _is_nan_or_inf(result):
            return self._function_error()
        return Operand.create(from_float(result))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("BESSELK")
class Function_BESSELK(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "BesselK", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "BesselK"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        x = float(args1.number_value())
        n = int(float(args2.number_value()))
        if x <= 0:
            return self._parameter_error(1)
        if n < 0:
            return self._parameter_error(2)

        try:
            result = bessel_k(n, x)
        except (ValueError, OverflowError, ZeroDivisionError):
            return self._function_error()
        # tiny x overflows to Infinity/NaN
        if _is_nan_or_inf(result):
            return self._function_error()
        return Operand.create(from_float(result))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("BESSELY")
class Function_BESSELY(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "BesselY", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "BesselY"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        x = float(args1.number_value())
        n = int(float(args2.number_value()))
        if x <= 0:
            return self._parameter_error(1)
        if n < 0:
            return self._parameter_error(2)

        try:
            result = bessel_y(n, x)
        except (ValueError, OverflowError, ZeroDivisionError):
            return self._function_error()
        # tiny x overflows to Infinity/NaN
        if _is_nan_or_inf(result):
            return self._function_error()
        return Operand.create(from_float(result))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("BETADIST")
class Function_BETADIST(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "BetaDist", 3, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "BetaDist"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3

        x = args1.number_value()
        if x < 0 or x > Decimal(1):
            return self._parameter_error(1)
        alpha = args2.number_value()
        if alpha <= 0:
            return self._parameter_error(2)
        beta = args3.number_value()
        if beta <= 0:
            return self._parameter_error(3)
        return Operand.create(from_float(beta_dist(float(x), float(alpha), float(beta))))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("BETAINV")
class Function_BETAINV(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "BetaInv", 3, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "BetaInv"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3

        p = args1.number_value()
        if p < 0 or p > Decimal(1):
            return self._parameter_error(1)
        alpha = args2.number_value()
        if alpha <= 0:
            return self._parameter_error(2)
        beta = args3.number_value()
        if beta <= 0:
            return self._parameter_error(3)
        try:
            return Operand.create(from_float(beta_inv(float(p), float(alpha), float(beta))))
        except Exception:  # noqa: BLE001 - mirrors the Java catch
            return self._function_error()

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("BINOMDIST")
class Function_BINOMDIST(Function_4):
    def __init__(self, funcs):
        _require_args(funcs, "BinomDist", 4, 4)
        super().__init__(funcs)

    def name(self) -> str:
        return "BinomDist"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3
        args4 = self._get_boolean_4(engine, tp)
        if args4.is_error_or_none():
            return args4

        n2 = args2.int_value()
        if n2 < 0:
            return self._parameter_error(2)
        k = args1.int_value()
        if k < 0 or k > n2:
            return self._parameter_error(1)
        n3 = args3.number_value()
        if n3 < 0 or n3 > Decimal(1):
            return self._parameter_error(3)
        return Operand.create(from_float(binom_dist(k, n2, float(n3), args4.boolean_value())))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func4._get_parameter_types(none_engine, result, OperandType.BOOLEAN)


@register("ERF")
class Function_ERF(Function_1):
    def __init__(self, funcs):
        _require_args(funcs, "Erf", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "Erf"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        return Operand.create(from_float(erf(float(args1.number_value()))))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("ERFC")
class Function_ERFC(Function_1):
    def __init__(self, funcs):
        _require_args(funcs, "Erfc", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "Erfc"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        return Operand.create(from_float(erfc(float(args1.number_value()))))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("EXPONDIST")
class Function_EXPONDIST(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "ExpOnDist", 3, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "ExpOnDist"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        args3 = self._get_boolean_3(engine, tp)
        if args3.is_error_or_none():
            return args3

        n1 = args1.number_value()
        if n1 < 0:
            return self._parameter_error(1)
        rate = args2.number_value()
        if rate <= 0:
            return self._parameter_error(2)
        result = _expon_dist(float(n1), float(rate), args3.boolean_value())
        return Operand.create(from_float(result))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.BOOLEAN)


@register("FDIST")
class Function_FDIST(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "FDist", 3, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "FDist"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3

        x = args1.number_value()
        if x <= 0:
            return self._parameter_error(1)
        degrees_freedom = args2.int_value()
        if degrees_freedom <= 0:
            return self._parameter_error(2)
        degrees_freedom2 = args3.int_value()
        if degrees_freedom2 <= 0:
            return self._parameter_error(3)
        return Operand.create(from_float(f_dist(float(x), degrees_freedom, degrees_freedom2)))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("FINV")
class Function_FINV(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "FInv", 3, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "FInv"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3

        p = args1.number_value()
        if p <= 0 or p >= Decimal(1):
            return self._parameter_error(1)
        degrees_freedom = args2.int_value()
        if degrees_freedom <= 0:
            return self._parameter_error(2)
        degrees_freedom2 = args3.int_value()
        if degrees_freedom2 <= 0:
            return self._parameter_error(3)
        try:
            return Operand.create(from_float(f_inv(float(p), degrees_freedom, degrees_freedom2)))
        except Exception:  # noqa: BLE001 - mirrors the Java catch
            return self._function_error()

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("FISHER")
class Function_FISHER(Function_1):
    def __init__(self, funcs):
        _require_args(funcs, "Fisher", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "Fisher"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1

        x = args1.number_value()
        if x >= Decimal(1) or x <= Decimal(-1):
            return self._parameter_error(1)
        xv = float(x)
        return Operand.create(from_float(0.5 * math.log((1 + xv) / (1 - xv))))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("FISHERINV")
class Function_FISHERINV(Function_1):
    def __init__(self, funcs):
        _require_args(funcs, "FisherInv", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "FisherInv"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1

        x = float(args1.number_value())
        try:
            exp2x = math.exp(2 * x)
        except OverflowError:
            exp2x = float("inf")
        return Operand.create(from_float((exp2x - 1) / (exp2x + 1)))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("GAMMADIST")
class Function_GAMMADIST(Function_4):
    def __init__(self, funcs):
        _require_args(funcs, "GammaDist", 4, 4)
        super().__init__(funcs)

    def name(self) -> str:
        return "GammaDist"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3
        args4 = self._get_boolean_4(engine, tp)
        if args4.is_error_or_none():
            return args4

        x = args1.number_value()
        if x < 0:
            return self._parameter_error(1)
        alpha = args2.number_value()
        if alpha <= 0:
            return self._parameter_error(2)
        beta = args3.number_value()
        if beta <= 0:
            return self._parameter_error(3)
        return Operand.create(
            from_float(gamma_dist(float(x), float(alpha), float(beta), args4.boolean_value()))
        )

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func4._get_parameter_types(none_engine, result, OperandType.BOOLEAN)


@register("GAMMAINV")
class Function_GAMMAINV(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "GammaInv", 3, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "GammaInv"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3

        probability = args1.number_value()
        if probability < 0 or probability > Decimal(1):
            return self._parameter_error(1)
        alpha = args2.number_value()
        if alpha <= 0:
            return self._parameter_error(2)
        beta = args3.number_value()
        if beta <= 0:
            return self._parameter_error(3)
        return Operand.create(
            from_float(gamma_inv(float(probability), float(alpha), float(beta)))
        )

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("GAMMALN")
class Function_GAMMALN(Function_1):
    def __init__(self, funcs):
        _require_args(funcs, "GammaLn", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "GammaLn"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1

        value = args1.number_value()
        if value <= 0:
            return self._parameter_error(1)
        return Operand.create(from_float(gammln(float(value))))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("HYPGEOMDIST")
class Function_HYPGEOMDIST(Function_4):
    def __init__(self, funcs):
        _require_args(funcs, "HypgeomDist", 4, 4)
        super().__init__(funcs)

    def name(self) -> str:
        return "HypgeomDist"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3
        args4 = self._get_number_4(engine, tp)
        if args4.is_error_or_none():
            return args4

        k = args1.int_value()
        if k < 0:
            return self._parameter_error(1)
        draws = args2.int_value()
        if draws < 0:
            return self._parameter_error(2)
        success = args3.int_value()
        if success < 0:
            return self._parameter_error(3)
        population = args4.int_value()
        if population < 0:
            return self._parameter_error(4)
        if k > draws:
            return self._parameter_error(1)
        if success > population:
            return self._parameter_error(3)
        if draws > population:
            return self._parameter_error(2)
        return Operand.create(from_float(hypgeom_dist(k, draws, success, population)))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func4._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("LOGINV")
class Function_LOGINV(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "LogInv", 3, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "LogInv"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1

        probability = args1.number_value()
        if probability <= 0 or probability >= Decimal(1):
            return self._parameter_error(1)

        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3

        n3 = args3.number_value()
        if n3 <= 0:
            return self._parameter_error(3)
        return Operand.create(
            from_float(log_inv(float(probability), float(args2.number_value()), float(n3)))
        )

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("LOGNORMDIST")
class Function_LOGNORMDIST(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "LogNormDist", 3, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "LogNormDist"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1

        x = args1.number_value()
        if x <= 0:
            return self._parameter_error(1)

        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3

        n3 = args3.number_value()
        if n3 <= 0:
            return self._parameter_error(3)
        return Operand.create(
            from_float(lognorm_dist(float(x), float(args2.number_value()), float(n3)))
        )

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("NEGBINOMDIST")
class Function_NEGBINOMDIST(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "NegBinomDist", 3, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "NegBinomDist"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3

        k = args1.int_value()
        if k < 0:
            return self._parameter_error(1)
        r = args2.number_value()
        if r <= 0:
            return self._parameter_error(2)
        p = args3.number_value()
        if p < 0 or p > Decimal(1):
            return self._parameter_error(3)
        return Operand.create(from_float(negbinom_dist(k, float(r), float(p))))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("NORMDIST")
class Function_NORMDIST(Function_4):
    def __init__(self, funcs):
        _require_args(funcs, "NormDist", 4, 4)
        super().__init__(funcs)

    def name(self) -> str:
        return "NormDist"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3
        args4 = self._get_boolean_4(engine, tp)
        if args4.is_error_or_none():
            return args4

        num = args1.number_value()
        avg = args2.number_value()
        stdev = args3.number_value()
        if stdev <= 0:
            return self._parameter_error(3)
        return Operand.create(
            from_float(norm_dist(float(num), float(avg), float(stdev), args4.boolean_value()))
        )

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func4._get_parameter_types(none_engine, result, OperandType.BOOLEAN)


@register("NORMINV")
class Function_NORMINV(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "NormInv", 3, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "NormInv"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3

        p = args1.number_value()
        if p <= 0 or p >= Decimal(1):
            return self._parameter_error(1)
        avg = args2.number_value()
        stdev = args3.number_value()
        if stdev <= 0:
            return self._parameter_error(3)
        return Operand.create(from_float(norm_inv(float(p), float(avg), float(stdev))))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("NORMSDIST")
class Function_NORMSDIST(Function_1):
    def __init__(self, funcs):
        _require_args(funcs, "NormSDist", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "NormSDist"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        return Operand.create(from_float(norm_s_dist(float(args1.number_value()))))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("NORMSINV")
class Function_NORMSINV(Function_1):
    def __init__(self, funcs):
        _require_args(funcs, "NormSInv", 1, 1)
        super().__init__(funcs)

    def name(self) -> str:
        return "NormSInv"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1

        p = args1.number_value()
        if p <= 0 or p >= Decimal(1):
            return self._parameter_error(1)
        return Operand.create(from_float(norm_s_inv(float(p))))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("TDIST")
class Function_TDIST(Function_3):
    def __init__(self, funcs):
        _require_args(funcs, "TDist", 3, 3)
        super().__init__(funcs)

    def name(self) -> str:
        return "TDist"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3

        degrees_freedom = args2.int_value()
        if degrees_freedom <= 0:
            return self._parameter_error(2)
        tails = args3.int_value()
        if tails < 1 or tails > 2:
            return self._parameter_error(3)
        if args1.number_value() <= 0:
            return self._parameter_error(1)
        try:
            return Operand.create(from_float(t_dist(float(args1.number_value()), degrees_freedom, tails)))
        except Exception:  # noqa: BLE001 - mirrors the Java catch
            return self._function_error()

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("TINV")
class Function_TINV(Function_2):
    def __init__(self, funcs):
        _require_args(funcs, "TInv", 2, 2)
        super().__init__(funcs)

    def name(self) -> str:
        return "TInv"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2

        p = args1.number_value()
        if p <= 0 or p >= Decimal(1):
            return self._parameter_error(1)
        degrees_freedom = args2.int_value()
        if degrees_freedom <= 0:
            return self._parameter_error(2)
        try:
            return Operand.create(from_float(t_inv(float(p), degrees_freedom)))
        except Exception:  # noqa: BLE001 - mirrors the Java catch
            return self._function_error()

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)


@register("WEIBULL")
class Function_WEIBULL(Function_4):
    def __init__(self, funcs):
        _require_args(funcs, "Weibull", 4, 4)
        super().__init__(funcs)

    def name(self) -> str:
        return "Weibull"

    def evaluate(self, engine, tp) -> Operand:
        args1 = self._get_number_1(engine, tp)
        if args1.is_error_or_none():
            return args1
        args2 = self._get_number_2(engine, tp)
        if args2.is_error_or_none():
            return args2
        args3 = self._get_number_3(engine, tp)
        if args3.is_error_or_none():
            return args3
        args4 = self._get_boolean_4(engine, tp)
        if args4.is_error_or_none():
            return args4

        x = args1.number_value()
        shape = args2.number_value()
        if shape <= 0:
            return self._parameter_error(2)
        scale = args3.number_value()
        if scale <= 0:
            return self._parameter_error(3)

        try:
            result = weibull(float(x), float(shape), float(scale), args4.boolean_value())
        except (ValueError, OverflowError, ZeroDivisionError):
            return self._function_error()
        # overflowing Pow/Exp yields Infinity, 0^negative yields NaN
        if _is_nan_or_inf(result):
            return self._function_error()
        return Operand.create(from_float(result))

    def get_result_type(self) -> OperandType:
        return OperandType.NUMBER

    def _get_parameter_types(self, none_engine, result, operand_type, op=None, val=None) -> None:
        self.func1._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func2._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func3._get_parameter_types(none_engine, result, OperandType.NUMBER)
        self.func4._get_parameter_types(none_engine, result, OperandType.BOOLEAN)
