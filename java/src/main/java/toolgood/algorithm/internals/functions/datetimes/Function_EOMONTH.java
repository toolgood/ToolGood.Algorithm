package toolgood.algorithm.internals.functions.datetimes;

import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.function.Function;

public class Function_EOMONTH extends Function_2 {
    public Function_EOMONTH(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.isNotDate()) {
            args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "EoMonth", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.isNotNumber()) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "EoMonth", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        try {
            toolgood.algorithm.internals.MyDate date = args1.DateValue();
            int months = args2.IntValue();
            // 添加指定的月份数（加上1）
            toolgood.algorithm.internals.MyDate tempDate = date.AddMonths(months + 1);
            // 创建一个新的日期，设置为该月的第一天
            toolgood.algorithm.internals.MyDate firstDayOfMonth = new toolgood.algorithm.internals.MyDate(tempDate.Year, tempDate.Month, 1, 0, 0, 0);
            // 减去一天，得到上个月的最后一天
            toolgood.algorithm.internals.MyDate lastDayOfMonth = firstDayOfMonth.AddDays(-1);
            return Operand.Create(lastDayOfMonth);
        } catch (Exception e) {
            // 捕获所有异常
        }
        return Operand.Error("Function '{0}' is error!", "EoMonth");
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "EoMonth");
    }
}