package toolgood.algorithm.internals.functions.datetimes;

import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_6;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.operands.MyDate;

public final class Function_DATE extends Function_6 {
    public Function_DATE(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Date";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }

        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) {
            return args2;
        }

        Operand args3 = GetNumber_3(engine, tempParameter);
        if (args3.IsErrorOrNone()) {
            return args3;
        }

        int year = args1.IntValue();
        int month = args2.IntValue();
        int day = args3.IntValue();

        if (month < 1 || month > 12) {
            return ParameterError(2);
        }
        if (day < 1 || day > 31) {
            return ParameterError(3);
        }

        MyDate d;
        if (func4 == null) {
            d = new MyDate(year, month, day, 0, 0, 0);
        } else if (func5 == null) {
            Operand args4 = GetNumber_4(engine, tempParameter);
            if (args4.IsErrorOrNone()) {
                return args4;
            }
            int hour = args4.IntValue();
            if (hour < 0 || hour > 23) {
                return ParameterError(4);
            }
            d = new MyDate(year, month, day, hour, 0, 0);
        } else if (func6 == null) {
            Operand args4 = GetNumber_4(engine, tempParameter);
            if (args4.IsErrorOrNone()) {
                return args4;
            }

            Operand args5 = GetNumber_5(engine, tempParameter);
            if (args5.IsErrorOrNone()) {
                return args5;
            }

            int hour = args4.IntValue();
            int minute = args5.IntValue();
            if (hour < 0 || hour > 23) {
                return ParameterError(4);
            }
            if (minute < 0 || minute > 59) {
                return ParameterError(5);
            }
            d = new MyDate(year, month, day, hour, minute, 0);
        } else {
            Operand args4 = GetNumber_4(engine, tempParameter);
            if (args4.IsErrorOrNone()) {
                return args4;
            }

            Operand args5 = GetNumber_5(engine, tempParameter);
            if (args5.IsErrorOrNone()) {
                return args5;
            }

            Operand args6 = GetNumber_6(engine, tempParameter);
            if (args6.IsErrorOrNone()) {
                return args6;
            }

            int hour = args4.IntValue();
            int minute = args5.IntValue();
            int second = args6.IntValue();
            if (hour < 0 || hour > 23) {
                return ParameterError(4);
            }
            if (minute < 0 || minute > 59) {
                return ParameterError(5);
            }
            if (second < 0 || second > 59) {
                return ParameterError(6);
            }
            d = new MyDate(year, month, day, hour, minute, second);
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
        func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        if (func4 != null)
            func4.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        if (func5 != null)
            func5.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        if (func6 != null)
            func6.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
