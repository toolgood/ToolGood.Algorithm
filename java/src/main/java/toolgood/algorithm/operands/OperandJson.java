package toolgood.algorithm.operands;

import java.util.ArrayList;
import java.util.List;

import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.litJson.JsonData;

final class OperandJson extends Operand {
    private final JsonData _value;

    public OperandJson(JsonData obj) {
        _value = obj;
    }

    @Override
    public boolean IsJson() { return true; }

    @Override
    public OperandType Type() { return OperandType.JSON; }

    @Override
    public JsonData JsonValue() { return _value; }

    @Override
    public Operand ToText(String errorMessage) {
        return Create(_value.toString());
    }

    @Override
    public Operand ToText(String errorMessage, Object[] args) {
        return Create(_value.toString());
    }

    @Override
    public Operand ToArray(String errorMessage) {
        return ToArrayInternal(errorMessage);
    }

    @Override
    public Operand ToArray(String errorMessage, Object[] args) {
        return ToArrayInternal(String.format(errorMessage, args));
    }

    private Operand ToArrayInternal(String errorMessage) {
        if (JsonValue().IsArray()) {
            List<Operand> list = new ArrayList<>(JsonValue().Count());
            for (JsonData v : JsonValue().getArray()) {
                if (v.IsString()) {
                    list.add(Operand.Create(v.getString()));
                } else if (v.IsBoolean()) {
                    list.add(Operand.Create(v.getBoolean()));
                } else if (v.IsDouble()) {
                    list.add(Operand.Create(v.getNumber()));
                } else if (v.IsNull()) {
                    list.add(Operand.Null);
                } else {
                    list.add(Operand.Create(v));
                }
            }
            return Create(list);
        }
        return Error(errorMessage != null ? errorMessage : "Convert to array error!");
    }

    @Override
    public Operand ToJson(String errorMessage) {
        return this;
    }

    @Override
    public String toString() {
        return _value.toString();
    }
}
