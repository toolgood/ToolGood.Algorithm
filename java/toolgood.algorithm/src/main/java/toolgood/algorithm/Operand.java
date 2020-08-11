package toolgood.algorithm;

import java.math.BigDecimal;
import java.text.DecimalFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Locale;

import toolgood.algorithm.litJson.JsonMapper;
import toolgood.algorithm.litJson.JsonData;

public abstract class Operand {
    private static Locale cultureInfo = Locale.US;
    public final static Operand True = Create(true);
    public final static Operand False = Create(false);

    public boolean IsNull() {return false;}
    public boolean IsError() {return false;}

    public String ErrorMsg() {return null;}
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
        return new OperandNumber(new Double(obj));
    }
    public static Operand Create(final int obj) {
        return new OperandNumber(new Double(obj));
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
        return new OperandNumber(obj.doubleValue());
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
    public static Operand Create(final MyDate obj) {
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
    public static Operand Create(List<Operand> obj) {
        return new OperandArray(obj);
    }
  
    public static Operand Error(final String msg) {
        return new OperandError(msg);
    }
    public static Operand CreateNull() {
        return new OperandNull();
    }
    public Operand ToNumber(final String errorMessage  )
    {
        if (Type() == OperandType.NUMBER) { return this; }
        if (IsError()) { return this; }
        if (Type() == OperandType.BOOLEAN) { return Create(BooleanValue() ? 1.0 : 0.0); }
        if (Type() == OperandType.DATE) { return Create((double) DateValue()); }
        if (Type() == OperandType.STRING)
        {
            try {
                Double d= Double.parseDouble(TextValue());
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
        if (Type() == OperandType.NUMBER) {
            return Create(NumberValue() != 0);
        }
        if (Type() == OperandType.DATE) {
            return Create(((double) DateValue()) != 0);
        }
        if (Type() == OperandType.STRING) {
            if (TextValue().toLowerCase().equals("true")) {
                return Create(true);
            }
            if (TextValue().toLowerCase().equals("false")) {
                return Create(false);
            }
            if (TextValue().equals("1")) {
                return Create(true);
            }
            if (TextValue().equals("0")) {
                return Create(false);
            }
        }
        return Error(errorMessage);
    }
    public Operand ToText(final String errorMessage )
    {
        if (Type() == OperandType.STRING) { return this; }
        if (IsError()) { return this; }
        if (Type() == OperandType.NUMBER) { return Create(NumberValue().ToString(cultureInfo)); }
        if (Type() == OperandType.BOOLEAN) { return Create(BooleanValue() ? "TRUE" : "FALSE"); }
        if (Type() == OperandType.DATE) { return Create(DateValue().toString()); }

        return Error(errorMessage);
    }
    public Operand ToDate(final String errorMessage   )
    {
        if (Type() == OperandType.DATE) { return this; }
        if (IsError()) { return this; }
        if (Type() == OperandType.NUMBER) { return Create((Date) NumberValue()); }
        if (Type() == OperandType.STRING)
        {
            Date d= new SimpleDateFormat("yyyy-MM-dd HH:mm:ss").parse(TextValue());
            return Create(new MyDate(t));
            // if (TimeSpan.TryParse(TextValue, cultureInfo, out TimeSpan t)) { return Create(new Date(t)); }
            // if (DateTime.TryParse(TextValue, cultureInfo, DateTimeStyles.None, out DateTime d)) { return Create(new Date(d)); }
        }
        return Error(errorMessage);
    }
    public Operand ToJson(final String errorMessage  ) throws Exception
    {
        if (Type() == OperandType.JSON) { return this; }
        if (IsError()) { return this; }
        if (Type() == OperandType.STRING)
        {
            final String txt = TextValue();
            if ((txt.startsWith("{") && txt.endsWith("}")) || (txt.startsWith("[") && txt.endsWith("]")))
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
    public Operand ToArray(final String errorMessage ) throws Exception
    {
        if (Type() == OperandType.ARRARY) { return this; }
        if (IsError()) { return this; }
        if (Type() == OperandType.JSON)
        {
            if (JsonValue().IsArray())
            {
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
}
