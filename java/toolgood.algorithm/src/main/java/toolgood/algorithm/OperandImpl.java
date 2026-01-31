package toolgood.algorithm;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;
import java.util.Locale;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.MyDate;
import toolgood.algorithm.internals.JsonData;
import toolgood.algorithm.internals.functions.FunctionUtil;

class OperandInt extends Operand {
    private final int value;

    public OperandInt(int obj) {
        value = obj;
    }

    @Override
    public boolean isNumber() { return true; }

    @Override
    public boolean isNotNumber() { return false; }

    @Override
    public OperandType getType() { return OperandType.NUMBER; }

    @Override
    public int getIntValue() { return value; }

    @Override
    public BigDecimal getNumberValue() { return BigDecimal.valueOf(value); }

    @Override
    public long getLongValue() { return value; }

    @Override
    public double getDoubleValue() { return value; }

    @Override
    public BigDecimal getBigDecimalValue() { return BigDecimal.valueOf(value); }

    @Override
    public Operand toNumber(String errorMessage) { return this; }

    @Override
    public Operand toNumber(String errorMessage, Object... args) { return this; }

    @Override
    public Operand toBoolean(String errorMessage) { return getIntValue() != 0 ? TRUE : FALSE; }

    @Override
    public Operand toBoolean(String errorMessage, Object... args) { return getIntValue() != 0 ? TRUE : FALSE; }

    @Override
    public Operand toText(String errorMessage) { return create(Integer.toString(value)); }

    @Override
    public Operand toText(String errorMessage, Object... args) { return create(Integer.toString(value)); }

    @Override
    public Operand toMyDate(String errorMessage) { return create(new MyDate(value)); }

    @Override
    public Operand toMyDate(String errorMessage, Object... args) { return create(new MyDate(value)); }

    @Override
    public String toString() { return getNumberValue().toString(); }
}

class OperandBigDecimal extends Operand {
    private final BigDecimal value;

    public OperandBigDecimal(BigDecimal obj) {
        value = obj;
    }

    @Override
    public boolean isNumber() { return true; }

    @Override
    public boolean isNotNumber() { return false; }

    @Override
    public OperandType getType() { return OperandType.NUMBER; }

    @Override
    public int getIntValue() { return value.intValue(); }

    @Override
    public BigDecimal getNumberValue() { return value; }

    @Override
    public long getLongValue() { return value.longValue(); }

    @Override
    public double getDoubleValue() { return value.doubleValue(); }

    @Override
    public BigDecimal getBigDecimalValue() { return value; }

    @Override
    public Operand toNumber(String errorMessage) { return this; }

    @Override
    public Operand toNumber(String errorMessage, Object... args) { return this; }

    @Override
    public Operand toBoolean(String errorMessage) { return value.compareTo(BigDecimal.ZERO) != 0 ? TRUE : FALSE; }

    @Override
    public Operand toBoolean(String errorMessage, Object... args) { return value.compareTo(BigDecimal.ZERO) != 0 ? TRUE : FALSE; }

    @Override
    public Operand toText(String errorMessage) { return create(value.toString()); }

    @Override
    public Operand toText(String errorMessage, Object... args) { return create(value.toString()); }

    @Override
    public Operand toMyDate(String errorMessage) { return create(new MyDate(value.longValue())); }

    @Override
    public Operand toMyDate(String errorMessage, Object... args) { return create(new MyDate(value.longValue())); }

    @Override
    public String toString() { return getNumberValue().toString(); }
}



class OperandBoolean extends Operand {
    private final boolean value;

    public OperandBoolean(boolean obj) {
        value = obj;
    }

    @Override
    public boolean isBoolean() { return true; }

    @Override
    public boolean isNotBoolean() { return false; }

    @Override
    public OperandType getType() { return OperandType.BOOLEAN; }

    @Override
    public boolean getBooleanValue() { return value; }

    @Override
    public Operand toNumber(String errorMessage) { return value ? ONE : ZERO; }

    @Override
    public Operand toNumber(String errorMessage, Object... args) { return value ? ONE : ZERO; }

    @Override
    public Operand toBoolean(String errorMessage) { return this; }

    @Override
    public Operand toBoolean(String errorMessage, Object... args) { return this; }

    @Override
    public Operand toText(String errorMessage) { return create(value ? "TRUE" : "FALSE"); }

    @Override
    public Operand toText(String errorMessage, Object... args) { return create(value ? "TRUE" : "FALSE"); }

    @Override
    public String toString() { return value ? "true" : "false"; }
}

class OperandString extends Operand {
    private final String value;

    public OperandString(String obj) {
        value = obj;
    }

    @Override
    public boolean isText() { return true; }

    @Override
    public boolean isNotText() { return false; }

    @Override
    public OperandType getType() { return OperandType.TEXT; }

    @Override
    public String getTextValue() { return value; }

