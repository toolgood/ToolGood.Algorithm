package toolgood.algorithm;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.regex.Pattern;

import org.joda.time.DateTime;

import toolgood.algorithm.litJson.JsonMapper;
import toolgood.algorithm.litJson.JsonData;

public abstract class Operand {
    public final static Operand True = new OperandBoolean(true);
    public final static Operand False = new OperandBoolean(false);
    public final static Operand One = Operand.Create(1);
    public final static Operand Zero = Operand.Create(0);

    public boolean IsNull() {
        return false;
    }

    public boolean IsError() {
        return false;
    }

    public String ErrorMsg() {
        return null;
    }

    public OperandType Type() {
        return OperandType.ERROR;
    }

    public double NumberValue() {
        return 0.0;
    }

    public int IntValue() {
        return 0;
    }

    public Object Value() {
        return null;
    }

    public String TextValue() {
        return null;
    }

    public boolean BooleanValue() {
        return false;
    }

    public List<Operand> ArrayValue() {
        return null;
    }

    public JsonData JsonValue() {
        return null;
    }

    public MyDate DateValue() {
        return null;
    }

    public static Operand Create(final boolean obj) {
        return obj ? True : False;
    }

    public static Operand Create(final short obj) {
        return new OperandLongNumber((long) obj);
    }

    public static Operand Create(final int obj) {
        return new OperandLongNumber((long) obj);
    }

    public static Operand Create(final long obj) {
        return new OperandLongNumber(obj);
    }

    public static Operand Create(final float obj) {
        return new OperandDoubleNumber((double) obj);
    }

    public static Operand Create(final double obj) {
        return new OperandDoubleNumber(obj);
    }

    public static Operand Create(final BigDecimal obj) {
        return new OperandDoubleNumber(obj.doubleValue());
    }

    public static Operand Create(final String obj) {
        if (obj.equals(null)) {
            return CreateNull();
        }
        return new OperandString(obj);
    }

    public static Operand CreateJson(final String txt) {
        if ((txt.startsWith("{") && txt.endsWith("}")) || (txt.startsWith("[") && txt.endsWith("]"))) {
            try {
                JsonData json = (JsonData) JsonMapper.ToObject(txt);
                return Create(json);
            } catch (final Exception e) {
            }
        }
        return Error("string to json is error!");
    }

    public static Operand Create(final MyDate obj) {
        return new OperandDate(obj);
    }

    public static Operand Create(final DateTime obj) {
        return new OperandDate(new MyDate(obj));
    }

    public static Operand Create(final Date obj) {
        return new OperandDate(new MyDate(obj));
    }

    public static Operand Create(final JsonData obj) {
        return new OperandJson(obj);
    }

    public static Operand Create(List<Operand> obj) {
        return new OperandArray(obj);
    }

    public static Operand Error(final String msg) {
        return new OperandError(msg);
    }

    public static Operand CreateNull() {
        return new OperandNull();
    }

    public Operand ToNumber(final String errorMessage) {
        if (Type().isNumber()) {
            return this;
        }
        if (IsError()) {
            return this;
        }
        if (Type() == OperandType.BOOLEAN) {
            return BooleanValue() ? One : Zero;
        }
        if (Type() == OperandType.DATE) {
            return Create((double) DateValue().ToNumber());
        }
        if (Type() == OperandType.TEXT) {
            try {
                Double d = Double.parseDouble(TextValue());
                return Create(d);
            } catch (Exception e) {
            }
        }
        return Error(errorMessage);
    }

    public Operand ToBoolean(final String errorMessage) {
        if (Type() == OperandType.BOOLEAN) {
            return this;
        }
        if (IsError()) {
            return this;
        }
        if (Type().isNumber()) {
            return (NumberValue() != 0) ? True : False;
        }
        if (Type() == OperandType.DATE) {
            return (((double) DateValue().ToNumber()) != 0) ? True : False;
        }
        if (Type() == OperandType.TEXT) {
            if (TextValue().toLowerCase().equals("true")) {
                return True;
            }
            if (TextValue().toLowerCase().equals("false")) {
                return False;
            }
            if (TextValue().equals("1")) {
                return True;
            }
            if (TextValue().equals("0")) {
                return False;
            }
        }
        return Error(errorMessage);
    }

    public Operand ToText(final String errorMessage) {
        if (Type() == OperandType.TEXT) {
            return this;
        }
        if (IsError()) {
            return this;
        }
        if (Type().isNumber()) {
            String str = ((Double) NumberValue()).toString();
            if (str.contains(".")) {
                str = Pattern.compile("(\\.)?0+$").matcher(str).replaceAll("");
            }
            return Create(str);
        }
        if (Type() == OperandType.BOOLEAN) {
            return Create(BooleanValue() ? "TRUE" : "FALSE");
        }
        if (Type() == OperandType.DATE) {
            return Create(DateValue().toString());
        }

        return Error(errorMessage);
    }

