using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
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
			if (valuesArg.IsErrorOrNone) return valuesArg;
			var values = new List<decimal>();
			foreach (var v in valuesArg.ArrayValue) {
				values.Add(v.NumberValue);
			}

			var datesArg = GetArray(engine, tempParameter, 1);
			if (datesArg.IsErrorOrNone) return datesArg;
			var dates = new List<DateTime>();
			foreach (var d in datesArg.ArrayValue) {
				if (d.IsDate) {
					dates.Add(d.DateValue.ToDateTime(DateTimeKind.Utc));
				} else if (d.IsText) {
					var myDate = MyDate.Parse(d.TextValue);
					if (myDate == null) return ParameterError(2);
					dates.Add(myDate.ToDateTime(DateTimeKind.Utc));
				} else {
					return ParameterError(2);
				}
			}

			if (values.Count != dates.Count || values.Count < 2) return FunctionError();

			decimal guess = 0.1m;
			if (funcs.Length > 2) {
				var guessArg = GetNumber(engine, tempParameter, 2);
				if (guessArg.IsErrorOrNone) return guessArg;
				guess = guessArg.NumberValue;
			}

			var xirr = NewtonRaphsonXIRR(values, dates, guess);
			return Operand.Create(xirr);
		}

		private decimal NewtonRaphsonXIRR(List<decimal> values, List<DateTime> dates, decimal rate)
		{
			var baseDate = dates[0];

			for (int iter = 0; iter < 100; iter++) {
				decimal npv = 0;
				decimal dnpv = 0;

				for (int i = 0; i < values.Count; i++) {
					var days = (decimal)(dates[i] - baseDate).TotalDays;
					var exp = days / 365.0m;
					var factor = MathEx.Pow(1 + rate, exp);
					npv += values[i] / factor;
					dnpv -= values[i] * exp / (factor * (1 + rate));
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
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}
	}
}
