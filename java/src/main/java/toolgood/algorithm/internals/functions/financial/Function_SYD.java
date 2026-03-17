package toolgood.algorithm.internals.functions.financial;

import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_4;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_SYD extends Function_4 {
    public Function_SYD(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "SYD";
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

        Operand periodArg = GetNumber_4(engine, tempParameter);
        if (periodArg.IsErrorOrNone()) return periodArg;
        double period = periodArg.DoubleValue();

        if (life == 0) return Div0Error();
        if (period < 1 || period > life) return ParameterError(4);
        if (life < 1) return ParameterError(3);

        double syd = (cost - salvage) * (life - period + 1) * 2 / (life * (life + 1));
        return Operand.Create(syd);
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
        func4.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
