package toolgood.algorithm.internals.functions.datetimes;

import toolgood.algorithm.internals.Function_1;
import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.function.Function;

public class Function_DAY extends Function_1 {
    public Function_DAY(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.isNotDate()) {
            args1 = args1.toMyDate("Function '{0}' parameter is error!", "Day");
            if (args1.IsError()) {
                return args1;
            }
        }
        toolgood.algorithm.internals.MyDate date = args1.getDateValue();
        if (date.Day == null) {
            return Operand.Error("Function '{0}' is error!", "Day");
        }
        return Operand.Create(date.Day);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Day");
    }
}