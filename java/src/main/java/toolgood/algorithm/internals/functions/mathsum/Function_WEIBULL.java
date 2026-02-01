package toolgood.algorithm.internals.functions.mathsum;



import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_4;
import toolgood.algorithm.mathNet.ExcelFunctions;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;


public class Function_WEIBULL extends Function_4 {
    public Function_WEIBULL(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) {
        super(func1, func2, func3, func4);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Weibull", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber()) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Weibull", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        Operand args3 = func3.Evaluate(work, tempParameter);
        if (args3.IsNotNumber()) {
            args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "Weibull", 3);
            if (args3.IsError()) {
                return args3;
            }
        }
        Operand args4 = func4.Evaluate(work, tempParameter);
        if (args4.IsNotBoolean()) {
            args4 = args4.ToBoolean("Function '{0}' parameter {1} is error!", "Weibull", 4);
            if (args4.IsError()) {
                return args4;
            }
        }

        double x = args1.DoubleValue();
        double shape = args2.DoubleValue();
        double scale = args3.DoubleValue();
        boolean state = args4.BooleanValue();

        if (shape <= 0.0 || scale <= 0.0) {
            return Operand.Error("Function '{0}' parameter is error!", "Weibull");
        }

        return Operand.Create(ExcelFunctions.WEIBULL(x, shape, scale, state));
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Weibull");
    }
}
