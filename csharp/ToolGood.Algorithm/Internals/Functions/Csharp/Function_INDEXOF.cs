using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal sealed class Function_INDEXOF : Function_4
	{
		public Function_INDEXOF(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length < 2) {
				throw new ArgumentException($"Function '{Name}' requires at least 2 parameters.");
			}
		}

		public override string Name => "IndexOf";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }

			var args2 = GetText_2(engine, tempParameter);
			if(args2.IsErrorOrNone) { return args2; }

			var text = args1.TextValue;
			if(func3 == null) {
				return Operand.Create(text.AsSpan().IndexOf(args2.TextValue) + engine.ExcelIndex);
			}

			var args3 = GetNumber_3(engine, tempParameter);
			if(args3.IsErrorOrNone) { return args3; }
			var startIndex = args3.IntValue - engine.ExcelIndex;
			if (startIndex < 0 || startIndex > text.Length) {
				return ParameterError(3);
			}

			if(func4 == null) {
				return Operand.Create(text.AsSpan(startIndex).IndexOf(args2.TextValue) + startIndex + engine.ExcelIndex);
			}

			var args4 = GetNumber_4(engine, tempParameter);
			if(args4.IsErrorOrNone) { return args4; }
			var count = args4.IntValue;
			if (count < 0 || startIndex + count > text.Length) {
				return ParameterError(4);
			}

			return Operand.Create(text.IndexOf(args2.TextValue, startIndex, count) + engine.ExcelIndex);
		}

		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
			func2.GetParameterTypes(noneEngine, result, OperandType.TEXT);
			if(func3 != null) {
				func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			}
			if(func4 != null) {
				func4.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			}
		}

	}

}
