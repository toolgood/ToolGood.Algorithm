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

public final class Function_WEEKNUM extends Function_2 {
    public Function_WEEKNUM(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Weeknum";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetDate_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }

        MyDate startMyDate = args1.DateValue();

        int days = startMyDate.DayOfYear() + (int) (new MyDate(startMyDate.Year, 1, 1, 0, 0, 0).DayOfWeek());
        if (func2 != null) {
            Operand args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone()) {
                return args2;
            }
            if (args2.IntValue() != 1 && args2.IntValue() != 2) {
                return ParameterError(2);
            }
            if (args2.IntValue() == 2) {
                days--;
            }
        }

        double week = Math.ceil(days / 7.0);
        return Operand.Create((int) week);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.DATE);
        if (func2 != null) {
            func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        }
    }
}
