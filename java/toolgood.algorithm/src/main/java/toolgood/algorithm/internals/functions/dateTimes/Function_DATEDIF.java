package toolgood.algorithm.internals.functions.dateTimes;

import java.util.List;
import org.joda.time.DateTime;
import org.joda.time.DateTimeZone;
import org.joda.time.Days;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;

final class Function_DATEDIF extends Function_3 {
    public Function_DATEDIF(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 3) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 3 parameters.");
        }
    }

    @Override
    public String Name() {
        return "DateDif";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetDate_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }

        Operand args2 = GetDate_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) { return args2; }

        Operand args3 = GetText_3(engine, tempParameter);
        if (args3.IsErrorOrNone()) { return args3; }
        DateTime startMyDate = args1.DateValue().ToDateTime();
        DateTime endMyDate = args2.DateValue().ToDateTime();
        String t = args3.TextValue();

        if (t.equalsIgnoreCase("Y")) {
            boolean b = false;
            if (startMyDate.getMonthOfYear() < endMyDate.getMonthOfYear()) {
                b = true;
            } else if (startMyDate.getMonthOfYear() == endMyDate.getMonthOfYear()) {
                if (startMyDate.getDayOfMonth() <= endMyDate.getDayOfMonth()) b = true;
            }
            if (b) {
                return Operand.Create((endMyDate.getYear() - startMyDate.getYear()));
            } else {
                return Operand.Create((endMyDate.getYear() - startMyDate.getYear() - 1));
            }
        } else if (t.equalsIgnoreCase("M")) {
            boolean b = false;
            if (startMyDate.getDayOfMonth() <= endMyDate.getDayOfMonth()) b = true;
            if (b) {
                return Operand.Create((endMyDate.getYear() * 12 + endMyDate.getMonthOfYear() - startMyDate.getYear() * 12 - startMyDate.getMonthOfYear()));
            } else {
                return Operand.Create((endMyDate.getYear() * 12 + endMyDate.getMonthOfYear() - startMyDate.getYear() * 12 - startMyDate.getMonthOfYear() - 1));
            }
        } else if (t.equalsIgnoreCase("D")) {
            return Operand.Create(Days.daysBetween(startMyDate, endMyDate).getDays());
        } else if (t.equalsIgnoreCase("YD")) {
            int day = endMyDate.getDayOfYear() - startMyDate.getDayOfYear();
            if (endMyDate.getYear() > startMyDate.getYear() && day < 0) {
                int daysInYear = new DateTime(startMyDate.getYear(), 12, 31, 0, 0, 0, DateTimeZone.UTC).getDayOfYear();
                day = daysInYear + day;
            }
            return Operand.Create((day));
        } else if (t.equalsIgnoreCase("MD")) {
            int mo = endMyDate.getDayOfMonth() - startMyDate.getDayOfMonth();
            if (mo < 0) {
                int daysInMonth;
                if (startMyDate.getMonthOfYear() == 12) {
                    daysInMonth = new DateTime(startMyDate.getYear() + 1, 1, 1, 0, 0, 0, DateTimeZone.UTC).minusDays(1).getDayOfMonth();
                } else {
                    daysInMonth = new DateTime(startMyDate.getYear(), startMyDate.getMonthOfYear() + 1, 1, 0, 0, 0, DateTimeZone.UTC).minusDays(1).getDayOfMonth();
                }
                mo += daysInMonth;
            }
            return Operand.Create((mo));
        } else if (t.equalsIgnoreCase("YM")) {
            int mo = endMyDate.getMonthOfYear() - startMyDate.getMonthOfYear();
            if (endMyDate.getDayOfMonth() < startMyDate.getDayOfMonth()) mo--;
            if (mo < 0) mo += 12;
            return Operand.Create((mo));
        }
        return ParameterError(3);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.DATE);
        func2.GetParameterTypes(noneEngine, result, OperandType.DATE);
        func3.GetParameterTypes(noneEngine, result, OperandType.TEXT);
    }
}
