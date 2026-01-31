package toolgood.algorithm.internals.functions.flow;

import toolgood.algorithm.Internals.Operand;
import toolgood.algorithm.Internals.AlgorithmEngine;
import toolgood.algorithm.internals.FunctionBase;

public class Function_ISODD extends Function_1 {
    public Function_ISODD(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.Function<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNumber()) {
            if (args1.IntValue() % 2 == 1) {
                return Operand.True();
            }
        }
        return Operand.False();
    }

    @Override
    public void ToString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "IsOdd");
    }
}
