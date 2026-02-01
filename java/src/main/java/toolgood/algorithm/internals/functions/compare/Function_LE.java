package toolgood.algorithm.internals.functions.compare;

import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.FunctionBase;

public class Function_LE extends Function_2 {
    public Function_LE(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter); if(args1.IsError()) { return args1; }
        Operand args2 = func2.Evaluate(work, tempParameter); if(args2.IsError()) { return args2; }

        if(args1.Type() == args2.Type()) {
            if(args1.IsNumber()) {
                return Operand.Create(args1.NumberValue().compareTo(args2.NumberValue()) <= 0);
            } else if(args1.IsText()) {
                int r = args1.TextValue().compareTo(args2.TextValue());
                return r <= 0 ? Operand.TRUE : Operand.FALSE;
            } else if(args1.IsDate() || args1.IsBoolean()) {
                args1 = args1.ToNumber(null);
                args2 = args2.ToNumber(null);
                return Operand.Create(args1.NumberValue().compareTo(args2.NumberValue()) <= 0);
            } else if(args1.IsJson()) {
                args1 = args1.ToText(null);
                args2 = args2.ToText(null);
                int r = args1.TextValue().compareTo(args2.TextValue());
                return r <= 0 ? Operand.TRUE : Operand.FALSE;
            } else if(args1.IsNull()) {
                return Operand.TRUE;
            } else {
                return Operand.Error("Function '{0}' compare is error.", "<=");
            }
        } else if(args1.IsNull() || args2.IsNull()) {
            return Operand.FALSE;
        } else if(args2.IsText()) {
            if(args1.IsBoolean()) {
                Operand a = args2.ToBoolean(null);
                if(!a.IsError()) {
                    return a.BooleanValue() != args1.BooleanValue() ? Operand.TRUE : Operand.FALSE;
                }
                args1 = args1.ToText(null);
                int r = args1.TextValue().compareTo(args2.TextValue());
                return r <= 0 ? Operand.TRUE : Operand.FALSE;
            } else if(args1.IsDate() || args1.IsNumber() || args1.IsJson()) {
                args1 = args1.ToText(null);
                int r = args1.TextValue().compareTo(args2.TextValue());
                return r <= 0 ? Operand.TRUE : Operand.FALSE;
            } else {
                return Operand.Error("Function '{0}' compare is error.", "<=");
            }
        } else if(args1.IsJson() || args2.IsJson() || args1.IsArray() || args2.IsArray() || args1.IsArrayJson() || args2.IsArrayJson()) {
            return Operand.Error("Function '{0}' compare is error.", "<=");
        }
        if(args1.IsNotNumber()) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "<=", 1); if(args1.IsError()) { return args1; } }
        if(args2.IsNotNumber()) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "<=", 2); if(args2.IsError()) { return args2; } }

        return Operand.Create(args1.NumberValue().compareTo(args2.NumberValue()) <= 0);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        if(addBrackets) stringBuilder.append('(');
        func1.toString(stringBuilder, false);
        stringBuilder.append(" <= ");
        func2.toString(stringBuilder, false);
        if(addBrackets) stringBuilder.append(')');
    }
}
