package toolgood.algorithm.internals.functions.dateTimes;

import java.util.List;
import org.joda.time.DateTime;
import org.joda.time.DateTimeZone;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;

public final class Function_DAYS360 extends Function_3 {
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
            // 欧洲方法: 仅将 31 日按 30 日计算, 日号仍需正常参与天数差
            days += Math.min(endMyDate.getDayOfMonth(), 30) - Math.min(startMyDate.getDayOfMonth(), 30);
        } else {
            // US (NASD) 方法: start 若为月末(含 2 月最后一天)则调整为 30 日,
            // end 若为月末, 依据调整后的 startDay 决定按 31 日(下月1日)或 30 日计算
            int startDay = startMyDate.getDayOfMonth();
            int endDay = endMyDate.getDayOfMonth();
            if (startMyDate.getMonthOfYear() == 12) {
                if (startDay == new DateTime(startMyDate.getYear() + 1, 1, 1, 0, 0, 0, DateTimeZone.UTC).minusDays(1).getDayOfMonth()) {
                    startDay = 30;
                }
            } else {
                if (startDay == new DateTime(startMyDate.getYear(), startMyDate.getMonthOfYear() + 1, 1, 0, 0, 0, DateTimeZone.UTC).minusDays(1).getDayOfMonth()) {
                    startDay = 30;
                }
            }
            if (endMyDate.getMonthOfYear() == 12) {
                if (endDay == new DateTime(endMyDate.getYear() + 1, 1, 1, 0, 0, 0, DateTimeZone.UTC).minusDays(1).getDayOfMonth()) {
                    endDay = startDay < 30 ? 31 : 30;
                }
            } else {
                if (endDay == new DateTime(endMyDate.getYear(), endMyDate.getMonthOfYear() + 1, 1, 0, 0, 0, DateTimeZone.UTC).minusDays(1).getDayOfMonth()) {
                    endDay = startDay < 30 ? 31 : 30;
                }
            }
            days += endDay - startDay;
        }
        return Operand.Create(days);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.DATE);
        func2.GetParameterTypes(noneEngine, result, OperandType.DATE);
        if (func3 != null) {
            func3.GetParameterTypes(noneEngine, result, OperandType.BOOLEAN);
        }
    }
}
