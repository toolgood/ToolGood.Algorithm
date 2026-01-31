package toolgood.algorithm.internals.functions.flow;

import toolgood.algorithm.Internals.Operand;
import toolgood.algorithm.Internals.AlgorithmEngine;
import toolgood.algorithm.internals.FunctionBase;

public class Function_NOT extends Function_1 {
    public Function_NOT(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.Function<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotBoolean()) {
            args1 = args1.ToBoolean("Function '{0}' parameter is error!", "Not");
            if (args1.IsError()) {
                return args1;
            }
        }
        return args1.BooleanValue() ? Operand.False() : Operand.True();
    }

    @Override
    public void ToString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Not");
    }
}
