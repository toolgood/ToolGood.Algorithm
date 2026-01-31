package toolgood.algorithm.internals.functions.csharpweb;

import toolgood.algorithm.internals.Function_1;
import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.function.Function;

public class Function_HTMLDECODE extends Function_1 {
    public Function_HTMLDECODE(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter is error!", "HtmlDecode");
            if (args1.IsError()) {
                return args1;
            }
        }
        String s = args1.getTextValue();
        String r = HtmlDecode(s);
        return Operand.Create(r);
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "HtmlDecode");
    }

    private String HtmlDecode(String value) {
        if (value == null) {
            return "";
        }
        return value.replace("&lt;", "<")
                   .replace("&gt;", ">")
                   .replace("&amp;", "&")
                   .replace("&quot;", "\"")
                   .replace("&#39;", "'");
    }
}