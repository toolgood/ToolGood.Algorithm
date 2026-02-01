package toolgood.algorithm.internals.functions.csharp;

import toolgood.algorithm.internals.functions.Function_4;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;



public class Function_INDEXOF extends Function_4 {
    public Function_INDEXOF(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) {
        super(func1, func2, func3, func4);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter {1} is error!", "IndexOf", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotText()) {
            args2 = args2.ToText("Function '{0}' parameter {1} is error!", "IndexOf", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        String text = args1.TextValue();
        if (func3 == null) {
            return Operand.Create(text.indexOf(args2.TextValue()) + work.ExcelIndex);
        }
        Operand args3 = func3.Evaluate(work, tempParameter);
        if (args3.IsNotNumber()) {
            args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "IndexOf", 3);
            if (args3.IsError()) {
                return args3;
            }
        }
        if (func4 == null) {
            int startIndex = args3.IntValue();
            if (startIndex >= text.length()) {
                return Operand.Create(-1 + work.ExcelIndex);
            }
            return Operand.Create(text.substring(startIndex).indexOf(args2.TextValue()) + startIndex + work.ExcelIndex);
        }
        Operand args4 = func4.Evaluate(work, tempParameter);
        if (args4.IsNotNumber()) {
            args4 = args4.ToNumber("Function '{0}' parameter {1} is error!", "IndexOf", 4);
            if (args4.IsError()) {
                return args4;
            }
        }
        int startIndex = args3.IntValue();
        int count = args4.IntValue();
        if (startIndex >= text.length()) {
            return Operand.Create(-1 + work.ExcelIndex);
        }
        int endIndex = Math.min(startIndex + count, text.length());
        String substring = text.substring(startIndex, endIndex);
        int result = substring.indexOf(args2.TextValue());
        if (result == -1) {
            return Operand.Create(-1 + work.ExcelIndex);
        }
        return Operand.Create(result + startIndex + work.ExcelIndex);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "IndexOf");
    }
}
