package toolgood.algorithm.internals.functions.datetimes;

import toolgood.algorithm.internals.functions.Function_1;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.function.Function;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class Function_TIMEVALUE extends Function_1 {
    public Function_TIMEVALUE(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.isNotText()) {
            args1 = args1.ToText("Function '{0}' parameter is error!", "TimeValue");
            if (args1.IsError()) {
                return args1;
            }
        }

        String timeString = args1.TextValue();
        // 尝试解析时间字符串
        toolgood.algorithm.internals.MyDate date = parseTimeString(timeString);
        if (date != null) {
            return Operand.Create(date);
        }
        return Operand.Error("Function '{0}' parameter is error!", "TimeValue");
    }

    private toolgood.algorithm.internals.MyDate parseTimeString(String timeString) {
        // 匹配格式: HH:MM:SS
        Pattern pattern1 = Pattern.compile("^(2[0123]|[01]?\\d):([012345]?\\d):([012345]?\\d)$");
        Matcher matcher1 = pattern1.matcher(timeString);
        if (matcher1.find()) {
            int hour = Integer.parseInt(matcher1.group(1));
            int minute = Integer.parseInt(matcher1.group(2));
            int second = Integer.parseInt(matcher1.group(3));
            return new toolgood.algorithm.internals.MyDate(0, 0, 0, hour, minute, second);
        }
        // 匹配格式: HH:MM
        Pattern pattern2 = Pattern.compile("^(2[0123]|[01]?\\d):([012345]?\\d)$");
        Matcher matcher2 = pattern2.matcher(timeString);
        if (matcher2.find()) {
            int hour = Integer.parseInt(matcher2.group(1));
            int minute = Integer.parseInt(matcher2.group(2));
            return new toolgood.algorithm.internals.MyDate(0, 0, 0, hour, minute, 0);
        }
        return null;
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "TimeValue");
    }
}