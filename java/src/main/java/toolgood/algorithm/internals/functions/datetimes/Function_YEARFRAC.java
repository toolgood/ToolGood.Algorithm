package toolgood.algorithm.internals.functions.datetimes;

import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.MyDate;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;

public class Function_YEARFRAC extends Function_3 {
    public Function_YEARFRAC(FunctionBase func1, FunctionBase func2, FunctionBase func3) {
        super(func1, func2, func3);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        if (func1 == null || func2 == null) {
            return ParameterError(1);
        }

        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotDate()) {
            args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "YEARFRAC", 1);
            if (args1.IsError()) return args1;
        }

        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotDate()) {
            args2 = args2.ToMyDate("Function '{0}' parameter {1} is error!", "YEARFRAC", 2);
            if (args2.IsError()) return args2;
        }

        int basis = 0;
        if (func3 != null) {
            Operand args3 = func3.Evaluate(work, tempParameter);
            if (!args3.IsNumber()) {
                args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "YEARFRAC", 3);
                if (args3.IsError()) return args3;
            }
            basis = (int) args3.DoubleValue();
            if (basis < 0 || basis > 4) {
                return ParameterError(3);
            }
        }

        MyDate startMyDate = args1.DateValue();
        MyDate endMyDate = args2.DateValue();

        double result = calculateYearFrac(startMyDate, endMyDate, basis);
        return Operand.Create(result);
    }

    private double calculateYearFrac(MyDate startDate, MyDate endDate, int basis) {
        // 确保 startDate <= endDate
        if (startDate.ToDateTime().isAfter(endDate.ToDateTime())) {
            MyDate temp = startDate;
            startDate = endDate;
            endDate = temp;
        }

        switch (basis) {
            case 0:
                return calculate30_360(startDate, endDate);
            case 1:
                return calculateActualActual(startDate, endDate);
            case 2:
                return daysBetween(startDate, endDate) / 360.0;
            case 3:
                return daysBetween(startDate, endDate) / 365.0;
            case 4:
                return calculate30_360E(startDate, endDate);
            default:
                return calculate30_360(startDate, endDate);
        }
    }

    private double calculate30_360(MyDate startDate, MyDate endDate) {
        int d1 = Math.min(30, startDate.Day);
        int d2 = endDate.Day;
        if (d1 == 30) d2 = Math.min(30, d2);
        return (360.0 * (endDate.Year - startDate.Year)
                + 30.0 * (endDate.Month - startDate.Month)
                + (d2 - d1)) / 360.0;
    }

    private double calculate30_360E(MyDate startDate, MyDate endDate) {
        int d1 = Math.min(30, startDate.Day);
        int d2 = Math.min(30, endDate.Day);
        return (360.0 * (endDate.Year - startDate.Year)
                + 30.0 * (endDate.Month - startDate.Month)
                + (d2 - d1)) / 360.0;
    }

    private double calculateActualActual(MyDate startDate, MyDate endDate) {
        int startYear = startDate.Year;
        int endYear = endDate.Year;

        if (startYear == endYear) {
            int daysInYear = isLeapYear(startYear) ? 366 : 365;
            return daysBetween(startDate, endDate) / (double) daysInYear;
        }

        int daysInStartYear = isLeapYear(startYear) ? 366 : 365;
        int daysInEndYear = isLeapYear(endYear) ? 366 : 365;

        // �?startDate 到当年年�?
        MyDate endOfStartYear = new MyDate(startYear, 12, 31, 0, 0, 0);
        double result = daysBetween(startDate, endOfStartYear) / (double) daysInStartYear;

        // �?endYear 年初�?endDate
        MyDate startOfEndYear = new MyDate(endYear, 1, 1, 0, 0, 0);
        result += daysBetween(startOfEndYear, endDate) / (double) daysInEndYear;

        // 中间完整年份
        result += endYear - startYear - 1;

        return result;
    }

    private long daysBetween(MyDate d1, MyDate d2) {
        long millis1 = d1.ToDateTime().withTimeAtStartOfDay().getMillis();
        long millis2 = d2.ToDateTime().withTimeAtStartOfDay().getMillis();
        return (millis2 - millis1) / (1000L * 60 * 60 * 24);
    }

    private boolean isLeapYear(int year) {
        return (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "YEARFRAC");
    }
}
