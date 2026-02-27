using System;
using System.Globalization;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Operator
{
	internal class Function_Div : Function_2
	{
		public Function_Div(FunctionBase[] funcs) : base(funcs)
		{
		}

		public Function_Div(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override string Name => "/";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(engine, tempParameter); if(args1.IsError) { return args1; }
			var args2 = GetNumber_2(engine, tempParameter); if(args2.IsError) { return args2; }

			if(args2.NumberValue == 0) { return Div0Error(); }
			if(args2.NumberValue == 1) { return args1; }

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
}