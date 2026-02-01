package toolgood.algorithm.internals.functions.mathtrigonometric;

import java.lang.StringBuilder;

import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_1;
import toolgood.algorithm.internals.functions.FunctionBase;

class Function_TANH extends Function_1 {
    public Function_TANH(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (!args1.IsNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter is error!", "Tanh");
            if (args1.IsError()) {
                return args1;
            }
        }
        return Operand.Create(Math.tanh(args1.DoubleValue
()));
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Tanh");
    }
}
