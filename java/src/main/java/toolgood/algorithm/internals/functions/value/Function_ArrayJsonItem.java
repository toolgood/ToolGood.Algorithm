package toolgood.algorithm.internals.functions.value;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_1;

public class Function_ArrayJsonItem extends Function_1 {
    private final String key;

    public Function_ArrayJsonItem(String key, FunctionBase func1) {
        super(func1);
        this.key = key;
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiBiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        KeyValue keyValue = new KeyValue();
        keyValue.setKey(key);
        keyValue.setValue(func1.Evaluate(work, tempParameter));
        return new OperandKeyValue(keyValue);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        stringBuilder.append(key);
        stringBuilder.append(':');
        func1.toString(stringBuilder, false);
    }
}
