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
    public static final Operand ONE = Operand.create(new BigDecimal("1"));

    /**
     * Zero
     */
    public static final Operand ZERO = Operand.create(new BigDecimal("0"));


    /**
     * 是否为空值
     */
    public boolean isNull() { return false; }
    /**
     * 是否为非空值
     */
    public boolean isNotNull() { return true; }

    /**
     * 是否数字
     */
    public boolean isNumber() { return false; }
    /**
     * 是否非数字
     */
    public boolean isNotNumber() { return true; }

    /**
     * 是否字符串
     */
    public boolean isText() { return false; }
    /**
     * 是否非字符串
     */
    public boolean isNotText() { return true; }

    /**
     * 是否布尔值
     */
    public boolean isBoolean() { return false; }
    /**
     * 是否非布尔值
     */
    public boolean isNotBoolean() { return true; }
    /**
     * 是否数组
     */
    public boolean isArray() { return false; }
    /**
     * 是否非数组
     */
    public boolean isNotArray() { return true; }
    /**
     * 是否日期
     */
    public boolean isDate() { return false; }
    /**
     * 是否非日期
     */
    public boolean isNotDate() { return true; }
    /**
     * 是否Json对象
     */
    public boolean isJson() { return false; }
    /**
     * 是否非Json对象
     */
    public boolean isNotJson() { return true; }
    /**
     * 是否Json数组
     */
    public boolean isArrayJson() { return false; }
    /**
     * 是否非Json数组
     */
    public boolean isNotArrayJson() { return true; }

    /**
     * 是否出错
     */
    public boolean isError() { return false; }

    /**
     * 错误信息
     */
    public String getErrorMsg() { return null; }
    
    /**
     * 操作数类型
     */
    public abstract OperandType getType();

    

    /**
     * 数字值
     */
    public BigDecimal getNumberValue() { throw new UnsupportedOperationException(); }

    /**
     * double值
     */
    public double getDoubleValue() { throw new UnsupportedOperationException(); }

    /**
     * BigDecimal值
     */
    public BigDecimal getBigDecimalValue() { throw new UnsupportedOperationException(); }

    /**
     * int值
     */
    public int getIntValue() { throw new UnsupportedOperationException(); }

    /**
     * long值
     */
    public long getLongValue() { throw new UnsupportedOperationException(); }

    /**
     * 字符串值
     */
    public String getTextValue() { throw new UnsupportedOperationException(); }

    /**
     * 布尔值
     */
    public boolean getBooleanValue() { throw new UnsupportedOperationException(); }

    /**
     * 数组值
     */
    public List<Operand> getArrayValue() { throw new UnsupportedOperationException(); }

    /**
     * Json值
     */
    JsonData getJsonValue() { throw new UnsupportedOperationException(); }

    /**
     * 时间值
     */
    public MyDate getDateValue() { throw new UnsupportedOperationException(); }

    
    
    /**
     * 创建操作数
     */
    public static Operand create(boolean obj) {
        return obj ? TRUE : FALSE;
    }

    

    /**
     * 创建操作数
     */
    public static Operand create(short obj) {
        return new OperandInt(obj);
    }

    /**
     * 创建操作数
     */
    public static Operand create(int obj) {
        return new OperandInt(obj);
    }

    /**
     * 创建操作数
     */
    public static Operand create(long obj) {
        return new OperandBigDecimal(new BigDecimal(obj));
    }

    /**
     * 创建操作数
     */
    public static Operand create(float obj) {
        return new OperandBigDecimal(new BigDecimal(obj));
    }

    /**
     * 创建操作数
     */
    public static Operand create(double obj) {
        return new OperandBigDecimal(new BigDecimal(obj));
    }

    /**
     * 创建操作数
     */
    public static Operand create(BigDecimal obj) {
        return new OperandBigDecimal(obj);
    }

    

    /**
     * 创建操作数
     */
    public static Operand create(String obj) {
        if (obj == null) {
            return Operand.createNull();
        }
        return new OperandString(obj);
    }

    /**
     * 创建操作数
     */
    public static Operand createJson(String txt) {
        if ((txt.startsWith("{") && txt.endsWith("}")) || (txt.startsWith("[") && txt.endsWith("]"))) {
            try {
                JsonData json = JsonData.parse(txt);
                return Operand.create(json);
            } catch (Exception e) {
            }
        }
        return Operand.error("Convert to json error!");
    }

    /**
     * 创建操作数
     */
    public static Operand create(MyDate obj) {
        return new OperandMyDate(obj);
    }

    /**
     * 创建操作数
     */
    public static Operand create(long timestamp) {
        return new OperandMyDate(new MyDate(timestamp));
    }

    /**
     * 创建操作数
     */
    public static Operand create(JsonData obj) {
        return new OperandJson(obj);
    }

    /**
     * 创建操作数
     */
    public static Operand create(List<Operand> obj) {
        return new OperandArray(obj);
    }

    /**
     * 创建操作数
     */
    public static Operand create(Collection<String> obj) {
        List<Operand> array = new ArrayList<>();
        for (String item : obj) {
            array.add(create(item));
        }
        return new OperandArray(array);
    }

    /**
     * 创建操作数
     */
    public static Operand create(Collection<Double> obj) {
        List<Operand> array = new ArrayList<>();
        for (Double item : obj) {
            array.add(create(new BigDecimal(item.toString())));
        }
        return new OperandArray(array);
    }

    /**
     * 创建操作数
     */
    public static Operand create(Collection<BigDecimal> obj) {
        List<Operand> array = new ArrayList<>();
        for (BigDecimal item : obj) {
            array.add(create(item));
        }
        return new OperandArray(array);
    }

    /**
     * 创建操作数
     */
    public static Operand create(Collection<Integer> obj) {
        List<Operand> array = new ArrayList<>();
        for (Integer item : obj) {
            array.add(create(item));
        }
        return new OperandArray(array);
    }

    /**
     * 创建操作数
     */
    public static Operand create(Collection<Boolean> obj) {
        List<Operand> array = new ArrayList<>();
        for (Boolean item : obj) {
            array.add(create(item));
        }
        return new OperandArray(array);
    }

    /**
     * 创建操作数
     */
    public static Operand error(String msg) {
        return new OperandError(msg);
    }

    /**
     * 创建操作数
     */
    public static Operand error(String msg, Object... args) {
        return new OperandError(String.format(msg, args));
    }

    /**
     * 创建操作数
     */
    public static Operand createNull() {
        return new OperandNull();
    }

    

    

    /**
     * 转数值类型
     */
    public Operand toNumber(String errorMessage) {
        return error(errorMessage != null ? errorMessage : "Convert to number error!");
    }

    /**
     * 转数值类型
     */
    public Operand toNumber(String errorMessage, Object... args) {
        return error(String.format(errorMessage, args));
    }

    /**
     * 转bool类型
     */
    public Operand toBoolean(String errorMessage) {
        return error(errorMessage != null ? errorMessage : "Convert to bool error!");
    }

    /**
     * 转bool类型
     */
    public Operand toBoolean(String errorMessage, Object... args) {
        return error(String.format(errorMessage, args));
    }

    /**
     * 转string类型
     */
    public Operand toText(String errorMessage) {
        return error(errorMessage != null ? errorMessage : "Convert to string error!");
    }

    /**
     * 转string类型
     */
    public Operand toText(String errorMessage, Object... args) {
        return error(String.format(errorMessage, args));
    }

    /**
     * 转MyDate类型
     */
    public Operand toMyDate(String errorMessage) {
        return error(errorMessage != null ? errorMessage : "Convert to date error!");
    }

    /**
     * 转MyDate类型
     */
    public Operand toMyDate(String errorMessage, Object... args) {
        return error(String.format(errorMessage, args));
    }

    /**
     * 转Array类型
     */
    public Operand toArray(String errorMessage) {
        return error(errorMessage != null ? errorMessage : "Convert to array error!");
    }

    /**
     * 转Array类型
     */
    public Operand toArray(String errorMessage, Object... args) {
        return error(String.format(errorMessage, args));
    }

    /**
     * 转Json类型
     */
    public Operand toJson(String errorMessage) {
        return error(errorMessage != null ? errorMessage : "Convert to json error!");
    }

    
}
