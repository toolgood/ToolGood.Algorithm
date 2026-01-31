package toolgood.algorithm.internals.functions.operator;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Locale;

import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.internals.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.FunctionUtil;

public class Function_Sub extends Function_2 {
    public Function_Sub(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsError()) {
            return args1;
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsError()) {
            return args2;
        }

        if (args1.IsNumber() && args2.IsNumber()) { // 优化性能
            if (args2.getNumberValue() == 0) {
                return args1;
            }
            return Operand.Create(args1.getNumberValue() - args2.getNumberValue());
        }
        if (args1.IsNull()) {
            return Operand.Error("Function '{0}' parameter {1} is NULL!", "-", 1);
        }
        if (args2.IsNull()) {
            return Operand.Error("Function '{0}' parameter {1} is NULL!", "-", 2);
        }

        if (args1.IsText()) {
            try {
                double d = Double.parseDouble(args1.getTextValue());
                args1 = Operand.Create(d);
            } catch (NumberFormatException e1) {
                Boolean b = FunctionUtil.TryParseBoolean(args1.getTextValue());
                if (b != null) {
                    args1 = b ? Operand.One : Operand.Zero;
                } else {
                    try {
                        SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss", Locale.US);
                        java.util.Date dt = sdf.parse(args1.getTextValue());
                        args1 = Operand.Create(new MyDate(dt));
                    } catch (ParseException e2) {
                        return Operand.Error("Function '{0}' is error", "-");
                    }
                }
            }
        }
        if (args2.IsText()) {
            try {
                double d = Double.parseDouble(args2.getTextValue());
                args2 = Operand.Create(d);
            } catch (NumberFormatException e1) {
                Boolean b = FunctionUtil.TryParseBoolean(args2.getTextValue());
                if (b != null) {
                    args2 = b ? Operand.One : Operand.Zero;
                } else {
                    try {
                        SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss", Locale.US);
                        java.util.Date dt = sdf.parse(args2.getTextValue());
                        args2 = Operand.Create(new MyDate(dt));
                    } catch (ParseException e2) {
                        return Operand.Error("Function '{0}' is error", "-");
                    }
                }
            }
        }
        if (args1.IsDate()) {
            if (args2.IsDate()) {
                return Operand.Create(args1.getDateValue().subtract(args2.getDateValue()));
            }
            if (args2.IsNotNumber()) {
                args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "-", 2);
                if (args2.IsError()) {
                    return args2;
                }
            }
            if (args2.getNumberValue() == 0) {
                return args1;
            }
            return Operand.Create(args1.getDateValue().subtract(args2.getNumberValue()));
        } else if (args2.IsDate()) {
            if (args1.IsNotNumber()) {
                args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "-", 1);
                if (args1.IsError()) {
                    return args1;
                }
            }
            return Operand.Create(args1.getNumberValue() - args2.getDateValue().toDouble());
        }
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "-", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        if (args2.IsNotNumber()) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "-", 2);
            if (args2.IsError()) {
                return args2;
            }
        }

        if (args2.getNumberValue() == 0) {
            return args1;
        }

        return Operand.Create(args1.getNumberValue() - args2.getNumberValue());
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        if (addBrackets) {
            stringBuilder.append('(');
        }
        func1.ToString(stringBuilder, false);
        stringBuilder.append(" - ");
        func2.ToString(stringBuilder, false);
        if (addBrackets) {
            stringBuilder.append(')');
        }
    }
}
