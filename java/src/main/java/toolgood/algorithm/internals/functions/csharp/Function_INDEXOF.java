package toolgood.algorithm.internals.functions.csharp;

import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.Function_4;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_INDEXOF extends Function_4 {
    public Function_INDEXOF(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "IndexOf";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetText_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }

        Operand args2 = GetText_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) { return args2; }

        String text = args1.TextValue();
        if (func3 == null) {
            return Operand.Create(text.indexOf(args2.TextValue()) + engine.ExcelIndex);
        }

        Operand args3 = GetNumber_3(engine, tempParameter);
        if (args3.IsErrorOrNone()) { return args3; }
        int startIndex = args3.IntValue() - engine.ExcelIndex;
        if (startIndex < 0 || startIndex > text.length()) {
            return ParameterError(3);
        }

        if (func4 == null) {
            return Operand.Create(text.substring(startIndex).indexOf(args2.TextValue()) + startIndex + engine.ExcelIndex);
        }

        Operand args4 = GetNumber_4(engine, tempParameter);
        if (args4.IsErrorOrNone()) { return args4; }
        int count = args4.IntValue();
        if (count < 0 || startIndex + count > text.length()) {
            return ParameterError(4);
        }

        return Operand.Create(text.indexOf(args2.TextValue(), startIndex, startIndex + count) + engine.ExcelIndex);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.TEXT, null, null);
        func2.GetParameterTypes(noneEngine, result, OperandType.TEXT, null, null);
        if (func3 != null) {
            func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER, null, null);
        }
        if (func4 != null) {
            func4.GetParameterTypes(noneEngine, result, OperandType.NUMBER, null, null);
        }
    }
}
