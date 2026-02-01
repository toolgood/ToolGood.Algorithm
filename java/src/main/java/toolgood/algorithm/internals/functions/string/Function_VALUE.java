package toolgood.algorithm.internals.functions.string;

import toolgood.algorithm.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_1;

import java.text.NumberFormat;
import java.text.ParseException;
import java.util.Locale;

public class Function_VALUE extends Function_1 {
    public Function_VALUE(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiBiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNumber()) {
            return args1;
        }
        if (args1.IsBoolean()) {
            return args1.BooleanValue() ? Operand.ONE : Operand.ZERO;
        }
        if (args1.isNotText()) {
            args1 = args1.ToText("Function '{0}' parameter is error!", "Value");
            if (args1.IsError()) {
                return args1;
            }
        }

        try {
            NumberFormat format = NumberFormat.getInstance(Locale.US);
            Number number = format.parse(args1.TextValue());
            return Operand.Create(number.doubleValue());
        } catch (ParseException e) {
            return Operand.Error("Function '{0}' parameter is error!", "Value");
        }
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Value");
    }
}
