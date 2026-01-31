package toolgood.algorithm.internals.functions.mathsum;

import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Function_1;
import toolgood.algorithm.internals.AlgorithmEngine;
import toolgood.algorithm.mathNet.ExcelFunctions;

public class Function_NORMSDIST extends Function_1 {
    public Function_NORMSDIST(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.Function<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter is error!", "NormSDist");
            if (args1.IsError()) {
                return args1;
            }
        }
        double num = args1.DoubleValue();
        return Operand.Create(ExcelFunctions.NormSDist(num));
    }

    @Override
    public void toString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "NormSDist");
    }
}
