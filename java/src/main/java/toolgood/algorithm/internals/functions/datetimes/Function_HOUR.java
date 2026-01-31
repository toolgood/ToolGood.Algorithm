package toolgood.algorithm.internals.functions.datetimes;

import toolgood.algorithm.internals.Function_1;
import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.function.Function;

public class Function_HOUR extends Function_1 {
    public Function_HOUR(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.isNotDate()) {
            args1 = args1.toMyDate("Function '{0}' parameter is error!", "Hour");
            if (args1.isError()) {
                return args1;
            }
        }
        toolgood.algorithm.internals.MyDate date = args1.getDateValue();
        return Operand.Create(date.Hour);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Hour");
    }
}