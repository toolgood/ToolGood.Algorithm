using System;
using System.Text;
using ToolGood.Algorithm.Internals.Visitors;

namespace ToolGood.Algorithm.Internals.Functions.DateTimes
{
	internal class Function_DATEDIF : Function_3
    {
        public Function_DATEDIF(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override string Name => "DateDif";

        public override Operand Evaluate(AlgorithmEngine work, Func<AlgorithmEngine, string, Operand> tempParameter)
        {
            var args1 = GetDate_1(work, tempParameter);
			if (args1.IsError) { return args1; }

			var args2 = GetDate_2(work, tempParameter);
			if (args2.IsError) { return args2; }

			var args3 = GetText_3(work, tempParameter);
			if (args3.IsError) { return args3; }
            var startMyDate = (DateTime)args1.DateValue;
            var endMyDate = (DateTime)args2.DateValue;
            var t = args3.TextValue.ToLower();

            if (CharUtil.Equals(t, 'Y')) {

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
            } else if (CharUtil.Equals(t, 'M')) {

                #region m

                bool b = false;
                if (startMyDate.Day <= endMyDate.Day) b = true;
                if (b) {
                    return Operand.Create((endMyDate.Year * 12 + endMyDate.Month - startMyDate.Year * 12 - startMyDate.Month));
                } else {
                    return Operand.Create((endMyDate.Year * 12 + endMyDate.Month - startMyDate.Year * 12 - startMyDate.Month - 1));
                }

                #endregion m
            } else if (CharUtil.Equals(t, 'D')) {
                return Operand.Create((endMyDate - startMyDate).Days);
            } else if (CharUtil.Equals(t, "YD")) {

                #region yd

                var day = endMyDate.DayOfYear - startMyDate.DayOfYear;
                if (endMyDate.Year > startMyDate.Year && day < 0) {
                    var days = new DateTime(startMyDate.Year, 12, 31).DayOfYear;
                    day = days + day;
                }
                return Operand.Create((day));

                #endregion yd
            } else if (CharUtil.Equals(t, "MD")) {

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
            } else if (CharUtil.Equals(t, "YM")) {

                #region ym

                var mo = endMyDate.Month - startMyDate.Month;
                if (endMyDate.Day < startMyDate.Day) mo--;
                if (mo < 0) mo += 12;
                return Operand.Create((mo));

                #endregion ym
            }
            return ParameterError(3);
        }


    }

}
