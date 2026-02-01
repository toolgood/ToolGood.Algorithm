package toolgood.algorithm.internals.functions.mathbase;

import java.lang.StringBuilder;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.FunctionBase;

public class Function_ABS extends Function_1 {
    public Function_ABS(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiBiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.isNotNumber()) {
            args1 = args1.toNumber("Function '{0}' parameter is error!", "Abs");
            if (args1.isError()) {
                return args1;
            }
        }
        return Operand.create(Math.abs(args1.getNumberValue()));
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Abs");
    }
}
