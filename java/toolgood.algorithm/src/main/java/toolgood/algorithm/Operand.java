package toolgood.algorithm;

import java.text.DecimalFormat;
import java.util.ArrayList;
import java.util.List;

import toolgood.algorithm.litJson.JsonMapper;
import toolgood.algorithm.litJson.JsonData;

public abstract class Operand {
    private static CultureInfo cultureInfo = CultureInfo.GetCultureInfo("en-US");
    /// <summary>
    /// True 值
    /// </summary>
    public final static Operand True = Create(true);
    /// <summary>
    /// False 值
    /// </summary>
    public final static Operand False = Create(false);

    /// <summary>
    /// 是否为空
    /// </summary>
    public abstract boolean IsNull();

    /// <summary>
    /// 是否出错
    /// </summary>
    public abstract boolean IsError();

    /// <summary>
    /// 错误信息
    /// </summary>
    public abstract String ErrorMsg();

    /// <summary>
    /// 操作数类型
    /// </summary>
    public abstract OperandType Type();

    /// <summary>
    /// 数字值
    /// </summary>
    public abstract double NumberValue();

    /// <summary>
    /// int值
    /// </summary>
    public abstract int IntValue();

    /// <summary>
    /// 字符串值
    /// </summary>
    public abstract String TextValue();

    /// <summary>
    /// 布尔值
    /// </summary>
    public abstract boolean BooleanValue();

    /// <summary>
    /// 数组值
    /// </summary>
    public abstract List<Operand> ArrayValue();

    abstract JsonData JsonValue();

    /// <summary>
    /// 时间值
    /// </summary>
    public abstract Date DateValue();

    /// <summary>
    /// 创建操作数
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Operand Create(final boolean obj) {
        return new OperandBoolean(obj);
    }

    /// <summary>
    /// 创建操作数
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Operand Create(final short obj) {
        return new OperandNumber(obj);
    }

    /// <summary>
    /// 创建操作数
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Operand Create(final int obj) {
        return new OperandNumber(obj);
    }

    /// <summary>
    /// 创建操作数
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Operand Create(final long obj) {
        return new OperandNumber((double) obj);
    }

    /// <summary>
    /// 创建操作数
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Operand Create(final float obj) {
        return new OperandNumber((double) obj);
    }

    /// <summary>
    /// 创建操作数
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Operand Create(final double obj) {
        return new OperandNumber(obj);
    }

    /// <summary>
    /// 创建操作数
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Operand Create(final DecimalFormat obj) {
        return new OperandNumber((double) obj);
    }

    /// <summary>
    /// 创建操作数
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Operand Create(final String obj) {
        if (obj.equals(null)) {
            return CreateNull();
        }
        return new OperandString(obj);
    }

    /// <summary>
    /// 创建操作数
    /// </summary>
    /// <param name="txt"></param>
    /// <returns></returns>
    public static Operand CreateJson(final String txt)
    {
        if ((txt.startsWith("{") && txt.endsWith("}")) || (txt.startsWith("[") && txt.endsWith("]")))
        {
            try
            {
                  JsonData json = (JsonData)JsonMapper.ToObject(txt);
                return Create(json);
            }
            catch (final Exception) { }
        }
        return Error("string to json is error!");
    }

    /// <summary>
    /// 创建操作数
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Operand Create(final Date obj) {
        return new OperandDate(obj);
    }

    /// <summary>
    /// 创建操作数
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Operand Create(final DateTime obj) {
        return new OperandDate(new Date(obj));
    }

    /// <summary>
    /// 创建操作数
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Operand Create(final TimeSpan obj) {
        return new OperandDate(new Date(obj));
    }

    /// <summary>
    /// 创建操作数
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    static Operand Create(final JsonData obj) {
        return new OperandJson(obj);
    }

    /// <summary>
    /// 创建操作数
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Operand Create(final List<Operand> obj) {
        return new OperandArray(obj);
    }

    /// <summary>
    /// 创建操作数
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Operand Create(final ICollection<string> obj)
    {
        final var array = new List<Operand>();
        foreach (var item in obj)
        {
            array.Add(Create(item));
        }
        return new OperandArray(array);
    }

    /// <summary>
    /// 创建操作数
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Operand Create(List<Double> obj)
    {
        var array = new List<Operand>();
        
        foreach (final var item final in obj)
        {
            array.Add(Create(item));
        }
        return new OperandArray(array);
    }

    /// <summary>
    /// 创建操作数
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Operand Create(ICollection<Inter> obj)
    {
        var array = new ArrayList<Operand>();
        foreach (final var item final in obj)
        {
            array.Add(Create(item));
        }
        return new OperandArray(array);
    }

