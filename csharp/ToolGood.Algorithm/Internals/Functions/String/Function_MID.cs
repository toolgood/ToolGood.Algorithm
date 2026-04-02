using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_MID : Function_3
	{
		public Function_MID(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 3) {
				throw new ArgumentException($"Function '{Name}' requires exactly 3 parameters.");
			}
		}

		public override string Name => "Mid";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }
			var args2 = GetNumber_2(engine, tempParameter);
			if(args2.IsErrorOrNone) { return args2; }
			var args3 = GetNumber_3(engine, tempParameter);
			if(args3.IsErrorOrNone) { return args3; }

			var text = args1.TextValue;
			var startIndex = args2.IntValue - engine.ExcelIndex;
			var length = args3.IntValue;

			if(startIndex < 0) {
				return ParameterError(2);
			}
			if(length < 0) {
				return ParameterError(3);
			}
			if(startIndex == 0 && length >= text.Length) {
				return args1;
			}
			if(startIndex >= text.Length) {
				return Operand.Create(string.Empty);
			}
			if(startIndex + length > text.Length) {
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
			func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}

}
