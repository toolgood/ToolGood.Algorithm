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
		internal static readonly CultureInfo cultureInfo = CultureInfo.GetCultureInfo("en-US");
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
		public static readonly Operand One = Operand.Create(1);
		/// <summary>
		/// Zero
		/// </summary>
		public static readonly Operand Zero = Operand.Create(0);

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
		/// 值
		/// </summary>
		public virtual object Value => throw new NotImplementedException();
		/// <summary>
		/// 数字值
		/// </summary>
		public virtual decimal NumberValue => throw new NotImplementedException();
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
			return new OperandNumber(obj);
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
		public static Operand Create(long obj)
		{
			return new OperandNumber((decimal)obj);
		}
		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(ushort obj)
		{
			return new OperandNumber((decimal)obj);
		}
		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(uint obj)
		{
			return new OperandNumber((decimal)obj);
		}
		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(ulong obj)
		{
			return new OperandNumber((decimal)obj);
		}
		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(float obj)
		{
			return new OperandNumber((decimal)obj);
		}
		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(double obj)
		{
			return new OperandNumber((decimal)obj);
		}
		/// <summary>
		/// 创建操作数
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static Operand Create(decimal obj)
		{
			return new OperandNumber((decimal)obj);
		}
		#endregion

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
		public virtual Operand ToNumber(string errorMessage = null) { return Error(errorMessage); }
		/// <summary>
		/// 转bool类型
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <returns></returns>
		public virtual Operand ToBoolean(string errorMessage = null) { return Error(errorMessage); }
		/// <summary>
		/// 转String类型
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <returns></returns>
		public virtual Operand ToText(string errorMessage = null) { return Error(errorMessage); }
		/// <summary>
		/// 转MyDate类型
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <returns></returns>
		public virtual Operand ToMyDate(string errorMessage = null) { return Error(errorMessage); }
		/// <summary>
		/// 转Json类型
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <returns></returns>
		public virtual Operand ToJson(string errorMessage = null) { return Error(errorMessage); }
		/// <summary>
		/// 转Array类型
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <returns></returns>
		public virtual Operand ToArray(string errorMessage = null) { return Error(errorMessage); }
		void IDisposable.Dispose() { }


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
			return Operand.Create((double)obj);
		}
		#endregion
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
		#endregion
	}
	abstract class Operand<T> : Operand
	{
		protected T _value { get; private set; }

		public override object Value => _value;
		public Operand(T obj)
		{
			_value = obj;
		}
	}

	sealed class OperandNumber : Operand<decimal>
	{
		public OperandNumber(decimal obj) : base(obj) { }
		public override OperandType Type => OperandType.NUMBER;
		public override int IntValue => (int)_value;
		public override decimal NumberValue => _value;
		public override long LongValue => (long)_value;

		public override Operand ToNumber(string errorMessage = null) { return this; }
		public override Operand ToBoolean(string errorMessage = null) { return NumberValue != 0 ? True : False; }
		public override Operand ToText(string errorMessage = null) { return Create(NumberValue.ToString(cultureInfo)); }
		public override Operand ToMyDate(string errorMessage = null) { return Create((MyDate)NumberValue); }

		public override Operand ToArray(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert number to array error!");
		}
		public override Operand ToJson(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert number to json error!");
		}
	}
	sealed class OperandBoolean : Operand<bool>
	{
		public OperandBoolean(bool obj) : base(obj) { }
		public override OperandType Type => OperandType.BOOLEAN;
		public override bool BooleanValue => _value;
		public override Operand ToNumber(string errorMessage = null) { return BooleanValue ? One : Zero; }
		public override Operand ToBoolean(string errorMessage = null) { return this; }
		public override Operand ToText(string errorMessage = null) { return Create(BooleanValue ? "TRUE" : "FALSE"); }

		public override Operand ToArray(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert bool to array error!");
		}
		public override Operand ToJson(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert bool to json error!");
		}
		public override Operand ToMyDate(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert bool to date error!");
		}
	}
	sealed class OperandString : Operand<string>
	{
		public OperandString(string obj) : base(obj) { }
		public override OperandType Type => OperandType.TEXT;
		public override string TextValue => _value;

		public override Operand ToNumber(string errorMessage = null)
		{
			if (decimal.TryParse(TextValue, out decimal d)) {
				return Operand.Create(d);
			}
			if (errorMessage == null) {
				return Error("Convert string to number error!");
			}
			return Error(errorMessage);
		}
		public override Operand ToText(string errorMessage = null)
		{
			return this;
		}
		public override Operand ToBoolean(string errorMessage = null)
		{
			if (TextValue.Equals("true", StringComparison.OrdinalIgnoreCase)) { return True; }
			if (TextValue.Equals("yes", StringComparison.OrdinalIgnoreCase)) { return True; }
			if (TextValue.Equals("false", StringComparison.OrdinalIgnoreCase)) { return False; }
			if (TextValue.Equals("no", StringComparison.OrdinalIgnoreCase)) { return False; }
			if (TextValue.Equals("1") || TextValue.Equals("是") || TextValue.Equals("有")) { return True; }
			if (TextValue.Equals("0") || TextValue.Equals("否") || TextValue.Equals("不是") || TextValue.Equals("无") || TextValue.Equals("没有")) { return False; }
			if (errorMessage == null) {
				return Error("Convert string to bool error!");
			}
			return Error(errorMessage);
		}
		public override Operand ToMyDate(string errorMessage = null)
		{
			if (TimeSpan.TryParse(TextValue, cultureInfo, out TimeSpan t)) { return Create(new MyDate(t)); }
			if (DateTime.TryParse(TextValue, cultureInfo, DateTimeStyles.None, out DateTime d)) { return Create(new MyDate(d)); }
			if (errorMessage == null) {
				return Error("Convert string to date error!");
			}
			return Error(errorMessage);
		}
		public override Operand ToJson(string errorMessage = null)
		{
			var txt = TextValue.Trim();
			if ((txt.StartsWith('{') && txt.EndsWith('}')) || (txt.StartsWith('[') && txt.EndsWith(']'))) {
				try {
					var json = JsonMapper.ToObject(txt);
					return Operand.Create(json);
				} catch (Exception) { }
			}
			if (errorMessage == null) {
				return Error("Convert string to json error!");
			}
			return Error(errorMessage);
		}

		public override Operand ToArray(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert string to array error!");
		}
	}
	sealed class OperandMyDate : Operand<MyDate>
	{
		public OperandMyDate(MyDate obj) : base(obj) { }
		public override OperandType Type => OperandType.DATE;
		public override MyDate DateValue => _value;
		public override Operand ToNumber(string errorMessage = null)
		{
			return Create((decimal)DateValue);
		}
		public override Operand ToBoolean(string errorMessage = null)
		{
			return ((decimal)DateValue) != 0 ? True : False;
		}
		public override Operand ToText(string errorMessage = null)
		{
			return Create(DateValue.ToString());
		}
		public override Operand ToMyDate(string errorMessage = null) { return this; }

		public override Operand ToArray(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert date to array error!");
		}
		public override Operand ToJson(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert date to json error!");
		}
	}
	sealed class OperandJson : Operand<JsonData>
	{
		public OperandJson(JsonData obj) : base(obj) { }
		public override OperandType Type => OperandType.JSON;
		internal override JsonData JsonValue => _value;

		public override Operand ToJson(string errorMessage = null) { return this; }
		public override Operand ToArray(string errorMessage = null)
		{
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
			return Error(errorMessage ?? "Convert json to array error!");
		}
		public override Operand ToBoolean(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert json to bool error!");
		}
		public override Operand ToMyDate(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert json to date error!");
		}
		public override Operand ToNumber(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert json to number error!");
		}
		public override Operand ToText(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert json to string error!");
		}
	}
	sealed class OperandArray : Operand<List<Operand>>
	{
		public OperandArray(List<Operand> obj) : base(obj) { }
		public override OperandType Type => OperandType.ARRARY;
		public override List<Operand> ArrayValue => _value;
		public override Operand ToArray(string errorMessage = null) { return this; }

	}
	sealed class OperandError : Operand
	{
		public override OperandType Type => OperandType.ERROR;
		public override bool IsError => true;
		private string _errorMsg;
		public override string ErrorMsg => _errorMsg;
		public OperandError(string msg)
		{
			_errorMsg = msg;
		}
		public override Operand ToNumber(string errorMessage = null) { return this; }
		public override Operand ToBoolean(string errorMessage = null) { return this; }
		public override Operand ToText(string errorMessage = null) { return this; }
		public override Operand ToArray(string errorMessage = null) { return this; }
		public override Operand ToJson(string errorMessage = null) { return this; }
		public override Operand ToMyDate(string errorMessage = null) { return this; }
	}

	sealed class OperandNull : Operand
	{
		public override OperandType Type => OperandType.NULL;
		public override bool IsNull => true;
		public override Operand ToArray(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert null to array error!");
		}
		public override Operand ToBoolean(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert null to bool error!");
		}
		public override Operand ToText(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert null to string error!");
		}
		public override Operand ToNumber(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert null to number error!");
		}
		public override Operand ToJson(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert null to json error!");
		}
		public override Operand ToMyDate(string errorMessage = null)
		{
			return Error(errorMessage ?? "Convert null to date error!");
		}

	}

}
