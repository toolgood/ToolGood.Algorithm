package toolgood.algorithm.internals.functions.mathbase;

import toolgood.algorithm.Internals.Operand;
import toolgood.algorithm.Internals.AlgorithmEngine;
import toolgood.algorithm.internals.FunctionBase;

public class Function_CEILING extends Function_2 {
    public Function_CEILING(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.Function<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Ceiling", 1);
            if (args1.IsError()) {
                return args1;
            }
        }

        if (func2 == null) {
            return Operand.Create(Math.ceil(args1.NumberValue()));
        }

        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber()) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Ceiling", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        double b = args2.NumberValue();
        if (b == 0) {
            return Operand.Create(0);
        }
        if (b < 0) {
            return Operand.Error("Function '{0}' parameter {1} is error!", "Ceiling", 2);
        }

        double a = args1.NumberValue();
        double d = Math.ceil(a / b) * b;
        return Operand.Create(d);
    }

    @Override
    public void ToString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Ceiling");
    }
}
