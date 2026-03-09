using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_NPV : Function_N
	{
		public Function_NPV(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "NPV";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if (funcs.Length < 2) return ParameterError(1);

			var rateArg = GetNumber(engine, tempParameter, 0);
			if (rateArg.IsErrorOrNone) return rateArg;
			var rate = rateArg.NumberValue;

			var values = new List<decimal>();
			for (int i = 1; i < funcs.Length; i++) {
				var arg = GetNumber(engine, tempParameter, i);
				if (arg.IsErrorOrNone) return arg;
				values.Add(arg.NumberValue);
			}

			decimal npv = 0;
			for (int i = 0; i < values.Count; i++) {
				npv += values[i] / MathEx.Pow((1 + rate), i + 1);
			}

			return Operand.Create(npv);
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			for(int i = 0; i < funcs.Length; i++) {
				funcs[i].GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			}
		}
	}
}
