package toolgood.algorithm.internals.functions.csharp;

import toolgood.algorithm.internals.functions.Function_4;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.function.Function;

public class Function_LASTINDEXOF extends Function_4 {
    public Function_LASTINDEXOF(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) {
        super(func1, func2, func3, func4);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter {1} is error!", "LastIndexOf", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotText()) {
            args2 = args2.ToText("Function '{0}' parameter {1} is error!", "LastIndexOf", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        String text = args1.TextValue();
        if (func3 == null) {
            return Operand.Create(text.lastIndexOf(args2.TextValue()) + work.getExcelIndex());
        }
        Operand args3 = func3.Evaluate(work, tempParameter);
        if (args3.IsNotNumber()) {
            args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "LastIndexOf", 3);
            if (args3.IsError()) {
                return args3;
            }
        }
        if (func4 == null) {
            int endIndex = args3.IntValue();
            if (endIndex < 0) {
                return Operand.Create(-1 + work.getExcelIndex());
            }
            endIndex = Math.min(endIndex, text.length());
            return Operand.Create(text.substring(0, endIndex).lastIndexOf(args2.TextValue()) + work.getExcelIndex());
        }
        Operand args4 = func4.Evaluate(work, tempParameter);
        if (args4.IsNotNumber()) {
            args4 = args4.ToNumber("Function '{0}' parameter {1} is error!", "LastIndexOf", 4);
            if (args4.IsError()) {
                return args4;
            }
        }
        int startIndex = args3.IntValue();
        int count = args4.IntValue();
        if (startIndex < 0) {
            return Operand.Create(-1 + work.getExcelIndex());
        }
        int endIndex = Math.min(startIndex + count, text.length());
        String substring = text.substring(0, endIndex);
        int result = substring.lastIndexOf(args2.TextValue());
        return Operand.Create(result + work.getExcelIndex());
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "LastIndexOf");
    }
}
