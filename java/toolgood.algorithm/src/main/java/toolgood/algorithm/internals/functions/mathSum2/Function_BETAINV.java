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
import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.mathNet.ExcelFunctions;

final class Function_BETAINV extends Function_3 {
    public Function_BETAINV(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() { return "BetaInv"; }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) return args1;

        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) return args2;

        Operand args3 = GetNumber_3(engine, tempParameter);
        if (args3.IsErrorOrNone()) return args3;

        BigDecimal p = args1.NumberValue();
        if (p.compareTo(BigDecimal.ZERO) < 0 || p.compareTo(BigDecimal.ONE) > 0) {
            return ParameterError(1);
        }
        BigDecimal alpha = args2.NumberValue();
        if (alpha.compareTo(BigDecimal.ZERO) <= 0) {
            return ParameterError(2);
        }
        BigDecimal beta = args3.NumberValue();
        if (beta.compareTo(BigDecimal.ZERO) <= 0) {
            return ParameterError(3);
        }
        try {
            return Operand.Create(BigDecimal.valueOf(ExcelFunctions.BetaInv(p.doubleValue(), alpha.doubleValue(), beta.doubleValue())));
        } catch (Exception e) {
            return FunctionError();
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
        func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
