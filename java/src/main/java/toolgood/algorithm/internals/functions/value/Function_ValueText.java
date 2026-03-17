package toolgood.algorithm.internals.functions.value;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.enums.OperandType;

public class Function_ValueText extends FunctionBase {
    private final Operand _value;
    private final String _showName;

    public Function_ValueText(Operand value) {
        this._value = value;
        this._showName = null;
    }

    public Function_ValueText(Operand value, String showName) {
        this._value = value;
        this._showName = showName;
    }

    @Override
    public String getName() {
        return "Value";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        return _value;
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.TEXT;
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        if (_showName != null && !_showName.isEmpty()) {
            stringBuilder.append(_showName);
            return;
        }
        stringBuilder.append('"');
        String stringValue = _value.TextValue();
        stringValue = stringValue.replace("\\", "\\\\");
        stringValue = stringValue.replace("\r", "\\r");
        stringValue = stringValue.replace("\n", "\\n");
        stringValue = stringValue.replace("\t", "\\t");
        stringValue = stringValue.replace("\0", "\\0");
        stringValue = stringValue.replace("\u000b", "\\v");
        stringValue = stringValue.replace("\u0007", "\\a");
        stringValue = stringValue.replace("\b", "\\b");
        stringValue = stringValue.replace("\f", "\\f");
        stringValue = stringValue.replace("\"", "\\\"");
        stringBuilder.append(stringValue);
        stringBuilder.append('"');
    }
}
