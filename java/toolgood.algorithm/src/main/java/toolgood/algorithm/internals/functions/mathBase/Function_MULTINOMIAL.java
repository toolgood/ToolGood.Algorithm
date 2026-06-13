package toolgood.algorithm.internals.functions.mathBase;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.internals.functions.Function_N;

public final class Function_MULTINOMIAL extends Function_N {
    public Function_MULTINOMIAL(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 1) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires at least 1 parameter.");
        }
    }

    @Override
    public String Name() {
        return "Multinomial";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        List<Operand> args = new ArrayList<>(funcs.length);
        Operand error = TryEvaluateAll(engine, tempParameter, args);
        if (error != null) {
            return error;
        }
        List<BigDecimal> list = new ArrayList<>();
        boolean o = FunctionUtil.FlattenToBigDecimalList(args, list);
        if (o == false) {
            return FunctionError();
        }
        BigDecimal sum = BigDecimal.ZERO;
        BigDecimal n = BigDecimal.ONE;
        for (int i = 0; i < list.size(); i++) {
            int a = list.get(i).intValue();
            if (a < 0) {
                return ParameterError(i + 1);
            }
            n = n.multiply(FunctionUtil.GetFactorial(BigDecimal.valueOf(a)));
            sum = sum.add(BigDecimal.valueOf(a));
        }
        BigDecimal r = FunctionUtil.GetFactorial(sum).divide(n);
        return Operand.Create(r);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        for (FunctionBase item : funcs) {
            item.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        }
    }
}
