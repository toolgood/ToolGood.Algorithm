package toolgood.algorithm.internals.functions.datetimes;

import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.MyDate;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;

public class Function_DAYS extends Function_2 {
    public Function_DAYS(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotDate()) {
            args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "Days", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotDate()) {
            args2 = args2.ToMyDate("Function '{0}' parameter {1} is error!", "Days", 2);
            if (args2.IsError()) {
                return args2;
            }
        }

        MyDate endMyDate = args1.DateValue();
        MyDate startMyDate = args2.DateValue();

        // 计算两个日期之间的天数差（仅比较日期部分）
        long endMillis = endMyDate.ToDateTime().withTimeAtStartOfDay().getMillis();
        long startMillis = startMyDate.ToDateTime().withTimeAtStartOfDay().getMillis();
        long days = (endMillis - startMillis) / (1000L * 60 * 60 * 24);

        return Operand.Create((int) days);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Days");
    }
}