    /// <summary>
    /// 创建操作数
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Operand Create(final List<Boolean> obj)
    {
        final List<Operand> array = new ArrayList<Operand>();
        foreach (var item in obj)
        {
            array.Add(Create(item));
        }
        return new OperandArray(array);
    }

    /// <summary>
    /// 创建操作数
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    public static Operand Error(final string msg) {
        return new OperandError(msg);
    }

    /// <summary>
    /// 创建操作数
    /// </summary>
    /// <returns></returns>
    public static Operand CreateNull() {
        return new OperandNull();
    }

    /// <summary>
    /// 转数值类型
    /// </summary>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    public Operand ToNumber(final string errorMessage  )
    {
        if (Type == OperandType.NUMBER) { return this; }
        if (IsError) { return this; }
        if (Type == OperandType.BOOLEAN) { return Create(BooleanValue ? 1.0 : 0.0); }
        if (Type == OperandType.DATE) { return Create((double) DateValue); }
        if (Type == OperandType.STRING)
        {
            if (double.TryParse(TextValue, NumberStyles.Any, cultureInfo, out double d))
            {
                return Create(d);
            }
        }
        return Error(errorMessage);
    }

    /// <summary>
    /// 转bool类型
    /// </summary>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    public Operand ToBoolean(final String errorMessage) {
        if (Type == OperandType.BOOLEAN) {
            return this;
        }
        if (IsError) {
            return this;
        }
        if (Type == OperandType.NUMBER) {
            return Create(NumberValue != 0);
        }
        if (Type == OperandType.DATE) {
            return Create(((double) DateValue) != 0);
        }
        if (Type == OperandType.STRING) {
            if (TextValue.Equals("true", StringComparison.OrdinalIgnoreCase)) {
                return Create(true);
            }
            if (TextValue.Equals("false", StringComparison.OrdinalIgnoreCase)) {
                return Create(false);
            }
            if (TextValue.Equals("1", StringComparison.OrdinalIgnoreCase)) {
                return Create(true);
            }
            if (TextValue.Equals("0", StringComparison.OrdinalIgnoreCase)) {
                return Create(false);
            }
        }
        return Error(errorMessage);
    }

    /// <summary>
    /// 转String类型
    /// </summary>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    public Operand ToText(final String errorMessage )
    {
        if (Type == OperandType.STRING) { return this; }
        if (IsError) { return this; }
        if (Type == OperandType.NUMBER) { return Create(NumberValue.ToString(cultureInfo)); }
        if (Type == OperandType.BOOLEAN) { return Create(BooleanValue ? "TRUE" : "FALSE"); }
        if (Type == OperandType.DATE) { return Create(DateValue.ToString()); }

        return Error(errorMessage);
    }

    /// <summary>
    /// 转Date类型
    /// </summary>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    public Operand ToDate(final String errorMessage   )
    {
        if (Type == OperandType.DATE) { return this; }
        if (IsError) { return this; }
        if (Type == OperandType.NUMBER) { return Create((Date) NumberValue); }
        if (Type == OperandType.STRING)
        {
            if (TimeSpan.TryParse(TextValue, cultureInfo, out TimeSpan t)) { return Create(new Date(t)); }
            if (DateTime.TryParse(TextValue, cultureInfo, DateTimeStyles.None, out DateTime d)) { return Create(new Date(d)); }
        }
        return Error(errorMessage);
    }

    /// <summary>
    /// 转Json类型
    /// </summary>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    public Operand ToJson(final String errorMessage  )
    {
        if (Type == OperandType.JSON) { return this; }
        if (IsError) { return this; }
        if (Type == OperandType.STRING)
        {
            final var txt = TextValue;
            if ((txt.StartsWith("{") && txt.EndsWith("}")) || (txt.StartsWith("[") && txt.EndsWith("]")))
            {
                try
                {
                    final var json = JsonMapper.ToObject(txt);
                    return Operand.Create(json);
                }
                catch (final Exception) { }
            }
        }
        return Error(errorMessage);
    }

    /// <summary>
    /// 转Array类型
    /// </summary>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    public Operand ToArray(final String errorMessage )
    {
        if (Type == OperandType.ARRARY) { return this; }
        if (IsError) { return this; }
        if (Type == OperandType.JSON)
        {
            if (JsonValue.IsArray)
            {
                final List<Operand> list = new List<Operand>();
                foreach (JsonData v in JsonValue)
                {
                    if (v.IsString)
                        list.Add(Create(v.StringValue));
                    else if (v.IsBoolean)
                        list.Add(Create(v.BooleanValue));
                    else if (v.IsDouble)
                        list.Add(Create(v.NumberValue));
                    else if (v.IsNull)
                        list.Add(CreateNull());
                    else
                        list.Add(Create(v));
                }
                return Create(list);
            }
        }
        return Error(errorMessage);
    }
}
