package toolgood.algorithm.internals.functions.string;

import java.math.BigDecimal;
import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_TEXT extends Function_2 {
    public Function_TEXT(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Text";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = func1.Evaluate(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }
        Operand args2 = GetText_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) { return args2; }

        if (args1.IsText()) {
            return args1;
        } else if (args1.IsBoolean()) {
            return Operand.Create(args1.BooleanValue() ? "TRUE" : "FALSE");
        } else if (args1.IsNumber()) {
            return Operand.Create(formatNumber(args1.NumberValue(), args2.TextValue()));
        } else if (args1.IsDate()) {
            return Operand.Create(args1.DateValue().toString(args2.TextValue()));
        }
        args1 = ConvertToText(args1, 1);
        if (args1.IsErrorOrNone()) { return args1; }
        return Operand.Create(args1.TextValue());
    }

    private String formatNumber(BigDecimal number, String format) {
        return number.toString();
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.TEXT;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NONE);
        func2.GetParameterTypes(noneEngine, result, OperandType.TEXT);
    }
}
