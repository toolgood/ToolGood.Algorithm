package toolgood.algorithm.internals.functions.mathbase;

import toolgood.algorithm.Internals.Operand;
import toolgood.algorithm.Internals.AlgorithmEngine;
import toolgood.algorithm.internals.FunctionBase;

public class Function_MROUND extends Function_2 {
    public Function_MROUND(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.Function<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "MRound", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber()) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "MRound", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        double a = args2.NumberValue();
        if (a <= 0) {
            return Operand.Error("Function '{0}' parameter {1} is error!", "MRound", 2);
        }

        double b = args1.NumberValue();
        double quotient = b / a;
        double roundedQuotient = Math.round(quotient);
        double r = roundedQuotient * a;
        return Operand.Create(r);
    }

    @Override
    public void toString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "MRound");
    }
}
