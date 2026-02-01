package toolgood.algorithm.internals.functions.mathbase;

import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.FunctionBase;

public class Function_EXP extends Function_1 {
    public Function_EXP(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.Function<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber
("Function '{0}' parameter is error!", "Exp");
            if (args1.IsError()) {
                return args1;
            }
        }
        return Operand.Create(Math.exp(args1.DoubleValue()));
    }

    @Override
    public void toString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Exp");
    }
}
