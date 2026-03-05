using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Operator
{
	internal sealed class Function_Sub : Function_2
	{
		public Function_Sub(FunctionBase[] funcs) : base(funcs)
		{
		}

		public Function_Sub(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override string Name => "-";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(engine, tempParameter); if(args1.IsError) { return args1; }
			var args2 = GetNumber_2(engine, tempParameter); if(args2.IsError) { return args2; }

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
}