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

public final class Function_NETWORKDAYS extends Function_N {
    public Function_NETWORKDAYS(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "NetworkDays";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetDate(engine, tempParameter, 0);
        if (args1.IsErrorOrNone()) {
            return args1;
        }

        Operand args2 = GetDate(engine, tempParameter, 1);
        if (args2.IsErrorOrNone()) {
            return args2;
        }

        MyDate startMyDate = args1.DateValue();
        MyDate endMyDate = args2.DateValue();

        HashSet<String> list = new HashSet<>();
        for (int i = 2; i < funcs.length; i++) {
            Operand ar = GetDate(engine, tempParameter, i);
            if (ar.IsErrorOrNone()) {
                return ar;
            }
            MyDate holiday = ar.DateValue();
            list.add(holiday.Year + "-" + holiday.Month + "-" + holiday.Day);
        }

        int days = 0;
        MyDate currentDate = startMyDate;
        while (!currentDate.ToDateTime().after(endMyDate.ToDateTime())) {
            int dayOfWeek = currentDate.DayOfWeek();
            if (dayOfWeek != 1 && dayOfWeek != 7) {
                String dateStr = currentDate.Year + "-" + currentDate.Month + "-" + currentDate.Day;
                if (!list.contains(dateStr)) {
                    days++;
                }
            }
            currentDate = currentDate.AddDays(1);
        }
        return Operand.Create(days);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        for (FunctionBase item : funcs) {
            item.GetParameterTypes(noneEngine, result, OperandType.DATE);
        }
    }
}
