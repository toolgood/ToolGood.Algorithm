package toolgood.algorithm.internals.functions.mathbase;

import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.FunctionBase;

public class Function_EVEN extends Function_1 {
    public Function_EVEN(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.Function<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter is error!", "Even");
            if (args1.IsError()) {
                return args1;
            }
        }
        double z = args1.NumberValue();
        if (z % 2 == 0) {
            return args1;
        }
        z = Math.ceil(z);
        if (z % 2 == 0) {
            return Operand.Create(z);
        }
        z++;
        return Operand.Create(z);
    }

    @Override
    public void toString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Even");
    }
}
