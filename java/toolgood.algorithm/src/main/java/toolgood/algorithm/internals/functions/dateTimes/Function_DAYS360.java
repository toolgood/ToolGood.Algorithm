package toolgood.algorithm.internals.functions.dateTimes;

import java.util.List;
import org.joda.time.DateTime;
import org.joda.time.DateTimeZone;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;

final class Function_DAYS360 extends Function_3 {
    public Function_DAYS360(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 2 || funcs.length > 3) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires 2 to 3 parameters.");
        }
    }

    @Override
    public String Name() {
        return "Days360";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetDate_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }

        Operand args2 = GetDate_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) { return args2; }

        DateTime startMyDate = args1.DateValue().ToDateTime();
        DateTime endMyDate = args2.DateValue().ToDateTime();

        boolean method = false;
        if (func3 != null) {
            Operand args3 = GetBoolean_3(engine, tempParameter);
            if (args3.IsErrorOrNone()) { return args3; }
            method = args3.BooleanValue();
        }
        int days = endMyDate.getYear() * 360 + (endMyDate.getMonthOfYear() - 1) * 30
                    - startMyDate.getYear() * 360 - (startMyDate.getMonthOfYear() - 1) * 30;
        if (method) {
            if (endMyDate.getDayOfMonth() == 31) days += 30;
            if (startMyDate.getDayOfMonth() == 31) days -= 30;
        } else {
            if (startMyDate.getMonthOfYear() == 12) {
                if (startMyDate.getDayOfMonth() == new DateTime(startMyDate.getYear() + 1, 1, 1, 0, 0, 0, DateTimeZone.UTC).minusDays(1).getDayOfMonth()) {
                    days -= 30;
                } else {
                    days -= startMyDate.getDayOfMonth();
                }
            } else {
                if (startMyDate.getDayOfMonth() == new DateTime(startMyDate.getYear(), startMyDate.getMonthOfYear() + 1, 1, 0, 0, 0, DateTimeZone.UTC).minusDays(1).getDayOfMonth()) {
                    days -= 30;
                } else {
                    days -= startMyDate.getDayOfMonth();
                }
            }
            if (endMyDate.getMonthOfYear() == 12) {
                if (endMyDate.getDayOfMonth() == new DateTime(endMyDate.getYear() + 1, 1, 1, 0, 0, 0, DateTimeZone.UTC).minusDays(1).getDayOfMonth()) {
                    if (startMyDate.getDayOfMonth() < 30) {
                        days += 31;
                    } else {
                        days += 30;
                    }
                } else {
                    days += endMyDate.getDayOfMonth();
                }
            } else {
                if (endMyDate.getDayOfMonth() == new DateTime(endMyDate.getYear(), endMyDate.getMonthOfYear() + 1, 1, 0, 0, 0, DateTimeZone.UTC).minusDays(1).getDayOfMonth()) {
                    if (startMyDate.getDayOfMonth() < 30) {
                        days += 31;
                    } else {
                        days += 30;
                    }
                } else {
                    days += endMyDate.getDayOfMonth();
                }
            }
        }
        return Operand.Create(days);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.DATE);
        func2.GetParameterTypes(noneEngine, result, OperandType.DATE);
        if (func3 != null) {
            func3.GetParameterTypes(noneEngine, result, OperandType.BOOLEAN);
        }
    }
}
