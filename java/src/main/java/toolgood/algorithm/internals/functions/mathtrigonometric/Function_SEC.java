package toolgood.algorithm.internals.functions.mathtrigonometric;

import java.lang.StringBuilder;
import java.util.function.Function;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_1;
import toolgood.algorithm.internals.functions.FunctionBase;

class Function_SEC extends Function_1 {
    public Function_SEC(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (!args1.IsNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter is error!", "Sec");
            if (args1.IsError()) {
                return args1;
            }
        }
        double value = args1.DoubleValue
();
        double cosValue = Math.cos(value);
        if (cosValue == 0) {
            return Operand.Create(Double.POSITIVE_INFINITY);
        }
        return Operand.Create(1.0 / cosValue);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Sec");
    }
}
