package toolgood.algorithm;

import org.joda.time.DateTime;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.litJson.JsonData;
import toolgood.algorithm.litJson.JsonMapper;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.regex.Pattern;

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

    public BigDecimal NumberValue() {
        return null;
    }

    public double DoubleValue() {
        return 0;
    }

    public int IntValue() {
        return 0;
    }

    public long LongValue() {
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
        return new OperandNumber(new BigDecimal(obj));
    }

    public static Operand Create(final int obj) {
        return new OperandNumber(new BigDecimal(obj));
    }

    public static Operand Create(final long obj) {
        return new OperandNumber(new BigDecimal(obj));
    }

    public static Operand Create(final float obj) {
        return new OperandNumber(new BigDecimal(obj));
    }

    public static Operand Create(final double obj) {
        return new OperandNumber(new BigDecimal(obj));
    }

    public static Operand Create(final BigDecimal obj) {
        return new OperandNumber(obj);
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
        return Error(errorMessage);
    }

    public Operand ToBoolean(final String errorMessage) {
        return Error(errorMessage);
    }

    public Operand ToText(final String errorMessage) {
        return Error(errorMessage);
    }

    public Operand ToDate(final String errorMessage) {
        return Error(errorMessage);
    }

    public Operand ToJson(final String errorMessage) {
        return Error(errorMessage);
    }

    public Operand ToArray(final String errorMessage) {
        return Error(errorMessage);
    }

    static abstract class OperandT<T> extends Operand {
        protected T _value;

        @Override
        public Object Value() {
            return _value;
        }

        public OperandT(final T obj) {
            _value = obj;
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
            return _value;
        }

        @Override
        public Operand ToArray(String errorMessage) {
            return this;
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
            return _value;
        }

        @Override
        public Operand ToNumber(String errorMessage) {
            return BooleanValue() ? One : Zero;
        }

        @Override
        public Operand ToBoolean(String errorMessage) {
            return this;
        }

        @Override
        public Operand ToText(String errorMessage) {
            return Create(BooleanValue() ? "TRUE" : "FALSE");
        }

        @Override
        public Operand ToArray(String errorMessage) {
            if (errorMessage == null) {
                errorMessage = "Convert bool to array error!";
            }
            return Error(errorMessage);
        }

        @Override
        public Operand ToJson(String errorMessage) {
            if (errorMessage == null) {
                errorMessage = "Convert bool to json error!";
            }
            return Error(errorMessage);
        }

        @Override
        public Operand ToDate(String errorMessage) {
            if (errorMessage == null) {
                errorMessage = "Convert bool to date error!";
            }
            return Error(errorMessage);
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
            return _value;
        }

        @Override
        public Operand ToNumber(String errorMessage) {
            return Create(DateValue().ToNumber());
        }

        @Override
        public Operand ToBoolean(String errorMessage) {
            return ((DateValue().ToNumber().compareTo(new BigDecimal(0))) != 0) ? True : False;
        }

        @Override
        public Operand ToText(String errorMessage) {
            return Create(DateValue().toString());
        }

        @Override
        public Operand ToDate(String errorMessage) {
            return this;
        }

        @Override
        public Operand ToArray(String errorMessage) {
            if (errorMessage == null) {
                errorMessage = "Convert date to array error!";
            }
            return Error(errorMessage);
        }

        @Override
        public Operand ToJson(String errorMessage) {
            if (errorMessage == null) {
                errorMessage = "Convert date to json error!";
            }
            return Error(errorMessage);
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

        @Override
        public Operand ToNumber(String errorMessage) {
            return this;
        }

        @Override
        public Operand ToBoolean(String errorMessage) {
            return this;
        }

        @Override
        public Operand ToText(String errorMessage) {
            return this;
        }

        @Override
        public Operand ToArray(String errorMessage) {
            return this;
        }

        @Override
        public Operand ToJson(String errorMessage) {
            return this;
        }

        @Override
        public Operand ToDate(String errorMessage) {
            return this;
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
            return _value;
        }

        @Override
        public Operand ToJson(String errorMessage) {
            return this;
        }

        @Override
        public Operand ToArray(String errorMessage) {
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
            if (errorMessage == null) {
                errorMessage = "Convert json to array error!";
            }
            return Error(errorMessage);
        }

        @Override
        public Operand ToBoolean(String errorMessage) {
            if (errorMessage == null) {
                errorMessage = "Convert json to bool error!";
            }
            return Error(errorMessage);
        }

        @Override
        public Operand ToDate(String errorMessage) {
            if (errorMessage == null) {
                errorMessage = "Convert json to date error!";
            }
            return Error(errorMessage);
        }

        @Override
        public Operand ToNumber(String errorMessage) {
            if (errorMessage == null) {
                errorMessage = "Convert json to number error!";
            }
            return Error(errorMessage);
        }

        @Override
        public Operand ToText(String errorMessage) {
            if (errorMessage == null) {
                errorMessage = "Convert number to string error!";
            }
            return Error(errorMessage);
        }
    }

    static class OperandNull extends Operand {

        @Override
        public OperandType Type() {
            return OperandType.NULL;
        }

        @Override
        public boolean IsNull() {
            return true;
        }

    }

    static class OperandNumber extends OperandT<BigDecimal> {

        public OperandNumber(BigDecimal obj) {
            super(obj);
        }

        @Override
        public OperandType Type() {
            return OperandType.NUMBER;
        }

        @Override
        public int IntValue() {
            return _value.intValue();
        }

        @Override
        public BigDecimal NumberValue() {
            return _value;
        }

        @Override
        public double DoubleValue() {
            return _value.doubleValue();
        }

        @Override
        public long LongValue() {
            return _value.longValue();
        }

        @Override
        public Operand ToNumber(String errorMessage) {
            return this;
        }

        @Override
        public Operand ToBoolean(String errorMessage) {
            return (NumberValue().compareTo(new BigDecimal(0)) != 0) ? True : False;
        }

        @Override
        public Operand ToText(String errorMessage) {
            String str = ((Double) NumberValue().doubleValue()).toString();
            if (str.contains(".")) {
                str = Pattern.compile("(\\.)?0+$").matcher(str).replaceAll("");
            }
            return Create(str);
        }

        @Override
        public Operand ToDate(String errorMessage) {
            return Create(new MyDate(NumberValue()));
        }

        @Override
        public Operand ToArray(String errorMessage) {
            if (errorMessage == null) {
                errorMessage = "Convert number to array error!";
            }
            return Error(errorMessage);
        }

        @Override
        public Operand ToJson(String errorMessage) {
            if (errorMessage == null) {
                errorMessage = "Convert number to json error!";
            }
            return Error(errorMessage);
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
            return _value;
        }

        @Override
        public Operand ToNumber(String errorMessage) {
            try {
                BigDecimal d = new BigDecimal(TextValue());
                return Create(d);
            } catch (Exception e) {
            }
            if (errorMessage == null) {
                return Error("Convert string to number error!");
            }
            return Error(errorMessage);
        }

        @Override
        public Operand ToText(String errorMessage) {
            return this;
        }

        @Override
        public Operand ToBoolean(String errorMessage) {
            if (TextValue().toLowerCase().equals("true") || TextValue().toLowerCase().equals("yes")) {
                return True;
            }
            if (TextValue().toLowerCase().equals("false") || TextValue().toLowerCase().equals("no")) {
                return False;
            }
            if (TextValue().equals("1") || TextValue().equals("是") || TextValue().equals("有")) {
                return True;
            }
            if (TextValue().equals("0") || TextValue().equals("否") || TextValue().equals("不是") || TextValue().equals("无") || TextValue().equals("没有")) {
                return False;
            }
            if (errorMessage == null) {
                return Error("Convert string to bool error!");
            }
            return Error(errorMessage);
        }

        @Override
        public Operand ToDate(String errorMessage) {
            MyDate date = MyDate.parse(TextValue());
            if (date != null) {
                return Create(date);
            }
            if (errorMessage == null) {
                return Error("Convert string to date error!");
            }
            return Error(errorMessage);
        }

        @Override
        public Operand ToJson(String errorMessage) {
            final String txt = TextValue();
            if ((txt.startsWith("{") && txt.endsWith("}")) || (txt.startsWith("[") && txt.endsWith("]"))) {
                try {
                    final JsonData json = JsonMapper.ToObject(txt);
                    return Operand.Create(json);
                } catch (final Exception e) {
                }
            }
            if (errorMessage == null) {
                return Error("Convert string to json error!");
            }
            return Error(errorMessage);
        }

        @Override
        public Operand ToArray(String errorMessage) {
            if (errorMessage == null) {
                errorMessage = "Convert string to array error!";
            }
            return Error(errorMessage);
        }

    }

    public static class KeyValue {
        public String Key;
        public Operand Value;
    }

    public static class OperandKeyValue extends OperandT<KeyValue> {
        public OperandKeyValue(KeyValue obj) {
            super(obj);
        }

        public OperandType Type() {
            return OperandType.ARRARYJSON;
        }
    }

    public static class OperandKeyValueList extends OperandT<KeyValue> {
        private final List<KeyValue> TextList = new ArrayList<>();

        public OperandKeyValueList(KeyValue obj) {
            super(obj);
        }

        public OperandType Type() {
            return OperandType.ARRARYJSON;
        }

        public List<Operand> ArrayValue() {
            List<Operand> result = new ArrayList<>();
            for (KeyValue kv : TextList) {
                result.add(kv.Value);
            }
            return result;
        }

        public Operand ToArray(String errorMessage) {
            return Create(this.ArrayValue());
        }

        public void AddValue(KeyValue keyValue) {
            TextList.add(keyValue);
        }

        public boolean HasKey(String key) {
            for (KeyValue item : TextList) {
                if (item.Key.equals("" + key)) {
                    return true;
                }
            }
            return false;
        }

        public Operand GetValue(String key) {
            for (KeyValue item : TextList) {
                if (item.Key.equals(key)) {
                    return item.Value;
                }
            }
            return null;
        }
        public boolean ContainsKey(String value) {
            for (KeyValue item : TextList) {
                if (item.Key.equals(value)) {
                    return true;
                }
            }
            return false;
        }

        public boolean ContainsValue(String value) {
            for (KeyValue item : TextList) {
                Operand op = item.Value;
                if (op.TextValue().equals(value)) {
                    return true;
                }
            }
            return false;
        }

        public boolean ContainsValue(Operand value) {
            for (KeyValue item : TextList) {
                Operand op = item.Value;
                if (value.Type() != op.Type()) {
                    continue;
                }
                if (value.Type() == OperandType.TEXT) {
                    if (value.TextValue().equals(op.TextValue())) {
                        return true;
                    }
                }
                if (value.Type() == OperandType.NUMBER) {
                    if (value.TextValue().equals(op.TextValue())) {
                        return true;
                    }
                }
            }
            return false;
        }

        public Operand TryGetValueFloor(double key, boolean range_lookup) {
            Operand value = null;
            for (KeyValue item : TextList) {
                try {
                    double num = Double.parseDouble(item.Key);
                    Double t = Math.round(key - num * 1000000000d) / 1000000000d;
                    if (t == 0) {
                        return item.Value;
                    } else if (range_lookup) {
                        if (t > 0) {
                            value = item.Value;
                        } else if (value != null) {
                            return value;
                        }
                    }
                } catch (Exception ex) {
                }
            }
            return value;
        }
    }


}
