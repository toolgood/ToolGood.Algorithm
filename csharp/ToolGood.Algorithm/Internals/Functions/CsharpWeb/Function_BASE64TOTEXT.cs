using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.CsharpWeb
{
	internal sealed class Function_BASE64TOTEXT : Function_1
	{
		public Function_BASE64TOTEXT(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "Base64ToText";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }
			try {
				var t = Encoding.UTF8.GetString(Convert.FromBase64String(args1.TextValue));
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
