using System;
using System.Text;
using ToolGood.Algorithm.Enums;

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
			var startIndex = args2.IntValue - engine.ExcelIndex;

			if (startIndex < 0) {
				return ParameterError(2);
			}
			if (startIndex >= text.Length) {
				return Operand.Create(string.Empty);
			}

			if(func3 == null) {
				return Operand.Create(text.AsSpan(startIndex).ToString());
			}

			var args3 = GetNumber_3(engine, tempParameter);
			if(args3.IsError) { return args3; }

			var length = args3.IntValue;
			if (length < 0) {
				return ParameterError(3);
			}
			if (startIndex + length > text.Length) {
				length = text.Length - startIndex;
			}
			return Operand.Create(text.AsSpan(startIndex, length).ToString());
		}
		public override OperandType GetRestltType()
		{
			return OperandType.TEXT;
		}
	}


}
