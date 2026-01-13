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

	#region 1 2 3 4 N

	internal abstract class Function_1 : FunctionBase
	{
		protected FunctionBase func1;

		protected Function_1(FunctionBase func1)
		{
			this.func1 = func1;
		}
		protected void AddFunction(StringBuilder stringBuilder, string functionName)
		{
			stringBuilder.Append(functionName);
			stringBuilder.Append('(');
			func1.ToString(stringBuilder, false);
			stringBuilder.Append(')');
		}
	}

	internal abstract class Function_2 : FunctionBase
	{
		protected FunctionBase func1;
		protected FunctionBase func2;

		public Function_2(FunctionBase func1, FunctionBase func2)
		{
			this.func1 = func1;
			this.func2 = func2;
		}

		protected void AddFunction(StringBuilder stringBuilder, string functionName)
		{
			stringBuilder.Append(functionName);
			stringBuilder.Append('(');
			func1.ToString(stringBuilder, false);
			if(func2 != null) {
				stringBuilder.Append(", ");
				func2.ToString(stringBuilder, false);
			}
			stringBuilder.Append(')');
		}
	}

	internal abstract class Function_3 : FunctionBase
	{
		protected FunctionBase func1;
		protected FunctionBase func2;
		protected FunctionBase func3;

		protected Function_3(FunctionBase func1, FunctionBase func2, FunctionBase func3)
		{
			this.func1 = func1;
			this.func2 = func2;
			this.func3 = func3;
		}
		protected void AddFunction(StringBuilder stringBuilder, string functionName)
		{
			stringBuilder.Append(functionName);
			stringBuilder.Append('(');
			func1.ToString(stringBuilder, false);
			if(func2 != null) {
				stringBuilder.Append(", ");
				func2.ToString(stringBuilder, false);
				if(func3 != null) {
					stringBuilder.Append(", ");
					func3.ToString(stringBuilder, false);
				}
			}
			stringBuilder.Append(')');
		}
	}

	internal abstract class Function_4 : FunctionBase
	{
		protected FunctionBase func1;
		protected FunctionBase func2;
		protected FunctionBase func3;
		protected FunctionBase func4;

		protected Function_4(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4)
		{
			this.func1 = func1;
			this.func2 = func2;
			this.func3 = func3;
			this.func4 = func4;
		}
		protected void AddFunction(StringBuilder stringBuilder, string functionName)
		{
			stringBuilder.Append(functionName);
			stringBuilder.Append('(');
			func1.ToString(stringBuilder, false);
			if(func2 != null) {
				stringBuilder.Append(", ");
				func2.ToString(stringBuilder, false);
				if(func3 != null) {
					stringBuilder.Append(", ");
					func3.ToString(stringBuilder, false);
					if(func4 != null) {
						stringBuilder.Append(", ");
						func4.ToString(stringBuilder, false);
					}
				}
			}
			stringBuilder.Append(')');
		}
	}

	internal abstract class Function_N : FunctionBase
	{
		protected FunctionBase[] funcs;

		protected Function_N(FunctionBase[] funcs)
		{
			this.funcs = funcs;
		}

		protected void AddFunction(StringBuilder stringBuilder, string functionName)
		{
			stringBuilder.Append(functionName);
			stringBuilder.Append('(');
			for(int i = 0; i < funcs.Length; i++) {
				if(i > 0) {
					stringBuilder.Append(", ");
				}
				funcs[i].ToString(stringBuilder, false);
			}
			stringBuilder.Append(')');
		}
	}
	#endregion

	#region * / % + - &

	internal class Function_Mul : Function_2
	{
		public Function_Mul(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if(args1.IsError) { return args1; }
			var args2 = func2.Evaluate(work, tempParameter); if(args2.IsError) { return args2; }

			if(args1.IsNumber && args2.IsNumber) { //  优化性能
				if(args1.NumberValue == 1m) { return args2; }
				if(args2.NumberValue == 1m) { return args1; }
				if(args1.NumberValue == 0m || args2.NumberValue == 0m) { return Operand.Zero; }
				return Operand.Create(args1.NumberValue * args2.NumberValue);
			}
			if(args1.IsNull) { return Operand.Error("Function '{0}' parameter {1} is NULL!", "*", 1); }
			if(args2.IsNull) { return Operand.Error("Function '{0}' parameter {1} is NULL!", "*", 2); }

			if(args1.IsText) {
				if(decimal.TryParse(args1.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
					args1 = Operand.Create(d);
				} else if(FunctionUtil.TryParseBoolean(args1.TextValue, out bool b)) {
					args1 = b ? Operand.One : Operand.Zero;
				} else if(TimeSpan.TryParse(args1.TextValue, CultureInfo.InvariantCulture, out TimeSpan ts)) {
					args1 = Operand.Create(ts);
				} else if(DateTime.TryParse(args1.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) {
					args1 = Operand.Create(new MyDate(dt));
				} else {
					return Operand.Error("Two types cannot be multiplied ");
				}
			}
			if(args2.IsText) {
				if(decimal.TryParse(args2.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
					args2 = Operand.Create(d);
				} else if(FunctionUtil.TryParseBoolean(args2.TextValue, out bool b)) {
					args2 = b ? Operand.One : Operand.Zero;
				} else if(TimeSpan.TryParse(args2.TextValue, CultureInfo.InvariantCulture, out TimeSpan ts)) {
					args2 = Operand.Create(ts);
				} else if(DateTime.TryParse(args2.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) {
					args2 = Operand.Create(new MyDate(dt));
				} else {
					return Operand.Error("Two types cannot be multiplied");
				}
			}
			if(args1.IsDate) {
				if(args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "*", 2); if(args2.IsError) { return args2; } }
				if(args2.NumberValue == 1) { return args1; }
				return Operand.Create((MyDate)(args1.DateValue * args2.NumberValue));
			} else if(args2.IsDate) {
				if(args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "*", 1); if(args1.IsError) { return args1; } }
				if(args1.NumberValue == 1) { return args2; }
				return Operand.Create((MyDate)(args2.DateValue * args1.NumberValue));
			}

			if(args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "*", 1); if(args1.IsError) { return args1; } }
			if(args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "*", 2); if(args2.IsError) { return args2; } }

			if(args1.NumberValue == 1m) { return args2; }
			if(args2.NumberValue == 1m) { return args1; }

			return Operand.Create(args1.NumberValue * args2.NumberValue);
		}

		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			if(addBrackets) stringBuilder.Append('(');
			func1.ToString(stringBuilder, true);
			stringBuilder.Append(" * ");
			func2.ToString(stringBuilder, true);
			if(addBrackets) stringBuilder.Append(')');
		}
	}

	internal class Function_Div : Function_2
	{
		public Function_Div(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if(args1.IsError) { return args1; }
			var args2 = func2.Evaluate(work, tempParameter); if(args2.IsError) { return args2; }

			if(args1.IsNumber && args2.IsNumber) { //  优化性能
				if(args2.NumberValue == 1m) { return args1; }
				if(args2.NumberValue == 0m) { return Operand.Error("Div 0 is error!"); }
				return Operand.Create(args1.NumberValue / args2.NumberValue);
			}
			if(args1.IsNull) { return Operand.Error("Function '{0}' parameter {1} is NULL!", "/", 1); }
			if(args2.IsNull) { return Operand.Error("Function '{0}' parameter {1} is NULL!", "/", 2); }

			if(args1.IsText) {
				if(decimal.TryParse(args1.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
					args1 = Operand.Create(d);
				} else if(FunctionUtil.TryParseBoolean(args1.TextValue, out bool b)) {
					args1 = b ? Operand.One : Operand.Zero;
				} else if(TimeSpan.TryParse(args1.TextValue, CultureInfo.InvariantCulture, out TimeSpan ts)) {
					args1 = Operand.Create(ts);
				} else if(DateTime.TryParse(args1.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) {
					args1 = Operand.Create(new MyDate(dt));
				} else {
					return Operand.Error("Two types cannot be divided ");
				}
			}
			if(args2.IsText) {
				if(decimal.TryParse(args2.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
					args2 = Operand.Create(d);
				} else if(FunctionUtil.TryParseBoolean(args2.TextValue, out bool b)) {
					args1 = b ? Operand.One : Operand.Zero;
				} else if(TimeSpan.TryParse(args2.TextValue, CultureInfo.InvariantCulture, out TimeSpan ts)) {
					args2 = Operand.Create(ts);
				} else if(DateTime.TryParse(args2.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) {
					args2 = Operand.Create(new MyDate(dt));
				} else {
					return Operand.Error("Two types cannot be divided");
				}
			}
			if(args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "/", 2); if(args2.IsError) { return args2; } }
			if(args2.NumberValue == 0) { return Operand.Error("Div 0 is error!"); }
			if(args2.NumberValue == 1) { return args1; }

			if(args1.IsDate) { return Operand.Create(args1.DateValue / args2.NumberValue); }
			if(args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "/", 1); if(args1.IsError) { return args1; } }
			return Operand.Create(args1.NumberValue / args2.NumberValue);
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			if(addBrackets) stringBuilder.Append('(');
			func1.ToString(stringBuilder, true);
			stringBuilder.Append(" / ");
			func2.ToString(stringBuilder, true);
			if(addBrackets) stringBuilder.Append(')');
		}
	}

	internal class Function_Mod : Function_2
	{
		public Function_Mod(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if(args1.IsError) { return args1; }
			var args2 = func2.Evaluate(work, tempParameter); if(args2.IsError) { return args2; }

			if(args1.IsNumber && args2.IsNumber) { //  优化性能
				if(args2.NumberValue == 0m) { return Operand.Error("Div 0 is error!"); }
				return Operand.Create(args1.NumberValue % args2.NumberValue);
			}
			if(args1.IsNull) { return Operand.Error("Function '{0}' parameter {1} is NULL!", "%", 1); }
			if(args2.IsNull) { return Operand.Error("Function '{0}' parameter {1} is NULL!", "%", 2); }

			if(args1.IsText) {
				if(decimal.TryParse(args1.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
					args1 = Operand.Create(d);
				} else if(FunctionUtil.TryParseBoolean(args1.TextValue, out bool b)) {
					args1 = b ? Operand.One : Operand.Zero;
				} else if(TimeSpan.TryParse(args1.TextValue, CultureInfo.InvariantCulture, out TimeSpan ts)) {
					args1 = Operand.Create(ts);
				} else if(DateTime.TryParse(args1.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) {
					args1 = Operand.Create(new MyDate(dt));
				} else {
					return Operand.Error("Two types cannot be modulo ");
				}
			}
			if(args2.IsText) {
				if(decimal.TryParse(args2.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
					args2 = Operand.Create(d);
				} else if(FunctionUtil.TryParseBoolean(args2.TextValue, out bool b)) {
					args1 = b ? Operand.One : Operand.Zero;
				} else if(TimeSpan.TryParse(args2.TextValue, CultureInfo.InvariantCulture, out TimeSpan ts)) {
					args2 = Operand.Create(ts);
				} else if(DateTime.TryParse(args2.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) {
					args2 = Operand.Create(new MyDate(dt));
				} else {
					return Operand.Error("Two types cannot be modulo");
				}
			}
			if(args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "%", 1); if(args1.IsError) { return args1; } }
			if(args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "%", 2); if(args2.IsError) { return args2; } }

			if(args2.NumberValue == 0m) { return Operand.Error("Div 0 is error!"); }

			return Operand.Create(args1.NumberValue % args2.NumberValue);
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			if(addBrackets) stringBuilder.Append('(');
			func1.ToString(stringBuilder, true);
			stringBuilder.Append(" % ");
			func2.ToString(stringBuilder, true);
			if(addBrackets) stringBuilder.Append(')');
		}
	}

	internal class Function_Add : Function_2
	{
		public Function_Add(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if(args1.IsError) { return args1; }
			var args2 = func2.Evaluate(work, tempParameter); if(args2.IsError) { return args2; }

			if(args1.IsNumber && args2.IsNumber) { //  优化性能
				if(args1.NumberValue == 0m) { return args2; }
				if(args2.NumberValue == 0m) { return args1; }
				return Operand.Create(args1.NumberValue + args2.NumberValue);
			}
			if(args1.IsNull) { return Operand.Error("Function '{0}' parameter {1} is NULL!", "+", 1); }
			if(args2.IsNull) { return Operand.Error("Function '{0}' parameter {1} is NULL!", "+", 2); }

			if(args1.IsText) {
				if(decimal.TryParse(args1.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
					args1 = Operand.Create(d);
				} else if(FunctionUtil.TryParseBoolean(args1.TextValue, out bool b)) {
					args1 = b ? Operand.One : Operand.Zero;
				} else if(TimeSpan.TryParse(args1.TextValue, CultureInfo.InvariantCulture, out TimeSpan ts)) {
					args1 = Operand.Create(ts);
				} else if(DateTime.TryParse(args1.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) {
					args1 = Operand.Create(new MyDate(dt));
				} else {
					return Operand.Error("Function '+' is error");
				}
			}
			if(args2.IsText) {
				if(decimal.TryParse(args2.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
					args2 = Operand.Create(d);
				} else if(FunctionUtil.TryParseBoolean(args2.TextValue, out bool b)) {
					args2 = b ? Operand.One : Operand.Zero;
				} else if(TimeSpan.TryParse(args2.TextValue, CultureInfo.InvariantCulture, out TimeSpan ts)) {
					args2 = Operand.Create(ts);
				} else if(DateTime.TryParse(args2.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) {
					args2 = Operand.Create(new MyDate(dt));
				} else {
					return Operand.Error("Function '+' is error");
				}
			}
			if(args1.IsDate) {
				if(args2.IsDate) return Operand.Create(args1.DateValue + args2.DateValue);
				if(args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "+", 2); if(args2.IsError) { return args2; } }
				if(args2.NumberValue == 0m) { return args1; }
				return Operand.Create(args1.DateValue + args2.NumberValue);
			} else if(args2.IsDate) {
				if(args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "+", 1); if(args1.IsError) { return args1; } }
				if(args1.NumberValue == 0m) { return args2; }
				return Operand.Create(args2.DateValue + args1.NumberValue);
			}
			if(args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "+", 1); if(args1.IsError) { return args1; } }
			if(args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "+", 2); if(args2.IsError) { return args2; } }

			if(args1.NumberValue == 0m) { return args2; }
			if(args2.NumberValue == 0m) { return args1; }

			return Operand.Create(args1.NumberValue + args2.NumberValue);
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			if(addBrackets) stringBuilder.Append('(');
			func1.ToString(stringBuilder, false);
			stringBuilder.Append(" + ");
			func2.ToString(stringBuilder, false);
			if(addBrackets) stringBuilder.Append(')');
		}
	}

	internal class Function_Sub : Function_2
	{
		public Function_Sub(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if(args1.IsError) { return args1; }
			var args2 = func2.Evaluate(work, tempParameter); if(args2.IsError) { return args2; }

			if(args1.IsNumber && args2.IsNumber) { //  优化性能
				if(args2.NumberValue == 0m) { return args1; }
				return Operand.Create(args1.NumberValue - args2.NumberValue);
			}
			if(args1.IsNull) { return Operand.Error("Function '{0}' parameter {1} is NULL!", "-", 1); }
			if(args2.IsNull) { return Operand.Error("Function '{0}' parameter {1} is NULL!", "-", 2); }

			if(args1.IsText) {
				if(decimal.TryParse(args1.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
					args1 = Operand.Create(d);
				} else if(FunctionUtil.TryParseBoolean(args1.TextValue, out bool b)) {
					args1 = b ? Operand.One : Operand.Zero;
				} else if(TimeSpan.TryParse(args1.TextValue, CultureInfo.InvariantCulture, out TimeSpan ts)) {
					args1 = Operand.Create(ts);
				} else if(DateTime.TryParse(args1.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) {
					args1 = Operand.Create(new MyDate(dt));
				} else {
					return Operand.Error("Function '-' is error");
				}
			}
			if(args2.IsText) {
				if(decimal.TryParse(args2.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
					args2 = Operand.Create(d);
				} else if(FunctionUtil.TryParseBoolean(args2.TextValue, out bool b)) {
					args2 = b ? Operand.One : Operand.Zero;
				} else if(TimeSpan.TryParse(args2.TextValue, CultureInfo.InvariantCulture, out TimeSpan ts)) {
					args2 = Operand.Create(ts);
				} else if(DateTime.TryParse(args2.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) {
					args2 = Operand.Create(new MyDate(dt));
				} else {
					return Operand.Error("Function '-' is error");
				}
			}
			if(args1.IsDate) {
				if(args2.IsDate) return Operand.Create(args1.DateValue - args2.DateValue);
				if(args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "-", 2); if(args2.IsError) { return args2; } }
				if(args2.NumberValue == 0m) { return args1; }
				return Operand.Create(args1.DateValue - args2.NumberValue);
			} else if(args2.IsDate) {
				if(args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "-", 1); if(args1.IsError) { return args1; } }
				return Operand.Create(args1.NumberValue - args2.DateValue);
			}
			if(args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "-", 1); if(args1.IsError) { return args1; } }
			if(args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "-", 2); if(args2.IsError) { return args2; } }

			if(args2.NumberValue == 0m) { return args1; }

			return Operand.Create(args1.NumberValue - args2.NumberValue);
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			if(addBrackets) stringBuilder.Append('(');
			func1.ToString(stringBuilder, false);
			stringBuilder.Append(" - ");
			func2.ToString(stringBuilder, false);
			if(addBrackets) stringBuilder.Append(')');
		}
	}

	internal class Function_Connect : Function_2
	{
		public Function_Connect(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if(args1.IsError) { return args1; }
			var args2 = func2.Evaluate(work, tempParameter); if(args2.IsError) { return args2; }

			if(args1.IsNull) {
				if(args2.IsNull) return args1;
				return args2.ToText("Function '{0}' parameter {1} is error!", "&", 2);
			} else if(args2.IsNull) {
				return args1.ToText("Function '{0}' parameter {1} is error!", "&", 1);
			}
			if(args1.IsNotText) { args1 = args1.ToText("Function '{0}' parameter {1} is error!", "&", 1); if(args1.IsError) { return args1; } }
			if(args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "&", 2); if(args2.IsError) { return args2; } }

			return Operand.Create(args1.TextValue + args2.TextValue);
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			if(addBrackets) stringBuilder.Append('(');
			func1.ToString(stringBuilder, false);
			stringBuilder.Append(" & ");
			func2.ToString(stringBuilder, false);
			if(addBrackets) stringBuilder.Append(')');
		}
	}

	#endregion * / % + - &

	#region == != >= <= > <

	internal class Function_EQ : Function_2
	{
		public Function_EQ(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if(args1.IsError) { return args1; }
			var args2 = func2.Evaluate(work, tempParameter); if(args2.IsError) { return args2; }

			if(args1.Type == args2.Type) {
				if(args1.IsNumber) {
					return Operand.Create(args1.NumberValue == args2.NumberValue);
				} else if(args1.IsText) {
					return Operand.Create(args1.TextValue == args2.TextValue);
				} else if(args1.IsBoolean) {
					return Operand.Create(args1.BooleanValue == args2.BooleanValue);
				} else if(args1.IsDate) {
					args1 = args1.ToNumber();
					args2 = args2.ToNumber();
					return Operand.Create(args1.NumberValue == args2.NumberValue);
				} else if(args1.IsJson) {
					args1 = args1.ToText();
					args2 = args2.ToText();
					return Operand.Create(args1.TextValue == args2.TextValue);
				} else if(args1.IsNull) {
					return Operand.True;
				} else {
					return Operand.Error("Function '==' compare is error.");
				}
			} else if(args1.IsNull || args2.IsNull) {
				return Operand.False;
			} else if(args2.IsText) {
				if(args1.IsBoolean) {
					var a = args2.ToBoolean();
					if(a.IsError == false) {
						return a.BooleanValue == args1.BooleanValue ? Operand.True : Operand.False;
					}
					args1 = args1.ToText();
					return Operand.Create(args1.TextValue == args2.TextValue);
				} else if(args1.IsDate || args1.IsNumber || args1.IsJson) {
					args1 = args1.ToText();
					return Operand.Create(args1.TextValue == args2.TextValue);
				} else {
					return Operand.Error("Function '==' compare is error.");
				}
			} else if(args1.IsJson || args2.IsJson || args1.IsArray || args2.IsArray || args1.IsArrayJson || args2.IsArrayJson) {
				return Operand.Error("Function '==' compare is error.");
			}
			if(args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "==", 1); if(args1.IsError) { return args1; } }
			if(args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "==", 2); if(args2.IsError) { return args2; } }

			return Operand.Create(args1.NumberValue == args2.NumberValue);
		}

		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			if(addBrackets) stringBuilder.Append('(');
			func1.ToString(stringBuilder, false);
			stringBuilder.Append(" == ");
			func2.ToString(stringBuilder, false);
			if(addBrackets) stringBuilder.Append(')');
		}
	}

	internal class Function_NE : Function_2
	{
		public Function_NE(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if(args1.IsError) { return args1; }
			var args2 = func2.Evaluate(work, tempParameter); if(args2.IsError) { return args2; }

			if(args1.Type == args2.Type) {
				if(args1.IsNumber) {
					return Operand.Create(args1.NumberValue != args2.NumberValue);
				} else if(args1.IsText) {
					return Operand.Create(args1.TextValue != args2.TextValue);
				} else if(args1.IsBoolean) {
					return Operand.Create(args1.BooleanValue != args2.BooleanValue);
				} else if(args1.IsDate) {
					args1 = args1.ToNumber();
					args2 = args2.ToNumber();
					return Operand.Create(args1.NumberValue != args2.NumberValue);
				} else if(args1.IsJson) {
					args1 = args1.ToText();
					args2 = args2.ToText();
					return Operand.Create(args1.TextValue != args2.TextValue);
				} else if(args1.IsNull) {
					return Operand.False;
				} else {
					return Operand.Error("Function '!=' compare is error.");
				}
			} else if(args1.IsNull || args2.IsNull) {
				return Operand.True;
			} else if(args2.IsText) {
				if(args1.IsBoolean) {
					var a = args2.ToBoolean();
					if(a.IsError == false) {
						return a.BooleanValue != args1.BooleanValue ? Operand.True : Operand.False;
					}
					args1 = args1.ToText();
					return Operand.Create(args1.TextValue != args2.TextValue);
				} else if(args1.IsDate || args1.IsNumber || args1.IsJson) {
					args1 = args1.ToText();
					return Operand.Create(args1.TextValue != args2.TextValue);
				} else {
					return Operand.Error("Function '!=' compare is error.");
				}
			} else if(args1.IsJson || args2.IsJson || args1.IsArray || args2.IsArray || args1.IsArrayJson || args2.IsArrayJson) {
				return Operand.Error("Function '!=' compare is error.");
			}
			if(args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "!=", 1); if(args1.IsError) { return args1; } }
			if(args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "!=", 2); if(args2.IsError) { return args2; } }

			return Operand.Create(args1.NumberValue != args2.NumberValue);
		}

		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			if(addBrackets) stringBuilder.Append('(');
			func1.ToString(stringBuilder, false);
			stringBuilder.Append(" != ");
			func2.ToString(stringBuilder, false);
			if(addBrackets) stringBuilder.Append(')');
		}
	}

	internal class Function_GE : Function_2
	{
		public Function_GE(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if(args1.IsError) { return args1; }
			var args2 = func2.Evaluate(work, tempParameter); if(args2.IsError) { return args2; }

			if(args1.Type == args2.Type) {
				if(args1.IsNumber) {
					return Operand.Create(args1.NumberValue >= args2.NumberValue);
				} else if(args1.IsText) {
					var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
					return r >= 0 ? Operand.True : Operand.False;
				} else if(args1.IsDate || args1.IsBoolean) {
					args1 = args1.ToNumber();
					args2 = args2.ToNumber();
					return Operand.Create(args1.NumberValue >= args2.NumberValue);
				} else if(args1.IsJson) {
					args1 = args1.ToText();
					args2 = args2.ToText();
					var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
					return r >= 0 ? Operand.True : Operand.False;
				} else if(args1.IsNull) {
					return Operand.True;
				} else {
					return Operand.Error("Function '>=' compare is error.");
				}
			} else if(args1.IsNull || args2.IsNull) {
				return Operand.False;
			} else if(args2.IsText) {
				if(args1.IsBoolean) {
					var a = args2.ToBoolean();
					if(a.IsError == false) {
						return a.BooleanValue != args1.BooleanValue ? Operand.True : Operand.False;
					}
					args1 = args1.ToText();
					var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
					return r >= 0 ? Operand.True : Operand.False;
				} else if(args1.IsDate || args1.IsNumber || args1.IsJson) {
					args1 = args1.ToText();
					var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
					return r >= 0 ? Operand.True : Operand.False;
				} else {
					return Operand.Error("Function '>=' compare is error.");
				}
			} else if(args1.IsJson || args2.IsJson || args1.IsArray || args2.IsArray || args1.IsArrayJson || args2.IsArrayJson) {
				return Operand.Error("Function '>=' compare is error.");
			}
			if(args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", ">=", 1); if(args1.IsError) { return args1; } }
			if(args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", ">=", 2); if(args2.IsError) { return args2; } }

			return Operand.Create(args1.NumberValue >= args2.NumberValue);
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			if(addBrackets) stringBuilder.Append('(');
			func1.ToString(stringBuilder, false);
			stringBuilder.Append(" >= ");
			func2.ToString(stringBuilder, false);
			if(addBrackets) stringBuilder.Append(')');
		}
	}

	internal class Function_LE : Function_2
	{
		public Function_LE(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if(args1.IsError) { return args1; }
			var args2 = func2.Evaluate(work, tempParameter); if(args2.IsError) { return args2; }

			if(args1.Type == args2.Type) {
				if(args1.IsNumber) {
					return Operand.Create(args1.NumberValue <= args2.NumberValue);
				} else if(args1.IsText) {
					var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
					return r <= 0 ? Operand.True : Operand.False;
				} else if(args1.IsDate || args1.IsBoolean) {
					args1 = args1.ToNumber();
					args2 = args2.ToNumber();
					return Operand.Create(args1.NumberValue <= args2.NumberValue);
				} else if(args1.IsJson) {
					args1 = args1.ToText();
					args2 = args2.ToText();
					var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
					return r <= 0 ? Operand.True : Operand.False;
				} else if(args1.IsNull) {
					return Operand.True;
				} else {
					return Operand.Error("Function '<=' compare is error.");
				}
			} else if(args1.IsNull || args2.IsNull) {
				return Operand.False;
			} else if(args2.IsText) {
				if(args1.IsBoolean) {
					var a = args2.ToBoolean();
					if(a.IsError == false) {
						return a.BooleanValue != args1.BooleanValue ? Operand.True : Operand.False;
					}
					args1 = args1.ToText();
					var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
					return r <= 0 ? Operand.True : Operand.False;
				} else if(args1.IsDate || args1.IsNumber || args1.IsJson) {
					args1 = args1.ToText();
					var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
					return r <= 0 ? Operand.True : Operand.False;
				} else {
					return Operand.Error("Function '<=' compare is error.");
				}
			} else if(args1.IsJson || args2.IsJson || args1.IsArray || args2.IsArray || args1.IsArrayJson || args2.IsArrayJson) {
				return Operand.Error("Function '<=' compare is error.");
			}
			if(args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "<=", 1); if(args1.IsError) { return args1; } }
			if(args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "<=", 2); if(args2.IsError) { return args2; } }

			return Operand.Create(args1.NumberValue <= args2.NumberValue);
		}

		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			if(addBrackets) stringBuilder.Append('(');
			func1.ToString(stringBuilder, false);
			stringBuilder.Append(" <= ");
			func2.ToString(stringBuilder, false);
			if(addBrackets) stringBuilder.Append(')');
		}
	}

	internal class Function_GT : Function_2
	{
		public Function_GT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if(args1.IsError) { return args1; }
			var args2 = func2.Evaluate(work, tempParameter); if(args2.IsError) { return args2; }

			if(args1.Type == args2.Type) {
				if(args1.IsNumber) {
					return Operand.Create(args1.NumberValue > args2.NumberValue);
				} else if(args1.IsText) {
					var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
					return r > 0 ? Operand.True : Operand.False;
				} else if(args1.IsDate || args1.IsBoolean) {
					args1 = args1.ToNumber();
					args2 = args2.ToNumber();
					return Operand.Create(args1.NumberValue > args2.NumberValue);
				} else if(args1.IsJson) {
					args1 = args1.ToText();
					args2 = args2.ToText();
					var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
					return r > 0 ? Operand.True : Operand.False;
				} else if(args1.IsNull) {
					return Operand.True;
				} else {
					return Operand.Error("Function '>' compare is error.");
				}
			} else if(args1.IsNull || args2.IsNull) {
				return Operand.False;
			} else if(args2.IsText) {
				if(args1.IsBoolean) {
					var a = args2.ToBoolean();
					if(a.IsError == false) {
						return a.BooleanValue != args1.BooleanValue ? Operand.True : Operand.False;
					}
					args1 = args1.ToText();
					var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
					return r > 0 ? Operand.True : Operand.False;
				} else if(args1.IsDate || args1.IsNumber || args1.IsJson) {
					args1 = args1.ToText();
					var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
					return r > 0 ? Operand.True : Operand.False;
				} else {
					return Operand.Error("Function '>' compare is error.");
				}
			} else if(args1.IsJson || args2.IsJson || args1.IsArray || args2.IsArray || args1.IsArrayJson || args2.IsArrayJson) {
				return Operand.Error("Function '>' compare is error.");
			}
			if(args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", ">", 1); if(args1.IsError) { return args1; } }
			if(args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", ">", 2); if(args2.IsError) { return args2; } }

			return Operand.Create(args1.NumberValue > args2.NumberValue);
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			if(addBrackets) stringBuilder.Append('(');
			func1.ToString(stringBuilder, false);
			stringBuilder.Append(" > ");
			func2.ToString(stringBuilder, false);
			if(addBrackets) stringBuilder.Append(')');
		}
	}

	internal class Function_LT : Function_2
	{
		public Function_LT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if(args1.IsError) { return args1; }
			var args2 = func2.Evaluate(work, tempParameter); if(args2.IsError) { return args2; }

			if(args1.Type == args2.Type) {
				if(args1.IsNumber) {
					return Operand.Create(args1.NumberValue < args2.NumberValue);
				} else if(args1.IsText) {
					var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
					return r < 0 ? Operand.True : Operand.False;
				} else if(args1.IsDate || args1.IsBoolean) {
					args1 = args1.ToNumber();
					args2 = args2.ToNumber();
					return Operand.Create(args1.NumberValue < args2.NumberValue);
				} else if(args1.IsJson) {
					args1 = args1.ToText();
					args2 = args2.ToText();
					var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
					return r < 0 ? Operand.True : Operand.False;
				} else if(args1.IsNull) {
					return Operand.True;
				} else {
					return Operand.Error("Function '<' compare is error.");
				}
			} else if(args1.IsNull || args2.IsNull) {
				return Operand.False;
			} else if(args2.IsText) {
				if(args1.IsBoolean) {
					var a = args2.ToBoolean();
					if(a.IsError == false) {
						return a.BooleanValue != args1.BooleanValue ? Operand.True : Operand.False;
					}
					args1 = args1.ToText();
					var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
					return r < 0 ? Operand.True : Operand.False;
				} else if(args1.IsDate || args1.IsNumber || args1.IsJson) {
					args1 = args1.ToText();
					var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
					return r < 0 ? Operand.True : Operand.False;
				} else {
					return Operand.Error("Function '<' compare is error.");
				}
			} else if(args1.IsJson || args2.IsJson
				  || args1.IsArray || args2.IsArray
				  || args1.IsArrayJson || args2.IsArrayJson
				  ) {
				return Operand.Error("Function '<' compare is error.");
			}
			if(args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "<", 1); if(args1.IsError) { return args1; } }
			if(args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "<", 2); if(args2.IsError) { return args2; } }

			return Operand.Create(args1.NumberValue < args2.NumberValue);
		}

		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			if(addBrackets) stringBuilder.Append('(');
			func1.ToString(stringBuilder, false);
			stringBuilder.Append(" < ");
			func2.ToString(stringBuilder, false);
			if(addBrackets) stringBuilder.Append(')');
		}
	}

	#endregion == != >= <= > <

	#region AND OR

	internal class Function_AND : Function_2
	{
		public Function_AND(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			// 程序 && and || or 与 excel的  AND(x,y) OR(x,y) 有区别
			// 在excel内 AND(x,y) OR(x,y) 先报错，
			// 在程序中，&& and  有true 直接返回true 就不会检测下一个会不会报错
			// 在程序中，|| or  有false 直接返回false 就不会检测下一个会不会报错
			var args1 = func1.Evaluate(work, tempParameter); if(args1.IsNotBoolean) { args1 = args1.ToBoolean(); if(args1.IsError) { return args1; } }
			if(args1.BooleanValue == false) {
				var args2 = func2.Evaluate(work, tempParameter).ToBoolean();
				if(args2.IsError) { return args2; }
				return Operand.False;
			}
			return func2.Evaluate(work, tempParameter).ToBoolean();
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			if(addBrackets) stringBuilder.Append('(');
			func1.ToString(stringBuilder, false);
			stringBuilder.Append(" && ");
			func2.ToString(stringBuilder, false);
			if(addBrackets) stringBuilder.Append(')');
		}
	}

	internal class Function_OR : Function_2
	{
		public Function_OR(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			// 程序 && and || or 与 excel的  AND(x,y) OR(x,y) 有区别
			// 在excel内 AND(x,y) OR(x,y) 先报错，
			// 在程序中，&& and  有true 直接返回true 就不会检测下一个会不会报错
			// 在程序中，|| or  有false 直接返回false 就不会检测下一个会不会报错
			var args1 = func1.Evaluate(work, tempParameter); if(args1.IsNotBoolean) { args1 = args1.ToBoolean(); if(args1.IsError) { return args1; } }
			if(args1.BooleanValue) {
				var args2 = func2.Evaluate(work, tempParameter).ToBoolean();
				if(args2.IsError) { return args2; }
				return Operand.True;
			}
			return func2.Evaluate(work, tempParameter).ToBoolean();
		}

		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			if(addBrackets) stringBuilder.Append('(');
			func1.ToString(stringBuilder, false);
			stringBuilder.Append(" || ");
			func2.ToString(stringBuilder, false);
			if(addBrackets) stringBuilder.Append(')');
		}
	}

	internal class Function_AND_N : Function_N
	{
		public Function_AND_N(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var index = 1;
			bool b = true;
			foreach(var item in funcs) {
				var a = item.Evaluate(work, tempParameter);
				if(a.IsNotBoolean) { a = a.ToBoolean("Function '{0}' parameter {1} is error!", "OR", index++); if(a.IsError) { return a; } }
				if(a.BooleanValue == false) b = false;
			}
			return b ? Operand.True : Operand.False;
		}

		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "AND");
		}
	}

	internal class Function_OR_N : Function_N
	{
		public Function_OR_N(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var index = 1;
			bool b = false;
			foreach(var item in funcs) {
				var a = item.Evaluate(work, tempParameter);
				if(a.IsNotBoolean) { a = a.ToBoolean("Function '{0}' parameter {1} is error!", "OR", index++); if(a.IsError) { return a; } }
				if(a.BooleanValue) b = true;
			}
			return b ? Operand.True : Operand.False;
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "OR");
		}
	}

	#endregion AND OR


	#region Lookup

	internal class Function_VLOOKUP : Function_4
	{
		public Function_VLOOKUP(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) : base(func1, func2, func3, func4)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			//  这个跟excel的vlookup有区别，excel是按列查找，这里是按array查找，参数位置不一样 用json代替吧
			var args1 = func1.Evaluate(work, tempParameter);
			if(args1.IsNotArray) { args1 = args1.ToArray("Function '{0}' parameter {1} is error!", "VLOOKUP", 1); if(args1.IsError) { return args1; } }
			var args2 = func2.Evaluate(work, tempParameter);
			if(args2.IsNotText) { args2 = args2.ToText("Function '{0}' parameter {1} is error!", "VLOOKUP", 2); if(args2.IsError) { return args2; } }
			var args3 = func3.Evaluate(work, tempParameter);
			if(args3.IsNotNumber) { args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "VLOOKUP", 3); if(args3.IsError) { return args3; } }

			var vague = true;
			if(func4 != null) {
				var args4 = func4.Evaluate(work, tempParameter);
				if(args4.IsNotBoolean) { args4 = args4.ToBoolean("Function '{0}' parameter {1} is error!", "VLOOKUP", 4); if(args4.IsError) { return args4; } }
				vague = args4.BooleanValue;
			}

			foreach(var item in args1.ArrayValue) {
				var o = item.ToArray("Function '{0}' parameter {1} is error!", "VLOOKUP", 1); if(o.IsError) { return o; }
				if(o.ArrayValue.Count > 0) {
					var o1 = o.ArrayValue[0];
					int b = -1;
					if(args2.IsNumber) {
						var o2 = o1.ToNumber(null);
						if(o2.IsError == false) {
							b = FunctionUtil.Compare(o2.NumberValue, args2.NumberValue);
						}
					} else {
						var o2 = o1.ToText(null);
						if(o2.IsError == false) {
							b = string.CompareOrdinal(o2.TextValue, args2.TextValue);
						}
					}
					if(b == 0) {
						var index = args3.IntValue - work.ExcelIndex;
						if(index < o.ArrayValue.Count) {
							return o.ArrayValue[index];
						}
					}
				}
			}

			if(vague) {//进行模糊匹配
				Operand last = null;
				var index = args3.IntValue - work.ExcelIndex;
				foreach(var item in args1.ArrayValue) {
					var o = item.ToArray(null);
					if(o.IsError) { return o; }
					if(o.ArrayValue.Count > 0) {
						var o1 = o.ArrayValue[0];
						int b = -1;
						if(args2.IsNumber) {
							var o2 = o1.ToNumber(null);
							if(o2.IsError == false) {
								b = FunctionUtil.Compare(o2.NumberValue, args2.NumberValue);
							}
						} else {
							var o2 = o1.ToText(null);
							if(o2.IsError == false) {
								b = string.CompareOrdinal(o2.TextValue, args2.TextValue);
							}
						}
						if(b < 0 && index < o.ArrayValue.Count) {
							last = o;
						} else if(b > 0 && last != null) {
							return last.ArrayValue[index];
						}
					}
				}
			}
			return Operand.Error("Function 'VLOOKUP' is not match!");
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "VLOOKUP");
		}
	}

	#endregion Lookup


}