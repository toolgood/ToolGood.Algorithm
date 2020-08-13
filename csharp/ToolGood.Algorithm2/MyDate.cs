using System;
using System.Text;
using System.Text.RegularExpressions;

namespace ToolGood.Algorithm
{
    /// <summary>
    /// MyDate
    /// </summary>
    public class MyDate
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

        private MyDate() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">时</param>
        /// <param name="minute">分</param>
        /// <param name="second">秒</param>
        public MyDate(int year, int month, int day, int hour, int minute, int second)
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
        public MyDate(double num)
        {
            int days = (int)num;
            if (days > 365) {
                var start = DateTime.MinValue.AddDays((int)days);
                Year = start.Year;
                Month = start.Month;
                Day = start.Day;
            } else {
                Year = null;
                Month = null;
                Day = days;
            }
            double d = num - days;
            Hour = (int)(d * 24);
            Minute = (int)((d * 24 - Hour) * 60.0);
            Second = (int)(((d * 24 - Hour) * 60.0 - Minute) * 60.0);
        }
        /// <summary>
        /// 字符串转MyDate
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static MyDate Parse(String txt)
        {
            String t = txt.Trim();
            var m = Regex.Match(t, "^(\\d{4})-(11|12|0?\\d)-(30|31|[012]?\\d) ([01]\\d?|2[1234]):([012345]?\\d):([012345]?\\d)$", RegexOptions.Compiled);
            if (m.Success) {
                MyDate date = new MyDate();
                date.Year = int.Parse(m.Groups[(1)].Value);
                date.Month = int.Parse(m.Groups[(2)].Value);
                date.Day = int.Parse(m.Groups[(3)].Value);
                date.Hour = int.Parse(m.Groups[(4)].Value);
                date.Minute = int.Parse(m.Groups[(5)].Value);
                date.Second = int.Parse(m.Groups[(6)].Value);
                return date;
            }
            m = Regex.Match(t, "^(\\d{4})-(11|12|0?\\d)-(30|31|[012]?\\d) ([01]\\d?|2[1234]):([012345]?\\d)$", RegexOptions.Compiled);
            if (m.Success) {
                MyDate date = new MyDate();
                date.Year = int.Parse(m.Groups[(1)].Value);
                date.Month = int.Parse(m.Groups[(2)].Value);
                date.Day = int.Parse(m.Groups[(3)].Value);
                date.Hour = int.Parse(m.Groups[(4)].Value);
                date.Minute = int.Parse(m.Groups[(5)].Value);
                return date;
            }
            m = Regex.Match(t, "^(\\d{4})-(11|12|0?\\d)-(30|31|[012]?\\d)$");
            if (m.Success) {
                MyDate date = new MyDate();
                date.Year = int.Parse(m.Groups[(1)].Value);
                date.Month = int.Parse(m.Groups[(2)].Value);
                date.Day = int.Parse(m.Groups[(3)].Value);
                return date;
            }
            m = Regex.Match(t, "^(\\d+) (2[1234]|[01]?\\d):([012345]?\\d):([012345]?\\d)$", RegexOptions.Compiled);
            if (m.Success) {
                MyDate date = new MyDate();
                date.Day = int.Parse(m.Groups[(1)].Value);
                date.Hour = int.Parse(m.Groups[(2)].Value);
                date.Minute = int.Parse(m.Groups[(3)].Value);
                date.Second = int.Parse(m.Groups[(4)].Value);
                return date;
            }
            m = Regex.Match(t, "^(2[1234]|[01]?\\d):([012345]?\\d):([012345]?\\d)$", RegexOptions.Compiled);
            if (m.Success) {
                MyDate date = new MyDate();
                date.Hour = int.Parse(m.Groups[(1)].Value);
                date.Minute = int.Parse(m.Groups[(2)].Value);
                date.Second = int.Parse(m.Groups[(3)].Value);
                return date;
            }
            m = Regex.Match(t, "^(2[1234]|[01]?\\d):([012345]?\\d)$", RegexOptions.Compiled);
            if (m.Success) {
                MyDate date = new MyDate();
                date.Hour = int.Parse(m.Groups[(1)].Value);
                date.Minute = int.Parse(m.Groups[(2)].Value);
                return date;
            }
            return null;
        }



