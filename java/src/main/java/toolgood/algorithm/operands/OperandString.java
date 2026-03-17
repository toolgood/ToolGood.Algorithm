package toolgood.algorithm.operands;

import java.math.BigDecimal;

import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.litJson.JsonData;

final class OperandString extends Operand {
    private final String _value;

    public OperandString(String obj) {
        _value = obj;
    }

    @Override
    public boolean IsText() { return true; }

    @Override
    public OperandType Type() { return OperandType.TEXT; }

    @Override
    public String TextValue() { return _value; }

    @Override
    public Operand ToNumber(String errorMessage) {
        return ToNumberInternal(errorMessage);
    }

    @Override
    public Operand ToNumber(String errorMessage, Object[] args) {
        return ToNumberInternal(String.format(errorMessage, args));
    }

    private Operand ToNumberInternal(String errorMessage) {
        String text = TextValue();
        if (text.indexOf('.') < 0) {
            try {
                int num = Integer.parseInt(text);
                return Operand.Create(num);
            } catch (NumberFormatException e) { }
        }
        try {
            BigDecimal d = new BigDecimal(text);
            return Operand.Create(d);
        } catch (NumberFormatException e) { }
        return Error(errorMessage != null ? errorMessage : "Convert to number error!");
    }

    @Override
    public Operand ToText(String errorMessage) { return this; }

    @Override
    public Operand ToText(String errorMessage, Object[] args) { return this; }

    @Override
    public Operand ToBoolean(String errorMessage) {
        return ToBooleanInternal(errorMessage);
    }

    @Override
    public Operand ToBoolean(String errorMessage, Object[] args) {
        return ToBooleanInternal(String.format(errorMessage, args));
    }

    private Operand ToBooleanInternal(String errorMessage) {
        boolean[] result = new boolean[1];
        if (FunctionUtil.TryParseBoolean(TextValue(), result)) {
            return result[0] ? Operand.True : Operand.False;
        }
        return Error(errorMessage != null ? errorMessage : "Convert to bool error!");
    }

    @Override
    public Operand ToMyDate(String errorMessage) {
        return ToMyDateInternal(errorMessage);
    }

    @Override
    public Operand ToMyDate(String errorMessage, Object[] args) {
        return ToMyDateInternal(String.format(errorMessage, args));
    }

    private Operand ToMyDateInternal(String errorMessage) {
        MyDate date = MyDate.Parse(TextValue());
        if (date != null) {
            return Create(date);
        }
        return Error(errorMessage != null ? errorMessage : "Convert to date error!");
    }

    @Override
    public Operand ToArray(String errorMessage) {
        return Error(errorMessage != null ? errorMessage : "Convert to array error!");
    }

    @Override
    public Operand ToJson(String errorMessage) {
        String span = TextValue().trim();
        if ((span.length() > 0 && span.charAt(0) == '{' && span.charAt(span.length() - 1) == '}') ||
            (span.length() > 0 && span.charAt(0) == '[' && span.charAt(span.length() - 1) == ']')) {
            try {
                JsonData json = JsonData.parse(span);
                return Operand.Create(json);
            } catch (Exception e) { }
        }
        return Error(errorMessage != null ? errorMessage : "Convert to json error!");
    }

    @Override
    public String toString() {
        int len = _value.length();
        for (int i = 0; i < len; i++) {
            char c = _value.charAt(i);
            if (c == '"' || c == '\\' || c == '\n' || c == '\r' || c == '\t'
                || c == '\0' || c == '\u000b' || c == '\u0007' || c == '\b' || c == '\f') {
                return ToStringInternal();
            }
        }
        return "\"" + _value + "\"";
    }

    private String ToStringInternal() {
        StringBuilder sb = new StringBuilder(_value.length() + 16);
        sb.append('"');
        for (int i = 0; i < _value.length(); i++) {
            char c = _value.charAt(i);
            switch (c) {
                case '"': sb.append("\\\""); break;
                case '\\': sb.append("\\\\"); break;
                case '\n': sb.append("\\n"); break;
                case '\r': sb.append("\\r"); break;
                case '\t': sb.append("\\t"); break;
                case '\0': sb.append("\\0"); break;
                case '\u000b': sb.append("\\v"); break;
                case '\u0007': sb.append("\\a"); break;
                case '\b': sb.append("\\b"); break;
                case '\f': sb.append("\\f"); break;
                default: sb.append(c); break;
            }
        }
        sb.append('"');
        return sb.toString();
    }
}
