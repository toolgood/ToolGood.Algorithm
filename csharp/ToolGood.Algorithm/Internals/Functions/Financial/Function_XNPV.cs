using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_XNPV : Function_N
	{
		public Function_XNPV(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "XNPV";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if (funcs.Length < 3) return ParameterError(1);

			var rateArg = GetNumber(engine, tempParameter, 0);
			if (rateArg.IsError) return rateArg;
			var rate = rateArg.NumberValue;

			var valuesArg = GetArray(engine, tempParameter, 1);
			if (valuesArg.IsError) return valuesArg;
			var values = valuesArg.ArrayValue;

			var datesArg = GetArray(engine, tempParameter, 2);
			if (datesArg.IsError) return datesArg;
			var dates = datesArg.ArrayValue;

			if (values.Count != dates.Count) return FunctionError();
			if (values.Count == 0) return FunctionError();

			var dateList = new List<DateTime>();
			foreach (var d in dates) {
				if (d.IsDate) {
					dateList.Add(d.DateValue.ToDateTime(DateTimeKind.Utc));
				} else if (d.IsText) {
					var myDate = MyDate.Parse(d.TextValue);
					if (myDate == null) return FunctionError();
					dateList.Add(myDate.ToDateTime(DateTimeKind.Utc));
				} else {
					return FunctionError();
				}
			}

			var baseDate = dateList[0];
			decimal xnpv = 0;

			for (int i = 0; i < values.Count; i++) {
				var days = (dateList[i] - baseDate).TotalDays;
				xnpv += values[i].NumberValue / (decimal)Math.Pow((double)(1 + rate), days / 365.0);
			}

			return Operand.Create(xnpv);
		}
	}
}
