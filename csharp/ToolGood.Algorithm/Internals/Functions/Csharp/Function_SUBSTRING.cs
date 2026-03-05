using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal sealed class Function_SUBSTRING : Function_3
	{
		public Function_SUBSTRING(FunctionBase[] funcs) : base(funcs)
		{
		}

		

		public override string Name => "Substring";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsError) { return args1; }

			var args2 = GetNumber_2(engine, tempParameter);
			if(args2.IsError) { return args2; }

			var text = args1.TextValue;
			if(func3 == null) {
				return Operand.Create(text.AsSpan(args2.IntValue - engine.ExcelIndex).ToString());
			}

			var args3 = GetNumber_3(engine, tempParameter);
			if(args3.IsError) { return args3; }

			return Operand.Create(text.AsSpan(args2.IntValue - engine.ExcelIndex, args3.IntValue).ToString());
		}

	}


}
