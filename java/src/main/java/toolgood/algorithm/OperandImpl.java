package toolgood.algorithm;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;
import java.util.Locale;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.MyDate;
import toolgood.algorithm.litJson.JsonData;
import toolgood.algorithm.internals.functions.FunctionUtil;

class OperandInt extends Operand {
    private final int value;

    public OperandInt(int obj) {
        value = obj;
    }

    @Override
    public boolean IsNumber() { return true; }

    @Override
    public boolean IsNotNumber() { return false; }

    @Override
    public OperandType Type() { return OperandType.NUMBER; }

    @Override
    public int IntValue() { return value; }

    @Override
    public BigDecimal NumberValue() { return BigDecimal.valueOf(value); }

    @Override
    public long LongValue() { return value; }

    @Override
    public double DoubleValue() { return value; }


    @Override
    public Operand ToNumber(String errorMessage) { return this; }

    @Override
    public Operand ToNumber(String errorMessage, Object... args) { return this; }

    @Override
    public Operand ToBoolean(String errorMessage) { return IntValue() != 0 ? TRUE : FALSE; }

    @Override
    public Operand ToBoolean(String errorMessage, Object... args) { return IntValue() != 0 ? TRUE : FALSE; }

    @Override
    public Operand ToText(String errorMessage) { return Create(Integer.toString(value)); }

    @Override
    public Operand ToText(String errorMessage, Object... args) { return Create(Integer.toString(value)); }

    @Override
    public Operand ToMyDate(String errorMessage) { return Create(new MyDate(value)); }

    @Override
    public Operand ToMyDate(String errorMessage, Object... args) { return Create(new MyDate(value)); }

    @Override
    public String toString() { return NumberValue().toString(); }
}

class OperandBigDecimal extends Operand {
    private final BigDecimal value;

    public OperandBigDecimal(BigDecimal obj) {
        value = obj;
    }

    @Override
    public boolean IsNumber() { return true; }

    @Override
    public boolean IsNotNumber() { return false; }

    @Override
    public OperandType Type() { return OperandType.NUMBER; }

    @Override
    public int IntValue() { return value.intValue(); }

    @Override
    public BigDecimal NumberValue() { return value; }

    @Override
    public long LongValue() { return value.longValue(); }

    @Override
    public double DoubleValue() { return value.doubleValue(); }


    @Override
    public Operand ToNumber(String errorMessage) { return this; }

    @Override
    public Operand ToNumber(String errorMessage, Object... args) { return this; }

    @Override
    public Operand ToBoolean(String errorMessage) { return value.compareTo(BigDecimal.ZERO) != 0 ? TRUE : FALSE; }

    @Override
    public Operand ToBoolean(String errorMessage, Object... args) { return value.compareTo(BigDecimal.ZERO) != 0 ? TRUE : FALSE; }

    @Override
    public Operand ToText(String errorMessage) { return Create(value.toString()); }

    @Override
    public Operand ToText(String errorMessage, Object... args) { return Create(value.toString()); }

    @Override
    public Operand ToMyDate(String errorMessage) { return Create(new MyDate(value.longValue())); }

    @Override
    public Operand ToMyDate(String errorMessage, Object... args) { return Create(new MyDate(value.longValue())); }

    @Override
    public String toString() { return NumberValue().toString(); }
}



class OperandBoolean extends Operand {
    private final boolean value;

    public OperandBoolean(boolean obj) {
        value = obj;
    }

    @Override
    public boolean IsBoolean() { return true; }

    @Override
    public boolean IsNotBoolean() { return false; }

    @Override
    public OperandType Type() { return OperandType.BOOLEAN; }

    @Override
    public boolean BooleanValue() { return value; }

    @Override
    public Operand ToNumber(String errorMessage) { return value ? ONE : ZERO; }

    @Override
    public Operand ToNumber(String errorMessage, Object... args) { return value ? ONE : ZERO; }

    @Override
    public Operand ToBoolean(String errorMessage) { return this; }

    @Override
    public Operand ToBoolean(String errorMessage, Object... args) { return this; }

    @Override
    public Operand ToText(String errorMessage) { return Create(value ? "TRUE" : "FALSE"); }

