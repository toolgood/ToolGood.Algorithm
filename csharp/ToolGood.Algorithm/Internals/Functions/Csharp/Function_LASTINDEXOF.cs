using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal sealed class Function_LASTINDEXOF : Function_4
	{
		public Function_LASTINDEXOF(FunctionBase[] funcs) : base(funcs)
		{
		}

		

		public override string Name => "LastIndexOf";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsError) { return args1; }

			var args2 = GetText_2(engine, tempParameter);
			if(args2.IsError) { return args2; }

			var text = args1.TextValue;
			if(func3 == null) {
				return Operand.Create(text.AsSpan().LastIndexOf(args2.TextValue) + engine.ExcelIndex);
			}

			var args3 = GetNumber_3(engine, tempParameter);
			if(args3.IsError) { return args3; }

			if(func4 == null) {
				return Operand.Create(text.AsSpan(0, args3.IntValue).LastIndexOf(args2.TextValue) + engine.ExcelIndex);
			}

			var args4 = GetNumber_4(engine, tempParameter);
			if(args4.IsError) { return args4; }

			return Operand.Create(text.LastIndexOf(args2.TextValue, args3.IntValue, args4.IntValue) + engine.ExcelIndex);
		}

	}


}
