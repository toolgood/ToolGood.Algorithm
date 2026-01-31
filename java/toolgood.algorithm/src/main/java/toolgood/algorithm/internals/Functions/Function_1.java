package toolgood.algorithm.internals.functions;

import java.lang.StringBuilder;

abstract class Function_1 extends FunctionBase {
    protected FunctionBase func1;

    protected Function_1(FunctionBase func1) {
        this.func1 = func1;
    }
    
    protected void addFunction(StringBuilder stringBuilder, String functionName) {
        stringBuilder.append(functionName);
        stringBuilder.append('(');
        func1.toString(stringBuilder, false);
        stringBuilder.append(')');
    }
}
