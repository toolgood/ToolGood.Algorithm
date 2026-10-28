package toolgood.algorithm;

import org.joda.time.DateTime;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.litJson.JsonData;
import toolgood.algorithm.litJson.JsonMapper;
import toolgood.algorithm.litJson.JsonType;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

public abstract class Operand {
    public static final Operand Version = new OperandString("ToolGood.Algorithm 6.3");
    public static final Operand True = new OperandBoolean(true);
    public static final Operand False = new OperandBoolean(false);
    public static final Operand One;
    public static final Operand Zero;
    public static final Operand Null = new OperandNull();
    public static final Operand None = new OperandNone();

    // 整数缓存范围: -1000 ~ 1000
    private static final int IntCacheOffset = 1000;
    private static final int IntCacheSize = 2001;
    private static final Operand[] IntCache = new Operand[IntCacheSize];
    private static final BigDecimal IntCacheMin = BigDecimal.valueOf(-IntCacheOffset);
    private static final BigDecimal IntCacheMax = BigDecimal.valueOf(IntCacheOffset);

    static {
        for (int i = 0; i < IntCacheSize; i++) {
            IntCache[i] = new OperandInt(i - IntCacheOffset);
        }
        One = Operand.Create(1);
        Zero = Operand.Create(0);
    }

    // region 类型判断
    public boolean IsErrorOrNone() { return false; }
    public boolean IsNone() { return false; }
    public boolean IsNull() { return false; }
    public boolean IsNumber() { return false; }
    public boolean IsText() { return false; }
    public boolean IsBoolean() { return false; }
    public boolean IsArray() { return false; }
    public boolean IsDate() { return false; }
    public boolean IsJson() { return false; }
    public boolean IsArrayJson() { return false; }
    public boolean IsError() { return false; }
    public String ErrorMsg() { return null; }
    // endregion

    public abstract OperandType Type();

    // region Value
    public BigDecimal NumberValue() { throw new UnsupportedOperationException(); }
    public double DoubleValue() { throw new UnsupportedOperationException(); }
    public int IntValue() { throw new UnsupportedOperationException(); }
    public long LongValue() { throw new UnsupportedOperationException(); }
    public String TextValue() { throw new UnsupportedOperationException(); }
    public boolean BooleanValue() { throw new UnsupportedOperationException(); }
    public List<Operand> ArrayValue() { throw new UnsupportedOperationException(); }
    public JsonData JsonValue() { throw new UnsupportedOperationException(); }
    public MyDate DateValue() { throw new UnsupportedOperationException(); }
    // endregion

    // region Create
    public static Operand Create(boolean obj) {
        return obj ? True : False;
    }

    public static Operand Create(short obj) {
        if (obj >= -IntCacheOffset && obj <= IntCacheOffset) {
            return IntCache[obj + IntCacheOffset];
        }
        return new OperandInt(obj);
    }

    public static Operand Create(int obj) {
        if (obj >= -IntCacheOffset && obj <= IntCacheOffset) {
            return IntCache[obj + IntCacheOffset];
        }
        return new OperandInt(obj);
    }

    public static Operand Create(long obj) {
        if (obj >= -IntCacheOffset && obj <= IntCacheOffset) {
            return IntCache[(int) obj + IntCacheOffset];
        }
        return new OperandDecimal(new BigDecimal(obj));
    }

    public static Operand Create(float obj) {
        if (Float.isNaN(obj) || Float.isInfinite(obj)) {
            return Error("Number is NaN or Infinity!");
        }
        if (obj == Math.floor(obj) && obj >= -IntCacheOffset && obj <= IntCacheOffset) {
            return IntCache[(int) obj + IntCacheOffset];
        }
        return new OperandDecimal(new BigDecimal(String.valueOf(obj)));
    }

