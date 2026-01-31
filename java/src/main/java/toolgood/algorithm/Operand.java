/**
 * 操作数
 */
package toolgood.algorithm;

import java.math.BigDecimal;
import java.util.List;
import java.util.Collection;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.MyDate;
import toolgood.algorithm.internals.JsonData;

public abstract class Operand {
    /**
     * 版本号
     */
    public static final Operand VERSION = new OperandString("ToolGood.Algorithm 6.1");

    /**
     * True
     */
    public static final Operand TRUE = new OperandBoolean(true);

    /**
     * False
     */
    public static final Operand FALSE = new OperandBoolean(false);

    /**
     * One
     */
    public static final Operand ONE = Operand.Create(new BigDecimal("1"));

    /**
     * Zero
     */
    public static final Operand ZERO = Operand.Create(new BigDecimal("0"));


    /**
     * 是否为空值
     */
    public boolean IsNull() { return false; }
    /**
     * 是否为非空值
     */
    public boolean IsNotNull() { return true; }

    /**
     * 是否数字
     */
    public boolean IsNumber() { return false; }
    /**
     * 是否非数字
     */
    public boolean IsNotNumber() { return true; }

    /**
     * 是否字符串
     */
    public boolean IsText() { return false; }
    /**
     * 是否非字符串
     */
    public boolean IsNotText() { return true; }

    /**
     * 是否布尔值
     */
    public boolean IsBoolean() { return false; }
    /**
     * 是否非布尔值
     */
    public boolean IsNotBoolean() { return true; }
    /**
     * 是否数组
     */
    public boolean IsArray() { return false; }
    /**
     * 是否非数组
     */
    public boolean IsNotArray() { return true; }
    /**
     * 是否日期
     */
    public boolean IsDate() { return false; }
    /**
     * 是否非日期
     */
    public boolean IsNotDate() { return true; }
    /**
     * 是否Json对象
     */
    public boolean IsJson() { return false; }
    /**
     * 是否非Json对象
     */
    public boolean IsNotJson() { return true; }
    /**
     * 是否Json数组
     */
    public boolean IsArrayJson() { return false; }
    /**
     * 是否非Json数组
     */
    public boolean IsNotArrayJson() { return true; }

    /**
     * 是否出错
     */
    public boolean IsError() { return false; }

    /**
     * 错误信息
     */
    public String ErrorMsg() { return null; }
    
    /**
     * 操作数类型
     */
    public abstract OperandType Type();

    

    /**
     * 数字值
     */
    public BigDecimal NumberValue() { throw new UnsupportedOperationException(); }

    /**
     * double值
     */
    public double DoubleValue() { throw new UnsupportedOperationException(); }

    /**
     * BigDecimal值
     */
    public Double DoubleValue() { throw new UnsupportedOperationException(); }

    /**
     * int值
     */
    public int IntValue() { throw new UnsupportedOperationException(); }

    /**
     * long值
     */
    public long LongValue() { throw new UnsupportedOperationException(); }

    /**
     * 字符串值
     */
    public String TextValue() { throw new UnsupportedOperationException(); }

    /**
     * 布尔值
     */
    public boolean BooleanValue() { throw new UnsupportedOperationException(); }

    /**
     * 数组值
     */
    public List<Operand> ArrayValue() { throw new UnsupportedOperationException(); }

    /**
     * Json值
     */
    JsonData JsonValue() { throw new UnsupportedOperationException(); }

    /**
     * 时间值
     */
    public MyDate DateValue() { throw new UnsupportedOperationException(); }

    
    
    /**
     * 创建操作数
     */
    public static Operand Create(boolean obj) {
        return obj ? TRUE : FALSE;
    }

    

    /**
     * 创建操作数
     */
    public static Operand Create(short obj) {
        return new OperandInt(obj);
    }

    /**
     * 创建操作数
     */
    public static Operand Create(int obj) {
        return new OperandInt(obj);
    }

    /**
     * 创建操作数
     */
    public static Operand Create(long obj) {
        return new OperandBigDecimal(new BigDecimal(obj));
    }

    /**
     * 创建操作数
     */
    public static Operand Create(float obj) {
        return new OperandBigDecimal(new BigDecimal(obj));
    }

    /**
     * 创建操作数
     */
    public static Operand Create(double obj) {
        return new OperandBigDecimal(new BigDecimal(obj));
    }

    /**
     * 创建操作数
     */
    public static Operand Create(BigDecimal obj) {
        return new OperandBigDecimal(obj);
    }

    