    @Override
    public Operand toNumber(String errorMessage) {
        try {
            BigDecimal bd = new BigDecimal(value);
            return Operand.create(bd);
        } catch (NumberFormatException e) {
            if (errorMessage == null) {
                return error("Convert to number error!");
            }
            return error(errorMessage);
        }
    }

    @Override
    public Operand toNumber(String errorMessage, Object... args) {
        try {
            BigDecimal bd = new BigDecimal(value);
            return Operand.create(bd);
        } catch (NumberFormatException e) {
            if (errorMessage == null) {
                return error("Convert to number error!");
            }
            return error(String.format(errorMessage, args));
        }
    }

    @Override
    public Operand toText(String errorMessage) { return this; }

    @Override
    public Operand toText(String errorMessage, Object... args) { return this; }

    @Override
    public Operand toBoolean(String errorMessage) {
        FunctionUtil.BooleanHolder boolHolder = new FunctionUtil.BooleanHolder();
        if (FunctionUtil.tryParseBoolean(value, boolHolder)) {
            return boolHolder.value ? Operand.TRUE : Operand.FALSE;
        }
        if (errorMessage == null) {
            return error("Convert to bool error!");
        }
        return error(errorMessage);
    }

    @Override
    public Operand toBoolean(String errorMessage, Object... args) {
        FunctionUtil.BooleanHolder boolHolder = new FunctionUtil.BooleanHolder();
        if (FunctionUtil.tryParseBoolean(value, boolHolder)) {
            return boolHolder.value ? Operand.TRUE : Operand.FALSE;
        }
        if (errorMessage == null) {
            return error("Convert to bool error!");
        }
        return error(String.format(errorMessage, args));
    }

    @Override
    public Operand toMyDate(String errorMessage) {
        try {
            // 尝试解析为时间戳
            long timestamp = Long.parseLong(value);
            return create(new MyDate(timestamp));
        } catch (NumberFormatException e) {
            // 尝试解析为日期字符串
            try {
                MyDate date = MyDate.parse(value);
                return create(date);
            } catch (Exception ex) {
                if (errorMessage == null) {
                    return error("Convert to date error!");
                }
                return error(errorMessage);
            }
        }
    }

    @Override
    public Operand toMyDate(String errorMessage, Object... args) {
        try {
            // 尝试解析为时间戳
            long timestamp = Long.parseLong(value);
            return create(new MyDate(timestamp));
        } catch (NumberFormatException e) {
            // 尝试解析为日期字符串
            try {
                MyDate date = MyDate.parse(value);
                return create(date);
            } catch (Exception ex) {
                if (errorMessage == null) {
                    return error("Convert to date error!");
                }
                return error(String.format(errorMessage, args));
            }
        }
    }

    @Override
    public Operand toArray(String errorMessage) {
        return error(errorMessage != null ? errorMessage : "Convert to array error!");
    }

    @Override
    public Operand toJson(String errorMessage) {
        String txt = value.trim();
        if ((txt.startsWith("{") && txt.endsWith("}")) || (txt.startsWith("[") && txt.endsWith("]"))) {
            try {
                JsonData json = JsonData.parse(txt);
                return Operand.create(json);
            } catch (Exception e) {
            }
        }
        return error(errorMessage != null ? errorMessage : "Convert to json error!");
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
    public boolean isDate() { return true; }

    @Override
    public boolean isNotDate() { return false; }

    @Override
    public OperandType getType() { return OperandType.DATE; }

    @Override
    public MyDate getDateValue() { return value; }

    @Override
    public Operand toNumber(String errorMessage) { return create(value.toTimestamp()); }

    @Override
    public Operand toNumber(String errorMessage, Object... args) { return create(value.toTimestamp()); }

    @Override
    public Operand toBoolean(String errorMessage) { return value.toTimestamp() != 0 ? TRUE : FALSE; }

    @Override
    public Operand toBoolean(String errorMessage, Object... args) { return value.toTimestamp() != 0 ? TRUE : FALSE; }

    @Override
    public Operand toText(String errorMessage) { return create(value.toString()); }

    @Override
    public Operand toText(String errorMessage, Object... args) { return create(value.toString()); }

    @Override
    public Operand toMyDate(String errorMessage) { return this; }

    @Override
    public Operand toMyDate(String errorMessage, Object... args) { return this; }

    @Override
    public String toString() { return '"' + value.toString() + '"'; }
}

class OperandJson extends Operand {
    private final JsonData value;

    public OperandJson(JsonData obj) {
        value = obj;
    }

    @Override
    public boolean isJson() { return true; }

    @Override
    public boolean isNotJson() { return false; }

    @Override
    public OperandType getType() { return OperandType.JSON; }

    @Override
    JsonData getJsonValue() { return value; }

    @Override
    public Operand toText(String errorMessage) {
        return create(value.toString());
    }

    @Override
    public Operand toText(String errorMessage, Object... args) {
        return create(value.toString());
    }