    public static Operand Create(double obj) {
        if (Double.isNaN(obj) || Double.isInfinite(obj)) {
            return Error("Number is NaN or Infinity!");
        }
        if (obj == Math.floor(obj) && obj >= -IntCacheOffset && obj <= IntCacheOffset) {
            return IntCache[(int) obj + IntCacheOffset];
        }
        return new OperandDecimal(new BigDecimal(String.valueOf(obj)));
    }

    public static Operand Create(BigDecimal obj) {
        // 用 scale + 范围判断替代 intValueExact 抛异常做控制流：
        // intValueExact 对带小数或超出 int 范围的值抛 ArithmeticException，
        // 而带小数的数值在公式计算里是常态，异常构造+栈填充远贵于两次 scale 判断。
        // 语义与原先完全一致：只有「整数值且落在缓存区间内」才复用缓存，其余走 OperandDecimal。
        BigDecimal s = obj.scale() <= 0 ? obj : obj.stripTrailingZeros();
        if (s.scale() <= 0 && s.compareTo(IntCacheMin) >= 0 && s.compareTo(IntCacheMax) <= 0) {
            return IntCache[s.intValue() + IntCacheOffset];
        }
        return new OperandDecimal(obj);
    }

    public static Operand Create(String obj) {
        if (obj == null) {
            return Null;
        }
        return new OperandString(obj);
    }

    public static Operand CreateJson(String txt) {
        if ((txt.startsWith("{") && txt.endsWith("}")) || (txt.startsWith("[") && txt.endsWith("]"))) {
            try {
                JsonData json = JsonMapper.ToObject(txt);
                return Operand.Create(json);
            } catch (Exception e) {
                return Operand.Error("Convert to json error!");
            }
        }
        return Operand.Error("Convert to json error!");
    }

    public static Operand Create(MyDate obj) {
        return new OperandMyDate(obj);
    }

    public static Operand Create(DateTime obj) {
        return new OperandMyDate(new MyDate(obj));
    }

    public static Operand Create(Date obj) {
        return new OperandMyDate(new MyDate(obj));
    }

    public static Operand Create(JsonData obj) {
        return new OperandJson(obj);
    }

    public static Operand Create(List<Operand> obj) {
        return new OperandArray(obj);
    }

    public static Operand Error(String msg) {
        return new OperandError(msg, null);
    }

    public static Operand Error(String msg, Object... args) {
        return new OperandError(msg, args);
    }
    // endregion Create

    // region 类型转换
    /**
     * 尝试直接取文本值（不构造错误消息）；成功时返回与 ToText 一致的操作数，不可转换返回 null。
     * 供 FunctionBase.ConvertToText 走快路径，避免每次成功转换都白付错误消息格式化。
     */
    public Operand TryGetText() { return null; }

    /**
     * 尝试直接取数组值（不构造错误消息）；成功时返回与 ToArray 一致的操作数，不可转换返回 null。
     */
    public Operand TryGetArray() { return null; }

    public Operand ToNumber(String errorMessage) {
        return Error(errorMessage != null ? errorMessage : "Convert to number error!");
    }

    public Operand ToNumber(String errorMessage, Object... args) {
        return Error(errorMessage, args);
    }

    public Operand ToBoolean(String errorMessage) {
        return Error(errorMessage != null ? errorMessage : "Convert to bool error!");
    }

    public Operand ToText(String errorMessage) {
        return Error(errorMessage != null ? errorMessage : "Convert to string error!");
    }

    public Operand ToMyDate(String errorMessage) {
        return Error(errorMessage != null ? errorMessage : "Convert to date error!");
    }

    public Operand ToArray(String errorMessage) {
        return Error(errorMessage != null ? errorMessage : "Convert to array error!");
    }

    public Operand ToJson(String errorMessage) {
        return Error(errorMessage != null ? errorMessage : "Convert to json error!");
    }
    // endregion

