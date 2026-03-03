using System;
using System.Collections.Generic;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_XIRR : Function_N
	{
		public Function_XIRR(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "XIRR";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if (funcs.Length < 2) return ParameterError(1);

			var valuesArg = GetArray(engine, tempParameter, 0);
			if (valuesArg.IsError) return valuesArg;
			var values = new List<decimal>();
			foreach (var v in valuesArg.ArrayValue) {
				values.Add(v.NumberValue);
			}

			var datesArg = GetArray(engine, tempParameter, 1);
			if (datesArg.IsError) return datesArg;
			var dates = new List<DateTime>();
			foreach (var d in datesArg.ArrayValue) {
				dates.Add(d.DateValue.ToDateTime(DateTimeKind.Utc));
			}

			if (values.Count != dates.Count) return FunctionError();

			decimal guess = 0.1m;
			if (funcs.Length > 2) {
				var guessArg = GetNumber(engine, tempParameter, 2);
				if (guessArg.IsError) return guessArg;
				guess = guessArg.NumberValue;
			}

			var xirr = NewtonRaphsonXIRR(values, dates, guess);
			return Operand.Create(xirr);
		}

		private decimal NewtonRaphsonXIRR(List<decimal> values, List<DateTime> dates, decimal guess)
		{
			var rate = guess;
			var baseDate = dates[0];

			for (int iter = 0; iter < 100; iter++) {
				decimal npv = 0;
				decimal dnpv = 0;

				for (int i = 0; i < values.Count; i++) {
					var days = (dates[i] - baseDate).TotalDays;
					var factor = Math.Pow(1 + (double)rate, days / 365.0);
					npv += values[i] / (decimal)factor;
					dnpv -= values[i] * (decimal)(days / 365.0) / (decimal)(factor * (1 + (double)rate));
				}

				if (Math.Abs(dnpv) < 1e-12m) break;
				var newRate = rate - npv / dnpv;

				if (Math.Abs(newRate - rate) < 1e-10m) {
					return newRate;
				}
				rate = newRate;
			}
			return rate;
		}
	}
}
