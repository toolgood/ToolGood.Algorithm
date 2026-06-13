package toolgood.algorithm.internals.functions.value;

import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.Operand.KeyValue;
import toolgood.algorithm.Operand.OperandKeyValue;
import toolgood.algorithm.Operand.OperandKeyValueList;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_ArrayJson extends Function_N {

    public Function_ArrayJson(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "ArrayJson";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        OperandKeyValueList result = new OperandKeyValueList(null);
        for (FunctionBase item : funcs) {
            Operand o = item.Evaluate(engine, tempParameter);
            if (o.IsErrorOrNone()) { return o; }
            if (o instanceof OperandKeyValue) {
                result.AddValue((KeyValue)((OperandKeyValue)o).Value());
            } else {
                return ParameterError(1);
            }
        }
        return result;
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        stringBuilder.append('{');
        for (int i = 0; i < funcs.length; i++) {
            if (i > 0) {
                stringBuilder.append(", ");
            }
            funcs[i].ToString(stringBuilder, false);
        }
        stringBuilder.append('}');
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.ARRAYJSON;
    }

    @Override
    void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        for (int i = 0; i < funcs.length; i++) {
            funcs[i].GetParameterTypes(noneEngine, result, OperandType.NONE);
        }
    }
}
