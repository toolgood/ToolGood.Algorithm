using System;

namespace ToolGood.Algorithm
{
    public class Date
    {
        public Date(int year, int month, int day, int hour, int minute, int second)
        {
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
        }
        public Date(DateTime dt)
        {
            Year = dt.Year;
            Month = dt.Month;
            Day = dt.Day;
            Hour = dt.Hour;
            Minute = dt.Minute;
            Second = dt.Second;
        }
        public Date(TimeSpan dt)
        {
            Day = dt.Days;
            Hour = dt.Hours;
            Minute = dt.Minutes;
            Second = dt.Seconds;
        }

        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }



        public override string ToString()
        {
            if (Year != null && Hour > 0 && Second > 0) {
                return ((DateTime)this).ToString("yyyy-MM-dd HH:mm:ss");
            } else if (Year > 0 && Hour > 0) {
                return ((DateTime)this).ToString("yyyy-MM-dd HH:mm");
            } else if (Year > 0) {
                return ((DateTime)this).ToString("yyyy-MM-dd");
            } else if (Hour > 0 && Second > 0) {
                return ((DateTime)this).ToString("HH:mm:ss");
            }
            return ((DateTime)this).ToString("HH:mm:ss");
        }

        public string ToString(string f)
        {
            return ((DateTime)this).ToString(f);
        }

        #region operator
        public static implicit operator Date(DateTime date)
        {
            return new Date(date);
        }
        public static implicit operator DateTime(Date date)
        {
            return new DateTime(date.Year ?? 0, date.Month ?? 0, date.Day ?? 0, date.Hour, date.Minute, date.Second);
        }
        public static implicit operator TimeSpan(Date date)
        {
            return new TimeSpan(date.Hour, date.Minute, date.Second);
        }

        public static implicit operator Date(double days)
        {
            var dt = DateTime.MinValue.AddDays((int)days);
            if (dt.Year > 1900) {
                dt = dt.AddSeconds((days - (int)days) * 24 * 60 * 60);
                return new Date(dt);
            }
            var day = (int)days;
            days = (days - day) * 24;
            var hour = (int)days;
            days = (days - hour) * 60;
            var minute = (int)days * 24;
            days = (days - minute) * 60;
            var second = (int)days;
            return new Date(new TimeSpan(day, hour, minute, second));
        }
        public static implicit operator double(Date date)
        {
            if (date.Year > 1900) {
                var dt = new DateTime((date.Year ?? 0), (date.Month ?? 0), (date.Day ?? 0), date.Hour, date.Minute, date.Second);
                double days = (double)(dt - DateTime.MinValue).TotalDays;
                days += (date.Hour + (date.Minute + date.Second / 60.0) / 60) / 24;
                return days;
            }
            return (date.Day ?? 0) + (date.Hour + (date.Minute + date.Second / 60.0) / 60) / 24;
        }



        public static Date operator +(Date date, Date num)
        {
            return (Date)((double)date + (double)num);
        }
        public static Date operator -(Date date, Date num)
        {
            return (Date)((double)date - (double)num);
            //var dt = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
            //var dt2 = new DateTime(num.Year, num.Month, num.Day, num.Hour, num.Minute, num.Second);

            //return date;
        }



        public static Date operator +(Date date, double num)
        {
            return (Date)((double)date + (double)num);
        }
        public static Date operator -(Date date, double num)
        {
            return (Date)((double)date - (double)num);
        }
        public static Date operator *(Date date, double num)
        {
            return (Date)((double)date * (double)num);
        }
        public static Date operator /(Date date, double num)
        {
            return (Date)((double)date / (double)num);
        }
        public static Date operator %(Date date, double num)
        {
            return (Date)((double)date % (double)num);
        }

        #endregion
    }
}