    public Operand ToDate(final String errorMessage) {
        if (Type() == OperandType.DATE) {
            return this;
        }
        if (IsError()) {
            return this;
        }
        if (Type().isNumber()) {
            return Create(new MyDate(NumberValue()));
        }
        if (Type() == OperandType.TEXT) {
            MyDate date = MyDate.parse(TextValue());
            if (date != null) {
                return Create(date);
            }
            // if (TimeSpan.TryParse(TextValue, cultureInfo, out TimeSpan t)) { return
            // Create(new Date(t)); }
            // if (DateTime.TryParse(TextValue, cultureInfo, DateTimeStyles.None, out
            // DateTime d)) { return Create(new Date(d)); }
        }
        return Error(errorMessage);
    }

    public Operand ToJson(final String errorMessage) {
        if (Type() == OperandType.JSON) {
            return this;
        }
        if (IsError()) {
            return this;
        }
        if (Type() == OperandType.TEXT) {
            final String txt = TextValue();
            if ((txt.startsWith("{") && txt.endsWith("}")) || (txt.startsWith("[") && txt.endsWith("]"))) {
                try {
                    final JsonData json = JsonMapper.ToObject(txt);
                    return Operand.Create(json);
                } catch (final Exception e) {
                }
            }
        }
        return Error(errorMessage);
    }

    public Operand ToArray(final String errorMessage) {
        if (Type() == OperandType.ARRARY) {
            return this;
        }
        if (IsError()) {
            return this;
        }
        if (Type() == OperandType.JSON) {
            if (JsonValue().IsArray()) {
                final List<Operand> list = new ArrayList<Operand>();
                for (JsonData v : JsonValue().inst_array) {
                    if (v.IsString())
                        list.add(Create(v.StringValue()));
                    else if (v.IsBoolean())
                        list.add(Create(v.BooleanValue()));
                    else if (v.IsDouble())
                        list.add(Create(v.NumberValue()));
                    else if (v.IsNull())
                        list.add(CreateNull());
                    else
                        list.add(Create(v));
                }
                return Create(list);
            }
        }
        return Error(errorMessage);
    }

    static abstract class OperandT<T> extends Operand {
        public T Value;

        public OperandT(final T obj) {
            Value = obj;
        }
    }

    static class OperandArray extends OperandT<java.util.List<Operand>> {
        public OperandArray(final List<Operand> obj) {
            super(obj);
        }

        @Override
        public OperandType Type() {
            return OperandType.ARRARY;
        }

        @Override
        public List<Operand> ArrayValue() {
            return Value;
        }
    }

    static class OperandBoolean extends OperandT<Boolean> {
        public OperandBoolean(final Boolean obj) {
            super(obj);
        }

        @Override
        public OperandType Type() {
            return OperandType.BOOLEAN;
        }

        @Override
        public boolean BooleanValue() {
            return Value;
        }
    }

    static class OperandDate extends OperandT<MyDate> {
        public OperandDate(final MyDate obj) {
            super(obj);
        }

        @Override
        public OperandType Type() {
            return OperandType.DATE;
        }

        @Override
        public MyDate DateValue() {
            return Value;
        }
    }

    static class OperandError extends Operand {
        private final String _errorMsg;

        public OperandError(final String msg) {
            _errorMsg = msg;
        }

        @Override
        public OperandType Type() {
            return OperandType.ERROR;
        }

        @Override
        public boolean IsError() {
            return true;
        }

        public String ErrorMsg() {
            return _errorMsg;
        }
    }

    static class OperandJson extends OperandT<JsonData> {
        public OperandJson(final JsonData obj) {
            super(obj);
        }

        @Override
        public OperandType Type() {
            return OperandType.JSON;
        }

        @Override
        public JsonData JsonValue() {
            return Value;
        }
    }

    static class OperandNull extends Operand {
        // public override OperandType Type => OperandType.NULL;
        // public override bool IsNull => true;

        @Override
        public OperandType Type() {
            return OperandType.NULL;
        }

        @Override
        public boolean IsNull() {
            return true;
        }

    }

    static class OperandDoubleNumber extends OperandT<Double> {

        public OperandDoubleNumber(Double obj) {
            super(obj);
        }

        @Override
        public OperandType Type() {
            return OperandType.DOUBLE_NUMBER;
        }

        @Override
        public double NumberValue() {
            return Value;
        }

        @Override
        public int IntValue() {
            return Value.intValue();
        }
    }

    static class OperandLongNumber extends OperandT<Long> {

        public OperandLongNumber(Long obj) {
            super(obj);
        }

        @Override
        public OperandType Type() {
            return OperandType.LONG_NUMBER;
        }

        @Override
        public Object Value() {
            return Value.longValue();
        }

        @Override
        public double NumberValue() {
            return Value;
        }

        @Override
        public int IntValue() {
            return Value.intValue();
        }
    }

    static class OperandString extends OperandT<String> {

        public OperandString(String obj) {
            super(obj);
        }

        @Override
        public OperandType Type() {
            return OperandType.TEXT;
        }

        @Override
        public String TextValue() {
            return Value;
        }

    }

}
