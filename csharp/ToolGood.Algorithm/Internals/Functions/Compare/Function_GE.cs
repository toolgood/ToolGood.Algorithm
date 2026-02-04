using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Compare
{
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
						return Operand.Error("Function '{0}' compare is error.", ">=");
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
					return Operand.Error("Function '{0}' compare is error.", ">=");
				}
			} else if(args1.IsJson || args2.IsJson || args1.IsArray || args2.IsArray || args1.IsArrayJson || args2.IsArrayJson) {
				return Operand.Error("Function '{0}' compare is error.", ">=");
			}
			args1 = FunctionUtil.ConvertToNumber(args1, ">=", 1);
			if(args1.IsError) { return args1; }
			args2 = FunctionUtil.ConvertToNumber(args2, ">=", 2);
			if(args2.IsError) { return args2; }

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
}