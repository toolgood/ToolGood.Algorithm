package toolgood.algorithm.internals.functions.dateTimes;

import java.util.List;
import org.joda.time.DateTime;
import org.joda.time.DateTimeZone;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.MyDate;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_6;

public final class Function_DATE extends Function_6 {
    public Function_DATE(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 3 || funcs.length > 6) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires 3 to 6 parameters.");
        }
    }

    @Override
    public String Name() {
        return "Date";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }

        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) { return args2; }

        Operand args3 = GetNumber_3(engine, tempParameter);
        if (args3.IsErrorOrNone()) { return args3; }

        int year = args1.IntValue();
        int month = args2.IntValue();
        int day = args3.IntValue();

        DateTime baseDate;
        try {
            baseDate = new DateTime(year, 1, 1, 0, 0, 0, DateTimeZone.UTC);
            baseDate = baseDate.plusMonths(month - 1);
            baseDate = baseDate.plusDays(day - 1);
        } catch (Exception e) {
            return ParameterError(1);
        }

        int hour = 0, minute = 0, second = 0;
        if (func4 != null) {
            Operand args4 = GetNumber_4(engine, tempParameter);
            if (args4.IsErrorOrNone()) { return args4; }
            hour = args4.IntValue();
            if (hour < 0 || hour > 23) {
                return ParameterError(4);
            }
        }
        if (func5 != null) {
            Operand args5 = GetNumber_5(engine, tempParameter);
            if (args5.IsErrorOrNone()) { return args5; }
            minute = args5.IntValue();
            if (minute < 0 || minute > 59) {
                return ParameterError(5);
            }
        }
        if (func6 != null) {
            Operand args6 = GetNumber_6(engine, tempParameter);
            if (args6.IsErrorOrNone()) { return args6; }
            second = args6.IntValue();
            if (second < 0 || second > 59) {
                return ParameterError(6);
            }
        }

        MyDate d = new MyDate(baseDate.getYear(), baseDate.getMonthOfYear(), baseDate.getDayOfMonth(), hour, minute, second);
        return Operand.Create(d);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.DATE;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        if (func4 != null) func4.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        if (func5 != null) func5.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        if (func6 != null) func6.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
