using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_REPT : Function_2
	{
		public Function_REPT(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "Rept";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }
			var args2 = GetNumber_2(engine, tempParameter);
			if (args2.IsErrorOrNone) { return args2; }

			var newtext = args1.TextValue;
			var length = args2.IntValue;
			if (length < 0) {
				return ParameterError(2);
			}
			if (length == 0) {
				return Operand.Create(string.Empty);
			}
			if (newtext.Length == 0) {
				return args1;
			}
			if (length > 32767 / newtext.Length) {
				return ParameterError(2);
			}
			if (length == 1) {
				return args1;
			}
			var sb = new StringBuilder(newtext.Length * length);
			for(int i = 0; i < length; i++) {
				sb.Append(newtext);
			}
			return Operand.Create(sb.ToString());
		}
		public override OperandType GetResultType()
		{
			return OperandType.TEXT;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
			func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}
}