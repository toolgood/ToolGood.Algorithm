package toolgood.algorithm.internals.functions.mathsum;



import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.mathNet.ExcelFunctions;


public class Function_POISSON extends Function_3 {
    public Function_POISSON(FunctionBase func1, FunctionBase func2, FunctionBase func3) {
        super(func1, func2, func3);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.getOperandType() != OperandType.Number) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Poisson", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.getOperandType() != OperandType.Number) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Poisson", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        Operand args3 = func3.Evaluate(work, tempParameter);
        if (args3.getOperandType() != OperandType.Boolean) {
            args3 = args3.ToBoolean("Function '{0}' parameter {1} is error!", "Poisson", 3);
            if (args3.IsError()) {
                return args3;
            }
        }
        int k = args1.IntValue();
        double lambda = args2.DoubleValue();
        boolean state = args3.BooleanValue();
        if (!(lambda > 0.0)) {
            return Operand.Error("Function '{0}' parameter is error!", "Poisson");
        }
        return Operand.Create(ExcelFunctions.Poisson(k, lambda, state));
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Poisson");
    }
}
