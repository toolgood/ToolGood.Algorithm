package toolgood.algorithm.internals.functions.mathbase;

import toolgood.algorithm.Internals.Operand;
import toolgood.algorithm.Internals.AlgorithmEngine;
import toolgood.algorithm.internals.FunctionBase;

public class Function_FACTDOUBLE extends Function_1 {
    public Function_FACTDOUBLE(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.Function<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter is error!", "FactDouble");
            if (args1.IsError()) {
                return args1;
            }
        }
        int z = args1.IntValue();
        if (z < 0) {
            return Operand.Error("Function '{0}' parameter is error!", "FactDouble");
        }

        double d = 1;
        for (int i = z; i > 0; i -= 2) {
            d *= i;
        }
        return Operand.Create(d);
    }

    @Override
    public void toString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "FactDouble");
    }
}
