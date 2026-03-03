using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Operands;

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
				if (d.IsDate) {
					dates.Add(d.DateValue.ToDateTime(DateTimeKind.Utc));
				} else if (d.IsText) {
					var myDate = MyDate.Parse(d.TextValue);
					if (myDate == null) return FunctionError();
					dates.Add(myDate.ToDateTime(DateTimeKind.Utc));
				} else {
					return FunctionError();
				}
			}

			if (values.Count != dates.Count || values.Count < 2) return FunctionError();

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
			var rate = (double)guess;
			var baseDate = dates[0];

			for (int iter = 0; iter < 100; iter++) {
				double npv = 0;
				double dnpv = 0;

				for (int i = 0; i < values.Count; i++) {
					var days = (dates[i] - baseDate).TotalDays;
					var exp = days / 365.0;
					var factor = Math.Pow(1 + rate, exp);
					npv += (double)values[i] / factor;
					dnpv -= (double)values[i] * exp / (factor * (1 + rate));
				}

				if (Math.Abs(dnpv) < 1e-12) break;
				var newRate = rate - npv / dnpv;

				if (Math.Abs(newRate - rate) < 1e-10) {
					return (decimal)newRate;
				}
				rate = newRate;
			}
			return (decimal)rate;
		}
	}
}
