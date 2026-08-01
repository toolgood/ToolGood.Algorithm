package toolgood.algorithm.internals.functions.dateTimes;

import java.util.HashSet;
import java.util.List;
import org.joda.time.DateTime;
import org.joda.time.DateTimeZone;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;

public final class Function_NETWORKDAYS extends Function_N {
    public Function_NETWORKDAYS(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 2 || funcs.length > 3) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires 2 to 3 parameters.");
        }
    }

    @Override
    public String Name() {
        return "NetworkDays";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetDate(engine, tempParameter, 0);
        if (args1.IsErrorOrNone()) { return args1; }

        Operand args2 = GetDate(engine, tempParameter, 1);
        if (args2.IsErrorOrNone()) { return args2; }

        DateTime startMyDate = args1.DateValue().ToDateTime();
        DateTime endMyDate = args2.DateValue().ToDateTime();

        HashSet<DateTime> list = new HashSet<DateTime>();
        for (int i = 2; i < funcs.length; i++) {
            Operand ar = GetDate(engine, tempParameter, i);
            if (ar.IsErrorOrNone()) { return ar; }
            list.add(ar.DateValue().ToDateTime());
        }
        int days = 0;
        boolean negative = false;
        if (startMyDate.isAfter(endMyDate)) {
            DateTime tmp = startMyDate;
            startMyDate = endMyDate;
            endMyDate = tmp;
            negative = true;
        }
        // 数学统计工作日:完整周数 × 5 + 余数逐天,再减去落在工作日的节假日
        DateTime startDay = startMyDate.withTimeAtStartOfDay();
        DateTime endDay = endMyDate.withTimeAtStartOfDay();
        int totalDays = org.joda.time.Days.daysBetween(startDay, endDay).getDays() + 1;
        int fullWeeks = totalDays / 7;
        int remainder = totalDays % 7;
        days = fullWeeks * 5;
        for (int i = 0; i < remainder; i++) {
            DateTime d = startDay.plusDays(i);
            if (d.getDayOfWeek() != 7 && d.getDayOfWeek() != 6) {
                days++;
            }
        }
        for (DateTime h : list) {
            DateTime hd = h.withTimeAtStartOfDay();
            if (!hd.isBefore(startDay) && !hd.isAfter(endDay)
                    && hd.getDayOfWeek() != 7 && hd.getDayOfWeek() != 6) {
                days--;
            }
        }
        return Operand.Create(negative ? -days : days);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        for (int i = 0; i < funcs.length; i++) {
            funcs[i].GetParameterTypes(noneEngine, result, OperandType.DATE);
        }
    }
}
