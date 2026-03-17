package toolgood.algorithm.internals.functions.datetimes;

import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.operands.MyDate;

public final class Function_TIME extends Function_3 {
    public Function_TIME(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Time";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }

        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) {
            return args2;
        }

        int hour = args1.IntValue();
        int minute = args2.IntValue();
        if (hour < 0 || hour > 23) {
            return ParameterError(1);
        }
        if (minute < 0 || minute > 59) {
            return ParameterError(2);
        }

        MyDate d;
        if (func3 != null) {
            Operand args3 = GetNumber_3(engine, tempParameter);
            if (args3.IsErrorOrNone()) {
                return args3;
            }
            int second = args3.IntValue();
            if (second < 0 || second > 59) {
                return ParameterError(3);
            }
            d = new MyDate(0, 0, 0, hour, minute, second);
        } else {
            d = new MyDate(0, 0, 0, hour, minute, 0);
        }
        return Operand.Create(d);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.DATE;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        if (func3 != null) {
            func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        }
    }
}