    @Override
    public Operand ToText(String errorMessage, Object... args) { return Create(value ? "TRUE" : "FALSE"); }

    @Override
    public String toString() { return value ? "true" : "false"; }
}

class OperandString extends Operand {
    private final String value;

    public OperandString(String obj) {
        value = obj;
    }

    @Override
    public boolean IsText() { return true; }

    @Override
    public boolean IsNotText() { return false; }

    @Override
    public OperandType Type() { return OperandType.TEXT; }

    @Override
    public String TextValue() { return value; }

    @Override
    public Operand ToNumber(String errorMessage) {
        try {
            BigDecimal bd = new BigDecimal(value);
            return Operand.Create(bd);
        } catch (NumberFormatException e) {
            if (errorMessage == null) {
                return Error("Convert to number error!");
            }
            return Error(errorMessage);
        }
    }

    @Override
    public Operand ToNumber(String errorMessage, Object... args) {
        try {
            BigDecimal bd = new BigDecimal(value);
            return Operand.Create(bd);
        } catch (NumberFormatException e) {
            if (errorMessage == null) {
                return Error("Convert to number error!");
            }
            return Error(String.format(errorMessage, args));
        }
    }

    @Override
    public Operand ToText(String errorMessage) { return this; }

    @Override
    public Operand ToText(String errorMessage, Object... args) { return this; }

    @Override
    public Operand ToBoolean(String errorMessage) {
        FunctionUtil.BooleanHolder boolHolder = new FunctionUtil.BooleanHolder();
        if (FunctionUtil.tryParseBoolean(value, boolHolder)) {
            return boolHolder.value ? Operand.TRUE : Operand.FALSE;
        }
        if (errorMessage == null) {
            return Error("Convert to bool error!");
        }
        return Error(errorMessage);
    }

    @Override
    public Operand ToBoolean(String errorMessage, Object... args) {
        FunctionUtil.BooleanHolder boolHolder = new FunctionUtil.BooleanHolder();
        if (FunctionUtil.tryParseBoolean(value, boolHolder)) {
            return boolHolder.value ? Operand.TRUE : Operand.FALSE;
        }
        if (errorMessage == null) {
            return Error("Convert to bool error!");
        }
        return Error(String.format(errorMessage, args));
    }

    @Override
    public Operand ToMyDate(String errorMessage) {
        try {
            // 尝试解析为时间戳
            long timestamp = Long.parseLong(value);
            return Create(new MyDate(timestamp));
        } catch (NumberFormatException e) {
            // 尝试解析为日期字符串
            try {
                MyDate date = MyDate.parse(value);
                return Create(date);
            } catch (Exception ex) {
                if (errorMessage == null) {
                    return Error("Convert to date error!");
                }
                return Error(errorMessage);
            }
        }
    }

    @Override
    public Operand ToMyDate(String errorMessage, Object... args) {
        try {
            // 尝试解析为时间戳
            long timestamp = Long.parseLong(value);
            return Create(new MyDate(timestamp));
        } catch (NumberFormatException e) {
            // 尝试解析为日期字符串
            try {
                MyDate date = MyDate.parse(value);
                return Create(date);
            } catch (Exception ex) {
                if (errorMessage == null) {
                    return Error("Convert to date error!");
                }
                return Error(String.format(errorMessage, args));
            }
        }
    }

    @Override
    public Operand ToArray(String errorMessage) {
        return Error(errorMessage != null ? errorMessage : "Convert to array error!");
    }

    @Override
    public Operand ToJson(String errorMessage) {
        String txt = value.trim();
        if ((txt.startsWith("{") && txt.endsWith("}")) || (txt.startsWith("[") && txt.endsWith("]"))) {
            try {
                JsonData json = JsonData.Parse(txt);
                return Operand.Create(json);
            } catch (Exception e) {
            }
        }
        return Error(errorMessage != null ? errorMessage : "Convert to json error!");
    }

