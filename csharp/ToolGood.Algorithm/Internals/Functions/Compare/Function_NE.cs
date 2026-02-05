using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Compare
{
	internal class Function_NE : Function_2
	{
		public Function_NE(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override string Name => "!=";

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
					return Operand.Create(args1.DateValue.ToLong() != args2.DateValue.ToLong());
				} else if(args1.IsNull) {
					return Operand.False;
				}
				return Operand.Error("Function '{0}' compare is error.", "!=");
			} else if(args1.IsNull || args2.IsNull) {
				return Operand.True;
			} else if(args1.IsDate || args2.IsDate || args1.IsJson || args2.IsJson || args1.IsArray || args2.IsArray || args1.IsArrayJson || args2.IsArrayJson) {
				return Operand.Error("Function '{0}' compare is error.", "!=");
			}
			args1 = FunctionUtil.ConvertToNumber(args1, "!=", 1);
			if(args1.IsError) { return args1; }
			args2 = FunctionUtil.ConvertToNumber(args2, "!=", 2);
			if(args2.IsError) { return args2; }

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
}