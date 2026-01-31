package toolgood.algorithm.internals.functions;

import java.lang.StringBuilder;

abstract class Function_4 extends FunctionBase {
    protected FunctionBase func1;
    protected FunctionBase func2;
    protected FunctionBase func3;
    protected FunctionBase func4;

    protected Function_4(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) {
        this.func1 = func1;
        this.func2 = func2;
        this.func3 = func3;
        this.func4 = func4;
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
                if (func4 != null) {
                    stringBuilder.append(", ");
                    func4.toString(stringBuilder, false);
                }
            }
        }
        stringBuilder.append(')');
    }
}
