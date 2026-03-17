package toolgood.algorithm.internals.functions.csharpweb;

import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_1;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_HTMLDECODE extends Function_1 {
    public Function_HTMLDECODE(FunctionBase func1) {
        super(func1);
    }

    @Override
    public String Name() {
        return "HtmlDecode";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetText_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }
        String s = args1.TextValue();
        String r = HtmlDecode(s);
        return Operand.Create(r);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.TEXT;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
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
