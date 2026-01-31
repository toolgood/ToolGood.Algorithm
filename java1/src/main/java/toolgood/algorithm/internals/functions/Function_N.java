package toolgood.algorithm.internals.functions;

import java.lang.StringBuilder;

abstract class Function_N extends FunctionBase {
    protected FunctionBase[] funcs;

    protected Function_N(FunctionBase[] funcs) {
        this.funcs = funcs;
    }

    protected void addFunction(StringBuilder stringBuilder, String functionName) {
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
}
