package toolgood.algorithm.internals.functions.datetimes;

import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.function.Function;

public class Function_WEEKDAY extends Function_2 {
    public Function_WEEKDAY(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.isNotDate()) {
            args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "WeekDay", 1);
            if (args1.IsError()) {
                return args1;
            }
        }

        int type = 1;
        if (func2 != null) {
            Operand args2 = func2.Evaluate(work, tempParameter);
            if (args2.isNotNumber()) {
                args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "WeekDay", 2);
                if (args2.IsError()) {
                    return args2;
                }
            }
            type = args2.IntValue();
        }

        toolgood.algorithm.internals.MyDate date = args1.DateValue();
        int dayOfWeek = date.DayOfWeek(); // 1-7，其中 1 是星期日，7 是星期六

        if (type == 1) {
            // 返回 1-7，其中 1 是星期日，7 是星期六
            return Operand.Create(dayOfWeek);
        } else if (type == 2) {
            // 返回 1-7，其中 1 是星期一，7 是星期日
            if (dayOfWeek == 1) {
                return Operand.Create(7);
            }
            return Operand.Create(dayOfWeek - 1);
        } else {
            // 返回 0-6，其中 0 是星期一，6 是星期日
            if (dayOfWeek == 1) {
                return Operand.Create(6);
            }
            return Operand.Create(dayOfWeek - 2);
        }
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "WeekDay");
    }
}