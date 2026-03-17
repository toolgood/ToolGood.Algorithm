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

public final class Function_DATEVALUE extends Function_2 {
    public Function_DATEVALUE(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "DateValue";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        if (func1 == null)
            return ParameterError(1);

        Operand args1 = func1.Evaluate(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }
        if (args1.IsDate()) {
            return args1;
        }

        int type = 0;
        if (func2 != null) {
            Operand args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone()) {
                return args2;
            }
            type = args2.IntValue();
        }
        if (type == 0) {
            if (args1.IsText()) {
                MyDate date = MyDate.parse(args1.TextValue());
                if (date != null) {
                    return Operand.Create(date);
                }
            }
            Operand arg1 = ConvertToNumber(args1, 1);
            if (arg1.IsErrorOrNone()) {
                return arg1;
            }
            if (arg1.LongValue() <= 2958465L) {
                return arg1.ToMyDate();
            }
            if (arg1.LongValue() <= 253402232399L) {
                return Operand.Create(new MyDate(arg1.LongValue() / 86400.0));
            }
            return Operand.Create(new MyDate(arg1.LongValue() / (86400.0 * 1000.0)));
        } else if (type == 1) {
            Operand arg1 = ConvertToText(args1, 1);
            if (arg1.IsErrorOrNone()) {
                return arg1;
            }
            MyDate date = MyDate.parse(arg1.TextValue());
            if (date != null) {
                return Operand.Create(date);
            }
        } else if (type == 2) {
            Operand arg1 = ConvertToNumber(args1, 1);
            if (arg1.IsErrorOrNone()) {
                return arg1;
            }
            return arg1.ToMyDate();
        } else if (type == 3) {
            Operand arg1 = ConvertToNumber(args1, 1);
            if (arg1.IsErrorOrNone()) {
                return arg1;
            }
            return Operand.Create(new MyDate(arg1.LongValue() / (86400.0 * 1000.0)));
        } else if (type == 4) {
            Operand arg1 = ConvertToNumber(args1, 1);
            if (arg1.IsErrorOrNone()) {
                return arg1;
            }
            return Operand.Create(new MyDate(arg1.LongValue() / 86400.0));
        }
        return ParameterError(1);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.DATE;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NONE);
        if (func2 != null)
            func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
