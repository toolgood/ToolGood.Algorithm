package toolgood.algorithm.internals.functions.flow;

import toolgood.algorithm.Internals.Operand;
import toolgood.algorithm.Internals.AlgorithmEngine;
import toolgood.algorithm.internals.FunctionBase;

public class Function_IsNumber extends Function_1 {
    public Function_IsNumber(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.Function<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNumber()) {
            return Operand.True();
        }
        return Operand.False();
    }

    @Override
    public void toString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "IsNumber");
    }
}
