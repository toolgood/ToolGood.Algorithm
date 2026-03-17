package toolgood.algorithm.internals.functions.mathsum2;

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
    public Function_TINV(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "TInv";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) return args1;

        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) return args2;

        java.math.BigDecimal p = args1.NumberValue();
        if (p.compareTo(java.math.BigDecimal.ZERO) <= 0 || p.compareTo(java.math.BigDecimal.ONE) >= 0) {
            return ParameterError(1);
        }
        int degreesFreedom = args2.IntValue();
        if (degreesFreedom <= 0) {
            return ParameterError(2);
        }
        return Operand.Create(ExcelFunctions.TInv(p, degreesFreedom));
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
