package toolgood.algorithm.internals.functions.dateTimes;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.List;
import org.joda.time.DateTime;
import org.joda.time.DateTimeZone;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;

public final class Function_WEEKNUM extends Function_2 {
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
                    && returnType != 14 && returnType != 15 && returnType != 16 && returnType != 17 && returnType != 21) {
                return ParameterError(2);
            }
        }

        if (returnType == 21) {
            // ISO 8601: 第1周是包含当年第一个周四的周
            int isoDow = startMyDate.getDayOfWeek(); // Joda-Time: 1=周一...7=周日
            DateTime thursday = startMyDate.plusDays(4 - isoDow); // 本周的周四（与目标周同属一个 ISO 年）
            int weekNumber = (thursday.getDayOfYear() - 1) / 7 + 1;
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
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.DATE);
        if (func2 != null) {
            func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        }
    }
}
