package toolgood.algorithm.internals.functions.mathtrigonometric;

import java.lang.StringBuilder;
import java.util.function.Function;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.internals.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_1;
import toolgood.algorithm.internals.functions.FunctionBase;

class Function_COT extends Function_1 {
    public Function_COT(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (!args1.IsNumber()) {
            args1 = args1.toNumber("Function '{0}' parameter is error!", "Cot");
            if (args1.IsError()) {
                return args1;
            }
        }
        double value = args1.getDoubleValue();
        double tanValue = Math.tan(value);
        if (tanValue == 0) {
            return Operand.Create(Double.POSITIVE_INFINITY);
        }
        return Operand.Create(1.0 / tanValue);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Cot");
    }
}
