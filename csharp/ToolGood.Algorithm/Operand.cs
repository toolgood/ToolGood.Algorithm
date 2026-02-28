using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.LitJson;
using ToolGood.Algorithm.Operands;

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
		public static readonly Operand Version = new OperandString("ToolGood.Algorithm 6.2");

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
		/// <summary>
		/// Null
		/// </summary>
		public static readonly Operand Null = new OperandNull();

		// 整数缓存范围: -1000 ~ 1000，共2001个值
		private const int IntCacheOffset = 1000;
		private const int IntCacheSize = 2001;
		private static readonly Operand[] IntCache = new Operand[IntCacheSize];
		static Operand()
		{
			for (int i = 0; i < IntCacheSize; i++)
				IntCache[i] = new OperandInt(i - IntCacheOffset);
		}

		#region  IsNull IsNumber IsText IsBoolean IsArray IsDate IsJson IsArrayJson IsError ErrorMsg
		/// <summary>
		/// 是否为空值
		/// </summary>
		public virtual bool IsNull => false;
		/// <summary>
		/// 是否数字
		/// </summary>
		public virtual bool IsNumber => false;
		/// <summary>
		/// 是否字符串
		/// </summary>
		public virtual bool IsText => false;
		/// <summary>
		/// 是否布尔值
		/// </summary>
		public virtual bool IsBoolean => false;
		/// <summary>
		/// 是否数组
		/// </summary>
		public virtual bool IsArray => false;
		/// <summary>
		/// 是否日期
		/// </summary>
		public virtual bool IsDate => false;
		/// <summary>
		/// 是否Json对象
		/// </summary>
		public virtual bool IsJson => false;
		/// <summary>
		/// 是否Json数组
		/// </summary>
		public virtual bool IsArrayJson => false;
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
			if (obj >= -IntCacheOffset && obj <= IntCacheOffset)
				return IntCache[obj + IntCacheOffset];
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
			var array = new List<Operand>(obj.Count);
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
			var array = new List<Operand>(obj.Count);
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
			var array = new List<Operand>(obj.Count);
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
			var array = new List<Operand>(obj.Count);
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
		public static Operand CreateNull() => Null;

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

}