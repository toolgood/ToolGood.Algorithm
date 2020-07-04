using System;
using System.Collections.Generic;
using System.Globalization;
using ToolGood.Algorithm.LitJson;

namespace ToolGood.Algorithm
{
    /// <summary>
    /// 操作数
    /// </summary>
    public abstract class Operand : IDisposable
    {
        private static readonly CultureInfo cultureInfo = CultureInfo.GetCultureInfo("en-US");
        public static readonly Operand True = Operand.Create(true);
        public static readonly Operand False = Operand.Create(false);

        public virtual bool IsError => false;
        public virtual string ErrorMsg => null;
        public abstract OperandType Type { get; }
        public virtual double NumberValue => throw new NotImplementedException();
        public virtual int IntValue => throw new NotImplementedException();
        public virtual string StringValue => throw new NotImplementedException();
        public virtual bool BooleanValue => throw new NotImplementedException();
        public virtual List<Operand> ArrayValue => throw new NotImplementedException();
        internal virtual JsonData JsonValue => throw new NotImplementedException();
        public virtual Date DateValue => throw new NotImplementedException();

        #region Create

        public static Operand Create(bool obj)
        {
            return new OperandBoolean(obj);
        }
        public static Operand Create(int obj)
        {
            return new OperandNumber(obj);
        }
        public static Operand Create(double obj)
        {
            return new OperandNumber(obj);
        }
        public static Operand Create(string obj)
        {
            return new OperandString(obj);
        }
        public static Operand Create(Date obj)
        {
            return new OperandDate(obj);
        }
        public static Operand Create(DateTime obj)
        {
            return new OperandDate(new Date(obj));
        }
        public static Operand Create(TimeSpan obj)
        {
            return new OperandDate(new Date(obj));
        }
        internal static Operand Create(JsonData obj)
        {
            return new OperandJson(obj);
        }
        public static Operand Create(List<Operand> obj)
        {
            return new OperandArray(obj);
        }
        public static Operand Create(ICollection<string> obj)
        {
            var array = new List<Operand>();
            foreach (var item in obj) {
                array.Add(Create(item));
            }
            return new OperandArray(array);
        }
        public static Operand Create(ICollection<double> obj)
        {
            var array = new List<Operand>();
            foreach (var item in obj) {
                array.Add(Create(item));
            }
            return new OperandArray(array);
        }
        public static Operand Create(ICollection<int> obj)
        {
            var array = new List<Operand>();
            foreach (var item in obj) {
                array.Add(Create(item));
            }
            return new OperandArray(array);
        }
        public static Operand Create(ICollection<bool> obj)
        {
            var array = new List<Operand>();
            foreach (var item in obj) {
                array.Add(Create(item));
            }
            return new OperandArray(array);
        }

        public static Operand Error(string msg)
        {
            return new OperandError(msg);
        }


