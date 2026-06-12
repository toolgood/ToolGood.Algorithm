package toolgood.algorithm.internals.functions.dateTimes;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.List;
import org.joda.time.DateTime;
import org.joda.time.DateTimeZone;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;

final class Function_WEEKNUM extends Function_2 {
    public Function_WEEKNUM(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 1 || funcs.length > 2) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires 1 to 2 parameters.");
        }
    }

    @Override
    public String Name() {
        return "Weeknum";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetDate_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }

        DateTime startMyDate = args1.DateValue().ToDateTime();

        int returnType = 1;
        if (func2 != null) {
            Operand args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone()) { return args2; }
            returnType = args2.IntValue();
            if (returnType != 1 && returnType != 2 && returnType != 11 && returnType != 12 && returnType != 13
                    && returnType != 14 && returnType != 15 && returnType != 16 && returnType != 17) {
                return ParameterError(2);
            }
        }

        if (returnType == 21) {
            DateTime jan4 = new DateTime(startMyDate.getYear(), 1, 4, 0, 0, 0, DateTimeZone.UTC);
            int daysSinceJan4 = org.joda.time.Days.daysBetween(jan4, startMyDate).getDays();
            int dayOfWeekJan4 = jan4.getDayOfWeek() % 7;
            int mondayOffset = dayOfWeekJan4 == 0 ? 6 : dayOfWeekJan4 - 1;
            DateTime weekStart = jan4.minusDays(mondayOffset);
            int weekNumber = (daysSinceJan4 + mondayOffset) / 7 + 1;
            return Operand.Create(weekNumber);
        }

        DateTime jan1 = new DateTime(startMyDate.getYear(), 1, 1, 0, 0, 0, DateTimeZone.UTC);
        int dayOfYear = startMyDate.getDayOfYear();
        int dayOfWeekJan1 = jan1.getDayOfWeek() % 7;

        int weekStartDay;
        if (returnType == 1 || returnType == 17) {
            weekStartDay = 0;
        } else if (returnType == 2 || returnType == 11) {
            weekStartDay = 1;
        } else if (returnType == 12) {
            weekStartDay = 2;
        } else if (returnType == 13) {
            weekStartDay = 3;
        } else if (returnType == 14) {
            weekStartDay = 4;
        } else if (returnType == 15) {
            weekStartDay = 5;
        } else {
            weekStartDay = 6;
        }

        int daysUntilWeekStart = (dayOfWeekJan1 - weekStartDay + 7) % 7;
        int adjustedDayOfYear = dayOfYear + daysUntilWeekStart;
        int week = (int) Math.ceil(adjustedDayOfYear / 7.0);

        return Operand.Create(week);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.DATE);
        if (func2 != null) {
            func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        }
    }
}
