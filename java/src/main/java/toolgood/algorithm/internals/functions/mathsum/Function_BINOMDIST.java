package toolgood.algorithm.internals.functions.mathsum;

import toolgood.algorithm.internals.functions.Function_4;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.mathNet.ExcelFunctions;


public class Function_BINOMDIST extends Function_4 {
    public Function_BINOMDIST(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) {
        super(func1, func2, func3, func4);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "BinomDist", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber()) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "BinomDist", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        Operand args3 = func3.Evaluate(work, tempParameter);
        if (args3.IsNotNumber()) {
            args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "BinomDist", 3);
            if (args3.IsError()) {
                return args3;
            }
        }
        Operand args4 = func4.Evaluate(work, tempParameter);
        if (!args4.IsBoolean()) {
            args4 = args4.ToBoolean("Function '{0}' parameter {1} is error!", "BinomDist", 4);
            if (args4.IsError()) {
                return args4;
            }
        }

        int n2 = args2.IntValue();
        double n3 = args3.DoubleValue();
        if (!(n3 >= 0.0 && n3 <= 1.0 && n2 >= 0)) {
            return Operand.Error("Function '{0}' parameter is error!", "BinomDist");
        }
        return Operand.Create(ExcelFunctions.BinomDist(args1.IntValue(), n2, n3, args4.BooleanValue()));
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "BinomDist");
    }
}
