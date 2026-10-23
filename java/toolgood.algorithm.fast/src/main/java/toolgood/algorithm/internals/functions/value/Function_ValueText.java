package toolgood.algorithm.internals.functions.value;

import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.Function_0;

public final class Function_ValueText extends Function_0 {
    private final Operand _value;

    public Function_ValueText(Operand value) {
        _value = value;
    }

    @Override
    public String Name() {
        return "Value";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        return _value;
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        stringBuilder.append('"');
        String stringValue = _value.TextValue();
        stringValue = stringValue.replace("\\", "\\\\");
        stringValue = stringValue.replace("\r", "\\r");
        stringValue = stringValue.replace("\n", "\\n");
        stringValue = stringValue.replace("\t", "\\t");
        stringValue = stringValue.replace("\0", "\\0");
        stringValue = stringValue.replace("\u000B", "\\v");
        stringValue = stringValue.replace("\u0007", "\\a");
        stringValue = stringValue.replace("\b", "\\b");
        stringValue = stringValue.replace("\f", "\\f");
        stringValue = stringValue.replace("\"", "\\\"");
        stringBuilder.append(stringValue);
        stringBuilder.append('"');
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.TEXT;
    }
}
