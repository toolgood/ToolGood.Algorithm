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
        /// <summary>
        /// True 值
        /// </summary>
        public static readonly Operand True = Operand.Create(true);
        /// <summary>
        /// False 值
        /// </summary>
        public static readonly Operand False = Operand.Create(false);
        /// <summary>
        /// 是否为空
        /// </summary>
        public virtual bool IsNull => false;
        /// <summary>
        /// 是否出错
        /// </summary>
        public virtual bool IsError => false;
        /// <summary>
        /// 错误信息
        /// </summary>
        public virtual string ErrorMsg => null;
        /// <summary>
        /// 操作数类型
        /// </summary>
        public abstract OperandType Type { get; }
        /// <summary>
        /// 数字值
        /// </summary>
        public virtual double NumberValue => throw new NotImplementedException();
        /// <summary>
        /// int值
        /// </summary>
        public virtual int IntValue => throw new NotImplementedException();
        /// <summary>
        /// 字符串值
        /// </summary>
        public virtual string TextValue => throw new NotImplementedException();
        /// <summary>
        /// 布尔值
        /// </summary>
        public virtual bool BooleanValue => throw new NotImplementedException();
        /// <summary>
        /// 数组值
        /// </summary>
        public virtual List<Operand> ArrayValue => throw new NotImplementedException();
        internal virtual JsonData JsonValue => throw new NotImplementedException();
        /// <summary>
        /// 时间值
        /// </summary>
        public virtual Date DateValue => throw new NotImplementedException();

        #region Create

        /// <summary>
        /// 创建操作数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Operand Create(bool obj)
        {
            return new OperandBoolean(obj);
        }
        /// <summary>
        /// 创建操作数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Operand Create(int obj)
        {
            return new OperandNumber(obj);
        }
        /// <summary>
        /// 创建操作数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Operand Create(double obj)
        {
            return new OperandNumber(obj);
        }
        /// <summary>
        /// 创建操作数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Operand Create(string obj)
        {
            if (object.Equals(null, obj)) {
                return Operand.CreateNull();
            }
            return new OperandString(obj);
        }
        /// <summary>
        /// 创建操作数
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static Operand CreateJson(string txt)
        {
            if ((txt.StartsWith("{") && txt.EndsWith("}")) || (txt.StartsWith("[") && txt.EndsWith("]"))) {
                try {
                    var json = JsonMapper.ToObject(txt);
                    return Operand.Create(json);
                } catch (Exception) { }
            }
            return Operand.Error("string to json is error!");
        }
        /// <summary>
        /// 创建操作数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Operand Create(Date obj)
        {
            return new OperandDate(obj);
        }
        /// <summary>
        /// 创建操作数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Operand Create(DateTime obj)
        {
            return new OperandDate(new Date(obj));
        }
        /// <summary>
        /// 创建操作数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Operand Create(TimeSpan obj)
        {
            return new OperandDate(new Date(obj));
        }
        /// <summary>
        /// 创建操作数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static Operand Create(JsonData obj)
        {
            return new OperandJson(obj);
        }
        /// <summary>
        /// 创建操作数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Operand Create(List<Operand> obj)
        {
            return new OperandArray(obj);
        }
        /// <summary>
        /// 创建操作数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Operand Create(ICollection<string> obj)
        {
            var array = new List<Operand>();
            foreach (var item in obj) {
                array.Add(Create(item));
            }
            return new OperandArray(array);
        }
        /// <summary>
        /// 创建操作数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Operand Create(ICollection<double> obj)
        {
            var array = new List<Operand>();
            foreach (var item in obj) {
                array.Add(Create(item));
            }
            return new OperandArray(array);
        }
        /// <summary>
        /// 创建操作数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Operand Create(ICollection<int> obj)
        {
            var array = new List<Operand>();
            foreach (var item in obj) {
                array.Add(Create(item));
            }
            return new OperandArray(array);
        }
        /// <summary>
        /// 创建操作数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Operand Create(ICollection<bool> obj)
        {
            var array = new List<Operand>();
            foreach (var item in obj) {
                array.Add(Create(item));
            }
            return new OperandArray(array);
        }
        /// <summary>
        /// 创建操作数
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Operand Error(string msg)
        {
            return new OperandError(msg);
        }
        /// <summary>
        /// 创建操作数
        /// </summary>
        /// <returns></returns>
        public static Operand CreateNull()
        {
            return new OperandNull();
        }

        #endregion
        /// <summary>
        /// 转数值类型
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public Operand ToNumber(string errorMessage = null)
        {
            if (Type == OperandType.NUMBER) { return this; }
            if (IsError) { return this; }
            if (Type == OperandType.BOOLEAN) { return Create(BooleanValue ? 1.0 : 0.0); }
            if (Type == OperandType.DATE) { return Create((double)DateValue); }
            if (Type == OperandType.STRING) {
                if (double.TryParse(TextValue, NumberStyles.Any, cultureInfo, out double d)) {
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
        public Operand ToBoolean(string errorMessage = null)
        {
            if (Type == OperandType.BOOLEAN) { return this; }
            if (IsError) { return this; }
            if (Type == OperandType.NUMBER) { return Create(NumberValue != 0); }
            if (Type == OperandType.DATE) { return Create(((double)DateValue) != 0); }
            if (Type == OperandType.STRING) {
                if (TextValue.Equals("true", StringComparison.OrdinalIgnoreCase)) { return Create(true); }
                if (TextValue.Equals("false", StringComparison.OrdinalIgnoreCase)) { return Create(false); }
                if (TextValue.Equals("1", StringComparison.OrdinalIgnoreCase)) { return Create(true); }
                if (TextValue.Equals("0", StringComparison.OrdinalIgnoreCase)) { return Create(false); }
            }
            return Error(errorMessage);
        }
        /// <summary>
        /// 转String类型
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public Operand ToText(string errorMessage = null)
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
        public Operand ToDate(string errorMessage = null)
        {
            if (Type == OperandType.DATE) { return this; }
            if (IsError) { return this; }
            if (Type == OperandType.NUMBER) { return Create((Date)NumberValue); }
            if (Type == OperandType.STRING) {
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
        public Operand ToJson(string errorMessage = null)
        {
            if (Type == OperandType.JSON) { return this; }
            if (IsError) { return this; }
            if (Type == OperandType.STRING) {
                var txt = TextValue;
                if ((txt.StartsWith("{") && txt.EndsWith("}")) || (txt.StartsWith("[") && txt.EndsWith("]"))) {
                    try {
                        var json = JsonMapper.ToObject(txt);
                        return Operand.Create(json);
                    } catch (Exception) { }
                }
            }
            return Error(errorMessage);
        }
        /// <summary>
        /// 转Array类型
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public Operand ToArray(string errorMessage = null)
        {
            if (Type == OperandType.ARRARY) { return this; }
            if (IsError) { return this; }
            if (Type == OperandType.JSON) {
                if (JsonValue.IsArray) {
                    List<Operand> list = new List<Operand>();
                    foreach (JsonData v in JsonValue) {
                        if (v.IsString)
                            list.Add(Operand.Create(v.StringValue));
                        else if (v.IsBoolean)
                            list.Add(Operand.Create(v.BooleanValue));
                        else if (v.IsDouble)
                            list.Add(Operand.Create(v.NumberValue));
                        else if (v.IsNull)
                            list.Add(Operand.CreateNull());
                        else
                            list.Add(Operand.Create(v));
                    }
                    return Create(list);
                }
            }
            return Error(errorMessage);
        }

        void IDisposable.Dispose() { }


        #region Operand
        #region number
        public static implicit operator Operand(Int16 obj)
        {
            return Operand.Create((int)obj);
        }
        public static implicit operator Operand(Int32 obj)
        {
            return Operand.Create(obj);
        }
        public static implicit operator Operand(Int64 obj)
        {
            return Operand.Create((double)obj);
        }
        public static implicit operator Operand(UInt16 obj)
        {
            return Operand.Create((double)obj);
        }
        public static implicit operator Operand(UInt32 obj)
        {
            return Operand.Create((double)obj);
        }
        public static implicit operator Operand(UInt64 obj)
        {
            return Operand.Create((double)obj);
        }

        public static implicit operator Operand(float obj)
        {
            return Operand.Create((double)obj);
        }
        public static implicit operator Operand(double obj)
        {
            return Operand.Create(obj);
        }
        public static implicit operator Operand(decimal obj)
        {
            return Operand.Create((double)obj);
        }
        #endregion

        public static implicit operator Operand(bool obj)
        {
            return Operand.Create(obj);
        }
        public static implicit operator Operand(string obj)
        {
            return Operand.Create(obj);
        }
        public static implicit operator Operand(DateTime obj)
        {
            return Operand.Create(obj);
        }
        public static implicit operator Operand(TimeSpan obj)
        {
            return Operand.Create(obj);
        }
        public static implicit operator Operand(List<string> obj)
        {
            return Operand.Create(obj);
        }
        public static implicit operator Operand(List<bool> obj)
        {
            return Operand.Create(obj);
        }
        public static implicit operator Operand(List<int> obj)
        {
            return Operand.Create(obj);
        }
        public static implicit operator Operand(List<double> obj)
        {
            return Operand.Create(obj);
        }
        #endregion
    }
    abstract class Operand<T> : Operand
    {
        public T Value { get; private set; }
        public Operand(T obj)
        {
            Value = obj;
        }
    }

    class OperandNumber : Operand<double>
    {
        public OperandNumber(double obj) : base(obj) { }
        public override OperandType Type => OperandType.NUMBER;
        public override int IntValue => (int)Value;
        public override double NumberValue => Value;
    }
    class OperandBoolean : Operand<bool>
    {
        public OperandBoolean(bool obj) : base(obj) { }
        public override OperandType Type => OperandType.BOOLEAN;
        public override bool BooleanValue => Value;
    }
    class OperandString : Operand<string>
    {
        public OperandString(string obj) : base(obj) { }
        public override OperandType Type => OperandType.STRING;
        public override string TextValue => Value;
    }
    class OperandDate : Operand<Date>
    {
        public OperandDate(Date obj) : base(obj) { }
        public override OperandType Type => OperandType.DATE;
        public override Date DateValue => Value;
    }
    class OperandJson : Operand<JsonData>
    {
        public OperandJson(JsonData obj) : base(obj) { }
        public override OperandType Type => OperandType.JSON;
        internal override JsonData JsonValue => Value;
    }
    class OperandArray : Operand<List<Operand>>
    {
        public OperandArray(List<Operand> obj) : base(obj) { }
        public override OperandType Type => OperandType.ARRARY;
        public override List<Operand> ArrayValue => Value;
    }
    class OperandError : Operand
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

    class OperandNull : Operand
    {
        public override OperandType Type => OperandType.NULL;
        public override bool IsNull => true;

    }

}
