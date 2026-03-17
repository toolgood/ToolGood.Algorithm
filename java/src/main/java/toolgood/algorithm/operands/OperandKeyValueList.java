package toolgood.algorithm.operands;

import java.util.ArrayList;
import java.util.List;

import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.litJson.JsonMapper;

public final class OperandKeyValueList extends Operand {
    private final List<KeyValue> _keyValueList;

    public OperandKeyValueList() {
        _keyValueList = new ArrayList<>();
    }

    @Override
    public boolean IsArrayJson() { return true; }

    @Override
    public OperandType Type() { return OperandType.ARRAYJSON; }

    @Override
    public List<Operand> ArrayValue() {
        List<Operand> result = new ArrayList<>(_keyValueList.size());
        for (KeyValue kv : _keyValueList) {
            result.add(kv.Value);
        }
        return result;
    }

    @Override
    public Operand ToText(String errorMessage) {
        return Create(toString());
    }

    @Override
    public Operand ToText(String errorMessage, Object[] args) {
        return Create(toString());
    }

    @Override
    public Operand ToArray(String errorMessage) {
        return Create(ArrayValue());
    }

    @Override
    public Operand ToArray(String errorMessage, Object[] args) {
        return Create(ArrayValue());
    }

    @Override
    public Operand ToJson(String errorMessage) {
        String txt = toString();
        try {
            return Operand.Create(JsonMapper.ToObject(txt));
        } catch (Exception e) { }
        return Error(errorMessage != null ? errorMessage : "Convert to json error!");
    }

    public void AddValue(KeyValue keyValue) {
        _keyValueList.add(keyValue);
    }

    public boolean TryGetValue(String key, Operand[] value) {
        for (KeyValue item : _keyValueList) {
            if (item.Key.equals(key)) {
                value[0] = item.Value;
                return true;
            }
        }
        return false;
    }

    public boolean ContainsKey(Operand value) {
        for (KeyValue item : _keyValueList) {
            if (item.Key.equals(value.TextValue())) {
                return true;
            }
        }
        return false;
    }

    public boolean ContainsValue(Operand value) {
        for (KeyValue item : _keyValueList) {
            Operand op = item.Value;
            if (value.Type() != op.Type()) {
                continue;
            }
            if (value.IsText() && value.TextValue().equals(op.TextValue())) {
                return true;
            }
        }
        return false;
    }

    @Override
    public String toString() {
        StringBuilder stringBuilder = new StringBuilder(_keyValueList.size() * 32);
        stringBuilder.append('{');
        for (int i = 0; i < _keyValueList.size(); i++) {
            if (i > 0) stringBuilder.append(',');
            stringBuilder.append('"');
            stringBuilder.append(_keyValueList.get(i).Key);
            stringBuilder.append('"');
            stringBuilder.append(':');
            stringBuilder.append(_keyValueList.get(i).Value.toString());
        }
        stringBuilder.append('}');
        return stringBuilder.toString();
    }
}
