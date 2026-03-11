using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal sealed class Function_DATEDIF : Function_3
    {
		public Function_DATEDIF(FunctionBase[] funcs) : base(funcs)
		{
		}

        public override string Name => "DateDif";

        public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetDate_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }

			var args2 = GetDate_2(engine, tempParameter);
			if (args2.IsErrorOrNone) { return args2; }

			var args3 = GetText_3(engine, tempParameter);
			if (args3.IsErrorOrNone) { return args3; }
            var startMyDate = args1.DateValue.ToDateTime();
            var endMyDate = args2.DateValue.ToDateTime();
            var t = args3.TextValue;

            if (t.Equals("Y", StringComparison.OrdinalIgnoreCase)) {

                #region y

                bool b = false;
                if (startMyDate.Month < endMyDate.Month) {
                    b = true;
                } else if (startMyDate.Month == endMyDate.Month) {
                    if (startMyDate.Day <= endMyDate.Day) b = true;
                }
                if (b) {
                    return Operand.Create((endMyDate.Year - startMyDate.Year));
                } else {
                    return Operand.Create((endMyDate.Year - startMyDate.Year - 1));
                }

                #endregion y
            } else if (t.Equals("M", StringComparison.OrdinalIgnoreCase)) {

                #region m

                bool b = false;
                if (startMyDate.Day <= endMyDate.Day) b = true;
                if (b) {
                    return Operand.Create((endMyDate.Year * 12 + endMyDate.Month - startMyDate.Year * 12 - startMyDate.Month));
                } else {
                    return Operand.Create((endMyDate.Year * 12 + endMyDate.Month - startMyDate.Year * 12 - startMyDate.Month - 1));
                }

                #endregion m
            } else if (t.Equals("D", StringComparison.OrdinalIgnoreCase)) {
                return Operand.Create((endMyDate - startMyDate).Days);
            } else if (t.Equals("YD", StringComparison.OrdinalIgnoreCase)) {

                #region yd

                var day = endMyDate.DayOfYear - startMyDate.DayOfYear;
                if (endMyDate.Year > startMyDate.Year && day < 0) {
                    var days = new DateTime(startMyDate.Year, 12, 31).DayOfYear;
                    day = days + day;
                }
                return Operand.Create((day));

                #endregion yd
            } else if (t.Equals("MD", StringComparison.OrdinalIgnoreCase)) {

                #region md

                var mo = endMyDate.Day - startMyDate.Day;
                if (mo < 0) {
                    int days;
                    if (startMyDate.Month == 12) {
                        days = new DateTime(startMyDate.Year + 1, 1, 1).AddDays(-1).Day;
                    } else {
                        days = new DateTime(startMyDate.Year, startMyDate.Month + 1, 1).AddDays(-1).Day;
                    }
                    mo += days;
                }
                return Operand.Create((mo));

                #endregion md
            } else if (t.Equals("YM", StringComparison.OrdinalIgnoreCase)) {

                #region ym

                var mo = endMyDate.Month - startMyDate.Month;
                if (endMyDate.Day < startMyDate.Day) mo--;
                if (mo < 0) mo += 12;
                return Operand.Create((mo));

                #endregion ym
            }
            return ParameterError(3);
        }

		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.DATE);
			func2.GetParameterTypes(noneEngine, result, OperandType.DATE);
			func3.GetParameterTypes(noneEngine, result, OperandType.TEXT);
		}
	}

}
