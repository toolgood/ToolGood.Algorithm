package toolgood.algorithm.internals.functions.mathsum;

import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.mathNet.ExcelFunctions;


public class Function_LOGINV extends Function_3 {
    public Function_LOGINV(FunctionBase func1, FunctionBase func2, FunctionBase func3) {
        super(func1, func2, func3);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.Function<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber
("Function '{0}' parameter {1} is error!", "LogInv", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber()) {
            args2 = args2.ToNumber
("Function '{0}' parameter {1} is error!", "LogInv", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        Operand args3 = func3.Evaluate(work, tempParameter);
        if (args3.IsNotNumber()) {
            args3 = args3.ToNumber
("Function '{0}' parameter {1} is error!", "LogInv", 3);
            if (args3.IsError()) {
                return args3;
            }
        }

        double n3 = args3.DoubleValue();
        if (n3 < 0.0) {
            return Operand.Error("Function '{0}' parameter is error!", "LogInv");
        }
        return Operand.Create(ExcelFunctions.LogInv(args1.DoubleValue(), args2.DoubleValue(), n3));
    }

    @Override
    public void toString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "LogInv");
    }
}
