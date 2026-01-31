package toolgood.algorithm.internals.functions.value;

import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.internals.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_2;

public class Function_PARAM extends Function_2 {
    public Function_PARAM(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Param", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        if (tempParameter != null) {
            Operand r = tempParameter.apply(work, args1.getTextValue());
            if (r != null) {
                return r;
            }
        }
        Operand result = work.getParameter(args1.getTextValue());
        if (result.IsError()) {
            if (func2 != null) {
                return func2.Evaluate(work, tempParameter);
            }
        }
        return result;
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Param");
    }
}
