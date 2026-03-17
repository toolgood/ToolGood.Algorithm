package toolgood.algorithm.internals.functions.string;

import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_1;

public class Function_UNICODE extends Function_1 {
    public Function_UNICODE(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand args1 = GetText_1(engine, tempParameter);
        if (args1.IsError()) return args1;

        String text = args1.TextValue();
        if (text == null || text.isEmpty()) {
            return ParameterError(1);
        }
        return Operand.Create(Character.codePointAt(text, 0));
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Unicode");
    }
}
