package toolgood.algorithm.internals.functions.value;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_1;

public class Function_JSON extends Function_1 {
    public Function_JSON(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiBiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsError()) {
            return args1;
        }
        if (args1.IsJson()) {
            return args1;
        }
        if (args1.IsArrayJson()) {
            args1 = args1.ToText();
        }
        if (args1.IsNotText()) {
            return Operand.Error("Function '{0}' parameter is error!", "Json");
        }
        String txt = args1.TextValue();
        if ((txt.startsWith("{") && txt.endsWith("}")) || (txt.startsWith("[") && txt.endsWith("]"))) {
            try {
                var json = JsonMapper.ToObject(txt);
                return Operand.Create(json);
            } catch (Exception e) {
            }
        }
        return Operand.Error("Function '{0}' parameter is error!", "Json");
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Json");
    }
}
