package toolgood.algorithm.internals.functions.mathsum2;

import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.mathNet.ExcelFunctions;

public final class Function_LOGNORMDIST extends Function_3 {
    public Function_LOGNORMDIST(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "LogNormDist";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) return args1;
        java.math.BigDecimal x = args1.NumberValue();
        if (x.compareTo(java.math.BigDecimal.ZERO) <= 0) {
            return ParameterError(1);
        }

        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) return args2;

        Operand args3 = GetNumber_3(engine, tempParameter);
        if (args3.IsErrorOrNone()) return args3;

        java.math.BigDecimal n3 = args3.NumberValue();
        if (n3.compareTo(java.math.BigDecimal.ZERO) <= 0) {
            return ParameterError(3);
        }
        return Operand.Create(ExcelFunctions.LognormDist(args1.NumberValue(), args2.NumberValue(), n3));
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
    }
}
