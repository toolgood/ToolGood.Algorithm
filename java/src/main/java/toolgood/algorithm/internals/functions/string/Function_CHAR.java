package toolgood.algorithm.internals.functions.string;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_1;

public class Function_CHAR extends Function_1 {
    public Function_CHAR(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter is error!", "Char");
            if (args1.IsError()) {
                return args1;
            }
        }
        char c = (char) args1.IntValue();
        return Operand.Create(String.valueOf(c));
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Char");
    }
}
