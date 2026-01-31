package toolgood.algorithm.internals.functions.datetimes;

import toolgood.algorithm.internals.Function_3;
import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.function.Function;

public class Function_DAYS360 extends Function_3 {
    public Function_DAYS360(FunctionBase func1, FunctionBase func2, FunctionBase func3) {
        super(func1, func2, func3);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.isNotDate()) {
            args1 = args1.toMyDate("Function '{0}' parameter {1} is error!", "Days360", 1);
            if (args1.isError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.isNotDate()) {
            args2 = args2.toMyDate("Function '{0}' parameter {1} is error!", "Days360", 2);
            if (args2.isError()) {
                return args2;
            }
        }

        toolgood.algorithm.internals.MyDate startMyDate = args1.getDateValue();
        toolgood.algorithm.internals.MyDate endMyDate = args2.getDateValue();

        boolean method = false;
        if (func3 != null) {
            Operand args3 = func3.Evaluate(work, tempParameter);
            if (args3.isNotBoolean()) {
                args3 = args3.toBoolean("Function '{0}' parameter {1} is error!", "Days360", 3);
                if (args3.isError()) {
                    return args3;
                }
            }
            if (args3.isError()) {
                return args3;
            }
            method = args3.getBooleanValue();
        }
        int days = endMyDate.Year * 360 + (endMyDate.Month - 1) * 30
                    - startMyDate.Year * 360 - (startMyDate.Month - 1) * 30;
        if (method) {
            if (endMyDate.Day == 31) days += 30;
            if (startMyDate.Day == 31) days -= 30;
        } else {
            if (startMyDate.Month == 12) {
                // 获取12月的天数
                int daysInMonth = 31;
                if (startMyDate.Day == daysInMonth) {
                    days -= 30;
                } else {
                    days -= startMyDate.Day;
                }
            } else {
                // 获取当月的天数
                int daysInMonth = getDaysInMonth(startMyDate.Year, startMyDate.Month);
                if (startMyDate.Day == daysInMonth) {
                    days -= 30;
                } else {
                    days -= startMyDate.Day;
                }
            }
            if (endMyDate.Month == 12) {
                // 获取12月的天数
                int daysInMonth = 31;
                if (endMyDate.Day == daysInMonth) {
                    if (startMyDate.Day < 30) {
                        days += 31;
                    } else {
                        days += 30;
                    }
                } else {
                    days += endMyDate.Day;
                }
            } else {
                // 获取当月的天数
                int daysInMonth = getDaysInMonth(endMyDate.Year, endMyDate.Month);
                if (endMyDate.Day == daysInMonth) {
                    if (startMyDate.Day < 30) {
                        days += 31;
                    } else {
                        days += 30;
                    }
                } else {
                    days += endMyDate.Day;
                }
            }
        }
        return Operand.Create(days);
    }

    private int getDaysInMonth(int year, int month) {
        switch (month) {
            case 1: case 3: case 5: case 7: case 8: case 10: case 12:
                return 31;
            case 4: case 6: case 9: case 11:
                return 30;
            case 2:
                // 检查是否是闰年
                if ((year % 4 == 0 && year % 100 != 0) || (year % 400 == 0)) {
                    return 29;
                } else {
                    return 28;
                }
            default:
                return 0;
        }
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Days360");
    }
}