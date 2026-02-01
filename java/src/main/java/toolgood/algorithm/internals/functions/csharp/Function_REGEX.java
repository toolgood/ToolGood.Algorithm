package toolgood.algorithm.internals.functions.csharp;

import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.function.Function;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class Function_REGEX extends Function_2 {
    public Function_REGEX(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Regex", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotText()) {
            args2 = args2.ToText("Function '{0}' parameter {1} is error!", "Regex", 2);
            if (args2.IsError()) {
                return args2;
            }
        }

        Pattern pattern = Pattern.compile(args2.TextValue());
        Matcher matcher = pattern.matcher(args1.TextValue());
        if (!matcher.find()) {
            return Operand.Error("Function '{0}' is error!", "Regex");
        }
        return Operand.Create(matcher.group());
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Regex");
    }
}
