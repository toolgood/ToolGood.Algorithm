using System;
using System.Collections.Generic;
using System.Globalization;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.LitJson;
using System.Text;
using ToolGood.Algorithm.Internals.Functions;

#if WebAssembly
using System.Linq2;
#else
using System.Linq;
#endif

namespace ToolGood.Algorithm
{
	/// <summary>
	/// 操作数
	/// </summary>
	public abstract class Operand
	{
		/// <summary>
		/// 版本号
		/// </summary>
		public static readonly Operand Version = new OperandString("ToolGood.Algorithm 6.0");

		/// <summary>
		/// True
		/// </summary>
		public static readonly Operand True = new OperandBoolean(true);

		/// <summary>
		/// False
		/// </summary>
		public static readonly Operand False = new OperandBoolean(false);

		/// <summary>
		/// One
		/// </summary>
		public static readonly Operand One = Operand.Create(1m);

		/// <summary>
		/// Zero
		/// </summary>
		public static readonly Operand Zero = Operand.Create(0m);

		#region  IsNull IsNumber IsText IsBoolean IsArray IsDate IsJson IsArrayJson IsError ErrorMsg
		/// <summary>
		/// 是否为空值
		/// </summary>
		public virtual bool IsNull => false;
		/// <summary>
		/// 是否为非空值
		/// </summary>
		public virtual bool IsNotNull => true;

		/// <summary>
		/// 是否数字
		/// </summary>
		public virtual bool IsNumber => false;
		/// <summary>
		/// 是否非数字
		/// </summary>
		public virtual bool IsNotNumber => true;

		/// <summary>
		/// 是否字符串
		/// </summary>
		public virtual bool IsText => false;
		/// <summary>
		/// 是否非字符串
		/// </summary>
		public virtual bool IsNotText => true;

		/// <summary>
		/// 是否布尔值
		/// </summary>
		public virtual bool IsBoolean => false;
		/// <summary>
		/// 是否非布尔值
		/// </summary>
		public virtual bool IsNotBoolean => true;
		/// <summary>
		/// 是否数组
		/// </summary>
		public virtual bool IsArray => false;
		/// <summary>
		/// 是否非数组
		/// </summary>
		public virtual bool IsNotArray => true;
		/// <summary>
		/// 是否日期
		/// </summary>
		public virtual bool IsDate => false;
		/// <summary>
		/// 是否非日期
		/// </summary>
		public virtual bool IsNotDate => true;
		/// <summary>
		/// 是否Json对象
		/// </summary>
		public virtual bool IsJson => false;
		/// <summary>
		/// 是否非Json对象
		/// </summary>
		public virtual bool IsNotJson => true;
		/// <summary>
		/// 是否Json数组
		/// </summary>
		public virtual bool IsArrayJson => false;
		/// <summary>
		/// 是否非Json数组
		/// </summary>
		public virtual bool IsNotArrayJson => true;

		/// <summary>
		/// 是否出错
		/// </summary>
		public virtual bool IsError => false;

		/// <summary>
		/// 错误信息
		/// </summary>
		public virtual string ErrorMsg => null;
		#endregion

		/// <summary>
		/// 操作数类型
		/// </summary>
		public abstract OperandType Type { get; }

		#region Value

		/// <summary>
		/// 数字值
		/// </summary>
		public virtual decimal NumberValue => throw new NotImplementedException();

		/// <summary>
		/// double值
		/// </summary>
		public virtual double DoubleValue => throw new NotImplementedException();

		/// <summary>
		/// int值
		/// </summary>
		public virtual int IntValue => throw new NotImplementedException();

		/// <summary>
		/// long值
		/// </summary>
		public virtual long LongValue => throw new NotImplementedException();

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
		public virtual MyDate DateValue => throw new NotImplementedException();

		#endregion

		#region Create

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(bool obj)
		{
			return obj ? True : False;
		}

