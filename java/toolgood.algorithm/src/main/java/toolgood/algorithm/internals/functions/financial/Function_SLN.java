package toolgood.algorithm.internals.functions.financial;

import java.math.BigDecimal;
import java.math.MathContext;
import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;

final class Function_SLN extends Function_3 {
    public Function_SLN(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 3) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 3 parameters.");
        }
    }

    @Override
    public String Name() {
        return "SLN";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand costArg = GetNumber_1(engine, tempParameter);
        if (costArg.IsErrorOrNone()) return costArg;
        BigDecimal cost = costArg.NumberValue();

        Operand salvageArg = GetNumber_2(engine, tempParameter);
        if (salvageArg.IsErrorOrNone()) return salvageArg;
        BigDecimal salvage = salvageArg.NumberValue();

        Operand lifeArg = GetNumber_3(engine, tempParameter);
        if (lifeArg.IsErrorOrNone()) return lifeArg;
        BigDecimal life = lifeArg.NumberValue();

        if (life.compareTo(BigDecimal.ZERO) == 0) return Div0Error();

        return Operand.Create(cost.subtract(salvage).divide(life, MathContext.DECIMAL128));
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
