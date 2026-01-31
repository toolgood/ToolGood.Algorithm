package toolgood.algorithm.internals.functions.datetimes;

import toolgood.algorithm.internals.Function_N;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.HashSet;
import java.util.function.Function;

public class Function_WORKDAY extends Function_N {
    public Function_WORKDAY(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        Operand args1 = funcs[0].Evaluate(work, tempParameter);
        if (args1.isNotDate()) {
            args1 = args1.toMyDate("Function '{0}' parameter {1} is error!", "Workday", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = funcs[1].Evaluate(work, tempParameter);
        if (args2.isNotNumber()) {
            args2 = args2.toNumber("Function '{0}' parameter {1} is error!", "Workday", 2);
            if (args2.IsError()) {
                return args2;
            }
        }

        toolgood.algorithm.internals.MyDate startMyDate = args1.getDateValue();
        int days = args2.getIntValue();
        HashSet<String> holidaySet = new HashSet<>();
        
        for (int i = 2; i < funcs.length; i++) {
            Operand ar = funcs[i].Evaluate(work, tempParameter);
            if (ar.isNotDate()) {
                ar = ar.toMyDate("Function '{0}' parameter {1} is error!", "Workday", i + 1);
                if (ar.IsError()) {
                    return ar;
                }
            }
            toolgood.algorithm.internals.MyDate holiday = ar.getDateValue();
            holidaySet.add(holiday.Year + "-" + holiday.Month + "-" + holiday.Day);
        }
        
        while (days > 0) {
            startMyDate = startMyDate.AddDays(1);
            int dayOfWeek = startMyDate.DayOfWeek();
            // 检查是否是周末（1 是星期日，7 是星期六）
            if (dayOfWeek == 1 || dayOfWeek == 7) {
                continue;
            }
            // 检查是否是节假日
            String dateStr = startMyDate.Year + "-" + startMyDate.Month + "-" + startMyDate.Day;
            if (holidaySet.contains(dateStr)) {
                continue;
            }
            days--;
        }
        return Operand.Create(startMyDate);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Workday");
    }
}