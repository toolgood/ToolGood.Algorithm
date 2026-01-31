package toolgood.algorithm.internals.functions.datetimes;

import toolgood.algorithm.internals.Function_2;
import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.function.Function;

public class Function_WEEKNUM extends Function_2 {
    public Function_WEEKNUM(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.isNotDate()) {
            args1 = args1.toMyDate("Function '{0}' parameter {1} is error!", "WeekNum", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        toolgood.algorithm.internals.MyDate startMyDate = args1.getDateValue();

        int dayOfYear = startMyDate.DayOfYear();
        // 计算当年第一天是星期几（1-7，1 是星期日）
        toolgood.algorithm.internals.MyDate firstDayOfYear = new toolgood.algorithm.internals.MyDate(startMyDate.Year, 1, 1, 0, 0, 0);
        int firstDayOfWeek = firstDayOfYear.DayOfWeek();
        
        int days = dayOfYear + (firstDayOfWeek - 1); // 减 1 是因为 DayOfWeek 返回 1-7，而我们需要 0-6
        
        if (func2 != null) {
            Operand args2 = func2.Evaluate(work, tempParameter);
            if (args2.isNotNumber()) {
                args2 = args2.toNumber("Function '{0}' parameter {1} is error!", "WeekNum", 2);
                if (args2.IsError()) {
                    return args2;
                }
            }
            if (args2.getIntValue() == 2) {
                days--;
            }
        }

        double week = Math.ceil(days / 7.0);
        return Operand.Create((int) week);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "WeekNum");
    }
}