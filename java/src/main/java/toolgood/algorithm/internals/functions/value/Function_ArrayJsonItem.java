package toolgood.algorithm.internals.functions.value;

import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_1;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.operands.KeyValue;
import toolgood.algorithm.operands.OperandKeyValue;

public final class Function_ArrayJsonItem extends Function_1 {
    private final String key;

    public Function_ArrayJsonItem(String key, FunctionBase func1) {
        super(func1);
        this.key = key;
    }

    @Override
    public String Name() {
        return "ArrayJsonItem";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        KeyValue keyValue = new KeyValue();
        keyValue.setKey(key);
        keyValue.setValue(func1.Evaluate(engine, tempParameter));
        return new OperandKeyValue(keyValue);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        stringBuilder.append(key);
        stringBuilder.append(':');
        func1.toString(stringBuilder, false);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.ARRAYJSON;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NONE);
    }
}
