using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal sealed class Function_SUBSTRING : Function_3
	{
		public Function_SUBSTRING(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length < 2 || funcs.Length > 3) {
				throw new ArgumentException($"Function '{Name}' requires 2 to 3 parameters.");
			}
		}

		public override string Name => "Substring";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }

			var args2 = GetNumber_2(engine, tempParameter);
			if(args2.IsErrorOrNone) { return args2; }

			var text = args1.TextValue;
			var startIndex = args2.IntValue - engine.ExcelIndex;

			if (startIndex < 0) {
				return ParameterError(2);
			}
			if (startIndex >= text.Length) {
				return Operand.Create(string.Empty);
			}

			if(func3 == null) {
				return Operand.Create(text.Substring(startIndex));
			}

			var args3 = GetNumber_3(engine, tempParameter);
			if(args3.IsErrorOrNone) { return args3; }

			var length = args3.IntValue;
			if (length < 0) {
				return ParameterError(3);
			}
			if (startIndex + length > text.Length) {
				length = text.Length - startIndex;
			}
			return Operand.Create(text.Substring(startIndex, length));
		}
		public override OperandType GetResultType()
		{
			return OperandType.TEXT;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
			func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func3 != null) {
				func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			}
		}
	}

}
