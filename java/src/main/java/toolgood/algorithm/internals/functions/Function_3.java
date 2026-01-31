package toolgood.algorithm.internals.functions;

import java.lang.StringBuilder;

abstract class Function_3 extends FunctionBase {
    protected FunctionBase func1;
    protected FunctionBase func2;
    protected FunctionBase func3;

    protected Function_3(FunctionBase func1, FunctionBase func2, FunctionBase func3) {
        this.func1 = func1;
        this.func2 = func2;
        this.func3 = func3;
    }
    
    protected void addFunction(StringBuilder stringBuilder, String functionName) {
        stringBuilder.append(functionName);
        stringBuilder.append('(');
        func1.toString(stringBuilder, false);
        if (func2 != null) {
            stringBuilder.append(", ");
            func2.toString(stringBuilder, false);
            if (func3 != null) {
                stringBuilder.append(", ");
                func3.toString(stringBuilder, false);
            }
        }
        stringBuilder.append(')');
    }
}
