package toolgood.algorithm.internals.functions.mathsum;

import java.util.function.Function;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.mathNet.ExcelFunctions;


public class Function_TINV extends Function_2 {
    public Function_TINV(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(Object work, Function<Object, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.Type() != OperandType.Number) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "TInv", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.Type() != OperandType.Number) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "TInv", 2);
            if (args2.IsError()) {
                return args2;
            }
        }

        double p = args1.DoubleValue
();
        int degreesFreedom = args2.IntValue();

        if (degreesFreedom <= 0 || p < 0.0 || p > 1.0) {
            return Operand.Error("Function '{0}' parameter is error!", "TInv");
        }

        return Operand.Create(ExcelFunctions.TInv(p, degreesFreedom));
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "TInv");
    }
}
