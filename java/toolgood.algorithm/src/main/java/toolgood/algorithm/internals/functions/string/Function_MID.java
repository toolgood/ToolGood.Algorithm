package toolgood.algorithm.internals.functions.string;

import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_MID extends Function_3 {

    public Function_MID(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 3) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 3 parameters.");
        }
    }

    @Override
    public String Name() {
        return "Mid";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetText_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }
        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) { return args2; }
        Operand args3 = GetNumber_3(engine, tempParameter);
        if (args3.IsErrorOrNone()) { return args3; }

        String text = args1.TextValue();
        int excelIndex = engine.ExcelIndex;
        int startIndex = args2.IntValue() - excelIndex;
        int length = args3.IntValue();

        if (startIndex < 0) {
            return ParameterError(2);
        }
        if (length < 0) {
            return ParameterError(3);
        }
        if (startIndex == 0 && length >= text.length()) {
            return args1;
        }
        if (startIndex >= text.length()) {
            return Operand.Create("");
        }
        if (startIndex + length > text.length()) {
            length = text.length() - startIndex;
        }
        return Operand.Create(text.substring(startIndex, startIndex + length));
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.TEXT;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
        func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
