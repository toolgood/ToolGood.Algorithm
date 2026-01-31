package toolgood.algorithm.internals.functions.mathbase;

import toolgood.algorithm.Internals.Operand;
import toolgood.algorithm.Internals.AlgorithmEngine;
import toolgood.algorithm.internals.FunctionBase;

public class Function_QUOTIENT extends Function_2 {
    public Function_QUOTIENT(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.Function<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Quotient", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber()) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Quotient", 2);
            if (args2.IsError()) {
                return args2;
            }
        }

        if (args2.NumberValue() == 0) {
            return Operand.Error("Function '{0}' div 0 error!", "Quotient");
        }
        return Operand.Create((int)(args1.NumberValue() / args2.NumberValue()));
    }

    @Override
    public void ToString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Quotient");
    }
}
