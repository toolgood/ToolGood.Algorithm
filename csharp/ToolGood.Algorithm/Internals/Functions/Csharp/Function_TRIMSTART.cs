using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal sealed class Function_TRIMSTART : Function_2
	{
		public Function_TRIMSTART(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length < 1 || funcs.Length > 2) {
				throw new ArgumentException($"Function '{Name}' requires 1 to 2 parameters.");
			}
		}

		public override string Name => "TrimStart";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }

			if(func2 == null) {
				return Operand.Create(args1.TextValue.TrimStart());
			}

			var args2 = GetText_2(engine, tempParameter);
			if(args2.IsErrorOrNone) { return args2; }

			char[] trimChars = args2.TextValue.ToCharArray();
			return Operand.Create(args1.TextValue.TrimStart(trimChars));
		}
		public override OperandType GetResultType()
		{
			return OperandType.TEXT;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
			if(func2 != null) {
				func2.GetParameterTypes(noneEngine, result, OperandType.TEXT);
			}
		}
	}

}
