package toolgood.algorithm.internals.functions.mathbase;

import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;

public class Function_ROUND extends Function_2 {
    public Function_ROUND(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Round", 1);
            if (args1.IsError()) {
                return args1;
            }
        }

        if (func2 == null) {
            return Operand.Create(Math.round(args1.DoubleValue()));
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber()) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Round", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        int decimalPlaces = args2.IntValue();
        double value = args1.DoubleValue();
        double factor = Math.pow(10, decimalPlaces);
        double roundedValue = Math.round(value * factor) / factor;
        return Operand.Create(roundedValue);
    }

    @Override
    public void toString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Round");
    }
}
