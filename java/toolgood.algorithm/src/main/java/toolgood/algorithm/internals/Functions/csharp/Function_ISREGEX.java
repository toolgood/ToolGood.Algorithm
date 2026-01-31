package toolgood.algorithm.internals.functions.csharp;

import toolgood.algorithm.internals.Function_2;
import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.function.Function;
import java.util.regex.Pattern;

public class Function_ISREGEX extends Function_2 {
    public Function_ISREGEX(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter {1} is error!", "IsRegex", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotText()) {
            args2 = args2.ToText("Function '{0}' parameter {1} is error!", "IsRegex", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        boolean b = Pattern.matches(args2.getTextValue(), args1.getTextValue());
        return Operand.Create(b);
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "IsRegex");
    }
}
