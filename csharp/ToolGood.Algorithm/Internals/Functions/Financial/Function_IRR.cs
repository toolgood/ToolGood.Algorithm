using System;
using System.Collections.Generic;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_IRR : Function_N
	{
		public Function_IRR(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "IRR";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if (funcs.Length < 1) return ParameterError(1);

			var valuesArg = GetArray(engine, tempParameter, 0);
			if (valuesArg.IsError) return valuesArg;
			var values = new List<double>();
			foreach (var v in valuesArg.ArrayValue) {
				values.Add(v.DoubleValue);
			}

			double guess = 0.1;
			if (funcs.Length > 1) {
				var guessArg = GetNumber(engine, tempParameter, 1);
				if (guessArg.IsError) return guessArg;
				guess = guessArg.DoubleValue;
			}

			var irr = NewtonRaphsonIRR(values, guess);
			return Operand.Create(irr);
		}

		private double NewtonRaphsonIRR(List<double> values, double guess)
		{
			var rate = guess;
			for (int iter = 0; iter < 100; iter++) {
				double npv = 0;
				double dnpv = 0;

				for (int i = 0; i < values.Count; i++) {
					var factor = (double)Math.Pow((1 + rate), i);
					npv += values[i] / factor;
					dnpv -= i * values[i] / (factor * (1 + rate));
				}

				if (Math.Abs(dnpv) < 1e-12) break;
				var newRate = rate - npv / dnpv;

				if (Math.Abs(newRate - rate) < 1e-10) {
					return newRate;
				}
				rate = newRate;
			}
			return rate;
		}
	}
}
