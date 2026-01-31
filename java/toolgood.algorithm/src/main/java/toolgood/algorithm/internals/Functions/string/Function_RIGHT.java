package toolgood.algorithm.internals.functions.string;

import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.internals.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_2;

public class Function_RIGHT extends Function_2 {
    public Function_RIGHT(FunctionBase func1) {
        super(func1, null);
    }

    public Function_RIGHT(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Right", 1);
            if (args1.IsError()) {
                return args1;
            }
        }

        if (args1.getTextValue().length() == 0) {
            return Operand.Create("");
        }
        if (func2 == null) {
            return Operand.Create(args1.getTextValue().substring(args1.getTextValue().length() - 1));
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber()) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Right", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        int length = Math.min(args2.getIntValue(), args1.getTextValue().length());
        int start = args1.getTextValue().length() - length;
        return Operand.Create(args1.getTextValue().substring(start));
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Right");
    }
}
