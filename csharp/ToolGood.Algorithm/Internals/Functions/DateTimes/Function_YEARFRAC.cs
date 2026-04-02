using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_YEARFRAC : Function_3
	{
		public Function_YEARFRAC(FunctionBase[] funcs) : base(funcs) {
			if (funcs.Length < 2) {
				throw new ArgumentException($"Function '{Name}' requires at least 2 parameters.");
			}
		}

		public override string Name => "YEARFRAC";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var startDateArg = GetDate_1(engine, tempParameter);
			if (startDateArg.IsErrorOrNone) return startDateArg;
			var startDate = startDateArg.DateValue.ToDateTime(DateTimeKind.Utc);

			var endDateArg = GetDate_2(engine, tempParameter);
			if (endDateArg.IsErrorOrNone) return endDateArg;
			var endDate = endDateArg.DateValue.ToDateTime(DateTimeKind.Utc);

			int basis = 0;
			if (func3 != null) {
				var basisArg = GetNumber_3(engine, tempParameter);
				if (basisArg.IsErrorOrNone) return basisArg;
				basis = basisArg.IntValue;
				if (basis < 0 || basis > 4) {
					return ParameterError(3);
				}
			}

			var result = CalculateYearFrac(startDate, endDate, basis);
			return Operand.Create(result);
		}

		private decimal CalculateYearFrac(DateTime startDate, DateTime endDate, int basis)
		{
			if (startDate > endDate) {
				var temp = startDate;
				startDate = endDate;
				endDate = temp;
			}

			switch (basis) {
				case 0:
					return Calculate30_360(startDate, endDate);
				case 1:
					return CalculateActualActual(startDate, endDate);
				case 2:
					return (decimal)(endDate - startDate).TotalDays / 360;
				case 3:
					return (decimal)(endDate - startDate).TotalDays / 365;
				case 4:
					return Calculate30_360E(startDate, endDate);
				default:
					return Calculate30_360(startDate, endDate);
			}
		}

		private decimal Calculate30_360(DateTime startDate, DateTime endDate)
		{
			int d1 = Math.Min(30, startDate.Day);
			int d2 = endDate.Day;

			if (d1 == 30) d2 = Math.Min(30, d2);

			return (360 * (endDate.Year - startDate.Year) + 30 * (endDate.Month - startDate.Month) + (d2 - d1)) / 360.0m;
		}

		private decimal Calculate30_360E(DateTime startDate, DateTime endDate)
		{
			int d1 = Math.Min(30, startDate.Day);
			int d2 = Math.Min(30, endDate.Day);

			return (360 * (endDate.Year - startDate.Year) + 30 * (endDate.Month - startDate.Month) + (d2 - d1)) / 360.0m;
		}

		private decimal CalculateActualActual(DateTime startDate, DateTime endDate)
		{
			int startYear = startDate.Year;
			int endYear = endDate.Year;

			if (startYear == endYear) {
				int daysInYear = DateTime.IsLeapYear(startYear) ? 366 : 365;
				return (decimal)(endDate - startDate).TotalDays / daysInYear;
			}

			decimal result = 0;
			int daysInStartYear = DateTime.IsLeapYear(startYear) ? 366 : 365;
			int daysInEndYear = DateTime.IsLeapYear(endYear) ? 366 : 365;

			result += (decimal)(new DateTime(startYear, 12, 31) - startDate).TotalDays / daysInStartYear;
			result += (decimal)(endDate - new DateTime(endYear, 1, 1)).TotalDays / daysInEndYear;
			result += endYear - startYear - 1;

			return result;
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.DATE);
			if(func2 != null) func2.GetParameterTypes(noneEngine, result, OperandType.DATE);
			if(func3 != null) func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}
}
