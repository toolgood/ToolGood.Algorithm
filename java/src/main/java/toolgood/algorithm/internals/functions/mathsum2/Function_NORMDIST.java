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

public final class Function_NORMDIST extends Function_4 {
    public Function_NORMDIST(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "NormDist";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) return args1;

        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) return args2;

        Operand args3 = GetNumber_3(engine, tempParameter);
        if (args3.IsErrorOrNone()) return args3;

        Operand args4 = GetBoolean_4(engine, tempParameter);
        if (args4.IsErrorOrNone()) return args4;

        java.math.BigDecimal num = args1.NumberValue();
        java.math.BigDecimal avg = args2.NumberValue();
        java.math.BigDecimal STDEV = args3.NumberValue();
        if (STDEV.compareTo(java.math.BigDecimal.ZERO) <= 0) {
            return ParameterError(3);
        }
        boolean b = args4.BooleanValue();

        return Operand.Create(ExcelFunctions.NormDist(num, avg, STDEV, b));
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
