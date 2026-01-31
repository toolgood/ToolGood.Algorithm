package toolgood.algorithm.internals.functions.mathbase;

import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.internals.AlgorithmEngine;

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
        if (args1.getNumberValue() < 0) {
            return Operand.Error("Function '{0}' parameter is error!", "Sqrt");
        }
        return Operand.Create(Math.sqrt(args1.getDoubleValue()));
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Sqrt");
    }
}
