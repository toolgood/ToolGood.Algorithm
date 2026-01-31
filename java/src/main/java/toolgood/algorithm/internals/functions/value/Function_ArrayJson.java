package toolgood.algorithm.internals.functions.value;

import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.internals.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_N;

public class Function_ArrayJson extends Function_N {
    public Function_ArrayJson(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        OperandKeyValueList result = new OperandKeyValueList();
        for (FunctionBase item : funcs) {
            Operand o = item.Evaluate(work, tempParameter);
            result.AddValue(((OperandKeyValue)o).getValue());
        }
        return result;
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        stringBuilder.append('{');
        for (int i = 0; i < funcs.length; i++) {
            if (i > 0) {
                stringBuilder.append(", ");
            }
            funcs[i].toString(stringBuilder, false);
        }
        stringBuilder.append('}');
    }
}
