package toolgood.algorithm.internals.functions.csharp;

import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.function.Function;

public class Function_SUBSTRING extends Function_3 {
    public Function_SUBSTRING(FunctionBase func1, FunctionBase func2, FunctionBase func3) {
        super(func1, func2, func3);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Substring", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber()) {
            args2 = args2.ToNumber
("Function '{0}' parameter {1} is error!", "Substring", 2);
            if (args2.IsError()) {
                return args2;
            }
        }

        String text = args1.TextValue();
        int startIndex = args2.IntValue() - work.getExcelIndex();
        if (startIndex < 0) {
            startIndex = 0;
        }
        if (startIndex >= text.length()) {
            return Operand.Create("");
        }
        if (func3 == null) {
            return Operand.Create(text.substring(startIndex));
        }
        Operand args3 = func3.Evaluate(work, tempParameter);
        if (args3.IsNotNumber()) {
            args3 = args3.ToNumber
("Function '{0}' parameter {1} is error!", "Substring", 3);
            if (args3.IsError()) {
                return args3;
            }
        }
        int length = args3.IntValue();
        int endIndex = startIndex + length;
        if (endIndex > text.length()) {
            endIndex = text.length();
        }
        return Operand.Create(text.substring(startIndex, endIndex));
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Substring");
    }
}
