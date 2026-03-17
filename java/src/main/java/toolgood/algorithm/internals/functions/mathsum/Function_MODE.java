package toolgood.algorithm.internals.functions.mathsum;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_MODE extends Function_N {
    public Function_MODE(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Mode";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        List<Operand> args = new ArrayList<>(funcs.length);
        Operand error = TryEvaluateAll(engine, tempParameter, args);
        if (error != null) {
            return error;
        }

        List<BigDecimal> list = new ArrayList<>();
        boolean o = FunctionUtil.FlattenToList_BigDecimal(args, list);
        if (o == false) {
            return FunctionError();
        }
        if (list.size() == 0) {
            return FunctionError();
        }

        Map<BigDecimal, Integer> dict = new HashMap<>();
        for (int i = 0; i < list.size(); i++) {
            BigDecimal key = list.get(i);
            if (dict.containsKey(key)) {
                dict.put(key, dict.get(key) + 1);
            } else {
                dict.put(key, 1);
            }
        }
        BigDecimal modeKey = BigDecimal.ZERO;
        int maxCount = 1;
        for (Map.Entry<BigDecimal, Integer> kvp : dict.entrySet()) {
            if (kvp.getValue() > maxCount) {
                maxCount = kvp.getValue();
                modeKey = kvp.getKey();
            }
        }
        if (maxCount == 1) {
            return FunctionError();
        }
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
