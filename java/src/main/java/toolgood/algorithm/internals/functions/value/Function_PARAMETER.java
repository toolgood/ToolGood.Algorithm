package toolgood.algorithm.internals.functions.value;

import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.internals.AlgorithmEngine;

public class Function_PARAMETER extends FunctionBase {
    private final String name;
    private final FunctionBase func1;

    public Function_PARAMETER(String name) {
        this.name = name;
        this.func1 = null;
    }

    public Function_PARAMETER(FunctionBase func1) {
        this.name = null;
        this.func1 = func1;
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        String txt = name;
        if (txt == null || txt.isEmpty()) {
            Operand args1 = func1.Evaluate(work, tempParameter);
            if (args1.IsNotText()) {
                args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Parameter", 1);
                if (args1.IsError()) {
                    return args1;
                }
            }
            txt = args1.getTextValue();
        }
        if (tempParameter != null) {
            Operand r = tempParameter.apply(work, txt);
            if (r != null) {
                return r;
            }
        }
        return work.getParameter(txt);
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        if (name == null || name.isEmpty()) {
            func1.ToString(stringBuilder, false);
        } else {
            stringBuilder.append(name);
        }
    }
}
