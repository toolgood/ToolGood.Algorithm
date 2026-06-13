package toolgood.algorithm.internals.functions.mathSum;

import java.util.List;
import java.util.ArrayList;
import java.math.BigDecimal;
import java.util.Map;
import java.util.HashMap;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.internals.functions.Function_N;

public final class Function_MODE extends Function_N {
    public Function_MODE(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 1) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires at least 1 parameter.");
        }
    }

    @Override
    public String Name() { return "Mode"; }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        List<Operand> args = new ArrayList<>(funcs.length);
        Operand error = TryEvaluateAll(engine, tempParameter, args);
        if (error != null) { return error; }

        List<BigDecimal> list = new ArrayList<>();
        boolean o = FunctionUtil.FlattenToBigDecimalList(args, list);
        if (!o) { return FunctionError(); }
        if (list.size() == 0) { return FunctionError(); }

        Map<BigDecimal, Integer> dict = new HashMap<>();
        for (int i = 0; i < list.size(); i++) {
            BigDecimal key = list.get(i);
            Integer count = dict.get(key);
            if (count != null) {
                dict.put(key, count + 1);
            } else {
                dict.put(key, 1);
            }
        }
        BigDecimal modeKey = BigDecimal.ZERO;
        int maxCount = 1;
        for (Map.Entry<BigDecimal, Integer> entry : dict.entrySet()) {
            if (entry.getValue() > maxCount) {
                maxCount = entry.getValue();
                modeKey = entry.getKey();
            }
        }
        if (maxCount == 1) { return FunctionError(); }
        return Operand.Create(modeKey);
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
