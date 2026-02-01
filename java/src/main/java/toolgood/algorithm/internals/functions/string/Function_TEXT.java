package toolgood.algorithm.internals.functions.string;

import java.text.DecimalFormat;
import java.text.SimpleDateFormat;
import java.util.Locale;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_2;

public class Function_TEXT extends Function_2 {
    public Function_TEXT(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiBiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsError()) {
            return args1;
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotText()) {
            args2 = args2.ToText("Function '{0}' parameter {1} is error!", "Text", 2);
            if (args2.IsError()) {
                return args2;
            }
        }

        if (args1.IsText()) {
            return args1;
        } else if (args1.IsBoolean()) {
            return Operand.Create(args1.BooleanValue() ? "TRUE" : "FALSE");
        } else if (args1.IsNumber()) {
            String format = args2.TextValue();
            try {
                DecimalFormat df = new DecimalFormat(format, new DecimalFormatSymbols(Locale.US));
                return Operand.Create(df.format(args1.getNumberValue()));
            } catch (Exception e) {
                return Operand.Error("Function '{0}' format is error!", "Text");
            }
        } else if (args1.IsDate()) {
            String format = args2.TextValue();
            try {
                SimpleDateFormat sdf = new SimpleDateFormat(format, Locale.US);
                return Operand.Create(sdf.format(args1.DateValue().toDate()));
            } catch (Exception e) {
                return Operand.Error("Function '{0}' format is error!", "Text");
            }
        }
        args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Text", 1);
        if (args1.IsError()) {
            return args1;
        }
        return Operand.Create(args1.TextValue());
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Text");
    }
}
