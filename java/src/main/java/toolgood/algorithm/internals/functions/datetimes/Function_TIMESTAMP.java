package toolgood.algorithm.internals.functions.datetimes;

import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.function.Function;

public class Function_TIMESTAMP extends Function_2 {
    public Function_TIMESTAMP(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args0 = func1.Evaluate(work, tempParameter);
        if (args0.IsError()) {
            return args0;
        }

        int type = 0; // 毫秒
        if (func2 != null) {
            Operand args2 = func2.Evaluate(work, tempParameter);
            if (args2.isNotNumber()) {
                args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "TimeStamp", 2);
                if (args2.IsError()) {
                    return args2;
                }
            }
            type = args2.IntValue();
        }
        Operand dateOperand = args0.ToMyDate("Function '{0}' parameter {1} is error!", "TimeStamp", 1);
        if (dateOperand.IsError()) {
            return dateOperand;
        }
        toolgood.algorithm.internals.MyDate myDate = dateOperand.DateValue();
        
        // 计算时间戳
        long timestamp = myDate.ToDateTime().getMillis();
        
        if (type == 0) {
            // 返回毫秒时间戳
            return Operand.Create(timestamp);
        } else if (type == 1) {
            // 返回秒时间戳
            return Operand.Create((double) timestamp / 1000);
        }
        return Operand.Error("Function '{0}' parameter is error!", "TimeStamp");
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "TimeStamp");
    }
}