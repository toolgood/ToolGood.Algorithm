package toolgood.algorithm.internals.functions.mathsum;

import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_1;
import toolgood.algorithm.AlgorithmEngine;

public class Function_FISHER extends Function_1 {
    public Function_FISHER(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.Function<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter is error!", "Fisher");
            if (args1.IsError()) {
                return args1;
            }
        }
        double x = args1.DoubleValue();
        if (x >= 1 || x <= -1) {
            return Operand.Error("Function '{0}' parameter is error!", "Fisher");
        }
        double n = 0.5 * Math.log((1 + x) / (1 - x));
        return Operand.Create(n);
    }

    @Override
    public void toString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Fisher");
    }
}
