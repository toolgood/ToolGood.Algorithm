package toolgood.algorithm.internals.functions.mathsum2;

import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_4;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.mathNet.ExcelFunctions;

public final class Function_BINOMDIST extends Function_4 {
    public Function_BINOMDIST(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "BinomDist";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) return args1;

        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) return args2;

        Operand args3 = GetNumber_3(engine, tempParameter);
        if (args3.IsErrorOrNone()) return args3;

        Operand args4 = GetBoolean_4(engine, tempParameter);
        if (args4.IsErrorOrNone()) return args4;

        int n2 = args2.IntValue();
        if (n2 < 0) {
            return ParameterError(2);
        }
        int k = args1.IntValue();
        if (k < 0 || k > n2) {
            return ParameterError(1);
        }
        java.math.BigDecimal n3 = args3.NumberValue();
        if (n3.compareTo(java.math.BigDecimal.ZERO) < 0 || n3.compareTo(java.math.BigDecimal.ONE) > 0) {
            return ParameterError(3);
        }
        return Operand.Create(ExcelFunctions.BinomDist(args1.IntValue(), n2, n3, args4.BooleanValue()));
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func4.GetParameterTypes(noneEngine, result, OperandType.BOOLEAN);
    }
}
