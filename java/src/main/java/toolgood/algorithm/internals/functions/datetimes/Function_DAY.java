package toolgood.algorithm.internals.functions.datetimes;

import toolgood.algorithm.internals.functions.Function_1;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;



public class Function_DAY extends Function_1 {
    public Function_DAY(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotDate()) {
            args1 = args1.ToMyDate("Function '{0}' parameter is error!", "Day");
            if (args1.IsError()) {
                return args1;
            }
        }
        toolgood.algorithm.internals.MyDate date = args1.DateValue();
        if (date.Day == null) {
            return Operand.Error("Function '{0}' is error!", "Day");
        }
        return Operand.Create((int)date.Day);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Day");
    }
}