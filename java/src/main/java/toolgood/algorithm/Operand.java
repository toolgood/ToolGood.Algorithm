package toolgood.algorithm;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.ArrayList;
import java.util.List;

import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.litJson.JsonData;
import toolgood.algorithm.litJson.JsonMapper;
import toolgood.algorithm.operands.MyDate;
import toolgood.algorithm.operands.OperandArray;
import toolgood.algorithm.operands.OperandBigDecimal;
import toolgood.algorithm.operands.OperandBoolean;
import toolgood.algorithm.operands.OperandError;
import toolgood.algorithm.operands.OperandInt;
import toolgood.algorithm.operands.OperandJson;
import toolgood.algorithm.operands.OperandMyDate;
import toolgood.algorithm.operands.OperandNull;
import toolgood.algorithm.operands.OperandNone;
import toolgood.algorithm.operands.OperandString;

public abstract class Operand {
    public static final Operand Version = new OperandString("ToolGood.Algorithm 6.2");

    public static final Operand True = new OperandBoolean(true);

    public static final Operand False = new OperandBoolean(false);

    public static final Operand One;

    public static final Operand Zero;

    public static final Operand Null = new OperandNull();

    public static final Operand None = new OperandNone();

    private static final int IntCacheOffset = 1000;
    private static final int IntCacheSize = 2001;
    private static final Operand[] IntCache = new Operand[IntCacheSize];

    static {
        for (int i = 0; i < IntCacheSize; i++) {
            IntCache[i] = new OperandInt(i - IntCacheOffset);
        }
        One = Operand.Create(1);
        Zero = Operand.Create(0);
    }

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

    public abstract OperandType Type();

    public BigDecimal NumberValue() { throw new UnsupportedOperationException(); }

    public double DoubleValue() { throw new UnsupportedOperationException(); }

    public int IntValue() { throw new UnsupportedOperationException(); }

    public long LongValue() { throw new UnsupportedOperationException(); }

    public String TextValue() { throw new UnsupportedOperationException(); }

    public boolean BooleanValue() { throw new UnsupportedOperationException(); }

    public List<Operand> ArrayValue() { throw new UnsupportedOperationException(); }

    public JsonData JsonValue() { throw new UnsupportedOperationException(); }

    public MyDate DateValue() { throw new UnsupportedOperationException(); }

    public static Operand Create(boolean obj) {
        return obj ? True : False;
    }

    public static Operand Create(short obj) {
        return IntCache[obj + IntCacheOffset];
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
        return new OperandBigDecimal(new BigDecimal(obj));
    }

    public static Operand Create(int obj, boolean isUnsigned) {
        if (obj <= IntCacheOffset) {
            return IntCache[obj + IntCacheOffset];
        }
        return new OperandBigDecimal(new BigDecimal(obj));
    }

    public static Operand Create(long obj, boolean isUnsigned) {
        if (obj <= IntCacheOffset) {
            return IntCache[(int) obj + IntCacheOffset];
        }
        return new OperandBigDecimal(new BigDecimal(obj));
    }

    public static Operand Create(float obj) {
        if (obj == Math.floor(obj) && obj >= -IntCacheOffset && obj <= IntCacheOffset) {
            return IntCache[(int) obj + IntCacheOffset];
        }
        return new OperandBigDecimal(new BigDecimal(obj));
    }

    public static Operand Create(double obj) {
        if (obj == Math.floor(obj) && obj >= -IntCacheOffset && obj <= IntCacheOffset) {
            return IntCache[(int) obj + IntCacheOffset];
        }
        return new OperandBigDecimal(new BigDecimal(obj));
    }

    public static Operand Create(BigDecimal obj) {
        if (obj.setScale(0, RoundingMode.DOWN).compareTo(obj) == 0 && obj.compareTo(BigDecimal.valueOf(-IntCacheOffset)) >= 0 && obj.compareTo(BigDecimal.valueOf(IntCacheOffset)) <= 0) {
            return IntCache[obj.intValue() + IntCacheOffset];
        }
        return new OperandBigDecimal(obj);
    }

    public static Operand Create(String obj) {
        return obj == null ? Null : new OperandString(obj);
    }

    public static Operand CreateJson(String txt) {
        if ((txt.startsWith("{") && txt.endsWith("}")) || (txt.startsWith("[") && txt.endsWith("]"))) {
            try {
                JsonData json = JsonMapper.ToObject(txt);
                return Operand.Create(json);
            } catch (Exception e) {
            }
        }
        return Operand.Error("Convert to json error!");
    }

    public static Operand Create(MyDate obj) {
        return new OperandMyDate(obj);
    }

    public static Operand Create(JsonData obj) {
        return new OperandJson(obj);
    }

    public static Operand Create(List<Operand> obj) {
        return new OperandArray(obj);
    }

    public static Operand CreateStringCollection(String[] obj) {
        List<Operand> array = new ArrayList<>(obj.length);
        for (String item : obj) {
            array.add(Create(item));
        }
        return new OperandArray(array);
    }

    public static Operand CreateDoubleCollection(Double[] obj) {
        List<Operand> array = new ArrayList<>(obj.length);
        for (Double item : obj) {
            array.add(Create(item));
        }
        return new OperandArray(array);
    }

    public static Operand CreateIntCollection(Integer[] obj) {
        List<Operand> array = new ArrayList<>(obj.length);
        for (Integer item : obj) {
            array.add(Create(item));
        }
        return new OperandArray(array);
    }

    public static Operand CreateBooleanCollection(Boolean[] obj) {
        List<Operand> array = new ArrayList<>(obj.length);
        for (Boolean item : obj) {
            array.add(Create(item));
        }
        return new OperandArray(array);
    }

    public static Operand Error(String msg) {
        return new OperandError(msg);
    }

    public static Operand Error(String msg, Object... args) {
        return new OperandError(msg, args);
    }

    public Operand ToNumber(String errorMessage) {
        return Error(errorMessage != null ? errorMessage : "Convert to number error!");
    }

    public Operand ToNumber(String errorMessage, Object... args) {
        return new OperandError(errorMessage, args);
    }

    public Operand ToBoolean(String errorMessage) {
        return Error(errorMessage != null ? errorMessage : "Convert to bool error!");
    }

    public Operand ToBoolean(String errorMessage, Object... args) {
        return new OperandError(errorMessage, args);
    }

    public Operand ToText(String errorMessage) {
        return Error(errorMessage != null ? errorMessage : "Convert to string error!");
    }

    public Operand ToText(String errorMessage, Object... args) {
        return new OperandError(errorMessage, args);
    }

    public Operand ToMyDate(String errorMessage) {
        return Error(errorMessage != null ? errorMessage : "Convert to date error!");
    }

    public Operand ToMyDate(String errorMessage, Object... args) {
        return new OperandError(errorMessage, args);
    }

    public Operand ToArray(String errorMessage) {
        return Error(errorMessage != null ? errorMessage : "Convert to array error!");
    }

    public Operand ToArray(String errorMessage, Object... args) {
        return new OperandError(errorMessage, args);
    }

    public Operand ToJson(String errorMessage) {
        return Error(errorMessage != null ? errorMessage : "Convert to json error!");
    }
}
