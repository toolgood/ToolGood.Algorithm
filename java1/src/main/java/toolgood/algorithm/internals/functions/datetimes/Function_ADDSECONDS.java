package toolgood.algorithm.internals.functions.datetimes;

import toolgood.algorithm.internals.Function_2;
import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.function.Function;

public class Function_ADDSECONDS extends Function_2 {
    public Function_ADDSECONDS(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.isNotDate()) {
            args1 = args1.toMyDate("Function '{0}' parameter {1} is error!", "AddSeconds", 1);
            if (args1.isError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.isNotNumber()) {
            args2 = args2.toNumber("Function '{0}' parameter {1} is error!", "AddSeconds", 2);
            if (args2.isError()) {
                return args2;
            }
        }
        try {
            toolgood.algorithm.internals.MyDate date = args1.getDateValue();
            int seconds = args2.getIntValue();
            toolgood.algorithm.internals.MyDate result = date.AddSeconds(seconds);
            return Operand.create(result);
        } catch (Exception e) {
            // 捕获所有异常
        }
        return Operand.error("Function '{0}' is error!", "AddSeconds");
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "AddSeconds");
    }
}