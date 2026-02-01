package toolgood.algorithm.internals.functions.datetimes;

import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.function.Function;

public class Function_DATEDIF extends Function_3 {
    public Function_DATEDIF(FunctionBase func1, FunctionBase func2, FunctionBase func3) {
        super(func1, func2, func3);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.isNotDate()) {
            args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "DateDif", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.isNotDate()) {
            args2 = args2.ToMyDate("Function '{0}' parameter {1} is error!", "DateDif", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        Operand args3 = func3.Evaluate(work, tempParameter);
        if (args3.isNotText()) {
            args3 = args3.ToText("Function '{0}' parameter {1} is error!", "DateDif", 3);
            if (args3.IsError()) {
                return args3;
            }
        }
        toolgood.algorithm.internals.MyDate startMyDate = args1.DateValue();
        toolgood.algorithm.internals.MyDate endMyDate = args2.DateValue();
        String t = args3.TextValue().toLowerCase();

        if (t.equals("y")) {
            // 计算年差
            boolean b = false;
            if (startMyDate.Month < endMyDate.Month) {
                b = true;
            } else if (startMyDate.Month == endMyDate.Month) {
                if (startMyDate.Day <= endMyDate.Day) b = true;
            }
            if (b) {
                return Operand.Create(endMyDate.Year - startMyDate.Year);
            } else {
                return Operand.Create(endMyDate.Year - startMyDate.Year - 1);
            }
        } else if (t.equals("m")) {
            // 计算月差
            boolean b = false;
            if (startMyDate.Day <= endMyDate.Day) b = true;
            if (b) {
                return Operand.Create(endMyDate.Year * 12 + endMyDate.Month - startMyDate.Year * 12 - startMyDate.Month);
            } else {
                return Operand.Create(endMyDate.Year * 12 + endMyDate.Month - startMyDate.Year * 12 - startMyDate.Month - 1);
            }
        } else if (t.equals("d")) {
            // 计算日差
            long days = endMyDate.ToDateTime().getMillis() - startMyDate.ToDateTime().getMillis();
            days = days / (1000 * 60 * 60 * 24);
            return Operand.Create((int) days);
        } else if (t.equals("yd")) {
            // 计算年内日差
            int startDayOfYear = startMyDate.DayOfYear();
            int endDayOfYear = endMyDate.DayOfYear();
            int day = endDayOfYear - startDayOfYear;
            if (endMyDate.Year > startMyDate.Year && day < 0) {
                // 获取 startMyDate 所在年份的天数
                int days = startMyDate.ToDateTime().dayOfYear().withMaximumValue().getDayOfYear();
                day = days + day;
            }
            return Operand.Create(day);
        } else if (t.equals("md")) {
            // 计算月内日差
            int mo = endMyDate.Day - startMyDate.Day;
            if (mo < 0) {
                int days;
                if (startMyDate.Month == 12) {
                    days = startMyDate.ToDateTime().withMonthOfYear(12).dayOfMonth().withMaximumValue().getDayOfMonth();
                } else {
                    days = startMyDate.ToDateTime().withMonthOfYear(startMyDate.Month).dayOfMonth().withMaximumValue().getDayOfMonth();
                }
                mo += days;
            }
            return Operand.Create(mo);
        } else if (t.equals("ym")) {
            // 计算年内地差
            int mo = endMyDate.Month - startMyDate.Month;
            if (endMyDate.Day < startMyDate.Day) mo--;
            if (mo < 0) mo += 12;
            return Operand.Create(mo);
        }
        return Operand.Error("Function '{0}' parameter {1} is error!", "DateDif", 3);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "DateDif");
    }
}