package toolgood.algorithm.internals.functions.mathbase;

import toolgood.algorithm.Internals.Operand;
import toolgood.algorithm.Internals.AlgorithmEngine;
import toolgood.algorithm.internals.FunctionBase;

public class Function_ROUNDUP extends Function_2 {
    public Function_ROUNDUP(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.Function<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "RoundUp", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber()) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "RoundUp", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        if (args1.NumberValue() == 0.0) {
            return args1;
        }
        double a = Math.pow(10, args2.IntValue());
        double b = args1.NumberValue();

        double t = Math.ceil(Math.abs(b) * a) / a;
        if (b > 0) {
            return Operand.Create(t);
        }
        return Operand.Create(-t);
    }

    @Override
    public void ToString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "RoundUp");
    }
}
