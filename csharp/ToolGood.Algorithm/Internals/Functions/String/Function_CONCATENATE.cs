using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_CONCATENATE : Function_N
	{
		public Function_CONCATENATE(FunctionBase[] funcs) : base(funcs)
		{
		}

		public override string Name => "Concatenate";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if (funcs.Length == 0) {
				return Operand.Create(string.Empty);
			}
			if (funcs.Length == 1) {
				var a = GetText(engine, tempParameter, 0);
				if (a.IsErrorOrNone) { return a; }
				return a; // 只有一个参数
			}
			var sb = new StringBuilder();
			for (int i = 0; i < funcs.Length; i++) {
				var a = GetText(engine, tempParameter, i);
				if (a.IsErrorOrNone) { return a; }
				sb.Append(a.TextValue);
			}
			return Operand.Create(sb.ToString());
		}
		public override OperandType GetResultType()
		{
			return OperandType.TEXT;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			for(int i = 0; i < funcs.Length; i++) {
				funcs[i].GetParameterTypes(noneEngine, result, OperandType.TEXT);
			}
		}
	}
}