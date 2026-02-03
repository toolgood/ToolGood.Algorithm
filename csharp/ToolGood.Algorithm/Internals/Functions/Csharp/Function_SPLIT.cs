using System;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal class Function_SPLIT : Function_2
	{
		public Function_SPLIT(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToText(args1, "Split", 1);
			if(args1.IsError) { return args1; }

			var args2 = func2.Evaluate(work, tempParameter);
			args2 = FunctionUtil.ConvertToText(args2, "Split", 2);
			if(args2.IsError) { return args2; }

			return Operand.Create(args1.TextValue.Split(args2.TextValue.ToArray()));
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "Split");
		}
	}


}