        #endregion
        public Operand ToNumber(string errorMessage)
        {
            if (Type == OperandType.NUMBER) { return this; }
            if (IsError) { return this; }
            if (Type == OperandType.BOOLEAN) { return Create(BooleanValue ? 1.0 : 0.0); }
            if (Type == OperandType.DATE) { return Create((double)DateValue); }
            if (Type == OperandType.STRING) {
                if (double.TryParse(StringValue, NumberStyles.Any, cultureInfo, out double d)) {
                    return Create(d);
                }
            }
            return Error(errorMessage);
        }
        public Operand ToBoolean(string errorMessage)
        {
            if (Type == OperandType.BOOLEAN) { return this; }
            if (IsError) { return this; }
            if (Type == OperandType.NUMBER) { return Create(NumberValue != 0); }
            if (Type == OperandType.DATE) { return Create(((double)DateValue) != 0); }
            if (Type == OperandType.STRING) {
                if (StringValue.Equals("true", StringComparison.OrdinalIgnoreCase)) { return Create(true); }
                if (StringValue.Equals("false", StringComparison.OrdinalIgnoreCase)) { return Create(false); }
                if (StringValue.Equals("1", StringComparison.OrdinalIgnoreCase)) { return Create(true); }
                if (StringValue.Equals("0", StringComparison.OrdinalIgnoreCase)) { return Create(false); }
            }
            return Error(errorMessage);
        }
        public Operand ToString(string errorMessage)
        {
            if (Type == OperandType.STRING) { return this; }
            if (IsError) { return this; }
            if (Type == OperandType.NUMBER) { return Create(NumberValue.ToString(cultureInfo)); }
            if (Type == OperandType.BOOLEAN) { return Create(BooleanValue ? "TRUE" : "FALSE"); }
            if (Type == OperandType.DATE) { return Create(DateValue.ToString()); }
            if (Type == OperandType.JSON) { return Create(JsonValue.ToString()); }

            return Error(errorMessage);
        }
        public Operand ToDate(string errorMessage)
        {
            if (Type == OperandType.DATE) { return this; }
            if (IsError) { return this; }
            if (Type == OperandType.NUMBER) { return Create((Date)NumberValue); }
            if (Type == OperandType.STRING) {
                if (TimeSpan.TryParse(StringValue, cultureInfo, out TimeSpan t)) { return Create(new Date(t)); }
                if (DateTime.TryParse(StringValue, cultureInfo, DateTimeStyles.None, out DateTime d)) { return Create(new Date(d)); }
            }
            return Error(errorMessage);
        }
        public Operand ToJson(string errorMessage)
        {
            if (Type == OperandType.JSON) { return this; }
            if (IsError) { return this; }
            if (Type == OperandType.STRING) {
                try {
                    var json = JsonMapper.ToObject(StringValue);
                    return Create(json);
                } catch (Exception) { }
            }
            return Error(errorMessage);
        }
        public Operand ToArray(string errorMessage)
        {
            if (Type == OperandType.ARRARY) { return this; }
            if (IsError) { return this; }
            if (Type == OperandType.JSON) {
                if (JsonValue.IsArray) {
                    List<Operand> list = new List<Operand>();
                    foreach (JsonData v in JsonValue) {
                        if (v.IsString)
                            list.Add(Operand.Create(v.ToString()));
                        else if (v.IsBoolean)
                            list.Add(Operand.Create(bool.Parse(v.ToString())));
                        else if (v.IsDouble)
                            list.Add(Operand.Create(double.Parse(v.ToString(), NumberStyles.Any, cultureInfo)));
                        else if (v.IsInt)
                            list.Add(Operand.Create(double.Parse(v.ToString(), NumberStyles.Any, cultureInfo)));
                        else if (v.IsLong)
                            list.Add(Operand.Create(double.Parse(v.ToString(), NumberStyles.Any, cultureInfo)));
                        else
                            list.Add(Operand.Create(v));
                    }
                    return Create(list);
                }
            }
            return Error(errorMessage);
        }

        public void Dispose() { }
    }
    public abstract class Operand<T> : Operand
    {
        public T Value { get; private set; }
        public Operand(T obj)
        {
            Value = obj;
        }
    }

    public class OperandNumber : Operand<double>
    {
        public OperandNumber(double obj) : base(obj) { }
        public override OperandType Type => OperandType.NUMBER;
        public override int IntValue => (int)Value;
        public override double NumberValue => Value;
    }
    public class OperandBoolean : Operand<bool>
    {
        public OperandBoolean(bool obj) : base(obj) { }
        public override OperandType Type => OperandType.BOOLEAN;
        public override bool BooleanValue => Value;
    }
    public class OperandString : Operand<string>
    {
        public OperandString(string obj) : base(obj) { }
        public override OperandType Type => OperandType.STRING;
        public override string StringValue => Value;
    }
    public class OperandDate : Operand<Date>
    {
        public OperandDate(Date obj) : base(obj) { }
        public override OperandType Type => OperandType.DATE;
        public override Date DateValue => Value;
    }
    internal class OperandJson : Operand<JsonData>
    {
        public OperandJson(JsonData obj) : base(obj) { }
        public override OperandType Type => OperandType.JSON;
        internal override JsonData JsonValue => Value;
    }
    public class OperandArray : Operand<List<Operand>>
    {
        public OperandArray(List<Operand> obj) : base(obj) { }
        public override OperandType Type => OperandType.ARRARY;
        public override List<Operand> ArrayValue => Value;
    }
    public class OperandError : Operand
    {
        public override OperandType Type => OperandType.ERROR;
        public override bool IsError => true;
        private string _errorMsg;
        public override string ErrorMsg => _errorMsg;
        public OperandError(string msg)
        {
            _errorMsg = msg;
        }
    }

}