        public override string ToString()
        {
            StringBuilder stringBuffer = new StringBuilder();
            if (Year != null) {
                stringBuffer.Append(Year);
                stringBuffer.Append("-");
                stringBuffer.Append(Month.Value.ToString("D2"));
                stringBuffer.Append("-");
                stringBuffer.Append(Day.Value.ToString("D2"));

                if (Second > 0 || Hour > 0 || Minute > 0) {
                    stringBuffer.Append(" ");
                    stringBuffer.Append(Hour.ToString("D2"));
                    stringBuffer.Append(":");
                    stringBuffer.Append(Minute.ToString("D2"));
                    if (Second > 0) {
                        stringBuffer.Append(":");
                        stringBuffer.Append(Second.ToString("D2"));
                    }
                }
            } else {
                if (Day != null) {
                    stringBuffer.Append(Day);
                    stringBuffer.Append(" ");
                }
                stringBuffer.Append(Hour.ToString("D2"));
                stringBuffer.Append(":");
                stringBuffer.Append(Minute.ToString("D2"));
                if (Second > 0) {
                    stringBuffer.Append(":");
                    stringBuffer.Append(Second.ToString("D2"));
                }
            }
            return stringBuffer.ToString();
        }

        public string ToString(string f)
        {
            if (Year == null) {
                return ((DateTime)this).ToString(f);
            }
            return ((DateTime)this).ToString(f);
        }

        /// <summary>
        /// 转DateTime
        /// </summary>
        /// <returns></returns>
        public DateTime ToDateTime()
        {
            return new DateTime(Year ?? 0, Month ?? 0, Day ?? 0, Hour, Minute, Second);
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
            return new MyDate(ToDateTime().AddYears(year));
        }
        /// <summary>
        /// 添加月
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public MyDate AddMonths(int month)
        {
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
            if (Year != null && Year > 1900) {
                return new MyDate(ToDateTime().AddSeconds(second));
            }
            return new MyDate(ToTimeSpan().Add(new TimeSpan(0, 0, second)));
        }

        #region operator
        public static implicit operator MyDate(DateTime myDate)
        {
            return new MyDate(myDate);
        }
        public static implicit operator DateTime(MyDate myDate)
        {
            return myDate.ToDateTime();
        }
        public static implicit operator TimeSpan(MyDate myDate)
        {
            return myDate.ToTimeSpan();
        }

        public static implicit operator MyDate(double days)
        {
            return new MyDate(days);
        }

        public static implicit operator double(MyDate MyDate)
        {
            if (MyDate.Year != null && MyDate.Year>1900) {
                var dt = new DateTime((MyDate.Year ?? 0), (MyDate.Month ?? 0), (MyDate.Day ?? 0), MyDate.Hour, MyDate.Minute, MyDate.Second);
                double days = (double)(dt - DateTime.MinValue).TotalDays;
                days += (MyDate.Hour + (MyDate.Minute + MyDate.Second / 60.0) / 60) / 24;
                return days;
            }
            return (MyDate.Day ?? 0) + (MyDate.Hour + (MyDate.Minute + MyDate.Second / 60.0) / 60) / 24;
        }


        public static MyDate operator +(MyDate myDate, MyDate num)
        {
            if (myDate.Year != null && num.Year == null) {
                return new MyDate(myDate.ToDateTime().Add(num.ToTimeSpan()));
            } else if (myDate.Year != null && num.Year == null) {
                return new MyDate(num.ToDateTime().Add(myDate.ToTimeSpan()));
            }
            return (MyDate)((double)myDate + (double)num);
        }
        public static MyDate operator -(MyDate myDate, MyDate num)
        {
            if (myDate.Year != null && num.Year == null) {
                return new MyDate(myDate.ToDateTime().Add(num.ToTimeSpan().Negate()));
            } else if (myDate.Year != null && num.Year == null) {
                return new MyDate(num.ToDateTime().Add(myDate.ToTimeSpan().Negate()));
            }
            return (MyDate)((double)myDate - (double)num);
        }
        public static MyDate operator +(MyDate myDate, double num)
        {
            return (MyDate)((double)myDate + (double)num);
        }
        public static MyDate operator -(MyDate myDate, double num)
        {
            return (MyDate)((double)myDate - (double)num);
        }
        public static MyDate operator *(MyDate myDate, double num)
        {
#if NETSTANDARD2_1
            if (myDate.Year != null) {
                return new MyDate(myDate.ToTimeSpan().Multiply(num));
            }
#endif
            return (MyDate)((double)myDate * (double)num);
        }
        public static MyDate operator /(MyDate myDate, double num)
        {
#if NETSTANDARD2_1
            if (myDate.Year != null) {
                return new MyDate(myDate.ToTimeSpan().Divide(num));
            }
#endif
            return (MyDate)((double)myDate / (double)num);
        }
        #endregion
    }
}
