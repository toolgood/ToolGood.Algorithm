package toolgood.algorithm.internals.functions.financial;

import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_SLN extends Function_3 {
    public Function_SLN(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "SLN";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand costArg = GetNumber_1(engine, tempParameter);
        if (costArg.IsErrorOrNone()) return costArg;
        double cost = costArg.DoubleValue();

        Operand salvageArg = GetNumber_2(engine, tempParameter);
        if (salvageArg.IsErrorOrNone()) return salvageArg;
        double salvage = salvageArg.DoubleValue();

        Operand lifeArg = GetNumber_3(engine, tempParameter);
        if (lifeArg.IsErrorOrNone()) return lifeArg;
        double life = lifeArg.DoubleValue();

        if (life == 0) return Div0Error();

        return Operand.Create((cost - salvage) / life);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
