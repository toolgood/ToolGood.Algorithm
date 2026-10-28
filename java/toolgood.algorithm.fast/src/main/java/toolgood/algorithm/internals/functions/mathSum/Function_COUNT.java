package toolgood.algorithm.internals.functions.mathSum;

import java.util.List;
import java.util.ArrayList;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.internals.functions.Function_N;

public final class Function_COUNT extends Function_N {
    public Function_COUNT(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 1) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires at least 1 parameter.");
        }
    }

    @Override
    public String Name() { return "Count"; }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        // 逐个求值并直接展平，免去中间的 List<Operand> 参数中转；
        // 此处必须展平为 Operand 列表——COUNT 需要保留每项原始操作数
        List<Operand> list = new ArrayList<>();
        for (int i = 0; i < funcs.length; i++) {
            Operand aa = funcs[i].Evaluate(engine, tempParameter);
            if (aa.IsErrorOrNone()) { return aa; }
            if (FunctionUtil.FlattenToList(aa, list) == false) { return FunctionError(); }
        }
        return Operand.Create(list.size());
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
                funcs[i].GetParameterTypes(noneEngine, result, OperandType.NONE);
            }
        }
    }
}
