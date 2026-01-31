package toolgood.algorithm.internals.functions.csharp;

import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.functions.FunctionBase;

public class Function_SUBSTRING extends Function_3 {
    public Function_SUBSTRING(FunctionBase func1, FunctionBase func2, FunctionBase func3) {
        super(func1, func2, func3);
    }

    @Override
    public Operand evaluate(AlgorithmEngine work, java.util.function.Function<String, Operand> tempParameter) {
        Operand args1 = func1.evaluate(work, tempParameter); if(args1.isNotText()) { args1 = args1.toText("Function '{0}' parameter {1} is error!", "Substring", 1); if(args1.isError()) { return args1; } }
        Operand args2 = func2.evaluate(work, tempParameter); if(args2.isNotNumber()) { args2 = args2.toNumber("Function '{0}' parameter {1} is error!", "Substring", 2); if(args2.isError()) { return args2; } }

        String text = args1.getTextValue();
        int startIndex = args2.getIntValue() - work.getExcelIndex();
        if(func3 == null) {
            if(startIndex < 0) startIndex = 0;
            if(startIndex > text.length()) startIndex = text.length();
            return Operand.create(text.substring(startIndex));
        }
        Operand args3 = func3.evaluate(work, tempParameter); if(args3.isNotNumber()) { args3 = args3.toNumber("Function '{0}' parameter {1} is error!", "Substring", 3); if(args3.isError()) { return args3; } }
        int length = args3.getIntValue();
        if(startIndex < 0) startIndex = 0;
        if(startIndex > text.length()) startIndex = text.length();
        int endIndex = startIndex + length;
        if(endIndex > text.length()) endIndex = text.length();
        return Operand.create(text.substring(startIndex, endIndex));
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        addFunction(stringBuilder, "Substring");
    }
}
