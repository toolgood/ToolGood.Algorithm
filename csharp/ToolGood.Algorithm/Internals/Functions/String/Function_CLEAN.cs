using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_CLEAN : Function_1
	{
		public Function_CLEAN(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 1) {
				throw new ArgumentException($"Function '{Name}' requires exactly 1 parameter.");
			}
		}

		public override string Name => "Clean";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }
			var t = args1.TextValue;
			bool needClean = false;
			for (int i = 0; i < t.Length; i++) {
				var c = t[i];
				if (c == '\f' || c == '\n' || c == '\r' || c == '\t' || c == '\v') {
					needClean = true;
					break;
				}
			}
			if (!needClean) {
				return args1; // no change
			}
			var sb = new StringBuilder(t.Length);
			for (int i = 0; i < t.Length; i++) {
				var c = t[i];
				if (c != '\f' && c != '\n' && c != '\r' && c != '\t' && c != '\v') {
					sb.Append(c);
				}
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
		}
	}

}
