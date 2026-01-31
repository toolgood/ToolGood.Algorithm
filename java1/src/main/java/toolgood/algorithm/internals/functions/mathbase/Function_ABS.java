package toolgood.algorithm.internals.functions.mathbase;

import toolgood.algorithm.Internals.Operand;
import toolgood.algorithm.Internals.AlgorithmEngine;
import toolgood.algorithm.internals.FunctionBase;

public class Function_ABS extends Function_1 {
    public Function_ABS(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.Function<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter is error!", "Abs");
            if (args1.IsError()) {
                return args1;
            }
        }
        return Operand.Create(Math.abs(args1.NumberValue()));
    }

    @Override
    public void ToString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Abs");
    }
}
