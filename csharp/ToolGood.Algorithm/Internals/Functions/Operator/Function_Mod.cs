using System;
using System.Globalization;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Operator
{
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
				if(args2.NumberValue == 0m) { return Operand.Error("Function '{0}' Div 0 is error!", "%"); }
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
					return Operand.Error("Function '{0}' Two types cannot be modulo", "%");
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
					return Operand.Error("Function '{0}' Two types cannot be modulo", "%");
				}
			}
			if(args1.IsNotNumber) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "%", 1); if(args1.IsError) { return args1; } }
			if(args2.IsNotNumber) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "%", 2); if(args2.IsError) { return args2; } }

			if(args2.NumberValue == 0m) { return Operand.Error("Function '{0}' Div 0 is error!", "%"); }

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
}