    // region ToJsonData
    /**
     * 按元素原 toString() → JsonMapper 往返的等价结果直接构造 JsonData，免去序列化再解析；
     * 无法等价转换的类型（OperandError/OperandNone，其 toString 非法 JSON，原往返必然失败）返回 null
     */
    static JsonData OperandToJsonData(Operand o) {
        JsonData d;
        if (o instanceof OperandString) {
            d = new JsonData();
            d.SetString(o.TextValue());
            return d;
        }
        if (o instanceof OperandInt || o instanceof OperandDecimal) {
            d = new JsonData();
            d.SetDouble(o.NumberValue());
            return d;
        }
        if (o instanceof OperandBoolean) {
            d = new JsonData();
            d.SetBoolean(o.BooleanValue());
            return d;
        }
        if (o instanceof OperandMyDate) {
            d = new JsonData();
            d.SetString(((OperandMyDate) o).DateValue().toString());
            return d;
        }
        if (o instanceof OperandNull) {
            d = new JsonData();
            d.SetNull();
            return d;
        }
        if (o instanceof OperandJson) {
            return o.JsonValue();
        }
        if (o instanceof OperandArray) {
            return ((OperandArray) o).ToJsonData();
        }
        if (o instanceof OperandKeyValueList) {
            return ((OperandKeyValueList) o).ToJsonData();
        }
        return null;
    }
    // endregion

    // ============== Inner classes ==============

    // region OperandInt (缓存的整数)
    static final class OperandInt extends Operand {
        private final int _value;

        OperandInt(int obj) { _value = obj; }

        @Override
        public boolean IsNumber() { return true; }
        @Override
        public OperandType Type() { return OperandType.NUMBER; }
        @Override
        public int IntValue() { return _value; }
        @Override
        public BigDecimal NumberValue() { return new BigDecimal(_value); }
        @Override
        public long LongValue() { return _value; }
        @Override
        public double DoubleValue() { return _value; }

        @Override
        public Operand ToNumber(String errorMessage) { return this; }
        @Override
        public Operand ToNumber(String errorMessage, Object... args) { return this; }

        @Override
        public Operand ToBoolean(String errorMessage) {
            if (_value == 0) return False;
            if (_value == 1) return True;
            if (errorMessage == null) errorMessage = "Convert number to bool error!";
            return Error(errorMessage);
        }
        public Operand ToBoolean(String errorMessage, Object... args) {
            if (_value == 0) return False;
            if (_value == 1) return True;
            return Error(errorMessage, args);
        }

        @Override
        public Operand ToText(String errorMessage) {
            return Create(String.valueOf(_value));
        }
        public Operand ToText(String errorMessage, Object... args) {
            return Create(String.valueOf(_value));
        }
        @Override
        public Operand TryGetText() { return Create(String.valueOf(_value)); }

        @Override
        public Operand ToMyDate(String errorMessage) {
            return Create(new MyDate(_value));
        }

        @Override
        public String toString() { return String.valueOf(_value); }
    }
    // endregion

    // region OperandDecimal
    static final class OperandDecimal extends Operand {
        private final BigDecimal _value;

        OperandDecimal(BigDecimal obj) { _value = obj; }

        @Override
        public boolean IsNumber() { return true; }
        @Override
        public OperandType Type() { return OperandType.NUMBER; }
        @Override
        public int IntValue() { return _value.intValue(); }
        @Override
        public BigDecimal NumberValue() { return _value; }
        @Override
        public long LongValue() { return _value.longValue(); }
        @Override
        public double DoubleValue() { return _value.doubleValue(); }

        @Override
        public Operand ToNumber(String errorMessage) { return this; }
        @Override
        public Operand ToNumber(String errorMessage, Object... args) { return this; }

        @Override
        public Operand ToBoolean(String errorMessage) {
            if (_value.compareTo(BigDecimal.ZERO) == 0) return False;
            if (_value.compareTo(BigDecimal.ONE) == 0) return True;
            if (errorMessage == null) errorMessage = "Convert number to bool error!";
            return Error(errorMessage);
        }
        public Operand ToBoolean(String errorMessage, Object... args) {
            if (_value.compareTo(BigDecimal.ZERO) == 0) return False;
            if (_value.compareTo(BigDecimal.ONE) == 0) return True;
            return Error(errorMessage, args);
        }

