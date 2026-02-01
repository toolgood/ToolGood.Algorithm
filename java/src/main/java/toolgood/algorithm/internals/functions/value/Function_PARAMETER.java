package toolgood.algorithm.internals.functions.value;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;

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
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiBiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        String txt = name;
        if (txt == null || txt.isEmpty()) {
            Operand args1 = func1.Evaluate(work, tempParameter);
            if (args1.IsNotText()) {
                args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Parameter", 1);
                if (args1.IsError()) {
                    return args1;
                }
            }
            txt = args1.TextValue();
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
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        if (name == null || name.isEmpty()) {
            func1.toString(stringBuilder, false);
        } else {
            stringBuilder.append(name);
        }
    }
}
