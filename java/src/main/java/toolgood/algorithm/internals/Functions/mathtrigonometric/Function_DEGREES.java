package toolgood.algorithm.internals.functions.mathtrigonometric;

import java.lang.StringBuilder;
import java.util.function.Function;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.internals.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_1;
import toolgood.algorithm.internals.functions.FunctionBase;

class Function_DEGREES extends Function_1 {
    public Function_DEGREES(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand evaluate(AlgorithmEngine work, Function<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.evaluate(work, tempParameter);
        if (!args1.isNumber()) {
            args1 = args1.toNumber("Function '{0}' parameter is error!", "Degrees");
            if (args1.isError()) {
                return args1;
            }
        }
        return Operand.create(Math.toDegrees(args1.getDoubleValue()));
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        addFunction(stringBuilder, "Degrees");
    }
}
