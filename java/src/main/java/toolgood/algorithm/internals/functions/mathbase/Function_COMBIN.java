package toolgood.algorithm.internals.functions.mathbase;

import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;

public class Function_COMBIN extends Function_2 {
    public Function_COMBIN(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Combin", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber()) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Combin", 2);
            if (args2.IsError()) {
                return args2;
            }
        }

        int total = args1.IntValue();
        int count = args2.IntValue();
        if (total < 0 || count < 0 || total < count) {
            return Operand.Error("Function '{0}' parameter is error!", "Combin");
        }
        double sum = 1;
        double sum2 = 1;
        for (int i = 0; i < count; i++) {
            sum *= (total - i);
            sum2 *= (i + 1);
        }
        return Operand.Create(sum / sum2);
    }

    @Override
    public void toString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Combin");
    }
}
