package toolgood.algorithm.internals.functions.datetimes;

import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.ArrayList;
import java.util.List;
import java.util.function.Function;

public class Function_DATEVALUE extends Function_N {
    public Function_DATEVALUE(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        List<Operand> args = new ArrayList<>();
        for (FunctionBase item : funcs) {
            Operand aa = item.Evaluate(work, tempParameter);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }
        if (args.get(0).isDate()) {
            return args.get(0);
        }
        int type = 0;
        if (args.size() == 2) {
            Operand args2 = args.get(1).ToNumber
("Function '{0}' parameter {1} is error!", "DateValue", 2);
            if (args2.IsError()) {
                return args2;
            }
            type = args2.IntValue();
        }
        if (type == 0) {
            if (args.get(0).isText()) {
                toolgood.algorithm.internals.MyDate date = toolgood.algorithm.internals.MyDate.parse(args.get(0).TextValue());
                if (date != null) {
                    return Operand.Create(date);
                }
            }
            Operand args1 = args.get(0).ToNumber
("Function '{0}' parameter {1} is error!", "DateValue", 1);
            if (args1.getLongValue() <= 2958465L) { // 9999-12-31 日时间在excel的数字为 2958465
                return args1.toMyDate();
            }
            if (args1.getLongValue() <= 253402232399L) { // 9999-12-31 12:59:59 日时间 转 时间截 为 253402232399L
                // 这里需要实现类似 FunctionUtil.StartDateUtc.AddSeconds 的功能
                // 暂时使用 MyDate 的构造函数来处理
                return Operand.Create(new toolgood.algorithm.internals.MyDate(args1.getLongValue() / 86400.0));
            }
            // 注：时间截 253402232399 ms 转时间 为 1978-01-12 05:30:32
            // 这里需要实现类似 FunctionUtil.StartDateUtc.AddMilliseconds 的功能
            return Operand.Create(new toolgood.algorithm.internals.MyDate(args1.getLongValue() / (86400.0 * 1000.0)));
        } else if (type == 1) {
            Operand args1 = args.get(0).toText("Function '{0}' parameter {1} is error!", "DateValue", 1);
            if (args1.IsError()) {
                return args1;
            }
            toolgood.algorithm.internals.MyDate date = toolgood.algorithm.internals.MyDate.parse(args1.TextValue());
            if (date != null) {
                return Operand.Create(date);
            }
        } else if (type == 2) {
            return args.get(0).ToNumber
("Function '{0}' parameter is error!", "DateValue").toMyDate();
        } else if (type == 3) {
            Operand args1 = args.get(0).ToNumber
("Function '{0}' parameter {1} is error!", "DateValue", 1);
            // 这里需要实现类似 FunctionUtil.StartDateUtc.AddMilliseconds 的功能
            return Operand.Create(new toolgood.algorithm.internals.MyDate(args1.getLongValue() / (86400.0 * 1000.0)));
        } else if (type == 4) {
            Operand args1 = args.get(0).ToNumber
("Function '{0}' parameter {1} is error!", "DateValue", 1);
            // 这里需要实现类似 FunctionUtil.StartDateUtc.AddSeconds 的功能
            return Operand.Create(new toolgood.algorithm.internals.MyDate(args1.getLongValue() / 86400.0));
        }
        return Operand.Error("Function '{0}' parameter is error!", "DateValue");
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "DateValue");
    }
}