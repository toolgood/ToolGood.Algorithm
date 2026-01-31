package toolgood.algorithm.internals.functions.compare;

import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.FunctionBase;

public class Function_NE extends Function_2 {
    public Function_NE(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.Function<String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter); if(args1.IsError()) { return args1; }
        Operand args2 = func2.Evaluate(work, tempParameter); if(args2.IsError()) { return args2; }

        if(args1.getType() == args2.getType()) {
            if(args1.IsNumber()) {
                return Operand.Create(args1.getNumberValue().compareTo(args2.getNumberValue()) != 0);
            } else if(args1.isText()) {
                return Operand.Create(!args1.getTextValue().equals(args2.getTextValue()));
            } else if(args1.isBoolean()) {
                return Operand.Create(args1.getBooleanValue() != args2.getBooleanValue());
            } else if(args1.isDate()) {
                args1 = args1.toNumber(null);
                args2 = args2.toNumber(null);
                return Operand.Create(args1.getNumberValue().compareTo(args2.getNumberValue()) != 0);
            } else if(args1.isJson()) {
                args1 = args1.toText(null);
                args2 = args2.toText(null);
                return Operand.Create(!args1.getTextValue().equals(args2.getTextValue()));
            } else if(args1.isNull()) {
                return Operand.FALSE;
            } else {
                return Operand.Error("Function '{0}' compare is error.", "!=");
            }
        } else if(args1.isNull() || args2.isNull()) {
            return Operand.TRUE;
        } else if(args2.isText()) {
            if(args1.isBoolean()) {
                Operand a = args2.toBoolean(null);
                if(!a.IsError()) {
                    return a.getBooleanValue() != args1.getBooleanValue() ? Operand.TRUE : Operand.FALSE;
                }
                args1 = args1.toText(null);
                return Operand.Create(!args1.getTextValue().equals(args2.getTextValue()));
            } else if(args1.isDate() || args1.IsNumber() || args1.isJson()) {
                args1 = args1.toText(null);
                return Operand.Create(!args1.getTextValue().equals(args2.getTextValue()));
            } else {
                return Operand.Error("Function '{0}' compare is error.", "!=");
            }
        } else if(args1.isJson() || args2.isJson() || args1.isArray() || args2.isArray() || args1.isArrayJson() || args2.isArrayJson()) {
            return Operand.Error("Function '{0}' compare is error.", "!=");
        }
        if(args1.isNotNumber()) { args1 = args1.toNumber("Function '{0}' parameter {1} is error!", "!=", 1); if(args1.IsError()) { return args1; } }
        if(args2.isNotNumber()) { args2 = args2.toNumber("Function '{0}' parameter {1} is error!", "!=", 2); if(args2.IsError()) { return args2; } }

        return Operand.Create(args1.getNumberValue().compareTo(args2.getNumberValue()) != 0);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        if(addBrackets) stringBuilder.append('(');
        func1.toString(stringBuilder, false);
        stringBuilder.append(" != ");
        func2.toString(stringBuilder, false);
        if(addBrackets) stringBuilder.append(')');
    }
}
