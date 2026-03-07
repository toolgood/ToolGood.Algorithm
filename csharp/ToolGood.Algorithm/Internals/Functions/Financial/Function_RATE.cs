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

			if (nper <= 0) {
				return ParameterError(1);
			}

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
				type = typeArg.IntValue;
				if (type != 0 && type != 1) {
					return ParameterError(5);
				}
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

		private decimal NewtonRaphson(decimal n, decimal p, decimal v, decimal f, decimal type, decimal rate)
		{
			for (int i = 0; i < 100; i++) {
				decimal factor = MathEx.Pow(1 + rate, n);
				decimal fn;
				
				if (type == 1) {
					fn = v * factor + p * (1 + rate) * (factor - 1) / rate + f;
				} else {
					fn = v * factor + p * (factor - 1) / rate + f;
				}

				decimal dfn;
				if (type == 1) {
					dfn = v * n * MathEx.Pow(1 + rate, n - 1) + 
						  p * (1 + rate) * (n * rate * MathEx.Pow(1 + rate, n - 1) - (factor - 1)) / (rate * rate) +
						  p * (factor - 1) / rate;
				} else {
					dfn = v * n * MathEx.Pow(1 + rate, n - 1) + 
						  p * (n * rate * MathEx.Pow(1 + rate, n - 1) - (factor - 1)) / (rate * rate);
				}

				if (Math.Abs(dfn) < 1e-12m) break;
				var newRate = rate - fn / dfn;

				if (Math.Abs(newRate - rate) < 1e-10m) {
					return newRate;
				}
				rate = newRate;
			}
			return rate;
		}
	}
}