    /**
     * 创建操作数
     */
    public static Operand Create(String obj) {
        if (obj == null) {
            return Operand.CreateNull();
        }
        return new OperandString(obj);
    }

    /**
     * 创建操作数
     */
    public static Operand CreateJson(String txt) {
        if ((txt.startsWith("{") && txt.endsWith("}")) || (txt.startsWith("[") && txt.endsWith("]"))) {
            try {
                JsonData json = JsonData.parse(txt);
                return Operand.Create(json);
            } catch (Exception e) {
            }
        }
        return Operand.Error("Convert to json Error!");
    }

    /**
     * 创建操作数
     */
    public static Operand Create(MyDate obj) {
        return new OperandMyDate(obj);
    }

    /**
     * 创建操作数
     */
    public static Operand Create(long timestamp) {
        return new OperandMyDate(new MyDate(timestamp));
    }

    /**
     * 创建操作数
     */
    public static Operand Create(JsonData obj) {
        return new OperandJson(obj);
    }

    /**
     * 创建操作数
     */
    public static Operand Create(List<Operand> obj) {
        return new OperandArray(obj);
    }

    /**
     * 创建操作数
     */
    public static Operand Create(Collection<String> obj) {
        List<Operand> array = new ArrayList<>();
        for (String item : obj) {
            array.add(Create(item));
        }
        return new OperandArray(array);
    }

    /**
     * 创建操作数
     */
    public static Operand Create(Collection<Double> obj) {
        List<Operand> array = new ArrayList<>();
        for (Double item : obj) {
            array.add(Create(new BigDecimal(item.toString())));
        }
        return new OperandArray(array);
    }

    /**
     * 创建操作数
     */
    public static Operand Create(Collection<BigDecimal> obj) {
        List<Operand> array = new ArrayList<>();
        for (BigDecimal item : obj) {
            array.add(Create(item));
        }
        return new OperandArray(array);
    }

    /**
     * 创建操作数
     */
    public static Operand Create(Collection<Integer> obj) {
        List<Operand> array = new ArrayList<>();
        for (Integer item : obj) {
            array.add(Create(item));
        }
        return new OperandArray(array);
    }

    /**
     * 创建操作数
     */
    public static Operand Create(Collection<Boolean> obj) {
        List<Operand> array = new ArrayList<>();
        for (Boolean item : obj) {
            array.add(Create(item));
        }
        return new OperandArray(array);
    }

    /**
     * 创建操作数
     */
    public static Operand Error(String msg) {
        return new OperandError(msg);
    }

    /**
     * 创建操作数
     */
    public static Operand Error(String msg, Object... args) {
        return new OperandError(String.format(msg, args));
    }

    /**
     * 创建操作数
     */
    public static Operand CreateNull() {
        return new OperandNull();
    }

    

    

    /**
     * 转数值类型
     */
    public Operand ToNumber(String errorMessage) {
        return Error(errorMessage != null ? errorMessage : "Convert to number Error!");
    }

    /**
     * 转数值类型
     */
    public Operand ToNumber(String errorMessage, Object... args) {
        return Error(String.format(errorMessage, args));
    }

    /**
     * 转bool类型
     */
    public Operand ToBoolean(String errorMessage) {
        return Error(errorMessage != null ? errorMessage : "Convert to bool Error!");
    }

    /**
     * 转bool类型
     */
    public Operand ToBoolean(String errorMessage, Object... args) {
        return Error(String.format(errorMessage, args));
    }

    /**
     * 转string类型
     */
    public Operand ToText(String errorMessage) {
        return Error(errorMessage != null ? errorMessage : "Convert to string Error!");
    }

    /**
     * 转string类型
     */
    public Operand ToText(String errorMessage, Object... args) {
        return Error(String.format(errorMessage, args));
    }

    /**
     * 转MyDate类型
     */
    public Operand ToMyDate(String errorMessage) {
        return Error(errorMessage != null ? errorMessage : "Convert to date Error!");
    }

    /**
     * 转MyDate类型
     */
    public Operand ToMyDate(String errorMessage, Object... args) {
        return Error(String.format(errorMessage, args));
    }

    /**
     * 转Array类型
     */
    public Operand ToArray(String errorMessage) {
        return Error(errorMessage != null ? errorMessage : "Convert to array Error!");
    }

    /**
     * 转Array类型
     */
    public Operand ToArray(String errorMessage, Object... args) {
        return Error(String.format(errorMessage, args));
    }

    /**
     * 转Json类型
     */
    public Operand ToJson(String errorMessage) {
        return Error(errorMessage != null ? errorMessage : "Convert to json Error!");
    }

    
}
