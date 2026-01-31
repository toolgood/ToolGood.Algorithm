package toolgood.algorithm.internals.functions.string;

import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.internals.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_4;

public class Function_SUBSTITUTE extends Function_4 {
    public Function_SUBSTITUTE(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) {
        super(func1, func2, func3, func4);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Substitute", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotText()) {
            args2 = args2.ToText("Function '{0}' parameter {1} is error!", "Substitute", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        Operand args3 = func3.Evaluate(work, tempParameter);
        if (args3.IsNotText()) {
            args3 = args3.ToText("Function '{0}' parameter {1} is error!", "Substitute", 3);
            if (args3.IsError()) {
                return args3;
            }
        }
        if (func4 == null) {
            return Operand.Create(args1.getTextValue().replace(args2.getTextValue(), args3.getTextValue()));
        }
        Operand args4 = func4.Evaluate(work, tempParameter);
        if (args4.IsNotNumber()) {
            args4 = args4.ToNumber("Function '{0}' parameter {1} is error!", "Substitute", 4);
            if (args4.IsError()) {
                return args4;
            }
        }
        String text = args1.getTextValue();
        String oldtext = args2.getTextValue();
        String newtext = args3.getTextValue();
        int index = args4.getIntValue();

        int index2 = 0;
        int estimatedCapacity = Math.max(text.length(), text.length() + (newtext.length() - oldtext.length()));
        StringBuilder sb = new StringBuilder(estimatedCapacity);
        for (int i = 0; i < text.length(); i++) {
            boolean b = true;
            for (int j = 0; j < oldtext.length(); j++) {
                if (i + j >= text.length()) {
                    b = false;
                    break;
                }
                char t = text.charAt(i + j);
                char t2 = oldtext.charAt(j);
                if (t != t2) {
                    b = false;
                    break;
                }
            }
            if (b) {
                index2++;
            }
            if (b && index2 == index) {
                sb.append(newtext);
                i += oldtext.length() - 1;
            } else {
                sb.append(text.charAt(i));
            }
        }
        return Operand.Create(sb.toString());
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Substitute");
    }
}
