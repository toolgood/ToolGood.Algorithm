package toolgood.algorithm.internals.functions.csharp;

import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.function.Function;

public class Function_STARTSWITH extends Function_3 {
    public Function_STARTSWITH(FunctionBase func1, FunctionBase func2, FunctionBase func3) {
        super(func1, func2, func3);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter {1} is error!", "StartsWith", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotText()) {
            args2 = args2.ToText("Function '{0}' parameter {1} is error!", "StartsWith", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        String text = args1.TextValue();
        if (func3 == null) {
            return Operand.Create(text.startsWith(args2.TextValue()));
        }
        Operand args3 = func3.Evaluate(work, tempParameter);
        if (args3.IsNotBoolean()) {
            args3 = args3.ToBoolean("Function '{0}' parameter {1} is error!", "StartsWith", 3);
            if (args3.IsError()) {
                return args3;
            }
        }
        if (args3.BooleanValue()) {
            return Operand.Create(text.toLowerCase().startsWith(args2.TextValue().toLowerCase()));
        } else {
            return Operand.Create(text.startsWith(args2.TextValue()));
        }
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "StartsWith");
    }
}
