using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_RIGHT : Function_2
	{
		public Function_RIGHT(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "Right";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }

			if (args1.TextValue.Length == 0) {
				return Operand.Create(string.Empty);
			}
			if (func2 == null) {
				return Operand.Create(args1.TextValue.Substring(args1.TextValue.Length - 1, 1));
			}
			var args2 = GetNumber_2(engine, tempParameter);
			if (args2.IsErrorOrNone) { return args2; }
			if (args2.IntValue < 0) {
				return ParameterError(2);
			}
			int length = Math.Min(args2.IntValue, args1.TextValue.Length);
			int start = args1.TextValue.Length - length;
			return Operand.Create(args1.TextValue.Substring(start, length));
		}
		public override OperandType GetResultType()
		{
			return OperandType.TEXT;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
			if(func2 != null) {
				func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			}
		}
	}

}
