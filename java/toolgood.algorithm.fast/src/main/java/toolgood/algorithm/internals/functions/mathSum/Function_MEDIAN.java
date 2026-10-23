package toolgood.algorithm.internals.functions.mathSum;

import java.util.List;
import java.util.ArrayList;
import java.math.BigDecimal;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.internals.functions.Function_N;

public final class Function_MEDIAN extends Function_N {

    public Function_MEDIAN(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 1) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires at least 1 parameter.");
        }
    }

    @Override
    public String Name() {
        return "Median";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        List<Operand> args = new ArrayList<>(funcs.length);
        Operand error = TryEvaluateAll(engine, tempParameter, args);
        if (error != null) { return error; }

        List<BigDecimal> list = new ArrayList<>();
        if (FunctionUtil.FlattenToBigDecimalList(args, list) == false) { return FunctionError(); }
        if (list.size() == 0) { return FunctionError(); }

        int n = list.size();
        if (n == 1) return Operand.Create(list.get(0));

        int mid = n / 2;
        if (n % 2 == 0) {
            BigDecimal a = FunctionUtil.QuickSelect(list, mid - 1, false);
            BigDecimal b = FunctionUtil.QuickSelect(list, mid, false);
            return Operand.Create(a.add(b).divide(BigDecimal.valueOf(2)));
        }
        return Operand.Create(FunctionUtil.QuickSelect(list, mid, false));
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
