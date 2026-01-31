package toolgood.algorithm.internals.functions.flow;

import toolgood.algorithm.Internals.Operand;
import toolgood.algorithm.Internals.AlgorithmEngine;
import toolgood.algorithm.internals.FunctionBase;

public class Function_ISNULLORWHITESPACE extends Function_1 {
    public Function_ISNULLORWHITESPACE(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.Function<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNull()) {
            return Operand.True();
        }
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter {1} is error!", "IsNullOrWhiteSpace", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        return Operand.Create(args1.TextValue() == null || args1.TextValue().trim().isEmpty());
    }

    @Override
    public void ToString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "IsNullOrWhiteSpace");
    }
}