        @Override
        public Operand ToText(String errorMessage) {
            String str = _value.toPlainString();
            if (str.contains(".")) {
                str = str.replaceAll("(\\.)?0+$", "");
            }
            return Create(str);
        }
        public Operand ToText(String errorMessage, Object... args) {
            String str = _value.toPlainString();
            if (str.contains(".")) {
                str = str.replaceAll("(\\.)?0+$", "");
            }
            return Create(str);
        }
        @Override
        public Operand TryGetText() { return Create(TrimTrailingZeros(_value)); }

        /** 等价于 toPlainString() 后按正则 (\\.)?0+$ 去掉尾部零与紧邻小数点，避免每次转换都走正则 */
        private static String TrimTrailingZeros(BigDecimal value) {
            String str = value.toPlainString();
            if (str.indexOf('.') < 0) return str;
            int end = str.length();
            while (end > 0 && str.charAt(end - 1) == '0') end--;
            if (end > 0 && str.charAt(end - 1) == '.') end--;
            return str.substring(0, end);
        }

        @Override
        public Operand ToMyDate(String errorMessage) {
            return Create(new MyDate(_value));
        }

        @Override
        public String toString() { return _value.toPlainString(); }
    }
    // endregion

    // region OperandArray
    static final class OperandArray extends Operand {
        private final List<Operand> _value;

        OperandArray(List<Operand> obj) { _value = obj; }

        @Override
        public boolean IsArray() { return true; }
        @Override
        public OperandType Type() { return OperandType.ARRAY; }
        @Override
        public List<Operand> ArrayValue() { return _value; }

        @Override
        public Operand ToNumber(String errorMessage) {
            if (errorMessage == null) errorMessage = "Convert array to number error!";
            return Error(errorMessage);
        }

        @Override
        public Operand ToBoolean(String errorMessage) {
            if (errorMessage == null) errorMessage = "Convert array to bool error!";
            return Error(errorMessage);
        }

        @Override
        public Operand ToText(String errorMessage) {
            return Create(toString());
        }
        public Operand ToText(String errorMessage, Object... args) {
            return Create(toString());
        }

        @Override
        public Operand ToArray(String errorMessage) { return this; }
        public Operand ToArray(String errorMessage, Object... args) { return this; }
        @Override
        public Operand TryGetText() { return Create(toString()); }
        @Override
        public Operand TryGetArray() { return this; }

        @Override
        public Operand ToJson(String errorMessage) {
            // 直接构造成 JsonData，免去 toString 序列化再 JsonMapper 解析的完整往返
            JsonData json = ToJsonData();
            if (json == null) return Error(errorMessage != null ? errorMessage : "Convert to json error!");
            return Operand.Create(json);
        }

        JsonData ToJsonData() {
            JsonData json = new JsonData();
            json.SetJsonType(JsonType.Array);
            for (int i = 0; i < _value.size(); i++) {
                JsonData data = OperandToJsonData(_value.get(i));
                if (data == null) return null;
                json.Add(data);
            }
            return json;
        }

        @Override
        public String toString() {
            StringBuilder sb = new StringBuilder("[");
            for (int i = 0; i < _value.size(); i++) {
                if (i > 0) sb.append(",");
                sb.append(_value.get(i).toString());
            }
            sb.append("]");
            return sb.toString();
        }
    }
    // endregion

    // region OperandBoolean
    static final class OperandBoolean extends Operand {
        private final boolean _value;

        OperandBoolean(boolean obj) { _value = obj; }

        @Override
        public boolean IsBoolean() { return true; }
        @Override
        public OperandType Type() { return OperandType.BOOLEAN; }
        @Override
        public boolean BooleanValue() { return _value; }

        @Override
        public Operand ToNumber(String errorMessage) {
            return _value ? One : Zero;
        }
        @Override
        public Operand ToNumber(String errorMessage, Object... args) {
            return _value ? One : Zero;
        }

