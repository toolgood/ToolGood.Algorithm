package toolgood.algorithm.internals.functions.mathSum2;

import java.math.BigDecimal;
import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_1;

final class Function_ERFC extends Function_1 {

    public Function_ERFC(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 1) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 1 parameter.");
        }
    }

    @Override
    public String Name() {
        return "Erfc";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }
        double x = args1.NumberValue().doubleValue();
        return Operand.Create(BigDecimal.valueOf(Erfc(x)));
    }

    private static double Erfc(double x) {
        if (x < 0) {
            return 2.0 - Erfc(-x);
        }
        if (x > 6.0) {
            return 0.0;
        }
        return 1.0 - Erf(x);
    }

    private static double Erf(double x) {
        final double a1 = 0.254829592;
        final double a2 = -0.284496736;
        final double a3 = 1.421413741;
        final double a4 = -1.453152027;
        final double a5 = 1.061405429;
        final double p = 0.3275911;

        double t = 1.0 / (1.0 + p * x);
        double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.exp(-x * x);

        return y;
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
