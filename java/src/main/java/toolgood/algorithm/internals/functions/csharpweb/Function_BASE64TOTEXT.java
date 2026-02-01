package toolgood.algorithm.internals.functions.csharpweb;

import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.function.Function;

public class Function_BASE64TOTEXT extends Function_2 {
    public Function_BASE64TOTEXT(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Base64ToText", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        try {
            java.nio.charset.Charset charset;
            if (func2 == null) {
                charset = java.nio.charset.StandardCharsets.UTF_8;
            } else {
                Operand args2 = func2.Evaluate(work, tempParameter);
                if (args2.IsNotText()) {
                    args2 = args2.ToText("Function '{0}' parameter {1} is error!", "Base64ToText", 2);
                    if (args2.IsError()) {
                        return args2;
                    }
                }
                charset = java.nio.charset.Charset.forName(args2.TextValue());
            }
            byte[] bytes = Base64.FromBase64String(args1.TextValue());
            String t = new String(bytes, charset);
            return Operand.Create(t);
        } catch (Exception e) {
            // 捕获所有异常
        }
        return Operand.Error("Function '{0}' is error!", "Base64ToText");
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Base64ToText");
    }
}
