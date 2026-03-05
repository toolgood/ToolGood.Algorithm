using System;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_RATE : Function_N
	{
		public Function_RATE(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "RATE";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if (funcs.Length < 3) return ParameterError(1);

			var nperArg = GetNumber(engine, tempParameter, 0);
			if (nperArg.IsError) return nperArg;
			var nper = nperArg.DoubleValue;

			var pmtArg = GetNumber(engine, tempParameter, 1);
			if (pmtArg.IsError) return pmtArg;
			var pmt = pmtArg.DoubleValue;

			var pvArg = GetNumber(engine, tempParameter, 2);
			if (pvArg.IsError) return pvArg;
			var pv = pvArg.DoubleValue;

			double fv = 0;
			if (funcs.Length > 3) {
				var fvArg = GetNumber(engine, tempParameter, 3);
				if (fvArg.IsError) return fvArg;
				fv = fvArg.DoubleValue;
			}

			int type = 0;
			if (funcs.Length > 4) {
				var typeArg = GetNumber(engine, tempParameter, 4);
				if (typeArg.IsError) return typeArg;
				type = typeArg.IntValue;
			}

			double guess = 0.1;
			if (funcs.Length > 5) {
				var guessArg = GetNumber(engine, tempParameter, 5);
				if (guessArg.IsError) return guessArg;
				guess = guessArg.DoubleValue;
			}

			var rate = NewtonRaphson(nper, pmt, pv, fv, type, guess);
			return Operand.Create(rate);
		}

		private double NewtonRaphson(double n, double p, double v, double f, double type, double rate)
		{
			for (int i = 0; i < 100; i++) {
				double factor = Math.Pow(1 + rate, n);
				double fn;
				
				if (type == 1) {
					fn = v * factor + p * (1 + rate) * (factor - 1) / rate + f;
				} else {
					fn = v * factor + p * (factor - 1) / rate + f;
				}

				double dfn;
				if (type == 1) {
					dfn = v * n * Math.Pow(1 + rate, n - 1) + 
						  p * (1 + rate) * (n * rate * Math.Pow(1 + rate, n - 1) - (factor - 1)) / (rate * rate) +
						  p * (factor - 1) / rate;
				} else {
					dfn = v * n * Math.Pow(1 + rate, n - 1) + 
						  p * (n * rate * Math.Pow(1 + rate, n - 1) - (factor - 1)) / (rate * rate);
				}

				if (Math.Abs(dfn) < 1e-12) break;
				var newRate = rate - fn / dfn;

				if (Math.Abs(newRate - rate) < 1e-10) {
					return newRate;
				}
				rate = newRate;
			}
			return rate;
		}
	}
}
