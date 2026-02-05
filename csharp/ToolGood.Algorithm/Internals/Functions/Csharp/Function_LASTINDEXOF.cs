using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal class Function_LASTINDEXOF : Function_4
	{
		public Function_LASTINDEXOF(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) : base(func1, func2, func3, func4)
		{
		}

		public override string Name => "LastIndexOf";

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter);
			args1 = ConvertToText(args1, "LastIndexOf", 1);
			if(args1.IsError) { return args1; }

			var args2 = func2.Evaluate(work, tempParameter);
			args2 = ConvertToText(args2, "LastIndexOf", 2);
			if(args2.IsError) { return args2; }

			var text = args1.TextValue;
			if(func3 == null) {
				return Operand.Create(text.AsSpan().LastIndexOf(args2.TextValue) + work.ExcelIndex);
			}

			var args3 = func3.Evaluate(work, tempParameter);
			args3 = ConvertToNumber(args3, "LastIndexOf", 3);
			if(args3.IsError) { return args3; }

			if(func4 == null) {
				return Operand.Create(text.AsSpan(0, args3.IntValue).LastIndexOf(args2.TextValue) + work.ExcelIndex);
			}

			var args4 = func4.Evaluate(work, tempParameter);
			args4 = ConvertToNumber(args4, "LastIndexOf", 4);
			if(args4.IsError) { return args4; }

			return Operand.Create(text.LastIndexOf(args2.TextValue, args3.IntValue, args4.IntValue) + work.ExcelIndex);
		}

	}


}
