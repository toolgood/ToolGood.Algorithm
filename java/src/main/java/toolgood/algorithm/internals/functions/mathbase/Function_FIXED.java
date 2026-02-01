package toolgood.algorithm.internals.functions.mathbase;

import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;

import java.text.DecimalFormat;

public class Function_FIXED extends Function_3 {
    public Function_FIXED(FunctionBase func1, FunctionBase func2, FunctionBase func3) {
        super(func1, func2, func3);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        int num = 2;
        if (func2 != null) {
            Operand args2 = func2.Evaluate(work, tempParameter);
            if (args2.IsNotNumber()) {
                args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Fixed", 2);
                if (args2.IsError()) {
                    return args2;
                }
            }
            num = args2.IntValue();
        }
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotNumber()) {
            args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Fixed", 1);
            if (args1.IsError()) {
                return args1;
            }
        }

        double value = args1.NumberValue();
        double factor = Math.pow(10, num);
        double s = Math.round(value * factor) / factor;
        boolean no = false;
        if (func3 != null) {
            Operand args3 = func3.Evaluate(work, tempParameter);
            if (args3.IsNotBoolean()) {
                args3 = args3.ToBoolean("Function '{0}' parameter {1} is error!", "Fixed", 3);
                if (args3.IsError()) {
                    return args3;
                }
            }
            no = args3.BooleanValue();
        }
        if (!no) {
            DecimalFormat df = new DecimalFormat();
            df.setMinimumFractionDigits(num);
            df.setMaximumFractionDigits(num);
            df.setGroupingUsed(true);
            return Operand.Create(df.format(s));
        }
        DecimalFormat df = new DecimalFormat();
        df.setMinimumFractionDigits(num);
        df.setMaximumFractionDigits(num);
        df.setGroupingUsed(false);
        return Operand.Create(df.format(s));
    }

    @Override
    public void toString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Fixed");
    }
}
