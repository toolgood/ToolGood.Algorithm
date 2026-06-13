package toolgood.algorithm.internals.functions.mathSum;

import java.util.List;
import java.util.ArrayList;
import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.internals.functions.Function_N;

public final class Function_AVEDEV extends Function_N {
    public Function_AVEDEV(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 1) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires at least 1 parameter.");
        }
    }

    @Override
    public String Name() { return "AveDev"; }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        List<Operand> args = new ArrayList<>(funcs.length);
        Operand error = TryEvaluateAll(engine, tempParameter, args);
        if (error != null) { return error; }

        List<BigDecimal> list = new ArrayList<>();
        boolean o = FunctionUtil.FlattenToBigDecimalList(args, list);
        if (!o) { return FunctionError(); }
        if (list.size() == 0) { return Operand.Zero; }

        BigDecimal sum = BigDecimal.ZERO;
        for (int i = 0; i < list.size(); i++) { sum = sum.add(list.get(i)); }
        BigDecimal avg = sum.divide(BigDecimal.valueOf(list.size()), RoundingMode.HALF_UP);

        BigDecimal sum2 = BigDecimal.ZERO;
        for (int i = 0; i < list.size(); i++) {
            BigDecimal diff = list.get(i).subtract(avg);
            sum2 = sum2.add(diff.abs());
        }
        return Operand.Create(sum2.divide(BigDecimal.valueOf(list.size()), RoundingMode.HALF_UP));
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        if (funcs.length == 1) {
            funcs[0].GetParameterTypes(noneEngine, result, OperandType.ARRAY);
        } else {
            for (int i = 0; i < funcs.length; i++) {
                funcs[i].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
            }
        }
    }
}
