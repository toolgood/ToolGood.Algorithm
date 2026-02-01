package toolgood.algorithm.internals.functions.operator;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_2;

public class Function_Connect extends Function_2 {
    public Function_Connect(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsError()) {
            return args1;
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsError()) {
            return args2;
        }

        if (args1.IsNull()) {
            if (args2.IsNull()) {
                return args1;
            }
            return args2.ToText("Function '{0}' parameter {1} is error!", "&", 2);
        } else if (args2.IsNull()) {
            return args1.ToText("Function '{0}' parameter {1} is error!", "&", 1);
        }
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter {1} is error!", "&", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        if (args2.IsNotText()) {
            args2 = args2.ToText("Function '{0}' parameter {1} is error!", "&", 2);
            if (args2.IsError()) {
                return args2;
            }
        }

        return Operand.Create(args1.TextValue() + args2.TextValue());
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        if (addBrackets) {
            stringBuilder.append('(');
        }
        func1.toString(stringBuilder, false);
        stringBuilder.append(" & ");
        func2.toString(stringBuilder, false);
        if (addBrackets) {
            stringBuilder.append(')');
        }
    }
}
