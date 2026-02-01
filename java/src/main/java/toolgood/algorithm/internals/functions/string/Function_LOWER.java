package toolgood.algorithm.internals.functions.string;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_1;

public class Function_LOWER extends Function_1 {
    public Function_LOWER(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter is error!", "Lower");
            if (args1.IsError()) {
                return args1;
            }
        }
        return Operand.Create(args1.TextValue().toLowerCase());
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Lower");
    }
}
