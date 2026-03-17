package toolgood.algorithm.internals.functions.mathsum;

import java.math.BigDecimal;
import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.system.MathEx;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_SERIESSUM extends Function_N {
    public Function_SERIESSUM(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "SERIESSUM";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        if (funcs.length < 4) return ParameterError(1);

        Operand xArg = GetNumber(engine, tempParameter, 0);
        if (xArg.IsErrorOrNone()) return xArg;
        BigDecimal x = xArg.NumberValue();

        Operand nArg = GetNumber(engine, tempParameter, 1);
        if (nArg.IsErrorOrNone()) return nArg;
        BigDecimal n = nArg.NumberValue();

        Operand mArg = GetNumber(engine, tempParameter, 2);
        if (mArg.IsErrorOrNone()) return mArg;
        BigDecimal m = mArg.NumberValue();

        Operand coefficientsArg = GetArray(engine, tempParameter, 3);
        if (coefficientsArg.IsErrorOrNone()) return coefficientsArg;

        BigDecimal result = BigDecimal.ZERO;
        int i = 0;
        for (Operand coef : coefficientsArg.ArrayValue()) {
            if (coef.IsNumber()) {
                BigDecimal power = n.add(new BigDecimal(i).multiply(m));
                result = result.add(coef.NumberValue().multiply(MathEx.Pow(x, power)));
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
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        funcs[0].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        funcs[1].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        funcs[2].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        funcs[3].GetParameterTypes(noneEngine, result, OperandType.ARRAY);
    }
}
