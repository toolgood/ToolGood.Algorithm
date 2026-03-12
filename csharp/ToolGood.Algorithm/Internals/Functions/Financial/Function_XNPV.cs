using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_XNPV : Function_3
	{
		public Function_XNPV(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "XNPV";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var rateArg = GetNumber_1(engine, tempParameter);
			if (rateArg.IsErrorOrNone) return rateArg;
			var rate = rateArg.NumberValue;

			var valuesArg = GetArray_2(engine, tempParameter);
			if (valuesArg.IsErrorOrNone) return valuesArg;
			var values = valuesArg.ArrayValue;

			var datesArg = GetArray_3(engine, tempParameter);
			if (datesArg.IsErrorOrNone) return datesArg;
			var dates = datesArg.ArrayValue;

			if (values.Count != dates.Count) return FunctionError();
			if (values.Count == 0) return ParameterError(1);

			var dateList = new List<DateTime>();
			foreach (var d in dates) {
				if (d.IsDate) {
					dateList.Add(d.DateValue.ToDateTime(DateTimeKind.Utc));
				} else if (d.IsText) {
					var myDate = MyDate.Parse(d.TextValue);
					if (myDate == null) return ParameterError(3);
					dateList.Add(myDate.ToDateTime(DateTimeKind.Utc));
				} else {
					return ParameterError(3);
				}
			}

			var baseDate = dateList[0];
			decimal xnpv = 0;

			for (int i = 0; i < values.Count; i++) {
				var days = (decimal)(dateList[i] - baseDate).TotalDays;
				xnpv += values[i].NumberValue / MathEx.Pow((1 + rate), days / 365.0m);
			}

			return Operand.Create(xnpv);
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			func2.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
			func3.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
		}
	}
}
