using System;
using System.Globalization;
using System.Text;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.Operator
{

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
				} else {
					return Operand.Error("Function '{0}' is error!", "+");
				}
			}
			if(args2.IsText) {
				if(decimal.TryParse(args2.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
					args2 = Operand.Create(d);
				} else if(FunctionUtil.TryParseBoolean(args2.TextValue, out bool b)) {
					args2 = b ? Operand.One : Operand.Zero;
				} else {
					return Operand.Error("Function '{0}' is error!", "+");
				}
			}
			args1 = FunctionUtil.ConvertToNumber(args1, "+", 1);
			if(args1.IsError) { return args1; }
			args2 = FunctionUtil.ConvertToNumber(args2, "+", 2);
			if(args2.IsError) { return args2; }

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

}