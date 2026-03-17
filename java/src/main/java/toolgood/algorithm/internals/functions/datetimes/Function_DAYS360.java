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

public final class Function_DAYS360 extends Function_3 {
    public Function_DAYS360(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Days360";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetDate_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }

        Operand args2 = GetDate_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) {
            return args2;
        }

        MyDate startMyDate = args1.DateValue();
        MyDate endMyDate = args2.DateValue();

        boolean method = false;
        if (func3 != null) {
            Operand args3 = GetBoolean_3(engine, tempParameter);
            if (args3.IsErrorOrNone()) {
                return args3;
            }
            method = args3.BooleanValue();
        }

        int days = endMyDate.Year * 360 + (endMyDate.Month - 1) * 30
                - startMyDate.Year * 360 - (startMyDate.Month - 1) * 30;
        if (method) {
            if (endMyDate.Day == 31)
                days += 30;
            if (startMyDate.Day == 31)
                days -= 30;
        } else {
            if (startMyDate.Month == 12) {
                int daysInMonth = 31;
                if (startMyDate.Day == daysInMonth) {
                    days -= 30;
                } else {
                    days -= startMyDate.Day;
                }
            } else {
                int daysInMonth = getDaysInMonth(startMyDate.Year, startMyDate.Month);
                if (startMyDate.Day == daysInMonth) {
                    days -= 30;
                } else {
                    days -= startMyDate.Day;
                }
            }
            if (endMyDate.Month == 12) {
                int daysInMonth = 31;
                if (endMyDate.Day == daysInMonth) {
                    if (startMyDate.Day < 30) {
                        days += 31;
                    } else {
                        days += 30;
                    }
                } else {
                    days += endMyDate.Day;
                }
            } else {
                int daysInMonth = getDaysInMonth(endMyDate.Year, endMyDate.Month);
                if (endMyDate.Day == daysInMonth) {
                    if (startMyDate.Day < 30) {
                        days += 31;
                    } else {
                        days += 30;
                    }
                } else {
                    days += endMyDate.Day;
                }
            }
        }
        return Operand.Create(days);
    }

    private int getDaysInMonth(int year, int month) {
        switch (month) {
            case 1:
            case 3:
            case 5:
            case 7:
            case 8:
            case 10:
            case 12:
                return 31;
            case 4:
            case 6:
            case 9:
            case 11:
                return 30;
            case 2:
                if ((year % 4 == 0 && year % 100 != 0) || (year % 400 == 0)) {
                    return 29;
                } else {
                    return 28;
                }
            default:
                return 0;
        }
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.DATE);
        func2.GetParameterTypes(noneEngine, result, OperandType.DATE);
        if (func3 != null) {
            func3.GetParameterTypes(noneEngine, result, OperandType.BOOLEAN);
        }
    }
}
