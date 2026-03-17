package toolgood.algorithm.internals.functions.mathsum2;

import java.math.BigDecimal;
import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.mathNet.ExcelFunctions;

public final class Function_TINV extends Function_2 {
    public Function_TINV(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    public Function_TINV(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "TInv";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }
        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) {
            return args2;
        }

        double probability = args1.DoubleValue();
        int degreesFreedom = args2.IntValue();

        if (degreesFreedom < 1 || probability <= 0.0 || probability > 1.0) {
            return Operand.Error("Function '{0}' parameter is error!", "TInv");
        }
        try {
            return Operand.Create(ExcelFunctions.TInv(probability, degreesFreedom));
        } catch (Exception e) {
            return Operand.Error("Function '{0}' parameter is error!", "TInv");
        }
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
