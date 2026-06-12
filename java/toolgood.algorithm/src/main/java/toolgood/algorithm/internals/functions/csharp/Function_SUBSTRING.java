package toolgood.algorithm.internals.functions.csharp;

import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.functions.NoneEngine;

final class Function_SUBSTRING extends Function_3 {

    Function_SUBSTRING(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 2 || funcs.length > 3) {
            throw new IllegalArgumentException("Function 'Substring' requires 2 to 3 parameters.");
        }
    }

    @Override
    public String Name() {
        return "Substring";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetText_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }

        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) { return args2; }

        String text = args1.TextValue();
        int startIndex = args2.IntValue() - (engine.UseExcelIndex ? 1 : 0);

        if (startIndex < 0) {
            return ParameterError(2);
        }
        if (startIndex >= text.length()) {
            return Operand.Create("");
        }

        if (func3 == null) {
            return Operand.Create(text.substring(startIndex));
        }

        Operand args3 = GetNumber_3(engine, tempParameter);
        if (args3.IsErrorOrNone()) { return args3; }

        int length = args3.IntValue();
        if (length < 0) {
            return ParameterError(3);
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
    void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
        func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        if (func3 != null) {
            func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        }
    }
}
