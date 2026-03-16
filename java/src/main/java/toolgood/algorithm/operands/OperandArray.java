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

class OperandArray extends Operand {
    private final List<Operand> value;

    public OperandArray(List<Operand> obj) {
        value = obj;
    }

    @Override
    public boolean IsArray() { return true; }

    @Override
    public boolean IsNotArray() { return false; }

    @Override
    public OperandType Type() { return OperandType.ARRARY; }

    @Override
    public List<Operand> ArrayValue() { return value; }

    @Override
    public Operand ToText(String errorMessage) {
        return Create(this.toString());
    }

    @Override
    public Operand ToText(String errorMessage, Object... args) {
        return Create(this.toString());
    }

    @Override
    public Operand ToArray(String errorMessage) { return this; }

    @Override
    public Operand ToArray(String errorMessage, Object... args) { return this; }

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

    @Override
    public String toString() {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.append('[');
        for (int i = 0; i < value.size(); i++) {
            if (i > 0) {
                stringBuilder.append(',');
            }
            stringBuilder.append(value.get(i).toString());
        }
        stringBuilder.append(']');
        return stringBuilder.toString();
    }
}