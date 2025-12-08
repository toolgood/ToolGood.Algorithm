using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions
{
    #region MyDate time

    internal class Function_DATEVALUE : Function_N
    {
        public Function_DATEVALUE(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args = new List<Operand>();
            foreach (var item in funcs) { var aa = item.Calculate(work); if (aa.IsError) { return aa; } args.Add(aa); }
            if (args[0].Type == OperandType.DATE) { return args[0]; }
            int type = 0;
            if (args.Count == 2) {
                var args2 = args[1].ToNumber("Function '{0}' parameter {1} is error!", "DateValue", 2);
                if (args2.IsError) { return args2; }
                type = args2.IntValue;
            }
            if (type == 0) {
                if (args[0].Type == OperandType.TEXT) {
                    if (DateTime.TryParse(args[0].TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime time)) {
                        return Operand.Create(time);
                    }
                }
                var args1 = args[0].ToNumber("Function '{0}' parameter {1} is error!", "DateValue", 1);
                if (args1.LongValue <= 2958465L) { // 9999-12-31 日时间在excel的数字为 2958465
                    return args1.ToMyDate();
                }
                if (args1.LongValue <= 253402232399L) { // 9999-12-31 12:59:59 日时间 转 时间截 为 253402232399L，
                    var time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(args1.LongValue);
                    if (work.UseLocalTime) { return Operand.Create(time.ToLocalTime()); }
                    return Operand.Create(time);
                }
                // 注：时间截 253402232399 ms 转时间 为 1978-01-12 05:30:32
                var time2 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(args1.LongValue);
                if (work.UseLocalTime) { return Operand.Create(time2.ToLocalTime()); }
                return Operand.Create(time2);
            } else if (type == 1) {
                var args1 = args[0].ToText("Function '{0}' parameter {1} is error!", "DateValue", 1);
                if (args1.IsError) { return args1; }
                if (DateTime.TryParse(args1.TextValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) {
                    return Operand.Create(dt);
                }
            } else if (type == 2) {
                return args[0].ToNumber("Function '{0}' parameter is error!", "DateValue").ToMyDate();
            } else if (type == 3) {
                var args1 = args[0].ToNumber("Function '{0}' parameter {1} is error!", "DateValue", 1);
                var time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(args1.LongValue);
                if (work.UseLocalTime) { return Operand.Create(time.ToLocalTime()); }
                return Operand.Create(time);
            } else if (type == 4) {
                var args1 = args[0].ToNumber("Function '{0}' parameter {1} is error!", "DateValue", 1);
                var time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(args1.LongValue);
                if (work.UseLocalTime) { return Operand.Create(time.ToLocalTime()); }
                return Operand.Create(time);
            }
            return Operand.Error("Function '{0}' parameter is error!", "DateValue");
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "DateValue");
        }
    }

    internal class Function_TIMESTAMP : Function_2
    {
        public Function_TIMESTAMP(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args0 = func1.Calculate(work); if (args0.IsError) { return args0; }

            int type = 0; // 毫秒
            if (func2 != null) {
                var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "TimeStamp", 2); if (args2.IsError) { return args2; } }
                type = args2.IntValue;
            }
            DateTime args1;
            if (work.UseLocalTime) {
                args1 = args0.ToMyDate("Function '{0}' parameter {1} is error!", "TimeStamp", 1).DateValue.ToDateTime(DateTimeKind.Local).ToUniversalTime();
            } else {
                args1 = args0.ToMyDate("Function '{0}' parameter {1} is error!", "TimeStamp", 1).DateValue.ToDateTime(DateTimeKind.Utc);
            }
            if (type == 0) {
                var ms = (args1 - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                return Operand.Create(ms);
            } else if (type == 1) {
                var s = (args1 - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
                return Operand.Create(s);
            }
            return Operand.Error("Function '{0}' parameter is error!", "TimeStamp");
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "TimeStamp");
        }
    }

    internal class Function_TIMEVALUE : Function_1
    {
        public Function_TIMEVALUE(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.TEXT) { args1 = args1.ToText("Function '{0}' parameter is error!", "TimeValue"); if (args1.IsError) { return args1; } }

            if (TimeSpan.TryParse(args1.TextValue, CultureInfo.InvariantCulture, out TimeSpan dt)) {
                return Operand.Create(dt);
            }
            return Operand.Error("Function '{0}' parameter is error!", "TimeValue");
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "TimeValue");
        }
    }

    internal class Function_DATE : Function_N
    {
        public Function_DATE(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = funcs[0].Calculate(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Date", 1); if (args1.IsError) { return args1; } }
            var args2 = funcs[1].Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Date", 2); if (args2.IsError) { return args2; } }
            var args3 = funcs[2].Calculate(work); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "Date", 3); if (args3.IsError) { return args3; } }

            MyDate d;
            if (funcs.Length == 3) {
                d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, 0, 0, 0);
            } else if (funcs.Length == 4) {
                var args4 = funcs[3].Calculate(work); if (args4.Type != OperandType.NUMBER) { args4 = args4.ToNumber("Function '{0}' parameter {1} is error!", "Date", 4); if (args4.IsError) { return args4; } }

                d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, args4.IntValue, 0, 0);
            } else if (funcs.Length == 5) {
                var args4 = funcs[3].Calculate(work); if (args4.Type != OperandType.NUMBER) { args4 = args4.ToNumber("Function '{0}' parameter {1} is error!", "Date", 4); if (args4.IsError) { return args4; } }
                var args5 = funcs[4].Calculate(work); if (args5.Type != OperandType.NUMBER) { args5 = args5.ToNumber("Function '{0}' parameter {1} is error!", "Date", 5); if (args5.IsError) { return args5; } }
                d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, args4.IntValue, args5.IntValue, 0);
            } else {
                var args4 = funcs[3].Calculate(work); if (args4.Type != OperandType.NUMBER) { args4 = args4.ToNumber("Function '{0}' parameter {1} is error!", "Date", 4); if (args4.IsError) { return args4; } }
                var args5 = funcs[4].Calculate(work); if (args5.Type != OperandType.NUMBER) { args5 = args5.ToNumber("Function '{0}' parameter {1} is error!", "Date", 5); if (args5.IsError) { return args5; } }
                var args6 = funcs[5].Calculate(work); if (args6.Type != OperandType.NUMBER) { args6 = args6.ToNumber("Function '{0}' parameter {1} is error!", "Date", 6); if (args6.IsError) { return args6; } }
                d = new MyDate(args1.IntValue, args2.IntValue, args3.IntValue, args4.IntValue, args5.IntValue, args6.IntValue);
            }
            return Operand.Create(d);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Date");
        }
    }

    internal class Function_DATEDIF : Function_3
    {
        public Function_DATEDIF(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "DateDif", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.DATE) { args2 = args2.ToMyDate("Function '{0}' parameter {1} is error!", "DateDif", 2); if (args2.IsError) { return args2; } }
            var args3 = func3.Calculate(work); if (args3.Type != OperandType.TEXT) { args3 = args3.ToText("Function '{0}' parameter {1} is error!", "DateDif", 3); if (args3.IsError) { return args3; } }
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
            return Operand.Error("Function '{0}' parameter {1} is error!", "DateDif", 3);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "DateDif");
        }

    }

    internal class Function_TIME : Function_3
    {
        public Function_TIME(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "Time", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Time", 2); if (args2.IsError) { return args2; } }

            MyDate d;
            if (func3 != null) {
                var args3 = func3.Calculate(work); if (args3.Type != OperandType.NUMBER) { args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "Time", 3); if (args3.IsError) { return args3; } }
                d = new MyDate(0, 0, 0, args1.IntValue, args2.IntValue, args3.IntValue);
            } else {
                d = new MyDate(0, 0, 0, args1.IntValue, args2.IntValue, 0);
            }
            return Operand.Create(d);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Time");
        }
    }

    internal class Function_NOW : FunctionBase
    {
        public override Operand Calculate(AlgorithmEngine work)
        {
            if (work.UseLocalTime) {
                return Operand.Create(DateTime.Now);
            } else {
                return Operand.Create(DateTime.UtcNow);
            }
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("Now()");
        }
    }

    internal class Function_TODAY : FunctionBase
    {
        public override Operand Calculate(AlgorithmEngine work)
        {
            DateTime now;
            if (work.UseLocalTime) {
                now = DateTime.Now;
            } else {
                now = DateTime.UtcNow;
            }
            return Operand.Create(new MyDate(now.Year, now.Month, now.Day, 0, 0, 0));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            stringBuilder.Append("Today()");
        }
    }

    internal class Function_SECOND : Function_1
    {
        public Function_SECOND(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work);
            if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function '{0}' parameter is error!", "Second"); if (args1.IsError) { return args1; } }
            return Operand.Create(args1.DateValue.Second);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Second");
        }
    }

    internal class Function_MINUTE : Function_1
    {
        public Function_MINUTE(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work);
            if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function '{0}' parameter is error!", "Minute"); if (args1.IsError) { return args1; } }
            return Operand.Create(args1.DateValue.Minute);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Minute");
        }
    }

    internal class Function_HOUR : Function_1
    {
        public Function_HOUR(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work);
            if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function '{0}' parameter is error!", "Hour"); if (args1.IsError) { return args1; } }
            return Operand.Create(args1.DateValue.Hour);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Hour");
        }
    }

    internal class Function_MONTH : Function_1
    {
        public Function_MONTH(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work);
            if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function '{0}' parameter is error!", "Month"); if (args1.IsError) { return args1; } }
            if (args1.DateValue.Month == null) {
                return Operand.Error("Function 'Month' is error!");
            }
            return Operand.Create((int)args1.DateValue.Month.Value);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Month");
        }
    }

    internal class Function_YEAR : Function_1
    {
        public Function_YEAR(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work);
            if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function '{0}' parameter is error!", "Year"); if (args1.IsError) { return args1; } }
            if (args1.DateValue.Year == null) {
                return Operand.Error("Function 'Year' is error!");
            }
            return Operand.Create(args1.DateValue.Year.Value);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Year");
        }
    }

    internal class Function_DAY : Function_1
    {
        public Function_DAY(FunctionBase func1) : base(func1)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work);
            if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function '{0}' parameter is error!", "Day"); if (args1.IsError) { return args1; } }
            if (args1.DateValue.Day == null) {
                return Operand.Error("Function 'Day' is error!");
            }
            return Operand.Create(args1.DateValue.Day.Value);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Day");
        }
    }

    internal class Function_WEEKDAY : Function_2
    {
        public Function_WEEKDAY(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "WeekDay", 1); if (args1.IsError) { return args1; } }

            var type = 1;
            if (func2 != null) {
                var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "WeekDay", 2); if (args2.IsError) { return args2; } }
                type = args2.IntValue;
            }

            var t = ((DateTime)args1.DateValue).DayOfWeek;
            if (type == 1) {
                return Operand.Create((double)(t + 1));
            } else if (type == 2) {
                if (t == 0) return Operand.Create(7d);
                return Operand.Create((double)t);
            }
            if (t == 0) {
                return Operand.Create(6d);
            }
            return Operand.Create((double)(t - 1));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "WeekDay");
        }
    }

    internal class Function_DAYSINMONTH : Function_2
    {
        public Function_DAYSINMONTH(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.NUMBER) { args1 = args1.ToNumber("Function '{0}' parameter {1} is error!", "DaysInMonth", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "DaysInMonth", 2); if (args2.IsError) { return args2; } }
            int year = args1.IntValue;
            int month = args2.IntValue;
            if (month < 1 || month > 12) { return Operand.Error("Function '{0}' parameter {1} is error!", "DaysInMonth", 2); }
            int days = DateTime.DaysInMonth(year, month);
            return Operand.Create((decimal)days);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "DaysInMonth");
        }
    }

    internal class Function_DAYS360 : Function_3
    {
        public Function_DAYS360(FunctionBase func1, FunctionBase func2, FunctionBase func3) : base(func1, func2, func3)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "Days360", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.DATE) { args2 = args2.ToMyDate("Function '{0}' parameter {1} is error!", "Days360", 2); if (args2.IsError) { return args2; } }

            var startMyDate = (DateTime)args1.DateValue;
            var endMyDate = (DateTime)args2.DateValue;

            var method = false;
            if (func3 != null) {
                var args3 = func3.Calculate(work); if (args3.Type != OperandType.BOOLEAN) { args3 = args3.ToBoolean("Function '{0}' parameter {1} is error!", "Days360", 3); if (args3.IsError) { return args3; } }
                if (args3.IsError) { return args3; }
                method = args3.BooleanValue;
            }
            var days = endMyDate.Year * 360 + (endMyDate.Month - 1) * 30
                        - startMyDate.Year * 360 - (startMyDate.Month - 1) * 30;
            if (method) {
                if (endMyDate.Day == 31) days += 30;
                if (startMyDate.Day == 31) days -= 30;
            } else {
                if (startMyDate.Month == 12) {
                    if (startMyDate.Day == new DateTime(startMyDate.Year + 1, 1, 1).AddDays(-1).Day) {
                        days -= 30;
                    } else {
                        days -= startMyDate.Day;
                    }
                } else {
                    if (startMyDate.Day == new DateTime(startMyDate.Year, startMyDate.Month + 1, 1).AddDays(-1).Day) {
                        days -= 30;
                    } else {
                        days -= startMyDate.Day;
                    }
                }
                if (endMyDate.Month == 12) {
                    if (endMyDate.Day == new DateTime(endMyDate.Year + 1, 1, 1).AddDays(-1).Day) {
                        if (startMyDate.Day < 30) {
                            days += 31;
                        } else {
                            days += 30;
                        }
                    } else {
                        days += endMyDate.Day;
                    }
                } else {
                    if (endMyDate.Day == new DateTime(endMyDate.Year, endMyDate.Month + 1, 1).AddDays(-1).Day) {
                        if (startMyDate.Day < 30) {
                            days += 31;
                        } else {
                            days += 30;
                        }
                    } else {
                        days += endMyDate.Day;
                    }
                }
            }
            return Operand.Create(days);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Days360");
        }
    }

    internal class Function_EDATE : Function_2
    {
        public Function_EDATE(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "EDate", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "EDate", 2); if (args2.IsError) { return args2; } }
            return Operand.Create((MyDate)(((DateTime)args1.DateValue).AddMonths(args2.IntValue)));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "EDate");
        }
    }

    internal class Function_EOMONTH : Function_2
    {
        public Function_EOMONTH(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "EoMonth", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "EoMonth", 2); if (args2.IsError) { return args2; } }
            var dt = ((DateTime)args1.DateValue).AddMonths(args2.IntValue + 1);
            dt = new DateTime(dt.Year, dt.Month, 1).AddDays(-1);
            return Operand.Create(dt);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "EoMonth");
        }
    }

    internal class Function_NETWORKDAYS : Function_N
    {
        public Function_NETWORKDAYS(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = funcs[0].Calculate(work); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "NetWorkdays", 1); if (args1.IsError) { return args1; } }
            var args2 = funcs[1].Calculate(work); if (args2.Type != OperandType.DATE) { args2 = args2.ToMyDate("Function '{0}' parameter {1} is error!", "NetWorkdays", 2); if (args2.IsError) { return args2; } }

            var startMyDate = (DateTime)args1.DateValue;
            var endMyDate = (DateTime)args2.DateValue;

            HashSet<DateTime> list = new HashSet<DateTime>();
            for (int i = 2; i < funcs.Length; i++) {
                var ar = funcs[i].Calculate(work).ToMyDate("Function '{0}' parameter {1} is error!", "NetWorkdays", i + 1);
                if (ar.IsError) { return ar; }
                list.Add(ar.DateValue);
            }
            var days = 0;
            while (startMyDate <= endMyDate) {
                if (startMyDate.DayOfWeek != DayOfWeek.Sunday && startMyDate.DayOfWeek != DayOfWeek.Saturday) {
                    if (list.Contains(startMyDate) == false) {
                        days++;
                    }
                }
                startMyDate = startMyDate.AddDays(1);
            }
            return Operand.Create(days);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "NetWorkdays");
        }
    }

    internal class Function_WORKDAY : Function_N
    {
        public Function_WORKDAY(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = funcs[0].Calculate(work); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "Workday", 1); if (args1.IsError) { return args1; } }
            var args2 = funcs[1].Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Workday", 2); if (args2.IsError) { return args2; } }

            var startMyDate = (DateTime)args1.DateValue;
            var days = args2.IntValue;
            HashSet<DateTime> list = new HashSet<DateTime>();
            for (int i = 2; i < funcs.Length; i++) {
                var ar = funcs[i].Calculate(work).ToMyDate("Function '{0}' parameter {1} is error!", "Workday", i + 1);
                if (ar.IsError) { return ar; }
                list.Add(ar.DateValue);
            }
            while (days > 0) {
                startMyDate = startMyDate.AddDays(1);
                if (startMyDate.DayOfWeek == DayOfWeek.Saturday) continue;
                if (startMyDate.DayOfWeek == DayOfWeek.Sunday) continue;
                if (list.Contains(startMyDate)) continue;
                days--;
            }
            return Operand.Create(startMyDate);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "Workday");
        }
    }

    internal class Function_WEEKNUM : Function_N
    {
        public Function_WEEKNUM(FunctionBase[] funcs) : base(funcs)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = funcs[0].Calculate(work); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "WeekNum", 1); if (args1.IsError) { return args1; } }
            var startMyDate = (DateTime)args1.DateValue;

            var days = startMyDate.DayOfYear + (int)(new DateTime(startMyDate.Year, 1, 1).DayOfWeek);
            if (funcs.Length == 2) {
                var args2 = funcs[1].Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "WeekNum", 2); if (args2.IsError) { return args2; } }
                if (args2.IntValue == 2) {
                    days--;
                }
            }

            var week = Math.Ceiling(days / 7.0);
            return Operand.Create(week);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "WeekNum");
        }
    }

    internal class Function_ADDMONTHS : Function_2
    {
        public Function_ADDMONTHS(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "AddMonths", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "AddMonths", 2); if (args2.IsError) { return args2; } }
            return Operand.Create((MyDate)(((DateTime)args1.DateValue).AddMonths(args2.IntValue)));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "AddMonths");
        }
    }

    internal class Function_ADDYEARS : Function_2
    {
        public Function_ADDYEARS(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "AddYears", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "AddYears", 2); if (args2.IsError) { return args2; } }
            return Operand.Create((MyDate)(((DateTime)args1.DateValue).AddYears(args2.IntValue)));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "AddYears");
        }
    }

    internal class Function_ADDSECONDS : Function_2
    {
        public Function_ADDSECONDS(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "AddSeconds", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "AddSeconds", 2); if (args2.IsError) { return args2; } }
            var date = args1.DateValue.AddSeconds(args2.IntValue);
            return Operand.Create(date);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "AddSeconds");
        }
    }

    internal class Function_ADDMINUTES : Function_2
    {
        public Function_ADDMINUTES(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "AddMinutes", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "AddMinutes", 2); if (args2.IsError) { return args2; } }
            var date = args1.DateValue.AddMinutes(args2.IntValue);
            return Operand.Create(date);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "AddMinutes");
        }
    }

    internal class Function_ADDHOURS : Function_2
    {
        public Function_ADDHOURS(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "AddHours", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "AddHours", 2); if (args2.IsError) { return args2; } }
            var date = args1.DateValue.AddHours(args2.IntValue);
            return Operand.Create(date);
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "AddHours");
        }
    }

    internal class Function_ADDDAYS : Function_2
    {
        public Function_ADDDAYS(FunctionBase func1, FunctionBase func2) : base(func1, func2)
        {
        }

        public override Operand Calculate(AlgorithmEngine work)
        {
            var args1 = func1.Calculate(work); if (args1.Type != OperandType.DATE) { args1 = args1.ToMyDate("Function '{0}' parameter {1} is error!", "AddDays", 1); if (args1.IsError) { return args1; } }
            var args2 = func2.Calculate(work); if (args2.Type != OperandType.NUMBER) { args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "AddDays", 2); if (args2.IsError) { return args2; } }
            return Operand.Create((MyDate)(((DateTime)args1.DateValue).AddDays((double)args2.NumberValue)));
        }
        public override void ToString(StringBuilder stringBuilder, bool addBrackets)
        {
            AddFunction(stringBuilder, "AddDays");
        }
    }

    #endregion MyDate time

}
