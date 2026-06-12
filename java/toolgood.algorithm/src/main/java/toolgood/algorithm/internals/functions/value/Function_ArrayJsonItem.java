package toolgood.algorithm.internals.functions.value;

import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.Operand.KeyValue;
import toolgood.algorithm.Operand.OperandKeyValue;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_1;
import toolgood.algorithm.internals.functions.NoneEngine;

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
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        KeyValue keyValue = new KeyValue();
        keyValue.Key = key;
        keyValue.Value = func1.Evaluate(engine, tempParameter);
        return new OperandKeyValue(keyValue);
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        stringBuilder.append(key);
        stringBuilder.append(':');
        func1.ToString(stringBuilder, false);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.ARRARYJSON;
    }

    @Override
    void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NONE);
    }
}
