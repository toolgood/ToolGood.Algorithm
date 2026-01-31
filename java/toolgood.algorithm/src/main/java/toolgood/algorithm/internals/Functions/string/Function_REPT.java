package toolgood.algorithm.internals.functions.string;

import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.internals.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_2;

public class Function_REPT extends Function_2 {
    public Function_REPT(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Rept", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber()) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Rept", 2);
            if (args2.IsError()) {
                return args2;
            }
        }

        String newtext = args1.getTextValue();
        int length = args2.getIntValue();
        if (length < 0) {
            return Operand.Error("Function '{0}' parameter {1} is error!", "Rept", 2);
        }
        if (length == 0) {
            return Operand.Create("");
        }
        StringBuilder sb = new StringBuilder(newtext.length() * length);
        for (int i = 0; i < length; i++) {
            sb.append(newtext);
        }
        return Operand.Create(sb.toString());
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Rept");
    }
}
