package toolgood.algorithm.internals.functions.mathsum;

import java.util.function.Function;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.mathNet.ExcelFunctions;


public class Function_TDIST extends Function_3 {
    public Function_TDIST(FunctionBase func1, FunctionBase func2, FunctionBase func3) {
        super(func1, func2, func3);
    }

    @Override
    public Operand Evaluate(Object work, Function<Object, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.getOperandType() != OperandType.Number) {
            args1 = args1.ToNumber
("Function '{0}' parameter {1} is error!", "TDist", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.getOperandType() != OperandType.Number) {
            args2 = args2.ToNumber
("Function '{0}' parameter {1} is error!", "TDist", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        Operand args3 = func3.Evaluate(work, tempParameter);
        if (args3.getOperandType() != OperandType.Number) {
            args3 = args3.ToNumber
("Function '{0}' parameter {1} is error!", "TDist", 3);
            if (args3.IsError()) {
                return args3;
            }
        }

        double x = args1.DoubleValue
();
        int degreesFreedom = args2.IntValue();
        int tails = args3.IntValue();

        if (degreesFreedom <= 0 || tails < 1 || tails > 2) {
            return Operand.Error("Function '{0}' parameter is error!", "TDist");
        }

        return Operand.Create(ExcelFunctions.TDist(x, degreesFreedom, tails));
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "TDist");
    }
}
