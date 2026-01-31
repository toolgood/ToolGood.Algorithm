package toolgood.algorithm.internals.functions.datetimes;

import toolgood.algorithm.internals.functions.Function_1;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.function.Function;

public class Function_SECOND extends Function_1 {
    public Function_SECOND(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.isNotDate()) {
            args1 = args1.toMyDate("Function '{0}' parameter is error!", "Second");
            if (args1.IsError()) {
                return args1;
            }
        }
        toolgood.algorithm.internals.MyDate date = args1.getDateValue();
        return Operand.Create(date.Second);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Second");
    }
}