        @Override
        public Operand ToBoolean(String errorMessage) { return this; }
        public Operand ToBoolean(String errorMessage, Object... args) { return this; }

        @Override
        public Operand ToText(String errorMessage) {
            return Create(_value ? "TRUE" : "FALSE");
        }
        public Operand ToText(String errorMessage, Object... args) {
            return Create(_value ? "TRUE" : "FALSE");
        }
        @Override
        public Operand TryGetText() { return Create(_value ? "TRUE" : "FALSE"); }

        @Override
        public Operand ToMyDate(String errorMessage) {
            if (errorMessage == null) errorMessage = "Convert bool to date error!";
            return Error(errorMessage);
        }

        @Override
        public Operand ToArray(String errorMessage) {
            if (errorMessage == null) errorMessage = "Convert bool to array error!";
            return Error(errorMessage);
        }

        @Override
        public Operand ToJson(String errorMessage) {
            if (errorMessage == null) errorMessage = "Convert bool to json error!";
            return Error(errorMessage);
        }

        @Override
        public String toString() { return _value ? "true" : "false"; }
    }
    // endregion

    // region OperandMyDate
    static final class OperandMyDate extends Operand {
        private final MyDate _value;

        OperandMyDate(MyDate obj) { _value = obj; }

        @Override
        public boolean IsDate() { return true; }
        @Override
        public OperandType Type() { return OperandType.DATE; }
        @Override
        public MyDate DateValue() { return _value; }

        @Override
        public Operand ToNumber(String errorMessage) {
            return Create(DateValue().ToNumber());
        }

        @Override
        public Operand ToBoolean(String errorMessage) {
            return DateValue().ToNumber().compareTo(BigDecimal.ZERO) != 0 ? True : False;
        }

        @Override
        public Operand ToText(String errorMessage) {
            return Create(DateValue().toString());
        }
        public Operand ToText(String errorMessage, Object... args) {
            return Create(DateValue().toString());
        }
        @Override
        public Operand TryGetText() { return Create(DateValue().toString()); }

        @Override
        public Operand ToMyDate(String errorMessage) { return this; }
        public Operand ToMyDate(String errorMessage, Object... args) { return this; }

        @Override
        public Operand ToArray(String errorMessage) {
            if (errorMessage == null) errorMessage = "Convert date to array error!";
            return Error(errorMessage);
        }

        @Override
        public Operand ToJson(String errorMessage) {
            if (errorMessage == null) errorMessage = "Convert date to json error!";
            return Error(errorMessage);
        }

        @Override
        public String toString() { return "\"" + _value.toString() + "\""; }
    }
    // endregion

    // region OperandError
    static final class OperandError extends Operand {
        private final String _errorMsg;
        private final Object[] _args;
        private String _formattedMsg;

        OperandError(String msg, Object[] args) {
            _errorMsg = msg;
            _args = (args == null || args.length == 0) ? null : args;
        }

        @Override
        public OperandType Type() { return OperandType.ERROR; }
        @Override
        public boolean IsError() { return true; }
        @Override
        public boolean IsErrorOrNone() { return true; }

        @Override
        public String ErrorMsg() {
            if (_formattedMsg != null) return _formattedMsg;
            if (_args == null || _args.length == 0) {
                _formattedMsg = _errorMsg;
            } else {
                _formattedMsg = java.text.MessageFormat.format(_errorMsg, _args);
            }
            return _formattedMsg;
        }

        @Override
        public Operand ToNumber(String errorMessage) { return this; }
        @Override
        public Operand ToNumber(String errorMessage, Object... args) { return this; }
        @Override
        public Operand ToBoolean(String errorMessage) { return this; }
        public Operand ToBoolean(String errorMessage, Object... args) { return this; }
        @Override
        public Operand ToText(String errorMessage) { return this; }
        public Operand ToText(String errorMessage, Object... args) { return this; }
        @Override
        public Operand TryGetText() { return this; }
        @Override
        public Operand ToArray(String errorMessage) { return this; }
        public Operand ToArray(String errorMessage, Object... args) { return this; }
        @Override
        public Operand ToJson(String errorMessage) { return this; }
        @Override
        public Operand ToMyDate(String errorMessage) { return this; }
        public Operand ToMyDate(String errorMessage, Object... args) { return this; }
    }
    // endregion

