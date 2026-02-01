package toolgood.algorithm.internals.functions.mathbase;

import java.lang.StringBuilder;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.FunctionBase;

public class Function_SQRT extends Function_1 {
    public Function_SQRT(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.isNotNumber()) {
            args1 = args1.toNumber("Function '{0}' parameter is error!", "Sqrt");
            if (args1.isError()) {
                return args1;
            }
        }
        if (args1.getNumberValue() < 0) {
            return Operand.error("Function '{0}' parameter is error!", "Sqrt");
        }
        return Operand.create(Math.sqrt(args1.getDoubleValue()));
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Sqrt");
    }
}
