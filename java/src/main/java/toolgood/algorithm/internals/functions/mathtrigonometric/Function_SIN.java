package toolgood.algorithm.internals.functions.mathtrigonometric;

import java.lang.StringBuilder;
import java.util.function.BiFunction;

import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_1;
import toolgood.algorithm.internals.functions.FunctionBase;

class Function_SIN extends Function_1 {
    public Function_SIN(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (!args1.IsNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter is error!", "Sin");
            if (args1.IsError()) {
                return args1;
            }
        }
        return Operand.Create(Math.sin(args1.DoubleValue()));
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Sin");
    }
}
