package toolgood.algorithm.operands;

import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.MyDate;
import toolgood.algorithm.litJson.JsonData;
import toolgood.algorithm.internals.functions.FunctionUtil;
import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;
import java.util.Locale;

class OperandKeyValueList extends Operand {
    private final List<KeyValue> textList;

    public OperandKeyValueList() {
        textList = new ArrayList<>();
    }

    @Override
    public boolean IsArrayJson() { return true; }

    @Override
    public boolean IsNotArrayJson() { return false; }

    @Override
    public OperandType Type() { return OperandType.ARRARYJSON; }

    @Override
    public List<Operand> ArrayValue() {
        List<Operand> result = new ArrayList<>();
        for (KeyValue kv : textList) {
            result.add(kv.value);
        }
        return result;
    }

    @Override
    public Operand ToText(String errorMessage) {
        return Create(this.toString());
    }

    @Override
    public Operand ToText(String errorMessage, Object... args) {
        return Create(this.toString());
    }

    @Override
    public Operand ToArray(String errorMessage) {
        return Create(this.ArrayValue());
    }

    @Override
    public Operand ToArray(String errorMessage, Object... args) {
        return Create(this.ArrayValue());
    }

    @Override
    public Operand ToJson(String errorMessage) {
        String txt = this.toString();
        try {
            JsonData json = JsonData.Parse(txt);
            return Operand.Create(json);
        } catch (Exception e) {
            return Error(errorMessage != null ? errorMessage : "Convert to json error!");
        }
    }

    public void addValue(KeyValue keyValue) {
        textList.add(keyValue);
    }

    public boolean tryGetValue(String key, Operand[] value) {
        for (KeyValue item : textList) {
            if (item.key.equals(key)) {
                value[0] = item.value;
                return true;
            }
        }
        return false;
    }

    public boolean containsKey(Operand value) {
        for (KeyValue item : textList) {
            if (value.TextValue().equals(item.key)) {
                return true;
            }
        }
        return false;
    }

    public boolean containsValue(Operand value) {
        for (KeyValue item : textList) {
            Operand op = item.value;
            if (value.Type() != op.Type()) {
                continue;
            }
            if (value.IsText()) {
                if (value.TextValue().equals(op.TextValue())) {
                    return true;
                }
            }
        }
        return false;
    }

    @Override
    public String toString() {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.append('{');
        for (int i = 0; i < textList.size(); i++) {
            if (i > 0) {
                stringBuilder.append(',');
            }
            stringBuilder.append('"');
            stringBuilder.append(textList.get(i).key);
            stringBuilder.append('"');
            stringBuilder.append(':');
            stringBuilder.append(textList.get(i).value.toString());
        }
        stringBuilder.append('}');
        return stringBuilder.toString();
    }
}