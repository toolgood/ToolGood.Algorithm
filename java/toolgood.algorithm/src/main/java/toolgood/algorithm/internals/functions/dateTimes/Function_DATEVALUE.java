package toolgood.algorithm.internals.functions.dateTimes;

import java.util.List;
import org.joda.time.DateTime;
import org.joda.time.DateTimeZone;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.MyDate;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.NoneEngine;

final class Function_DATEVALUE extends Function_2 {
    public Function_DATEVALUE(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 1 || funcs.length > 2) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires 1 to 2 parameters.");
        }
    }

    @Override
    public String Name() {
        return "DateValue";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }
        if (args1.IsDate()) { return args1; }

        int type = 0;
        if (func2 != null) {
            Operand args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone()) { return args2; }
            type = args2.IntValue();
        }
        if (type == 0) {
            if (args1.IsText()) {
                MyDate md = MyDate.parse(args1.TextValue());
                if (md != null) {
                    return Operand.Create(md);
                }
            }
            Operand arg1 = ConvertToNumber(args1, 1);
            if (arg1.IsErrorOrNone()) { return arg1; }
            if (arg1.LongValue() <= 2958465L) {
                return arg1.ToDate(null);
            }
            if (arg1.LongValue() <= 253402232399L) {
                DateTime time = new DateTime(FunctionUtil.StartDateUtc.getMillis() + arg1.LongValue() * 1000, DateTimeZone.UTC);
                if (engine.UseLocalTime) {
                    return Operand.Create(time.toDateTime(DateTimeZone.getDefault()));
                }
                return Operand.Create(time);
            }
            DateTime time2 = new DateTime(FunctionUtil.StartDateUtc.getMillis() + arg1.LongValue(), DateTimeZone.UTC);
            if (engine.UseLocalTime) {
                return Operand.Create(time2.toDateTime(DateTimeZone.getDefault()));
            }
            return Operand.Create(time2);
        } else if (type == 1) {
            Operand arg1 = ConvertToText(args1, 1);
            if (arg1.IsErrorOrNone()) { return arg1; }
            MyDate md = MyDate.parse(arg1.TextValue());
            if (md != null) {
                return Operand.Create(md);
            }
        } else if (type == 2) {
            Operand arg1 = ConvertToNumber(args1, 1);
            if (arg1.IsErrorOrNone()) { return arg1; }
            return arg1.ToDate(null);
        } else if (type == 3) {
            Operand arg1 = ConvertToNumber(args1, 1);
            if (arg1.IsErrorOrNone()) { return arg1; }
            DateTime time = new DateTime(FunctionUtil.StartDateUtc.getMillis() + arg1.LongValue(), DateTimeZone.UTC);
            if (engine.UseLocalTime) {
                return Operand.Create(time.toDateTime(DateTimeZone.getDefault()));
            }
            return Operand.Create(time);
        } else if (type == 4) {
            Operand arg1 = ConvertToNumber(args1, 1);
            if (arg1.IsErrorOrNone()) { return arg1; }
            DateTime time = new DateTime(FunctionUtil.StartDateUtc.getMillis() + arg1.LongValue() * 1000, DateTimeZone.UTC);
            if (engine.UseLocalTime) {
                return Operand.Create(time.toDateTime(DateTimeZone.getDefault()));
            }
            return Operand.Create(time);
        }
        return ParameterError(1);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.DATE;
    }

    @Override
    void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NONE);
        if (func2 != null) func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