    @Override
    public String toString() {
        StringBuilder sb = new StringBuilder();
        sb.append('"');
        for (char c : value.toCharArray()) {
            switch (c) {
                case '"':
                    sb.append("\\\"");
                    break;
                case '\n':
                    sb.append("\\n");
                    break;
                case '\r':
                    sb.append("\\r");
                    break;
                case '\t':
                    sb.append("\\t");
                    break;
                case '\0':
                    sb.append("\\0");
                    break;
                case '\v':
                    sb.append("\\v");
                    break;
                case '\a':
                    sb.append("\\a");
                    break;
                case '\b':
                    sb.append("\\b");
                    break;
                case '\f':
                    sb.append("\\f");
                    break;
                default:
                    sb.append(c);
                    break;
            }
        }
        sb.append('"');
        return sb.toString();
    }
}

class OperandMyDate extends Operand {
    private final MyDate value;

    public OperandMyDate(MyDate obj) {
        value = obj;
    }

    @Override
    public boolean IsDate() { return true; }

    @Override
    public boolean IsNotDate() { return false; }

    @Override
    public OperandType Type() { return OperandType.DATE; }

    @Override
    public MyDate DateValue() { return value; }

    @Override
    public Operand ToNumber(String errorMessage) { return Create(value.ToNumber
()); }

    @Override
    public Operand ToNumber(String errorMessage, Object... args) { return Create(value.ToNumber
()); }

    @Override
    public Operand ToBoolean(String errorMessage) { return value.ToNumber
() != 0 ? TRUE : FALSE; }

    @Override
    public Operand ToBoolean(String errorMessage, Object... args) { return value.ToNumber
() != 0 ? TRUE : FALSE; }

    @Override
    public Operand ToText(String errorMessage) { return Create(value.toString()); }

    @Override
    public Operand ToText(String errorMessage, Object... args) { return Create(value.toString()); }

    @Override
    public Operand ToMyDate(String errorMessage) { return this; }

    @Override
    public Operand ToMyDate(String errorMessage, Object... args) { return this; }

    @Override
    public String toString() { return '"' + value.toString() + '"'; }
}

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
                if (v.isString()) {
                    list.add(Operand.Create(v.getString()));
                } else if (v.IsBoolean()) {
                    list.add(Operand.Create(v.getBoolean()));
                } else if (v.IsNumber()) {
                    list.add(Operand.Create(new BigDecimal(v.getNumber().toString())));
                } else if (v.isNull()) {
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

class OperandError extends Operand {
    private final String errorMsg;

    public OperandError(String msg) {
        errorMsg = msg;
    }

    @Override
    public OperandType Type() { return OperandType.ERROR; }

    @Override
    public boolean IsError() { return true; }

    @Override
    public String ErrorMsg() { return errorMsg; }

    @Override
    public Operand ToNumber(String errorMessage) { return this; }

    @Override
    public Operand ToNumber(String errorMessage, Object... args) { return this; }

    @Override
    public Operand ToBoolean(String errorMessage) { return this; }

    @Override
    public Operand ToBoolean(String errorMessage, Object... args) { return this; }

    @Override
    public Operand ToText(String errorMessage) { return this; }

    @Override
    public Operand ToText(String errorMessage, Object... args) { return this; }

    @Override
    public Operand ToArray(String errorMessage) { return this; }

    @Override
    public Operand ToArray(String errorMessage, Object... args) { return this; }

    @Override
    public Operand ToMyDate(String errorMessage) { return this; }

    @Override
    public Operand ToMyDate(String errorMessage, Object... args) { return this; }
}

class OperandNull extends Operand {
    @Override
    public boolean IsNull() { return true; }

    @Override
    public boolean IsNotNull() { return false; }

    @Override
    public OperandType Type() { return OperandType.NULL; }

    @Override
    public String toString() { return "null"; }
}

class KeyValue {
    public String key;
    public Operand value;
}

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

class OperandKeyValue extends Operand {
    private final KeyValue value;

    public OperandKeyValue(KeyValue obj) {
        value = obj;
    }

    @Override
    public boolean IsArrayJson() { return true; }

    @Override
    public boolean IsNotArrayJson() { return false; }

    @Override
    public OperandType Type() { return OperandType.ARRARYJSON; }

    public KeyValue Value() { return value; }
}

