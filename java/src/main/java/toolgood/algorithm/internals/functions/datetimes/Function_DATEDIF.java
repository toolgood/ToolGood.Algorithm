package toolgood.algorithm.internals.functions.datetimes;

import java.util.Calendar;
import java.util.GregorianCalendar;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.operands.MyDate;

public final class Function_DATEDIF extends Function_3 {
    public Function_DATEDIF(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "DateDif";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetDate_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }

        Operand args2 = GetDate_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) {
            return args2;
        }

        Operand args3 = GetText_3(engine, tempParameter);
        if (args3.IsErrorOrNone()) {
            return args3;
        }

        MyDate startMyDate = args1.DateValue();
        MyDate endMyDate = args2.DateValue();
        String t = args3.TextValue();

        if (t.equalsIgnoreCase("Y")) {
            boolean b = false;
            if (startMyDate.Month < endMyDate.Month) {
                b = true;
            } else if (startMyDate.Month == endMyDate.Month) {
                if (startMyDate.Day <= endMyDate.Day)
                    b = true;
            }
            if (b) {
                return Operand.Create(endMyDate.Year - startMyDate.Year);
            } else {
                return Operand.Create(endMyDate.Year - startMyDate.Year - 1);
            }
        } else if (t.equalsIgnoreCase("M")) {
            boolean b = false;
            if (startMyDate.Day <= endMyDate.Day)
                b = true;
            if (b) {
                return Operand.Create(endMyDate.Year * 12 + endMyDate.Month - startMyDate.Year * 12 - startMyDate.Month);
            } else {
                return Operand.Create(endMyDate.Year * 12 + endMyDate.Month - startMyDate.Year * 12 - startMyDate.Month - 1);
            }
        } else if (t.equalsIgnoreCase("D")) {
            long days = endMyDate.ToDateTime().getTime() - startMyDate.ToDateTime().getTime();
            days = days / (1000 * 60 * 60 * 24);
            return Operand.Create((int) days);
        } else if (t.equalsIgnoreCase("YD")) {
            int startDayOfYear = startMyDate.DayOfYear();
            int endDayOfYear = endMyDate.DayOfYear();
            int day = endDayOfYear - startDayOfYear;
            if (endMyDate.Year > startMyDate.Year && day < 0) {
                int days = isLeapYear(startMyDate.Year) ? 366 : 365;
                day = days + day;
            }
            return Operand.Create(day);
        } else if (t.equalsIgnoreCase("MD")) {
            int mo = endMyDate.Day - startMyDate.Day;
            if (mo < 0) {
                int days = getDaysInMonth(startMyDate.Year, startMyDate.Month);
                mo += days;
            }
            return Operand.Create(mo);
        } else if (t.equalsIgnoreCase("YM")) {
            int mo = endMyDate.Month - startMyDate.Month;
            if (endMyDate.Day < startMyDate.Day)
                mo--;
            if (mo < 0)
                mo += 12;
            return Operand.Create(mo);
        }
        return ParameterError(3);
    }

    private boolean isLeapYear(int year) {
        return (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
    }

    private int getDaysInMonth(int year, int month) {
        Calendar cal = new GregorianCalendar(year, month - 1, 1);
        return cal.getActualMaximum(Calendar.DAY_OF_MONTH);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.DATE);
        func2.GetParameterTypes(noneEngine, result, OperandType.DATE);
        func3.GetParameterTypes(noneEngine, result, OperandType.TEXT);
    }
}
