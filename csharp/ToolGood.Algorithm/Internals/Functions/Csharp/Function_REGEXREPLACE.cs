using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal sealed class Function_REGEXREPLACE : Function_3
	{
		public Function_REGEXREPLACE(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "RegexReplace";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }

			var args2 = GetText_2(engine, tempParameter);
			if(args2.IsErrorOrNone) { return args2; }

			var args3 = GetText_3(engine, tempParameter);
			if(args3.IsErrorOrNone) { return args3; }

			try {
				var b = Regex.Replace(args1.TextValue, args2.TextValue, args3.TextValue);
				return Operand.Create(b);
			} catch (ArgumentException) {
				return ParameterError(2);
			}
		}
		public override OperandType GetResultType()
		{
			return OperandType.TEXT;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
			func2.GetParameterTypes(noneEngine, result, OperandType.TEXT);
			func3.GetParameterTypes(noneEngine, result, OperandType.TEXT);
		}
	}

}
