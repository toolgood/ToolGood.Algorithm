package toolgood.algorithm.internals.functions.string;

import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_4;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_SUBSTITUTE extends Function_4 {
    public Function_SUBSTITUTE(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Substitute";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetText_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }
        Operand args2 = GetText_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) { return args2; }
        Operand args3 = GetText_3(engine, tempParameter);
        if (args3.IsErrorOrNone()) { return args3; }
        if (func4 == null) {
            return Operand.Create(args1.TextValue().replace(args2.TextValue(), args3.TextValue()));
        }
        Operand args4 = GetNumber_4(engine, tempParameter);
        if (args4.IsErrorOrNone()) { return args4; }
        String text = args1.TextValue();
        String oldtext = args2.TextValue();
        String newtext = args3.TextValue();
        int replaceIndex = args4.IntValue();

        if (oldtext.length() == 0) {
            return Operand.Create(text);
        }

        int estimatedCapacity = Math.max(text.length(), text.length() + (newtext.length() - oldtext.length()));
        StringBuilder sb = new StringBuilder(estimatedCapacity);
        int currentIndex = 0;
        int foundCount = 0;
        int searchPos = 0;

        while (searchPos <= text.length() - oldtext.length()) {
            int foundPos = text.indexOf(oldtext, searchPos);
            if (foundPos < 0) break;

            foundCount++;
            if (foundCount == replaceIndex) {
                sb.append(text.substring(currentIndex, foundPos));
                sb.append(newtext);
                currentIndex = foundPos + oldtext.length();
                break;
            }
            searchPos = foundPos + oldtext.length();
        }

        if (currentIndex == 0) {
            return Operand.Create(text);
        }
        sb.append(text.substring(currentIndex));
        return Operand.Create(sb.toString());
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.TEXT;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
        func2.GetParameterTypes(noneEngine, result, OperandType.TEXT);
        func3.GetParameterTypes(noneEngine, result, OperandType.TEXT);
        if (func4 != null) {
            func4.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        }
    }
}
