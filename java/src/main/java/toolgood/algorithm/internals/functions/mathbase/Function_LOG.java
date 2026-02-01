package toolgood.algorithm.internals.functions.mathbase;

import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;

public class Function_LOG extends Function_2 {
    public Function_LOG(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Log", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        if (func2 != null) {
            Operand args2 = func2.Evaluate(work, tempParameter);
            if (args2.IsNotNumber()) {
                args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Log", 2);
                if (args2.IsError()) {
                    return args2;
                }
            }
            double value = args1.DoubleValue();
            double baseValue = args2.DoubleValue();
            return Operand.Create(Math.log(value) / Math.log(baseValue));
        }
        return Operand.Create(Math.log10(args1.DoubleValue()));
    }

    @Override
    public void toString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Log");
    }
}
