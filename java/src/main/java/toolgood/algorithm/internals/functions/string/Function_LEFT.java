package toolgood.algorithm.internals.functions.string;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_2;

public class Function_LEFT extends Function_2 {
    public Function_LEFT(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiBiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Left", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        if (args1.TextValue().length() == 0) {
            return Operand.Create("");
        }
        if (func2 == null) {
            return Operand.Create(args1.TextValue().substring(0, 1));
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber()) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Left", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        int length = Math.min(args2.IntValue(), args1.TextValue().length());
        return Operand.Create(args1.TextValue().substring(0, length));
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Left");
    }
}
