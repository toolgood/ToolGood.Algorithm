package toolgood.algorithm.internals.functions.mathtrigonometric;

import java.lang.StringBuilder;
import java.util.function.BiFunction;

import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_1;
import toolgood.algorithm.internals.functions.FunctionBase;

class Function_ACOTH extends Function_1 {
    public Function_ACOTH(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (!args1.IsNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter is error!", "Acoth");
            if (args1.IsError()) {
                return args1;
            }
        }
        double d = args1.DoubleValue();
        if (Math.abs(d) <= 1) {
            return ParameterError(1);
        }
        return Operand.Create(0.5 * Math.log((d + 1) / (d - 1)));
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Acoth");
    }
}
