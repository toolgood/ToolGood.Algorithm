package toolgood.algorithm.internals.functions.mathbase;

import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.internals.AlgorithmEngine;

public class Function_TRUNC extends Function_1 {
    public Function_TRUNC(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter is error!", "Trunc");
            if (args1.IsError()) {
                return args1;
            }
        }
        return Operand.Create((int) args1.getIntValue());
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Trunc");
    }
}
