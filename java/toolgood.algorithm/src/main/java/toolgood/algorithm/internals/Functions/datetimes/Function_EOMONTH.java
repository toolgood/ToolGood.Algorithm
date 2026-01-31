package toolgood.algorithm.internals.functions.datetimes;

import toolgood.algorithm.internals.Function_2;
import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.function.Function;

public class Function_EOMONTH extends Function_2 {
    public Function_EOMONTH(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.isNotDate()) {
            args1 = args1.toMyDate("Function '{0}' parameter {1} is error!", "EoMonth", 1);
            if (args1.isError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.isNotNumber()) {
            args2 = args2.toNumber("Function '{0}' parameter {1} is error!", "EoMonth", 2);
            if (args2.isError()) {
                return args2;
            }
        }
        try {
            toolgood.algorithm.internals.MyDate date = args1.getDateValue();
            int months = args2.getIntValue();
            // 添加指定的月份数（加上1）
            toolgood.algorithm.internals.MyDate tempDate = date.AddMonths(months + 1);
            // 创建一个新的日期，设置为该月的第一天
            toolgood.algorithm.internals.MyDate firstDayOfMonth = new toolgood.algorithm.internals.MyDate(tempDate.Year, tempDate.Month, 1, 0, 0, 0);
            // 减去一天，得到上个月的最后一天
            toolgood.algorithm.internals.MyDate lastDayOfMonth = firstDayOfMonth.AddDays(-1);
            return Operand.create(lastDayOfMonth);
        } catch (Exception e) {
            // 捕获所有异常
        }
        return Operand.error("Function '{0}' is error!", "EoMonth");
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "EoMonth");
    }
}