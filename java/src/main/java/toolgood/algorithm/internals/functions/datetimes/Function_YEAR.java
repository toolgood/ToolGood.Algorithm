package toolgood.algorithm.internals.functions.datetimes;

import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.FunctionBase;

public class Function_YEAR extends Function_1 {
    public Function_YEAR(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.Function<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotDate()) {
            args1 = args1.ToMyDate("Function '{0}' parameter is error!", "Year");
            if (args1.IsError()) {
                return args1;
            }
        }
        if (args1.DateValue().year() == null) {
            return Operand.Error("Function '{0}' is error!", "Year");
        }
        return Operand.Create(args1.DateValue().year().getValue());
    }

    @Override
    public void toString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Year");
    }
}