    // region OperandJson
    static final class OperandJson extends Operand {
        private final JsonData _value;

        OperandJson(JsonData obj) { _value = obj; }

        @Override
        public boolean IsJson() { return true; }
        @Override
        public OperandType Type() { return OperandType.JSON; }
        @Override
        public JsonData JsonValue() { return _value; }

        @Override
        public Operand ToJson(String errorMessage) { return this; }

        @Override
        public Operand ToText(String errorMessage) {
            return Create(_value.toString());
        }
        public Operand ToText(String errorMessage, Object... args) {
            return Create(_value.toString());
        }
        @Override
        public Operand TryGetText() { return Create(_value.toString()); }
        @Override
        public Operand TryGetArray() {
            // IsArray 时不会触到错误分支，传 null 即可；非数组回落原路径构造错误消息
            if (JsonValue().IsArray()) return ToArray(null);
            return null;
        }

        @Override
        public Operand ToArray(String errorMessage) {
            if (JsonValue().IsArray()) {
                List<Operand> list = new ArrayList<>();
                for (JsonData v : JsonValue().inst_array) {
                    if (v.IsString())
                        list.add(Create(v.StringValue()));
                    else if (v.IsBoolean())
                        list.add(Create(v.BooleanValue()));
                    else if (v.IsDouble())
                        list.add(Create(v.NumberValue()));
                    else if (v.IsNull())
                        list.add(Null);
                    else
                        list.add(Create(v));
                }
                return Create(list);
            }
            if (errorMessage == null) errorMessage = "Convert json to array error!";
            return Error(errorMessage);
        }
        public Operand ToArray(String errorMessage, Object... args) {
            if (JsonValue().IsArray()) {
                List<Operand> list = new ArrayList<>();
                for (JsonData v : JsonValue().inst_array) {
                    if (v.IsString())
                        list.add(Create(v.StringValue()));
                    else if (v.IsBoolean())
                        list.add(Create(v.BooleanValue()));
                    else if (v.IsDouble())
                        list.add(Create(v.NumberValue()));
                    else if (v.IsNull())
                        list.add(Null);
                    else
                        list.add(Create(v));
                }
                return Create(list);
            }
            if (errorMessage == null) return Error("Convert json to array error!");
            return Error(errorMessage, args);
        }

        @Override
        public Operand ToBoolean(String errorMessage) {
            if (errorMessage == null) errorMessage = "Convert json to bool error!";
            return Error(errorMessage);
        }

        @Override
        public Operand ToNumber(String errorMessage) {
            if (errorMessage == null) errorMessage = "Convert json to number error!";
            return Error(errorMessage);
        }

        @Override
        public Operand ToMyDate(String errorMessage) {
            if (errorMessage == null) errorMessage = "Convert json to date error!";
            return Error(errorMessage);
        }

        @Override
        public String toString() { return _value.toString(); }
    }
    // endregion

    // region OperandNull
    static final class OperandNull extends Operand {
        @Override
        public OperandType Type() { return OperandType.NULL; }
        @Override
        public boolean IsNull() { return true; }
        @Override
        public String toString() { return "null"; }
    }
    // endregion

    // region OperandNone
    static class OperandNone extends Operand {
        @Override
        public OperandType Type() { return OperandType.NONE; }
        @Override
        public boolean IsNone() { return true; }
        @Override
        public boolean IsErrorOrNone() { return true; }
    }
    // endregion

    // region OperandString
    static final class OperandString extends Operand {
        private final String _value;

