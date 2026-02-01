package toolgood.algorithm.internals.functions.mathbase;

import java.lang.StringBuilder;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_1;

public class Function_SQRT extends Function_1 {
    public Function_SQRT(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter is error!", "Sqrt");
            if (args1.IsError()) {
                return args1;
            }
        }
        if (args1.DoubleValue() < 0) {
            return Operand.Error("Function '{0}' parameter is error!", "Sqrt");
        }
        return Operand.Create(Math.sqrt(args1.DoubleValue()));
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Sqrt");
    }
}
