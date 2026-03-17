package toolgood.algorithm.internals.functions.financial;

import java.util.ArrayList;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_NPV extends Function_N {
    public Function_NPV(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "NPV";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand rateArg = GetNumber(engine, tempParameter, 0);
        if (rateArg.IsErrorOrNone()) return rateArg;
        double rate = rateArg.DoubleValue();
        if (rate == -1) {
            return Div0Error();
        }

        List<Double> values = new ArrayList<>();
        for (int i = 1; i < funcs.length; i++) {
            Operand arg = GetNumber(engine, tempParameter, i);
            if (arg.IsErrorOrNone()) return arg;
            values.add(arg.DoubleValue());
        }

        double npv = 0;
        for (int i = 0; i < values.size(); i++) {
            npv += values.get(i) / Math.pow(1 + rate, i + 1);
        }

        return Operand.Create(npv);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        for (int i = 0; i < funcs.length; i++) {
            funcs[i].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        }
    }
}
