package toolgood.algorithm.internals.functions.datetimes;

import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;



public class Function_TIME extends Function_3 {
    public Function_TIME(FunctionBase func1, FunctionBase func2, FunctionBase func3) {
        super(func1, func2, func3);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Time", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber()) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Time", 2);
            if (args2.IsError()) {
                return args2;
            }
        }

        toolgood.algorithm.internals.MyDate d;
        if (func3 != null) {
            Operand args3 = func3.Evaluate(work, tempParameter);
            if (args3.IsNotNumber()) {
                args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "Time", 3);
                if (args3.IsError()) {
                    return args3;
                }
            }
            d = new toolgood.algorithm.internals.MyDate(0, 0, 0, args1.IntValue(), args2.IntValue(), args3.IntValue());
        } else {
            d = new toolgood.algorithm.internals.MyDate(0, 0, 0, args1.IntValue(), args2.IntValue(), 0);
        }
        return Operand.Create(d);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Time");
    }
}