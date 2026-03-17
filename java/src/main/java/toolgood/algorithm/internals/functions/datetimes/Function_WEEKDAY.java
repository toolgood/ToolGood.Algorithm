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

public final class Function_WEEKDAY extends Function_2 {
    public Function_WEEKDAY(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Weekday";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetDate_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }

        int type = 1;
        if (func2 != null) {
            Operand args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone()) {
                return args2;
            }
            type = args2.IntValue();
            if (type != 1 && type != 2 && type != 3) {
                return ParameterError(2);
            }
        }

        int t = args1.DateValue().DayOfWeek();
        if (type == 1) {
            return Operand.Create(t);
        } else if (type == 2) {
            if (t == 0)
                return Operand.Create(7);
            return Operand.Create(t);
        }
        if (t == 0) {
            return Operand.Create(6);
        }
        return Operand.Create(t - 1);
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
