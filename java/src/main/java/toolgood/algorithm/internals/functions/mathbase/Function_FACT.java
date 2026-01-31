package toolgood.algorithm.internals.functions.mathbase;

import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.FunctionBase;

public class Function_FACT extends Function_1 {
    public Function_FACT(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.Function<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter is error!", "Fact");
            if (args1.IsError()) {
                return args1;
            }
        }
        if (args1.IsError()) {
            return args1;
        }

        int z = args1.IntValue();
        if (z < 0) {
            return Operand.Error("Function '{0}' parameter is error!", "Fact");
        }
        double d = 1;
        for (int i = 1; i <= z; i++) {
            d *= i;
        }
        return Operand.Create(d);
    }

    @Override
    public void toString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Fact");
    }
}
