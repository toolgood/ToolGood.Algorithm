package toolgood.algorithm.internals.functions.value;

import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.internals.AlgorithmEngine;

public class Function_Value extends FunctionBase {
    private final Operand _value;
    private final String _showName;

    public Function_Value(Operand value) {
        this._value = value;
        this._showName = null;
    }

    public Function_Value(Operand value, String showName) {
        this._value = value;
        this._showName = showName;
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        return _value;
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        if (_showName != null && !_showName.isEmpty()) {
            stringBuilder.append(_showName);
            return;
        }
        if (_value.IsText()) {
            stringBuilder.append('"');
            String stringValue = _value.getTextValue();
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
        } else if (_value.IsDate()) {
            stringBuilder.append('"');
            stringBuilder.append(_value.getDateValue().toString());
            stringBuilder.append('"');
        } else if (_value.IsBoolean()) {
            stringBuilder.append(_value.getBooleanValue() ? "true" : "false");
        } else {
            stringBuilder.append(_value.toString());
        }
    }
}
