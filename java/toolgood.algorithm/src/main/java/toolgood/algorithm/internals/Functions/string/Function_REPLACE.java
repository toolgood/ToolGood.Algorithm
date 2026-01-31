package toolgood.algorithm.internals.functions.string;

import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.internals.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_4;

public class Function_REPLACE extends Function_4 {
    public Function_REPLACE(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) {
        super(func1, func2, func3, func4);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Replace", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        String oldtext = args1.getTextValue();
        if (func4 == null) {
            Operand args22 = func2.Evaluate(work, tempParameter);
            if (args22.IsNotText()) {
                args22 = args22.ToText("Function '{0}' parameter {1} is error!", "Replace", 2);
                if (args22.IsError()) {
                    return args22;
                }
            }
            Operand args32 = func3.Evaluate(work, tempParameter);
            if (args32.IsNotText()) {
                args32 = args32.ToText("Function '{0}' parameter {1} is error!", "Replace", 3);
                if (args32.IsError()) {
                    return args32;
                }
            }

            String old = args22.getTextValue();
            String newstr = args32.getTextValue();
            return Operand.Create(oldtext.replace(old, newstr));
        }

        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber()) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Replace", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        Operand args3 = func3.Evaluate(work, tempParameter);
        if (args3.IsNotNumber()) {
            args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "Replace", 3);
            if (args3.IsError()) {
                return args3;
            }
        }
        Operand args4 = func4.Evaluate(work, tempParameter);
        if (args4.IsNotText()) {
            args4 = args4.ToText("Function '{0}' parameter {1} is error!", "Replace", 4);
            if (args4.IsError()) {
                return args4;
            }
        }

        int start = args2.getIntValue() - work.getExcelIndex();
        int length = args3.getIntValue();
        String newtext = args4.getTextValue();

        StringBuilder sb = new StringBuilder(oldtext.length() - length + newtext.length());
        for (int i = 0; i < oldtext.length(); i++) {
            if (i < start) {
                sb.append(oldtext.charAt(i));
            } else if (i == start) {
                sb.append(newtext);
            } else if (i >= start + length) {
                sb.append(oldtext.charAt(i));
            }
        }
        return Operand.Create(sb.toString());
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Replace");
    }
}
