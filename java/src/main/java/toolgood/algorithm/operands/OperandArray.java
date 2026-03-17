package toolgood.algorithm.operands;

import java.util.List;

import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.litJson.JsonMapper;

final class OperandArray extends Operand {
    private final List<Operand> _value;

    public OperandArray(List<Operand> obj) {
        _value = obj;
    }

    @Override
    public boolean IsArray() { return true; }

    @Override
    public OperandType Type() { return OperandType.ARRAY; }

    @Override
    public List<Operand> ArrayValue() { return _value; }

    @Override
    public Operand ToText(String errorMessage) {
        return Create(toString());
    }

    @Override
    public Operand ToText(String errorMessage, Object[] args) {
        return Create(toString());
    }

    @Override
    public Operand ToArray(String errorMessage) { return this; }

    @Override
    public Operand ToArray(String errorMessage, Object[] args) { return this; }

    @Override
    public Operand ToJson(String errorMessage) {
        String txt = toString();
        try {
            return Operand.Create(JsonMapper.ToObject(txt));
        } catch (Exception e) { }
        return Error(errorMessage != null ? errorMessage : "Convert to json error!");
    }

    @Override
    public String toString() {
        StringBuilder stringBuilder = new StringBuilder(_value.size() * 16);
        stringBuilder.append('[');
        for (int i = 0; i < _value.size(); i++) {
            if (i > 0) stringBuilder.append(',');
            stringBuilder.append(_value.get(i).toString());
        }
        stringBuilder.append(']');
        return stringBuilder.toString();
    }
}
