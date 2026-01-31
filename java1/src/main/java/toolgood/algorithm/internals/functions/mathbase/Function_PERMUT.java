package toolgood.algorithm.internals.functions.mathbase;

import toolgood.algorithm.Internals.Operand;
import toolgood.algorithm.Internals.AlgorithmEngine;
import toolgood.algorithm.internals.FunctionBase;

public class Function_PERMUT extends Function_2 {
    public Function_PERMUT(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.Function<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Permut", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber()) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Permut", 2);
            if (args2.IsError()) {
                return args2;
            }
        }

        int total = args1.IntValue();
        int count = args2.IntValue();

        double sum = 1;
        for (int i = 0; i < count; i++) {
            sum *= (total - i);
        }
        return Operand.Create(sum);
    }

    @Override
    public void ToString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Permut");
    }
}
