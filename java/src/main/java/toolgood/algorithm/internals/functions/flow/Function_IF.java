package toolgood.algorithm.internals.functions.flow;

import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.FunctionBase;

public class Function_IF extends Function_3 {
    public Function_IF(FunctionBase func1, FunctionBase func2, FunctionBase func3) {
        super(func1, func2, func3);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotBoolean()) {
            args1 = args1.ToBoolean("Function '{0}' parameter {1} is error!", "If", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        if (args1.BooleanValue()) {
            return func2.Evaluate(work, tempParameter);
        }
        if (func3 == null) {
            return Operand.False();
        }
        return func3.Evaluate(work, tempParameter);
    }

    @Override
    public void toString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "IF");
    }
}
