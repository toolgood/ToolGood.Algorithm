package toolgood.algorithm.internals.functions.compare;

import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.FunctionBase;

public class Function_EQ extends Function_2 {
    public Function_EQ(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter); if(args1.IsError()) { return args1; }
        Operand args2 = func2.Evaluate(work, tempParameter); if(args2.IsError()) { return args2; }

        if(args1.getType() == args2.getType()) {
            if(args1.IsNumber()) {
                return Operand.Create(args1.getNumberValue().compareTo(args2.getNumberValue()) == 0);
            } else if(args1.IsText()) {
                return Operand.Create(args1.TextValue().equals(args2.TextValue()));
            } else if(args1.IsBoolean()) {
                return Operand.Create(args1.BooleanValue() == args2.BooleanValue());
            } else if(args1.IsDate()) {
                args1 = args1.ToNumber(null);
                args2 = args2.ToNumber(null);
                return Operand.Create(args1.getNumberValue().compareTo(args2.getNumberValue()) == 0);
            } else if(args1.IsJson()) {
                args1 = args1.ToText(null);
                args2 = args2.ToText(null);
                return Operand.Create(args1.TextValue().equals(args2.TextValue()));
            } else if(args1.isNull()) {
                return Operand.TRUE;
            } else {
                return Operand.Error("Function '{0}' compare is error.", "==");
            }
        } else if(args1.isNull() || args2.isNull()) {
            return Operand.FALSE;
        } else if(args2.IsText()) {
            if(args1.IsBoolean()) {
                Operand a = args2.ToBoolean(null);
                if(!a.IsError()) {
                    return a.BooleanValue() == args1.BooleanValue() ? Operand.TRUE : Operand.FALSE;
                }
                args1 = args1.ToText(null);
                return Operand.Create(args1.TextValue().equals(args2.TextValue()));
            } else if(args1.IsDate() || args1.IsNumber() || args1.IsJson()) {
                args1 = args1.ToText(null);
                return Operand.Create(args1.TextValue().equals(args2.TextValue()));
            } else {
                return Operand.Error("Function '{0}' compare is error.", "==");
            }
        } else if(args1.IsJson() || args2.IsJson() || args1.IsArray() || args2.IsArray() || args1.isArrayJson() || args2.isArrayJson()) {
            return Operand.Error("Function '{0}' compare is error.", "==");
        }
        if(args1.isNotNumber()) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "==", 1); if(args1.IsError()) { return args1; } }
        if(args2.isNotNumber()) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "==", 2); if(args2.IsError()) { return args2; } }

        return Operand.Create(args1.getNumberValue().compareTo(args2.getNumberValue()) == 0);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        if(addBrackets) stringBuilder.append('(');
        func1.toString(stringBuilder, false);
        stringBuilder.append(" == ");
        func2.toString(stringBuilder, false);
        if(addBrackets) stringBuilder.append(')');
    }
}
