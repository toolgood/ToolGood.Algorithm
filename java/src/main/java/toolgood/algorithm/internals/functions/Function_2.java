package toolgood.algorithm.internals.functions;

import java.lang.StringBuilder;

public abstract class Function_2 extends FunctionBase {
    protected FunctionBase func1;
    protected FunctionBase func2;

    public Function_2(FunctionBase func1, FunctionBase func2) {
        this.func1 = func1;
        this.func2 = func2;
    }

    protected void AddFunction(StringBuilder stringBuilder, String functionName) {
        stringBuilder.append(functionName);
        stringBuilder.append('(');
        func1.toString(stringBuilder, false);
        if (func2 != null) {
            stringBuilder.append(", ");
            func2.toString(stringBuilder, false);
        }
        stringBuilder.append(')');
    }
}
