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

		public override string Name => "+";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(work, tempParameter); if(args1.IsError) { return args1; }
			var args2 = GetNumber_2(work, tempParameter); if(args2.IsError) { return args2; }

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