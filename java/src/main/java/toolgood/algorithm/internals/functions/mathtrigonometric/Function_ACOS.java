package toolgood.algorithm.internals.functions.mathtrigonometric;

import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_1;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.system.MathEx;

public final class Function_ACOS extends Function_1 {
    public Function_ACOS(FunctionBase func1) {
        super(func1);
    }

    @Override
    public String Name() {
        return "Acos";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }
        java.math.BigDecimal x = args1.NumberValue();
        if (x.compareTo(java.math.BigDecimal.ONE) > 0 || x.compareTo(java.math.BigDecimal.ONE.negate()) < 0) {
            return ParameterError(1);
        }
        return Operand.Create(MathEx.Acos(x));
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
