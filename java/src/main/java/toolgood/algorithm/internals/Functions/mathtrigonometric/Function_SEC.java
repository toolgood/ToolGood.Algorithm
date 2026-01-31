package toolgood.algorithm.internals.functions.mathtrigonometric;

import java.lang.StringBuilder;
import java.util.function.Function;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.internals.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_1;
import toolgood.algorithm.internals.functions.FunctionBase;

class Function_SEC extends Function_1 {
    public Function_SEC(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand evaluate(AlgorithmEngine work, Function<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.evaluate(work, tempParameter);
        if (!args1.isNumber()) {
            args1 = args1.toNumber("Function '{0}' parameter is error!", "Sec");
            if (args1.isError()) {
                return args1;
            }
        }
        double value = args1.getDoubleValue();
        double cosValue = Math.cos(value);
        if (cosValue == 0) {
            return Operand.create(Double.POSITIVE_INFINITY);
        }
        return Operand.create(1.0 / cosValue);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        addFunction(stringBuilder, "Sec");
    }
}
