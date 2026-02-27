using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Compare
{
	internal sealed class Function_LT : Function_2
	{
		public Function_LT(FunctionBase[] funcs) : base(funcs)
		{
		}

		

		public override string Name => "<";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(engine, tempParameter); if(args1.IsError) { return args1; }
			var args2 = func2.Evaluate(engine, tempParameter); if(args2.IsError) { return args2; }

			if(args1.Type == args2.Type) {
				if(args1.IsNumber) {
					return Operand.Create(args1.NumberValue < args2.NumberValue);
				} else if(args1.IsText) {
					var r = string.CompareOrdinal(args1.TextValue, args2.TextValue);
					return r < 0 ? Operand.True : Operand.False;
				} else if(args1.IsDate) {
					return Operand.Create(args1.DateValue.ToLong() < args2.DateValue.ToLong());
				} else if(args1.IsBoolean) {
					args1 = args1.ToNumber();
					args2 = args2.ToNumber();
					return Operand.Create(args1.NumberValue < args2.NumberValue);
				} else if(args1.IsNull) {
					return Operand.True;
				}
				return CompareError();
			} else if(args1.IsNull || args2.IsNull) {
				return Operand.False;
			} else if(args1.IsDate || args2.IsDate || args1.IsJson || args2.IsJson || args1.IsArray || args2.IsArray || args1.IsArrayJson || args2.IsArrayJson) {
				return CompareError();
			}
			args1 = ConvertToNumber(args1, 1);
			if(args1.IsError) { return args1; }
			args2 = ConvertToNumber(args2, 2);
			if(args2.IsError) { return args2; }

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

}