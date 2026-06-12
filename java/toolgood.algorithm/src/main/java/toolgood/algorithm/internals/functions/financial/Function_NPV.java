package toolgood.algorithm.internals.functions.financial;

import java.math.BigDecimal;
import java.math.MathContext;
import java.math.RoundingMode;
import java.util.ArrayList;
import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;

final class Function_NPV extends Function_N {
    public Function_NPV(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 1) {
            throw new IllegalArgumentException(String.format("Function '%s' requires at least 1 parameter.", Name()));
        }
    }

    @Override
    public String Name() {
        return "NPV";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand rateArg = GetNumber(engine, tempParameter, 0);
        if (rateArg.IsErrorOrNone()) return rateArg;
        BigDecimal rate = rateArg.NumberValue();
        if (rate.compareTo(BigDecimal.ONE.negate()) == 0) {
            return Div0Error();
        }

        List<BigDecimal> values = new ArrayList<>();
        for (int i = 1; i < funcs.length; i++) {
            Operand arg = GetNumber(engine, tempParameter, i);
            if (arg.IsErrorOrNone()) return arg;
            values.add(arg.NumberValue());
        }

        BigDecimal npv = BigDecimal.ZERO;
        for (int i = 0; i < values.size(); i++) {
            BigDecimal factor = BigDecimal.valueOf(Math.pow(BigDecimal.ONE.add(rate).doubleValue(), i + 1));
            npv = npv.add(values.get(i).divide(factor, MathContext.DECIMAL128));
        }

        return Operand.Create(npv);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        for (int i = 0; i < funcs.length; i++) {
            funcs[i].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        }
    }
}
