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

public final class Function_LASTINDEXOF extends Function_4 {
    public Function_LASTINDEXOF(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "LastIndexOf";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetText_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }

        Operand args2 = GetText_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) { return args2; }

        String text = args1.TextValue();
        if (func3 == null) {
            return Operand.Create(text.lastIndexOf(args2.TextValue()) + engine.ExcelIndex);
        }

        Operand args3 = GetNumber_3(engine, tempParameter);
        if (args3.IsErrorOrNone()) { return args3; }
        int startIndex = args3.IntValue() - engine.ExcelIndex;
        if (startIndex < 0 || startIndex > text.length()) {
            return ParameterError(3);
        }

        if (func4 == null) {
            return Operand.Create(text.substring(0, startIndex).lastIndexOf(args2.TextValue()) + engine.ExcelIndex);
        }

        Operand args4 = GetNumber_4(engine, tempParameter);
        if (args4.IsErrorOrNone()) { return args4; }
        int count = args4.IntValue();
        if (count < 0 || count > startIndex + 1) {
            return ParameterError(4);
        }

        return Operand.Create(text.lastIndexOf(args2.TextValue(), startIndex, startIndex - count + 1) + engine.ExcelIndex);
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
