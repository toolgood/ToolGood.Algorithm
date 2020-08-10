package toolgood.algorithm;

import java.text.DecimalFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import toolgood.algorithm.litJson.JsonMapper;
import toolgood.algorithm.litJson.JsonData;

public abstract class Operand {
    private static Locale cultureInfo = Locale.US;
    public final static Operand True = Create(true);
    public final static Operand False = Create(false);

    public boolean IsNull() throws Exception {throw new Exception("");}
    public boolean IsError() throws Exception   {throw new Exception("");}

    public String ErrorMsg() throws Exception  {throw new Exception("");}
    public OperandType Type() throws Exception  {throw new Exception("");}
    public double NumberValue() throws Exception  {throw new Exception("");}
    public int IntValue() throws Exception {throw new Exception("");}
    public String TextValue() throws Exception {throw new Exception("");}
    public boolean BooleanValue() throws Exception {throw new Exception("");}
    public List<Operand> ArrayValue() throws Exception  {throw new Exception("");}
    public JsonData JsonValue() throws Exception  {throw new Exception("");}
    public MyDate DateValue() throws Exception  {throw new Exception("");}

    public static Operand Create(final boolean obj) {
        return new OperandBoolean(obj);
    }
    public static Operand Create(final short obj) {
        return new OperandNumber(obj);
    }
    public static Operand Create(final int obj) {
        return new OperandNumber(obj);
    }
    public static Operand Create(final long obj) {
        return new OperandNumber((double) obj);
    }
    public static Operand Create(final float obj) {
        return new OperandNumber((double) obj);
    }
    public static Operand Create(final double obj) {
        return new OperandNumber(obj);
    }
    public static Operand Create(final BigDecimal obj) {
        return new OperandNumber((double) obj);
    }
    public static Operand Create(final String obj) {
        if (obj.equals(null)) {
            return CreateNull();
        }
        return new OperandString(obj);
    }
    public static Operand CreateJson(final String txt)
    {
        if ((txt.startsWith("{") && txt.endsWith("}")) || (txt.startsWith("[") && txt.endsWith("]")))
        {
            try
            {
                JsonData json = (JsonData)JsonMapper.ToObject(txt);
                return Create(json);
            }
            catch (final Exception e) { }
        }
        return Error("string to json is error!");
    }
    public static Operand Create(final Date obj) {
        return new OperandDate(obj);
    }
    public static Operand Create(final DateTime obj) {
        return new OperandDate(new Date(obj));
    }
    public static Operand Create(final TimeSpan obj) {
        return new OperandDate(new Date(obj));
    }
    public static Operand Create(final JsonData obj) {
        return new OperandJson(obj);
    }
    public static Operand Create(final List<Operand> obj) {
        return new OperandArray(obj);
    }
    public static Operand Create(final List<String> obj)
    {
        final var array = new List<Operand>();
        for (String item : obj) {
            array.Add(Create(item));
        }
        return new OperandArray(array);
    }
    public static Operand Create(List<Double> obj)
    {
        var array = new List<Operand>();
        for (Double item : obj) {
            array.Add(Create(item));
        }
        return new OperandArray(array);
    }
    public static Operand Create(List<Integer> obj)
    {
        var array = new ArrayList<Operand>();
        for (Integer item : obj) {
            array.Add(Create(item));
        }
        return new OperandArray(array);
    }
    public static Operand Create(final List<Boolean> obj)
    {
        final List<Operand> array = new ArrayList<Operand>();
        for (Boolean item : obj) {
            array.Add(Create(item));
        }
        return new OperandArray(array);
    }
    public static Operand Error(final string msg) {
        return new OperandError(msg);
    }
    public static Operand CreateNull() {
        return new OperandNull();
    }
    public Operand ToNumber(final string errorMessage  )
    {
        if (Type == OperandType.NUMBER) { return this; }
        if (IsError) { return this; }
        if (Type == OperandType.BOOLEAN) { return Create(BooleanValue() ? 1.0 : 0.0); }
        if (Type == OperandType.DATE) { return Create((double) DateValue()); }
        if (Type == OperandType.STRING)
        {
            try {
                var d= Double.parseDouble(TextValue());
                return Create(d);
            } catch (Exception e) {
            }
        }
        return Error(errorMessage);
    }
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
            if (TextValue.equals("true", StringComparison.OrdinalIgnoreCase)) {
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
    public Operand ToText(final String errorMessage )
    {
        if (Type == OperandType.STRING) { return this; }
        if (IsError) { return this; }
        if (Type == OperandType.NUMBER) { return Create(NumberValue.ToString(cultureInfo)); }
        if (Type == OperandType.BOOLEAN) { return Create(BooleanValue ? "TRUE" : "FALSE"); }
        if (Type == OperandType.DATE) { return Create(DateValue.ToString()); }

        return Error(errorMessage);
    }
    public Operand ToDate(final String errorMessage   )
    {
        if (Type == OperandType.DATE) { return this; }
        if (IsError) { return this; }
        if (Type == OperandType.NUMBER) { return Create((Date) NumberValue); }
        if (Type == OperandType.STRING)
        {
            Date d= new SimpleDateFormat("yyyy-MM-dd HH:mm:ss").parse(TextValue());
            return Create(new Date(t));
            // if (TimeSpan.TryParse(TextValue, cultureInfo, out TimeSpan t)) { return Create(new Date(t)); }
            // if (DateTime.TryParse(TextValue, cultureInfo, DateTimeStyles.None, out DateTime d)) { return Create(new Date(d)); }
        }
        return Error(errorMessage);
    }
    public Operand ToJson(final String errorMessage  )
    {
        if (Type == OperandType.JSON) { return this; }
        if (IsError) { return this; }
        if (Type == OperandType.STRING)
        {
            final String txt = TextValue();
            if ((txt.StartsWith("{") && txt.EndsWith("}")) || (txt.StartsWith("[") && txt.EndsWith("]")))
            {
                try
                {
                    final JsonData json = JsonMapper.ToObject(txt);
                    return Operand.Create(json);
                }
                catch (final Exception e) { }
            }
        }
        return Error(errorMessage);
    }
    public Operand ToArray(final String errorMessage )
    {
        if (Type == OperandType.ARRARY) { return this; }
        if (IsError) { return this; }
        if (Type == OperandType.JSON)
        {
            if (JsonValue.IsArray)
            {
                final List<Operand> list = new ArrayList<Operand>();
                for (JsonData v : JsonValue().inst_array) {
                    if (v.IsString)
                        list.Add(Create(v.StringValue()));
                    else if (v.IsBoolean)
                        list.Add(Create(v.BooleanValue()));
                    else if (v.IsDouble)
                        list.Add(Create(v.NumberValue()));
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
