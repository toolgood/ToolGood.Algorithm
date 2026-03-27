using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_TRIM : Function_1
	{
		private static readonly Regex s_multipleSpaces = new Regex(@" +", RegexOptions.Compiled);

		public Function_TRIM(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "Trim";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }
			var text = args1.TextValue.Trim();
			text = s_multipleSpaces.Replace(text, " ");
			return Operand.Create(text);
		}
		public override OperandType GetResultType()
		{
			return OperandType.TEXT;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
		}
	}

}
