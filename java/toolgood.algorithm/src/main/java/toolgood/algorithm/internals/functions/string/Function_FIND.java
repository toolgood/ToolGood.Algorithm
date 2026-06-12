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

final class Function_FIND extends Function_3 {

    public Function_FIND(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 2 || funcs.length > 3) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires 2 to 3 parameters.");
        }
    }

    @Override
    public String Name() {
        return "Find";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetText_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }
        Operand args2 = GetText_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) { return args2; }
        if (func3 == null) {
            int p = args2.TextValue().indexOf(args1.TextValue()) + (engine.UseExcelIndex ? 1 : 0);
            return Operand.Create(p);
        }
        Operand count = GetNumber_3(engine, tempParameter);
        if (count.IsErrorOrNone()) { return count; }
        int excelIndex = engine.UseExcelIndex ? 1 : 0;
        int startIndex = count.IntValue() - excelIndex;
        if (startIndex < 0 || startIndex >= args2.TextValue().length()) {
            return ParameterError(3);
        }
        int p2 = args2.TextValue().indexOf(args1.TextValue(), startIndex);
        if (p2 < 0) {
            return FunctionError();
        }
        return Operand.Create(p2 + startIndex + excelIndex);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
        func2.GetParameterTypes(noneEngine, result, OperandType.TEXT);
        if (func3 != null) {
            func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        }
    }
}
