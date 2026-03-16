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
                case '\u000b':
                    sb.append("\\v");
                    break;
                case '\u0007':
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