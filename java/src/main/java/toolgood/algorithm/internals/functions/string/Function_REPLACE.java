package toolgood.algorithm.internals.functions.string;

import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_4;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_REPLACE extends Function_4 {
    public Function_REPLACE(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Replace";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetText_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }
        String oldtext = args1.TextValue();
        if (func4 == null) {
            Operand args22 = GetText_2(engine, tempParameter);
            if (args22.IsErrorOrNone()) { return args22; }
            Operand args32 = GetText_3(engine, tempParameter);
            if (args32.IsErrorOrNone()) { return args32; }

            String old = args22.TextValue();
            String newstr = args32.TextValue();
            return Operand.Create(oldtext.replace(old, newstr));
        }

        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) { return args2; }
        Operand args3 = GetNumber_3(engine, tempParameter);
        if (args3.IsErrorOrNone()) { return args3; }
        Operand args4 = GetText_4(engine, tempParameter);
        if (args4.IsErrorOrNone()) { return args4; }

        int start = args2.IntValue() - engine.ExcelIndex;
        int length = args3.IntValue();
        String newtext = args4.TextValue();

        if (start < 0) {
            return ParameterError(2);
        }
        if (length < 0) {
            return ParameterError(3);
        }
        if (start >= oldtext.length()) {
            return Operand.Create(oldtext + newtext);
        }

        StringBuilder sb = new StringBuilder(oldtext.length() - length + newtext.length());
        sb.append(oldtext.substring(0, start));
        sb.append(newtext);
        int endIndex = start + length;
        if (endIndex < oldtext.length()) {
            sb.append(oldtext.substring(endIndex));
        }
        return Operand.Create(sb.toString());
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.TEXT;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
        if (func4 == null) {
            func2.GetParameterTypes(noneEngine, result, OperandType.TEXT);
            func3.GetParameterTypes(noneEngine, result, OperandType.TEXT);
        } else {
            func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
            func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
            func4.GetParameterTypes(noneEngine, result, OperandType.TEXT);
        }
    }
}
