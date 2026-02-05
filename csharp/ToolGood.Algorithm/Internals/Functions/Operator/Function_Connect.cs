using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Operator
{
	internal class Function_Connect : Function_2
	{
		public Function_Connect(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override string Name => "&";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter); if(args1.IsError) { return args1; }
			var args2 = func2.Evaluate(work, tempParameter); if(args2.IsError) { return args2; }

			if(args1.IsNull) {
				if(args2.IsNull) return args1;
				return ConvertToText(args2, "&", 2);
			} else if(args2.IsNull) {
				return ConvertToText(args1, "&", 1);
			}
			args1 = ConvertToText(args1, "&", 1);
			if(args1.IsError) { return args1; }
			args2 = ConvertToText(args2, "&", 2);
			if(args2.IsError) { return args2; }

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

}