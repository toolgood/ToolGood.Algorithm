package toolgood.algorithm.internals.functions.string;

import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_1;

public class Function_UNICHAR extends Function_1 {
    public Function_UNICHAR(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetNumber_1(engine, tempParameter);
        if (args1.IsError()) return args1;

        int code = args1.IntValue();
        if (code < 0 || code > 0x10FFFF || (code >= 0xD800 && code <= 0xDFFF)) {
            return ParameterError(1);
        }
        try {
            return Operand.Create(new String(Character.toChars(code)));
        } catch (Exception e) {
            return ParameterError(1);
        }
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "UniChar");
    }
}
