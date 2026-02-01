package toolgood.algorithm.internals.functions.csharp;

import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.function.Function;

public class Function_REMOVEEND extends Function_3 {
    public Function_REMOVEEND(FunctionBase func1, FunctionBase func2, FunctionBase func3) {
        super(func1, func2, func3);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter {1} is error!", "RemoveEnd", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotText()) {
            args2 = args2.ToText("Function '{0}' parameter {1} is error!", "RemoveEnd", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        boolean ignoreCase = false;
        if (func3 != null) {
            Operand args3 = func3.Evaluate(work, tempParameter);
            if (args3.IsNotBoolean()) {
                args3 = args3.ToBoolean("Function '{0}' parameter {1} is error!", "RemoveEnd", 3);
                if (args3.IsError()) {
                    return args3;
                }
            }
            if (args3.BooleanValue()) {
                ignoreCase = true;
            }
        }
        String text = args1.TextValue();
        String suffix = args2.TextValue();
        boolean endsWith = false;
        if (ignoreCase) {
            endsWith = text.toLowerCase().endsWith(suffix.toLowerCase());
        } else {
            endsWith = text.endsWith(suffix);
        }
        if (endsWith) {
            return Operand.Create(text.substring(0, text.length() - suffix.length()));
        }
        return args1;
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "RemoveEnd");
    }
}
