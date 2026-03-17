package toolgood.algorithm.internals.functions.string;

import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_FIND extends Function_3 {
    public Function_FIND(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Find";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetText_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }
        Operand args2 = GetText_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) { return args2; }
        if (func3 == null) {
            int p = args2.TextValue().indexOf(args1.TextValue()) + engine.ExcelIndex;
            return Operand.Create(p);
        }
        Operand count = GetNumber_3(engine, tempParameter);
        if (count.IsErrorOrNone()) { return count; }
        int startIndex = count.IntValue() - engine.ExcelIndex;
        if (startIndex < 0 || startIndex >= args2.TextValue().length()) {
            return ParameterError(3);
        }
        int p2 = args2.TextValue().substring(startIndex).indexOf(args1.TextValue());
        if (p2 < 0) {
            return FunctionError();
        }
        return Operand.Create(p2 + startIndex + engine.ExcelIndex);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
        func2.GetParameterTypes(noneEngine, result, OperandType.TEXT);
        if (func3 != null) {
            func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        }
    }
}
