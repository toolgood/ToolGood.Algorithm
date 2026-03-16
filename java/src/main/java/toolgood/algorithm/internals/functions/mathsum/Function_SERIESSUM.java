package toolgood.algorithm.internals.functions.mathsum;

import java.math.BigDecimal;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.internals.functions.NoneEngine;

public class Function_SERIESSUM extends Function_N {
    public Function_SERIESSUM(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "SERIESSUM";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        if (funcs.length < 4) return ParameterError(1);

        Operand xArg = GetNumber(engine, tempParameter, 0);
        if (xArg.IsError()) return xArg;
        BigDecimal x = xArg.NumberValue();

        Operand nArg = GetNumber(engine, tempParameter, 1);
        if (nArg.IsError()) return nArg;
        BigDecimal n = nArg.NumberValue();

        Operand mArg = GetNumber(engine, tempParameter, 2);
        if (mArg.IsError()) return mArg;
        BigDecimal m = mArg.NumberValue();

        Operand coefficientsArg = GetArray(engine, tempParameter, 3);
        if (coefficientsArg.IsError()) return coefficientsArg;

        BigDecimal result = BigDecimal.ZERO;
        int i = 0;
        for (Operand coef : coefficientsArg.ArrayValue()) {
            if (coef.IsNumber()) {
                // power = n + i * m
                BigDecimal power = n.add(new BigDecimal(i).multiply(m));
                double powD = Math.pow(x.doubleValue(), power.doubleValue());
                result = result.add(coef.NumberValue().multiply(new BigDecimal(powD)));
                i++;
            }
        }

        return Operand.Create(result);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void getParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        funcs[0].getParameterTypes(noneEngine, result, OperandType.NUMBER, null, null);
        funcs[1].getParameterTypes(noneEngine, result, OperandType.NUMBER, null, null);
        funcs[2].getParameterTypes(noneEngine, result, OperandType.NUMBER, null, null);
        funcs[3].getParameterTypes(noneEngine, result, OperandType.ARRAY, null, null);
    }
}
