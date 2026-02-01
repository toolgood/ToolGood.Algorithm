package toolgood.algorithm.internals.functions.string;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_1;

public class Function_CODE extends Function_1 {
    public Function_CODE(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter is error!", "CODE");
            if (args1.IsError()) {
                return args1;
            }
        }
        if (args1.TextValue().isEmpty()) {
            return Operand.Error("Function '{0}' parameter is error!", "CODE");
        }
        char c = args1.TextValue().charAt(0);
        return Operand.Create((int) c);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "CODE");
    }
}
