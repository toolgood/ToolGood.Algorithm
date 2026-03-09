using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.CsharpWeb
{
	internal sealed class Function_TEXTTOBASE64URL : Function_1
	{
		public Function_TEXTTOBASE64URL(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "TextToBase64Url";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }
			try {
				var bytes = Encoding.UTF8.GetBytes(args1.TextValue);
				var t = Convert.ToBase64String(bytes).Replace('+', '-').Replace('/', '_').TrimEnd('=');
				return Operand.Create(t);
			} catch(Exception) {
				return ParameterError(1);
			}
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
