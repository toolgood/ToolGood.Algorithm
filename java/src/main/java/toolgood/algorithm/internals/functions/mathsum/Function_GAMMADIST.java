package toolgood.algorithm.internals.functions.mathsum;

import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.Function_4;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.mathNet.ExcelFunctions;

public class Function_GAMMADIST extends Function_4 {
    public Function_GAMMADIST(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) {
        super(func1, func2, func3, func4);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.Function<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "GammaDist", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber()) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "GammaDist", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        Operand args3 = func3.Evaluate(work, tempParameter);
        if (args3.IsNotNumber()) {
            args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "GammaDist", 3);
            if (args3.IsError()) {
                return args3;
            }
        }
        Operand args4 = func4.Evaluate(work, tempParameter);
        if (args4.IsNotBoolean()) {
            args4 = args4.ToBoolean("Function '{0}' parameter {1} is error!", "GammaDist", 4);
            if (args4.IsError()) {
                return args4;
            }
        }
        double x = args1.DoubleValue();
        double alpha = args2.DoubleValue();
        double beta = args3.DoubleValue();
        boolean cumulative = args4.BooleanValue();
        if (alpha < 0.0 || beta < 0.0) {
            return Operand.Error("Function '{0}' parameter is error!", "GammaDist");
        }
        return Operand.Create(ExcelFunctions.GammaDist(x, alpha, beta, cumulative));
    }

    @Override
    public void toString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "GammaDist");
    }
}
