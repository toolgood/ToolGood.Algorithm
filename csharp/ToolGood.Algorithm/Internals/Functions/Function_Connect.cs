using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions
{
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

}