package toolgood.algorithm.internals.functions.mathbase;

import toolgood.algorithm.Internals.Operand;
import toolgood.algorithm.Internals.AlgorithmEngine;
import toolgood.algorithm.internals.FunctionBase;

public class Function_FLOOR extends Function_2 {
    public Function_FLOOR(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.Function<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Floor", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        if (func2 == null) {
            return Operand.Create(Math.floor(args1.NumberValue()));
        }

        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber()) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Floor", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        double b = args2.NumberValue();
        if (b >= 1) {
            return Operand.Create(args1.IntValue());
        }
        if (b <= 0) {
            return Operand.Error("Function '{0}' parameter {1} is error!", "Floor", 2);
        }

        double a = args1.NumberValue();
        double d = Math.floor(a / b) * b;
        return Operand.Create(d);
    }

    @Override
    public void toString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Floor");
    }
}
