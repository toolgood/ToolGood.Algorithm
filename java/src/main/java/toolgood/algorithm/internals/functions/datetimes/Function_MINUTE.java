package toolgood.algorithm.internals.functions.datetimes;

import toolgood.algorithm.internals.functions.Function_1;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;



public class Function_MINUTE extends Function_1 {
    public Function_MINUTE(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotDate()) {
            args1 = args1.ToMyDate("Function '{0}' parameter is error!", "Minute");
            if (args1.IsError()) {
                return args1;
            }
        }
        toolgood.algorithm.internals.MyDate date = args1.DateValue();
        return Operand.Create(date.Minute);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Minute");
    }
}