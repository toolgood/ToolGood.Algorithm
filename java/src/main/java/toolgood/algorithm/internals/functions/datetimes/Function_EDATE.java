package toolgood.algorithm.internals.functions.datetimes;

import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;



public class Function_EDATE extends Function_2 {
    public Function_EDATE(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.isNotDate()) {
            args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "EDate", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.isNotNumber()) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "EDate", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        try {
            toolgood.algorithm.internals.MyDate date = args1.DateValue();
            int months = args2.IntValue();
            toolgood.algorithm.internals.MyDate result = date.AddMonths(months);
            return Operand.Create(result);
        } catch (Exception e) {
            // 捕获所有异常
        }
        return Operand.Error("Function '{0}' is error!", "EDate");
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "EDate");
    }
}