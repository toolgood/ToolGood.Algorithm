package toolgood.algorithm.internals.functions.datetimes;

import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;



public class Function_ADDHOURS extends Function_2 {
    public Function_ADDHOURS(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.isNotDate()) {
            args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "AddHours", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.isNotNumber()) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "AddHours", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        try {
            toolgood.algorithm.internals.MyDate date = args1.DateValue();
            int hours = args2.IntValue();
            toolgood.algorithm.internals.MyDate result = date.AddHours(hours);
            return Operand.Create(result);
        } catch (Exception e) {
            // 捕获所有异常
        }
        return Operand.Error("Function '{0}' is error!", "AddHours");
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "AddHours");
    }
}