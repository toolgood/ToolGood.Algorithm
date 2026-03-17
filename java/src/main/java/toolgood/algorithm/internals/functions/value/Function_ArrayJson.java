package toolgood.algorithm.internals.functions.value;

import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.operands.OperandKeyValue;
import toolgood.algorithm.operands.OperandKeyValueList;

public final class Function_ArrayJson extends Function_N {
    public Function_ArrayJson(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "ArrayJson";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        OperandKeyValueList result = new OperandKeyValueList();
        for (FunctionBase item : funcs) {
            Operand o = item.Evaluate(engine, tempParameter);
            if (o.IsErrorOrNone()) {
                return o;
            }
            if (o instanceof OperandKeyValue) {
                result.AddValue(((OperandKeyValue) o).Value());
            } else {
                return ParameterError(1);
            }
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

    @Override
    public OperandType GetResultType() {
        return OperandType.ARRAYJSON;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        for (int i = 0; i < funcs.length; i++) {
            funcs[i].GetParameterTypes(noneEngine, result, OperandType.NONE);
        }
    }
}
