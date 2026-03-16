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

class OperandJson extends Operand {
    private final JsonData value;

    public OperandJson(JsonData obj) {
        value = obj;
    }

    @Override
    public boolean IsJson() { return true; }

    @Override
    public boolean IsNotJson() { return false; }

    @Override
    public OperandType Type() { return OperandType.JSON; }

    @Override
    JsonData JsonValue() { return value; }

    @Override
    public Operand ToText(String errorMessage) {
        return Create(value.toString());
    }

    @Override
    public Operand ToText(String errorMessage, Object... args) {
        return Create(value.toString());
    }

    @Override
    public Operand ToArray(String errorMessage) {
        if (value.IsArray()) {
            List<Operand> list = new ArrayList<>();
            for (JsonData v : value.getArray()) {
                if (v.IsString()) {
                    list.add(Operand.Create(v.getString()));
                } else if (v.IsBoolean()) {
                    list.add(Operand.Create(v.getBoolean()));
                } else if (v.IsNumber()) {
                    list.add(Operand.Create(new BigDecimal(v.getNumber().toString())));
                } else if (v.IsNull()) {
                    list.add(Operand.CreateNull());
                } else {
                    list.add(Operand.Create(v));
                }
            }
            return Operand.Create(list);
        }
        return Error(errorMessage != null ? errorMessage : "Convert to array error!");
    }

    @Override
    public Operand ToArray(String errorMessage, Object... args) {
        if (value.IsArray()) {
            List<Operand> list = new ArrayList<>();
            for (JsonData v : value.getArray()) {
                if (v.IsString()) {
                    list.add(Operand.Create(v.getString()));
                } else if (v.IsBoolean()) {
                    list.add(Operand.Create(v.getBoolean()));
                } else if (v.IsNumber()) {
                    list.add(Operand.Create(new BigDecimal(v.getNumber().toString())));
                } else if (v.IsNull()) {
                    list.add(Operand.CreateNull());
                } else {
                    list.add(Operand.Create(v));
                }
            }
            return Operand.Create(list);
        }
        return Error(String.format(errorMessage, args));
    }

    @Override
    public Operand ToJson(String errorMessage) {
        return this;
    }

    @Override
    public String toString() {
        return value.toString();
    }
}