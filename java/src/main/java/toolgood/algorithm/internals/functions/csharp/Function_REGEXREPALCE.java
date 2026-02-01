package toolgood.algorithm.internals.functions.csharp;

import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.function.Function;
import java.util.regex.Pattern;

public class Function_REGEXREPALCE extends Function_3 {
    public Function_REGEXREPALCE(FunctionBase func1, FunctionBase func2, FunctionBase func3) {
        super(func1, func2, func3);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter {1} is error!", "RegexReplace", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotText()) {
            args2 = args2.ToText("Function '{0}' parameter {1} is error!", "RegexReplace", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        Operand args3 = func3.Evaluate(work, tempParameter);
        if (args3.IsNotText()) {
            args3 = args3.ToText("Function '{0}' parameter {1} is error!", "RegexReplace", 3);
            if (args3.IsError()) {
                return args3;
            }
        }
        String result = Pattern.compile(args2.getTextValue()).matcher(args1.getTextValue()).replaceAll(args3.getTextValue());
        return Operand.Create(result);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "RegexReplace");
    }
}
