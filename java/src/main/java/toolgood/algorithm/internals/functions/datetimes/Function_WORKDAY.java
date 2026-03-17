package toolgood.algorithm.internals.functions.datetimes;

import java.util.HashSet;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.operands.MyDate;

public final class Function_WORKDAY extends Function_N {
    public Function_WORKDAY(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Workday";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetDate(engine, tempParameter, 0);
        if (args1.IsErrorOrNone()) {
            return args1;
        }

        Operand args2 = GetNumber(engine, tempParameter, 1);
        if (args2.IsErrorOrNone()) {
            return args2;
        }

        MyDate startMyDate = args1.DateValue();
        int days = args2.IntValue();
        HashSet<String> list = new HashSet<>();

        for (int i = 2; i < funcs.length; i++) {
            Operand ar = GetDate(engine, tempParameter, i);
            if (ar.IsErrorOrNone()) {
                return ar;
            }
            MyDate holiday = ar.DateValue();
            list.add(holiday.Year + "-" + holiday.Month + "-" + holiday.Day);
        }

        while (days > 0) {
            startMyDate = startMyDate.AddDays(1);
            int dayOfWeek = startMyDate.DayOfWeek();
            if (dayOfWeek == 1 || dayOfWeek == 7) {
                continue;
            }
            String dateStr = startMyDate.Year + "-" + startMyDate.Month + "-" + startMyDate.Day;
            if (list.contains(dateStr)) {
                continue;
            }
            days--;
        }
        return Operand.Create(startMyDate);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.DATE;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        funcs[0].GetParameterTypes(noneEngine, result, OperandType.DATE);
        funcs[1].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        for (int i = 2; i < funcs.length; i++) {
            funcs[i].GetParameterTypes(noneEngine, result, OperandType.DATE);
        }
    }
}
