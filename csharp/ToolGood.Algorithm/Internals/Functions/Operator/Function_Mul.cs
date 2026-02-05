using System;
using System.Globalization;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Operator
{

	internal class Function_Mul : Function_2
	{
		public Function_Mul(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override string Name => "*";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(work, tempParameter); if(args1.IsError) { return args1; }
			var args2 = GetNumber_2(work, tempParameter); if(args2.IsError) { return args2; }
			 
			if(args1.NumberValue == 1m) { return args2; }
			if(args2.NumberValue == 1m) { return args1; }

			return Operand.Create(args1.NumberValue * args2.NumberValue);
		}

		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			if(addBrackets) stringBuilder.Append('(');
			func1.ToString(stringBuilder, true);
			stringBuilder.Append(" * ");
			func2.ToString(stringBuilder, true);
			if(addBrackets) stringBuilder.Append(')');
		}
	}

}