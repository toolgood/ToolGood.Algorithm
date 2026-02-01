package toolgood.algorithm.internals.functions.mathtransformation;

import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.FunctionBase;

public class Function_DEC2OCT extends Function_2 {
    public Function_DEC2OCT(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.Function<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber
("Function '{0}' parameter {1} is error!", "DEC2OCT", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        String num = Integer.toOctalString(args1.IntValue());
        if (func2 != null) {
            Operand args2 = func2.Evaluate(work, tempParameter);
            if (args2.IsNotNumber()) {
                args2 = args2.ToNumber
("Function '{0}' parameter {1} is error!", "DEC2OCT", 2);
                if (args2.IsError()) {
                    return args2;
                }
            }
            if (num.length() <= args2.IntValue()) {
                return Operand.Create(num);
            }
            return Operand.Error("Function '{0}' parameter {1} is error!", "DEC2OCT", 2);
        }
        return Operand.Create(num);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "DEC2OCT");
    }
}
