package toolgood.algorithm.internals.functions.mathsum;

import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.mathNet.ExcelFunctions;


public class Function_FDIST extends Function_3 {
    public Function_FDIST(FunctionBase func1, FunctionBase func2, FunctionBase func3) {
        super(func1, func2, func3);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.Function<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "FDist", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber()) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "FDist", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        Operand args3 = func3.Evaluate(work, tempParameter);
        if (args3.IsNotNumber()) {
            args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "FDist", 3);
            if (args3.IsError()) {
                return args3;
            }
        }

        double x = args1.NumberValue();
        int degreesFreedom1 = args2.IntValue();
        int degreesFreedom2 = args3.IntValue();
        if (degreesFreedom1 <= 0 || degreesFreedom2 <= 0) {
            return Operand.Error("Function '{0}' parameter is error!", "FDist");
        }
        return Operand.Create(ExcelFunctions.FDist(x, degreesFreedom1, degreesFreedom2));
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "FDist");
    }
}
