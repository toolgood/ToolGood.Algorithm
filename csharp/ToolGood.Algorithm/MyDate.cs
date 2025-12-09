using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace ToolGood.Algorithm
{
    /// <summary>
    /// MyDate
    /// </summary>
    public sealed class MyDate
    {
        /// <summary>
        /// 年
        /// </summary>
        public int? Year { get; private set; }

        /// <summary>
        /// 月
        /// </summary>
        public int? Month { get; private set; }

        /// <summary>
        /// 日
        /// </summary>
        public int? Day { get; private set; }

        /// <summary>
        /// 时
        /// </summary>
        public int Hour { get; private set; }

        /// <summary>
        /// 分
        /// </summary>
        public int Minute { get; private set; }

        /// <summary>
        /// 秒
        /// </summary>
        public int Second { get; private set; }

        private MyDate()
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">时</param>
        /// <param name="minute">分</param>
        /// <param name="second">秒</param>
        public MyDate(int? year, int? month, int? day, int hour, int minute, int second)
        {
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dt">日期时间</param>
        public MyDate(DateTime dt)
        {
            Year = dt.Year;
            Month = dt.Month;
            Day = dt.Day;
            Hour = dt.Hour;
            Minute = dt.Minute;
            Second = dt.Second;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dt">时间</param>
        public MyDate(TimeSpan dt)
        {
            Year = null;
            Month = null;
            Day = dt.Days;
            Hour = dt.Hours;
            Minute = dt.Minutes;
            Second = dt.Seconds;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="num"></param>
        public MyDate(decimal num)
        {
            int days = (int)num;
            if (days > 365) {
                var start = new DateTime(1900, 1, 1).AddDays(((int)days) - 2);
                Year = start.Year;
                Month = start.Month;
                Day = start.Day;
            } else {
                Year = null;
                Month = null;
                Day = days;
            }
            decimal d = num - days;
            Hour = (int)(d * 24);
            Minute = (int)((d * 24 - Hour) * 60.0m);
            Second = (int)Math.Round(((d * 24 - Hour) * 60.0m - Minute) * 60.0m);
            if (Second == 60) {
                Second = 0;
                Minute++;
                if (Minute == 60) {
                    Minute = 0;
                    Hour++;
                }
            }
        }


        private static readonly Regex dateTimeRegex = new Regex("^(\\d{4})-(1[012]|0?\\d)-(30|31|[012]?\\d) ([01]?\\d|2[0123]):([012345]?\\d):([012345]?\\d)$", RegexOptions.Compiled);
        private static readonly Regex dateTimeRegex2 = new Regex("^(\\d{4})/(1[012]|0?\\d)/(30|31|[012]?\\d) ([01]?\\d|2[0123]):([012345]?\\d):([012345]?\\d)$", RegexOptions.Compiled);
        private static readonly Regex dateTimeRegex3 = new Regex("^(\\d{4})-(1[012]|0?\\d)-(30|31|[012]?\\d) ([01]?\\d|2[0123]):([012345]?\\d)$", RegexOptions.Compiled);
        private static readonly Regex dateTimeRegex4 = new Regex("^(\\d{4})/(1[012]|0?\\d)/(30|31|[012]?\\d) ([01]?\\d|2[0123]):([012345]?\\d)$", RegexOptions.Compiled);

        private static readonly Regex dateRegex = new Regex("^(\\d{4})-(1[012]|0?\\d)-(30|31|[012]?\\d)$", RegexOptions.Compiled);
        private static readonly Regex dateRegex2 = new Regex("^(\\d{4})-(1[012]|0?\\d)-(30|31|[012]?\\d)$", RegexOptions.Compiled);

        private static readonly Regex dayTimeRegex = new Regex("^(\\d+) (2[0123]|[01]?\\d):([012345]?\\d):([012345]?\\d)$", RegexOptions.Compiled);
        private static readonly Regex dayTimeRegex2 = new Regex("^(\\d+) (2[0123]|[01]?\\d):([012345]?\\d)$", RegexOptions.Compiled);

        private static readonly Regex timeRegex = new Regex("^(2[0123]|[01]?\\d):([012345]?\\d):([012345]?\\d)$", RegexOptions.Compiled);
        private static readonly Regex timeRegex2 = new Regex("^(2[0123]|[01]?\\d):([012345]?\\d)$", RegexOptions.Compiled);

        /// <summary>
        /// 字符串转MyDate
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static MyDate Parse(String txt)
        {
            CultureInfo cultureInfo = CultureInfo.InvariantCulture;// CultureInfo.GetCultureInfo("zh-cn");
            String t = txt.Trim();
            var m = dateTimeRegex.Match(t); // 年月日 时分秒
            if (m.Success == false) { m = dateTimeRegex2.Match(t); }
            if (m.Success) {
                MyDate date = new MyDate {
                    Year = int.Parse(m.Groups[(1)].Value, cultureInfo),
                    Month = int.Parse(m.Groups[(2)].Value, cultureInfo),
                    Day = int.Parse(m.Groups[(3)].Value, cultureInfo),
                    Hour = int.Parse(m.Groups[(4)].Value, cultureInfo),
                    Minute = int.Parse(m.Groups[(5)].Value, cultureInfo),
                    Second = int.Parse(m.Groups[(6)].Value, cultureInfo)
                };
                return date;
            }

            m = dateTimeRegex3.Match(t);// 年月日 时分
            if (m.Success == false) { m = dateTimeRegex4.Match(t); }
            if (m.Success) {
                MyDate date = new MyDate {
                    Year = int.Parse(m.Groups[(1)].Value, cultureInfo),
                    Month = int.Parse(m.Groups[(2)].Value, cultureInfo),
                    Day = int.Parse(m.Groups[(3)].Value, cultureInfo),
                    Hour = int.Parse(m.Groups[(4)].Value, cultureInfo),
                    Minute = int.Parse(m.Groups[(5)].Value, cultureInfo)
                };
                return date;
            }
            m = dateRegex.Match(t);// 年月日
            if (m.Success == false) { m = dateRegex2.Match(t); }
            if (m.Success) {
                MyDate date = new MyDate {
                    Year = int.Parse(m.Groups[(1)].Value, cultureInfo),
                    Month = int.Parse(m.Groups[(2)].Value, cultureInfo),
                    Day = int.Parse(m.Groups[(3)].Value, cultureInfo)
                };
                return date;
            }

            m = dayTimeRegex.Match(t);// 日 时分秒
            if (m.Success) {
                MyDate date = new MyDate {
                    Day = int.Parse(m.Groups[(1)].Value, cultureInfo),
                    Hour = int.Parse(m.Groups[(2)].Value, cultureInfo),
                    Minute = int.Parse(m.Groups[(3)].Value, cultureInfo),
                    Second = int.Parse(m.Groups[(4)].Value, cultureInfo)
                };
                return date;
            }
            m = dayTimeRegex2.Match(t);// 日 时分
            if (m.Success) {
                MyDate date = new MyDate {
                    Day = int.Parse(m.Groups[(1)].Value, cultureInfo),
                    Hour = int.Parse(m.Groups[(2)].Value, cultureInfo),
                    Minute = int.Parse(m.Groups[(3)].Value, cultureInfo),
                };
                return date;
            }

            m = timeRegex.Match(t);// 时分秒
            if (m.Success) {
                MyDate date = new MyDate {
                    Hour = int.Parse(m.Groups[(1)].Value, cultureInfo),
                    Minute = int.Parse(m.Groups[(2)].Value, cultureInfo),
                    Second = int.Parse(m.Groups[(3)].Value, cultureInfo)
                };
                return date;
            }
            m = timeRegex2.Match(t);// 时分
            if (m.Success) {
                MyDate date = new MyDate {
                    Hour = int.Parse(m.Groups[(1)].Value, cultureInfo),
                    Minute = int.Parse(m.Groups[(2)].Value, cultureInfo)
                };
                return date;
            }
            return null;
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder stringBuffer = new StringBuilder();
            if (Year != null && Year > 0) {
                stringBuffer.Append(Year);
                stringBuffer.Append('-');
                stringBuffer.Append(Month.Value.ToString("D2"));
                stringBuffer.Append('-');
                stringBuffer.Append(Day.Value.ToString("D2"));

                stringBuffer.Append(' ');
                stringBuffer.Append(Hour.ToString("D2"));
                stringBuffer.Append(':');
                stringBuffer.Append(Minute.ToString("D2"));
                stringBuffer.Append(':');
                stringBuffer.Append(Second.ToString("D2"));
            } else {
                if (Day != null && Day > 0) {
                    stringBuffer.Append(Day);
                    stringBuffer.Append(' ');
                }
                stringBuffer.Append(Hour.ToString("D2"));
                stringBuffer.Append(':');
                stringBuffer.Append(Minute.ToString("D2"));
                stringBuffer.Append(':');
                stringBuffer.Append(Second.ToString("D2"));
            }
            return stringBuffer.ToString();
        }

        internal string ToString(string f)
        {
            if (Year == null || Year == 0) {
                return this.ToString(f);
            }
            return ((DateTime)this).ToString(f);
        }

        /// <summary>
        /// 转DateTime
        /// </summary>
        /// <returns></returns>
        public DateTime ToDateTime()
        {
            return new DateTime(Year ?? 0, Month ?? 0, Day ?? 0, Hour, Minute, Second, DateTimeKind.Utc);
        }

        /// <summary>
        /// 转DateTime
        /// </summary>
        /// <returns></returns>
        public DateTime ToDateTime(DateTimeKind dateTimeKind)
        {
            return new DateTime(Year ?? 0, Month ?? 0, Day ?? 0, Hour, Minute, Second, dateTimeKind);
        }

        /// <summary>
        /// 转TimeSpan
        /// </summary>
        /// <returns></returns>
        public TimeSpan ToTimeSpan()
        {
            return new TimeSpan(Day ?? 0, Hour, Minute, Second);
        }

        /// <summary>
        /// 添加年
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public MyDate AddYears(int year)
        {
            var t = (this.Year ?? 0) + year;
            return new MyDate(t, Month, Day, Hour, Minute, Second);
            //return new MyDate(ToDateTime().AddYears(year));
        }

        /// <summary>
        /// 添加月
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public MyDate AddMonths(int month)
        {
            var t = (this.Month ?? 0) + month;
            if (t >= 1 && t <= 12) {
                return new MyDate(Year, t, Day, Hour, Minute, Second);
            }
            return new MyDate(ToDateTime().AddMonths(month));
        }

        /// <summary>
        /// 添加日
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public MyDate AddDays(int day)
        {
            if (Year != null && Year > 1900) {
                var d = (this.Day ?? 0) + day;
                if (d >= 1 && d <= 28) {
                    return new MyDate(Year, Month, d, Hour, Minute, Second);
                }
                return new MyDate(ToDateTime().AddDays(day));
            }
            return new MyDate(ToTimeSpan().Add(new TimeSpan(day, 0, 0, 0)));
        }

        /// <summary>
        /// 添加时
        /// </summary>
        /// <param name="hour"></param>
        /// <returns></returns>
        public MyDate AddHours(int hour)
        {
            var t = this.Hour + hour;
            if (t >= 0 && t < 24) {
                return new MyDate(Year, Month, Day, t, Minute, Second);
            }
            if (Year != null && Year > 1900) {
                return new MyDate(ToDateTime().AddHours(hour));
            }
            return new MyDate(ToTimeSpan().Add(new TimeSpan(hour, 0, 0)));
        }

        /// <summary>
        /// 添加分
        /// </summary>
        /// <param name="minute"></param>
        /// <returns></returns>
        public MyDate AddMinutes(int minute)
        {
            var t = this.Minute + minute;
            if (t >= 0 && t <= 59) {
                return new MyDate(Year, Month, Day, Hour, t, Second);
            }
            if (Year != null && Year > 1900) {
                return new MyDate(ToDateTime().AddMinutes(minute));
            }
            return new MyDate(ToTimeSpan().Add(new TimeSpan(0, minute, 0)));
        }

        /// <summary>
        /// 添加秒
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        public MyDate AddSeconds(int second)
        {
            var t = this.Second + second;
            if (t >= 0 && t <= 59) {
                return new MyDate(Year, Month, Day, Hour, Minute, t);
            }
            if (Year != null && Year > 1900) {
                return new MyDate(ToDateTime().AddSeconds(second));
            }
            return new MyDate(ToTimeSpan().Add(new TimeSpan(0, 0, second)));
        }

        #region operator

        /// <summary>
        /// DateTime=>MyDate
        /// </summary>
        /// <param name="myDate"></param>
        public static implicit operator MyDate(DateTime myDate)
        {
            return new MyDate(myDate);
        }

        /// <summary>
        /// MyDate=>DateTime
        /// </summary>
        /// <param name="myDate"></param>
        public static implicit operator DateTime(MyDate myDate)
        {
            return myDate.ToDateTime();
        }

        /// <summary>
        /// MyDate=>TimeSpan
        /// </summary>
        /// <param name="myDate"></param>
        public static implicit operator TimeSpan(MyDate myDate)
        {
            return myDate.ToTimeSpan();
        }

        /// <summary>
        /// decimal=>MyDate
        /// </summary>
        /// <param name="days"></param>
        public static implicit operator MyDate(decimal days)
        {
            return new MyDate(days);
        }

        /// <summary>
        /// MyDate=>decimal
        /// </summary>
        /// <param name="MyDate"></param>
        public static implicit operator decimal(MyDate MyDate)
        {
            if (MyDate.Year != null && MyDate.Year > 1900) {
                var dt = new DateTime((MyDate.Year ?? 0), (MyDate.Month ?? 0), (MyDate.Day ?? 0), MyDate.Hour, MyDate.Minute, MyDate.Second);
                //decimal days = (decimal)(dt -  DateTime.MinValue).TotalDays;
                decimal days = (decimal)(dt - new DateTime(1900, 1, 1)).TotalDays + 2;
                days += (MyDate.Hour + (MyDate.Minute + MyDate.Second / 60.0m) / 60) / 24;
                return days;
            }
            return (MyDate.Day ?? 0) + (MyDate.Hour + (MyDate.Minute + MyDate.Second / 60.0m) / 60) / 24;
        }

        /// <summary>
        /// 加
        /// </summary>
        /// <param name="myDate"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static MyDate operator +(MyDate myDate, MyDate num)
        {
            if (myDate.Year != null && num.Year == null) {
                return new MyDate(myDate.ToDateTime().Add(num.ToTimeSpan()));
            } else if (myDate.Year != null && num.Year == null) {
                return new MyDate(num.ToDateTime().Add(myDate.ToTimeSpan()));
            }
            return (MyDate)((decimal)myDate + (decimal)num);
        }

        /// <summary>
        /// 减
        /// </summary>
        /// <param name="myDate"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static MyDate operator -(MyDate myDate, MyDate num)
        {
            if (myDate.Year != null && num.Year == null) {
                return new MyDate(myDate.ToDateTime().Add(num.ToTimeSpan().Negate()));
            } else if (myDate.Year != null && num.Year == null) {
                return new MyDate(num.ToDateTime().Add(myDate.ToTimeSpan().Negate()));
            }
            return (MyDate)((decimal)myDate - (decimal)num);
        }

        /// <summary>加
        ///
        /// </summary>
        /// <param name="myDate"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static MyDate operator +(MyDate myDate, decimal num)
        {
            return (MyDate)((decimal)myDate + (decimal)num);
        }

        /// <summary>
        /// 减
        /// </summary>
        /// <param name="myDate"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static MyDate operator -(MyDate myDate, decimal num)
        {
            return (MyDate)((decimal)myDate - (decimal)num);
        }

        /// <summary>
        /// 乘
        /// </summary>
        /// <param name="myDate"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static MyDate operator *(MyDate myDate, decimal num)
        {
#if NETSTANDARD2_1
            if (myDate.Year != null) {
                return new MyDate(myDate.ToTimeSpan().Multiply((double)num));
            }
#endif
            return (MyDate)((decimal)myDate * (decimal)num);
        }

        /// <summary>
        /// 除
        /// </summary>
        /// <param name="myDate"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static MyDate operator /(MyDate myDate, decimal num)
        {
#if NETSTANDARD2_1
            if (myDate.Year != null) {
                return new MyDate(myDate.ToTimeSpan().Divide((double)num));
            }
#endif
            return (MyDate)((decimal)myDate / (decimal)num);
        }

        #endregion operator
    }
}