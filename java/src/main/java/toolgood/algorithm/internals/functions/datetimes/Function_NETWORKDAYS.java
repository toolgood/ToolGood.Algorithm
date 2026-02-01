package toolgood.algorithm.internals.functions.datetimes;

import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.HashSet;
import java.util.function.Function;

public class Function_NETWORKDAYS extends Function_N {
    public Function_NETWORKDAYS(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        Operand args1 = funcs[0].Evaluate(work, tempParameter);
        if (args1.isNotDate()) {
            args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "NetWorkdays", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = funcs[1].Evaluate(work, tempParameter);
        if (args2.isNotDate()) {
            args2 = args2.ToMyDate("Function '{0}' parameter {1} is error!", "NetWorkdays", 2);
            if (args2.IsError()) {
                return args2;
            }
        }

        toolgood.algorithm.internals.MyDate startMyDate = args1.DateValue();
        toolgood.algorithm.internals.MyDate endMyDate = args2.DateValue();

        HashSet<String> list = new HashSet<>();
        for (int i = 2; i < funcs.length; i++) {
            Operand ar = funcs[i].Evaluate(work, tempParameter);
            if (ar.isNotDate()) {
                ar = ar.ToMyDate("Function '{0}' parameter {1} is error!", "NetWorkdays", i + 1);
                if (ar.IsError()) {
                    return ar;
                }
            }
            toolgood.algorithm.internals.MyDate holiday = ar.DateValue();
            list.add(holiday.Year + "-" + holiday.Month + "-" + holiday.Day);
        }
        int days = 0;
        toolgood.algorithm.internals.MyDate currentDate = startMyDate;
        while (currentDate.ToDateTime().isBefore(endMyDate.ToDateTime()) || currentDate.ToDateTime().isEqual(endMyDate.ToDateTime())) {
            int dayOfWeek = currentDate.DayOfWeek();
            // 1-7，其中 1 是周日，7 是周六
            if (dayOfWeek != 1 && dayOfWeek != 7) {
                String dateStr = currentDate.Year + "-" + currentDate.Month + "-" + currentDate.Day;
                if (!list.contains(dateStr)) {
                    days++;
                }
            }
            currentDate = currentDate.AddDays(1);
        }
        return Operand.Create(days);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "NetWorkdays");
    }
}