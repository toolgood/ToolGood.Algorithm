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
			var nper = nperArg.NumberValue;

			var pmtArg = GetNumber(engine, tempParameter, 1);
			if (pmtArg.IsError) return pmtArg;
			var pmt = pmtArg.NumberValue;

			var pvArg = GetNumber(engine, tempParameter, 2);
			if (pvArg.IsError) return pvArg;
			var pv = pvArg.NumberValue;

			decimal fv = 0;
			if (funcs.Length > 3) {
				var fvArg = GetNumber(engine, tempParameter, 3);
				if (fvArg.IsError) return fvArg;
				fv = fvArg.NumberValue;
			}

			int type = 0;
			if (funcs.Length > 4) {
				var typeArg = GetNumber(engine, tempParameter, 4);
				if (typeArg.IsError) return typeArg;
				type = (int)typeArg.NumberValue;
			}

			decimal guess = 0.1m;
			if (funcs.Length > 5) {
				var guessArg = GetNumber(engine, tempParameter, 5);
				if (guessArg.IsError) return guessArg;
				guess = guessArg.NumberValue;
			}

			var rate = NewtonRaphson(nper, pmt, pv, fv, type, guess);
			return Operand.Create(rate);
		}

		private decimal NewtonRaphson(decimal nper, decimal pmt, decimal pv, decimal fv, int type, decimal guess)
		{
			var rate = guess;
			for (int i = 0; i < 100; i++) {
				var f = CalculateF(rate, nper, pmt, pv, fv, type);
				var df = CalculateDF(rate, nper, pmt, pv, fv, type);
				if (Math.Abs(df) < 1e-12m) break;
				var newRate = rate - f / df;
				if (Math.Abs(newRate - rate) < 1e-10m) {
					return newRate;
				}
				rate = newRate;
			}
			return rate;
		}

		private decimal CalculateF(decimal rate, decimal nper, decimal pmt, decimal pv, decimal fv, int type)
		{
			if (rate == 0) {
				return pv + pmt * nper + fv;
			}
			var factor = (decimal)Math.Pow((double)(1 + rate), (double)nper);
			var f = pv * factor + pmt * (factor - 1) / rate + fv;
			if (type == 1) {
				f = pv * factor + pmt * (1 + rate) * (factor - 1) / rate + fv;
			}
			return f;
		}

		private decimal CalculateDF(decimal rate, decimal nper, decimal pmt, decimal pv, decimal fv, int type)
		{
			if (rate == 0) return 0;
			var factor = (decimal)Math.Pow((double)(1 + rate), (double)nper);
			var n = nper;
			var df = pv * n * (decimal)Math.Pow((double)(1 + rate), (double)n - 1) +
					 pmt * (n * rate * (factor - 1) / rate - (factor - 1)) / (rate * rate);
			if (type == 1) {
				df = pv * n * (decimal)Math.Pow((double)(1 + rate), (double)n - 1) +
					 pmt * ((n * rate * (factor - 1) / rate - (factor - 1)) / (rate * rate) * (1 + rate) + (factor - 1) / rate);
			}
			return df;
		}
	}
}
