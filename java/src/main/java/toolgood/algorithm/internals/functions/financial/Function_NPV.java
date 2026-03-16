package toolgood.algorithm.internals.functions.financial;

import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;

/**
 * NPV: 基于一系列定期的现金流和贴现率，返回一项投资的净现值
 * NPV(rate, value1, value2, ...)
 */
public class Function_NPV extends Function_N {
    public Function_NPV(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand rateArg = GetNumber(engine, tempParameter, 0);
        if (rateArg.IsError()) return rateArg;
        double rate = rateArg.DoubleValue();

        if (rate == -1) return Div0Error();

        double npv = 0;
        for (int i = 1; i < funcs.length; i++) {
            Operand arg = GetNumber(engine, tempParameter, i);
            if (arg.IsError()) return arg;
            npv += arg.DoubleValue() / Math.pow(1 + rate, i);
        }

        return Operand.Create(npv);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "NPV");
    }
}