        OperandString(String obj) { _value = obj; }

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
        public Operand ToNumber(String errorMessage, Object... args) {
            return ToNumberInternal(String.format(errorMessage, args));
        }

        private Operand ToNumberInternal(String errorMessage) {
            try {
                return Create(Integer.parseInt(_value.trim()));
            } catch (NumberFormatException e) {
                // 继续尝试
            }
            try {
                return Create(new BigDecimal(_value.trim()));
            } catch (NumberFormatException e) {
                if (errorMessage == null) return Error("Convert string to number error!");
                return Error(errorMessage);
            }
        }

        @Override
        public Operand ToText(String errorMessage) { return this; }
        public Operand ToText(String errorMessage, Object... args) { return this; }
        @Override
        public Operand TryGetText() { return this; }

        @Override
        public Operand ToBoolean(String errorMessage) {
            return ToBooleanInternal(errorMessage);
        }
        public Operand ToBoolean(String errorMessage, Object... args) {
            return ToBooleanInternal(String.format(errorMessage, args));
        }

        private Operand ToBooleanInternal(String errorMessage) {
            String txt = _value.trim();
            if (txt.equals("true") || txt.equals("TRUE") || txt.equals("True")) return True;
            if (txt.equals("false") || txt.equals("FALSE") || txt.equals("False")) return False;
            if (txt.equals("1") || txt.equals("\u662f") || txt.equals("\u6709")) return True;
            if (txt.equals("0") || txt.equals("\u5426") || txt.equals("\u4e0d\u662f") || txt.equals("\u65e0") || txt.equals("\u6ca1\u6709")) return False;
            if (errorMessage == null) return Error("Convert string to bool error!");
            return Error(errorMessage);
        }

        @Override
        public Operand ToMyDate(String errorMessage) {
            return ToMyDateInternal(errorMessage);
        }
        public Operand ToMyDate(String errorMessage, Object... args) {
            return ToMyDateInternal(String.format(errorMessage, args));
        }

        private Operand ToMyDateInternal(String errorMessage) {
            MyDate date = MyDate.parse(_value);
            if (date != null) return Create(date);
            if (errorMessage == null) return Error("Convert string to date error!");
            return Error(errorMessage);
        }

        @Override
        public Operand ToArray(String errorMessage) {
            if (errorMessage == null) errorMessage = "Convert string to array error!";
            return Error(errorMessage);
        }

        @Override
        public Operand ToJson(String errorMessage) {
            String txt = _value.trim();
            if ((txt.startsWith("{") && txt.endsWith("}")) || (txt.startsWith("[") && txt.endsWith("]"))) {
                try {
                    JsonData json = JsonMapper.ToObject(txt);
                    return Operand.Create(json);
                } catch (Exception e) {
                }
            }
            if (errorMessage == null) return Error("Convert string to json error!");
            return Error(errorMessage);
        }

        @Override
        public String toString() {
            int len = _value.length();
            if (len == 0) return "\"\"";
            
            // Count characters that need escaping
            int escapeCount = 0;
            for (int i = 0; i < len; i++) {
                char c = _value.charAt(i);
                if (c == '"' || c == '\\' || c < ' ') {
                    escapeCount++;
                }
            }
            
            if (escapeCount == 0) {
                return "\"" + _value + "\"";
            }
            
            StringBuilder sb = new StringBuilder(len + escapeCount + 2);
            sb.append('"');
            for (int i = 0; i < len; i++) {
                char c = _value.charAt(i);
                switch (c) {
                    case '"': sb.append("\\\""); break;
                    case '\\': sb.append("\\\\"); break;
                    case '\n': sb.append("\\n"); break;
                    case '\r': sb.append("\\r"); break;
                    case '\t': sb.append("\\t"); break;
                    case '\b': sb.append("\\b"); break;
                    case '\f': sb.append("\\f"); break;
                    default:
                        if (c < ' ') {
                            sb.append("\\u");
                            sb.append(String.format("%04x", (int) c));
                        } else {
                            sb.append(c);
                        }
                        break;
                }
            }
            sb.append('"');
            return sb.toString();
        }
    }
    // endregion

