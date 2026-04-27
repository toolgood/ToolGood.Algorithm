using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Csharp
{
	internal sealed class Function_REGEX : Function_2
	{
		public Function_REGEX(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 2) {
				throw new ArgumentException($"Function '{Name}' requires exactly 2 parameters.");
			}
		}

		public override string Name => "Regex";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }

			var args2 = GetText_2(engine, tempParameter);
			if(args2.IsErrorOrNone) { return args2; }

			try {
				var b = Regex.Match(args1.TextValue, args2.TextValue, RegexOptions.None, TimeSpan.FromSeconds(1));
				if(b.Success == false) {
					return FunctionError();
				}
				return Operand.Create(b.Value);
			} catch (RegexMatchTimeoutException) {
				return Operand.Error("Function '{0}' regex match timeout!", Name);
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
		}
	}

}