		#region number

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(short obj)
		{
			return new OperandInt(obj);
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(int obj)
		{
			return new OperandInt(obj);
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(long obj)
		{
			return new OperandDecimal((decimal)obj);
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(ushort obj)
		{
			return new OperandDecimal((decimal)obj);
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(uint obj)
		{
			return new OperandDecimal((decimal)obj);
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(ulong obj)
		{
			return new OperandDecimal((decimal)obj);
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(float obj)
		{
			return new OperandDouble((double)obj);
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(double obj)
		{
			return new OperandDouble((double)obj);
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(decimal obj)
		{
			return new OperandDecimal((decimal)obj);
		}

		#endregion number

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(string obj)
		{
			if(object.Equals(null, obj)) {
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
			if((txt.StartsWith('{') && txt.EndsWith('}')) || (txt.StartsWith('[') && txt.EndsWith(']'))) {
				try {
					var json = JsonMapper.ToObject(txt);
					return Operand.Create(json);
				} catch(Exception) { }
			}
			return Operand.Error("Convert to json error!");
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(MyDate obj)
		{
			return new OperandMyDate(obj);
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(DateTime obj)
		{
			return new OperandMyDate(new MyDate(obj));
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(TimeSpan obj)
		{
			return new OperandMyDate(new MyDate(obj));
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
			foreach(var item in obj) {
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
			foreach(var item in obj) {
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
			foreach(var item in obj) {
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
			foreach(var item in obj) {
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
		/// <param name="msg"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static Operand Error(string msg, params object[] args)
		{
			return new OperandError(string.Format(msg, args));
		}

		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <returns></returns>
		public static Operand CreateNull()
		{
			return new OperandNull();
		}

		#endregion Create

		#region 转化类型

		/// <summary>
		/// 转数值类型
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <returns></returns>
		public virtual Operand ToNumber(string errorMessage = null) { return Error(errorMessage ?? "Convert to number error!"); }
		/// <summary>
		/// 转数值类型
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public virtual Operand ToNumber(string errorMessage, params object[] args) { return Error(string.Format(errorMessage, args)); }

		/// <summary>
		/// 转bool类型
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <returns></returns>
		public virtual Operand ToBoolean(string errorMessage = null) { return Error(errorMessage ?? "Convert to bool error!"); }
		/// <summary>
		/// 转bool类型
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public virtual Operand ToBoolean(string errorMessage, params object[] args) { return Error(string.Format(errorMessage, args)); }
		/// <summary>
		/// 转string类型
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <returns></returns>
		public virtual Operand ToText(string errorMessage = null) { return Error(errorMessage ?? "Convert to string error!"); }
		/// <summary>
		/// 转string类型
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public virtual Operand ToText(string errorMessage, params object[] args) { return Error(string.Format(errorMessage, args)); }

		/// <summary>
		/// 转MyDate类型
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <returns></returns>
		public virtual Operand ToMyDate(string errorMessage = null) { return Error(errorMessage ?? "Convert to date error!"); }
		/// <summary>
		/// 转MyDate类型
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public virtual Operand ToMyDate(string errorMessage, params object[] args) { return Error(string.Format(errorMessage, args)); }

		/// <summary>
		/// 转Array类型
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <returns></returns>
		public virtual Operand ToArray(string errorMessage = null) { return Error(errorMessage ?? "Convert to array error!"); }

		/// <summary>
		/// 转Array类型
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public virtual Operand ToArray(string errorMessage, params object[] args) { return Error(string.Format(errorMessage, args)); }

		/// <summary>
		/// 转Json类型
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <returns></returns>
		public virtual Operand ToJson(string errorMessage = null) { return Error(errorMessage ?? "Convert to json error!"); }

		#endregion

		#region Operand

		#region number

		/// <summary>
		///
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(Int16 obj)
		{
			return Operand.Create((int)obj);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(Int32 obj)
		{
			return Operand.Create(obj);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(Int64 obj)
		{
			return Operand.Create((double)obj);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(UInt16 obj)
		{
			return Operand.Create((double)obj);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(UInt32 obj)
		{
			return Operand.Create((double)obj);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(UInt64 obj)
		{
			return Operand.Create((double)obj);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(float obj)
		{
			return Operand.Create((double)obj);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(double obj)
		{
			return Operand.Create(obj);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(decimal obj)
		{
			return Operand.Create(obj);
		}

		#endregion number

		/// <summary>
		///
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(bool obj)
		{
			return Operand.Create(obj);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(string obj)
		{
			return Operand.Create(obj);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(DateTime obj)
		{
			return Operand.Create(obj);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(TimeSpan obj)
		{
			return Operand.Create(obj);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(List<string> obj)
		{
			return Operand.Create(obj);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(List<bool> obj)
		{
			return Operand.Create(obj);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(List<int> obj)
		{
			return Operand.Create(obj);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="obj"></param>
		public static implicit operator Operand(List<double> obj)
		{
			return Operand.Create(obj);
		}

		#endregion Operand

	}


	#region Number

	internal sealed class OperandInt : Operand
	{
		private readonly int _value;
		public OperandInt(int obj)
		{
			_value = obj;
		}
		public override bool IsNumber => true;
		public override bool IsNotNumber => false;
		public override OperandType Type => OperandType.NUMBER;
		public override int IntValue => (int)_value;
		public override decimal NumberValue => _value;
		public override long LongValue => (long)_value;
		public override double DoubleValue => (double)_value;

		public override Operand ToNumber(string errorMessage) { return this; }
		public override Operand ToNumber(string errorMessage, params object[] args) { return this; }

		public override Operand ToBoolean(string errorMessage) { return IntValue != 0 ? True : False; }
		public override Operand ToBoolean(string errorMessage, params object[] args) { return IntValue != 0 ? True : False; }

		public override Operand ToText(string errorMessage) { return Create(IntValue.ToString(CultureInfo.InvariantCulture)); }
		public override Operand ToText(string errorMessage, params object[] args) { return Create(IntValue.ToString(CultureInfo.InvariantCulture)); }

		public override Operand ToMyDate(string errorMessage) { return Create((MyDate)NumberValue); }
		public override Operand ToMyDate(string errorMessage, params object[] args) { return Create((MyDate)NumberValue); }

		public override string ToString() { return NumberValue.ToString(CultureInfo.InvariantCulture); }
	}

	internal sealed class OperandDouble : Operand
	{
		private readonly double _value;
		public OperandDouble(double obj)
		{
			_value = obj;
		}
		public override bool IsNumber => true;
		public override bool IsNotNumber => false;
		public override OperandType Type => OperandType.NUMBER;
		public override int IntValue => (int)_value;
		public override decimal NumberValue => (decimal)_value;
		public override long LongValue => (long)_value;
		public override double DoubleValue => (double)_value;

		public override Operand ToNumber(string errorMessage) { return this; }
		public override Operand ToNumber(string errorMessage, params object[] args) { return this; }

		public override Operand ToBoolean(string errorMessage) { return DoubleValue != 0 ? True : False; }
		public override Operand ToBoolean(string errorMessage, params object[] args) { return DoubleValue != 0 ? True : False; }

		public override Operand ToText(string errorMessage) { return Create(DoubleValue.ToString(CultureInfo.InvariantCulture)); }
		public override Operand ToText(string errorMessage, params object[] args) { return Create(DoubleValue.ToString(CultureInfo.InvariantCulture)); }

		public override Operand ToMyDate(string errorMessage) { return Create((MyDate)NumberValue); }
		public override Operand ToMyDate(string errorMessage, params object[] args) { return Create((MyDate)NumberValue); }

		public override string ToString() { return NumberValue.ToString(CultureInfo.InvariantCulture); }
	}

	internal sealed class OperandDecimal : Operand
	{
		private readonly decimal _value;
		public OperandDecimal(decimal obj)
		{
			_value = obj;
		}
		public override bool IsNumber => true;
		public override bool IsNotNumber => false;
		public override OperandType Type => OperandType.NUMBER;
		public override int IntValue => (int)_value;
		public override decimal NumberValue => _value;
		public override long LongValue => (long)_value;
		public override double DoubleValue => (double)_value;

		public override Operand ToNumber(string errorMessage) { return this; }
		public override Operand ToNumber(string errorMessage, params object[] args) { return this; }

		public override Operand ToBoolean(string errorMessage) { return NumberValue != 0 ? True : False; }
		public override Operand ToBoolean(string errorMessage, params object[] args) { return NumberValue != 0 ? True : False; }

		public override Operand ToText(string errorMessage) { return Create(NumberValue.ToString(CultureInfo.InvariantCulture)); }
		public override Operand ToText(string errorMessage, params object[] args) { return Create(NumberValue.ToString(CultureInfo.InvariantCulture)); }

		public override Operand ToMyDate(string errorMessage) { return Create((MyDate)NumberValue); }
		public override Operand ToMyDate(string errorMessage, params object[] args) { return Create((MyDate)NumberValue); }

		public override string ToString() { return NumberValue.ToString(CultureInfo.InvariantCulture); }
	}

	#endregion

	internal sealed class OperandBoolean : Operand
	{
		private readonly bool _value;
		public OperandBoolean(bool obj)
		{
			_value = obj;
		}
		public override bool IsBoolean => true;
		public override bool IsNotBoolean => false;
		public override OperandType Type => OperandType.BOOLEAN;
		public override bool BooleanValue => _value;

		public override Operand ToNumber(string errorMessage) { return _value ? One : Zero; }
		public override Operand ToNumber(string errorMessage, params object[] args) { return _value ? One : Zero; }

		public override Operand ToBoolean(string errorMessage) { return this; }
		public override Operand ToBoolean(string errorMessage, params object[] args) { return this; }

		public override Operand ToText(string errorMessage) { return Create(_value ? "TRUE" : "FALSE"); }
		public override Operand ToText(string errorMessage, params object[] args) { return Create(_value ? "TRUE" : "FALSE"); }

		public override string ToString() { return _value ? "true" : "false"; }
	}

	internal sealed class OperandString : Operand
	{
		private readonly string _value;
		public OperandString(string obj)
		{
			_value = obj;
		}

		public override bool IsText => true;
		public override bool IsNotText => false;
		public override OperandType Type => OperandType.TEXT;
		public override string TextValue => _value;

		public override Operand ToNumber(string errorMessage)
		{
			if(TextValue.Contains('.') == false && int.TryParse(TextValue, out int num)) {
				return Operand.Create(num);
			}
			if(decimal.TryParse(TextValue, out decimal d)) {
				return Operand.Create(d);
			}
			if(errorMessage == null) {
				return Error("Convert to number error!");
			}
			return Error(errorMessage);
		}
		public override Operand ToNumber(string errorMessage, params object[] args)
		{
			if(TextValue.Contains('.') == false && int.TryParse(TextValue, out int num)) {
				return Operand.Create(num);
			}
			if(decimal.TryParse(TextValue, out decimal d)) {
				return Operand.Create(d);
			}
			if(errorMessage == null) {
				return Error("Convert to number error!");
			}
			return Error(string.Format(errorMessage, args));
		}


		public override Operand ToText(string errorMessage) { return this; }
		public override Operand ToText(string errorMessage, params object[] args) { return this; }

		public override Operand ToBoolean(string errorMessage)
		{
			if(FunctionUtil.TryParseBoolean(TextValue, out bool b)) {
				return b ? Operand.True : Operand.False;
			}
			if(errorMessage == null) {
				return Error("Convert to bool error!");
			}
			return Error(errorMessage);
		}
		public override Operand ToBoolean(string errorMessage, params object[] args)
		{
			if(FunctionUtil.TryParseBoolean(TextValue, out bool b)) {
				return b ? Operand.True : Operand.False;
			}
			if(errorMessage == null) {
				return Error("Convert to bool error!");
			}
			return Error(string.Format(errorMessage, args));
		}


		public override Operand ToMyDate(string errorMessage)
		{
			if(TimeSpan.TryParse(TextValue, CultureInfo.InvariantCulture, out TimeSpan t)) { return Create(new MyDate(t)); }
			if(DateTime.TryParse(TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime d)) { return Create(new MyDate(d)); }
			if(errorMessage == null) {
				return Error("Convert to date error!");
			}
			return Error(errorMessage);
		}
		public override Operand ToMyDate(string errorMessage, params object[] args)
		{
			if(TimeSpan.TryParse(TextValue, CultureInfo.InvariantCulture, out TimeSpan t)) { return Create(new MyDate(t)); }
			if(DateTime.TryParse(TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime d)) { return Create(new MyDate(d)); }
			if(errorMessage == null) {
				return Error("Convert to date error!");
			}
			return Error(string.Format(errorMessage, args));
		}

		public override Operand ToArray(string errorMessage)
		{
			return Error(errorMessage ?? "Convert to array error!");
		}
		public override Operand ToJson(string errorMessage = null)
		{
			var txt = this.TextValue.Trim();
			if((txt.StartsWith('{') && txt.EndsWith('}')) || (txt.StartsWith('[') && txt.EndsWith(']'))) {
				try {
					var json = JsonMapper.ToObject(txt);
					return Operand.Create(json);
				} catch(Exception) { }
			}
			return Error(errorMessage ?? "Convert to json error!");
		}


		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append('"');
			foreach(char c in _value) {
				switch(c) {
					case '"': sb.Append("\\\""); break;
					case '\n': sb.Append("\\n"); break;
					case '\r': sb.Append("\\r"); break;
					case '\t': sb.Append("\\t"); break;
					case '\0': sb.Append("\\0"); break;
					case '\v': sb.Append("\\v"); break;
					case '\a': sb.Append("\\a"); break;
					case '\b': sb.Append("\\b"); break;
					case '\f': sb.Append("\\f"); break;
					default: sb.Append(c); break;
				}
			}
			sb.Append('"');
			return sb.ToString();
		}
	}

	internal sealed class OperandMyDate : Operand
	{
		private readonly MyDate _value;
		public OperandMyDate(MyDate obj)
		{
			_value = obj;
		}
		public override bool IsDate => true;
		public override bool IsNotDate => false;
		public override OperandType Type => OperandType.DATE;
		public override MyDate DateValue => _value;

		public override Operand ToNumber(string errorMessage) { return Create((decimal)DateValue); }
		public override Operand ToNumber(string errorMessage, params object[] args) { return Create((decimal)DateValue); }

		public override Operand ToBoolean(string errorMessage) { return ((decimal)DateValue) != 0 ? True : False; }
		public override Operand ToBoolean(string errorMessage, params object[] args) { return ((decimal)DateValue) != 0 ? True : False; }

		public override Operand ToText(string errorMessage) { return Create(DateValue.ToString()); }
		public override Operand ToText(string errorMessage, params object[] args) { return Create(DateValue.ToString()); }

		public override Operand ToMyDate(string errorMessage) { return this; }
		public override Operand ToMyDate(string errorMessage, params object[] args) { return this; }

		public override string ToString() { return "\"" + DateValue.ToString() + "\""; }
	}

	internal sealed class OperandJson : Operand
	{
		private readonly JsonData _value;
		public OperandJson(JsonData obj)
		{
			_value = obj;
		}
		public override bool IsJson => true;
		public override bool IsNotJson => false;
		public override OperandType Type => OperandType.JSON;
		internal override JsonData JsonValue => _value;

		public override Operand ToText(string errorMessage = null)
		{
			return Create(_value.ToString());
		}
		public override Operand ToText(string errorMessage, params object[] args)
		{
			return Create(_value.ToString());
		}

		public override Operand ToArray(string errorMessage)
		{
			if(JsonValue.IsArray) {
				var list = new List<Operand>();
				foreach(JsonData v in JsonValue) {
					if(v.IsString)
						list.Add(Operand.Create(v.StringValue));
					else if(v.IsBoolean)
						list.Add(Operand.Create(v.BooleanValue));
					else if(v.IsDouble)
						list.Add(Operand.Create(v.NumberValue));
					else if(v.IsNull)
						list.Add(Operand.CreateNull());
					else
						list.Add(Operand.Create(v));
				}
				return Create(list);
			}
			return Error(errorMessage ?? "Convert to array error!");
		}
		public override Operand ToArray(string errorMessage, params object[] args)
		{
			if(JsonValue.IsArray) {
				var list = new List<Operand>();
				foreach(JsonData v in JsonValue) {
					if(v.IsString)
						list.Add(Operand.Create(v.StringValue));
					else if(v.IsBoolean)
						list.Add(Operand.Create(v.BooleanValue));
					else if(v.IsDouble)
						list.Add(Operand.Create(v.NumberValue));
					else if(v.IsNull)
						list.Add(Operand.CreateNull());
					else
						list.Add(Operand.Create(v));
				}
				return Create(list);
			}
			return Error(string.Format(errorMessage, args));
		}
		public override Operand ToJson(string errorMessage = null)
		{
			return this;
		}
		public override string ToString()
		{
			return _value.ToString();
		}
	}

	internal sealed class OperandArray : Operand
	{
		private readonly List<Operand> _value;
		public OperandArray(List<Operand> obj)
		{
			_value = obj;
		}
		public override bool IsArray => true;
		public override bool IsNotArray => false;
		public override OperandType Type => OperandType.ARRARY;
		public override List<Operand> ArrayValue => _value;

		public override Operand ToText(string errorMessage = null)
		{
			return Create(this.ToString());
		}
		public override Operand ToText(string errorMessage, params object[] args)
		{
			return Create(this.ToString());
		}

		public override Operand ToArray(string errorMessage) { return this; }
		public override Operand ToArray(string errorMessage, params object[] args) { return this; }

		public override Operand ToJson(string errorMessage = null)
		{
			var txt = this.ToString();
			try {
				var json = JsonMapper.ToObject(txt);
				return Operand.Create(json);
			} catch(Exception) { }
			return Error(errorMessage ?? "Convert to json error!");
		}

		public override string ToString()
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.Append('[');
			for(int i = 0; i < ArrayValue.Count; i++) {
				if(i > 0) stringBuilder.Append(',');
				stringBuilder.Append(ArrayValue[i].ToString());
			}
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}
	}

	internal sealed class OperandError : Operand
	{
		public override OperandType Type => OperandType.ERROR;
		public override bool IsError => true;
		private readonly string _errorMsg;
		public override string ErrorMsg => _errorMsg;

		public OperandError(string msg)
		{
			_errorMsg = msg;
		}

		public override Operand ToNumber(string errorMessage) { return this; }
		public override Operand ToNumber(string errorMessage, params object[] args) { return this; }

		public override Operand ToBoolean(string errorMessage) { return this; }
		public override Operand ToBoolean(string errorMessage, params object[] args) { return this; }

		public override Operand ToText(string errorMessage) { return this; }
		public override Operand ToText(string errorMessage, params object[] args) { return this; }

		public override Operand ToArray(string errorMessage) { return this; }
		public override Operand ToArray(string errorMessage, params object[] args) { return this; }

		public override Operand ToMyDate(string errorMessage) { return this; }
		public override Operand ToMyDate(string errorMessage, params object[] args) { return this; }
	}

	internal sealed class OperandNull : Operand
	{
		public override bool IsNull => true;
		public override bool IsNotNull => false;
		public override OperandType Type => OperandType.NULL;
		public override string ToString() { return "null"; }
	}

	internal class KeyValue
	{
		public string Key;
		public Operand Value;
	}

	internal sealed class OperandKeyValueList : Operand
	{
		private readonly List<KeyValue> TextList;
		public OperandKeyValueList()
		{
			TextList = new List<KeyValue>();
		}

		public override bool IsArrayJson => true;
		public override bool IsNotArrayJson => false;
		public override OperandType Type => OperandType.ARRARYJSON;
		public override List<Operand> ArrayValue => TextList.Select(q => q.Value).ToList();

		public override Operand ToText(string errorMessage = null)
		{
			return Create(this.ToString());
		}
		public override Operand ToText(string errorMessage, params object[] args)
		{
			return Create(this.ToString());
		}

		public override Operand ToArray(string errorMessage)
		{
			return Create(this.ArrayValue);
		}
		public override Operand ToArray(string errorMessage, params object[] args)
		{
			return Create(this.ArrayValue);
		}

		public override Operand ToJson(string errorMessage = null)
		{
			var txt = this.ToString();
			try {
				var json = JsonMapper.ToObject(txt);
				return Operand.Create(json);
			} catch(Exception) { }
			return Error(errorMessage ?? "Convert to json error!");
		}

		public void AddValue(KeyValue keyValue)
		{
			TextList.Add(keyValue);
		}

		public bool TryGetValue(string key, out Operand value)
		{
			foreach(var item in TextList) {
				if(item.Key == key.ToString()) {
					value = item.Value;
					return true;
				}
			}
			value = null;
			return false;
		}

		public bool ContainsKey(Operand value)
		{
			foreach(var item in TextList) {
				if(value.TextValue == item.Key) {
					return true;
				}
			}
			return false;
		}

		public bool ContainsValue(Operand value)
		{
			foreach(var item in TextList) {
				var op = item.Value;
				if(value.Type != op.Type) { continue; }
				if(value.IsText) {
					if(value.TextValue == op.TextValue) {
						return true;
					}
				}
			}
			return false;
		}

		public override string ToString()
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.Append('{');
			for(int i = 0; i < TextList.Count; i++) {
				if(i > 0) stringBuilder.Append(',');
				stringBuilder.Append('"');
				stringBuilder.Append(TextList[i].Key);
				stringBuilder.Append('"');
				stringBuilder.Append(':');
				stringBuilder.Append(TextList[i].Value.ToString());
			}
			stringBuilder.Append('}');
			return stringBuilder.ToString();
		}
	}

	internal sealed class OperandKeyValue : Operand
	{
		private readonly KeyValue _value;
		public OperandKeyValue(KeyValue obj)
		{
			_value = obj;
		}
		public override bool IsArrayJson => true;
		public override bool IsNotArrayJson => false;
		public override OperandType Type => OperandType.ARRARYJSON;
		public KeyValue Value => _value;
	}
}