    @Override
    public Operand toArray(String errorMessage) {
        if (value.isArray()) {
            List<Operand> list = new ArrayList<>();
            for (JsonData v : value.getArray()) {
                if (v.isString()) {
                    list.add(Operand.create(v.getString()));
                } else if (v.isBoolean()) {
                    list.add(Operand.create(v.getBoolean()));
                } else if (v.isNumber()) {
                    list.add(Operand.create(new BigDecimal(v.getNumber().toString())));
                } else if (v.isNull()) {
                    list.add(Operand.createNull());
                } else {
                    list.add(Operand.create(v));
                }
            }
            return Operand.create(list);
        }
        return error(errorMessage != null ? errorMessage : "Convert to array error!");
    }

    @Override
    public Operand toArray(String errorMessage, Object... args) {
        if (value.isArray()) {
            List<Operand> list = new ArrayList<>();
            for (JsonData v : value.getArray()) {
                if (v.isString()) {
                    list.add(Operand.create(v.getString()));
                } else if (v.isBoolean()) {
                    list.add(Operand.create(v.getBoolean()));
                } else if (v.isNumber()) {
                    list.add(Operand.create(new BigDecimal(v.getNumber().toString())));
                } else if (v.isNull()) {
                    list.add(Operand.createNull());
                } else {
                    list.add(Operand.create(v));
                }
            }
            return Operand.create(list);
        }
        return error(String.format(errorMessage, args));
    }

    @Override
    public Operand toJson(String errorMessage) {
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
    public boolean isArray() { return true; }

    @Override
    public boolean isNotArray() { return false; }

    @Override
    public OperandType getType() { return OperandType.ARRARY; }

    @Override
    public List<Operand> getArrayValue() { return value; }

    @Override
    public Operand toText(String errorMessage) {
        return create(this.toString());
    }

    @Override
    public Operand toText(String errorMessage, Object... args) {
        return create(this.toString());
    }

    @Override
    public Operand toArray(String errorMessage) { return this; }

    @Override
    public Operand toArray(String errorMessage, Object... args) { return this; }

    @Override
    public Operand toJson(String errorMessage) {
        String txt = this.toString();
        try {
            JsonData json = JsonData.parse(txt);
            return Operand.create(json);
        } catch (Exception e) {
            return error(errorMessage != null ? errorMessage : "Convert to json error!");
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
    public OperandType getType() { return OperandType.ERROR; }

    @Override
    public boolean isError() { return true; }

    @Override
    public String getErrorMsg() { return errorMsg; }

    @Override
    public Operand toNumber(String errorMessage) { return this; }

    @Override
    public Operand toNumber(String errorMessage, Object... args) { return this; }

    @Override
    public Operand toBoolean(String errorMessage) { return this; }

    @Override
    public Operand toBoolean(String errorMessage, Object... args) { return this; }

    @Override
    public Operand toText(String errorMessage) { return this; }

    @Override
    public Operand toText(String errorMessage, Object... args) { return this; }

    @Override
    public Operand toArray(String errorMessage) { return this; }

    @Override
    public Operand toArray(String errorMessage, Object... args) { return this; }

    @Override
    public Operand toMyDate(String errorMessage) { return this; }

    @Override
    public Operand toMyDate(String errorMessage, Object... args) { return this; }
}

class OperandNull extends Operand {
    @Override
    public boolean isNull() { return true; }

    @Override
    public boolean isNotNull() { return false; }

    @Override
    public OperandType getType() { return OperandType.NULL; }

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
    public boolean isArrayJson() { return true; }

    @Override
    public boolean isNotArrayJson() { return false; }

    @Override
    public OperandType getType() { return OperandType.ARRARYJSON; }

    @Override
    public List<Operand> getArrayValue() {
        List<Operand> result = new ArrayList<>();
        for (KeyValue kv : textList) {
            result.add(kv.value);
        }
        return result;
    }

    @Override
    public Operand toText(String errorMessage) {
        return create(this.toString());
    }

    @Override
    public Operand toText(String errorMessage, Object... args) {
        return create(this.toString());
    }

    @Override
    public Operand toArray(String errorMessage) {
        return create(this.getArrayValue());
    }

    @Override
    public Operand toArray(String errorMessage, Object... args) {
        return create(this.getArrayValue());
    }

    @Override
    public Operand toJson(String errorMessage) {
        String txt = this.toString();
        try {
            JsonData json = JsonData.parse(txt);
            return Operand.create(json);
        } catch (Exception e) {
            return error(errorMessage != null ? errorMessage : "Convert to json error!");
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
            if (value.getTextValue().equals(item.key)) {
                return true;
            }
        }
        return false;
    }

    public boolean containsValue(Operand value) {
        for (KeyValue item : textList) {
            Operand op = item.value;
            if (value.getType() != op.getType()) {
                continue;
            }
            if (value.isText()) {
                if (value.getTextValue().equals(op.getTextValue())) {
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
    public boolean isArrayJson() { return true; }

    @Override
    public boolean isNotArrayJson() { return false; }

    @Override
    public OperandType getType() { return OperandType.ARRARYJSON; }

    public KeyValue getValue() { return value; }
}

#endregion
