using System;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_YEARFRAC : Function_N
	{
		public Function_YEARFRAC(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "YEARFRAC";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if (funcs.Length < 2) return ParameterError(1);

			var startDateArg = GetDate(engine, tempParameter, 0);
			if (startDateArg.IsError) return startDateArg;
			var startDate = startDateArg.DateValue.ToDateTime(DateTimeKind.Utc);

			var endDateArg = GetDate(engine, tempParameter, 1);
			if (endDateArg.IsError) return endDateArg;
			var endDate = endDateArg.DateValue.ToDateTime(DateTimeKind.Utc);

			int basis = 0;
			if (funcs.Length > 2) {
				var basisArg = GetNumber(engine, tempParameter, 2);
				if (basisArg.IsError) return basisArg;
				basis = basisArg.IntValue;
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
				case 0: // US (NASD) 30/360
					return Calculate30_360(startDate, endDate);
				case 1: // Actual/actual
					return CalculateActualActual(startDate, endDate);
				case 2: // Actual/360
					return (decimal)(endDate - startDate).TotalDays / 360;
				case 3: // Actual/365
					return (decimal)(endDate - startDate).TotalDays / 365;
				case 4: // European 30/360
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
	}
}