    // region KeyValue
    public static class KeyValue {
        public String Key;
        public Operand Value;
    }
    // endregion

    // region OperandKeyValue
    public static class OperandKeyValue extends Operand {
        private final KeyValue _value;

        public OperandKeyValue(KeyValue obj) { _value = obj; }

        @Override
        public boolean IsArrayJson() { return true; }
        @Override
        public OperandType Type() { return OperandType.ARRAYJSON; }
        public KeyValue Value() { return _value; }
    }
    // endregion

    // region OperandKeyValueList
    public static class OperandKeyValueList extends Operand {
        private final List<KeyValue> TextList = new ArrayList<>();

        public OperandKeyValueList() {}

        @Override
        public boolean IsArrayJson() { return true; }
        @Override
        public OperandType Type() { return OperandType.ARRAYJSON; }

        @Override
        public List<Operand> ArrayValue() {
            List<Operand> result = new ArrayList<>();
            for (KeyValue kv : TextList) {
                result.add(kv.Value);
            }
            return result;
        }

        @Override
        public Operand ToText(String errorMessage) {
            return Create(toString());
        }
        public Operand ToText(String errorMessage, Object... args) {
            return Create(toString());
        }

        @Override
        public Operand ToArray(String errorMessage) {
            return Create(ArrayValue());
        }
        public Operand ToArray(String errorMessage, Object... args) {
            return Create(ArrayValue());
        }
        @Override
        public Operand TryGetText() { return Create(toString()); }
        @Override
        public Operand TryGetArray() { return Create(ArrayValue()); }

        @Override
        public Operand ToJson(String errorMessage) {
            // 直接构造成 JsonData，免去 toString 序列化再 JsonMapper 解析的完整往返
            JsonData json = ToJsonData();
            if (json == null) return Error(errorMessage != null ? errorMessage : "Convert to json error!");
            return Operand.Create(json);
        }

        JsonData ToJsonData() {
            JsonData json = new JsonData();
            json.SetJsonType(JsonType.Object);
            for (int i = 0; i < TextList.size(); i++) {
                KeyValue kv = TextList.get(i);
                JsonData data = OperandToJsonData(kv.Value);
                if (data == null) return null;
                json.Set(kv.Key, data);
            }
            return json;
        }

        public void AddValue(KeyValue keyValue) {
            TextList.add(keyValue);
        }

        public Operand GetValue(String key) {
            for (KeyValue item : TextList) {
                if (item.Key.equals(key)) {
                    return item.Value;
                }
            }
            return null;
        }

        public boolean TryGetValue(String key, Operand[] outValue) {
            for (KeyValue item : TextList) {
                if (item.Key.equals(key)) {
                    outValue[0] = item.Value;
                    return true;
                }
            }
            return false;
        }

        public boolean ContainsKey(Operand value) {
            for (KeyValue item : TextList) {
                if (item.Key.equals(value.TextValue())) {
                    return true;
                }
            }
            return false;
        }

        public boolean ContainsValue(Operand value) {
            String v = value.TextValue();
            for (KeyValue item : TextList) {
                Operand op = item.Value;
                if (op.IsText()) {
                    if (op.TextValue().equals(v)) return true;
                } else if (op.IsNumber()) {
                    if (op.NumberValue().toString().equals(v)) return true;
                } else if (op.IsBoolean()) {
                    if (String.valueOf(op.BooleanValue()).equalsIgnoreCase(v)) return true;
                }
            }
            return false;
        }

        @Override
        public String toString() {
            StringBuilder sb = new StringBuilder("{");
            for (int i = 0; i < TextList.size(); i++) {
                if (i > 0) sb.append(",");
                KeyValue kv = TextList.get(i);
                sb.append(new OperandString(kv.Key).toString()).append(":").append(kv.Value.toString());
            }
            sb.append("}");
            return sb.toString();
        }
    }
    // endregion
}
