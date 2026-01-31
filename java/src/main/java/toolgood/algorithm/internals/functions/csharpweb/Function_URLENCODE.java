package toolgood.algorithm.internals.functions.csharpweb;

import toolgood.algorithm.internals.Function_1;
import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.function.Function;

public class Function_URLENCODE extends Function_1 {
    public Function_URLENCODE(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter is error!", "UrlEncode");
            if (args1.IsError()) {
                return args1;
            }
        }
        try {
            String s = args1.getTextValue();
            String r = java.net.URLEncoder.encode(s, "UTF-8");
            return Operand.Create(r);
        } catch (Exception e) {
            // 捕获所有异常
        }
        return Operand.Error("Function '{0}' is error!", "UrlEncode");
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "UrlEncode");
    }
}