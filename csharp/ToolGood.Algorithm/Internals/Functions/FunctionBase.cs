using System;
using System.Globalization;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions
{
	/// <summary>
	/// Represents the base class for all function implementations that can be calculated by an algorithm engine.
	/// </summary>
	/// <remarks>This abstract class defines a contract for functions that can be evaluated within the context
	/// of an algorithm engine. Derived classes must implement the <see cref="Evaluate"/> method to provide specific
	/// function logic.</remarks>
	public abstract class FunctionBase
	{
		/// <summary>
		/// 进行计算
		/// </summary>
		/// <param name="work"></param>
		/// <param name="tempParameter">临时参数，未找到返回null</param>
		/// <returns></returns>
		public abstract Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter = null);

		#region TryEvaluate

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="tempParameter"></param>
		/// <returns></returns>
		public ushort TryEvaluate(AlgorithmEngine work, ushort def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			try {
				var obj = this.Evaluate(work, tempParameter);
				if(obj.IsNotNumber) {
					obj = obj.ToNumber("It can't be converted to number!");
					if(obj.IsError) {
						work.LastError = obj.ErrorMsg;
						return def;
					}
				}
				return (ushort)obj.IntValue;
			} catch(Exception ex) {
				work.LastError = ex.Message + "\r\n" + ex.StackTrace;
			}
			return def;
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="tempParameter"></param>
		/// <returns></returns>
		public uint TryEvaluate(AlgorithmEngine work, uint def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			try {
				var obj = this.Evaluate(work, tempParameter);
				if(obj.IsNotNumber) {
					obj = obj.ToNumber("It can't be converted to number!");
					if(obj.IsError) {
						work.LastError = obj.ErrorMsg;
						return def;
					}
				}
				return (uint)obj.IntValue;
			} catch(Exception ex) {
				work.LastError = ex.Message + "\r\n" + ex.StackTrace;
			}
			return def;
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="tempParameter"></param>
		/// <returns></returns>
		public ulong TryEvaluate(AlgorithmEngine work, ulong def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			try {
				var obj = this.Evaluate(work, tempParameter);
				if(obj.IsNotNumber) {
					obj = obj.ToNumber("It can't be converted to number!");
					if(obj.IsError) {
						work.LastError = obj.ErrorMsg;
						return def;
					}
				}
				return (ulong)obj.IntValue;
			} catch(Exception ex) {
				work.LastError = ex.Message + "\r\n" + ex.StackTrace;
			}
			return def;
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="tempParameter"></param>
		/// <returns></returns>
		public short TryEvaluate(AlgorithmEngine work, short def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			try {
				var obj = this.Evaluate(work, tempParameter);
				if(obj.IsNotNumber) {
					obj = obj.ToNumber("It can't be converted to number!");
					if(obj.IsError) {
						work.LastError = obj.ErrorMsg;
						return def;
					}
				}
				return (short)obj.IntValue;
			} catch(Exception ex) {
				work.LastError = ex.Message + "\r\n" + ex.StackTrace;
			}
			return def;
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="tempParameter"></param>
		/// <returns></returns>
		public int TryEvaluate(AlgorithmEngine work, int def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			try {
				var obj = this.Evaluate(work, tempParameter);
				if(obj.IsNotNumber) {
					obj = obj.ToNumber("It can't be converted to number!");
					if(obj.IsError) {
						work.LastError = obj.ErrorMsg;
						return def;
					}
				}
				return obj.IntValue;
			} catch(Exception ex) {
				work.LastError = ex.Message + "\r\n" + ex.StackTrace;
			}
			return def;
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="tempParameter"></param>
		/// <returns></returns>
		public long TryEvaluate(AlgorithmEngine work, long def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			try {
				var obj = this.Evaluate(work, tempParameter);
				if(obj.IsNotNumber) {
					obj = obj.ToNumber("It can't be converted to number!");
					if(obj.IsError) {
						work.LastError = obj.ErrorMsg;
						return def;
					}
				}
				return obj.LongValue;
			} catch(Exception ex) {
				work.LastError = ex.Message + "\r\n" + ex.StackTrace;
			}
			return def;
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="tempParameter"></param>
		/// <returns></returns>
		public float TryEvaluate(AlgorithmEngine work, float def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			try {
				var obj = this.Evaluate(work, tempParameter);
				if(obj.IsNotNumber) {
					obj = obj.ToNumber("It can't be converted to number!");
					if(obj.IsError) {
						work.LastError = obj.ErrorMsg;
						return def;
					}
				}
				return (float)obj.DoubleValue;
			} catch(Exception ex) {
				work.LastError = ex.Message + "\r\n" + ex.StackTrace;
			}
			return def;
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="tempParameter"></param>
		/// <returns></returns>
		public double TryEvaluate(AlgorithmEngine work, double def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			try {
				var obj = this.Evaluate(work, tempParameter);
				if(obj.IsNotNumber) {
					obj = obj.ToNumber("It can't be converted to number!");
					if(obj.IsError) {
						work.LastError = obj.ErrorMsg;
						return def;
					}
				}
				return (double)obj.DoubleValue;
			} catch(Exception ex) {
				work.LastError = ex.Message + "\r\n" + ex.StackTrace;
			}
			return def;
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="tempParameter"></param>
		/// <returns></returns>
		public decimal TryEvaluate(AlgorithmEngine work, decimal def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			try {
				var obj = this.Evaluate(work, tempParameter);
				if(obj.IsNotNumber) {
					obj = obj.ToNumber("It can't be converted to number!");
					if(obj.IsError) {
						work.LastError = obj.ErrorMsg;
						return def;
					}
				}
				return (decimal)obj.NumberValue;
			} catch(Exception ex) {
				work.LastError = ex.Message + "\r\n" + ex.StackTrace;
			}
			return def;
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="tempParameter"></param>
		/// <returns></returns>
		public string TryEvaluate(AlgorithmEngine work, string def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			try {
				var obj = this.Evaluate(work, tempParameter);
				if(obj.IsNotText) {
					obj = obj.ToText("It can't be converted to string!");
					if(obj.IsError) {
						work.LastError = obj.ErrorMsg;
						return def;
					}
				}
				return obj.TextValue;
			} catch(Exception ex) {
				work.LastError = ex.Message + "\r\n" + ex.StackTrace;
			}
			return def;
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="tempParameter"></param>
		/// <returns></returns>
		public bool TryEvaluate(AlgorithmEngine work, bool def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			try {
				var obj = this.Evaluate(work, tempParameter);
				if(obj.IsNotBoolean) {
					obj = obj.ToBoolean("It can't be converted to bool!");
					if(obj.IsError) {
						work.LastError = obj.ErrorMsg;
						return def;
					}
				}
				return obj.BooleanValue;
			} catch(Exception ex) {
				work.LastError = ex.Message + "\r\n" + ex.StackTrace;
			}
			return def;
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="tempParameter"></param>
		/// <returns></returns>
		public DateTime TryEvaluate(AlgorithmEngine work, DateTime def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			try {
				var obj = this.Evaluate(work, tempParameter);
				if(obj.IsNotDate) {
					obj = obj.ToMyDate("It can't be converted to DateTime!");
					if(obj.IsError) {
						work.LastError = obj.ErrorMsg;
						return def;
					}
				}
				if(work.UseLocalTime) {
					return obj.DateValue.ToDateTime(DateTimeKind.Local);
				}
				return obj.DateValue.ToDateTime(DateTimeKind.Utc);
			} catch(Exception ex) {
				work.LastError = ex.Message + "\r\n" + ex.StackTrace;
			}
			return def;
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值
		/// </summary>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="tempParameter"></param>
		/// <returns></returns>
		public TimeSpan TryEvaluate(AlgorithmEngine work, TimeSpan def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			try {
				var obj = this.Evaluate(work, tempParameter);
				if(obj.IsNotDate) {
					obj = obj.ToMyDate("It can't be converted to DateTime!");
					if(obj.IsError) {
						work.LastError = obj.ErrorMsg;
						return def;
					}
				}
				return (TimeSpan)obj.DateValue;
			} catch(Exception ex) {
				work.LastError = ex.Message + "\r\n" + ex.StackTrace;
			}
			return def;
		}

		/// <summary>
		/// 执行函数,如果异常，返回默认值。
		/// 解决 def 为 null 二义性问题
		/// </summary>
		/// <param name="work"></param>
		/// <param name="def"></param>
		/// <param name="tempParameter"></param>
		/// <returns></returns>
		public MyDate TryEvaluate_MyDate(AlgorithmEngine work, MyDate def, Func<AlgorithmEngine, string, Operand> tempParameter = null)
		{
			try {
				var obj = this.Evaluate(work, tempParameter);
				if(obj.IsNotDate) {
					obj = obj.ToMyDate("It can't be converted to DateTime!");
					if(obj.IsError) {
						work.LastError = obj.ErrorMsg;
						return def;
					}
				}
				return obj.DateValue;
			} catch(Exception ex) {
				work.LastError = ex.Message + "\r\n" + ex.StackTrace;
			}
			return def;
		}

		#endregion TryEvaluate
		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			var stringBuilder = new StringBuilder();
			ToString(stringBuilder, false);
			return stringBuilder.ToString();
		}
		/// <summary>
		/// Appends a string representation of the current object to the specified StringBuilder, optionally including
		/// brackets.
		/// </summary>
		/// <param name="stringBuilder">The StringBuilder to which the string representation will be appended. Cannot be null.</param>
		/// <param name="addBrackets">true to enclose the string representation in brackets; otherwise, false.</param>
		public abstract void ToString(StringBuilder stringBuilder, bool addBrackets);
	}
}