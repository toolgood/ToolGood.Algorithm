package toolgood.algorithm.internals.functions.datetimes;

import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.operands.MyDate;

public final class Function_EOMONTH extends Function_2 {
    public Function_EOMONTH(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "EOMonth";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetDate_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }

        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) {
            return args2;
        }

        MyDate dt = args1.DateValue().AddMonths(args2.IntValue() + 1);
        MyDate firstDayOfMonth = new MyDate(dt.Year, dt.Month, 1, 0, 0, 0);
        MyDate lastDayOfMonth = firstDayOfMonth.AddDays(-1);
        return Operand.Create(lastDayOfMonth);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.DATE;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.DATE);
        func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
