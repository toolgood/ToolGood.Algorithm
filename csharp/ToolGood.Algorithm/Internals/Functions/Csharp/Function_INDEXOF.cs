using System;
using System.Text;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal class Function_INDEXOF : Function_4
	{
		public Function_INDEXOF(FunctionBase func1, FunctionBase func2, FunctionBase func3, FunctionBase func4) : base(func1, func2, func3, func4)
		{
		}

		public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = func1.Evaluate(work, tempParameter);
			args1 = FunctionUtil.ConvertToText(args1, "IndexOf", 1);
			if(args1.IsError) { return args1; }

			var args2 = func2.Evaluate(work, tempParameter);
			args2 = FunctionUtil.ConvertToText(args2, "IndexOf", 2);
			if(args2.IsError) { return args2; }

			var text = args1.TextValue;
			if(func3 == null) {
				return Operand.Create(text.AsSpan().IndexOf(args2.TextValue) + work.ExcelIndex);
			}

			var args3 = func3.Evaluate(work, tempParameter);
			args3 = FunctionUtil.ConvertToNumber(args3, "IndexOf", 3);
			if(args3.IsError) { return args3; }

			if(func4 == null) {
				return Operand.Create(text.AsSpan(args3.IntValue).IndexOf(args2.TextValue) + args3.IntValue + work.ExcelIndex);
			}

			var args4 = func4.Evaluate(work, tempParameter);
			args4 = FunctionUtil.ConvertToNumber(args4, "IndexOf", 4);
			if(args4.IsError) { return args4; }

			return Operand.Create(text.IndexOf(args2.TextValue, args3.IntValue, args4.IntValue) + work.ExcelIndex);
		}
		public override void ToString(StringBuilder stringBuilder, bool addBrackets)
		{
			AddFunction(stringBuilder, "IndexOf");
		}
	}


}
