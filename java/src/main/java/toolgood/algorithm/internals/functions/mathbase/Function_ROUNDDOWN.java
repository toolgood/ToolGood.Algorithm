package toolgood.algorithm.internals.functions.mathbase;

import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;

public class Function_ROUNDDOWN extends Function_2 {
    public Function_ROUNDDOWN(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "RoundDown", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber()) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "RoundDown", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        if (args1.DoubleValue() == 0.0) {
            return args1;
        }
        double a = Math.pow(10, args2.IntValue());
        double b = args1.DoubleValue();

        b = (Math.floor(b * a)) / a;
        return Operand.Create(b);
    }

    @Override
    public void toString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "RoundDown");
    }
}
