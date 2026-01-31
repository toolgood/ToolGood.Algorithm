package toolgood.algorithm.internals.functions.datetimes;

import toolgood.algorithm.internals.Function_3;
import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.function.Function;

public class Function_TIME extends Function_3 {
    public Function_TIME(FunctionBase func1, FunctionBase func2, FunctionBase func3) {
        super(func1, func2, func3);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.isNotNumber()) {
            args1 = args1.toNumber("Function '{0}' parameter {1} is error!", "Time", 1);
            if (args1.isError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.isNotNumber()) {
            args2 = args2.toNumber("Function '{0}' parameter {1} is error!", "Time", 2);
            if (args2.isError()) {
                return args2;
            }
        }

        toolgood.algorithm.internals.MyDate d;
        if (func3 != null) {
            Operand args3 = func3.Evaluate(work, tempParameter);
            if (args3.isNotNumber()) {
                args3 = args3.toNumber("Function '{0}' parameter {1} is error!", "Time", 3);
                if (args3.isError()) {
                    return args3;
                }
            }
            d = new toolgood.algorithm.internals.MyDate(0, 0, 0, args1.getIntValue(), args2.getIntValue(), args3.getIntValue());
        } else {
            d = new toolgood.algorithm.internals.MyDate(0, 0, 0, args1.getIntValue(), args2.getIntValue(), 0);
        }
        return Operand.create(d);
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Time");
    }
}