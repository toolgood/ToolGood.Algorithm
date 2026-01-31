package toolgood.algorithm.internals.functions.mathsum;

import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Function_1;
import toolgood.algorithm.internals.AlgorithmEngine;

public class Function_FISHERINV extends Function_1 {
    public Function_FISHERINV(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.Function<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter is error!", "FisherInv");
            if (args1.IsError()) {
                return args1;
            }
        }
        double x = args1.DoubleValue();
        double n = (Math.exp(2 * x) - 1) / (Math.exp(2 * x) + 1);
        return Operand.Create(n);
    }

    @Override
    public void toString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "FisherInv");
    }
}
