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
                Day = days;
            }
            double d = num - days;
            Hour = (int)(d * 24);
            Minute = (int)((d * 24 - Hour) * 60.0);
            Second = (int)(((d * 24 - Hour) * 60.0 - Minute) * 60.0);
        }
        public static MyDate Parse(String txt)
        {
            String t = txt.Trim();
            var m = Regex.Match(t, "^(\\d{4})-(11|12|0?\\d)-(30|31|[012]?\\d) ([01]\\d?|2[1234]):([012345]?\\d):([012345]?\\d)$",RegexOptions.Compiled);
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


        /// <summary>
        /// 年
        /// </summary>
        public int? Year { get; set; }
        /// <summary>
        /// 月
        /// </summary>
        public int? Month { get; set; }
        /// <summary>
        /// 日
        /// </summary>
        public int? Day { get; set; }
        /// <summary>
        /// 时
        /// </summary>
        public int Hour { get; set; }
        /// <summary>
        /// 分
        /// </summary>
        public int Minute { get; set; }
        /// <summary>
        /// 秒
        /// </summary>
        public int Second { get; set; }



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

        #region operator
        public static implicit operator MyDate(DateTime MyDate)
        {
            return new MyDate(MyDate);
        }
        public static implicit operator DateTime(MyDate MyDate)
        {
            return new DateTime(MyDate.Year ?? 0, MyDate.Month ?? 0, MyDate.Day ?? 0, MyDate.Hour, MyDate.Minute, MyDate.Second);
        }
        public static implicit operator TimeSpan(MyDate MyDate)
        {
            return new TimeSpan(MyDate.Day ?? 0, MyDate.Hour, MyDate.Minute, MyDate.Second);
        }

        public static implicit operator MyDate(double days)
        {
            return new MyDate(days);
        }

        public static implicit operator double(MyDate MyDate)
        {
            if (MyDate.Year != null) {
                var dt = new DateTime((MyDate.Year ?? 0), (MyDate.Month ?? 0), (MyDate.Day ?? 0), MyDate.Hour, MyDate.Minute, MyDate.Second);
                double days = (double)(dt - DateTime.MinValue).TotalDays;
                days += (MyDate.Hour + (MyDate.Minute + MyDate.Second / 60.0) / 60) / 24;
                return days;
            }
            return (MyDate.Day ?? 0) + (MyDate.Hour + (MyDate.Minute + MyDate.Second / 60.0) / 60) / 24;
        }



        public static MyDate operator +(MyDate MyDate, MyDate num)
        {
            return (MyDate)((double)MyDate + (double)num);
        }
        public static MyDate operator -(MyDate MyDate, MyDate num)
        {
            return (MyDate)((double)MyDate - (double)num);
        }
        public static MyDate operator +(MyDate MyDate, double num)
        {
            return (MyDate)((double)MyDate + (double)num);
        }
        public static MyDate operator -(MyDate MyDate, double num)
        {
            return (MyDate)((double)MyDate - (double)num);
        }
        public static MyDate operator *(MyDate MyDate, double num)
        {
            return (MyDate)((double)MyDate * (double)num);
        }
        public static MyDate operator /(MyDate MyDate, double num)
        {
            return (MyDate)((double)MyDate / (double)num);
        }
        #endregion
    }
}
