package toolgood.algorithm.internals.functions.mathSum2;

import java.math.BigDecimal;
import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.mathNet.SpecialFunctions;

public final class Function_BESSELK extends Function_2 {

    public Function_BESSELK(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 2) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 2 parameters.");
        }
    }

    @Override
    public String Name() {
        return "BesselK";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }
        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) {
            return args2;
        }

        double x = args1.NumberValue().doubleValue();
        int n = (int) args2.NumberValue().doubleValue();

        if (x <= 0) {
            return ParameterError(1);
        }
        if (n < 0) {
            return ParameterError(2);
        }

        // 复用 SpecialFunctions 中基于 Chebyshev 展开的实现(移植自 MathNet.Numerics)
        double result = SpecialFunctions.BesselK(n, x);
        // x 过小时结果溢出为 Infinity/NaN,返回错误
        if (Double.isNaN(result) || Double.isInfinite(result)) {
            return FunctionError();
        }
        return Operand.Create(BigDecimal.valueOf(result));
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
