package toolgood.algorithm.internals.functions;

import java.lang.StringBuilder;
import java.util.List;

import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;

public abstract class Function_N extends FunctionBase {
    protected FunctionBase[] funcs;

    protected Function_N(FunctionBase[] funcs) {
        this.funcs = funcs;
    }

    protected void AddFunction(StringBuilder stringBuilder, String functionName) {
        stringBuilder.append(functionName);
        stringBuilder.append('(');
        for (int i = 0; i < funcs.length; i++) {
            if (i > 0) {
                stringBuilder.append(", ");
            }
            funcs[i].toString(stringBuilder, false);
        }
        stringBuilder.append(')');
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NONE;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result,
            OperandType operandType, String op, String val) {
    }
}
