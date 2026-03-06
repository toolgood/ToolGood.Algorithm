using System;
using System.Collections.Generic;

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
			if (rateArg.IsError) return rateArg;
			var rate = rateArg.NumberValue;

			var values = new List<decimal>();
			for (int i = 1; i < funcs.Length; i++) {
				var arg = GetNumber(engine, tempParameter, i);
				if (arg.IsError) return arg;
				values.Add(arg.NumberValue);
			}

			decimal npv = 0;
			for (int i = 0; i < values.Count; i++) {
				npv += values[i] / MathEx.Pow((1 + rate), i + 1);
			}

			return Operand.Create(npv);
		}
	}
}
